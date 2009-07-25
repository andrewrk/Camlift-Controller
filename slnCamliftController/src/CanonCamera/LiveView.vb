Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.IO
Imports System.Threading

Namespace CanonCamera

    Public Class LiveView
        Implements IDisposable

        Private ReadOnly m_cam As Camera

        Private WithEvents m_imageGetter As ImageGetter
        Private Sub m_imageGetter_ImageUpdated(ByVal sender As Object, ByVal e As ImageUpdatedEventArgs) Handles m_imageGetter.ImageUpdated
            RaiseEvent ImageUpdated(Me, e)
        End Sub
        Public Event ImageUpdated As EventHandler(Of ImageUpdatedEventArgs)

        Friend Event Disposed()

        Friend Sub New(ByVal cam As Camera)
            m_cam = cam

            m_imageGetter = New ImageGetter(cam)
        End Sub

        Public Sub SetZoomPosition(ByVal p As Point)

        End Sub

#Region "IDisposable"
        Private m_disposed As Boolean = False ' To detect redundant calls
        Public Sub Dispose() Implements IDisposable.Dispose
            If m_disposed Then Return

            m_imageGetter.Dispose()
            Edsdk.EndLiveView(m_cam)
            RaiseEvent Disposed()

            m_disposed = True
        End Sub
#End Region

        Private Class ImageGetter
            Implements IDisposable

            Private Const initialWaitTime = 700
            Private Const intervalTime = 200

            Private ReadOnly m_cam As Camera

            Private m_streamManager As EvfMemStreamManager
            Private m_imageGetterThread As Thread

            Private m_keepRunning As Boolean = True

            Friend Event ImageUpdated As EventHandler(Of ImageUpdatedEventArgs)

            Friend Sub New(ByVal cam As Camera)
                m_cam = cam

                m_streamManager = New EvfMemStreamManager
                m_imageGetterThread = New Thread(AddressOf run)
                m_imageGetterThread.Start()
            End Sub

            Dim i As Integer
            Private Sub run()
                Thread.Sleep(initialWaitTime) ' initial wait
                'Try
                While m_keepRunning
                    i = 0
                    Dim nowPlusInterval = Now.Ticks + intervalTime
                    i = 1
                    Dim img = getImage()
                    i = 2
                    If img IsNot Nothing Then
                        i = 3
                        RaiseEvent ImageUpdated(Me, New ImageUpdatedEventArgs(img))
                        i = 4
                    End If
                    i = 5
                    Thread.Sleep(Math.Max(nowPlusInterval - Now.Ticks, 0))
                    i = 6
                End While
                'Catch ex As SdkException When ex.Error = CameraErrors.StreamWriteError
                '    MsgBox("LiveView image buffer too small!", MsgBoxStyle.Critical, MsgBoxTitle)
                'End Try
            End Sub

            Private Function getImage() As EvfImageInfo
                Try
                    Return m_streamManager.ReadEvfImage(m_cam)
                Catch ex As SdkException When ex.SdkError = SdkErrors.ObjectNotready
                    Return Nothing
                End Try
            End Function

#Region "IDisposable"
            Private m_disposed As Boolean = False ' To detect redundant calls
            Public Sub Dispose() Implements IDisposable.Dispose
                If m_disposed Then Return

                m_keepRunning = False
                Dim tries = 0
                Do Until m_imageGetterThread.Join(intervalTime)
                    tries += 1
                    If tries >= 3 Then Exit Do
                Loop
                m_streamManager.Dispose()

                m_disposed = True
            End Sub
#End Region
        End Class

    End Class

    Public Class ImageUpdatedEventArgs
        Inherits EventArgs

        Private m_CanonImage As EvfImageInfo
        Public ReadOnly Property CanonImage() As EvfImageInfo
            Get
                Return m_CanonImage
            End Get
        End Property

        Friend Sub New(ByVal img As EvfImageInfo)
            m_CanonImage = img
        End Sub

    End Class

    Friend Structure Transform2D
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

End Namespace
