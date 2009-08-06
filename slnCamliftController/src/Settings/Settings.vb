Imports System.Drawing
Imports VisionaryDigital
Imports VisionaryDigital.SmartSteps
Imports VisionaryDigital.Silverpak23CE
Imports System.Reflection

Namespace Settings

    Public Class AllSettings

        Public Shared ReadOnly GlobalSettingsDir As String = Application.StartupPath & "\settings"

#If DEBUG Then
        Public Shared ReadOnly UserSettingsDir As String = GlobalSettingsDir
#Else
        Public Shared ReadOnly UserSettingsDir As String = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\settings"
#End If

        Private Shared ReadOnly settingsIndexFile As String = GlobalSettingsDir & "\settings index.xml"

        Private m_SettingsIndex As SettingsIndexSettings
        Public Property SettingsIndex() As SettingsIndexSettings
            Get
                Return m_SettingsIndex
            End Get
            Set(ByVal value As SettingsIndexSettings)
                m_SettingsIndex = value
            End Set
        End Property

        Private m_Objectives As ObjectiveListSettings
        Public Property Objectives() As ObjectiveListSettings
            Get
                Return m_Objectives
            End Get
            Set(ByVal value As ObjectiveListSettings)
                m_Objectives = value
            End Set
        End Property

        Private m_PositionManager As PositionManagerSettings
        Public Property PositionManager() As PositionManagerSettings
            Get
                Return m_PositionManager
            End Get
            Set(ByVal value As PositionManagerSettings)
                m_PositionManager = value
            End Set
        End Property

        Private m_Silverpak As SilverpakSettings
        Public Property Silverpak() As SilverpakSettings
            Get
                Return m_Silverpak
            End Get
            Set(ByVal value As SilverpakSettings)
                m_Silverpak = value
            End Set
        End Property

        Private m_SmartSteps As SmartStepsSettings
        Public Property SmartSteps() As SmartStepsSettings
            Get
                Return m_SmartSteps
            End Get
            Set(ByVal value As SmartStepsSettings)
                m_SmartSteps = value
            End Set
        End Property

        Private m_Window As WindowSettings
        Public Property Window() As WindowSettings
            Get
                Return m_Window
            End Get
            Set(ByVal value As WindowSettings)
                m_Window = value
            End Set
        End Property

        Public Sub New()
            m_SettingsIndex = New SettingsIndexSettings(loadSettings(settingsIndexFile))
            m_Objectives = New ObjectiveListSettings(loadSettings(SettingsIndexSettings.Decode(m_SettingsIndex.ObjectivesFile)))
            m_PositionManager = New PositionManagerSettings(loadSettings(SettingsIndexSettings.Decode(m_SettingsIndex.PositionManagerFile)))
            m_Silverpak = New SilverpakSettings(loadSettings(SettingsIndexSettings.Decode(m_SettingsIndex.SilverpakFile)))
            m_SmartSteps = New SmartStepsSettings(loadSettings(SettingsIndexSettings.Decode(m_SettingsIndex.SmartStepsFile)))
            m_Window = New WindowSettings(loadSettings(SettingsIndexSettings.Decode(m_SettingsIndex.WindowFile)))
            '...
        End Sub

        Public Sub Save()
            ensureFoldersExist()

            Save(settingsIndexFile, m_SettingsIndex)

            Save(SettingsIndexSettings.Decode(m_SettingsIndex.ObjectivesFile), m_Objectives)
            Save(SettingsIndexSettings.Decode(m_SettingsIndex.PositionManagerFile), m_PositionManager)
            Save(SettingsIndexSettings.Decode(m_SettingsIndex.SilverpakFile), m_Silverpak)
            Save(SettingsIndexSettings.Decode(m_SettingsIndex.SmartStepsFile), m_SmartSteps)
            Save(SettingsIndexSettings.Decode(m_SettingsIndex.WindowFile), m_Window)
            '...
        End Sub
        Private Sub Save(ByVal filename As String, ByVal settings As SettingsBase)
            XmlSerializer.ToXml(filename, New KeyValuePair(Of String, Object)("", settings.GetContents))
        End Sub

        Private Sub ensureFoldersExist()
            If Not My.Computer.FileSystem.DirectoryExists(GlobalSettingsDir) Then My.Computer.FileSystem.CreateDirectory(GlobalSettingsDir)
            If Not My.Computer.FileSystem.DirectoryExists(UserSettingsDir) Then My.Computer.FileSystem.CreateDirectory(UserSettingsDir)
        End Sub

        Private Sub m_settings_RequestSave(ByVal sender As Object, ByVal e As EventArgs)
            Dim settings As SettingsBase = sender
            'TODO: get filename and save the settings
        End Sub

        Private Shared Function loadSettings(ByVal fileName As String) As Object
            'If Not My.Computer.FileSystem.FileExists(fileName) Then Return Nothing
            Dim nullableNameValue = XmlSerializer.FromXml(fileName)
            Return If(nullableNameValue IsNot Nothing, nullableNameValue.Value.Value, Nothing)
        End Function
    End Class

    Public MustInherit Class SettingsBase

        Public Event RequestSave As EventHandler
        Protected Sub OnRequestSave()
            RaiseEvent RequestSave(Me, New EventArgs)
        End Sub

        Public Sub Save()
            OnRequestSave()
        End Sub

        Public MustOverride Function GetContents() As Object

        Protected Shared Function deserialize(ByVal value As Object, ByVal type As Type) As Object
            If type Is GetType(String) OrElse type Is GetType(Integer) Then
                'primitive
                If value.GetType Is type Then Return value
            ElseIf type.IsSubclassOf(GetType(SettingsBase)) Then
                'initializable Settings
                Dim ctr = type.GetConstructor(New Type() {GetType(Object)})
                If ctr Is Nothing Then Return Nothing
                Return ctr.Invoke(New Object() {value})
            ElseIf TypeOf value Is String Then
                'check for stringified types (Boolean, Point)
                Dim strValue As String = value
                If type Is GetType(Boolean) Then
                    'String -> Boolean
                    If strValue.ToLower = "true" Then
                        Return True
                    ElseIf strValue.ToLower = "false" Then
                        Return False
                    End If
                    Return Nothing
                End If
                If type Is GetType(Point) Then
                    'String -> Point
                    Return pointFromString(strValue)
                End If
            End If
            Return Nothing
        End Function
        Protected Shared Function serialize(ByVal value As Object) As Object
            If TypeOf value Is String Or TypeOf value Is Integer Then
                Return value
            ElseIf TypeOf value Is Boolean Then
                Return value.ToString.ToLower
            ElseIf TypeOf value Is Point Then
                Return pointToString(value)
            ElseIf TypeOf value Is SettingsBase Then
                Dim valueSettings As SettingsBase = value
                Return valueSettings.GetContents
            End If
            Return Nothing
        End Function

        Protected Shared Function tryGetValue(ByVal dict As IDictionary(Of String, Object), ByVal key As String, ByRef field As String) As Boolean
            Dim objValue As Object = Nothing
            If dict.TryGetValue(key, objValue) Then
                If TypeOf objValue Is String Then
                    field = objValue
                    Return True
                End If
            End If
            Return False
        End Function
        Protected Shared Function tryGetValue(ByVal dict As IDictionary(Of String, Object), ByVal key As String, ByRef field As Integer) As Boolean
            Dim objValue As Object = Nothing
            If dict.TryGetValue(key, objValue) Then
                If TypeOf objValue Is Integer Then
                    field = objValue
                    Return True
                End If
            End If
            Return False
        End Function
        Protected Shared Function tryGetValue(ByVal dict As IDictionary(Of String, Object), ByVal key As String, ByRef field As IDictionary(Of String, Object)) As Boolean
            Dim objValue As Object = Nothing
            If dict.TryGetValue(key, objValue) Then
                If TypeOf objValue Is IDictionary(Of String, Object) Then
                    field = objValue
                    Return True
                End If
            End If
            Return False
        End Function
        Protected Shared Function tryGetValue(ByVal dict As IDictionary(Of String, Object), ByVal key As String, ByRef field As IList(Of KeyValuePair(Of String, Object))) As Boolean
            Dim objValue As Object = Nothing
            If dict.TryGetValue(key, objValue) Then
                If TypeOf objValue Is IList(Of KeyValuePair(Of String, Object)) Then
                    field = objValue
                    Return True
                End If
            End If
            Return False
        End Function
        Protected Shared Function tryGetValue(ByVal dict As IDictionary(Of String, Object), ByVal key As String, ByRef field As Boolean) As Boolean
            Dim strValue As String = Nothing
            If tryGetValue(dict, key, strValue) Then
                If strValue.ToLower = "true" Then
                    field = True
                    Return True
                ElseIf strValue.ToLower = "false" Then
                    field = False
                    Return True
                End If
            End If
            Return False
        End Function
        Protected Shared Function tryGetValue(ByVal dict As IDictionary(Of String, Object), ByVal key As String, ByRef field As Point) As Boolean
            Dim strValue As String = Nothing
            If tryGetValue(dict, key, strValue) Then
                'Return pointFromString(strValue, field)
            End If
            Return False
        End Function

        Private Shared Function pointFromString(ByVal s As String) As Object
            Dim sArr = s.Split(",")
            If sArr.Length <> 2 Then Return Nothing
            If Not (IsNumeric(sArr(0)) And IsNumeric(sArr(1))) Then Return Nothing
            Return New Point(sArr(0), sArr(1))
        End Function
        Protected Shared Function pointToString(ByVal p As Point) As String
            Return p.X & "," & p.Y
        End Function

    End Class
    Public MustInherit Class SettingsDict
        Inherits SettingsBase

        Protected Sub init(ByVal contents As Object)
            Dim dict = TryCast(contents, IDictionary(Of String, Object))
            If dict Is Nothing Then Exit Sub
            For Each prop In getSerializableProperties()
                Dim value = deserialize(dict, prop.Name, prop.PropertyType)
                Try
                    If value IsNot Nothing Then prop.SetValue(Me, value, Nothing)
                Catch ex As ArgumentException
                    'Validation failed. oh well.
                End Try
            Next
        End Sub
        Protected Overridable Function getSerializableProperties() As PropertyInfo()
            Return Me.GetType.GetProperties(BindingFlags.Instance Or BindingFlags.Public)
        End Function

        Protected Overloads Shared Function deserialize(ByVal dict As IDictionary(Of String, Object), ByVal key As String, ByVal type As Type) As Object
            Dim objValue As Object = Nothing
            If Not dict.TryGetValue(key, objValue) Then Return Nothing
            Return deserialize(objValue, type)
        End Function

        Public Overrides Function GetContents() As Object
            Dim dict As New Dictionary(Of String, Object)
            For Each prop In getSerializableProperties()
                Dim value = serialize(prop.GetValue(Me, Nothing))
                If value IsNot Nothing Then dict.Add(prop.Name, value)
            Next
            Return dict
        End Function
    End Class
    Public MustInherit Class SettingsList(Of T)
        Inherits SettingsBase

        Private m_ContentsList As New List(Of KeyValuePair(Of String, T))
        Protected Property ContentsList() As IList(Of KeyValuePair(Of String, T))
            Get
                Return m_ContentsList
            End Get
            Set(ByVal value As IList(Of KeyValuePair(Of String, T)))
                m_ContentsList = value
            End Set
        End Property

        Protected Sub init(ByVal contents As Object)
            Dim srcList = TryCast(contents, IList(Of KeyValuePair(Of String, Object)))
            If srcList Is Nothing Then Exit Sub
            Dim destList = m_ContentsList
            For Each item In srcList
                Dim nullableDestKvp = deserialize(item)
                If nullableDestKvp IsNot Nothing Then destList.Add(nullableDestKvp.Value)
            Next
        End Sub

        Protected Overloads Shared Function deserialize(ByVal kvp As KeyValuePair(Of String, Object)) As KeyValuePair(Of String, T)?
            Dim rtnValue = deserialize(kvp.Value, GetType(T))
            If rtnValue Is Nothing Then Return Nothing
            Return New KeyValuePair(Of String, T)(kvp.Key, rtnValue)
        End Function

        Public Overrides Function GetContents() As Object
            Dim lst = New List(Of KeyValuePair(Of String, Object))
            For Each kvp In ContentsList
                Dim value = serialize(kvp.Value)
                If value IsNot Nothing Then lst.Add(New KeyValuePair(Of String, Object)(kvp.Key, value))
            Next
            Return lst
        End Function

        Protected Shared Function findByName(ByVal lst As List(Of KeyValuePair(Of String, T)), ByVal name As String) As T
            For Each kvp In lst
                If kvp.Key = name Then Return kvp.Value
            Next
            Return Nothing
        End Function
    End Class

    Public Class SettingsIndexSettings
        Inherits SettingsDict

        Private Const globalPrefix = "%global%"
        Private Const userPrefix = "%user%"

        Private m_ObjectivesFile As String = globalPrefix & "\objectives.xml"
        Private m_PositionManagerSettingsFile As String = userPrefix & "\position manager.xml"
        Private m_SilverpakFile As String = globalPrefix & "\motor.xml"
        Private m_SmartStepsFile As String = userPrefix & "\smartsteps.xml"
        Private m_WindowFile As String = userPrefix & "\window.xml"

        Private m_savePictureFolder As String = My.Computer.FileSystem.SpecialDirectories.MyPictures

        Public Property SavePicturesFolder() As String
            Get
                Return m_savePictureFolder
            End Get
            Set(ByVal value As String)
                m_savePictureFolder = value
            End Set
        End Property

        Public Property ObjectivesFile() As String
            Get
                Return m_ObjectivesFile
            End Get
            Set(ByVal value As String)
                m_ObjectivesFile = value
            End Set
        End Property


        Public Property PositionManagerFile() As String
            Get
                Return m_PositionManagerSettingsFile
            End Get
            Set(ByVal value As String)
                m_PositionManagerSettingsFile = value
            End Set
        End Property


        Public Property SilverpakFile() As String
            Get
                Return m_SilverpakFile
            End Get
            Set(ByVal value As String)
                m_SilverpakFile = value
            End Set
        End Property


        Public Property SmartStepsFile() As String
            Get
                Return m_SmartStepsFile
            End Get
            Set(ByVal value As String)
                m_SmartStepsFile = value
            End Set
        End Property


        Public Property WindowFile() As String
            Get
                Return m_WindowFile
            End Get
            Set(ByVal value As String)
                m_WindowFile = value
            End Set
        End Property

        Public Sub New(ByVal contents As Object)
            init(contents)
        End Sub

        Public Shared Function Decode(ByVal path As String) As String
            Dim firstPart As String
            Dim secondPart As String
            If path.StartsWith(globalPrefix) Then
                firstPart = AllSettings.GlobalSettingsDir
                secondPart = path.Substring(globalPrefix.Length)
            ElseIf path.StartsWith(userPrefix) Then
                firstPart = AllSettings.UserSettingsDir
                secondPart = path.Substring(userPrefix.Length)
            Else
                Return ""
            End If
            Return firstPart & secondPart
        End Function
    End Class

    Public Class ObjectiveListSettings
        Inherits SettingsList(Of ObjectiveSettings)

        Public Property Objectives() As IList(Of KeyValuePair(Of String, ObjectiveSettings))
            Get
                Return ContentsList
            End Get
            Set(ByVal value As IList(Of KeyValuePair(Of String, ObjectiveSettings)))
                ContentsList = value
            End Set
        End Property

        Public Sub New(ByVal contents As Object)
            init(contents)
        End Sub

        Public Function GetObjective(ByVal objectiveName As String) As ObjectiveSettings
            Return findByName(Objectives, objectiveName)
        End Function
        Public Function GetMag(ByVal objectiveName As String, ByVal magName As String) As MagSettings
            Dim objective = GetObjective(objectiveName)
            If objective Is Nothing Then Return Nothing
            Return objective.GetMag(magName)
        End Function
        Public Function GetIris(ByVal objectiveName As String, ByVal magName As String, ByVal irisName As String) As Integer?
            Dim mag = GetMag(objectiveName, magName)
            If mag Is Nothing Then Return Nothing
            Return mag.GetIris(irisName)
        End Function
    End Class

    Public Class ObjectiveSettings
        Inherits SettingsList(Of MagSettings)

        Public Property Mags() As List(Of KeyValuePair(Of String, MagSettings))
            Get
                Return ContentsList
            End Get
            Set(ByVal value As List(Of KeyValuePair(Of String, MagSettings)))
                ContentsList = value
            End Set
        End Property

        Public Sub New(ByVal contents As Object)
            init(contents)
        End Sub

        Public Function GetMag(ByVal magName As String) As MagSettings
            'Return findByName(m_Mags, magName)
        End Function

        Public Function GetIris(ByVal magName As String, ByVal irisName As String) As Integer?
            Dim mag = GetMag(magName)
            If mag Is Nothing Then Return Nothing
            Return mag.GetIris(irisName)
        End Function

    End Class
    Public Class MagSettings
        Inherits SettingsList(Of Integer)

        Public Property Irises() As List(Of KeyValuePair(Of String, Integer))
            Get
                Return ContentsList
            End Get
            Set(ByVal value As List(Of KeyValuePair(Of String, Integer)))
                ContentsList = value
            End Set
        End Property

        Public Sub New(ByVal contents As Object)
            init(contents)
        End Sub

        Public Function GetIris(ByVal irisName As String) As Integer?
            For Each kvp In Irises
                If kvp.Key = irisName Then Return kvp.Value
            Next
            Return Nothing
        End Function
    End Class

    Public Class PositionManagerSettings
        Inherits SettingsDict

        Private Const MemRegSizeName As String = "MemRegSize"
        Private m_MemRegSize As Integer = 5
        Public Property MemRegSize() As Integer
            Get
                Return m_MemRegSize
            End Get
            Set(ByVal value As Integer)
                m_MemRegSize = value
            End Set
        End Property

        Public Sub New(ByVal contents As Object)
            init(contents)
        End Sub
    End Class

    Public Class SilverpakSettings
        Inherits SettingsDict

        Public Const AccelerationMin As Integer = 50
        Public Const AccelerationMax As Integer = 500
        Private m_Acceleration As Integer = SilverpakManager.DefaultAcceleration
        Public Property Acceleration() As Integer
            Get
                Return m_Acceleration
            End Get
            Set(ByVal value As Integer)
                If IsValidateAcceleration(value) Then m_Acceleration = value Else Throw New ArgumentException
            End Set
        End Property
        Public Shared Function IsValidateAcceleration(ByVal value As Integer) As Boolean
            Return AccelerationMin <= value And value <= AccelerationMax
        End Function

        Public Const RunningCurrentMin As Integer = 5
        Public Const RunningCurrentMax As Integer = 75
        Private m_RunningCurrent As Integer = SilverpakManager.DefaultRunningCurrent
        Public Property RunningCurrent() As Integer
            Get
                Return m_RunningCurrent
            End Get
            Set(ByVal value As Integer)
                If IsValidateRunningCurrent(value) Then m_RunningCurrent = value Else Throw New ArgumentException
            End Set
        End Property
        Public Shared Function IsValidateRunningCurrent(ByVal value As Integer) As Boolean
            Return RunningCurrentMin <= value And value <= RunningCurrentMax
        End Function

        Public Const VelocityMin As Integer = 50000
        Public Const VelocityMax As Integer = 300000
        Private m_Velocity As Integer = SilverpakManager.DefaultVelocity
        Public Property Velocity() As Integer
            Get
                Return m_Velocity
            End Get
            Set(ByVal value As Integer)
                If IsValidateVelocity(value) Then m_Velocity = value Else Throw New ArgumentException
            End Set
        End Property
        Public Shared Function IsValidateVelocity(ByVal value As Integer) As Boolean
            Return VelocityMin <= value And value <= VelocityMax
        End Function

        Public Sub New(ByVal contents As Object)
            init(contents)
        End Sub
    End Class

    Public Class SmartStepsSettings
        Inherits SettingsDict

        Private m_AutorunSetups As AutorunSetupListSettings
        Public Property AutorunSetups() As AutorunSetupListSettings
            Get
                Return m_AutorunSetups
            End Get
            Set(ByVal value As AutorunSetupListSettings)
                m_AutorunSetups = value
            End Set
        End Property

        Private m_LastAutorunSetup As AutorunSetupSettings
        Public Property LastAutorunSetup() As AutorunSetupSettings
            Get
                Return m_LastAutorunSetup
            End Get
            Set(ByVal value As AutorunSetupSettings)
                m_LastAutorunSetup = value
            End Set
        End Property

        Private m_ReturnToTop As Boolean = True
        Public Property ReturnToTop() As Boolean
            Get
                Return m_ReturnToTop
            End Get
            Set(ByVal value As Boolean)
                m_ReturnToTop = value
            End Set
        End Property

        Public Sub New(ByVal contents As Object)
            init(contents)
            'Dim autorunSetupsArg As IList(Of KeyValuePair(Of String, Object)) = Nothing
            'Dim lastAutorunSetupArg As IDictionary(Of String, Object) = Nothing
            ''...
            'Dim dict = TryCast(contents, IDictionary(Of String, Object))
            'If dict IsNot Nothing Then
            '    tryGetValue(dict, AutorunSetupsName, autorunSetupsArg)
            '    tryGetValue(dict, LastAutorunSetupName, lastAutorunSetupArg)
            '    tryGetValue(dict, ReturnToTopName, m_ReturnToTop)
            '    '...
            'End If
            'm_AutorunSetups = New AutorunSetupListSettings(autorunSetupsArg)
            'm_LastAutorunSetup = New AutorunSetupSettings(lastAutorunSetupArg)
            ''...
        End Sub

    End Class
    Public Class AutorunSetupListSettings
        Inherits SettingsList(Of AutorunSetupSettings)

        Public Property AutorunSetups() As List(Of KeyValuePair(Of String, AutorunSetupSettings))
            Get
                Return ContentsList
            End Get
            Set(ByVal value As List(Of KeyValuePair(Of String, AutorunSetupSettings)))
                ContentsList = value
            End Set
        End Property

        Public Sub New(ByVal contents As Object)
            init(contents)
        End Sub
    End Class
    Public Class AutorunRunSettings
        Inherits SettingsDict

        Private m_AutorunStart As String = ""
        Public Property AutorunStart() As String
            Get
                Return m_AutorunStart
            End Get
            Set(ByVal value As String)
                m_AutorunStart = value
            End Set
        End Property
        Public Function HasValidAutorunStart() As Boolean
            Return IsValidAutorunStart(m_AutorunStart)
        End Function
        Public Shared Function IsValidAutorunStart(ByVal value As String) As Boolean
            Return value <> "" AndAlso IsNumeric(value)
        End Function

        Private m_UseStopPosition As Boolean = True
        Public Property UseStopPosition() As Boolean
            Get
                Return m_UseStopPosition
            End Get
            Set(ByVal value As Boolean)
                m_UseStopPosition = value
            End Set
        End Property

        Private m_AutorunStop As String = ""
        Public Property AutorunStop() As String
            Get
                Return m_AutorunStop
            End Get
            Set(ByVal value As String)
                m_AutorunStop = value
            End Set
        End Property
        Public Function hasValidAutorunStop() As Boolean
            Return isValidAutorunStop(m_AutorunStop)
        End Function
        Public Shared Function isValidAutorunStop(ByVal value As String) As Boolean
            Return value <> "" AndAlso IsNumeric(value)
        End Function

        Private m_Slices As String = ""
        Public Property Slices() As String
            Get
                Return m_Slices
            End Get
            Set(ByVal value As String)
                m_Slices = value
            End Set
        End Property
        Public Function hasValidSlices() As Boolean
            Return isValidSlices(m_Slices)
        End Function
        Public Shared Function isValidSlices(ByVal value As String) As Boolean
            Return value <> "" AndAlso IsNumeric(value)
        End Function

        Public Function IsValid() As Boolean
            If Not HasValidAutorunStart() Then Return False
            If m_UseStopPosition Then
                Return (hasValidAutorunStop() AndAlso _
                        0 + m_AutorunStart < m_AutorunStop + 0)
            Else
                Return hasValidSlices()
            End If
        End Function

        Public Sub New(ByVal contents As Object)
            init(contents)
        End Sub
    End Class
    Public Class AutorunSetupSettings
        Inherits SettingsDict

        Private m_CalculateStepSize As Boolean = False
        Public Property CalculateStepSize() As Boolean
            Get
                Return m_CalculateStepSize
            End Get
            Set(ByVal value As Boolean)
                m_CalculateStepSize = value
            End Set
        End Property

        Private m_StepSize As String = ""
        Public Property StepSize() As String
            Get
                Return m_StepSize
            End Get
            Set(ByVal value As String)
                m_StepSize = value
            End Set
        End Property
        Public Function HasValidStepSize() As Boolean
            Return Not m_CalculateStepSize AndAlso isValidStepSize(m_StepSize)
        End Function
        Public Shared Function isValidStepSize(ByVal value As String) As Boolean
            Return value <> "" AndAlso IsNumeric(value) AndAlso 0 < value
        End Function

        Private m_Objective As String = ""
        Public Property Objective() As String
            Get
                Return m_Objective
            End Get
            Set(ByVal value As String)
                m_Objective = value
            End Set
        End Property

        Private m_Mag As String = ""
        Public Property Mag() As String
            Get
                Return m_Mag
            End Get
            Set(ByVal value As String)
                m_Mag = value
            End Set
        End Property

        Private m_Iris As String = ""
        Public Property Iris() As String
            Get
                Return m_Iris
            End Get
            Set(ByVal value As String)
                m_Iris = value
            End Set
        End Property

        Public Function GetStepSize(ByVal objectives As ObjectiveListSettings) As String
            Dim nullableIris = objectives.GetIris(m_Objective, m_Mag, m_Iris)
            Return If(nullableIris IsNot Nothing, nullableIris.Value.ToString, "")
        End Function
        Public Function HasValidStepSize(ByVal objectives As ObjectiveListSettings) As Boolean
            If Not m_CalculateStepSize Then Return False
            Dim nullableIris = objectives.GetIris(m_Objective, m_Mag, m_Iris)
            If nullableIris Is Nothing Then Return False
            Return isValidStepSize(nullableIris.Value)
        End Function

        Public Const DefaultDwell = "2000"
        Public Const MaxDwell = 5000
        Public Const MinDwell = 300
        Private m_Dwell As String = DefaultDwell
        Public Property Dwell() As String
            Get
                Return m_Dwell
            End Get
            Set(ByVal value As String)
                m_Dwell = value
            End Set
        End Property
        Public Function HasValidDwell() As Boolean
            Return IsValidDwell(m_Dwell)
        End Function
        Public Shared Function IsValidDwell(ByVal value As String)
            Return value <> "" AndAlso (MinDwell <= value And value <= MaxDwell)
        End Function

        Public Sub New(ByVal contents As Object)
            init(contents)
        End Sub

    End Class

    Public Class WindowSettings
        Inherits SettingsDict

        Private m_AlwaysOnTop As Boolean = False
        Public Property AlwaysOnTop() As Boolean
            Get
                Return m_AlwaysOnTop
            End Get
            Set(ByVal value As Boolean)
                m_AlwaysOnTop = value
            End Set
        End Property

        Private m_StartPosition As New Point(0, 200)
        Public Property StartPosition() As Point
            Get
                Return m_StartPosition
            End Get
            Set(ByVal value As Point)
                m_StartPosition = value
            End Set
        End Property
        Public Function GetStartPositionInScreen(ByVal windowSize As Size) As Point
            Return RectangleIntoScreen(New Rectangle(m_StartPosition, windowSize)).Location
        End Function

        Public Sub New(ByVal contents As Object)
            init(contents)
        End Sub

        Public Shared Function RectangleIntoScreen(ByVal fromRect As Rectangle) As Rectangle
            Dim closestScreenBounds As Rectangle = Screen.FromRectangle(fromRect).Bounds
            If Not closestScreenBounds.Contains(fromRect) Then
                'Rectangle needs to be moved into the closest screen
                fromRect.X = Math.Min(fromRect.X, closestScreenBounds.Right - fromRect.Width) 'maybe nudge left
                fromRect.Y = Math.Min(fromRect.Y, closestScreenBounds.Bottom - fromRect.Height) 'maybe nudge up
                fromRect.X = Math.Max(fromRect.X, closestScreenBounds.X) 'maybe nudge right
                fromRect.Y = Math.Max(fromRect.Y, closestScreenBounds.Y) 'maybe nudge down
            End If
            Return fromRect
        End Function
    End Class

End Namespace
