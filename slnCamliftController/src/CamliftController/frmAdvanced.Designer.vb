Namespace CamliftController


    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmAdvanced
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.btnApply = New System.Windows.Forms.Button
            Me.btnOk = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.grpAdvancedSettings = New System.Windows.Forms.GroupBox
            Me.btnUnlock = New System.Windows.Forms.Button
            Me.txtCurrent = New System.Windows.Forms.TextBox
            Me.lblCurrentLabel = New System.Windows.Forms.Label
            Me.btnRestoreDefaults = New System.Windows.Forms.Button
            Me.txtAcceleration = New System.Windows.Forms.TextBox
            Me.txtVelocity = New System.Windows.Forms.TextBox
            Me.lblAccelerationLabel = New System.Windows.Forms.Label
            Me.lblVelocityLabel = New System.Windows.Forms.Label
            Me.lblRolloverHelp = New System.Windows.Forms.Label
            Me.grpAdvancedSettings.SuspendLayout()
            Me.SuspendLayout()
            '
            'btnApply
            '
            Me.btnApply.Location = New System.Drawing.Point(174, 200)
            Me.btnApply.Name = "btnApply"
            Me.btnApply.Size = New System.Drawing.Size(75, 23)
            Me.btnApply.TabIndex = 4
            Me.btnApply.Text = "&Apply"
            Me.btnApply.UseVisualStyleBackColor = True
            '
            'btnOk
            '
            Me.btnOk.Location = New System.Drawing.Point(12, 200)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(75, 23)
            Me.btnOk.TabIndex = 2
            Me.btnOk.Text = "&OK"
            Me.btnOk.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(93, 200)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 3
            Me.btnCancel.Text = "&Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'grpAdvancedSettings
            '
            Me.grpAdvancedSettings.Controls.Add(Me.btnUnlock)
            Me.grpAdvancedSettings.Controls.Add(Me.txtCurrent)
            Me.grpAdvancedSettings.Controls.Add(Me.lblCurrentLabel)
            Me.grpAdvancedSettings.Controls.Add(Me.btnRestoreDefaults)
            Me.grpAdvancedSettings.Controls.Add(Me.txtAcceleration)
            Me.grpAdvancedSettings.Controls.Add(Me.txtVelocity)
            Me.grpAdvancedSettings.Controls.Add(Me.lblAccelerationLabel)
            Me.grpAdvancedSettings.Controls.Add(Me.lblVelocityLabel)
            Me.grpAdvancedSettings.Location = New System.Drawing.Point(12, 12)
            Me.grpAdvancedSettings.Name = "grpAdvancedSettings"
            Me.grpAdvancedSettings.Size = New System.Drawing.Size(237, 126)
            Me.grpAdvancedSettings.TabIndex = 0
            Me.grpAdvancedSettings.TabStop = False
            Me.grpAdvancedSettings.Text = "Advanced Settings"
            '
            'btnUnlock
            '
            Me.btnUnlock.Location = New System.Drawing.Point(8, 97)
            Me.btnUnlock.Name = "btnUnlock"
            Me.btnUnlock.Size = New System.Drawing.Size(75, 23)
            Me.btnUnlock.TabIndex = 6
            Me.btnUnlock.Text = "Edit &Settings"
            Me.btnUnlock.UseVisualStyleBackColor = True
            '
            'txtCurrent
            '
            Me.txtCurrent.Enabled = False
            Me.txtCurrent.Location = New System.Drawing.Point(131, 71)
            Me.txtCurrent.Name = "txtCurrent"
            Me.txtCurrent.Size = New System.Drawing.Size(100, 20)
            Me.txtCurrent.TabIndex = 5
            Me.txtCurrent.Visible = False
            '
            'lblCurrentLabel
            '
            Me.lblCurrentLabel.AutoSize = True
            Me.lblCurrentLabel.Enabled = False
            Me.lblCurrentLabel.Location = New System.Drawing.Point(6, 74)
            Me.lblCurrentLabel.Name = "lblCurrentLabel"
            Me.lblCurrentLabel.Size = New System.Drawing.Size(87, 13)
            Me.lblCurrentLabel.TabIndex = 4
            Me.lblCurrentLabel.Text = "Running &Current:"
            Me.lblCurrentLabel.Visible = False
            '
            'btnRestoreDefaults
            '
            Me.btnRestoreDefaults.Enabled = False
            Me.btnRestoreDefaults.Location = New System.Drawing.Point(134, 97)
            Me.btnRestoreDefaults.Name = "btnRestoreDefaults"
            Me.btnRestoreDefaults.Size = New System.Drawing.Size(97, 23)
            Me.btnRestoreDefaults.TabIndex = 7
            Me.btnRestoreDefaults.Text = "Restore &Defaults"
            Me.btnRestoreDefaults.UseVisualStyleBackColor = True
            '
            'txtAcceleration
            '
            Me.txtAcceleration.Enabled = False
            Me.txtAcceleration.Location = New System.Drawing.Point(131, 45)
            Me.txtAcceleration.Name = "txtAcceleration"
            Me.txtAcceleration.Size = New System.Drawing.Size(100, 20)
            Me.txtAcceleration.TabIndex = 3
            '
            'txtVelocity
            '
            Me.txtVelocity.Enabled = False
            Me.txtVelocity.Location = New System.Drawing.Point(131, 19)
            Me.txtVelocity.Name = "txtVelocity"
            Me.txtVelocity.Size = New System.Drawing.Size(100, 20)
            Me.txtVelocity.TabIndex = 1
            '
            'lblAccelerationLabel
            '
            Me.lblAccelerationLabel.AutoSize = True
            Me.lblAccelerationLabel.Enabled = False
            Me.lblAccelerationLabel.Location = New System.Drawing.Point(6, 48)
            Me.lblAccelerationLabel.Name = "lblAccelerationLabel"
            Me.lblAccelerationLabel.Size = New System.Drawing.Size(99, 13)
            Me.lblAccelerationLabel.TabIndex = 2
            Me.lblAccelerationLabel.Text = "Motor &Acceleration:"
            '
            'lblVelocityLabel
            '
            Me.lblVelocityLabel.AutoSize = True
            Me.lblVelocityLabel.Enabled = False
            Me.lblVelocityLabel.Location = New System.Drawing.Point(6, 22)
            Me.lblVelocityLabel.Name = "lblVelocityLabel"
            Me.lblVelocityLabel.Size = New System.Drawing.Size(77, 13)
            Me.lblVelocityLabel.TabIndex = 0
            Me.lblVelocityLabel.Text = "Motor &Velocity:"
            '
            'lblRolloverHelp
            '
            Me.lblRolloverHelp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblRolloverHelp.ForeColor = System.Drawing.SystemColors.ControlDarkDark
            Me.lblRolloverHelp.Location = New System.Drawing.Point(12, 141)
            Me.lblRolloverHelp.Name = "lblRolloverHelp"
            Me.lblRolloverHelp.Size = New System.Drawing.Size(237, 56)
            Me.lblRolloverHelp.TabIndex = 1
            '
            'frmAdvanced
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(253, 227)
            Me.ControlBox = False
            Me.Controls.Add(Me.lblRolloverHelp)
            Me.Controls.Add(Me.grpAdvancedSettings)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.btnApply)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(259, 255)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(259, 255)
            Me.Name = "frmAdvanced"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
            Me.Text = "Advanced Settings"
            Me.grpAdvancedSettings.ResumeLayout(False)
            Me.grpAdvancedSettings.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents btnApply As System.Windows.Forms.Button
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents grpAdvancedSettings As System.Windows.Forms.GroupBox
        Friend WithEvents lblAccelerationLabel As System.Windows.Forms.Label
        Friend WithEvents lblVelocityLabel As System.Windows.Forms.Label
        Friend WithEvents txtCurrent As System.Windows.Forms.TextBox
        Friend WithEvents lblCurrentLabel As System.Windows.Forms.Label
        Friend WithEvents btnRestoreDefaults As System.Windows.Forms.Button
        Friend WithEvents txtAcceleration As System.Windows.Forms.TextBox
        Friend WithEvents txtVelocity As System.Windows.Forms.TextBox
        Friend WithEvents btnUnlock As System.Windows.Forms.Button
        Friend WithEvents lblRolloverHelp As System.Windows.Forms.Label
    End Class
End Namespace
