Imports VisionaryDigital.Settings

Namespace CamliftController

    Public Class frmPreferences

        Private m_steps As List(Of KeyValuePair(Of String, Integer))
        Private m_f_updateSteps As Action
        Private m_stepSizeSettings As List(Of KeyValuePair(Of String, StepSizeList))

        Public Sub New(ByRef stepSizeSettings As StepSizeListList, ByVal steps As List(Of KeyValuePair(Of String, Integer)), ByVal f_updateSteps As Action)
            InitializeComponent() ' This call is required by the Windows Form Designer.

            If steps.Count <> StepsCount Then Throw New ArgumentException()
            m_steps = steps
            m_f_updateSteps = f_updateSteps
            m_stepSizeSettings = stepSizeSettings.StepSizeSetups

            RefreshProfiles()
        End Sub

        Private Function GetSelectedIndex() As Integer
            If cboProfiles.SelectedIndex = -1 Then Return -1
            For i As Integer = 0 To m_stepSizeSettings.Count - 1
                If m_stepSizeSettings(i).Key = cboProfiles.Text Then Return i
            Next
            Return -1
        End Function

        Private Sub RefreshProfiles()
            cboProfiles.Items.Clear()
            For i As Integer = 0 To m_stepSizeSettings.Count - 1
                cboProfiles.Items.Add(m_stepSizeSettings(i).Key)
            Next

            configureControls()
        End Sub

        Private Sub configureControls()
            btnLoadProfile.Enabled = findNameInSetups(cboProfiles.Text) >= 0
            btnSaveProfile.Enabled = Len(cboProfiles.Text) > 0
            btnDeleteProfile.Enabled = btnLoadProfile.Enabled
        End Sub

        Private Sub frmPreferences_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim defaultStepSizes = Settings.StepSizeList.DefaultStepSizes
            For i = 0 To m_steps.Count - 1
                If i < LabeledStepsCount Then
                    Dim txtLabel As TextBox = grpStepSizes.Controls("txtLabel" & i + 1)
                    txtLabel.Text = m_steps(i).Key
                    AddHandler txtLabel.TextChanged, AddressOf txt_TextChanged

                    Dim lblLabelDefault As Label = grpStepSizes.Controls("lblLabelDefault" & i + 1)
                    lblLabelDefault.Text = defaultStepSizes(i).Key

                    Dim txtStep As TextBox = grpStepSizes.Controls("txtStep" & i + 1)
                    txtStep.Text = m_steps(i).Value
                    AddHandler txtStep.TextChanged, AddressOf txt_TextChanged

                    Dim lblDefault As Label = grpStepSizes.Controls("lblDefault" & i + 1)
                    lblDefault.Text = defaultStepSizes(i).Value
                Else
                    Dim txtStep As TextBox = grpStepSizes.Controls("txtStep" & i + 1)
                    AddHandler txtStep.TextChanged, AddressOf txtStep8_9_TextChanged
                    txtStep.Text = m_steps(i).Value
                    AddHandler txtStep.TextChanged, AddressOf txt_TextChanged

                    Dim lblDefault As Label = grpStepSizes.Controls("lblDefault" & i + 1)
                    lblDefault.Text = defaultStepSizes(i).Value
                End If
            Next
        End Sub

        Private Sub PopulateControlsFromSteps()
            For i = 0 To m_steps.Count - 1
                If i < LabeledStepsCount Then
                    Dim txtLabel As TextBox = grpStepSizes.Controls("txtLabel" & i + 1)
                    txtLabel.Text = m_steps(i).Key

                    Dim lblLabelDefault As Label = grpStepSizes.Controls("lblLabelDefault" & i + 1)

                    Dim txtStep As TextBox = grpStepSizes.Controls("txtStep" & i + 1)
                    txtStep.Text = m_steps(i).Value

                Else
                    Dim txtStep As TextBox = grpStepSizes.Controls("txtStep" & i + 1)
                    txtStep.Text = m_steps(i).Value

                End If
            Next

        End Sub

        Private Sub txt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            btnApply.Enabled = True
            Me.AcceptButton = btnApply
        End Sub

        Private Sub txtStep8_9_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim txtStep As TextBox = sender
            Dim lblLabel As Label = grpStepSizes.Controls("lblLabel" & txtStep.Name.Substring("txtStep".Length))
            If IsNumeric(txtStep.Text) AndAlso (txtStep.Text >= 1 And txtStep.Text <= Silverpak23CE.SilverpakManager.DefaultMaxPosition) Then
                lblLabel.Text = MicrostepsToMilimeters(txtStep.Text) & "mm"
            Else
                lblLabel.Text = ""
            End If
        End Sub


        Private Sub btnRestoreDefaults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestoreDefaults.Click
            For i As Integer = 1 To 7
                Dim txtLabel As TextBox = grpStepSizes.Controls("txtLabel" & i)
                Dim lblLabelDefault As Label = grpStepSizes.Controls("lblLabelDefault" & i)
                txtLabel.Text = lblLabelDefault.Text

                Dim txtStep As TextBox = grpStepSizes.Controls("txtStep" & i)
                Dim lblDefault As Label = grpStepSizes.Controls("lblDefault" & i)
                txtStep.Text = lblDefault.Text
            Next

            For i As Integer = 8 To 9
                Dim txtStep As TextBox = grpStepSizes.Controls("txtStep" & i)
                Dim lblDefault As Label = grpStepSizes.Controls("lblDefault" & i)
                txtStep.Text = lblDefault.Text
            Next
        End Sub

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
            If ApplyChanges() Then
                Me.Close()
            End If
        End Sub
        Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Me.Close()
        End Sub
        Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
            If ApplyChanges() Then
                Me.AcceptButton = btnOk
                btnApply.Enabled = False
            End If
        End Sub

        Private Function ApplyChanges() As Boolean
            For i As Integer = 0 To StepsCount - 1
                Dim name As String = Nothing
                If i < LabeledStepsCount Then
                    Dim txtLabel As TextBox = grpStepSizes.Controls("txtLabel" & i + 1)
                    name = txtLabel.Text
                End If
                Dim txtStep As TextBox = grpStepSizes.Controls("txtStep" & i + 1)
                If Not ValidateRange(txtStep, 1, Silverpak23CE.SilverpakManager.DefaultMaxPosition) Then Return False

                m_steps(i) = New KeyValuePair(Of String, Integer)(name, txtStep.Text)
            Next
            m_f_updateSteps()
            Return True
        End Function

        Private Sub btnSaveProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveProfile.Click
            Dim index = findNameInSetups(cboProfiles.Text)

            ApplyChanges()
            If index = -1 Then
                'must be created
                m_stepSizeSettings.Add(New KeyValuePair(Of String, StepSizeList)(cboProfiles.Text, New StepSizeList(Nothing)))
                index = findNameInSetups(cboProfiles.Text)
                Debug.Assert(index <> -1)
            End If

            m_stepSizeSettings(index).Value.StepSizes = m_steps.ToList
            

            RefreshProfiles()
            configureControls()
        End Sub

        Private Sub btnLoadProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadProfile.Click
            Dim index = findNameInSetups(cboProfiles.Text)

            Debug.Assert(index <> -1)

            For i As Integer = 0 To StepsCount - 1
                m_steps(i) = m_stepSizeSettings(index).Value.StepSizes(i)
            Next

            PopulateControlsFromSteps()
        End Sub

        Private Sub cboProfiles_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboProfiles.TextChanged
            configureControls()
        End Sub

        Private Function findNameInSetups(ByVal name As String) As Integer
            For i As Integer = 0 To m_stepSizeSettings.Count - 1
                If m_stepSizeSettings(i).Key = name Then Return i
            Next
            Return -1 ' not found
        End Function

        Private Sub btnDeleteProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteProfile.Click
            Dim index As Integer = findNameInSetups(cboProfiles.Text)
            If index = -1 Then Exit Sub
            m_stepSizeSettings.RemoveAt(index)
            cboProfiles.Text = ""
            RefreshProfiles()
            configureControls()
        End Sub
    End Class

End Namespace
