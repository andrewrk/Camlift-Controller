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

        Private m_selectedObjective As KeyValuePair(Of String, ObjectiveSettings) = Nothing
        Private m_selectedMag As KeyValuePair(Of String, MagSettings) = Nothing
        Private m_selectedIris As KeyValuePair(Of String, Integer) = Nothing

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

            grpObjective.Enabled = m_currentSetup.CalculateStepSize
            grpMag.Enabled = m_currentSetup.CalculateStepSize
            grpIris.Enabled = m_currentSetup.CalculateStepSize

            lblOverlap.Enabled = m_currentSetup.CalculateStepSize
            nudOverlap.Enabled = m_currentSetup.CalculateStepSize

            loadStepSize(m_currentSetup)
        End Sub

        Private Sub rdoObjective_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim rdoObjective As RadioButton = sender
            Dim name = rdoObjective.Text
            m_selectedObjective = (From kvp In m_objecitveList.Objectives Where kvp.Key = name)(0)
            loadMags()
        End Sub
        Private Sub rdoMag_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim rdoMag As RadioButton = sender
            Dim name = rdoMag.Text
            m_selectedMag = (From kvp In m_selectedObjective.Value.Mags Where kvp.Key = name)(0)
            loadIrises()
        End Sub
        Private Sub rdoIris_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim rdoIris As RadioButton = sender
            Dim name = rdoIris.Text
            m_selectedIris = (From kvp In m_selectedMag.Value.Irises Where kvp.Key = name)(0)
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
            nudOverlap.Value = setup.SliceOverlap

            checkCorrectRadio(grpObjective, setup.Objective)
            checkcorrectradio(grpMag, setup.Mag)
            checkcorrectradio(grpIris, setup.Iris)
        End Sub
        Private Sub loadStepSize(ByVal setup As AutorunSetupSettings)
            If Not setup.CalculateStepSize Then
                txtStepSize.Text = setup.StepSize
            Else
                trySelectRadio(grpObjective, setup.Objective)
                trySelectRadio(grpMag, setup.Mag)
                trySelectRadio(grpIris, setup.Iris)
                'nudOverlap.Value = setup.Overlap
                loadCalculatedStepSize()
            End If
        End Sub

        Private Sub loadObjectives()
            fillGroupWithRadios(grpObjective, (From kvp In m_objecitveList.Objectives Select kvp.Key), AddressOf rdoObjective_CheckedChanged)
            grpMag.Controls.Clear()
            grpIris.Controls.Clear()
        End Sub
        Private Sub loadMags()
            Dim lastMagName = m_selectedMag.Key
            fillGroupWithRadios(grpMag, (From kvp In m_selectedObjective.Value.Mags Select kvp.Key), AddressOf rdoMag_CheckedChanged)
            trySelectRadio(grpMag, lastMagName)
        End Sub
        Private Sub loadIrises()
            Dim lastIrisName = m_selectedIris.Key
            fillGroupWithRadios(grpIris, (From kvp In m_selectedMag.Value.Irises Select kvp.Key), AddressOf rdoIris_CheckedChanged)
            trySelectRadio(grpIris, lastIrisName)
        End Sub
        Private Sub loadCalculatedStepSize()
            Dim stepSize As String = m_objecitveList.GetIris(m_selectedObjective.Key, m_selectedMag.Key, m_selectedIris.Key)
            If stepSize <> "" Then
                stepSize = Int(stepSize * (1 - nudOverlap.Value / 100))
            End If
            txtStepSize.Text = stepSize
        End Sub
        Private Shared ReadOnly firstLocation As New Point(6, 16)
        Private Const vertDelta As Integer = 23
        Private Sub fillGroupWithRadios(ByVal grp As GroupBox, ByVal names As IEnumerable(Of String), ByVal eventHandler As EventHandler)
            grp.Controls.Clear()
            Dim loc = firstLocation
            For Each name As String In names
                Dim rdo = New RadioButton()
                rdo.Location = loc
                loc.Y += vertDelta
                rdo.Text = name
                AddHandler rdo.CheckedChanged, eventHandler
                grp.Controls.Add(rdo)
            Next
        End Sub

        Private Function trySelectRadio(ByVal grp As GroupBox, ByVal name As String) As Boolean
            Dim results = From rdo As RadioButton In grp.Controls Where rdo.Text = name
            If results.Count > 0 Then
                results(0).Checked = True
                Return True
            ElseIf grp.Controls.Count > 0 Then
                CType(grp.Controls(0), RadioButton).Checked = True
            End If
            Return False
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
                m_currentSetup.Objective = GetSelectedRadioValue(grpObjective)
                m_currentSetup.Mag = GetSelectedRadioValue(grpMag)
                m_currentSetup.Iris = GetSelectedRadioValue(grpIris)
            End If
            m_currentSetup.SliceOverlap = nudOverlap.Value
            m_currentSetup.Dwell = getValidString(txtDwell)
        End Sub

        Private Function GetSelectedRadioValue(ByVal group As GroupBox) As String
            For Each btn As RadioButton In group.Controls
                If btn.Checked Then Return btn.Text
            Next
            Return ""
        End Function

        Private Sub CheckCorrectRadio(ByVal group As GroupBox, ByVal text As String)
            For Each btn As RadioButton In group.Controls
                If btn.Text = text Then
                    btn.Checked = True
                    Exit For
                End If
            Next
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
                    txtStopPosition.Text = m_currentRun.AutorunStart + stepSize * (m_currentRun.Slices - 1)
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
            Dim setups As New frmAutoRunSetups(m_smartStepsManager, DialogType.Save)

            If setups.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Dim newkvp As New KeyValuePair(Of String, AutorunSetupSettings)(setups.SelectedName, New AutorunSetupSettings(m_currentSetup.GetContents))
                Dim found As Boolean = False
                For i As Integer = 0 To m_smartStepsManager.AutorunSetups.AutorunSetups.Count - 1
                    If m_smartStepsManager.AutorunSetups.AutorunSetups(i).Key = setups.SelectedName Then
                        m_smartStepsManager.AutorunSetups.AutorunSetups(i) = newkvp
                        found = True
                        Exit For
                    End If
                Next

                'not found
                If Not found Then m_smartStepsManager.AutorunSetups.AutorunSetups.Add(newkvp)

                m_smartStepsManager.SaveSettings()
            End If

        End Sub

        Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
            If m_smartStepsManager.AutorunSetups.AutorunSetups.Count = 0 Then
                MsgBox(MsgBoxNoSavedAutorunSetups, MsgBoxStyle.OkOnly)
                Exit Sub
            End If

            Dim setups As New frmAutoRunSetups(m_smartStepsManager, DialogType.Load)

            If setups.ShowDialog() = Windows.Forms.DialogResult.OK Then


                For Each kvp In m_smartStepsManager.AutorunSetups.AutorunSetups
                    If kvp.Key = setups.SelectedName Then
                        loadSetup(New AutorunSetupSettings(kvp.Value.GetContents))
                        Exit Sub
                    End If
                Next
                'not found
                Debug.Assert(False)
            End If
        End Sub

    End Class
End Namespace
