Imports System.Drawing


Namespace CanonCamera

    Public Class frmLiveView

        Private m_cam As Camera

        Friend m_liveViewViewer As LiveViewViewer

        Public Sub New(ByVal cam As Camera)
            InitializeComponent() ' This call is required by the Windows Form Designer.

            m_liveViewViewer = New LiveViewViewer(cam)
            Dim pct = m_liveViewViewer.PictureBox
            pct.Dock = DockStyle.Fill
            TableLayoutPanel2.Controls.Add(pct)
        End Sub

        Private Sub frmLiveView_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
            m_liveViewViewer.Dispose()
        End Sub

        Private Sub btnGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrid.Click
            m_liveViewViewer.ShowGrid = Not m_liveViewViewer.ShowGrid
        End Sub

        Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
            Me.Close()
        End Sub
    End Class

    Friend Class LiveViewViewer
        Implements IDisposable

        Private m_session As Camera.Session
        Private WithEvents m_liveView As LiveView

        Private WithEvents m_PictureBox As PictureBox
        Public ReadOnly Property PictureBox() As PictureBox
            Get
                Return m_PictureBox
            End Get
        End Property

        Private m_ShowGrid As Boolean = False
        Public Property ShowGrid() As Boolean
            Get
                Return m_ShowGrid
            End Get
            Set(ByVal value As Boolean)
                m_ShowGrid = value
                m_PictureBox.Invalidate()
            End Set
        End Property

        Friend Sub New(ByVal cam As Camera)
            m_session = cam.BeginSession
            m_liveView = m_session.BeginLiveView

            m_PictureBox = New PictureBox
            m_PictureBox.SizeMode = PictureBoxSizeMode.Zoom
        End Sub

        Private Sub setImage(ByVal canonImg As EvfImageInfo)
            Dim oldImg = m_PictureBox.Image
            m_PictureBox.Image = canonImg.Image
            If oldImg IsNot Nothing Then oldImg.Dispose() ' really is required.
            'update state
            m_canonImageState = New CanonImageState(canonImg, m_PictureBox.ClientRectangle, canonImg.Image.Size)
        End Sub
        Private Sub m_liveView_ImageUpdated(ByVal sender As Object, ByVal e As ImageUpdatedEventArgs) Handles m_liveView.ImageUpdated
            Try
                If m_PictureBox.InvokeRequired Then
                    m_PictureBox.Invoke(New Action(Of EvfImageInfo)(AddressOf setImage), e.CanonImage)
                Else
                    setImage(e.CanonImage)
                End If
            Catch ex As ObjectDisposedException
            Catch ex As InvalidOperationException
            Catch ex As System.ComponentModel.InvalidAsynchronousStateException
            End Try
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            m_liveView.Dispose()
            m_session.Dispose()
        End Sub

        Private m_canonImageState As CanonImageState

        Private Sub m_PictureBox_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles m_PictureBox.Paint
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
        Private Sub m_PictureBox_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles m_PictureBox.MouseMove
            'Dim zoomRect = m_canonImageState.ZoomRect


        End Sub

        Private Class CanonImageState
            Friend ImgTransform As Transform2D
            Friend ZoomRect As RectangleF
            Friend Sub New(ByVal canonImg As EvfImageInfo, ByVal paintAreaRect As Rectangle, ByVal imgSize As Size)
                Dim imgRect = Transform2D.RectAtOrigin(imgSize)
                Dim trans = Transform2D.FromZoomScale(paintAreaRect, imgRect)
                Dim paintRect As Rectangle = trans.Transform(imgRect)
            End Sub
        End Class
    End Class

End Namespace
