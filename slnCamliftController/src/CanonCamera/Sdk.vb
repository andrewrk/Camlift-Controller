Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.IO
Imports System.Threading

Namespace CanonCamera

    Public NotInheritable Class Edsdk 'abstracts low level functions from the EDSDK
        Implements IDisposable

        Private Shared s_singleton As Edsdk = Nothing
        Private Sub New()
            verifyNoError(Sdk.EdsInitializeSDK())
        End Sub
        Public Shared Function Init() As Edsdk
            If s_singleton Is Nothing Then s_singleton = New Edsdk
            Return s_singleton
        End Function

        Private WithEvents s_onlyCamera As Camera = Nothing
        Private Sub s_onlyCamera_Disposed(ByVal sender As Object, ByVal e As EventArgs) Handles s_onlyCamera.Disposed
            s_onlyCamera = Nothing
        End Sub

        Public Function GetOnlyCamera() As Camera
            If s_onlyCamera IsNot Nothing Then Return s_onlyCamera
            Using camLst = getCameraList()
                Dim count = camLst.GetChildCount()
                If count = 0 Then Throw New NoCameraFoundException
                If count <> 1 Then Throw New TooManyCamerasFoundException
                ' count is 1
                s_onlyCamera = camLst.GetChildAtIndex(0)
                Return s_onlyCamera
            End Using
        End Function
        Private Function getCameraList() As CameraList
            Dim ptr As IntPtr
            verifyNoError(Sdk.EdsGetCameraList(ptr))
            Return New CameraList(ptr)
        End Function

#Region "IDisposable"
        Private m_disposed As Boolean = False ' To detect redundant calls
        Public Sub Dispose() Implements IDisposable.Dispose
            Try
                Sdk.EdsTerminateSDK()
            Finally
                m_disposed = True
            End Try
        End Sub
