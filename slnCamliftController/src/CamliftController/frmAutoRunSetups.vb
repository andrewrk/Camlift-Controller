Imports VisionaryDigital.SmartSteps
Imports VisionaryDigital.Settings

Public Class frmAutoRunSetups

    Private m_smartStepsManager As SmartStepsManager
    Private m_setups As List(Of KeyValuePair(Of String, AutorunSetupSettings))
    Private m_mode As DialogType

    Public SelectedName As String

    Public Sub New(ByVal smartStepsManager As SmartStepsManager, ByVal mode As DialogType)
        InitializeComponent() ' This call is required by the Windows Form Designer.

        m_smartStepsManager = smartStepsManager
        m_setups = smartStepsManager.AutorunSetups.AutorunSetups.ToList()
        m_mode = mode
    End Sub

    Private Function GetSelectedIndex() As Integer
        If lvwSetups.SelectedItems.Count = 0 Then Return -1
        For i As Integer = 0 To m_setups.Count - 1
            If m_setups(i).Key = lvwSetups.SelectedItems(0).Text Then Return i
        Next
        Return -1
    End Function

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim index = GetSelectedIndex()
        If index = -1 Then Exit Sub
        m_setups.RemoveAt(index)
        RefreshList()

    End Sub

    Private Sub RefreshList()
        lvwSetups.Items.Clear()
        For i As Integer = 0 To m_setups.Count - 1
            lvwSetups.Items.Add(m_setups(i).Key)
        Next

        configureControls()
    End Sub
    Private Function findNameInSetups(ByVal name As String) As Integer
        For i As Integer = 0 To m_setups.Count - 1
            If m_setups(i).Key = name Then Return i
        Next
        Return -1 ' not found
    End Function
    Private Function getNames() As String()
        Return (From kvp In m_setups Select kvp.Key).ToArray()
    End Function

    Private Sub configureControls()
        If lvwSetups.SelectedItems.Count = 0 Then
            btnDelete.Enabled = False
            btnRename.Enabled = False
            btnOk.Enabled = txtName.Text.Length > 0
        Else
            btnDelete.Enabled = True
            btnRename.Enabled = True
            txtName.Text = lvwSetups.SelectedItems(0).Text
            txtName.Enabled = True
            btnOk.Enabled = True
        End If
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If m_mode = DialogType.Load AndAlso GetSelectedIndex() = -1 Then Exit Sub

        m_smartStepsManager.AutorunSetups.AutorunSetups = m_setups
        SelectedName = txtName.Text

        DialogResult = DialogResult.OK 'closes dialog
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel 'closes dialog
    End Sub

    Private Sub frmAutoRunSetups_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If m_mode = DialogType.Load Then
            Me.Text = "Load an Autorun Setup"
            btnOk.Text = "Load"
            lblAs.Visible = False
            txtName.Visible = False

            lvwSetups.Focus()
        Else
            Me.Text = "Save Autorun Setup"
            btnOk.Text = "Save"
            lblAs.Visible = True
            txtName.Visible = True

            txtName.Text = "setup name"
            txtName.SelectAll()
            txtName.Focus()
        End If

        RefreshList()

    End Sub

    Private Sub lvwSetups_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.LabelEditEventArgs) Handles lvwSetups.AfterLabelEdit
        Dim index = GetSelectedIndex()

        If index = -1 Then Exit Sub
        Dim kvitem = m_setups(index)

        m_setups(index) = New KeyValuePair(Of String, AutorunSetupSettings)(e.Label, kvitem.Value)

        txtName.Text = e.Label
    End Sub

    Private Sub lvwSetups_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwSetups.DoubleClick
        btnOk_Click(sender, e)
    End Sub

    Private Sub lvwSetups_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwSetups.SelectedIndexChanged
        configureControls()
    End Sub

    Private Sub btnRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRename.Click
        If lvwSetups.SelectedItems.Count = 0 Then Exit Sub
        lvwSetups.SelectedItems(0).BeginEdit()
    End Sub

    Private Sub txtName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.TextChanged
        configureControls()
    End Sub
End Class