Namespace CamliftController

    Public Class frmLocations

        Private m_parentForm As frmControls

        Private Sub New(ByVal parentForm As frmControls)
            InitializeComponent() ' This call is required by the Windows Form Designer.

            m_parentForm = parentForm
        End Sub

#If False Then
        Private Sub frmLocations_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            refreshList()
        End Sub

        Private Sub lsvLocations_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lsvLocations.SelectedIndexChanged
            Dim b As Boolean = (lsvLocations.SelectedIndices.Count > 0)
            btnLoad.Enabled = b
            btnDelete.Enabled = b
        End Sub
        Private Sub lsvLocations_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lsvLocations.MouseDoubleClick
            If lsvLocations.SelectedIndices.Count > 0 Then
                loadItem()
            End If
        End Sub

        Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
            loadItem()
        End Sub
        Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
            deleteItem()
        End Sub


        Private Sub refreshList()
            lsvLocations.Items.Clear()
            For Each sKey As String In m_parentForm.Locations.Keys
                lsvLocations.Items.Add(New ListViewItem(New String() {sKey, m_parentForm.Locations(sKey)}))
            Next
            lsvLocations.Refresh()
        End Sub

        Private Sub loadItem()
            Me.Tag = CType(lsvLocations.SelectedItems(0).SubItems(1).Text, Integer)
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End Sub
        Private Sub deleteItem()
            If MsgBox(MsgBoxConfirmDeleteMessage, MsgBoxStyle.YesNo, MsgBoxTitle) <> MsgBoxResult.Yes Then Exit Sub
            Dim lsvItem As ListViewItem = lsvLocations.SelectedItems(0)
            m_parentForm.Locations.Remove(lsvItem.SubItems(0).Text)
            lsvLocations.Items.Remove(lsvItem)
        End Sub
#End If
    End Class

End Namespace
