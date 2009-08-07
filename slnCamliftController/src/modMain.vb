Imports VisionaryDigital.Silverpak23CE
Imports System.Drawing
Imports System.Xml
Imports VisionaryDigital.CanonCamera
Imports VisionaryDigital.CamliftController
Imports VisionaryDigital.SmartSteps
Imports VisionaryDigital.Settings


Public Module modMain

    Public Const MicrostepsPerMm As Integer = 1250

    Public Const MsgBoxSilverpakNotFoundMessage As String = "Camlift motor not found. Please check all connections."
    Public Const MsgBoxCameraNotFoundMessage = "No Camera Found. Please check all connections."
    Public Const MsgBoxTooManyCamerasMessage = "More than one camera found. Please make sure no other cameras are connected."
    Public Const MsgBoxDeviceIsBusyMessage = "Camera device is busy. You can disconnect the power to force it to reset."

    Public Const ConnectionLostResult = "Connection Lost"
    Public Const MsgBoxTitle As String = "Camlift Controller"
    Public Const MsgBoxInitializationAbortedMessage As String = "Initialization was interrupted." & vbCrLf & vbCrLf & "Continue initialization?"
    Public Const MsgBoxInitializeCoordinatesMessage As String = "Initializing will raise the carriage of the P-51 Camlift to its upper most position." & vbCrLf & vbCrLf & "Continue?"
    Public Const MsgBoxEditAdvancedSettingsMessage As String = "WARNING: changing the values in this ""Advanced Settings"" dialog box will alter the way the P-51 CamLift operates." & vbCrLf & vbCrLf & _
                                                               "These adjustments are recommended for advanced users only are you sure you want to prodeed?"
    Public Const MsgBoxShutDownMessage As String = "After parking the carriage slide the lower limit switch to the upper most position and secure it." & vbCrLf & vbCrLf & _
                                                   "Are you sure you want shut down?"
    Public Const MsgBoxConfirmDeleteMessage As String = "Are you sure you want to delete this position?"
    Public ReadOnly MsgBoxAboutMessage As String = "P-51 Camlift Controller" & vbCrLf & _
                                                   "Copyright " & ChrW(169) & "  2008 Visionary Digital" & vbCrLf & _
                                                   "Version " & Application.ProductVersion

    Public Const RolloverHelpVelocity As String = "Increasing this value will speed up the lift; decreasing the value will slow it down."
    Public Const RolloverHelpAcceleration As String = "A lower value will cause the motor to slow or ramp down before stopping. It will also cause the motor to ramp up prior to full speed."
    Public Const RolloverHelpNone As String = ""

    Public Const StepsCount = 9
    Public Const LabeledStepsCount = 6



    Private ReadOnly globalErrorLogFile = My.Computer.FileSystem.SpecialDirectories.Desktop & "\Camlift Error Report.txt"

    Public ReadOnly HelpFileName As String = Application.StartupPath & "\help.html"

    Public Sub Main()
#If Not Debug Then
        AddHandler Application.ThreadException, AddressOf applicationExceptionHandler
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf appDomainExceptionHandler
#End If

        Application.EnableVisualStyles()

        Dim settings As New AllSettings
        Using cam As New Camera, spm As New Silverpak(settings.Silverpak)
            
            While True
                Try
                    spm.EstablishConnection()
                    Exit While
                Catch ex As SilverpakNotFoundException
                    If MsgBox(MsgBoxSilverpakNotFoundMessage, MsgBoxStyle.RetryCancel + MsgBoxStyle.Critical, MsgBoxTitle) = MsgBoxResult.Cancel Then Exit Sub
                End Try
            End While

            Dim form As New CamliftController.frmControls(settings, spm, cam)

            form.ShowDialog()
        End Using
    End Sub

    Public Function HandleCameraException(ByVal ex As Exception) As Boolean
        If TypeOf ex Is NoCameraFoundException Then
            Return Not (MsgBoxResult.Cancel = MsgBox(MsgBoxCameraNotFoundMessage, MsgBoxStyle.RetryCancel + MsgBoxStyle.Critical, MsgBoxTitle))
        ElseIf TypeOf ex Is TooManyCamerasFoundException Then
            Return Not (MsgBoxResult.Cancel = MsgBox(MsgBoxTooManyCamerasMessage, MsgBoxStyle.RetryCancel + MsgBoxStyle.Critical, MsgBoxTitle))
        ElseIf TypeOf ex Is SdkException Then
            If CType(ex, SdkException).SdkError = SdkErrors.DeviceBusy Then
                Return Not (MsgBoxResult.Cancel = MsgBox(MsgBoxDeviceIsBusyMessage, MsgBoxStyle.RetryCancel + MsgBoxStyle.Critical, MsgBoxTitle))
            Else
                Throw ex
            End If
        Else
            Throw ex
        End If
    End Function

    Public Function MicrostepsToMilimeters(ByVal microsteps As Integer) As String
        Return Format(microsteps / MicrostepsPerMm, "0.00")
    End Function
    Public Function ValidateNumeric(ByVal textControl As TextBox, ByVal msg As Boolean) As Boolean
        If IsNumeric(textControl.Text) Then Return True
        If msg Then
            MsgBox("Value must be numeric.", MsgBoxStyle.Exclamation, MsgBoxTitle)
            textControl.Select()
            textControl.SelectAll()
        Else

        End If
        Return False
    End Function
    Public Function ValidateRange(ByVal textControl As TextBox, ByVal minVal As Double, ByVal maxVal As Double) As Boolean
        If Not ValidateNumeric(textControl, True) Then Return False
        If textControl.Text >= minVal And textControl.Text <= maxVal Then Return True
        MsgBox("Value must be in the range " & Format(minVal, "0") & " to " & Format(maxVal, "0"), MsgBoxStyle.Exclamation, MsgBoxTitle)
        textControl.Select()
        textControl.SelectAll()
        Return False
    End Function

    Private Sub applicationExceptionHandler(ByVal sender As Object, ByVal e As Threading.ThreadExceptionEventArgs)
        GlobalCatch(e.Exception)
    End Sub
    Private Sub appDomainExceptionHandler(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
        GlobalCatch(e.ExceptionObject)
    End Sub

    Public Sub GlobalCatch(ByVal ex As Exception)
        Dim outStr As String = "Exception time: " & Now.ToString & vbCrLf
        outStr &= "Tpye: " & ex.GetType.ToString & vbCrLf
        outStr &= "Message: " & ex.Message & vbCrLf
        outStr &= ex.ToString & vbCrLf
        outStr &= vbCrLf
        outStr &= vbCrLf
        My.Computer.FileSystem.WriteAllText(globalErrorLogFile, outStr, True)

        MsgBox("An unexpected error occurred! A report has been made at """ & globalErrorLogFile & """.", MsgBoxStyle.Critical, MsgBoxTitle)
    End Sub

End Module

