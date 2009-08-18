Imports System.Windows.Forms
Imports VisionaryDigital.CamliftController

Public Class frmManageMemoryRegisters

    Private m_positionManager As PositionManager
    Private newReg As List(Of Integer?)
    Private IgnoreTextChange As Boolean

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        m_positionManager.m_memReg = newReg.ToArray()
        DialogResult = DialogResult.OK
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        DialogResult = DialogResult.Cancel
    End Sub

    Public Sub New(ByVal posMan As PositionManager)
        InitializeComponent() ' This call is required by the Windows Form Designer.

        m_positionManager = posMan

        newReg = m_positionManager.m_memReg.ToList()

        RefreshList()

        lstMem.SelectedIndex = 0
        IgnoreTextChange = False
    End Sub

    Private Sub RefreshList()
        lstMem.Items.Clear()

        For i As Integer = 0 To newReg.Count() - 1
            lstMem.Items.Add(ItemName(i))
        Next

        btnRemove.Enabled = newReg.Count > 0
    End Sub

    Private Sub lstMem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstMem.SelectedIndexChanged
        If lstMem.SelectedIndex < 0 OrElse lstMem.SelectedIndex > lstMem.SelectedIndex >= newReg.Count Then Exit Sub

        Dim item = newReg(lstMem.SelectedIndex)

        IgnoreTextChange = True
        If item Is Nothing Then
            txtValue.Text = ""
        Else
            txtValue.Text = item
        End If
        IgnoreTextChange = False

        txtValue.Focus()
    End Sub

    Private Sub txtValue_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtValue.TextChanged
        If IgnoreTextChange Then Exit Sub

        Try
            newReg(lstMem.SelectedIndex) = txtValue.Text
        Catch ex As Exception
            newReg(lstMem.SelectedIndex) = Nothing
        End Try

        lstMem.Items(lstMem.SelectedIndex) = ItemName(lstMem.SelectedIndex)
    End Sub

    Private Function ItemName(ByVal index As Integer) As String
        If newReg(index) Is Nothing Then
            Return (index + 1) & " - not set"
        Else
            Return (index + 1) & " - " & newReg(index)
        End If
    End Function

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        newReg.Add(Nothing)
        RefreshList()
        lstMem.SelectedIndex = lstMem.Items.Count - 1
    End Sub

    Private Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        If newReg.Count = 0 Then Exit Sub

        Dim index As Integer = lstMem.SelectedIndex
        newReg.RemoveAt(index)
        RefreshList()

        If index >= newReg.Count Then index = newReg.Count - 1
        lstMem.SelectedIndex = index
    End Sub
End Class
