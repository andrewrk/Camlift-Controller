Namespace CamliftController

    Public Class frmPreferences

        Private m_steps As IEnumerable(Of KeyValuePair(Of String, Integer))
        Private m_f_updateSteps As Action(Of IEnumerable(Of KeyValuePair(Of String, Integer)))

        Public Sub New(ByVal steps As IEnumerable(Of KeyValuePair(Of String, Integer)), ByVal f_updateSteps As Action(Of IEnumerable(Of KeyValuePair(Of String, Integer))))
            InitializeComponent() ' This call is required by the Windows Form Designer.

            If steps.Count <> StepsCount Then Throw New ArgumentException()
            m_steps = steps
            m_f_updateSteps = f_updateSteps
        End Sub
        Private Sub frmPreferences_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim i = 1
            For Each kvp In m_steps
                If i <= LabeledStepsCount Then
                    Dim txtLabel As TextBox = grpStepSizes.Controls("txtLabel" & i)
                    txtLabel.Text = kvp.Key
                    AddHandler txtLabel.TextChanged, AddressOf txt_TextChanged

                    Dim lblLabelDefault As Label = grpStepSizes.Controls("lblLabelDefault" & i)
                    lblLabelDefault.Text = frmControls.DefaultStepLabels(i - 1)

                    Dim txtStep As TextBox = grpStepSizes.Controls("txtStep" & i)
                    txtStep.Text = kvp.Value
                    AddHandler txtStep.TextChanged, AddressOf txt_TextChanged

                    Dim lblDefault As Label = grpStepSizes.Controls("lblDefault" & i)
                    lblDefault.Text = frmControls.DefaultStepSizes(i - 1)
                Else
                    Dim txtStep As TextBox = grpStepSizes.Controls("txtStep" & i)
                    AddHandler txtStep.TextChanged, AddressOf txtStep7_9_TextChanged
                    txtStep.Text = kvp.Value
                    AddHandler txtStep.TextChanged, AddressOf txt_TextChanged

                    Dim lblDefault As Label = grpStepSizes.Controls("lblDefault" & i)
                    lblDefault.Text = frmControls.DefaultStepSizes(i - 1)
                End If

                i += 1
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
            Dim stepSizes As New List(Of KeyValuePair(Of String, Integer))
            For i As Integer = 1 To StepsCount
                Dim name As String = Nothing
                If i <= LabeledStepsCount Then
                    Dim txtLabel As TextBox = grpStepSizes.Controls("txtLabel" & i)
                    name = txtLabel.Text
                End If
                Dim txtStep As TextBox = grpStepSizes.Controls("txtStep" & i)
                If Not ValidateRange(txtStep, 1, Silverpak23CE.SilverpakManager.DefaultMaxPosition) Then Return False

                stepSizes.Add(New KeyValuePair(Of String, Integer)(name, txtStep.Text))
            Next
            m_f_updateSteps(stepSizes)
            Return True
        End Function
    End Class

End Namespace
