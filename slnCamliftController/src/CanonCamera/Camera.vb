Imports System.Drawing

Namespace CanonCamera

    Public MustInherit Class ObjectWrapper 'EdsBaseRef
        Implements IDisposable

        Private ReadOnly m_ptr As IntPtr
        Friend ReadOnly Property Pointer() As IntPtr
            Get
                Return m_ptr
            End Get
        End Property

        Protected Sub New(ByVal ptr As IntPtr)
            m_ptr = ptr
        End Sub

#Region "IDisposable"
        Public Event Disposed As EventHandler
        Protected Sub verifyNotDisposed()
            If m_disposed Then Throw New ObjectDisposedException(Me.GetType.Name)
        End Sub
        Private m_disposed As Boolean = False ' To detect redundant calls
        Public Sub Dispose() Implements IDisposable.Dispose
            If Not m_disposed Then
                Cleanup() ' allow subclasses to free their resources
                Edsdk.Release(Me)
                m_disposed = True
                RaiseEvent Disposed(Me, New EventArgs)
            End If
        End Sub
        Protected Overridable Sub Cleanup()
            'Default empty
        End Sub
#End Region
    End Class

    Friend Class CameraList 'EdsCameraListRef
        Inherits ObjectWrapper

        Friend Sub New(ByVal ptr As IntPtr)
            MyBase.New(ptr)
        End Sub

        Public Function GetChildCount() As Integer
            verifyNotDisposed()
            Return Edsdk.GetChildCount(Me)
        End Function
        Public Function GetChildAtIndex(ByVal index As Integer) As Camera
            verifyNotDisposed()
            Return Edsdk.GetChildAtIndex(Me, index)
        End Function

    End Class


    ''' <summary>wrapper around EdsStreamRef</summary>
    Public Class Camera
        Inherits ObjectWrapper

        Friend Sub New(ByVal ptr As IntPtr)
            MyBase.New(ptr)
        End Sub

        Private WithEvents m_CurrentSession As Session
        Public ReadOnly Property CurrentSession() As Session
            Get
                verifyNotDisposed()
                Return m_CurrentSession
            End Get
        End Property
        Public ReadOnly Property InSession() As Boolean
            Get
                verifyNotDisposed()
                Return m_CurrentSession IsNot Nothing AndAlso m_CurrentSession.IsOpen
            End Get
        End Property

        Public Function BeginSession() As Session
            'validate state
            verifyNotDisposed()
            If InSession Then Throw New InvalidCameraOperationException

            m_CurrentSession = Edsdk.OpenSession(Me)
            Return m_CurrentSession
        End Function
        Private Sub m_CurrentSession_Closed() Handles m_CurrentSession.Closed
            m_CurrentSession = Nothing
        End Sub

        Protected Overrides Sub Cleanup()
            If m_CurrentSession IsNot Nothing Then m_CurrentSession.Dispose()
        End Sub

        Public Class Session
            Implements IDisposable

            Private ReadOnly m_cam As Camera
            Private m_downloadListener As IDisposable

            Friend Event Closed()

            Private WithEvents m_liveView As LiveView = Nothing
            Public ReadOnly Property CurrentLiveView() As LiveView
                Get
                    Return m_liveView
                End Get
            End Property
            Private Sub m_liveView_Disposed() Handles m_liveView.Disposed
                m_liveView = Nothing
            End Sub

            Public ReadOnly Property IsOpen() As Boolean
                Get
                    Return (Not m_disposed AndAlso _
                            m_cam IsNot Nothing AndAlso _
                            m_cam.CurrentSession Is Me)
                End Get
            End Property

            Friend Sub New(ByVal cam As Camera)
                m_cam = cam
                Edsdk.SetSaveToHost(cam)
                m_downloadListener = Edsdk.MakeDownloadListener(cam, "C:\temptest\")
            End Sub

            Public Sub TakePicture()
                verifyOpen()
                Edsdk.TakePicture(m_cam)
            End Sub

            Public Function BeginLiveView() As LiveView
                verifyOpen()
                m_liveView = Edsdk.BeginLiveView(m_cam)
                Return m_liveView
            End Function

            Public Sub SetZoomPosition(ByVal p As Point)

            End Sub

            Private Sub verifyOpen()
                If Not IsOpen Then Throw New InvalidCameraOperationException
            End Sub
#Region "IDisposable"
            Private m_disposed As Boolean = False ' To detect redundant calls
            Public Sub Dispose() Implements IDisposable.Dispose
                If m_disposed Then Return
                If IsOpen Then
                    m_downloadListener.Dispose()
                    Edsdk.CloseSession(m_cam)
                    RaiseEvent Closed()
                End If
                m_disposed = True
            End Sub
#End Region
        End Class

    End Class

End Namespace
