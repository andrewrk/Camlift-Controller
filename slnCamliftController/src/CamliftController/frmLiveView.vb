Imports System.Drawing
Imports VisionaryDigital.CanonCamera.Sdk

Namespace CanonCamera

    Public Class frmLiveView

        Private m_cam As Camera
        Private m_ShowGrid As Boolean

        Private WhiteBalanceValues As Integer()

        Private ZoomRatios As Integer()
        Private m_zoomIndex As Integer

        Private Const MaxZoomWidth = 3888
        Private Const MaxZoomHeight = 2592

        Private m_mouseDownZoomLocation As Point
        Private m_mouseDownPt As Point
        Private m_mouseDown As Boolean
        Private m_zoomPosition As Point

        Private Sub SetWhiteBalanceCombo(ByVal value As Integer)
            For i = 0 To WhiteBalanceValues.Length - 1
                If WhiteBalanceValues(i) = value Then
                    cboWhiteBalance.SelectedIndex = i
                    Exit For
                End If
            Next
        End Sub

        Public Sub New(ByVal cam As Camera)
            InitializeComponent() ' This call is required by the Windows Form Designer.

            WhiteBalanceValues = New Integer() { _
                EdsWhiteBalance.kEdsWhiteBalance_Click, _
                EdsWhiteBalance.kEdsWhiteBalance_Auto, _
                EdsWhiteBalance.kEdsWhiteBalance_Daylight, _
                EdsWhiteBalance.kEdsWhiteBalance_Cloudy, _
                EdsWhiteBalance.kEdsWhiteBalance_Tangsten, _
                EdsWhiteBalance.kEdsWhiteBalance_Fluorescent, _
                EdsWhiteBalance.kEdsWhiteBalance_Strobe, _
                EdsWhiteBalance.kEdsWhiteBalance_Shade, _
                EdsWhiteBalance.kEdsWhiteBalance_ColorTemp, _
                EdsWhiteBalance.kEdsWhiteBalance_PCSet1, _
                EdsWhiteBalance.kEdsWhiteBalance_PCSet2, _
                EdsWhiteBalance.kEdsWhiteBalance_PCSet3}

            ZoomRatios = New Integer() {1, 5, 10}
            m_zoomIndex = 0
            btnZoomOut.Enabled = False

            m_ShowGrid = False
            m_cam = cam

            m_cam.StartLiveView(Me.picLiveView)

            SetWhiteBalanceCombo(m_cam.WhiteBalance)
            m_cam.ZoomRatio = ZoomRatios(m_zoomIndex)
        End Sub


        Private Sub btnGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrid.Click
            m_ShowGrid = Not m_ShowGrid
            picLiveView.Refresh()
        End Sub

        Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
            Me.Close()
        End Sub

        Private Sub picLiveView_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picLiveView.MouseDown
            m_mouseDown = True
            m_mouseDownPt.X = e.X
            m_mouseDownPt.Y = e.Y
            m_mouseDownZoomLocation = GetZoomLoc()
        End Sub

        Private Function GetZoomLoc() As Point
            Dim zoomCamLoc As Point = m_cam.ZoomPosition
            Return New Point(zoomCamLoc.X / MaxZoomWidth * picLiveView.Width, _
                                             zoomCamLoc.Y / MaxZoomHeight * picLiveView.Height)
        End Function

        Private Function GetZoomSize() As Size
            Dim ratio As Integer = ZoomRatios(Math.Max(m_zoomIndex, 1))
            Return New Size(MaxZoomWidth / ratio, MaxZoomHeight / ratio)
        End Function

        Private Sub picLiveView_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picLiveView.MouseMove
            If Not e.Button = Windows.Forms.MouseButtons.Left Then m_mouseDown = False
            If Not m_mouseDown Then Exit Sub

            Dim newpt As Point = m_mouseDownZoomLocation

            If m_zoomIndex = 0 Then
                'move the little white box around
                newpt.Offset(e.X - m_mouseDownPt.X, e.Y - m_mouseDownPt.Y)
            Else
                'move the zoom position
                newpt.Offset(m_mouseDownPt.X - e.X, m_mouseDownPt.Y - e.Y)
            End If

            newpt.X = (newpt.X / picLiveView.Width) * MaxZoomWidth
            newpt.Y = (newpt.Y / picLiveView.Height) * MaxZoomHeight

            Dim ZSize As Size = GetZoomSize()
            If newpt.X < 0 Then newpt.X = 0
            If newpt.X + ZSize.Width > MaxZoomWidth Then newpt.X = MaxZoomWidth - ZSize.Width
            If newpt.Y < 0 Then newpt.Y = 0
            If newpt.Y + ZSize.Height > MaxZoomHeight Then newpt.Y = MaxZoomHeight - ZSize.Height

            m_cam.ZoomPosition = newpt
        End Sub

        Private Sub picLiveView_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picLiveView.MouseUp
            m_mouseDown = False
        End Sub

        Private Sub picLiveView_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles picLiveView.Paint
            If picLiveView.Image Is Nothing OrElse m_cam.LiveViewImageSize = Nothing OrElse m_zoomIndex <> 0 Then Return

            Dim g = e.Graphics
            Dim imgRect = New Rectangle(New Point(0, 0), m_cam.LiveViewImageSize)
            Dim trans As Transform2D
            Try
                trans = Transform2D.FromZoomScale(picLiveView.ClientRectangle, imgRect)
            Catch ex As DivideByZeroException
                Return ' rectagles are too small. just forget drawing anything.
            End Try
            Dim paintRect As Rectangle = trans.Transform(imgRect)
            If m_ShowGrid Then
                Using gridPen = New Pen(Color.Gray, 2)
                    Dim x1 As Integer = paintRect.X + paintRect.Width / 3, x2 As Integer = paintRect.X + 2 * paintRect.Width / 3
                    Dim y1 As Integer = paintRect.Y + paintRect.Height / 3, y2 As Integer = paintRect.Y + 2 * paintRect.Height / 3
                    g.DrawLine(gridPen, x1, paintRect.Top, x1, paintRect.Bottom)
                    g.DrawLine(gridPen, x2, paintRect.Top, x2, paintRect.Bottom)
                    g.DrawLine(gridPen, paintRect.Left, y1, paintRect.Right, y1)
                    g.DrawLine(gridPen, paintRect.Left, y2, paintRect.Right, y2)
                End Using
            End If

            Dim MaxZoom As Integer = ZoomRatios(m_zoomIndex + 1)
            ' draw zoom
            Dim zoomSize = New Size(m_cam.LiveViewImageSize.Width / MaxZoom, m_cam.LiveViewImageSize.Height / MaxZoom)
            Dim zoomLoc As Point = GetZoomLoc()
            Dim zoomRect = New Rectangle(zoomLoc, zoomSize)
            Using shadowPen As Pen = New Pen(Color.Black, 1)
                Dim shadowRect As Rectangle = zoomRect
                shadowRect.Offset(1, 1)
                g.DrawRectangle(shadowPen, shadowRect)
            End Using
            Using whitePen As Pen = New Pen(Color.White, 2)
                g.DrawRectangle(whitePen, zoomRect)
            End Using

        End Sub

        Private Sub frmLiveView_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            m_cam.StopLiveView()
        End Sub

        Private Structure Transform2D
            Private m_shift As Point
            Private m_scale As SizeF

            Public Shared Function FromZoomScale(ByVal boxBounds As Rectangle, ByVal content As Rectangle) As Transform2D
                Try
                    Dim boxAspect = boxBounds.Size.Height / boxBounds.Size.Width
                    Dim contentAspect = content.Size.Height / content.Size.Width
                    Dim linearScale As Double
                    If boxAspect < contentAspect Then
                        linearScale = boxBounds.Height / content.Height ' snug on top and bottom. left and right dangle
                    Else
                        linearScale = boxBounds.Width / content.Width ' snug on left and right. top and bottom dangle
                    End If
                    Dim actualImgSize = New Size(content.Width * linearScale, content.Height * linearScale)
                    Dim ulCorner = New Point(boxBounds.Left + boxBounds.Width / 2 - actualImgSize.Width / 2, boxBounds.Top + boxBounds.Height / 2 - actualImgSize.Height / 2)
                    Dim imgRect = New Rectangle(ulCorner, actualImgSize)
                    Return New Transform2D With {.m_shift = New Point(ulCorner.X - content.X, ulCorner.Y - content.Y), _
                                               .m_scale = New SizeF(imgRect.Width / content.Width, imgRect.Height / content.Height)}
                Catch ex As DivideByZeroException
                    Return Transform2D.Zero 'sizes have zeros
                End Try
            End Function

            Public Function Transform(ByVal p As Point) As Point
                Return New Point(p.X * m_scale.Width + m_shift.X, p.Y * m_scale.Height + m_shift.Y)
            End Function
            Public Function Transform(ByVal s As Size) As Size
                Return New Size(s.Width * m_scale.Width, s.Height * m_scale.Height)
            End Function
            Public Function Transform(ByVal r As Rectangle) As Rectangle
                Return New Rectangle(Transform(r.Location), Transform(r.Size))
            End Function

            Public Shared ReadOnly Property Identity() As Transform2D
                Get
                    Return New Transform2D With {.m_shift = New Point(0, 0), _
                                                 .m_scale = New SizeF(1, 1)}
                End Get
            End Property
            Public Shared ReadOnly Property Zero() As Transform2D
                Get
                    Return New Transform2D With {.m_shift = New Point(0, 0), _
                                                 .m_scale = New SizeF(0, 0)}
                End Get
            End Property

            ''' <param name="p">values between 0.0 and 1.0</param>
            Public Shared Function Scale(ByVal p As PointF, ByVal s As Size) As Point
                Return New Point(p.X * s.Width, p.Y * s.Height)
            End Function
            Public Shared Function RectAtOrigin(ByVal s As Size) As Rectangle
                Return New Rectangle(New Point(0, 0), s)
            End Function

        End Structure

        Private Sub cboWhiteBalance_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWhiteBalance.SelectedIndexChanged
            m_cam.WhiteBalance = WhiteBalanceValues(cboWhiteBalance.SelectedIndex)
        End Sub

        Private Sub btnZoomIn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnZoomIn.Click
            m_zoomIndex += 1
            UpdateZoom()
        End Sub

        Private Sub UpdateZoom()
            btnZoomOut.Enabled = m_zoomIndex > 0
            btnZoomIn.Enabled = m_zoomIndex < ZoomRatios.Length - 1
            lblZoom.Text = (ZoomRatios(m_zoomIndex) * 100) & "%"
            m_cam.ZoomRatio = ZoomRatios(m_zoomIndex)
        End Sub

        Private Sub btnZoomOut_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnZoomOut.Click
            m_zoomIndex -= 1
            UpdateZoom()
        End Sub
    End Class
End Namespace
