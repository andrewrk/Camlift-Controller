Imports Klugesaurus.CanonCamera.Sdk
Imports System.Runtime.InteropServices

Module Module1
    Dim m_cam As IntPtr
    Private m_oeh As EdsObjectEventHandler
    Private m_seh As EdsStateEventHandler
    Private m_peh As EdsPropertyEventHandler

    Private Function StaticObjectEventHandler(ByVal inEvent As Integer, ByVal inRef As IntPtr, ByVal inContext As IntPtr) As Long
        Return 0
    End Function

    Private Function StaticStateEventHandler(ByVal inEvent As Integer, ByVal inParameter As Integer, ByVal inContext As IntPtr) As Long
        Return 0
    End Function

    Private Function StaticPropertyEventHandler(ByVal inEvent As Integer, ByVal inPropertyID As Integer, ByVal inParam As Integer, ByVal inContext As IntPtr) As Long
        Return 0
    End Function

    Private Sub LieToTheCameraAboutHowMuchSpaceWeHaveOnTheComputer()
        ' tell the camera how much disk space we have left
        Dim caps As EdsCapacity

        caps.reset = True
        caps.bytesPerSector = 512
        caps.numberOfFreeClusters = Marshal.SizeOf(GetType(Integer)) ' arbitrary large number
        EdsSetCapacity(m_cam, caps)

    End Sub


    Sub Main()
        Dim camList As IntPtr
        Dim numCams As Integer

        EdsInitializeSDK()
        EdsGetCameraList(camList)
        EdsGetChildCount(camList, numCams)
        EdsGetChildAtIndex(camList, 0, m_cam)
        'open a session
        EdsOpenSession(m_cam)

        ' handlers
        m_seh = New EdsStateEventHandler(AddressOf StaticStateEventHandler)
        EdsSetCameraStateEventHandler(m_cam, kEdsStateEvent_All, m_seh, New IntPtr(0))

        m_oeh = New EdsObjectEventHandler(AddressOf StaticObjectEventHandler)
        EdsSetObjectEventHandler(m_cam, kEdsObjectEvent_All, m_oeh, New IntPtr(0))

        m_peh = New EdsPropertyEventHandler(AddressOf StaticPropertyEventHandler)
        EdsSetPropertyEventHandler(m_cam, kEdsPropertyEvent_All, m_peh, New IntPtr(0))

        EdsSetPropertyData(m_cam, kEdsPropID_SaveTo, 0, Marshal.SizeOf(GetType(Integer)), CType(EdsSaveTo.kEdsSaveTo_Host, Integer))

        LieToTheCameraAboutHowMuchSpaceWeHaveOnTheComputer()
        LieToTheCameraAboutHowMuchSpaceWeHaveOnTheComputer()


    End Sub

End Module
