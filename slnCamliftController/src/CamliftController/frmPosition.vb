Imports System.Windows.Forms
Imports VisionaryDigital.Settings

Public Enum DialogType
    Load
    Save
End Enum


Public Class frmPosition

    Private m_mode As DialogType
    Private m_value As Integer
    Private m_settings As PositionManagerSettings
    Private m_newPositions As List(Of KeyValuePair(Of String, Integer))
    Private m_ignoreTextChange As Boolean

    Public SelectedPosition As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If m_mode = DialogType.Load Then
            SelectedPosition = m_newPositions(GetSelectedPosItem()).Value
        Else
            m_newPositions.Add(New KeyValuePair(Of String, Integer)(txtName.Text, m_value))
        End If
        m_settings.Positions.Positions = m_newPositions

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Function GetSelectedPosItem() As Integer
        If lvwPositions.SelectedItems.Count = 0 Then Return -1

        For i As Integer = 0 To m_newPositions.Count - 1
            If m_newPositions(i).Key = lvwPositions.SelectedItems(0).Text Then Return i
        Next

        Return -1 'not found
    End Function

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub New(ByVal settings As PositionManagerSettings, ByVal mode As DialogType, Optional ByVal value As Integer = 0)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        m_mode = mode
        m_settings = settings
        m_value = value

        m_newPositions = m_settings.Positions.Positions.ToList()

        m_ignoreTextChange = False
    End Sub

    Private Sub RefreshList()
        Dim ignore = m_ignoreTextChange
        m_ignoreTextChange = True

        lvwPositions.Items.Clear()
        For Each item As KeyValuePair(Of String, Integer) In m_newPositions
            Dim lvitem = lvwPositions.Items.Add(item.Key)
            lvitem.SubItems.Add(item.Value)
        Next

        ignore = m_ignoreTextChange

        ConfigureControls()
    End Sub

    Private Sub frmPosition_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If m_mode = DialogType.Load Then
            Me.Text = "Load Saved Position"
            Me.OK_Button.Text = "Load"
            Me.lblAs.Visible = False
            Me.txtName.Visible = False

            lvwPositions.Focus()
        Else
            Me.Text = "Save Position"
            Me.OK_Button.Text = "Save"
            Me.lblAs.Visible = True
            Me.txtName.Visible = True

            txtName.Text = "position title"
            txtName.SelectAll()
            txtName.Focus()
        End If

        RefreshList()


    End Sub

    Private Sub lvwPositions_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.LabelEditEventArgs) Handles lvwPositions.AfterLabelEdit
        Dim index = GetSelectedPosItem()

        If index = -1 Then Exit Sub
        Dim kvitem = m_newPositions(index)

        m_newPositions(index) = New KeyValuePair(Of String, Integer)(e.Label, kvitem.Value)
    End Sub

    Private Sub lvwPositions_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwPositions.DoubleClick
        OK_Button_Click(sender, e)
    End Sub

    Private Sub ConfigureControls()
        If lvwPositions.SelectedItems.Count = 0 Then
            btnRemove.Enabled = False
            btnRename.Enabled = False
        Else
            btnRemove.Enabled = True
            btnRename.Enabled = True
        End If
    End Sub

    Private Sub lvwPositions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwPositions.SelectedIndexChanged
        configurecontrols()
        If lvwPositions.SelectedItems.Count = 0 Then Exit Sub

        txtName.Text = lvwPositions.SelectedItems(0).Text
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Dim index = GetSelectedPosItem()

        If index = -1 Then Exit Sub

        m_newPositions.RemoveAt(index)
        RefreshList()
    End Sub

    Private Sub btnRename_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRename.Click
        If lvwPositions.SelectedItems.Count = 0 Then Exit Sub
        lvwPositions.SelectedItems(0).BeginEdit()
    End Sub
End Class
