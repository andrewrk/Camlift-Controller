Imports VisionaryDigital.Silverpak23CE
Imports VisionaryDigital.Settings

Namespace CamliftController

    Public Class frmAdvanced

        Private WithEvents m_spmMain As SilverpakManager
        Private m_parentForm As frmControls

        Public Sub New(ByVal spm As SilverpakManager, ByVal parentForm As frmControls)
            InitializeComponent() ' This call is required by the Windows Form Designer.

            m_spmMain = spm
            m_parentForm = parentForm
        End Sub
        Private Sub frmAdvanced_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            txtVelocity.Text = m_spmMain.Velocity
            txtAcceleration.Text = m_spmMain.Acceleration
            txtCurrent.Text = m_spmMain.RunningCurrent
            Me.AcceptButton = btnOk
            btnApply.Enabled = False
        End Sub
        Private Sub frmAdvanced_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
            If e.Modifiers = Keys.Control And e.KeyCode = Keys.R Then
                'unlock running current
                lblCurrentLabel.Visible = True
                txtCurrent.Visible = True
            End If
        End Sub

        Private Sub ctrVelocity_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs)
            lblRolloverHelp.Text = RolloverHelpVelocity
        End Sub
        Private Sub ctrVelocity_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs)
            lblRolloverHelp.Text = RolloverHelpNone
        End Sub

        Private Sub ctrAcceleration_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs)
            lblRolloverHelp.Text = RolloverHelpAcceleration
        End Sub
        Private Sub ctrAcceleration_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs)
            lblRolloverHelp.Text = RolloverHelpNone
        End Sub

        Private Sub txt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtVelocity.TextChanged, txtAcceleration.TextChanged, txtCurrent.TextChanged
            btnApply.Enabled = True
            Me.AcceptButton = btnApply
        End Sub

        Private Sub btnUnlock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnlock.Click
            If MsgBox(MsgBoxEditAdvancedSettingsMessage, MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, MsgBoxTitle) = MsgBoxResult.Yes Then
                lblVelocityLabel.Enabled = True
                lblAccelerationLabel.Enabled = True
                lblCurrentLabel.Enabled = True
                txtVelocity.Enabled = True
                txtAcceleration.Enabled = True
                txtCurrent.Enabled = True
                btnRestoreDefaults.Enabled = True
                btnUnlock.Enabled = False

                AddHandler lblVelocityLabel.MouseEnter, AddressOf ctrVelocity_MouseEnter
                AddHandler txtVelocity.MouseEnter, AddressOf ctrVelocity_MouseEnter
                AddHandler lblVelocityLabel.MouseLeave, AddressOf ctrVelocity_MouseLeave
                AddHandler txtVelocity.MouseLeave, AddressOf ctrVelocity_MouseLeave

                AddHandler lblAccelerationLabel.MouseEnter, AddressOf ctrAcceleration_MouseEnter
                AddHandler txtAcceleration.MouseEnter, AddressOf ctrAcceleration_MouseEnter
                AddHandler lblAccelerationLabel.MouseLeave, AddressOf ctrAcceleration_MouseLeave
                AddHandler txtAcceleration.MouseLeave, AddressOf ctrAcceleration_MouseLeave
            End If
        End Sub
        Private Sub btnRestoreDefaults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestoreDefaults.Click
            txtVelocity.Text = SilverpakManager.DefaultVelocity
            txtAcceleration.Text = SilverpakManager.DefaultAcceleration
            If txtCurrent.Visible Then txtCurrent.Text = SilverpakManager.DefaultRunningCurrent
        End Sub

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
            If ApplySettings() Then
                Me.Close()
            End If
        End Sub
        Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Me.Close()
        End Sub
        Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
            If ApplySettings() Then
                Me.AcceptButton = btnOk
                btnApply.Enabled = False
            End If
        End Sub

        Private Sub m_spmMain_ConnectionLost(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_spmMain.ConnectionLost
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of String)(AddressOf returnForm), ConnectionLostResult)
            Else
                returnForm(ConnectionLostResult)
            End If
        End Sub

        Private Function ApplySettings() As Boolean
            'Validate fields
            If Not checkNumericRange(txtVelocity, SilverpakSettings.VelocityMin, SilverpakSettings.VelocityMax) Then Return False
            If Not checkNumericRange(txtAcceleration, SilverpakSettings.AccelerationMin, SilverpakSettings.AccelerationMax) Then Return False
            If Not checkNumericRange(txtCurrent, SilverpakSettings.RunningCurrentMin, SilverpakSettings.RunningCurrentMax) Then Return False
            If True Then Return False
            'All fields are valid
            m_spmMain.Velocity = txtVelocity.Text
            m_spmMain.Acceleration = txtAcceleration.Text
            If txtCurrent.Visible Then m_spmMain.RunningCurrent = txtCurrent.Text
            Try
                m_spmMain.ResendMotorSettings()
            Catch ex As InvalidSilverpakOperationException
                returnForm(ConnectionLostResult)
            End Try
            Return True
        End Function

        Private Sub returnForm(ByVal rslt As String)
            Me.Tag = rslt
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End Sub

        Private Shared Function checkNumericRange(ByVal txtVal As TextBox, ByVal minVal As Integer, ByVal maxVal As Integer) As Boolean
            If Not IsNumeric(txtVal.Text) Then GoTo ValidationFail
            If txtVal.Text < minVal Or txtVal.Text > maxVal Then GoTo ValidationFail
            Return True

ValidationFail:
            MsgBox("Value must be a number between " & minVal & " and " & maxVal & ".", MsgBoxStyle.Exclamation, MsgBoxTitle)
            txtVal.Select()
            txtVal.SelectAll()
            Return False
        End Function
    End Class

End Namespace
