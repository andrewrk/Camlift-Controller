Imports System.Drawing


Namespace CanonCamera

    Public Class frmLiveView

        Private m_cam As Camera
        Private m_ShowGrid As Boolean

        Public Sub New(ByVal cam As Camera)
            InitializeComponent() ' This call is required by the Windows Form Designer.

            m_ShowGrid = False
            m_cam = cam
            m_cam.StartLiveView(Me.picLiveView)
        End Sub


        Private Sub btnGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrid.Click
            m_ShowGrid = Not m_ShowGrid
            picLiveView.Refresh()
        End Sub

        Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
            Me.Close()
        End Sub

        Private Sub picLiveView_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles picLiveView.Paint
            'If m_PictureBox.Image Is Nothing Then Return
            'Dim state As CanonImageState = m_canonImageState 'copy ref
            'Dim g = e.Graphics
            'Dim imgRect = New Rectangle(New Point(0, 0), m_PictureBox.Image.Size)
            'Dim trans As Transform2D
            'Try
            '    trans = Transform2D.FromZoomScale(m_PictureBox.ClientRectangle, imgRect)
            'Catch ex As DivideByZeroException
            '    Return ' rectagles are too small. just forget drawing anything.
            'End Try
            'Dim paintRect As Rectangle = trans.Transform(imgRect)
            'If m_ShowGrid Then
            '    Using gridPen = New Pen(Color.Gray, 2)
            '        Dim x1 As Integer = paintRect.X + paintRect.Width / 3, x2 As Integer = paintRect.X + 2 * paintRect.Width / 3
            '        Dim y1 As Integer = paintRect.Y + paintRect.Height / 3, y2 As Integer = paintRect.Y + 2 * paintRect.Height / 3
            '        g.DrawLine(gridPen, x1, paintRect.Top, x1, paintRect.Bottom)
            '        g.DrawLine(gridPen, x2, paintRect.Top, x2, paintRect.Bottom)
            '        g.DrawLine(gridPen, paintRect.Left, y1, paintRect.Right, y1)
            '        g.DrawLine(gridPen, paintRect.Left, y2, paintRect.Right, y2)
            '    End Using
            'End If
            '' draw zoom
            'Dim zoomSize = New Size(paintRect.Width / 5, paintRect.Height / 5)
            'Dim zoomUl = trans.Transform(Transform2D.Scale(canonImg.ZoomPosition, imgRect.Size))
            'Dim zoomRect = New Rectangle(zoomUl, zoomSize)
            'Using shadowPen As Pen = New Pen(Color.Black, 1)
            '    Dim shadowRect As Rectangle = zoomRect
            '    shadowRect.Offset(1, 1)
            '    g.DrawRectangle(shadowPen, shadowRect)
            'End Using
            'Using whitePen As Pen = New Pen(Color.White, 2)
            '    g.DrawRectangle(whitePen, zoomRect)
            'End Using
        End Sub

        Private Sub frmLiveView_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            m_cam.StopLiveView()
        End Sub
    End Class
End Namespace