#End Region

        'EdsBaseRef
        Friend Shared Sub Release(ByVal obj As ObjectWrapper)
            Release(obj.Pointer)
        End Sub
        Private Shared Sub Release(ByVal ptr As IntPtr)
            verifyNoError(Sdk.EdsRelease(ptr))
        End Sub

        'EdsCameraListRef
        Friend Shared Function GetChildCount(ByVal camLst As CameraList) As Integer
            Dim count As Integer
            verifyNoError(Sdk.EdsGetChildCount(camLst.Pointer, count))
            Return count
        End Function
        Friend Shared Function GetChildAtIndex(ByVal camLst As CameraList, ByVal index As Integer) As Camera
            Dim ptrChild As IntPtr
            verifyNoError(Sdk.EdsGetChildAtIndex(camLst.Pointer, index, ptrChild))
            Return New Camera(ptrChild)
        End Function

        'EdsCameraRef
        Friend Shared Function OpenSession(ByVal cam As Camera) As Camera.Session
            verifyNoError(Sdk.EdsOpenSession(cam.Pointer))
            Return New Camera.Session(cam)
        End Function
        Friend Shared Sub TakePicture(ByVal cam As Camera)
            verifyNoError(Sdk.EdsSendCommand(cam.Pointer, Sdk.EdsCameraCommand.kEdsCameraCommand_TakePicture, 0))
        End Sub
        Friend Shared Sub CloseSession(ByVal cam As Camera)
            verifyNoError(Sdk.EdsCloseSession(cam.Pointer))
        End Sub
        Friend Shared Function BeginLiveView(ByVal cam As Camera) As LiveView
            setProperty(cam, Sdk.kEdsPropID_Evf_OutputDevice, Sdk.EdsEvfOutputDevice.kEdsEvfOutputDevice_PC)
            Return New LiveView(cam)
        End Function
        Friend Shared Sub EndLiveView(ByVal cam As Camera)
            setProperty(cam, Sdk.kEdsPropID_Evf_OutputDevice, 0)
        End Sub
        Friend Shared Function DownloadEvfImage(ByVal cam As Camera, ByVal stream As MemStream) As EvfImage
            Dim imgRef As IntPtr
            verifyNoError(Sdk.EdsCreateEvfImageRef(stream.Pointer, imgRef))
            verifyNoError(Sdk.EdsDownloadEvfImage(cam.Pointer, imgRef))
            Return New EvfImage(imgRef)
        End Function
        Friend Shared Sub SetSaveToHost(ByVal cam As Camera)
            setProperty(cam, Sdk.kEdsPropID_SaveTo, Sdk.EdsSaveTo.kEdsSaveTo_Host)
        End Sub
        Friend Shared Function MakeDownloadListener(ByVal cam As Camera, ByVal folderName As String) As IDisposable
            Return DownloadListener.Begin(cam, folderName)
        End Function

        Friend Shared Function CreateStream(ByVal bufferPointer As IntPtr, ByVal size As Integer) As MemStream
            Dim ptr As IntPtr
            verifyNoError(Sdk.EdsCreateMemoryStreamFromPointer(bufferPointer, size, ptr))
            Return New MemStream(ptr)
        End Function

        'EdsDirectoryItem
        Friend Shared Sub DownloadImage(ByVal dirItem As DirectoryItem, ByVal destFolder As String)
            Dim dirItemInfo = GetDirectoryItemInfo(dirItem)
            Dim streamRef As IntPtr
            verifyNoError(Sdk.EdsCreateFileStream(destFolder & dirItemInfo.szFileName, Sdk.EdsFileCreateDisposition.kEdsFileCreateDisposition_CreateAlways, _
                                                  Sdk.EdsAccess.kEdsAccess_ReadWrite, streamRef))
            Try
                verifyNoError(Sdk.EdsDownload(dirItem.Pointer, dirItemInfo.size, streamRef))
                verifyNoError(Sdk.EdsDownloadComplete(dirItem.Pointer))
            Finally
                Release(streamRef)
            End Try
        End Sub
        Private Shared Function GetDirectoryItemInfo(ByVal dirItem As DirectoryItem) As Sdk.EdsDirectoryItemInfo
            Dim dirItemInfo As Sdk.EdsDirectoryItemInfo = Nothing
            verifyNoError(Sdk.EdsGetDirectoryItemInfo(dirItem.Pointer, dirItemInfo))
            Return dirItemInfo
        End Function

        'EdsEvfImageRef
        Friend Shared Function GetImagePosition(ByVal rawImg As EvfImage) As Point
            Dim edsPnt = GetPropertyData(Of Sdk.EdsPoint)(rawImg.Pointer, Sdk.kEdsPropID_Evf_ImagePosition, 0)
            Return PointFromEdsPoint(edsPnt)
        End Function
        Friend Shared Function GetZoomPosition(ByVal rawImg As EvfImage) As Point
            Dim edsPnt = GetPropertyData(Of Sdk.EdsPoint)(rawImg.Pointer, Sdk.kEdsPropID_Evf_ZoomPosition, 0)
            Return PointFromEdsPoint(edsPnt)
        End Function


        Private Shared Function PointFromEdsPoint(ByVal edsPnt As Sdk.EdsPoint)
            Return New Point(edsPnt.x, edsPnt.y)
        End Function
        Private Shared Sub setProperty(ByVal cam As Camera, ByVal propertyID As Integer, ByVal value As Integer)
            verifyNoError(Sdk.EdsSetPropertyData(cam.Pointer, propertyID, 0, Marshal.SizeOf(GetType(Integer)), value))
        End Sub
        Private Shared Sub verifyNoError(ByVal err As Integer)
            If err <> Sdk.EDS_ERR_OK Then Throw New SdkException(err)
        End Sub
        Private Shared Function GetPropertyData(Of T As Structure)(ByVal inRef As IntPtr, ByVal inPropertyID As UInt32, ByVal inParam As Integer) As T
            Using pointRef = New StructurePointer(Of T)
                verifyNoError(Sdk.EDSDK.EdsGetPropertyData(inRef, inPropertyID, inParam, pointRef.Size, pointRef.Pointer))
                Return pointRef.Value
            End Using
        End Function

        Private Class DownloadListener
            Implements IDisposable

            Private Shared s_lock As New Object
            Private Shared s_instance As DownloadListener = Nothing
            Private Shared f_object_listener As New Sdk.EdsObjectEventHandler(AddressOf object_listener)

            Public Shared Function Begin(ByVal cam As Camera, ByVal folderName As String) As DownloadListener
                SyncLock s_lock
                    If s_instance IsNot Nothing Then Throw New InvalidCameraOperationException
                    verifyNoError(Sdk.EdsSetObjectEventHandler(cam.Pointer, Sdk.kEdsObjectEvent_All, f_object_listener, IntPtr.Zero))
                    s_instance = New DownloadListener(cam, folderName)
                    Return s_instance
                End SyncLock
            End Function

            Public Shared Function object_listener(ByVal inEvent As Integer, ByVal inRef As IntPtr, ByVal inContext As IntPtr) As Long
                s_instance.handle_event(inEvent, inRef, inContext)
            End Function


            Private ReadOnly m_camera As Camera
            Private ReadOnly m_folderName As String
            Private m_currentDownloader As ImageDownloader = Nothing

            Private Sub New(ByVal cam As Camera, ByVal folderName As String)
                m_camera = cam
                m_folderName = folderName
            End Sub

            Private Function handle_event(ByVal inEvent As Integer, ByVal inRef As IntPtr, ByVal inContext As IntPtr) As Long
                Select Case inEvent
                    Case Sdk.kEdsObjectEvent_DirItemRequestTransfer, Sdk.kEdsObjectEvent_DirItemRequestTransferDT
                        Dim dirItem = New DirectoryItem(inRef)
                        m_currentDownloader = New ImageDownloader(dirItem, m_folderName)
                        m_currentDownloader.Start()
                End Select
            End Function
