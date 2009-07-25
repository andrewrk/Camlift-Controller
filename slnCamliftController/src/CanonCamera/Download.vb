Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.IO

Namespace CanonCamera

    Friend MustInherit Class MemStreamManager
        Implements IDisposable

        Protected MustOverride ReadOnly Property BufferSize() As Integer

        Protected m_buffer As Byte()
        Private m_bufferHandle As GCHandle
        Protected m_memStream As MemStream

        Friend Sub New()

            ReDim m_buffer(BufferSize)
            m_bufferHandle = GCHandle.Alloc(m_buffer, GCHandleType.Pinned)

            m_memStream = Edsdk.CreatStream(m_bufferHandle.AddrOfPinnedObject, BufferSize)
        End Sub

#Region "IDisposable"
        Private m_disposed As Boolean = False ' To detect redundant calls
        Public Sub Dispose() Implements IDisposable.Dispose
            If m_disposed Then Return

            m_memStream.Dispose()
            m_bufferHandle.Free()
            m_buffer = Nothing

            m_disposed = True
        End Sub
#End Region
    End Class
    Friend Class EvfMemStreamManager
        Inherits MemStreamManager

        Protected Overrides ReadOnly Property BufferSize() As Integer
            Get
                Return &H80000 'this number seemed to work...
            End Get
        End Property

        Friend Function ReadEvfImage(ByVal cam As Camera) As EvfImageInfo
            Try
                Using evfImg = Edsdk.DownloadEvfImage(cam, m_memStream)
                    Return New EvfImageInfo(evfImg, Image.FromStream(New MemoryStream(m_buffer))) 'do not dispose the MemoryStream (Image.FromStream)
                End Using
            Catch ex As SdkException When False 'ex.SdkError = SdkErrors.DeviceBusy
                Return Nothing
            End Try
        End Function
    End Class

    Public Class EvfImageInfo

        Private m_Image As Image
        Public ReadOnly Property Image() As Image
            Get
                Return m_Image
            End Get
        End Property

        Private m_ZoomPosition As PointF
        Public ReadOnly Property ZoomPosition() As PointF
            Get
                Return m_ZoomPosition
            End Get
        End Property

        ''' <param name="evfImg">can be disposed after this constructor returns</param>
        Friend Sub New(ByVal evfImg As EvfImage, ByVal img As Image)
            m_Image = img

            m_ZoomPosition = evfImg.GetZoomPosition

        End Sub
    End Class

    Public Class ImageInfo

        Private m_Image As Image
        Public ReadOnly Property Image() As Image
            Get
                Return m_Image
            End Get
        End Property

        ''' <param name="evfImg">can be disposed after this constructor returns</param>
        Friend Sub New(ByVal evfImg As EvfImage, ByVal img As Image)
            m_Image = img
        End Sub
    End Class

    ''' <summary>wrapper around EdsStreamRef</summary>
    Friend Class MemStream
        Inherits ObjectWrapper

        Friend Sub New(ByVal ptr As IntPtr)
            MyBase.New(ptr)
        End Sub
    End Class
    ''' <summary>wrapper around EdsEvfImageRef</summary>
    Friend Class EvfImage
        Inherits ObjectWrapper

        Friend Shared ReadOnly LargeJpegSize As New Size(3888, 2592)

        Friend Function GetZoomPosition() As PointF
            Dim p = Edsdk.GetZoomPosition(Me)
            Return New PointF(p.X / LargeJpegSize.Width, p.Y / LargeJpegSize.Height)
        End Function

        Friend Sub New(ByVal ptr As IntPtr)
            MyBase.New(ptr)
        End Sub
    End Class



End Namespace
