Namespace CamliftController

    Public Class frmPreferences

        Private m_steps As List(Of KeyValuePair(Of String, Integer))
        Private m_f_updateSteps As Action

        Public Sub New(ByVal steps As List(Of KeyValuePair(Of String, Integer)), ByVal f_updateSteps As Action)
            InitializeComponent() ' This call is required by the Windows Form Designer.

            If steps.Count <> StepsCount Then Throw New ArgumentException()
            m_steps = steps
            m_f_updateSteps = f_updateSteps
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
                    AddHandler txtStep.TextChanged, AddressOf txtStep7_9_TextChanged
                    txtStep.Text = m_steps(i).Value
                    AddHandler txtStep.TextChanged, AddressOf txt_TextChanged

                    Dim lblDefault As Label = grpStepSizes.Controls("lblDefault" & i + 1)
                    lblDefault.Text = defaultStepSizes(i).Value
                End If
            Next
        End Sub

        Private Sub txt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            btnApply.Enabled = True
            Me.AcceptButton = btnApply
        End Sub

        Private Sub txtStep7_9_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim txtStep As TextBox = sender
            Dim lblLabel As Label = grpStepSizes.Controls("lblLabel" & txtStep.Name.Substring("txtStep".Length))
            If IsNumeric(txtStep.Text) AndAlso (txtStep.Text >= 1 And txtStep.Text <= Silverpak23CE.SilverpakManager.DefaultMaxPosition) Then
                lblLabel.Text = MicrostepsToMilimeters(txtStep.Text) & "mm"
            Else
                lblLabel.Text = ""
            End If
        End Sub


        Private Sub btnRestoreDefaults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestoreDefaults.Click
            For i As Integer = 1 To 6
                Dim txtLabel As TextBox = grpStepSizes.Controls("txtLabel" & i)
                Dim lblLabelDefault As Label = grpStepSizes.Controls("lblLabelDefault" & i)
                txtLabel.Text = lblLabelDefault.Text

                Dim txtStep As TextBox = grpStepSizes.Controls("txtStep" & i)
                Dim lblDefault As Label = grpStepSizes.Controls("lblDefault" & i)
                txtStep.Text = lblDefault.Text
            Next

            For i As Integer = 7 To 9
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
    End Class

End Namespace
