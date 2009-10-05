Imports System.Threading
Imports System.Drawing
Imports VisionaryDigital.CanonCamera
Imports VisionaryDigital.Silverpak23CE
Imports VisionaryDigital.SmartSteps
Imports VisionaryDigital.Settings

Namespace CamliftController

    Public Class frmControls
        'state fields
        Private m_currentPosition As Integer
        Private m_movementMode As enuMovementMode = enuMovementMode.Initializing
        Private m_autorunMode As enuAutorunMode = enuAutorunMode.Off
        Private m_isLiveViewActive As Boolean = False
        Private m_isShuttingDown As Boolean = False

        'parallel forms
        Private WithEvents m_frmLiveView As frmLiveView = Nothing

        'all settings
        Private m_allSettings As AllSettings

        'my settings
        Private m_windowSettings As WindowSettings

        'managers with settings
        Private m_positionManager As PositionManager
        Private WithEvents m_silverpakManager As SilverpakManager
        Private m_smartStepsManager As SmartStepsManager

        'resources without settings
        Private m_cam As Camera
        Private WithEvents m_autorunStepper As AsyncStepper = Nothing

        Private m_numPicsTaken As Integer = 0

        Private m_stepSizes As List(Of KeyValuePair(Of String, Integer))

        'TODO: implement this
        Public Const DefaultParkDistance As Integer = 500
        Private m_ParkDistance As Integer = DefaultParkDistance
        Public Property ParkDistance() As Integer
            Get
                Return m_ParkDistance
            End Get
            Set(ByVal value As Integer)
                m_ParkDistance = value
            End Set
        End Property

        'Constructor
        Public Sub New(ByVal settings As AllSettings, ByVal silverpakManager As Silverpak, ByVal camera As Camera)
            InitializeComponent() ' This call is required by the Windows Form Designer.

            'all settings
            m_allSettings = settings

            'my settings
            m_windowSettings = settings.Window
            Me.TopMost = m_windowSettings.AlwaysOnTop
            Me.Location = m_windowSettings.GetStartPositionInScreen(Me.Size)
            m_stepSizes = settings.PositionManager.StepSizes.StepSizes

            'managers
            m_silverpakManager = silverpakManager
            m_smartStepsManager = New SmartStepsManager(settings.SmartSteps, AddressOf moveToPosition_safe, camera, AddressOf isMoveFinished_safe, m_allSettings)
            m_positionManager = New PositionManager(settings.PositionManager, m_smartStepsManager)

            'resources
            m_cam = camera
        End Sub

        'Gui-thread event handlers
        Private Sub frmControls_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            tkbPos.Maximum = m_silverpakManager.MaxPosition
            AlwaysOnTopToolStripMenuItem.Checked = Me.TopMost
            'updateSteps_gui()
            m_movementMode = enuMovementMode.Initializing
            updateControlsEnabled_gui()
            updateSteps_gui()
            checkForInitialized_gui()
        End Sub

        Private Sub frmControls_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            If e.CloseReason = CloseReason.UserClosing And Not m_isShuttingDown Then
                Select Case MsgBox("Do you want to move the camera back to the top?", MsgBoxStyle.YesNoCancel + MsgBoxStyle.Question, MsgBoxTitle)
                    Case MsgBoxResult.Yes
                        e.Cancel = True
                        m_isShuttingDown = True
                        moveShutDown_gui()
                    Case MsgBoxResult.No

                    Case MsgBoxResult.Cancel
                        e.Cancel = True
                End Select
            End If
        End Sub

        Private Sub frmControls_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
            If m_isLiveViewActive Then m_frmLiveView.Close()
            saveSettings_gui()
        End Sub

        Private Sub PreferencesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PreferencesToolStripMenuItem.Click
            Using f As New frmPreferences(m_stepSizes, AddressOf updateSteps_safe)
                f.ShowDialog(Me)
            End Using
            resync(New Action(AddressOf saveSettings_gui))
        End Sub
        Private Sub AdvancedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdvancedToolStripMenuItem.Click
            Using f As New frmAdvanced(m_silverpakManager, Me)
                f.ShowDialog(Me)
                If f.Tag = ConnectionLostResult Then returnForm_gui(ConnectionLostResult)
            End Using
        End Sub
        Private Sub AlwaysOnTopToolStripMenuItem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AlwaysOnTopToolStripMenuItem.CheckedChanged
            Me.TopMost = AlwaysOnTopToolStripMenuItem.Checked
        End Sub
        Private Sub UsersGuideToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UsersGuideToolStripMenuItem.Click
            System.Diagnostics.Process.Start(HelpFileName)
        End Sub
        Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
            MsgBox(MsgBoxAboutMessage, MsgBoxStyle.Information, MsgBoxTitle)
        End Sub

        Private Sub tkbPos_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tkbPos.Scroll
            txtPos.Text = (tkbPos.Maximum - tkbPos.Value) '- m_trackbarOrigin
        End Sub
        Private Sub tkbPos_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tkbPos.MouseUp
            Static sl_lastPositision As Integer = 0
            If ValidateNumeric(txtPos, True) Then
                Dim newPosition As Integer = txtPos.Text
                If newPosition <> sl_lastPositision Then
                    moveAbsolute_gui(txtPos.Text)
                    sl_lastPositision = newPosition
                End If
            End If
        End Sub

        Private Sub txtPos_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPos.KeyDown
            If e.KeyCode = Keys.Return Then
                If ValidateNumeric(txtPos, True) Then moveAbsolute_gui(txtPos.Text)
            End If
        End Sub
        Private Sub btnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGo.Click
            If ValidateNumeric(txtPos, True) Then moveAbsolute_gui(txtPos.Text)
        End Sub
        Private Sub btnUp_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnUp.MouseDown
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If tkbDist.Value = 10 Then
                    beginInfinite_gui(True)
                End If
            End If
        End Sub
        Private Sub btnUp_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnUp.MouseUp
            If m_movementMode <> enuMovementMode.Absolute Then
                stopInifite_gui()
            End If
        End Sub
        Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click
            If tkbDist.Value < 10 Then
                moveStep_gui(True)
            End If
        End Sub
        Private Sub btnUp_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUp.LostFocus
            If m_movementMode <> enuMovementMode.Absolute Then
                stopInifite_gui()
            End If
        End Sub

        Private Sub btnDown_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnDown.MouseDown
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If tkbDist.Value = 10 Then
                    beginInfinite_gui(False)
                End If
            End If
        End Sub
        Private Sub btnDown_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnDown.MouseUp
            If m_movementMode <> enuMovementMode.Absolute Then
                stopInifite_gui()
            End If
        End Sub
        Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown.Click
            If tkbDist.Value < 10 Then
                moveStep_gui(False)
            End If
        End Sub
        Private Sub btnDown_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDown.LostFocus
            If m_movementMode <> enuMovementMode.Absolute Then
                stopInifite_gui()
            End If
        End Sub

        Private Sub btnAutorun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAutorun.Click
            If m_frmLiveView IsNot Nothing Then m_frmLiveView.Close()
            Using frm As New frmAutoRun(m_smartStepsManager, m_positionManager, m_allSettings.Objectives, Me)
                If frm.ShowDialog(Me) = DialogResult.OK Then
                    m_autorunStepper = frm.Tag
                    m_autorunMode = enuAutorunMode.Running
                    m_autorunStepper.Start()
                End If
            End Using
        End Sub

        Private Sub btnTakePic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTakePic.Click
            Dim TryAgain As Boolean = True

            While TryAgain
                TryAgain = False

                Try
                    m_cam.TakeSinglePicture(m_allSettings.SettingsIndex.SavePicturesFolder)
                Catch ex As SdkException When ex.Message = SdkErrors.TakePictureAfNg
                    MsgBox("Autofocus failed!" & vbCrLf & vbCrLf & "NOTE: This software is intended to be used with the camera in manual focus mode", MsgBoxStyle.Exclamation)
                Catch ex As CameraIsBusyException
                    ' do nothing
                Catch ex As DirectoryDoesNotExistException
                    m_allSettings.SettingsIndex.SavePicturesFolder = My.Computer.FileSystem.SpecialDirectories.MyPictures
                    MsgBox("Note: Your Save Pictures To folder is missing. Defaulting to " & m_allSettings.SettingsIndex.SavePicturesFolder & ". You can change this in Settings.", MsgBoxStyle.Information, MsgBoxTitle)
                    TryAgain = True
                Catch ex As Exception
                    TryAgain = HandleCameraException(ex)
                End Try
            End While

        End Sub

        Private Sub btnLiveView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLiveView.Click
            If Not m_isLiveViewActive Then
                Dim TryAgain As Boolean = True

                While TryAgain
                    TryAgain = False


                    Try
                        m_frmLiveView = New frmLiveView(m_cam)
                        m_frmLiveView.Show()
                        m_isLiveViewActive = True
                    Catch ex As SdkException When ex.SdkError = SdkErrors.CommPortIsInUse
                        MsgBox("Camera is in use!", MsgBoxStyle.Critical, MsgBoxTitle)
                    Catch ex As Exception
                        TryAgain = HandleCameraException(ex)
                    End Try
                End While
            Else
                m_frmLiveView.Focus()
            End If
        End Sub

        Private Sub tkbDist_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tkbDist.Scroll
            updateCurrentDist_gui()
        End Sub

        Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click
            emergencyStop_safe()
            btnStop.Select()
            Dim f = Me
        End Sub

        Private Sub m_silverpakManager_ConnectionLost_gui()
            returnForm_gui(ConnectionLostResult)
        End Sub
        Private Sub m_silverpakManager_PositionChanged_gui()
            m_currentPosition = m_silverpakManager.Position
            tkbPos.Value = Math.Max(0, Math.Min(tkbPos.Maximum - m_currentPosition, tkbPos.Maximum))
            txtPos.Text = m_currentPosition
            txtPos.Refresh()
        End Sub
        Private Sub m_silverpakManager_StoppedMoving_gui(ByVal reason As StoppedMovingReason)
            Select Case reason
                Case StoppedMovingReason.Normal, StoppedMovingReason.Initialized
                    If m_movementMode = enuMovementMode.ShutDown Then Close()
                    If m_autorunMode = enuAutorunMode.Off Then updateStatusStrip_gui("Stopped")
                    m_movementMode = enuMovementMode.Stopped
                    updateControlsEnabled_gui()
                Case StoppedMovingReason.InitializationAborted
                    updateStatusStrip_gui("Initialization Interrupted")
                    If MsgBox(MsgBoxInitializationAbortedMessage, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkCancel, MsgBoxTitle) = MsgBoxResult.Ok Then
                        m_silverpakManager.InitializeCoordinates()
                    Else
                        returnForm_gui(DialogResult.Cancel)
                    End If
            End Select
        End Sub

        Private Sub m_autorunStepper_Aborted_gui(ByVal sdkEx As SdkException)
            m_autorunMode = enuAutorunMode.Off
            updateStatusStrip_gui("Autorun Aborted")
            updateControlsEnabled_gui()
            If sdkEx IsNot Nothing Then
                MsgBox(sdkEx.Message, MsgBoxStyle.Exclamation, MsgBoxTitle)
            End If
        End Sub
        Private Sub m_autorunStepper_Finished_gui()
            m_autorunMode = enuAutorunMode.Off
            updateStatusStrip_gui("Autorun Finished")
            updateControlsEnabled_gui()
        End Sub
        Private Sub m_autorunStepper_ProgressReported_gui()
            updateStatusStrip_gui("Autorun..." & m_autorunStepper.Progress & "%")
        End Sub

        Private Sub m_frmLiveView_FormClosed_gui()
            m_isLiveViewActive = False
        End Sub

        'Thread-safe event handlers
        Private Sub m_silverpakManager_ConnectionLost(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_silverpakManager.ConnectionLost
            Try
                resync(New Action(AddressOf m_silverpakManager_ConnectionLost_gui))
            Catch ex As ObjectDisposedException
            End Try
        End Sub
        Private Sub m_silverpakManager_PositionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_silverpakManager.PositionChanged
            Try
                resync(New Action(AddressOf m_silverpakManager_PositionChanged_gui))
            Catch ex As ObjectDisposedException
            End Try
        End Sub
        Private Sub m_silverpakManager_StoppedMoving(ByVal sender As Object, ByVal e As StoppedMovingEventArgs) Handles m_silverpakManager.StoppedMoving
            Try
                resync(New Action(Of StoppedMovingReason)(AddressOf m_silverpakManager_StoppedMoving_gui), e.Reason)
            Catch ex As ObjectDisposedException
            End Try
        End Sub

        Private Sub m_autorunStepper_Aborted(ByVal sender As Object, ByVal e As SmartSteps.AbortedEventArgs) Handles m_autorunStepper.Aborted
            resync(New Action(Of SdkException)(AddressOf m_autorunStepper_Aborted_gui), e.AbortException)
        End Sub
        Private Sub m_autorunStepper_Finished(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_autorunStepper.Finished
            resync(New Action(AddressOf m_autorunStepper_Finished_gui))
        End Sub
        Private Sub m_autorunStepper_ProgressReported(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_autorunStepper.ProgressReported
            resync(New Action(AddressOf m_autorunStepper_ProgressReported_gui))
        End Sub

        Private Sub m_frmLiveView_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles m_frmLiveView.FormClosed
            resync(New Action(AddressOf m_frmLiveView_FormClosed_gui))
        End Sub

        'Thread-safe delegate targets
        Private Sub loadPosition_safe(ByVal value As Integer)
            resync(New Action(Of Integer)(AddressOf loadPosition_gui), value)
        End Sub
        Private Sub moveToPosition_safe(ByVal position As Integer, ByVal ParamArray array As String())
            Try
                m_silverpakManager.GoToPosition(position)
                resync(New Action(AddressOf updateControlsEnabled_gui))
            Catch ex As InvalidSilverpakOperationException
                resync(New Action(Of String)(AddressOf returnForm_gui), ConnectionLostResult)
            End Try
        End Sub
        Private Sub emergencyStop_safe()
            Try
                m_silverpakManager.StopMotor()
            Catch ex As InvalidSilverpakOperationException
            End Try
            resync(New Action(AddressOf movementAborted_gui))
        End Sub
        Private Function isMoveFinished_safe() As Boolean
            Return m_silverpakManager.IsReady
        End Function
        Private Sub updateSteps_safe()
            resync(New Action(AddressOf updateSteps_gui))
        End Sub

        'resync (thread safe)
        Private Sub resync(ByVal method As System.Delegate)
            resync(method, New Object() {}) ' "'Optional' and 'ParamAray' cannot be combined"
        End Sub
        Private Sub resync(ByVal method As System.Delegate, ByVal ParamArray args As Object())
            Try
                If Me.InvokeRequired Then
                    Me.Invoke(method, args)
                Else
                    method.Method.Invoke(Me, args)
                End If
            Catch ex As ObjectDisposedException
            End Try
        End Sub

        'Gui-thread subs
        Private Sub checkForInitialized_gui()
            If m_movementMode = enuMovementMode.Initializing AndAlso m_silverpakManager.IsReady Then
                'we missed the initialization event
                m_silverpakManager_StoppedMoving_gui(StoppedMovingReason.Initialized)
            End If
        End Sub
        Private Sub loadPosition_gui(ByVal value As Integer)
            txtPos.Text = value
            txtPos.Select()
        End Sub
        Private Sub saveSettings_gui()
            'my settings
            m_windowSettings.AlwaysOnTop = Me.TopMost
            m_windowSettings.StartPosition = Me.Location
            m_allSettings.PositionManager.StepSizes.StepSizes = m_stepSizes
            'managers
            m_positionManager.SaveSettings()
            With m_allSettings.Silverpak
                .Acceleration = m_silverpakManager.Acceleration
                .RunningCurrent = m_silverpakManager.RunningCurrent
                .Velocity = m_silverpakManager.Velocity
            End With
            m_smartStepsManager.SaveSettings()
            'all settings
            m_allSettings.Save()
        End Sub
        Private Sub beginInfinite_gui(ByVal UpNotDown As Boolean)
            updateStatusStrip_gui("Moving " & IIf(UpNotDown, "Up", "Down") & "...")
            Try
                m_silverpakManager.GoInfinite(Not UpNotDown)
                m_movementMode = enuMovementMode.Infinite
                updateControlsEnabled_gui()
            Catch ex As InvalidSilverpakOperationException
                returnForm_gui(ConnectionLostResult)
            End Try
        End Sub
        Private Sub stopInifite_gui()
            Try
                m_silverpakManager.StopMotor()
            Catch ex As InvalidSilverpakOperationException
            End Try
            updateStatusStrip_gui("Stopping...")
        End Sub
        Private Sub moveStep_gui(ByVal UpNotDown As Boolean)
            Dim steps As Integer = m_stepSizes(tkbDist.Value - 1).Value
            updateStatusStrip_gui("Moving " & IIf(UpNotDown, "Up", "Down") & " " & steps & " steps...")
            m_movementMode = enuMovementMode.Absolute
            moveToPosition_safe(m_silverpakManager.Position + If(UpNotDown, -steps, steps))
        End Sub
        Private Sub moveAbsolute_gui(ByVal position As Integer)
            updateStatusStrip_gui("Moving to " & position & "...")
            m_movementMode = enuMovementMode.Absolute
            moveToPosition_safe(position)
        End Sub
        Private Sub moveShutDown_gui()
            updateStatusStrip_gui("Shutting Down...")
            m_movementMode = enuMovementMode.ShutDown
            moveToPosition_safe(0)
        End Sub
        Private Sub movementAborted_gui()
            If m_autorunMode = enuAutorunMode.Running Then
                m_autorunStepper.Abort()
                m_autorunMode = enuAutorunMode.Aborting
            End If
            updateStatusStrip_gui("Emergency Stop")
        End Sub

        Private Sub updateCurrentDist_gui()
            If tkbDist.Value = 10 Then
                lblCurrentDist.Text = "Infinite"
            Else
                lblCurrentDist.Text = m_stepSizes(tkbDist.Value - 1).Value
            End If
        End Sub

        Private Sub updateControlsEnabled_gui()
            'lists of Controls/ToolStripItems that enable/disable together
            Static movementControls As Control() = { _
                tkbPos, _
                txtPos, _
                btnGo, _
                btnUp, _
                btnDown}
            Static otherMenuItems As ToolStripItem() = { _
                PreferencesToolStripMenuItem, _
                AdvancedToolStripMenuItem, _
                SavePicturesFolderMenuItem}
            Static otherControls As Control() = { _
                btnAutorun, _
                btnAutoStart, _
                btnAutoStop, _
                btnTakePic, _
                btnLiveView, _
                tkbDist}

            'determine whether each group is enabled/disabled
            Dim movementEnabled = True
            Dim otherEnabled = True
            Select Case m_movementMode
                Case enuMovementMode.Initializing
                    movementEnabled = False
                    otherEnabled = False
                Case enuMovementMode.Absolute, enuMovementMode.Infinite, enuMovementMode.ShutDown
                    otherEnabled = False
                Case enuMovementMode.Stopped
                    ' both true
            End Select
            Select Case m_autorunMode
                Case Is <> enuAutorunMode.Off
                    movementEnabled = False
                    otherEnabled = False
            End Select

            'apply to each group
            For Each item In movementControls
                item.Enabled = movementEnabled
            Next
            For Each item In otherMenuItems
                item.Enabled = otherEnabled
            Next
            For Each item In otherControls
                item.Enabled = otherEnabled
            Next
        End Sub

        Private Sub updateSteps_gui()
            For i = 0 To m_stepSizes.Count - 1
                Dim lblDist As Label = pnlDist.Controls("lblDist" & i + 1)
                If i < LabeledStepsCount Then
                    lblDist.Text = m_stepSizes(i).Key
                Else
                    lblDist.Text = MicrostepsToMilimeters(m_stepSizes(i).Value) & "mm"
                End If
            Next
            updateCurrentDist_gui()
        End Sub

        Private Sub updateStatusStrip_gui(ByVal newStatus As String)
            tslStatus.Text = newStatus
            stsStatusStrip.Refresh()
        End Sub

        Private Sub returnForm_gui(ByVal rslt As String)
            Me.Tag = rslt
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End Sub

        'Private Enums
        Private Enum enuMovementMode
            Initializing
            Stopped
            Infinite
            Absolute
            ShutDown
        End Enum
        Private Enum enuAutorunMode
            Off
            Running
            Aborting
            Finishing
        End Enum

        Private Sub SavePicturesFolderMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SavePicturesFolderMenuItem.Click
            ' pop up a folder location dialog
            Dim dialog As New FolderBrowserDialog
            dialog.SelectedPath = m_allSettings.SettingsIndex.SavePicturesFolder
            Dim result As DialogResult = dialog.ShowDialog()

            If result = DialogResult.Cancel Then Exit Sub

            m_allSettings.SettingsIndex.SavePicturesFolder = dialog.SelectedPath


        End Sub

        Private Sub SetAutorunStart()
            If ValidateNumeric(txtPos, True) Then m_smartStepsManager.LastAutorunRun.AutorunStart = Val(txtPos.Text)
            lblAutoStart.Text = m_smartStepsManager.LastAutorunRun.AutorunStart
        End Sub

        Private Sub SetAutorunStop()
            If ValidateNumeric(txtPos, True) Then m_smartStepsManager.LastAutorunRun.AutorunStop = Val(txtPos.Text)
            lblAutoStop.Text = m_smartStepsManager.LastAutorunRun.AutorunStop
        End Sub

        Private Sub btnAutoStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAutoStart.Click
            SetAutorunStart()
        End Sub

        Private Sub btnAutoStop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAutoStop.Click
            SetAutorunStop()
        End Sub

        Private Sub BuildLoadMenu(ByVal menu As ToolStripItemCollection)
            menu.Clear()
            Dim loadItems = m_positionManager.MakeLoadMenu(AddressOf loadPosition_safe).Items
            While loadItems.Count > 0
                menu.Add(loadItems(0))
            End While
        End Sub

        Private Sub BuildStoreMenu(ByVal menu As ToolStripItemCollection)
            menu.Clear()
            Dim position As Integer
            Try
                position = Val(txtPos.Text)
            Catch ex As Exception
                position = 0
            End Try
            Dim storeItems As ToolStripItemCollection = m_positionManager.MakeStoreMenu(position).Items
            While storeItems.Count > 0
                menu.Add(storeItems(0))
            End While
        End Sub

        Private Sub cmsPosition_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cmsPosition.Opening
            BuildLoadMenu(tsmLoad.DropDownItems)
            BuildStoreMenu(tsmStore.DropDownItems)
        End Sub

        Private Function initValueMenuItem_safe(ByVal baseTitle As String, ByVal value As Object, ByVal clickHandler As EventHandler) As ToolStripMenuItem
            Dim tempText = baseTitle
            Dim valueIsNull = value Is Nothing OrElse value.ToString = ""
            If Not valueIsNull Then tempText &= " = " & value
            Dim tsmi = New ToolStripMenuItem(tempText)
            tsmi.Enabled = Not valueIsNull
            AddHandler tsmi.Click, clickHandler
            Return tsmi
        End Function

        Private Sub SetAutorunStartToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetAutorunStartToolStripMenuItem.Click
            SetAutorunStart()
        End Sub

        Private Sub SetAutorunStopHereToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetAutorunStopHereToolStripMenuItem.Click
            SetAutorunStop()
        End Sub

        Private Sub ManageMemoryRegistersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManageMemoryRegistersToolStripMenuItem.Click
            Dim inst As New frmManageMemoryRegisters(m_positionManager)

            inst.ShowDialog()
        End Sub

        Private Sub btnSavePos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSavePos.Click
            Dim menu As New ContextMenuStrip
            BuildStoreMenu(menu.Items)
            menu.Show(btnSavePos, New Point(0, btnSavePos.Height))
        End Sub

        Private Sub btnLoadPos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadPos.Click
            Dim menu As New ContextMenuStrip
            BuildLoadMenu(menu.Items)
            menu.Show(btnLoadPos, New Point(0, btnLoadPos.Height))
        End Sub
    End Class

End Namespace
