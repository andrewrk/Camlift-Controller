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
            Me.btnStopSave = New System.Windows.Forms.Button
            Me.btnStartSave = New System.Windows.Forms.Button
            Me.txtSlices = New System.Windows.Forms.TextBox
            Me.txtStopPosition = New System.Windows.Forms.TextBox
            Me.btnStopPositionLoad = New System.Windows.Forms.Button
            Me.txtDwell = New System.Windows.Forms.TextBox
            Me.lblDwell = New System.Windows.Forms.Label
            Me.grpSetup = New System.Windows.Forms.GroupBox
            Me.grpIris = New System.Windows.Forms.GroupBox
            Me.RadioButton11 = New System.Windows.Forms.RadioButton
            Me.RadioButton12 = New System.Windows.Forms.RadioButton
            Me.RadioButton13 = New System.Windows.Forms.RadioButton
            Me.RadioButton14 = New System.Windows.Forms.RadioButton
            Me.RadioButton15 = New System.Windows.Forms.RadioButton
            Me.grpMag = New System.Windows.Forms.GroupBox
            Me.RadioButton6 = New System.Windows.Forms.RadioButton
            Me.RadioButton7 = New System.Windows.Forms.RadioButton
            Me.RadioButton8 = New System.Windows.Forms.RadioButton
            Me.RadioButton9 = New System.Windows.Forms.RadioButton
            Me.RadioButton10 = New System.Windows.Forms.RadioButton
            Me.grpObjective = New System.Windows.Forms.GroupBox
            Me.RadioButton16 = New System.Windows.Forms.RadioButton
            Me.RadioButton17 = New System.Windows.Forms.RadioButton
            Me.RadioButton5 = New System.Windows.Forms.RadioButton
            Me.RadioButton4 = New System.Windows.Forms.RadioButton
            Me.RadioButton3 = New System.Windows.Forms.RadioButton
            Me.RadioButton2 = New System.Windows.Forms.RadioButton
            Me.RadioButton1 = New System.Windows.Forms.RadioButton
            Me.lblOverlap = New System.Windows.Forms.Label
            Me.nudOverlap = New System.Windows.Forms.NumericUpDown
            Me.btnLoad = New System.Windows.Forms.Button
            Me.btnSave = New System.Windows.Forms.Button
            Me.rdoStepSizeCalculated = New System.Windows.Forms.RadioButton
            Me.rdoStepSizeManual = New System.Windows.Forms.RadioButton
            Me.chkReturnToTop = New System.Windows.Forms.CheckBox
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnStart = New System.Windows.Forms.Button
            Me.btnStartHere = New System.Windows.Forms.Button
            Me.btnStopHere = New System.Windows.Forms.Button
            Me.grpRun.SuspendLayout()
            Me.grpSetup.SuspendLayout()
            Me.grpIris.SuspendLayout()
            Me.grpMag.SuspendLayout()
            Me.grpObjective.SuspendLayout()
            CType(Me.nudOverlap, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblStart
            '
            Me.lblStart.AutoSize = True
            Me.lblStart.Location = New System.Drawing.Point(6, 22)
            Me.lblStart.Name = "lblStart"
            Me.lblStart.Size = New System.Drawing.Size(72, 13)
            Me.lblStart.TabIndex = 0
            Me.lblStart.Text = "St&art Position:"
            Me.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'txtStart
            '
            Me.txtStart.Location = New System.Drawing.Point(131, 19)
            Me.txtStart.Name = "txtStart"
            Me.txtStart.Size = New System.Drawing.Size(79, 20)
            Me.txtStart.TabIndex = 1
            '
            'btnStartLoad
            '
            Me.btnStartLoad.AutoSize = True
            Me.btnStartLoad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.btnStartLoad.Location = New System.Drawing.Point(216, 17)
            Me.btnStartLoad.Name = "btnStartLoad"
            Me.btnStartLoad.Size = New System.Drawing.Size(41, 23)
            Me.btnStartLoad.TabIndex = 2
            Me.btnStartLoad.Text = "Load"
            Me.btnStartLoad.UseVisualStyleBackColor = True
            '
            'rdoSlices
            '
            Me.rdoSlices.AutoSize = True
            Me.rdoSlices.Location = New System.Drawing.Point(6, 78)
            Me.rdoSlices.Name = "rdoSlices"
            Me.rdoSlices.Size = New System.Drawing.Size(108, 17)
            Me.rdoSlices.TabIndex = 6
            Me.rdoSlices.TabStop = True
            Me.rdoSlices.Text = "Number of S&lices:"
            Me.rdoSlices.UseVisualStyleBackColor = True
            '
            'rdoStopPosition
            '
            Me.rdoStopPosition.AutoSize = True
            Me.rdoStopPosition.Location = New System.Drawing.Point(6, 49)
            Me.rdoStopPosition.Name = "rdoStopPosition"
            Me.rdoStopPosition.Size = New System.Drawing.Size(90, 17)
            Me.rdoStopPosition.TabIndex = 3
            Me.rdoStopPosition.TabStop = True
            Me.rdoStopPosition.Text = "Sto&p Position:"
            Me.rdoStopPosition.UseVisualStyleBackColor = True
            '
            'txtStepSize
            '
            Me.txtStepSize.Location = New System.Drawing.Point(131, 19)
            Me.txtStepSize.Name = "txtStepSize"
            Me.txtStepSize.Size = New System.Drawing.Size(79, 20)
            Me.txtStepSize.TabIndex = 1
            '
            'grpRun
            '
            Me.grpRun.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.grpRun.Controls.Add(Me.btnStopHere)
            Me.grpRun.Controls.Add(Me.btnStartHere)
            Me.grpRun.Controls.Add(Me.btnStopSave)
            Me.grpRun.Controls.Add(Me.btnStartSave)
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
            Me.grpRun.Size = New System.Drawing.Size(370, 103)
            Me.grpRun.TabIndex = 0
            Me.grpRun.TabStop = False
            Me.grpRun.Text = "Run"
            '
            'btnStopSave
            '
            Me.btnStopSave.Location = New System.Drawing.Point(264, 45)
            Me.btnStopSave.Name = "btnStopSave"
            Me.btnStopSave.Size = New System.Drawing.Size(43, 23)
            Me.btnStopSave.TabIndex = 8
            Me.btnStopSave.Text = "Save"
            Me.btnStopSave.UseVisualStyleBackColor = True
            '
            'btnStartSave
            '
            Me.btnStartSave.Location = New System.Drawing.Point(264, 17)
            Me.btnStartSave.Name = "btnStartSave"
            Me.btnStartSave.Size = New System.Drawing.Size(43, 23)
            Me.btnStartSave.TabIndex = 8
            Me.btnStartSave.Text = "Save"
            Me.btnStartSave.UseVisualStyleBackColor = True
            '
            'txtSlices
            '
            Me.txtSlices.Location = New System.Drawing.Point(131, 77)
            Me.txtSlices.Name = "txtSlices"
            Me.txtSlices.Size = New System.Drawing.Size(79, 20)
            Me.txtSlices.TabIndex = 7
            '
            'txtStopPosition
            '
            Me.txtStopPosition.Location = New System.Drawing.Point(131, 48)
            Me.txtStopPosition.Name = "txtStopPosition"
            Me.txtStopPosition.Size = New System.Drawing.Size(79, 20)
            Me.txtStopPosition.TabIndex = 4
            '
            'btnStopPositionLoad
            '
            Me.btnStopPositionLoad.AutoSize = True
            Me.btnStopPositionLoad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.btnStopPositionLoad.Location = New System.Drawing.Point(216, 46)
            Me.btnStopPositionLoad.Name = "btnStopPositionLoad"
            Me.btnStopPositionLoad.Size = New System.Drawing.Size(41, 23)
            Me.btnStopPositionLoad.TabIndex = 5
            Me.btnStopPositionLoad.Text = "Load"
            Me.btnStopPositionLoad.UseVisualStyleBackColor = True
            '
            'txtDwell
            '
            Me.txtDwell.Location = New System.Drawing.Point(131, 296)
            Me.txtDwell.Name = "txtDwell"
            Me.txtDwell.Size = New System.Drawing.Size(79, 20)
            Me.txtDwell.TabIndex = 9
            '
            'lblDwell
            '
            Me.lblDwell.AutoSize = True
            Me.lblDwell.Location = New System.Drawing.Point(6, 299)
            Me.lblDwell.Name = "lblDwell"
            Me.lblDwell.Size = New System.Drawing.Size(58, 13)
            Me.lblDwell.TabIndex = 8
            Me.lblDwell.Text = "D&well (ms):"
            Me.lblDwell.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'grpSetup
            '
            Me.grpSetup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.grpSetup.Controls.Add(Me.grpIris)
            Me.grpSetup.Controls.Add(Me.grpMag)
            Me.grpSetup.Controls.Add(Me.grpObjective)
            Me.grpSetup.Controls.Add(Me.lblOverlap)
            Me.grpSetup.Controls.Add(Me.nudOverlap)
            Me.grpSetup.Controls.Add(Me.btnLoad)
            Me.grpSetup.Controls.Add(Me.btnSave)
            Me.grpSetup.Controls.Add(Me.txtDwell)
            Me.grpSetup.Controls.Add(Me.rdoStepSizeCalculated)
            Me.grpSetup.Controls.Add(Me.lblDwell)
            Me.grpSetup.Controls.Add(Me.rdoStepSizeManual)
            Me.grpSetup.Controls.Add(Me.txtStepSize)
            Me.grpSetup.Location = New System.Drawing.Point(13, 122)
            Me.grpSetup.Name = "grpSetup"
            Me.grpSetup.Size = New System.Drawing.Size(370, 351)
            Me.grpSetup.TabIndex = 1
            Me.grpSetup.TabStop = False
            Me.grpSetup.Text = "Setup"
            '
            'grpIris
            '
            Me.grpIris.Controls.Add(Me.RadioButton11)
            Me.grpIris.Controls.Add(Me.RadioButton12)
            Me.grpIris.Controls.Add(Me.RadioButton13)
            Me.grpIris.Controls.Add(Me.RadioButton14)
            Me.grpIris.Controls.Add(Me.RadioButton15)
            Me.grpIris.Location = New System.Drawing.Point(246, 66)
            Me.grpIris.Name = "grpIris"
            Me.grpIris.Size = New System.Drawing.Size(107, 179)
            Me.grpIris.TabIndex = 5
            Me.grpIris.TabStop = False
            Me.grpIris.Text = "&Iris"
            '
            'RadioButton11
            '
            Me.RadioButton11.AutoSize = True
            Me.RadioButton11.Location = New System.Drawing.Point(6, 108)
            Me.RadioButton11.Name = "RadioButton11"
            Me.RadioButton11.Size = New System.Drawing.Size(96, 17)
            Me.RadioButton11.TabIndex = 4
            Me.RadioButton11.TabStop = True
            Me.RadioButton11.Text = "RadioButton11"
            Me.RadioButton11.UseVisualStyleBackColor = True
            '
            'RadioButton12
            '
            Me.RadioButton12.AutoSize = True
            Me.RadioButton12.Location = New System.Drawing.Point(6, 85)
            Me.RadioButton12.Name = "RadioButton12"
            Me.RadioButton12.Size = New System.Drawing.Size(96, 17)
            Me.RadioButton12.TabIndex = 3
            Me.RadioButton12.TabStop = True
            Me.RadioButton12.Text = "RadioButton12"
            Me.RadioButton12.UseVisualStyleBackColor = True
            '
            'RadioButton13
            '
            Me.RadioButton13.AutoSize = True
            Me.RadioButton13.Location = New System.Drawing.Point(6, 62)
            Me.RadioButton13.Name = "RadioButton13"
            Me.RadioButton13.Size = New System.Drawing.Size(96, 17)
            Me.RadioButton13.TabIndex = 2
            Me.RadioButton13.TabStop = True
            Me.RadioButton13.Text = "RadioButton13"
            Me.RadioButton13.UseVisualStyleBackColor = True
            '
            'RadioButton14
            '
            Me.RadioButton14.AutoSize = True
            Me.RadioButton14.Location = New System.Drawing.Point(6, 39)
            Me.RadioButton14.Name = "RadioButton14"
            Me.RadioButton14.Size = New System.Drawing.Size(96, 17)
            Me.RadioButton14.TabIndex = 1
            Me.RadioButton14.TabStop = True
            Me.RadioButton14.Text = "RadioButton14"
            Me.RadioButton14.UseVisualStyleBackColor = True
            '
            'RadioButton15
            '
            Me.RadioButton15.AutoSize = True
            Me.RadioButton15.Location = New System.Drawing.Point(6, 16)
            Me.RadioButton15.Name = "RadioButton15"
            Me.RadioButton15.Size = New System.Drawing.Size(38, 17)
            Me.RadioButton15.TabIndex = 0
            Me.RadioButton15.TabStop = True
            Me.RadioButton15.Text = "P3"
            Me.RadioButton15.UseVisualStyleBackColor = True
            '
            'grpMag
            '
            Me.grpMag.Controls.Add(Me.RadioButton6)
            Me.grpMag.Controls.Add(Me.RadioButton7)
            Me.grpMag.Controls.Add(Me.RadioButton8)
            Me.grpMag.Controls.Add(Me.RadioButton9)
            Me.grpMag.Controls.Add(Me.RadioButton10)
            Me.grpMag.Location = New System.Drawing.Point(131, 66)
            Me.grpMag.Name = "grpMag"
            Me.grpMag.Size = New System.Drawing.Size(109, 179)
            Me.grpMag.TabIndex = 4
            Me.grpMag.TabStop = False
            Me.grpMag.Text = "Ma&g:"
            '
            'RadioButton6
            '
            Me.RadioButton6.AutoSize = True
            Me.RadioButton6.Location = New System.Drawing.Point(6, 108)
            Me.RadioButton6.Name = "RadioButton6"
            Me.RadioButton6.Size = New System.Drawing.Size(90, 17)
            Me.RadioButton6.TabIndex = 4
            Me.RadioButton6.TabStop = True
            Me.RadioButton6.Text = "RadioButton6"
            Me.RadioButton6.UseVisualStyleBackColor = True
            '
            'RadioButton7
            '
            Me.RadioButton7.AutoSize = True
            Me.RadioButton7.Location = New System.Drawing.Point(6, 85)
            Me.RadioButton7.Name = "RadioButton7"
            Me.RadioButton7.Size = New System.Drawing.Size(90, 17)
            Me.RadioButton7.TabIndex = 3
            Me.RadioButton7.TabStop = True
            Me.RadioButton7.Text = "RadioButton7"
            Me.RadioButton7.UseVisualStyleBackColor = True
            '
            'RadioButton8
            '
            Me.RadioButton8.AutoSize = True
            Me.RadioButton8.Location = New System.Drawing.Point(6, 62)
            Me.RadioButton8.Name = "RadioButton8"
            Me.RadioButton8.Size = New System.Drawing.Size(90, 17)
            Me.RadioButton8.TabIndex = 2
            Me.RadioButton8.TabStop = True
            Me.RadioButton8.Text = "RadioButton8"
            Me.RadioButton8.UseVisualStyleBackColor = True
            '
            'RadioButton9
            '
            Me.RadioButton9.AutoSize = True
            Me.RadioButton9.Location = New System.Drawing.Point(6, 39)
            Me.RadioButton9.Name = "RadioButton9"
            Me.RadioButton9.Size = New System.Drawing.Size(90, 17)
            Me.RadioButton9.TabIndex = 1
            Me.RadioButton9.TabStop = True
            Me.RadioButton9.Text = "RadioButton9"
            Me.RadioButton9.UseVisualStyleBackColor = True
            '
            'RadioButton10
            '
            Me.RadioButton10.AutoSize = True
            Me.RadioButton10.Location = New System.Drawing.Point(6, 16)
            Me.RadioButton10.Name = "RadioButton10"
            Me.RadioButton10.Size = New System.Drawing.Size(38, 17)
            Me.RadioButton10.TabIndex = 0
            Me.RadioButton10.TabStop = True
            Me.RadioButton10.Text = "P3"
            Me.RadioButton10.UseVisualStyleBackColor = True
            '
            'grpObjective
            '
            Me.grpObjective.Controls.Add(Me.RadioButton16)
            Me.grpObjective.Controls.Add(Me.RadioButton17)
            Me.grpObjective.Controls.Add(Me.RadioButton5)
            Me.grpObjective.Controls.Add(Me.RadioButton4)
            Me.grpObjective.Controls.Add(Me.RadioButton3)
            Me.grpObjective.Controls.Add(Me.RadioButton2)
            Me.grpObjective.Controls.Add(Me.RadioButton1)
            Me.grpObjective.Location = New System.Drawing.Point(6, 66)
            Me.grpObjective.Name = "grpObjective"
            Me.grpObjective.Size = New System.Drawing.Size(119, 179)
            Me.grpObjective.TabIndex = 3
            Me.grpObjective.TabStop = False
            Me.grpObjective.Text = "O&bjective:"
            '
            'RadioButton16
            '
            Me.RadioButton16.AutoSize = True
            Me.RadioButton16.Location = New System.Drawing.Point(6, 154)
            Me.RadioButton16.Name = "RadioButton16"
            Me.RadioButton16.Size = New System.Drawing.Size(96, 17)
            Me.RadioButton16.TabIndex = 6
            Me.RadioButton16.TabStop = True
            Me.RadioButton16.Text = "RadioButton16"
            Me.RadioButton16.UseVisualStyleBackColor = True
            '
            'RadioButton17
            '
            Me.RadioButton17.AutoSize = True
            Me.RadioButton17.Location = New System.Drawing.Point(6, 131)
            Me.RadioButton17.Name = "RadioButton17"
            Me.RadioButton17.Size = New System.Drawing.Size(96, 17)
            Me.RadioButton17.TabIndex = 5
            Me.RadioButton17.TabStop = True
            Me.RadioButton17.Text = "RadioButton17"
            Me.RadioButton17.UseVisualStyleBackColor = True
            '
            'RadioButton5
            '
            Me.RadioButton5.AutoSize = True
            Me.RadioButton5.Location = New System.Drawing.Point(6, 108)
            Me.RadioButton5.Name = "RadioButton5"
            Me.RadioButton5.Size = New System.Drawing.Size(90, 17)
            Me.RadioButton5.TabIndex = 4
            Me.RadioButton5.TabStop = True
            Me.RadioButton5.Text = "RadioButton5"
            Me.RadioButton5.UseVisualStyleBackColor = True
            '
            'RadioButton4
            '
            Me.RadioButton4.AutoSize = True
            Me.RadioButton4.Location = New System.Drawing.Point(6, 85)
            Me.RadioButton4.Name = "RadioButton4"
            Me.RadioButton4.Size = New System.Drawing.Size(90, 17)
            Me.RadioButton4.TabIndex = 3
            Me.RadioButton4.TabStop = True
            Me.RadioButton4.Text = "RadioButton4"
            Me.RadioButton4.UseVisualStyleBackColor = True
            '
            'RadioButton3
            '
            Me.RadioButton3.AutoSize = True
            Me.RadioButton3.Location = New System.Drawing.Point(6, 62)
            Me.RadioButton3.Name = "RadioButton3"
            Me.RadioButton3.Size = New System.Drawing.Size(90, 17)
            Me.RadioButton3.TabIndex = 2
            Me.RadioButton3.TabStop = True
            Me.RadioButton3.Text = "RadioButton3"
            Me.RadioButton3.UseVisualStyleBackColor = True
            '
            'RadioButton2
            '
            Me.RadioButton2.AutoSize = True
            Me.RadioButton2.Location = New System.Drawing.Point(6, 39)
            Me.RadioButton2.Name = "RadioButton2"
            Me.RadioButton2.Size = New System.Drawing.Size(90, 17)
            Me.RadioButton2.TabIndex = 1
            Me.RadioButton2.TabStop = True
            Me.RadioButton2.Text = "RadioButton2"
            Me.RadioButton2.UseVisualStyleBackColor = True
            '
            'RadioButton1
            '
            Me.RadioButton1.AutoSize = True
            Me.RadioButton1.Location = New System.Drawing.Point(6, 16)
            Me.RadioButton1.Name = "RadioButton1"
            Me.RadioButton1.Size = New System.Drawing.Size(75, 17)
            Me.RadioButton1.TabIndex = 0
            Me.RadioButton1.TabStop = True
            Me.RadioButton1.Text = "10X Nikon"
            Me.RadioButton1.UseVisualStyleBackColor = True
            '
            'lblOverlap
            '
            Me.lblOverlap.AutoSize = True
            Me.lblOverlap.Location = New System.Drawing.Point(6, 272)
            Me.lblOverlap.Name = "lblOverlap"
            Me.lblOverlap.Size = New System.Drawing.Size(90, 13)
            Me.lblOverlap.TabIndex = 6
            Me.lblOverlap.Text = "Slice O&verlap (%):"
            '
            'nudOverlap
            '
            Me.nudOverlap.Location = New System.Drawing.Point(131, 270)
            Me.nudOverlap.Maximum = New Decimal(New Integer() {70, 0, 0, 0})
            Me.nudOverlap.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
            Me.nudOverlap.Name = "nudOverlap"
            Me.nudOverlap.Size = New System.Drawing.Size(79, 20)
            Me.nudOverlap.TabIndex = 7
            Me.nudOverlap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'btnLoad
            '
            Me.btnLoad.Location = New System.Drawing.Point(182, 322)
            Me.btnLoad.Name = "btnLoad"
            Me.btnLoad.Size = New System.Drawing.Size(75, 23)
            Me.btnLoad.TabIndex = 11
            Me.btnLoad.Text = "L&oad..."
            Me.btnLoad.UseVisualStyleBackColor = True
            '
            'btnSave
            '
            Me.btnSave.Location = New System.Drawing.Point(101, 322)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New System.Drawing.Size(75, 23)
            Me.btnSave.TabIndex = 10
            Me.btnSave.Text = "&Save..."
            Me.btnSave.UseVisualStyleBackColor = True
            '
            'rdoStepSizeCalculated
            '
            Me.rdoStepSizeCalculated.AutoSize = True
            Me.rdoStepSizeCalculated.Location = New System.Drawing.Point(6, 43)
            Me.rdoStepSizeCalculated.Name = "rdoStepSizeCalculated"
            Me.rdoStepSizeCalculated.Size = New System.Drawing.Size(75, 17)
            Me.rdoStepSizeCalculated.TabIndex = 2
            Me.rdoStepSizeCalculated.TabStop = True
            Me.rdoStepSizeCalculated.Text = "&Calculated"
            Me.rdoStepSizeCalculated.UseVisualStyleBackColor = True
            '
            'rdoStepSizeManual
            '
            Me.rdoStepSizeManual.AutoSize = True
            Me.rdoStepSizeManual.Location = New System.Drawing.Point(6, 20)
            Me.rdoStepSizeManual.Name = "rdoStepSizeManual"
            Me.rdoStepSizeManual.Size = New System.Drawing.Size(60, 17)
            Me.rdoStepSizeManual.TabIndex = 0
            Me.rdoStepSizeManual.TabStop = True
            Me.rdoStepSizeManual.Text = "&Manual"
            Me.rdoStepSizeManual.UseVisualStyleBackColor = True
            '
            'chkReturnToTop
            '
            Me.chkReturnToTop.AutoSize = True
            Me.chkReturnToTop.Location = New System.Drawing.Point(22, 479)
            Me.chkReturnToTop.Name = "chkReturnToTop"
            Me.chkReturnToTop.Size = New System.Drawing.Size(151, 17)
            Me.chkReturnToTop.TabIndex = 2
            Me.chkReturnToTop.Text = "Return to top after &finished"
            Me.chkReturnToTop.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(329, 502)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 4
            Me.btnCancel.Text = "Ca&ncel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnStart
            '
            Me.btnStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnStart.Location = New System.Drawing.Point(248, 502)
            Me.btnStart.Name = "btnStart"
            Me.btnStart.Size = New System.Drawing.Size(75, 23)
            Me.btnStart.TabIndex = 3
            Me.btnStart.Text = "Star&t"
            Me.btnStart.UseVisualStyleBackColor = True
            '
            'btnStartHere
            '
            Me.btnStartHere.Location = New System.Drawing.Point(313, 17)
            Me.btnStartHere.Name = "btnStartHere"
            Me.btnStartHere.Size = New System.Drawing.Size(40, 23)
            Me.btnStartHere.TabIndex = 9
            Me.btnStartHere.Text = "Here"
            Me.btnStartHere.UseVisualStyleBackColor = True
            '
            'btnStopHere
            '
            Me.btnStopHere.Location = New System.Drawing.Point(313, 45)
            Me.btnStopHere.Name = "btnStopHere"
            Me.btnStopHere.Size = New System.Drawing.Size(40, 23)
            Me.btnStopHere.TabIndex = 9
            Me.btnStopHere.Text = "Here"
            Me.btnStopHere.UseVisualStyleBackColor = True
            '
            'frmAutoRun
            '
            Me.AcceptButton = Me.btnStart
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoSize = True
            Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(417, 538)
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
            Me.grpIris.ResumeLayout(False)
            Me.grpIris.PerformLayout()
            Me.grpMag.ResumeLayout(False)
            Me.grpMag.PerformLayout()
            Me.grpObjective.ResumeLayout(False)
            Me.grpObjective.PerformLayout()
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
        Friend WithEvents rdoStepSizeCalculated As System.Windows.Forms.RadioButton
        Friend WithEvents rdoStepSizeManual As System.Windows.Forms.RadioButton
        Friend WithEvents nudOverlap As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblOverlap As System.Windows.Forms.Label
        Friend WithEvents grpObjective As System.Windows.Forms.GroupBox
        Friend WithEvents RadioButton5 As System.Windows.Forms.RadioButton
        Friend WithEvents RadioButton4 As System.Windows.Forms.RadioButton
        Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
        Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
        Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
        Friend WithEvents grpIris As System.Windows.Forms.GroupBox
        Friend WithEvents RadioButton11 As System.Windows.Forms.RadioButton
        Friend WithEvents RadioButton12 As System.Windows.Forms.RadioButton
        Friend WithEvents RadioButton13 As System.Windows.Forms.RadioButton
        Friend WithEvents RadioButton14 As System.Windows.Forms.RadioButton
        Friend WithEvents RadioButton15 As System.Windows.Forms.RadioButton
        Friend WithEvents grpMag As System.Windows.Forms.GroupBox
        Friend WithEvents RadioButton6 As System.Windows.Forms.RadioButton
        Friend WithEvents RadioButton7 As System.Windows.Forms.RadioButton
        Friend WithEvents RadioButton8 As System.Windows.Forms.RadioButton
        Friend WithEvents RadioButton9 As System.Windows.Forms.RadioButton
        Friend WithEvents RadioButton10 As System.Windows.Forms.RadioButton
        Friend WithEvents RadioButton16 As System.Windows.Forms.RadioButton
        Friend WithEvents RadioButton17 As System.Windows.Forms.RadioButton
        Friend WithEvents btnStopSave As System.Windows.Forms.Button
        Friend WithEvents btnStartSave As System.Windows.Forms.Button
        Friend WithEvents btnStopHere As System.Windows.Forms.Button
        Friend WithEvents btnStartHere As System.Windows.Forms.Button
    End Class

End Namespace
