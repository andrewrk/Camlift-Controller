Imports System.Threading
Imports VisionaryDigital.Settings

Namespace SmartSteps
    Public Class AsyncStepper

        Public Const AbortedProgress = -2
        Public Const NotStartedProgress = -1

        Private m_allSettings As AllSettings
        Private ReadOnly m_camera As Camera
        Private ReadOnly m_f_goToLocation As Action(Of Integer)
        Private ReadOnly m_f_isMoveFinished As Func(Of Boolean)
        Private ReadOnly m_locations As IEnumerable(Of Integer)
        Private ReadOnly m_dwell As Integer
        Private ReadOnly m_returnToTop As Boolean
        Private m_runnerThread As Thread
        Private m_break As Boolean = False

        Private m_Progress As Integer = NotStartedProgress
        Public ReadOnly Property Progress() As Integer
            Get
                Return m_Progress
            End Get
        End Property

        Public Event ProgressReported As EventHandler
        Public Event Finished As EventHandler
        Public Event Aborted As EventHandler(Of AbortedEventArgs)

        Public Sub New(ByVal _camera As Camera, ByVal f_goToLocation As Action(Of Integer), ByVal f_isMoveFinished As Func(Of Boolean), _
                       ByVal locations As IEnumerable(Of Integer), ByVal dwell As Integer, ByVal returnToTop As Boolean, ByVal settings As AllSettings)
            m_camera = _camera
            m_f_goToLocation = f_goToLocation
            m_f_isMoveFinished = f_isMoveFinished
            m_locations = locations
            m_dwell = dwell
            m_returnToTop = returnToTop
            m_allSettings = settings
        End Sub

        Public Sub Start()
            If m_runnerThread IsNot Nothing Then Throw New InvalidOperationException
            m_runnerThread = New Thread(AddressOf execute_run)
            m_runnerThread.Start()
        End Sub

        Public Sub Abort()
            m_camera.EndFastPictures()
            If m_runnerThread Is Nothing Then Throw New InvalidOperationException
            m_break = True
        End Sub

        Private Sub AbortRun(ByVal abortException As Exception)
            m_Progress = AbortedProgress
            RaiseEvent Aborted(Me, New AbortedEventArgs(abortException))
        End Sub

        Private Sub execute_run()
            Dim total = m_locations.Count()
            m_Progress = 0
            RaiseEvent ProgressReported(Me, New EventArgs)

            'go to first location (indefinite wait)
            m_f_goToLocation.Invoke(m_locations.First)
            Do
                Thread.Sleep(100)
            Loop Until m_f_isMoveFinished.Invoke
            'Check for Abort before beginning the loop
            If m_break Then
                AbortRun(Nothing)
                Exit Sub
            End If

            'reached first location

            Dim soFar = 0
            For Each location In m_locations
                'Move
                m_f_goToLocation.Invoke(location)

                'Report Progress
                m_Progress = 100 * soFar / total
                RaiseEvent ProgressReported(Me, New EventArgs)

                'Check for Abort before Take Picture
                If m_break Then
                    AbortRun(Nothing)
                    Exit Sub
                End If

                ' sleep for dwell amount
                Thread.Sleep(m_dwell)


                'Take Picture
                Try
                    m_camera.TakeSinglePicture(m_allSettings.SettingsIndex.SavePicturesFolder)
                Catch ex As SdkException
                    AbortRun(ex)
                    Exit Sub
                Catch ex As Exception
                    AbortRun(Nothing)
                    Exit Sub
                End Try

                ' check for abort
                Application.DoEvents()

                'Check for Abort before next move
                If m_break Then
                    AbortRun(Nothing)
                    Exit Sub
                End If

                'Next
                soFar += 1
            Next
            If m_returnToTop Then
                m_f_goToLocation.Invoke(m_locations.First)
            End If

            m_Progress = 100 ' complete
            RaiseEvent Finished(Me, New EventArgs)
            Exit Sub
        End Sub

    End Class

    Public Class AbortedEventArgs
        Inherits EventArgs
        Dim m_AbortException As SdkException
        Public ReadOnly Property AbortException() As SdkException
            Get
                Return m_AbortException
            End Get
        End Property
        Public Sub New(ByVal abortException As SdkException)
            m_AbortException = abortException
        End Sub
    End Class

    Public Class SmartStepsManager

        Public Const DefaultDwell = 500
        Public Const DefaultPostDwell = 100

        Private m_f_moveTo As Action(Of Integer)
        Private m_camera As Camera
        Private m_f_isMoveFinished As Func(Of Boolean)

        Private m_settings As SmartStepsSettings
        Private m_allSettings As AllSettings

        Private m_LastAutorunSetup As AutorunSetupSettings
        Public Property LastAutorunSetup() As AutorunSetupSettings
            Get
                Return m_LastAutorunSetup
            End Get
            Set(ByVal value As AutorunSetupSettings)
                m_LastAutorunSetup = value
            End Set
        End Property

        Private m_AutorunSetups As AutorunSetupListSettings
        Public Property AutorunSetups() As AutorunSetupListSettings
            Get
                Return m_AutorunSetups
            End Get
            Set(ByVal value As AutorunSetupListSettings)
                m_AutorunSetups = value
            End Set
        End Property

        Private m_ReturnToTop As Boolean
        Public Property ReturnToTop() As Boolean
            Get
                Return m_ReturnToTop
            End Get
            Set(ByVal value As Boolean)
                m_ReturnToTop = value
            End Set
        End Property

        Private m_LastAutorunRun As New AutorunRunSettings(Nothing) 'fresh
        Public Property LastAutorunRun() As AutorunRunSettings
            Get
                Return m_LastAutorunRun
            End Get
            Set(ByVal value As AutorunRunSettings)
                m_LastAutorunRun = value
            End Set
        End Property

        Public Sub New(ByVal settings As SmartStepsSettings, ByVal f_moveTo As Action(Of Integer), _
                       ByVal _camera As Camera, ByVal f_isMoveFinished As Func(Of Boolean), ByVal allSettings As AllSettings)
            m_f_moveTo = f_moveTo
            m_camera = _camera
            m_f_isMoveFinished = f_isMoveFinished

            m_settings = settings

            m_LastAutorunSetup = settings.LastAutorunSetup
            m_AutorunSetups = settings.AutorunSetups
            m_ReturnToTop = settings.ReturnToTop

            m_allSettings = allSettings
        End Sub

        Public Sub SaveSettings()
            m_settings.LastAutorunSetup = m_LastAutorunSetup
            m_settings.AutorunSetups = m_AutorunSetups
            'last autorun run is not saved
            m_settings.ReturnToTop = m_ReturnToTop
        End Sub

        Public Function GetAutorunStepper(ByVal locations As IEnumerable(Of Integer), ByVal dwell As Integer)
            Return New AsyncStepper(m_camera, m_f_moveTo, m_f_isMoveFinished, locations, dwell, m_ReturnToTop, m_allSettings)
        End Function

    End Class

End Namespace

