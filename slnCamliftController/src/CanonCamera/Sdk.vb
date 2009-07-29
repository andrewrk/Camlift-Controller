'Imports System.Runtime.InteropServices
'Imports System.Drawing
'Imports System.IO
'Imports System.Threading

'Namespace CanonCamera

'    Public NotInheritable Class Edsdk 'abstracts low level functions from the EDSDK <andy: hardly!!!>
'        Friend Shared Function BeginLiveView(ByVal cam As Camera) As LiveView
'            setProperty(cam, Sdk.kEdsPropID_Evf_OutputDevice, Sdk.EdsEvfOutputDevice.kEdsEvfOutputDevice_PC)
'            Return New LiveView(cam)
'        End Function
'        Friend Shared Sub EndLiveView(ByVal cam As Camera)
'            setProperty(cam, Sdk.kEdsPropID_Evf_OutputDevice, 0)
'        End Sub
'        Friend Shared Function DownloadEvfImage(ByVal cam As Camera, ByVal stream As MemStream) As EvfImage
'            Dim imgRef As IntPtr
'            verifyNoError(Sdk.EdsCreateEvfImageRef(stream.Pointer, imgRef))
'            verifyNoError(Sdk.EdsDownloadEvfImage(cam.Pointer, imgRef))
'            Return New EvfImage(imgRef)
'        End Function

'        Friend Shared Function CreateStream(ByVal bufferPointer As IntPtr, ByVal size As Integer) As MemStream
'            Dim ptr As IntPtr
'            verifyNoError(Sdk.EdsCreateMemoryStreamFromPointer(bufferPointer, size, ptr))
'            Return New MemStream(ptr)
'        End Function


'        'EdsEvfImageRef
'        Friend Shared Function GetImagePosition(ByVal rawImg As EvfImage) As Point
'            Dim edsPnt = GetPropertyData(Of Sdk.EdsPoint)(rawImg.Pointer, Sdk.kEdsPropID_Evf_ImagePosition, 0)
'            Return PointFromEdsPoint(edsPnt)
'        End Function
'        Friend Shared Function GetZoomPosition(ByVal rawImg As EvfImage) As Point
'            Dim edsPnt = GetPropertyData(Of Sdk.EdsPoint)(rawImg.Pointer, Sdk.kEdsPropID_Evf_ZoomPosition, 0)
'            Return PointFromEdsPoint(edsPnt)
'        End Function


'        Private Shared Function PointFromEdsPoint(ByVal edsPnt As Sdk.EdsPoint)
'            Return New Point(edsPnt.x, edsPnt.y)
'        End Function

'    End Class


'    Public Class InvalidCameraOperationException
'        Inherits InvalidOperationException
'    End Class

'End Namespace
