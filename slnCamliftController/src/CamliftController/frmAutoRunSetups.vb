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

    Private Sub lstSetups_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstSetups.DoubleClick
        btnOk_Click(sender, e)
    End Sub

    Private Sub lstSetups_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstSetups.SelectedIndexChanged
        configureControls()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If lstSetups.SelectedIndex = -1 Then Exit Sub
        m_setups.RemoveAt(lstSetups.SelectedIndex)
        RefreshList()
    End Sub

    Private Sub txtName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.GotFocus
        lstSetups.Sorted = False
    End Sub

    Private Sub txtName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.LostFocus
        lstSetups.Sorted = True
    End Sub

    Private Sub RefreshList()
        Dim duplicateName = False 'TODO: add duplicate name detection
        lstSetups.Enabled = Not duplicateName
        btnOk.Enabled = Not duplicateName
        If duplicateName Then Return

        lstSetups.Items.Clear()
        lstSetups.Items.AddRange(getNames())

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
        If lstSetups.SelectedItems.Count = 0 Then
            btnDelete.Enabled = False
            txtName.Text = ""
            txtName.Enabled = False
            btnOk.Enabled = False
        Else
            btnDelete.Enabled = True
            txtName.Text = lstSetups.SelectedItem
            txtName.Enabled = True
            btnOk.Enabled = True
        End If
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If lstSetups.SelectedIndex = -1 Then Exit Sub

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

            lstSetups.Focus()
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
End Class