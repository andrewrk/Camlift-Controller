Imports VisionaryDigital.SmartSteps
Imports VisionaryDigital.Settings

Public Class frmAutoRunSetups

    Private m_smartStepsManager As SmartStepsManager
    Private m_setups As List(Of KeyValuePair(Of String, AutorunSetupSettings))
    Private m_saveMode As Boolean
    Private m_selectedSelectedIndex As Integer = -1

    Public Sub New(ByVal smartStepsManager As SmartStepsManager, ByVal saveMode As Boolean)
        InitializeComponent() ' This call is required by the Windows Form Designer.

        m_smartStepsManager = smartStepsManager
        m_setups = smartStepsManager.AutorunSetups.AutorunSetups.ToList
        m_saveMode = saveMode
    End Sub

    Private Sub frmAutoRunSetups_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        updatedSelection()
    End Sub

    Private Sub lstSetups_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstSetups.SelectedIndexChanged
        m_selectedSelectedIndex = findNameInSetups(lstSetups.SelectedItem)
        updatedSelection()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        lstSetups.Items.RemoveAt(lstSetups.SelectedIndex)
        updatedSelection()
    End Sub

    Private Sub txtName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.GotFocus
        lstSetups.Sorted = False
    End Sub

    Private Sub txtName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.LostFocus
        lstSetups.Sorted = True
    End Sub

    Private Sub txtName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtName.TextChanged
        Dim currentEntry = m_setups(m_selectedSelectedIndex)
        m_setups(m_selectedSelectedIndex) = New KeyValuePair(Of String, AutorunSetupSettings)(txtName.Text, currentEntry.Value)
        fillList()
    End Sub

    Private Sub fillList()
        Dim duplicateName = False 'TODO: add duplicate name detection
        lstSetups.Enabled = Not duplicateName
        btnOk.Enabled = Not duplicateName
        If duplicateName Then Return

        lstSetups.Items.Clear()
        lstSetups.Items.AddRange(getNames())
        lstSetups.SelectedItem = m_setups(m_selectedSelectedIndex).Key
    End Sub
    Private Function findNameInSetups(ByVal name As String) As Integer
        Dim i = 0
        For Each kvp In m_setups
            If kvp.Key = name Then Return i
            i += 1
        Next
        Return -1
    End Function
    Private Function getNames() As String()
        Return (From kvp In m_setups Select kvp.Key).ToArray
    End Function

    Private Sub updatedSelection()
        If lstSetups.SelectedItems.Count = 0 Then
            btnDelete.Enabled = False
            txtName.Text = ""
            txtName.Enabled = False
        Else
            btnDelete.Enabled = True
            txtName.Text = lstSetups.SelectedItem
            txtName.Enabled = True
        End If
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        applySettings()
        DialogResult = DialogResult.OK
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
    End Sub

    Private Sub applySettings()
        m_smartStepsManager.AutorunSetups.AutorunSetups = m_setups.ToList
    End Sub
End Class