#Region "IDisposable"
            Private m_disposed As Boolean = False ' To detect redundant calls
            Public Sub Dispose() Implements IDisposable.Dispose
                If m_disposed Then Return
                verifyNoError(Sdk.EdsSetObjectEventHandler(m_camera.Pointer, Sdk.kEdsObjectEvent_All, Nothing, IntPtr.Zero))
                SyncLock s_lock
                    s_instance = Nothing
                End SyncLock
                m_disposed = True
            End Sub
#End Region

            Private Class ImageDownloader
                Private ReadOnly m_dirItem As DirectoryItem
                Private ReadOnly m_folderName As String
                Private m_thread As Thread
                Private lock As New Object
                Public Sub New(ByVal dirItem As DirectoryItem, ByVal folderName As String)
                    m_dirItem = dirItem
                    m_folderName = folderName
                End Sub
                Public Sub Start()
                    'SyncLock lock
                    '    If m_thread IsNot Nothing Then Throw New InvalidCameraOperationException
                    '    m_thread = New Thread(AddressOf run)
                    'End SyncLock
                    'm_thread.Start()
                    run()
                End Sub
                Private Sub run()
                    Edsdk.DownloadImage(m_dirItem, m_folderName)
                End Sub
            End Class
        End Class
    End Class

    Public NotInheritable Class SdkErrors
        Private Sub New() 'static class
        End Sub

        Private Shared m_dict As Dictionary(Of Integer, String)

        Public Shared Function StringFromErrorCode(ByVal errCode As Integer) As String
            If m_dict Is Nothing Then initDict()
            If m_dict.ContainsKey(errCode) Then
                Return m_dict.Item(errCode)
            Else
                Return "Error code: " & errCode
            End If
        End Function

