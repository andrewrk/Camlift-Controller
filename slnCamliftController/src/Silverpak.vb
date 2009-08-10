Imports VisionaryDigital.Silverpak23CE
Imports VisionaryDigital.Settings
Imports System.Threading

#Const USE_FAKE_SILVERPAK = False

Public Class Silverpak
#If USE_FAKE_SILVERPAK Then
    Inherits DebugSilverpakManager
#Else
    Inherits SilverpakManager
#End If
    Implements IDisposable


    Public Sub New(ByVal settings As SilverpakSettings)
        MyBase.New()
        Me.BaudRate = 9600
        Me.DriverAddress = DriverAddresses.Driver1
        Me.Velocity = settings.Velocity
        Me.Acceleration = settings.Acceleration
        Me.RunningCurrent = settings.RunningCurrent

    End Sub

    Public Shadows Sub Dispose()
        If Me.IsActive Then Me.StopMotor()
        MyBase.Dispose()
    End Sub

    Public Sub EstablishConnection()
        Me.PortName = SilverpakManager.DefaultPortname
        If Me.FindAndConnect() Then
            InitializeMotor()
            If MsgBox(MsgBoxInitializeCoordinatesMessage, MsgBoxStyle.OkCancel, MsgBoxTitle) = MsgBoxResult.Ok Then
                TryInitializeCoordinates()
            Else
                End 'crash program
            End If
        Else
            Throw New SilverpakNotFoundException
        End If
    End Sub

    Private Sub InitializeMotor()
        Try
            Me.InitializeMotorSettings()
            Me.InitializeSmoothMotion()
        Catch ex As InvalidSilverpakOperationException
            Reconnect()
            Exit Sub
        End Try
    End Sub

    Private Sub Reconnect()
        If Me.IsActive Then Me.Disconnect()
        EstablishConnection()
    End Sub

    Private Sub TryInitializeCoordinates()
        Try
            Me.InitializeCoordinates()
        Catch ex As InvalidSilverpakOperationException
            Reconnect()
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        Try
            Dispose()
        Catch ex As Exception
        End Try
        MyBase.Finalize()
    End Sub
End Class


#Region "qwerty up"
#If USE_FAKE_SILVERPAK Then


Public Class DebugSilverpakManager
    Inherits SilverpakManager

    Private m_breakMotion As Boolean = False
    Private m_moving As Boolean = False

    Public Const DefaultSpeed As Integer = 1234

    Private m_Speed As Integer = DefaultSpeed
    Public Property Speed() As Integer
        Get
            Return m_Speed
        End Get
        Set(ByVal value As Integer)
            m_Speed = value
        End Set
    End Property

    Public Overrides Function Connect() As Boolean
        'do nothing
        Return True
    End Function
    Public Overrides Sub Disconnect()
        'do nothing
    End Sub
    Public Overrides Function FindAndConnect() As Boolean
        'do nothing
        Return True
    End Function
    Private m_actualPosition As Integer = 0
    Private m_destinationPosition As Integer = 0
    Public Overrides ReadOnly Property Position() As Integer
        Get
            Return m_actualPosition
        End Get
    End Property
    Public Overrides ReadOnly Property IsReady() As Boolean
        Get
            Return Not m_moving
        End Get
    End Property
    Public Overrides Sub GoToPosition(ByVal position As Integer)
        m_destinationPosition = position
        Static t As Thread
        t = New Thread(AddressOf animatePosition_run)
        t.Start()
    End Sub
    Private Sub animatePosition_run()
        Dim direction As Integer = Math.Sign(m_destinationPosition - m_actualPosition)
        If direction = 0 Then Exit Sub
        Dim velocity As Integer = direction * m_Speed

        m_breakMotion = False
        m_moving = True
        Dim almostDestination As Integer = m_destinationPosition - velocity
        While Not m_breakMotion AndAlso direction * m_actualPosition < direction * almostDestination
            m_actualPosition += velocity
            OnPositionChanged()
            Thread.Sleep(50)
        End While
        If Not m_breakMotion Then ' snap to destination
            m_actualPosition = m_destinationPosition
            OnPositionChanged()
            Thread.Sleep(50)
        End If
        m_moving = False
        OnStoppedMoving()
    End Sub
    Public Overrides Sub InitializeCoordinates()
        'raise completed event after 1 second
        Static t As Thread ' retain reference to prevent garbage collection
        t = New Thread(AddressOf waitAndRaise)
        t.Start()
    End Sub
    Private Sub waitAndRaise(ByVal o As Object)
        Thread.Sleep(1000) ' wait 1 second
        OnPositionChanged()
        OnCoordinatesInitialized()
    End Sub
    Public Overrides Sub InitializeMotorSettings()
        'do nothing
    End Sub
    Public Overrides Sub InitializeSmoothMotion()
        'do nothing
    End Sub
    Public Overrides Sub ResendMotorSettings()
        'do nothing
    End Sub
    Public Overrides Sub StopMotor()
        m_breakMotion = True
        'do nothing
    End Sub
End Class

#End If
#End Region

Public Class SilverpakNotFoundException
    Inherits Exception
End Class