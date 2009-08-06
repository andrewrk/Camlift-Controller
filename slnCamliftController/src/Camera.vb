Imports VisionaryDigital.CanonCamera.Sdk
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Drawing
Imports System.IO


Public Class Camera
    Implements IDisposable

    Private m_cam As IntPtr
    Private m_oeh As EdsObjectEventHandler
    Private m_seh As EdsStateEventHandler
    Private m_peh As EdsPropertyEventHandler

    Private Shared s_instance As Camera = Nothing

    Private m_waitingOnPic As Boolean
    Private m_picOutFile As String

    ' live view
    Private Const LiveViewDelay = 200
    Private Const LiveViewFrameBufferSize = &H80000
    Private m_liveViewThread As Thread
    Private m_liveViewOn As Boolean
    Private m_waitingToStartLiveView As Boolean
    Private m_liveViewPicBox As PictureBox
    Private m_stoppingLiveView As Boolean
    Private m_liveViewFrameBuffer As Byte()
    Private m_liveViewBufferHandle As GCHandle
    Private m_liveViewStreamPtr As IntPtr
    Private m_zoomPosition As StructurePointer(Of EdsPoint)
    Private m_zoomRatio As StructurePointer(Of Integer)

    Private Const SleepTimeout = 20000 ' how many milliseconds to wait before giving up
    Private Const SleepAmount = 50 ' how many milliseconds to sleep before doing the event pump

    Private m_transferQueue As Queue(Of TransferItem)

    Public Sub New()
        If s_instance Is Nothing Then
            s_instance = Me
            m_waitingOnPic = False
            m_liveViewOn = False
            m_waitingToStartLiveView = False
            m_liveViewPicBox = Nothing
            m_liveViewThread = Nothing
            m_stoppingLiveView = False
            ReDim m_liveViewFrameBuffer(0)
            m_liveViewBufferHandle = Nothing
            m_liveViewStreamPtr = IntPtr.Zero
            m_transferQueue = New Queue(Of TransferItem)
            m_zoomPosition = New StructurePointer(Of EdsPoint)
            m_zoomRatio = New StructurePointer(Of Integer)

            CheckError(EdsInitializeSDK())
        Else
            Throw New OnlyOneInstanceAllowedException
        End If
    End Sub

    ''' <summary>
    ''' connect the computer to the camera. must be called before doing anything else.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub EstablishSession()
        Dim camList As IntPtr
        Dim numCams As Integer

        CheckError(EdsGetCameraList(camList))
        CheckError(EdsGetChildCount(camList, numCams))

        If numCams > 1 Then
            CheckError(EdsRelease(camList))
            Throw New TooManyCamerasFoundException
        ElseIf numCams = 0 Then
            CheckError(EdsRelease(camList))
            Throw New NoCameraFoundException
        End If

        'get the only camera
        CheckError(EdsGetChildAtIndex(camList, 0, m_cam))

        'release the camera list data
        CheckError(EdsRelease(camList))

        'open a session
        CheckError(EdsOpenSession(m_cam))

        ' handlers
        m_seh = New EdsStateEventHandler(AddressOf StaticStateEventHandler)
        CheckError(EdsSetCameraStateEventHandler(m_cam, kEdsStateEvent_All, m_seh, New IntPtr(0)))

        m_oeh = New EdsObjectEventHandler(AddressOf StaticObjectEventHandler)
        CheckError(EdsSetObjectEventHandler(m_cam, kEdsObjectEvent_All, m_oeh, New IntPtr(0)))

        m_peh = New EdsPropertyEventHandler(AddressOf StaticPropertyEventHandler)
        CheckError(EdsSetPropertyEventHandler(m_cam, kEdsPropertyEvent_All, m_peh, New IntPtr(0)))

        'set default options
        'save to computer, not memory card
        CheckError(EdsSetPropertyData(m_cam, kEdsPropID_SaveTo, 0, Marshal.SizeOf(GetType(Integer)), CType(EdsSaveTo.kEdsSaveTo_Host, Integer)))

        ' enforce JPEG format
        Dim qs As New StructurePointer(Of UInt32)
        CheckError(EdsGetPropertyData(m_cam, kEdsPropID_ImageQuality, 0, qs.Size, qs.Pointer))
        ' clear the old image type setting and set the new one
        qs.Value = qs.Value And &HFF0FFFFFL Or (EdsImageType.kEdsImageType_Jpeg << 20)
        CheckError(EdsSetPropertyData(m_cam, kEdsPropID_ImageQuality, 0, qs.Size, qs.Value))
    End Sub

    Private m_disposed As Boolean = False
    Public Sub Dispose() Implements System.IDisposable.Dispose
        If m_disposed Then Exit Sub
        m_disposed = True
        StopLiveView() 'stops it only if it's running

        'FlushTransferQueue()
        EdsCloseSession(m_cam)
        EdsRelease(m_cam)
        EdsTerminateSDK()

        s_instance = Nothing
    End Sub

    Private Sub CheckError(ByVal Err As Integer)
        ' throw errors if necessary
        If Err <> EDS_ERR_OK Then Throw New SdkException(Err)
    End Sub

    Private Shared Function StaticObjectEventHandler(ByVal inEvent As Integer, ByVal inRef As IntPtr, ByVal inContext As IntPtr) As Long
        'transfer from static to member
        s_instance.ObjectEventHandler(inEvent, inRef, inContext)
        Return 0
    End Function

    Private Shared Function StaticStateEventHandler(ByVal inEvent As Integer, ByVal inParameter As Integer, ByVal inContext As IntPtr) As Long
        'transfer from static to member
        s_instance.StateEventHandler(inEvent, inParameter, inContext)
        Return 0
    End Function

    Private Shared Function StaticPropertyEventHandler(ByVal inEvent As Integer, ByVal inPropertyID As Integer, ByVal inParam As Integer, ByVal inContext As IntPtr) As Long
        'transfer from static to member
        s_instance.PropertyEventHandler(inEvent, inPropertyID, inParam, inContext)
        Return 0
    End Function


    Private Sub ObjectEventHandler(ByVal inEvent As Integer, ByVal inRef As IntPtr, ByVal inContext As IntPtr)
        Select Case inEvent
            Case kEdsObjectEvent_DirItemRequestTransfer
                ' queue up the transfer request
                Dim transfer As New TransferItem
                transfer.sdkRef = inRef
                transfer.outFile = m_picOutFile
                m_transferQueue.Enqueue(transfer)

                m_waitingOnPic = False ' allow other thread to continue
            Case Else
                Debug.Print(String.Format("ObjectEventHandler: event {0}", inEvent))

        End Select

    End Sub

    Public Sub FlushTransferQueue()
        While m_transferQueue.Count > 0
            ' transfer the image in memory to disk
            Dim transfer As TransferItem = m_transferQueue.Dequeue()
            Dim dirItemInfo As EdsDirectoryItemInfo = Nothing
            Dim outStream As IntPtr

            CheckError(EdsGetDirectoryItemInfo(transfer.sdkRef, dirItemInfo))

            ' This creates the outStream that is used by EdsDownload to actually grab and write out the file.
            CheckError(EdsCreateFileStream(transfer.outFile, EdsFileCreateDisposition.kEdsFileCreateDisposition_CreateAlways, EdsAccess.kEdsAccess_Write, outStream))

            ' do the transfer
            CheckError(EdsDownload(transfer.sdkRef, dirItemInfo.size, outStream))
            CheckError(EdsDownloadComplete(transfer.sdkRef))
            CheckError(EdsRelease(outStream))
        End While
    End Sub

    Private Sub StateEventHandler(ByVal inEvent As Integer, ByVal inParameter As Integer, ByVal inContext As IntPtr)
        Debug.Print(String.Format("stateEventHandler: event {0}, parameter {1}", inEvent, inParameter))
    End Sub

    Private Sub PropertyEventHandler(ByVal inEvent As Integer, ByVal inPropertyID As Integer, ByVal inParam As Integer, ByVal inContext As IntPtr)
        Select Case inPropertyID
            Case kEdsPropID_Evf_OutputDevice
                If m_waitingToStartLiveView Then
                    'start live view thread
                    m_liveViewThread = New Thread(AddressOf UpdateLiveView)
                    m_liveViewThread.Start()

                    'save state
                    m_waitingToStartLiveView = False
                    m_liveViewOn = True
                End If
            Case Else
                Debug.Print("property event handler called, propid = " & inPropertyID)
        End Select

    End Sub

    '''<summary>snap a photo with the camera and write it to outfile</summary> 
    ''' <param name="OutFile">the file to save the picture to</param>
    Public Sub TakePicture(ByVal OutFile As String)
        Dim interuptingLiveView As Boolean = m_liveViewOn
        Dim lvBox As PictureBox = m_liveViewPicBox

        If m_waitingOnPic Or m_waitingToStartLiveView Then
            ' bad programmer. should have disabled user controls
            Throw New CameraIsBusyException
            Exit Sub
        End If

        If interuptingLiveView Then StopLiveView()

        ' set flag indicating we are waiting on a callback call
        m_waitingOnPic = True
        m_picOutFile = OutFile

        ' take a picture with the camera and save it to outfile
        Dim err As Integer = EdsSendCommand(m_cam, EdsCameraCommand.kEdsCameraCommand_TakePicture, 0)

        If err <> EDS_ERR_OK Then
            m_waitingOnPic = False
            CheckError(err)
        End If

        Dim I As Integer
        For I = 0 To SleepTimeout / SleepAmount
            System.Threading.Thread.Sleep(SleepAmount)
            Application.DoEvents()

            If Not m_waitingOnPic Then
                If interuptingLiveView Then StartLiveView(lvBox)
                Exit Sub 'success 
            End If
        Next I

        ' we never got a callback. throw an error
        If interuptingLiveView Then StartLiveView(lvBox)

        m_waitingOnPic = False
        Throw New TakePictureFailedException
    End Sub

    Private Sub StartBulb()
        Dim err As Integer

        CheckError(EdsSendStatusCommand(m_cam, EdsCameraStatusCommand.kEdsCameraStatusCommand_UILock, 0))

        err = EdsSendCommand(m_cam, EdsCameraCommand.kEdsCameraCommand_BulbStart, 0)

        ' call ui unlock if bulbstart fails
        If err <> EDS_ERR_OK Then
            EdsSendStatusCommand(m_cam, EdsCameraStatusCommand.kEdsCameraStatusCommand_UIUnLock, 0)
            CheckError(err)
        End If
    End Sub

    Private Sub StopBulb()
        Dim err As Integer, err2 As Integer

        ' call ui unlock even if bulb end fails
        err = EdsSendCommand(m_cam, EdsCameraCommand.kEdsCameraCommand_BulbEnd, 0)
        err2 = EdsSendCommand(m_cam, EdsCameraStatusCommand.kEdsCameraStatusCommand_UIUnLock, 0)

        CheckError(err)
        CheckError(err2)
    End Sub

    '''<summary>start streaming live video to pbox</summary>
    '''<param name="pbox">the picture box to send live video to</param>
    ''' <remarks>you can only have one live view going at a time.</remarks>
    Public Sub StartLiveView(ByVal pbox As PictureBox)
        While m_stoppingLiveView
            Application.DoEvents()
        End While

        If m_waitingToStartLiveView Then
            m_liveViewPicBox = pbox
            Return
        ElseIf m_liveViewOn Then
            StopLiveView()
        End If

        Dim device As New StructurePointer(Of UInt32)

        ' tell the camera to send live data to the computer
        CheckError(EdsGetPropertyData(m_cam, kEdsPropID_Evf_OutputDevice, 0, device.Size, device.Pointer))
        device.Value = device.Value Or EdsEvfOutputDevice.kEdsEvfOutputDevice_PC
        CheckError(EdsSetPropertyData(m_cam, kEdsPropID_Evf_OutputDevice, 0, device.Size, device.Value))

        ' get ready to stream
        m_liveViewPicBox = pbox
        m_waitingToStartLiveView = True

        ' set up buffer
        ReDim m_liveViewFrameBuffer(LiveViewFrameBufferSize)
        m_liveViewBufferHandle = GCHandle.Alloc(m_liveViewFrameBuffer, GCHandleType.Pinned)
        CheckError(EdsCreateMemoryStreamFromPointer(m_liveViewBufferHandle.AddrOfPinnedObject, LiveViewFrameBufferSize, m_liveViewStreamPtr))

        ' pause this thread until live view starts
        Dim I As Integer
        For I = 0 To SleepTimeout / SleepAmount
            System.Threading.Thread.Sleep(SleepAmount)
            Application.DoEvents()

            If Not m_waitingToStartLiveView Then Exit Sub 'success 
        Next I

        ' we never got a callback. throw an error
        StopLiveView()
        Throw New LiveViewFailedException

    End Sub

    ''' <summary>
    ''' stop streaming live video
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub StopLiveView()
        Dim device As New StructurePointer(Of UInt32)

        If m_stoppingLiveView Or (Not m_waitingToStartLiveView And Not m_liveViewOn) Then Exit Sub

        If m_liveViewOn Then
            ' stop thread
            m_stoppingLiveView = True
            m_liveViewThread.Join()
        End If

        ' save state
        m_liveViewOn = False
        m_waitingToStartLiveView = False
        m_liveViewPicBox = Nothing
        m_stoppingLiveView = False

        ' tell the camera not to send live data to the computer
        CheckError(EdsGetPropertyData(m_cam, kEdsPropID_Evf_OutputDevice, 0, device.Size, device.Pointer))
        device.Value = device.Value And Not EdsEvfOutputDevice.kEdsEvfOutputDevice_PC
        CheckError(EdsSetPropertyData(m_cam, kEdsPropID_Evf_OutputDevice, 0, device.Size, device.Value))

        ' clean up
        CheckError(EdsRelease(m_liveViewStreamPtr))
        m_liveViewBufferHandle.Free()
    End Sub

    Private Sub UpdateLiveView()
        Dim nowPlusInterval As Long

        While Not m_stoppingLiveView
            nowPlusInterval = Now.Ticks + LiveViewDelay
            ShowLiveViewFrame()
            Thread.Sleep(Math.Max(nowPlusInterval - Now.Ticks, 0))
        End While
    End Sub

    Private Sub ShowLiveViewFrame()
        Dim err As Integer = 0
        Dim imagePtr As IntPtr

        ' create image
        CheckError(EdsCreateEvfImageRef(m_liveViewStreamPtr, imagePtr))

        ' download the frame
        Try
            CheckError(EdsDownloadEvfImage(m_cam, imagePtr))
        Catch ex As SdkException When ex.SdkError = SdkErrors.ObjectNotready
            CheckError(EdsRelease(imagePtr))
            Exit Sub
        End Try
        ' get incidental data
        ' zoom ratio
        CheckError(EdsGetPropertyData(imagePtr, kEdsPropID_Evf_Zoom, 0, m_zoomRatio.Size, m_zoomRatio.Pointer))
        ' zoom position
        CheckError(EdsGetPropertyData(imagePtr, kEdsPropID_Evf_ZoomPosition, 0, m_zoomPosition.Size, m_zoomPosition.Pointer))

        ' get it into the picture box image
        Dim canonImg As Image = Image.FromStream(New MemoryStream(m_liveViewFrameBuffer)) 'do not dispose the MemoryStream (Image.FromStream)
        Dim oldImg As Image = m_liveViewPicBox.Image
        m_liveViewPicBox.Image = canonImg
        If oldImg IsNot Nothing Then oldImg.Dispose() 'really is required.

        ' release image
        CheckError(EdsRelease(imagePtr))

    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

    Public ReadOnly Property ZoomPosition() As Point
        Get
            Return New Point(m_zoomPosition.Value.x, m_zoomPosition.Value.y)
        End Get
    End Property

    Public ReadOnly Property ZoomRatio() As Integer
        Get
            Return m_zoomRatio.Value
        End Get
    End Property

    Private Class StructurePointer(Of T As Structure)

        Private m_Size As Integer 'in bytes
        Private m_ptr As IntPtr

        Public ReadOnly Property Size() As Integer
            Get
                Return m_Size
            End Get
        End Property

        Public ReadOnly Property Pointer() As IntPtr
            Get
                Return m_ptr
            End Get
        End Property

        Public Property Value() As T
            Get
                Return Marshal.PtrToStructure(m_ptr, GetType(T))
            End Get
            Set(ByVal value As T)
                Marshal.StructureToPtr(value, m_ptr, True)
            End Set
        End Property

        Public Sub New()
            m_Size = Marshal.SizeOf(GetType(T))
            m_ptr = Marshal.AllocHGlobal(m_Size)
        End Sub

        Protected Overrides Sub Finalize()
            Marshal.FreeHGlobal(m_ptr)
        End Sub
    End Class

    Private Structure TransferItem
        Public sdkRef As IntPtr
        Public outFile As String
    End Structure

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
        m_dict.Add(EDS_ERR_UNIMPLEMENTED, Unimplemented)
        m_dict.Add(EDS_ERR_INTERNAL_ERROR, InternalError)
        m_dict.Add(EDS_ERR_MEM_ALLOC_FAILED, MemAllocFailed)
        m_dict.Add(EDS_ERR_MEM_FREE_FAILED, MemFreeFailed)
        m_dict.Add(EDS_ERR_OPERATION_CANCELLED, OperationCancelled)
        m_dict.Add(EDS_ERR_INCOMPATIBLE_VERSION, IncompatibleVersion)
        m_dict.Add(EDS_ERR_NOT_SUPPORTED, NotSupported)
        m_dict.Add(EDS_ERR_UNEXPECTED_EXCEPTION, UnexpectedException)
        m_dict.Add(EDS_ERR_PROTECTION_VIOLATION, ProtectionViolation)
        m_dict.Add(EDS_ERR_MISSING_SUBCOMPONENT, MissingSubcomponent)
        m_dict.Add(EDS_ERR_SELECTION_UNAVAILABLE, SelectionUnavailable)

        ' File errors
        m_dict.Add(EDS_ERR_FILE_IO_ERROR, FileIoError)
        m_dict.Add(EDS_ERR_FILE_TOO_MANY_OPEN, FileTooManyOpen)
        m_dict.Add(EDS_ERR_FILE_NOT_FOUND, FileNotFound)
        m_dict.Add(EDS_ERR_FILE_OPEN_ERROR, FileOpenError)
        m_dict.Add(EDS_ERR_FILE_CLOSE_ERROR, FileCloseError)
        m_dict.Add(EDS_ERR_FILE_SEEK_ERROR, FileSeekError)
        m_dict.Add(EDS_ERR_FILE_TELL_ERROR, FileTellError)
        m_dict.Add(EDS_ERR_FILE_READ_ERROR, FileReadError)
        m_dict.Add(EDS_ERR_FILE_WRITE_ERROR, FileWriteError)
        m_dict.Add(EDS_ERR_FILE_PERMISSION_ERROR, FilePermissionError)
        m_dict.Add(EDS_ERR_FILE_DISK_FULL_ERROR, FileDiskFullError)
        m_dict.Add(EDS_ERR_FILE_ALREADY_EXISTS, FileAlreadyExists)
        m_dict.Add(EDS_ERR_FILE_FORMAT_UNRECOGNIZED, FileFormatUnrecognized)
        m_dict.Add(EDS_ERR_FILE_DATA_CORRUPT, FileDataCorrupt)
        m_dict.Add(EDS_ERR_FILE_NAMING_NA, FileNamingNa)

        ' Directory errors
        m_dict.Add(EDS_ERR_DIR_NOT_FOUND, DirNotFound)
        m_dict.Add(EDS_ERR_DIR_IO_ERROR, DirIoError)
        m_dict.Add(EDS_ERR_DIR_ENTRY_NOT_FOUND, DirEntryNotFound)
        m_dict.Add(EDS_ERR_DIR_ENTRY_EXISTS, DirEntryExists)
        m_dict.Add(EDS_ERR_DIR_NOT_EMPTY, DirNotEmpty)

        ' Property errors
        m_dict.Add(EDS_ERR_PROPERTIES_UNAVAILABLE, PropertiesUnavailable)
        m_dict.Add(EDS_ERR_PROPERTIES_MISMATCH, PropertiesMismatch)
        m_dict.Add(EDS_ERR_PROPERTIES_NOT_LOADED, PropertiesNotLoaded)

        ' Function Parameter errors
        m_dict.Add(EDS_ERR_INVALID_PARAMETER, InvalidParameter)
        m_dict.Add(EDS_ERR_INVALID_HANDLE, InvalidHandle)
        m_dict.Add(EDS_ERR_INVALID_POINTER, InvalidPointer)
        m_dict.Add(EDS_ERR_INVALID_INDEX, InvalidIndex)
        m_dict.Add(EDS_ERR_INVALID_LENGTH, InvalidLength)
        m_dict.Add(EDS_ERR_INVALID_FN_POINTER, InvalidFnPointer)
        m_dict.Add(EDS_ERR_INVALID_SORT_FN, InvalidSortFn)

        ' Device errors
        m_dict.Add(EDS_ERR_DEVICE_NOT_FOUND, DeviceNotFound)
        m_dict.Add(EDS_ERR_DEVICE_BUSY, DeviceBusy)
        m_dict.Add(EDS_ERR_DEVICE_INVALID, DeviceInvalid)
        m_dict.Add(EDS_ERR_DEVICE_EMERGENCY, DeviceEmergency)
        m_dict.Add(EDS_ERR_DEVICE_MEMORY_FULL, DeviceMemoryFull)
        m_dict.Add(EDS_ERR_DEVICE_INTERNAL_ERROR, DeviceInternalError)
        m_dict.Add(EDS_ERR_DEVICE_INVALID_PARAMETER, DeviceInvalidParameter)
        m_dict.Add(EDS_ERR_DEVICE_NO_DISK, DeviceNoDisk)
        m_dict.Add(EDS_ERR_DEVICE_DISK_ERROR, DeviceDiskError)
        m_dict.Add(EDS_ERR_DEVICE_CF_GATE_CHANGED, DeviceCfGateChanged)
        m_dict.Add(EDS_ERR_DEVICE_DIAL_CHANGED, DeviceDialChanged)
        m_dict.Add(EDS_ERR_DEVICE_NOT_INSTALLED, DeviceNotInstalled)
        m_dict.Add(EDS_ERR_DEVICE_STAY_AWAKE, DeviceStayAwake)
        m_dict.Add(EDS_ERR_DEVICE_NOT_RELEASED, DeviceNotReleased)

        ' Stream errors
        m_dict.Add(EDS_ERR_STREAM_IO_ERROR, StreamIoError)
        m_dict.Add(EDS_ERR_STREAM_NOT_OPEN, StreamNotOpen)
        m_dict.Add(EDS_ERR_STREAM_ALREADY_OPEN, StreamAlreadyOpen)
        m_dict.Add(EDS_ERR_STREAM_OPEN_ERROR, StreamOpenError)
        m_dict.Add(EDS_ERR_STREAM_CLOSE_ERROR, StreamCloseError)
        m_dict.Add(EDS_ERR_STREAM_SEEK_ERROR, StreamSeekError)
        m_dict.Add(EDS_ERR_STREAM_TELL_ERROR, StreamTellError)
        m_dict.Add(EDS_ERR_STREAM_READ_ERROR, StreamReadError)
        m_dict.Add(EDS_ERR_STREAM_WRITE_ERROR, StreamWriteError)
        m_dict.Add(EDS_ERR_STREAM_PERMISSION_ERROR, StreamPermissionError)
        m_dict.Add(EDS_ERR_STREAM_COULDNT_BEGIN_THREAD, StreamCouldntBeginThread)
        m_dict.Add(EDS_ERR_STREAM_BAD_OPTIONS, StreamBadOptions)
        m_dict.Add(EDS_ERR_STREAM_END_OF_STREAM, StreamEndOfStream)

        ' Communications errors
        m_dict.Add(EDS_ERR_COMM_PORT_IS_IN_USE, CommPortIsInUse)
        m_dict.Add(EDS_ERR_COMM_DISCONNECTED, CommDisconnected)
        m_dict.Add(EDS_ERR_COMM_DEVICE_INCOMPATIBLE, CommDeviceIncompatible)
        m_dict.Add(EDS_ERR_COMM_BUFFER_FULL, CommBufferFull)
        m_dict.Add(EDS_ERR_COMM_USB_BUS_ERR, CommUsbBusErr)

        ' Lock/Unlock
        m_dict.Add(EDS_ERR_USB_DEVICE_LOCK_ERROR, UsbDeviceLockError)
        m_dict.Add(EDS_ERR_USB_DEVICE_UNLOCK_ERROR, UsbDeviceUnlockError)

        ' STI/WIA
        m_dict.Add(EDS_ERR_STI_UNKNOWN_ERROR, StiUnknownError)
        m_dict.Add(EDS_ERR_STI_INTERNAL_ERROR, StiInternalError)
        m_dict.Add(EDS_ERR_STI_DEVICE_CREATE_ERROR, StiDeviceCreateError)
        m_dict.Add(EDS_ERR_STI_DEVICE_RELEASE_ERROR, StiDeviceReleaseError)
        m_dict.Add(EDS_ERR_DEVICE_NOT_LAUNCHED, DeviceNotLaunched)

        m_dict.Add(EDS_ERR_ENUM_NA, EnumNa)
        m_dict.Add(EDS_ERR_INVALID_FN_CALL, InvalidFnCall)
        m_dict.Add(EDS_ERR_HANDLE_NOT_FOUND, HandleNotFound)
        m_dict.Add(EDS_ERR_INVALID_ID, InvalidId)
        m_dict.Add(EDS_ERR_WAIT_TIMEOUT_ERROR, WaitTimeoutError)

        ' PTP
        m_dict.Add(EDS_ERR_SESSION_NOT_OPEN, SessionNotOpen)
        m_dict.Add(EDS_ERR_INVALID_TRANSACTIONID, InvalidTransactionid)
        m_dict.Add(EDS_ERR_INCOMPLETE_TRANSFER, IncompleteTransfer)
        m_dict.Add(EDS_ERR_INVALID_STRAGEID, InvalidStrageid)
        m_dict.Add(EDS_ERR_DEVICEPROP_NOT_SUPPORTED, DevicepropNotSupported)
        m_dict.Add(EDS_ERR_INVALID_OBJECTFORMATCODE, InvalidObjectformatcode)
        m_dict.Add(EDS_ERR_SELF_TEST_FAILED, SelfTestFailed)
        m_dict.Add(EDS_ERR_PARTIAL_DELETION, PartialDeletion)
        m_dict.Add(EDS_ERR_SPECIFICATION_BY_FORMAT_UNSUPPORTED, SpecificationByFormatUnsupported)
        m_dict.Add(EDS_ERR_NO_VALID_OBJECTINFO, NoValidObjectinfo)
        m_dict.Add(EDS_ERR_INVALID_CODE_FORMAT, InvalidCodeFormat)
        m_dict.Add(EDS_ERR_UNKNOWN_VENDER_CODE, UnknownVenderCode)
        m_dict.Add(EDS_ERR_CAPTURE_ALREADY_TERMINATED, CaptureAlreadyTerminated)
        m_dict.Add(EDS_ERR_INVALID_PARENTOBJECT, InvalidParentobject)
        m_dict.Add(EDS_ERR_INVALID_DEVICEPROP_FORMAT, InvalidDevicepropFormat)
        m_dict.Add(EDS_ERR_INVALID_DEVICEPROP_VALUE, InvalidDevicepropValue)
        m_dict.Add(EDS_ERR_SESSION_ALREADY_OPEN, SessionAlreadyOpen)
        m_dict.Add(EDS_ERR_TRANSACTION_CANCELLED, TransactionCancelled)
        m_dict.Add(EDS_ERR_SPECIFICATION_OF_DESTINATION_UNSUPPORTED, SpecificationOfDestinationUnsupported)
        m_dict.Add(EDS_ERR_UNKNOWN_COMMAND, UnknownCommand)
        m_dict.Add(EDS_ERR_OPERATION_REFUSED, OperationRefused)
        m_dict.Add(EDS_ERR_LENS_COVER_CLOSE, LensCoverClose)
        m_dict.Add(EDS_ERR_LOW_BATTERY, LowBattery)
        m_dict.Add(EDS_ERR_OBJECT_NOTREADY, ObjectNotready)

        m_dict.Add(EDS_ERR_TAKE_PICTURE_AF_NG, TakePictureAfNg)
        m_dict.Add(EDS_ERR_TAKE_PICTURE_RESERVED, TakePictureReserved)
        m_dict.Add(EDS_ERR_TAKE_PICTURE_MIRROR_UP_NG, TakePictureMirrorUpNg)
        m_dict.Add(EDS_ERR_TAKE_PICTURE_SENSOR_CLEANING_NG, TakePictureSensorCleaningNg)
        m_dict.Add(EDS_ERR_TAKE_PICTURE_SILENCE_NG, TakePictureSilenceNg)
        m_dict.Add(EDS_ERR_TAKE_PICTURE_NO_CARD_NG, TakePictureNoCardNg)
        m_dict.Add(EDS_ERR_TAKE_PICTURE_CARD_NG, TakePictureCardNg)
        m_dict.Add(EDS_ERR_TAKE_PICTURE_CARD_PROTECT_NG, TakePictureCardProtectNg)

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

Public Class GetOnlyCameraException
    Inherits Exception
End Class
Public Class NoCameraFoundException
    Inherits GetOnlyCameraException
End Class
Public Class TooManyCamerasFoundException
    Inherits GetOnlyCameraException
End Class

Public Class OnlyOneInstanceAllowedException
    Inherits Exception
End Class

Public Class TakePictureFailedException
    Inherits Exception
End Class
Public Class CameraIsBusyException
    Inherits Exception
End Class
Public Class LiveViewFailedException
    Inherits Exception
End Class