#Region "Generated Code"

        Private Shared Sub initDict()
            m_dict = New Dictionary(Of Integer, String)(117)

            ' Miscellaneous errors
            m_dict.Add(Sdk.EDS_ERR_UNIMPLEMENTED, Unimplemented)
            m_dict.Add(Sdk.EDS_ERR_INTERNAL_ERROR, InternalError)
            m_dict.Add(Sdk.EDS_ERR_MEM_ALLOC_FAILED, MemAllocFailed)
            m_dict.Add(Sdk.EDS_ERR_MEM_FREE_FAILED, MemFreeFailed)
            m_dict.Add(Sdk.EDS_ERR_OPERATION_CANCELLED, OperationCancelled)
            m_dict.Add(Sdk.EDS_ERR_INCOMPATIBLE_VERSION, IncompatibleVersion)
            m_dict.Add(Sdk.EDS_ERR_NOT_SUPPORTED, NotSupported)
            m_dict.Add(Sdk.EDS_ERR_UNEXPECTED_EXCEPTION, UnexpectedException)
            m_dict.Add(Sdk.EDS_ERR_PROTECTION_VIOLATION, ProtectionViolation)
            m_dict.Add(Sdk.EDS_ERR_MISSING_SUBCOMPONENT, MissingSubcomponent)
            m_dict.Add(Sdk.EDS_ERR_SELECTION_UNAVAILABLE, SelectionUnavailable)

            ' File errors
            m_dict.Add(Sdk.EDS_ERR_FILE_IO_ERROR, FileIoError)
            m_dict.Add(Sdk.EDS_ERR_FILE_TOO_MANY_OPEN, FileTooManyOpen)
            m_dict.Add(Sdk.EDS_ERR_FILE_NOT_FOUND, FileNotFound)
            m_dict.Add(Sdk.EDS_ERR_FILE_OPEN_ERROR, FileOpenError)
            m_dict.Add(Sdk.EDS_ERR_FILE_CLOSE_ERROR, FileCloseError)
            m_dict.Add(Sdk.EDS_ERR_FILE_SEEK_ERROR, FileSeekError)
            m_dict.Add(Sdk.EDS_ERR_FILE_TELL_ERROR, FileTellError)
            m_dict.Add(Sdk.EDS_ERR_FILE_READ_ERROR, FileReadError)
            m_dict.Add(Sdk.EDS_ERR_FILE_WRITE_ERROR, FileWriteError)
            m_dict.Add(Sdk.EDS_ERR_FILE_PERMISSION_ERROR, FilePermissionError)
            m_dict.Add(Sdk.EDS_ERR_FILE_DISK_FULL_ERROR, FileDiskFullError)
            m_dict.Add(Sdk.EDS_ERR_FILE_ALREADY_EXISTS, FileAlreadyExists)
            m_dict.Add(Sdk.EDS_ERR_FILE_FORMAT_UNRECOGNIZED, FileFormatUnrecognized)
            m_dict.Add(Sdk.EDS_ERR_FILE_DATA_CORRUPT, FileDataCorrupt)
            m_dict.Add(Sdk.EDS_ERR_FILE_NAMING_NA, FileNamingNa)

            ' Directory errors
            m_dict.Add(Sdk.EDS_ERR_DIR_NOT_FOUND, DirNotFound)
            m_dict.Add(Sdk.EDS_ERR_DIR_IO_ERROR, DirIoError)
            m_dict.Add(Sdk.EDS_ERR_DIR_ENTRY_NOT_FOUND, DirEntryNotFound)
            m_dict.Add(Sdk.EDS_ERR_DIR_ENTRY_EXISTS, DirEntryExists)
            m_dict.Add(Sdk.EDS_ERR_DIR_NOT_EMPTY, DirNotEmpty)

            ' Property errors
            m_dict.Add(Sdk.EDS_ERR_PROPERTIES_UNAVAILABLE, PropertiesUnavailable)
            m_dict.Add(Sdk.EDS_ERR_PROPERTIES_MISMATCH, PropertiesMismatch)
            m_dict.Add(Sdk.EDS_ERR_PROPERTIES_NOT_LOADED, PropertiesNotLoaded)

            ' Function Parameter errors
            m_dict.Add(Sdk.EDS_ERR_INVALID_PARAMETER, InvalidParameter)
            m_dict.Add(Sdk.EDS_ERR_INVALID_HANDLE, InvalidHandle)
            m_dict.Add(Sdk.EDS_ERR_INVALID_POINTER, InvalidPointer)
            m_dict.Add(Sdk.EDS_ERR_INVALID_INDEX, InvalidIndex)
            m_dict.Add(Sdk.EDS_ERR_INVALID_LENGTH, InvalidLength)
            m_dict.Add(Sdk.EDS_ERR_INVALID_FN_POINTER, InvalidFnPointer)
            m_dict.Add(Sdk.EDS_ERR_INVALID_SORT_FN, InvalidSortFn)

            ' Device errors
            m_dict.Add(Sdk.EDS_ERR_DEVICE_NOT_FOUND, DeviceNotFound)
            m_dict.Add(Sdk.EDS_ERR_DEVICE_BUSY, DeviceBusy)
            m_dict.Add(Sdk.EDS_ERR_DEVICE_INVALID, DeviceInvalid)
            m_dict.Add(Sdk.EDS_ERR_DEVICE_EMERGENCY, DeviceEmergency)
            m_dict.Add(Sdk.EDS_ERR_DEVICE_MEMORY_FULL, DeviceMemoryFull)
            m_dict.Add(Sdk.EDS_ERR_DEVICE_INTERNAL_ERROR, DeviceInternalError)
            m_dict.Add(Sdk.EDS_ERR_DEVICE_INVALID_PARAMETER, DeviceInvalidParameter)
            m_dict.Add(Sdk.EDS_ERR_DEVICE_NO_DISK, DeviceNoDisk)
            m_dict.Add(Sdk.EDS_ERR_DEVICE_DISK_ERROR, DeviceDiskError)
            m_dict.Add(Sdk.EDS_ERR_DEVICE_CF_GATE_CHANGED, DeviceCfGateChanged)
            m_dict.Add(Sdk.EDS_ERR_DEVICE_DIAL_CHANGED, DeviceDialChanged)
            m_dict.Add(Sdk.EDS_ERR_DEVICE_NOT_INSTALLED, DeviceNotInstalled)
            m_dict.Add(Sdk.EDS_ERR_DEVICE_STAY_AWAKE, DeviceStayAwake)
            m_dict.Add(Sdk.EDS_ERR_DEVICE_NOT_RELEASED, DeviceNotReleased)

            ' Stream errors
            m_dict.Add(Sdk.EDS_ERR_STREAM_IO_ERROR, StreamIoError)
            m_dict.Add(Sdk.EDS_ERR_STREAM_NOT_OPEN, StreamNotOpen)
            m_dict.Add(Sdk.EDS_ERR_STREAM_ALREADY_OPEN, StreamAlreadyOpen)
            m_dict.Add(Sdk.EDS_ERR_STREAM_OPEN_ERROR, StreamOpenError)
            m_dict.Add(Sdk.EDS_ERR_STREAM_CLOSE_ERROR, StreamCloseError)
            m_dict.Add(Sdk.EDS_ERR_STREAM_SEEK_ERROR, StreamSeekError)
            m_dict.Add(Sdk.EDS_ERR_STREAM_TELL_ERROR, StreamTellError)
            m_dict.Add(Sdk.EDS_ERR_STREAM_READ_ERROR, StreamReadError)
            m_dict.Add(Sdk.EDS_ERR_STREAM_WRITE_ERROR, StreamWriteError)
            m_dict.Add(Sdk.EDS_ERR_STREAM_PERMISSION_ERROR, StreamPermissionError)
            m_dict.Add(Sdk.EDS_ERR_STREAM_COULDNT_BEGIN_THREAD, StreamCouldntBeginThread)
            m_dict.Add(Sdk.EDS_ERR_STREAM_BAD_OPTIONS, StreamBadOptions)
            m_dict.Add(Sdk.EDS_ERR_STREAM_END_OF_STREAM, StreamEndOfStream)

            ' Communications errors
            m_dict.Add(Sdk.EDS_ERR_COMM_PORT_IS_IN_USE, CommPortIsInUse)
            m_dict.Add(Sdk.EDS_ERR_COMM_DISCONNECTED, CommDisconnected)
            m_dict.Add(Sdk.EDS_ERR_COMM_DEVICE_INCOMPATIBLE, CommDeviceIncompatible)
            m_dict.Add(Sdk.EDS_ERR_COMM_BUFFER_FULL, CommBufferFull)
            m_dict.Add(Sdk.EDS_ERR_COMM_USB_BUS_ERR, CommUsbBusErr)

            ' Lock/Unlock
            m_dict.Add(Sdk.EDS_ERR_USB_DEVICE_LOCK_ERROR, UsbDeviceLockError)
            m_dict.Add(Sdk.EDS_ERR_USB_DEVICE_UNLOCK_ERROR, UsbDeviceUnlockError)

            ' STI/WIA
            m_dict.Add(Sdk.EDS_ERR_STI_UNKNOWN_ERROR, StiUnknownError)
            m_dict.Add(Sdk.EDS_ERR_STI_INTERNAL_ERROR, StiInternalError)
            m_dict.Add(Sdk.EDS_ERR_STI_DEVICE_CREATE_ERROR, StiDeviceCreateError)
            m_dict.Add(Sdk.EDS_ERR_STI_DEVICE_RELEASE_ERROR, StiDeviceReleaseError)
            m_dict.Add(Sdk.EDS_ERR_DEVICE_NOT_LAUNCHED, DeviceNotLaunched)

            m_dict.Add(Sdk.EDS_ERR_ENUM_NA, EnumNa)
            m_dict.Add(Sdk.EDS_ERR_INVALID_FN_CALL, InvalidFnCall)
            m_dict.Add(Sdk.EDS_ERR_HANDLE_NOT_FOUND, HandleNotFound)
            m_dict.Add(Sdk.EDS_ERR_INVALID_ID, InvalidId)
            m_dict.Add(Sdk.EDS_ERR_WAIT_TIMEOUT_ERROR, WaitTimeoutError)

            ' PTP
            m_dict.Add(Sdk.EDS_ERR_SESSION_NOT_OPEN, SessionNotOpen)
            m_dict.Add(Sdk.EDS_ERR_INVALID_TRANSACTIONID, InvalidTransactionid)
            m_dict.Add(Sdk.EDS_ERR_INCOMPLETE_TRANSFER, IncompleteTransfer)
            m_dict.Add(Sdk.EDS_ERR_INVALID_STRAGEID, InvalidStrageid)
            m_dict.Add(Sdk.EDS_ERR_DEVICEPROP_NOT_SUPPORTED, DevicepropNotSupported)
            m_dict.Add(Sdk.EDS_ERR_INVALID_OBJECTFORMATCODE, InvalidObjectformatcode)
            m_dict.Add(Sdk.EDS_ERR_SELF_TEST_FAILED, SelfTestFailed)
            m_dict.Add(Sdk.EDS_ERR_PARTIAL_DELETION, PartialDeletion)
            m_dict.Add(Sdk.EDS_ERR_SPECIFICATION_BY_FORMAT_UNSUPPORTED, SpecificationByFormatUnsupported)
            m_dict.Add(Sdk.EDS_ERR_NO_VALID_OBJECTINFO, NoValidObjectinfo)
            m_dict.Add(Sdk.EDS_ERR_INVALID_CODE_FORMAT, InvalidCodeFormat)
            m_dict.Add(Sdk.EDS_ERR_UNKNOWN_VENDER_CODE, UnknownVenderCode)
            m_dict.Add(Sdk.EDS_ERR_CAPTURE_ALREADY_TERMINATED, CaptureAlreadyTerminated)
            m_dict.Add(Sdk.EDS_ERR_INVALID_PARENTOBJECT, InvalidParentobject)
            m_dict.Add(Sdk.EDS_ERR_INVALID_DEVICEPROP_FORMAT, InvalidDevicepropFormat)
            m_dict.Add(Sdk.EDS_ERR_INVALID_DEVICEPROP_VALUE, InvalidDevicepropValue)
            m_dict.Add(Sdk.EDS_ERR_SESSION_ALREADY_OPEN, SessionAlreadyOpen)
            m_dict.Add(Sdk.EDS_ERR_TRANSACTION_CANCELLED, TransactionCancelled)
            m_dict.Add(Sdk.EDS_ERR_SPECIFICATION_OF_DESTINATION_UNSUPPORTED, SpecificationOfDestinationUnsupported)
            m_dict.Add(Sdk.EDS_ERR_UNKNOWN_COMMAND, UnknownCommand)
            m_dict.Add(Sdk.EDS_ERR_OPERATION_REFUSED, OperationRefused)
            m_dict.Add(Sdk.EDS_ERR_LENS_COVER_CLOSE, LensCoverClose)
            m_dict.Add(Sdk.EDS_ERR_LOW_BATTERY, LowBattery)
            m_dict.Add(Sdk.EDS_ERR_OBJECT_NOTREADY, ObjectNotready)

            m_dict.Add(Sdk.EDS_ERR_TAKE_PICTURE_AF_NG, TakePictureAfNg)
            m_dict.Add(Sdk.EDS_ERR_TAKE_PICTURE_RESERVED, TakePictureReserved)
            m_dict.Add(Sdk.EDS_ERR_TAKE_PICTURE_MIRROR_UP_NG, TakePictureMirrorUpNg)
            m_dict.Add(Sdk.EDS_ERR_TAKE_PICTURE_SENSOR_CLEANING_NG, TakePictureSensorCleaningNg)
            m_dict.Add(Sdk.EDS_ERR_TAKE_PICTURE_SILENCE_NG, TakePictureSilenceNg)
            m_dict.Add(Sdk.EDS_ERR_TAKE_PICTURE_NO_CARD_NG, TakePictureNoCardNg)
            m_dict.Add(Sdk.EDS_ERR_TAKE_PICTURE_CARD_NG, TakePictureCardNg)
            m_dict.Add(Sdk.EDS_ERR_TAKE_PICTURE_CARD_PROTECT_NG, TakePictureCardProtectNg)

            ' 44313 ???
        End Sub

        ' Miscellaneous errors
        Public Const Unimplemented = "Unimplemented"
        Public Const InternalError = "Internal Error"
        Public Const MemAllocFailed = "Mem Alloc Failed"
        Public Const MemFreeFailed = "Mem Free Failed"
        Public Const OperationCancelled = "Operation Cancelled"
        Public Const IncompatibleVersion = "Incompatible Version"
        Public Const NotSupported = "Not Supported"
        Public Const UnexpectedException = "Unexpected Exception"
        Public Const ProtectionViolation = "Protection Violation"
        Public Const MissingSubcomponent = "Missing Subcomponent"
        Public Const SelectionUnavailable = "Selection Unavailable"

        ' File errors
        Public Const FileIoError = "File IO Error"
        Public Const FileTooManyOpen = "File Too Many Open"
        Public Const FileNotFound = "File Not Found"
        Public Const FileOpenError = "File Open Error"
        Public Const FileCloseError = "File Close Error"
        Public Const FileSeekError = "File Seek Error"
        Public Const FileTellError = "File Tell Error"
        Public Const FileReadError = "File Read Error"
        Public Const FileWriteError = "File Write Error"
        Public Const FilePermissionError = "File Permission Error"
        Public Const FileDiskFullError = "File Disk Full Error"
        Public Const FileAlreadyExists = "File Already Exists"
        Public Const FileFormatUnrecognized = "File Format Unrecognized"
        Public Const FileDataCorrupt = "File Data Corrupt"
        Public Const FileNamingNa = "File Naming NA"

        ' Directory errors
        Public Const DirNotFound = "Dir Not Found"
        Public Const DirIoError = "Dir IO Error"
        Public Const DirEntryNotFound = "Dir Entry Not Found"
        Public Const DirEntryExists = "Dir Entry Exists"
        Public Const DirNotEmpty = "Dir Not Empty"

        ' Property errors
        Public Const PropertiesUnavailable = "Properties Unavailable"
        Public Const PropertiesMismatch = "Properties Mismatch"
        Public Const PropertiesNotLoaded = "Properties Not Loaded"

        ' Function Parameter errors
        Public Const InvalidParameter = "Invalid Parameter"
        Public Const InvalidHandle = "Invalid Handle"
        Public Const InvalidPointer = "Invalid Pointer"
        Public Const InvalidIndex = "Invalid Index"
        Public Const InvalidLength = "Invalid Length"
        Public Const InvalidFnPointer = "Invalid Function Pointer"
        Public Const InvalidSortFn = "Invalid Sort Function"

        ' Device errors
        Public Const DeviceNotFound = "Device Not Found"
        Public Const DeviceBusy = "Device Busy"
        Public Const DeviceInvalid = "Device Invalid"
        Public Const DeviceEmergency = "Device Emergency"
        Public Const DeviceMemoryFull = "Device Memory Full"
        Public Const DeviceInternalError = "Device Internal Error"
        Public Const DeviceInvalidParameter = "Device Invalid Parameter"
        Public Const DeviceNoDisk = "Device No Disk"
        Public Const DeviceDiskError = "Device Disk Error"
        Public Const DeviceCfGateChanged = "Device CF Gate Changed"
        Public Const DeviceDialChanged = "Device Dial Changed"
        Public Const DeviceNotInstalled = "Device Not Installed"
        Public Const DeviceStayAwake = "Device Stay Awake"
        Public Const DeviceNotReleased = "Device Not Released"

        ' Stream errors
        Public Const StreamIoError = "Stream IO Error"
        Public Const StreamNotOpen = "Stream Not Open"
        Public Const StreamAlreadyOpen = "Stream Already Open"
        Public Const StreamOpenError = "Stream Open Error"
        Public Const StreamCloseError = "Stream Close Error"
        Public Const StreamSeekError = "Stream Seek Error"
        Public Const StreamTellError = "Stream Tell Error"
        Public Const StreamReadError = "Stream Read Error"
        Public Const StreamWriteError = "Stream Write Error"
        Public Const StreamPermissionError = "Stream Permission Error"
        Public Const StreamCouldntBeginThread = "Stream Couldn't Begin Thread"
        Public Const StreamBadOptions = "Stream Bad Options"
        Public Const StreamEndOfStream = "Stream End of Stream"

        ' Communications errors
        Public Const CommPortIsInUse = "Comm Port Is in Use"
        Public Const CommDisconnected = "Comm Disconnected"
        Public Const CommDeviceIncompatible = "Comm Device Incompatible"
        Public Const CommBufferFull = "Comm Buffer Full"
        Public Const CommUsbBusErr = "Comm USB Bus Err"

        ' Lock/Unlock
        Public Const UsbDeviceLockError = "USB Device Lock Error"
        Public Const UsbDeviceUnlockError = "USB Device Unlock Error"

        ' STI/WIA
        Public Const StiUnknownError = "STI Unknown Error"
        Public Const StiInternalError = "STI Internal Error"
        Public Const StiDeviceCreateError = "STI Device Create Error"
        Public Const StiDeviceReleaseError = "STI Device Release Error"
        Public Const DeviceNotLaunched = "Device Not Launched"

        Public Const EnumNa = "Enum NA"
        Public Const InvalidFnCall = "Invalid Function Call"
        Public Const HandleNotFound = "Handle Not Found"
        Public Const InvalidId = "Invalid ID"
        Public Const WaitTimeoutError = "Wait Timeout Error"

        ' PTP
        Public Const SessionNotOpen = "Session Not Open"
        Public Const InvalidTransactionid = "Invalid Transactionid"
        Public Const IncompleteTransfer = "Incomplete Transfer"
        Public Const InvalidStrageid = "Invalid Storage ID"
        Public Const DevicepropNotSupported = "Deviceprop Not Supported"
        Public Const InvalidObjectformatcode = "Invalid Object Format Code"
        Public Const SelfTestFailed = "Self Test Failed"
        Public Const PartialDeletion = "Partial Deletion"
        Public Const SpecificationByFormatUnsupported = "Specification by Format Unsupported"
        Public Const NoValidObjectinfo = "No Valid Object Info"
        Public Const InvalidCodeFormat = "Invalid Code Format"
        Public Const UnknownVenderCode = "Unknown Vender Code"
        Public Const CaptureAlreadyTerminated = "Capture Already Terminated"
        Public Const InvalidParentobject = "Invalid Parent Object"
        Public Const InvalidDevicepropFormat = "Invalid Deviceprop Format"
        Public Const InvalidDevicepropValue = "Invalid Deviceprop Value"
        Public Const SessionAlreadyOpen = "Session Already Open"
        Public Const TransactionCancelled = "Transaction Cancelled"
        Public Const SpecificationOfDestinationUnsupported = "Specification of Destination Unsupported"
        Public Const UnknownCommand = "Unknown Command"
        Public Const OperationRefused = "Operation Refused"
        Public Const LensCoverClose = "Lens Cover Close"
        Public Const LowBattery = "Low Battery"
        Public Const ObjectNotready = "Object Not Ready"

        Public Const TakePictureAfNg = "Take Picture AF NG"
        Public Const TakePictureReserved = "Take Picture Reserved"
        Public Const TakePictureMirrorUpNg = "Take Picture Mirror up NG"
        Public Const TakePictureSensorCleaningNg = "Take Picture Sensor Cleaning NG"
        Public Const TakePictureSilenceNg = "Take Picture Silence NG"
        Public Const TakePictureNoCardNg = "Take Picture No Card NG"
        Public Const TakePictureCardNg = "Take Picture Card NG"
        Public Const TakePictureCardProtectNg = "Take Picture Card Protect NG"

