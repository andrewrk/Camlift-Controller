Imports System.Drawing
Imports VisionaryDigital.CamliftController
Imports VisionaryDigital.Settings

Namespace SmartSteps

    Public Class frmAutoRun

        Private m_smartStepsManager As SmartStepsManager
        Private m_positionManager As PositionManager

        Private m_objecitveList As ObjectiveListSettings

        Private m_currentRun As AutorunRunSettings
        Private m_currentSetup As AutorunSetupSettings
        Private m_currentReturnToTop As Boolean

        Public Sub New(ByVal smartStepsManager As SmartStepsManager, ByVal positionManager As PositionManager, ByVal objectiveList As ObjectiveListSettings)
            InitializeComponent() ' This call is required by the Windows Form Designer.

            m_smartStepsManager = smartStepsManager
            m_positionManager = positionManager

            m_objecitveList = objectiveList

            m_currentRun = New AutorunRunSettings(m_smartStepsManager.LastAutorunRun.GetContents)
            m_currentSetup = New AutorunSetupSettings(m_smartStepsManager.LastAutorunSetup.GetContents)
            m_currentReturnToTop = m_smartStepsManager.ReturnToTop
        End Sub

        Private Sub frmAutoRun_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            For Each txt In New TextBox() {txtStart, txtStopPosition, txtStopPosition, txtSlices, txtStepSize, txtDwell}
                AddHandler txt.TextChanged, AddressOf txt_TextChanged
            Next
            loadObjectives()

            Dim currentRun = New AutorunRunSettings(m_currentRun.GetContents)
            Dim currentSetup = New AutorunSetupSettings(m_currentSetup.GetContents)

            loadRun(currentRun)
            loadSetup(currentSetup)
            chkReturnToTop.Checked = m_currentReturnToTop
        End Sub

        Private Sub txt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim txt As TextBox = sender
            If txt.Enabled Or txt Is txtStepSize Then validateForm()
        End Sub

        Private Sub rdoStop_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdoStopPosition.CheckedChanged, rdoSlices.CheckedChanged
            Dim rdoStop As RadioButton = sender
            If Not rdoStop.Checked Then Exit Sub
            m_currentRun.UseStopPosition = (sender Is rdoStopPosition)
            txtStopPosition.Enabled = m_currentRun.UseStopPosition
            btnStopPositionLoad.Enabled = m_currentRun.UseStopPosition
            txtSlices.Enabled = Not m_currentRun.UseStopPosition
            loadStop(m_currentRun)
        End Sub

        Private Sub btnStartLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartLoad.Click
            Dim cms = m_positionManager.MakeLoadMenu(AddressOf setStart)
            cms.Show(btnStartLoad, btnStartLoad.Width, 0)
        End Sub
        Private Sub setStart(ByVal value As Integer)
            txtStart.Text = value
        End Sub
        Private Sub btnStopPositionLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStopPositionLoad.Click
            Dim cms = m_positionManager.MakeLoadMenu(AddressOf setStop)
            cms.Show(btnStopPositionLoad, btnStopPositionLoad.Width, 0)
        End Sub
        Private Sub setStop(ByVal value As Integer)
            txtStopPosition.Text = value
        End Sub

        Private Sub rdoStepSize_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoStepSizeManual.CheckedChanged, rdoStepSizeCalculated.CheckedChanged
            Dim rdoStepSize As RadioButton = sender
            If Not rdoStepSize.Checked Then Exit Sub 'only update for becoming checked, not leaving being checked
            m_currentSetup.CalculateStepSize = (rdoStepSize Is rdoStepSizeCalculated)

            txtStepSize.Enabled = Not m_currentSetup.CalculateStepSize

            lblObjective.Enabled = m_currentSetup.CalculateStepSize
            cboObjective.Enabled = m_currentSetup.CalculateStepSize
            lblMag.Enabled = m_currentSetup.CalculateStepSize
            cboMag.Enabled = m_currentSetup.CalculateStepSize
            lblIris.Enabled = m_currentSetup.CalculateStepSize
            cboIris.Enabled = m_currentSetup.CalculateStepSize
            lblOverlap.Enabled = m_currentSetup.CalculateStepSize
            nudOverlap.Enabled = m_currentSetup.CalculateStepSize

            loadStepSize(m_currentSetup)
        End Sub

        Private Sub cboObjective_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboObjective.SelectedIndexChanged
            Dim lastMagName = cboMag.Text
            loadMags()
            trySetCombo(cboMag, lastMagName)
        End Sub
        Private Sub cboMag_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMag.SelectedIndexChanged
            Dim lastIrisName = cboIris.Text
            loadIrises()
            trySetCombo(cboIris, lastIrisName)
        End Sub
        Private Sub cboIris_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboIris.SelectedIndexChanged
            loadCalculatedStepSize()
        End Sub
        Private Sub nudOverlap_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudOverlap.ValueChanged
            loadCalculatedStepSize()
        End Sub

        Private Sub chkReturnToTop_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkReturnToTop.CheckedChanged
            m_currentReturnToTop = chkReturnToTop.Checked
        End Sub

        Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
            setAutorun()
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End Sub

        Private Sub loadRun(ByVal run As AutorunRunSettings)
            txtStart.Text = m_currentRun.AutorunStart
            If run.UseStopPosition Then
                rdoStopPosition.Checked = True
            Else
                rdoSlices.Checked = True
            End If
            loadStop(run)
        End Sub
        Private Sub loadStop(ByVal run As AutorunRunSettings)
            If run.UseStopPosition Then
                txtStopPosition.Text = run.AutorunStop
            Else
                txtSlices.Text = run.Slices
            End If
        End Sub
        Private Sub loadSetup(ByVal setup As AutorunSetupSettings)
            If Not setup.CalculateStepSize Then
                rdoStepSizeManual.Checked = True
            Else
                rdoStepSizeCalculated.Checked = True
            End If
            loadStepSize(setup)
            txtDwell.Text = setup.Dwell
        End Sub
        Private Sub loadStepSize(ByVal setup As AutorunSetupSettings)
            If Not setup.CalculateStepSize Then
                txtStepSize.Text = setup.StepSize
            Else
                trySetCombo(cboObjective, setup.Objective)
                trySetCombo(cboMag, setup.Mag)
                trySetCombo(cboIris, setup.Iris)
                'nudOverlap.Value = setup.Overlap
                loadCalculatedStepSize()
            End If
        End Sub

        Private Sub loadObjectives()
            cboMag.Items.Clear()
            For Each kvp In m_objecitveList.Objectives
                cboObjective.Items.Add(kvp.Key)
            Next
        End Sub
        Private Sub loadMags()
            cboMag.Items.Clear()
            Dim objective = m_objecitveList.GetObjective(cboObjective.Text)
            If objective Is Nothing Then Return
            For Each kvp In objective.Mags
                cboMag.Items.Add(kvp.Key)
            Next
        End Sub
        Private Sub loadIrises()
            cboIris.Items.Clear()
            Dim mag = m_objecitveList.GetMag(cboObjective.Text, cboMag.Text)
            If mag Is Nothing Then Return
            For Each kvp In mag.Irises
                cboIris.Items.Add(kvp.Key)
            Next
        End Sub
        Private Sub loadCalculatedStepSize()
            Dim stepSize As String = m_objecitveList.GetIris(cboObjective.Text, cboMag.Text, cboIris.Text)
            If stepSize <> "" Then
                stepSize = Int(stepSize * (1 - nudOverlap.Value / 100))
            End If
            txtStepSize.Text = stepSize
        End Sub

        Private Function trySetCombo(ByVal cbo As ComboBox, ByVal name As String) As Boolean
            For Each iName In cbo.Items
                If iName = name Then
                    cbo.Text = iName
                    Return True
                End If
            Next
            'select the first one
            If cbo.Items.Count > 0 Then
                cbo.Text = cbo.Items.Item(0)
                Return True
            Else
                'no items
                Return False
            End If
        End Function

        Private Sub validateForm()
            storeCurrentRun()
            storeCurrentSetup()
            validateRun()
        End Sub
        Private Sub storeCurrentRun()
            m_currentRun.AutorunStart = getValidString(txtStart)
            m_currentRun.UseStopPosition = rdoStopPosition.Checked
            If m_currentRun.UseStopPosition Then
                m_currentRun.AutorunStop = getValidString(txtStopPosition)
            Else
                m_currentRun.Slices = getValidString(txtSlices)
            End If
        End Sub
        Private Sub storeCurrentSetup()
            m_currentSetup.CalculateStepSize = rdoStepSizeCalculated.Checked
            If Not m_currentSetup.CalculateStepSize Then
                m_currentSetup.StepSize = getValidString(txtStepSize)
            Else
                m_currentSetup.Objective = cboObjective.Text
                m_currentSetup.Mag = cboMag.Text
                m_currentSetup.Iris = cboIris.Text
                'm_currentSetup.StepSize = getCurrentIris()
            End If
            m_currentSetup.Dwell = getValidString(txtDwell)
        End Sub

        Private Sub validateRun()
            Dim valid = True ' assume success

            'locations
            If (m_currentRun.IsValid AndAlso _
                If(Not m_currentSetup.CalculateStepSize, _
                   m_currentSetup.HasValidStepSize, _
                   m_currentSetup.HasValidStepSize(m_objecitveList))) Then
                Dim stepSize As Integer = If(Not m_currentSetup.CalculateStepSize, m_currentSetup.StepSize, m_currentSetup.GetStepSize(m_objecitveList))
                If m_currentRun.UseStopPosition Then ' use stop position
                    'show slices
                    txtSlices.Text = Math.Ceiling((m_currentRun.AutorunStop - m_currentRun.AutorunStart) / stepSize) + 1
                Else ' use slices
                    'show stop position
                    txtStopPosition.Text = m_currentRun.AutorunStart + stepSize * m_currentRun.Slices
                End If
            Else
                'invalid
                If m_currentRun.UseStopPosition Then ' use stop position
                    txtSlices.Text = ""
                Else ' use slices
                    txtStopPosition.Text = ""
                End If
                valid = False
            End If

            'dwell
            valid = valid AndAlso m_currentSetup.HasValidDwell

            btnStart.Enabled = valid
        End Sub

        Private Function getValidString(ByVal txt As TextBox) As String
            Dim valid = isNaturalNumber(txt.Text)
            colorTextBoxValid(txt, valid)
            Return If(valid, txt.Text, "")
        End Function
        Private Function isNaturalNumber(ByVal value As String) As Boolean
            Return (value <> "" AndAlso _
                    IsNumeric(value) AndAlso _
                    0 <= value AndAlso _
                    Not value.Contains("."))
        End Function
        Private Sub colorTextBoxValid(ByVal txt As TextBox, ByVal valid As Boolean)
            'TODO
            'Dim isValidColor As Boolean = valid OrElse txt.Text = "" ' don't color empty boxes red. they're obviously invalid
            'Dim color As Color = If(isValidColor, color.White, color.Tomato)
            ''If txt.Enabled Then
            ''    color = If(isValidColor, color.White, color.Tomato)
            ''Else
            ''    color = If(isValidColor, color.White, color.Tomato)
            ''    Const offset = 25
            ''    color = color.FromArgb(color.R - offset, color.G - offset, color.B - offset)
            ''End If
            'txt.BackColor = color
        End Sub

        Private Sub setAutorun()

            m_smartStepsManager.LastAutorunRun = m_currentRun
            m_smartStepsManager.LastAutorunSetup = m_currentSetup
            m_smartStepsManager.ReturnToTop = m_currentReturnToTop
            m_smartStepsManager.SaveSettings()

            Dim locations = getLocations()
            Me.Tag = m_smartStepsManager.GetAutorunStepper(locations, m_currentSetup.Dwell)
        End Sub
        Private Function getLocations() As IEnumerable(Of Integer)
            Dim stepSize As Integer = If(Not m_currentSetup.CalculateStepSize, m_currentSetup.StepSize, m_currentSetup.GetStepSize(m_objecitveList))
            Dim count As Integer
            If m_currentRun.UseStopPosition Then
                count = Math.Ceiling((m_currentRun.AutorunStop - m_currentRun.AutorunStart) / stepSize) ' number of moves after the first position
                count += 1 ' plus the first location = number of slices
            Else
                count = m_currentRun.Slices
            End If
            Dim locations(count - 1) As Integer
            For i = 0 To count - 1
                locations(i) = m_currentRun.AutorunStart + i * stepSize
            Next
            Return locations
        End Function

        Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Dim name = InputBox("Enter name (this will get fancier...):", MsgBoxTitle)
            If name = "" Then Exit Sub
            m_smartStepsManager.AutorunSetups.AutorunSetups.Add(New KeyValuePair(Of String, AutorunSetupSettings)(name, New AutorunSetupSettings(m_currentSetup.GetContents)))
            m_smartStepsManager.SaveSettings()
        End Sub

        Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
            Dim allNames = ""
            For Each kvp In m_smartStepsManager.AutorunSetups.AutorunSetups
                allNames &= """" & kvp.Key & """, "
            Next
            allNames = If(allNames = "", "(none)", allNames.Substring(0, allNames.Length - 2))
            Dim name = InputBox("Enter name (this will get fancier...):" & vbCrLf & "available names: " & allNames, MsgBoxTitle)
            If name = "" Then Exit Sub
            For Each kvp In m_smartStepsManager.AutorunSetups.AutorunSetups
                If kvp.Key = name Then
                    loadSetup(New AutorunSetupSettings(kvp.Value.GetContents))
                    Exit Sub
                End If
            Next
            'not found
            MsgBox("name not found. (this will get fancier...)", , MsgBoxTitle)
        End Sub

    End Class

End Namespace
