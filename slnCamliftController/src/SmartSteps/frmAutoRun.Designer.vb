Namespace SmartSteps

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmAutoRun
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
            Me.lblStart = New System.Windows.Forms.Label
            Me.txtStart = New System.Windows.Forms.TextBox
            Me.btnStartLoad = New System.Windows.Forms.Button
            Me.rdoSlices = New System.Windows.Forms.RadioButton
            Me.rdoStopPosition = New System.Windows.Forms.RadioButton
            Me.txtStepSize = New System.Windows.Forms.TextBox
            Me.grpRun = New System.Windows.Forms.GroupBox
            Me.txtSlices = New System.Windows.Forms.TextBox
            Me.txtStopPosition = New System.Windows.Forms.TextBox
            Me.btnStopPositionLoad = New System.Windows.Forms.Button
            Me.txtDwell = New System.Windows.Forms.TextBox
            Me.lblDwell = New System.Windows.Forms.Label
            Me.grpSetup = New System.Windows.Forms.GroupBox
            Me.btnLoad = New System.Windows.Forms.Button
            Me.btnSave = New System.Windows.Forms.Button
            Me.lblIris = New System.Windows.Forms.Label
            Me.lblMag = New System.Windows.Forms.Label
            Me.lblObjective = New System.Windows.Forms.Label
            Me.cboIris = New System.Windows.Forms.ComboBox
            Me.cboObjective = New System.Windows.Forms.ComboBox
            Me.rdoStepSizeCalculated = New System.Windows.Forms.RadioButton
            Me.rdoStepSizeManual = New System.Windows.Forms.RadioButton
            Me.cboMag = New System.Windows.Forms.ComboBox
            Me.chkReturnToTop = New System.Windows.Forms.CheckBox
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnStart = New System.Windows.Forms.Button
            Me.nudOverlap = New System.Windows.Forms.NumericUpDown
            Me.lblOverlap = New System.Windows.Forms.Label
            Me.grpRun.SuspendLayout()
            Me.grpSetup.SuspendLayout()
            CType(Me.nudOverlap, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblStart
            '
            Me.lblStart.AutoSize = True
            Me.lblStart.Location = New System.Drawing.Point(6, 22)
            Me.lblStart.Name = "lblStart"
            Me.lblStart.Size = New System.Drawing.Size(72, 13)
            Me.lblStart.TabIndex = 100
            Me.lblStart.Text = "Start Position:"
            Me.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'txtStart
            '
            Me.txtStart.Location = New System.Drawing.Point(131, 19)
            Me.txtStart.Name = "txtStart"
            Me.txtStart.Size = New System.Drawing.Size(79, 20)
            Me.txtStart.TabIndex = 0
            '
            'btnStartLoad
            '
            Me.btnStartLoad.AutoSize = True
            Me.btnStartLoad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.btnStartLoad.Location = New System.Drawing.Point(216, 17)
            Me.btnStartLoad.Name = "btnStartLoad"
            Me.btnStartLoad.Size = New System.Drawing.Size(41, 23)
            Me.btnStartLoad.TabIndex = 20
            Me.btnStartLoad.Text = "Load"
            Me.btnStartLoad.UseVisualStyleBackColor = True
            '
            'rdoSlices
            '
            Me.rdoSlices.AutoSize = True
            Me.rdoSlices.Location = New System.Drawing.Point(6, 78)
            Me.rdoSlices.Name = "rdoSlices"
            Me.rdoSlices.Size = New System.Drawing.Size(108, 17)
            Me.rdoSlices.TabIndex = 10
            Me.rdoSlices.TabStop = True
            Me.rdoSlices.Text = "Number of Slices:"
            Me.rdoSlices.UseVisualStyleBackColor = True
            '
            'rdoStopPosition
            '
            Me.rdoStopPosition.AutoSize = True
            Me.rdoStopPosition.Location = New System.Drawing.Point(6, 49)
            Me.rdoStopPosition.Name = "rdoStopPosition"
            Me.rdoStopPosition.Size = New System.Drawing.Size(90, 17)
            Me.rdoStopPosition.TabIndex = 11
            Me.rdoStopPosition.TabStop = True
            Me.rdoStopPosition.Text = "Stop Position:"
            Me.rdoStopPosition.UseVisualStyleBackColor = True
            '
            'txtStepSize
            '
            Me.txtStepSize.Location = New System.Drawing.Point(131, 19)
            Me.txtStepSize.Name = "txtStepSize"
            Me.txtStepSize.Size = New System.Drawing.Size(79, 20)
            Me.txtStepSize.TabIndex = 3
            '
            'grpRun
            '
            Me.grpRun.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.grpRun.Controls.Add(Me.rdoSlices)
            Me.grpRun.Controls.Add(Me.rdoStopPosition)
            Me.grpRun.Controls.Add(Me.txtSlices)
            Me.grpRun.Controls.Add(Me.txtStopPosition)
            Me.grpRun.Controls.Add(Me.btnStopPositionLoad)
            Me.grpRun.Controls.Add(Me.txtStart)
            Me.grpRun.Controls.Add(Me.btnStartLoad)
            Me.grpRun.Controls.Add(Me.lblStart)
            Me.grpRun.Location = New System.Drawing.Point(13, 13)
            Me.grpRun.Name = "grpRun"
            Me.grpRun.Size = New System.Drawing.Size(263, 103)
            Me.grpRun.TabIndex = 102
            Me.grpRun.TabStop = False
            Me.grpRun.Text = "Run"
            '
            'txtSlices
            '
            Me.txtSlices.Location = New System.Drawing.Point(131, 77)
            Me.txtSlices.Name = "txtSlices"
            Me.txtSlices.Size = New System.Drawing.Size(79, 20)
            Me.txtSlices.TabIndex = 2
            '
            'txtStopPosition
            '
            Me.txtStopPosition.Location = New System.Drawing.Point(131, 48)
            Me.txtStopPosition.Name = "txtStopPosition"
            Me.txtStopPosition.Size = New System.Drawing.Size(79, 20)
            Me.txtStopPosition.TabIndex = 1
            '
            'btnStopPositionLoad
            '
            Me.btnStopPositionLoad.AutoSize = True
            Me.btnStopPositionLoad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.btnStopPositionLoad.Location = New System.Drawing.Point(216, 46)
            Me.btnStopPositionLoad.Name = "btnStopPositionLoad"
            Me.btnStopPositionLoad.Size = New System.Drawing.Size(41, 23)
            Me.btnStopPositionLoad.TabIndex = 21
            Me.btnStopPositionLoad.Text = "Load"
            Me.btnStopPositionLoad.UseVisualStyleBackColor = True
            '
            'txtDwell
            '
            Me.txtDwell.Location = New System.Drawing.Point(131, 182)
            Me.txtDwell.Name = "txtDwell"
            Me.txtDwell.Size = New System.Drawing.Size(79, 20)
            Me.txtDwell.TabIndex = 4
            '
            'lblDwell
            '
            Me.lblDwell.AutoSize = True
            Me.lblDwell.Location = New System.Drawing.Point(6, 185)
            Me.lblDwell.Name = "lblDwell"
            Me.lblDwell.Size = New System.Drawing.Size(58, 13)
            Me.lblDwell.TabIndex = 104
            Me.lblDwell.Text = "Dwell (ms):"
            Me.lblDwell.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'grpSetup
            '
            Me.grpSetup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.grpSetup.Controls.Add(Me.lblOverlap)
            Me.grpSetup.Controls.Add(Me.nudOverlap)
            Me.grpSetup.Controls.Add(Me.btnLoad)
            Me.grpSetup.Controls.Add(Me.btnSave)
            Me.grpSetup.Controls.Add(Me.lblIris)
            Me.grpSetup.Controls.Add(Me.lblMag)
            Me.grpSetup.Controls.Add(Me.lblObjective)
            Me.grpSetup.Controls.Add(Me.txtDwell)
            Me.grpSetup.Controls.Add(Me.cboIris)
            Me.grpSetup.Controls.Add(Me.cboObjective)
            Me.grpSetup.Controls.Add(Me.rdoStepSizeCalculated)
            Me.grpSetup.Controls.Add(Me.lblDwell)
            Me.grpSetup.Controls.Add(Me.rdoStepSizeManual)
            Me.grpSetup.Controls.Add(Me.cboMag)
            Me.grpSetup.Controls.Add(Me.txtStepSize)
            Me.grpSetup.Location = New System.Drawing.Point(13, 122)
            Me.grpSetup.Name = "grpSetup"
            Me.grpSetup.Size = New System.Drawing.Size(263, 237)
            Me.grpSetup.TabIndex = 106
            Me.grpSetup.TabStop = False
            Me.grpSetup.Text = "Settings"
            '
            'btnLoad
            '
            Me.btnLoad.Location = New System.Drawing.Point(182, 208)
            Me.btnLoad.Name = "btnLoad"
            Me.btnLoad.Size = New System.Drawing.Size(75, 23)
            Me.btnLoad.TabIndex = 31
            Me.btnLoad.Text = "Load..."
            Me.btnLoad.UseVisualStyleBackColor = True
            '
            'btnSave
            '
            Me.btnSave.Location = New System.Drawing.Point(101, 208)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New System.Drawing.Size(75, 23)
            Me.btnSave.TabIndex = 30
            Me.btnSave.Text = "Save..."
            Me.btnSave.UseVisualStyleBackColor = True
            '
            'lblIris
            '
            Me.lblIris.AutoSize = True
            Me.lblIris.Location = New System.Drawing.Point(23, 129)
            Me.lblIris.Name = "lblIris"
            Me.lblIris.Size = New System.Drawing.Size(23, 13)
            Me.lblIris.TabIndex = 113
            Me.lblIris.Text = "Iris:"
            '
            'lblMag
            '
            Me.lblMag.AutoSize = True
            Me.lblMag.Location = New System.Drawing.Point(23, 99)
            Me.lblMag.Name = "lblMag"
            Me.lblMag.Size = New System.Drawing.Size(31, 13)
            Me.lblMag.TabIndex = 112
            Me.lblMag.Text = "Mag:"
            '
            'lblObjective
            '
            Me.lblObjective.AutoSize = True
            Me.lblObjective.Location = New System.Drawing.Point(23, 69)
            Me.lblObjective.Name = "lblObjective"
            Me.lblObjective.Size = New System.Drawing.Size(55, 13)
            Me.lblObjective.TabIndex = 111
            Me.lblObjective.Text = "Objective:"
            '
            'cboIris
            '
            Me.cboIris.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboIris.FormattingEnabled = True
            Me.cboIris.Location = New System.Drawing.Point(131, 126)
            Me.cboIris.Name = "cboIris"
            Me.cboIris.Size = New System.Drawing.Size(79, 21)
            Me.cboIris.TabIndex = 110
            '
            'cboObjective
            '
            Me.cboObjective.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboObjective.FormattingEnabled = True
            Me.cboObjective.Location = New System.Drawing.Point(131, 66)
            Me.cboObjective.Name = "cboObjective"
            Me.cboObjective.Size = New System.Drawing.Size(79, 21)
            Me.cboObjective.TabIndex = 109
            '
            'rdoStepSizeCalculated
            '
            Me.rdoStepSizeCalculated.AutoSize = True
            Me.rdoStepSizeCalculated.Location = New System.Drawing.Point(6, 43)
            Me.rdoStepSizeCalculated.Name = "rdoStepSizeCalculated"
            Me.rdoStepSizeCalculated.Size = New System.Drawing.Size(75, 17)
            Me.rdoStepSizeCalculated.TabIndex = 107
            Me.rdoStepSizeCalculated.TabStop = True
            Me.rdoStepSizeCalculated.Text = "Calculated"
            Me.rdoStepSizeCalculated.UseVisualStyleBackColor = True
            '
            'rdoStepSizeManual
            '
            Me.rdoStepSizeManual.AutoSize = True
            Me.rdoStepSizeManual.Location = New System.Drawing.Point(6, 20)
            Me.rdoStepSizeManual.Name = "rdoStepSizeManual"
            Me.rdoStepSizeManual.Size = New System.Drawing.Size(60, 17)
            Me.rdoStepSizeManual.TabIndex = 108
            Me.rdoStepSizeManual.TabStop = True
            Me.rdoStepSizeManual.Text = "Manual"
            Me.rdoStepSizeManual.UseVisualStyleBackColor = True
            '
            'cboMag
            '
            Me.cboMag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboMag.FormattingEnabled = True
            Me.cboMag.Location = New System.Drawing.Point(131, 96)
            Me.cboMag.Name = "cboMag"
            Me.cboMag.Size = New System.Drawing.Size(79, 21)
            Me.cboMag.TabIndex = 106
            '
            'chkReturnToTop
            '
            Me.chkReturnToTop.AutoSize = True
            Me.chkReturnToTop.Location = New System.Drawing.Point(22, 364)
            Me.chkReturnToTop.Name = "chkReturnToTop"
            Me.chkReturnToTop.Size = New System.Drawing.Size(151, 17)
            Me.chkReturnToTop.TabIndex = 104
            Me.chkReturnToTop.Text = "Return to top after finished"
            Me.chkReturnToTop.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(201, 387)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 51
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnStart
            '
            Me.btnStart.Location = New System.Drawing.Point(120, 387)
            Me.btnStart.Name = "btnStart"
            Me.btnStart.Size = New System.Drawing.Size(75, 23)
            Me.btnStart.TabIndex = 50
            Me.btnStart.Text = "Start"
            Me.btnStart.UseVisualStyleBackColor = True
            '
            'nudOverlap
            '
            Me.nudOverlap.Location = New System.Drawing.Point(131, 156)
            Me.nudOverlap.Maximum = New Decimal(New Integer() {70, 0, 0, 0})
            Me.nudOverlap.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
            Me.nudOverlap.Name = "nudOverlap"
            Me.nudOverlap.Size = New System.Drawing.Size(79, 20)
            Me.nudOverlap.TabIndex = 114
            Me.nudOverlap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'lblOverlap
            '
            Me.lblOverlap.AutoSize = True
            Me.lblOverlap.Location = New System.Drawing.Point(6, 158)
            Me.lblOverlap.Name = "lblOverlap"
            Me.lblOverlap.Size = New System.Drawing.Size(90, 13)
            Me.lblOverlap.TabIndex = 115
            Me.lblOverlap.Text = "Slice Overlap (%):"
            '
            'frmAutoRun
            '
            Me.AcceptButton = Me.btnStart
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoSize = True
            Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(353, 440)
            Me.Controls.Add(Me.btnStart)
            Me.Controls.Add(Me.chkReturnToTop)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.grpSetup)
            Me.Controls.Add(Me.grpRun)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmAutoRun"
            Me.Padding = New System.Windows.Forms.Padding(10)
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.Text = "Auto Run"
            Me.grpRun.ResumeLayout(False)
            Me.grpRun.PerformLayout()
            Me.grpSetup.ResumeLayout(False)
            Me.grpSetup.PerformLayout()
            CType(Me.nudOverlap, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents txtStart As System.Windows.Forms.TextBox
        Friend WithEvents lblStart As System.Windows.Forms.Label
        Friend WithEvents btnStartLoad As System.Windows.Forms.Button
        Friend WithEvents rdoSlices As System.Windows.Forms.RadioButton
        Friend WithEvents rdoStopPosition As System.Windows.Forms.RadioButton
        Friend WithEvents grpRun As System.Windows.Forms.GroupBox
        Friend WithEvents txtStepSize As System.Windows.Forms.TextBox
        Friend WithEvents btnStopPositionLoad As System.Windows.Forms.Button
        Friend WithEvents txtStopPosition As System.Windows.Forms.TextBox
        Friend WithEvents txtSlices As System.Windows.Forms.TextBox
        Friend WithEvents txtDwell As System.Windows.Forms.TextBox
        Friend WithEvents lblDwell As System.Windows.Forms.Label
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnStart As System.Windows.Forms.Button
        Friend WithEvents btnSave As System.Windows.Forms.Button
        Friend WithEvents btnLoad As System.Windows.Forms.Button
        Friend WithEvents chkReturnToTop As System.Windows.Forms.CheckBox
        Friend WithEvents grpSetup As System.Windows.Forms.GroupBox
        Friend WithEvents cboMag As System.Windows.Forms.ComboBox
        Friend WithEvents cboObjective As System.Windows.Forms.ComboBox
        Friend WithEvents rdoStepSizeCalculated As System.Windows.Forms.RadioButton
        Friend WithEvents rdoStepSizeManual As System.Windows.Forms.RadioButton
        Friend WithEvents lblIris As System.Windows.Forms.Label
        Friend WithEvents lblMag As System.Windows.Forms.Label
        Friend WithEvents lblObjective As System.Windows.Forms.Label
        Friend WithEvents cboIris As System.Windows.Forms.ComboBox
        Friend WithEvents nudOverlap As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblOverlap As System.Windows.Forms.Label
    End Class

End Namespace