#End Region

    End Class

    Friend Class StructurePointer(Of T As Structure)
        Implements IDisposable

        Private m_Size As Integer 'in bytes
        Friend ReadOnly Property Size() As Integer
            Get
                Return m_Size
            End Get
        End Property

        Private m_ptr As IntPtr
        Friend ReadOnly Property Pointer() As IntPtr
            Get
                Return m_ptr
            End Get
        End Property

        Friend Property Value() As T
            Get
                Return Marshal.PtrToStructure(m_ptr, GetType(T))
            End Get
            Set(ByVal value As T)
                Marshal.StructureToPtr(value, m_ptr, True)
            End Set
        End Property

        Friend Sub New()
            m_Size = Marshal.SizeOf(GetType(T))
            m_ptr = Marshal.AllocHGlobal(m_Size)
        End Sub

#Region "IDisposable"
        Private m_disposed As Boolean = False ' To detect redundant calls
        Public Sub Dispose() Implements IDisposable.Dispose
            If Not m_disposed Then
                Marshal.FreeHGlobal(m_ptr)

                m_disposed = True
            End If
        End Sub
#End Region
    End Class

    ''' <summary>wrapper around EdsDirectoryItemRef</summary>
    Friend Class DirectoryItem
        Inherits ObjectWrapper

        Friend Sub New(ByVal ptr As IntPtr)
            MyBase.New(ptr)
        End Sub
    End Class

    Public Class GetOnlyCameraException
        Inherits Exception
    End Class
    Public Class NoCameraFoundException
        Inherits GetOnlyCameraException
    End Class
    Public Class TooManyCamerasFoundException
        Inherits GetOnlyCameraException
    End Class

    Public Class SdkException
        Inherits Exception

        Private m_Message As String

        Public ReadOnly Property SdkError() As String
            Get
                Return m_Message
            End Get
        End Property

        Public Sub New(ByVal errCode As Integer)
            m_Message = SdkErrors.StringFromErrorCode(errCode)
        End Sub

        Public Overrides ReadOnly Property Message() As String
            Get
                Return m_Message
            End Get
        End Property
    End Class

    Public Class InvalidCameraOperationException
        Inherits InvalidOperationException
    End Class

End Namespace
