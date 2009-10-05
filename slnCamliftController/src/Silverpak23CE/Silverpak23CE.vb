
'===============================================================================
'=====================          Silverpak23CE.vb          ======================
'=====================       Version 1.1  8/20/2008       ======================
'=====================  Copyright 2008 Visionary Digital  ======================
'=====================  http://www.visionarydigital.com/  ======================
'=====================          Visual Basic 9.0          ======================
'===============================================================================
'
'This document provides functionality for interfacing with a Lin Engineering 
'Silverpak23CE stepper motor connected locally to a COM port. Documentation for 
'this namespace and its members is primarilly integrated into xml comments 
'accessible through IntelliSense. 
'
'For further information, see README.txt


Imports System.IO.Ports
Imports System.Threading
Imports System.ComponentModel

Namespace Silverpak23CE
    'Public classes
    '''<summary>Provides an interface to a Lin Engineering Silverpak23CE stepper motor</summary>
    <ToolboxItem(False)> _
    Public Class SilverpakManager
        Inherits Component

        'Public Fields
        '''<summary>The default value for the Acceleration property.</summary>
        Public Const DefaultAcceleration As Integer = 500
        '''<summary>The default value for the BaudRate property.</summary>
        Public Const DefaultBaudRate As Integer = -1
        '''<summary>The default value for the DriverAddress property.</summary>
        Public Const DefaultDriverAddress As DriverAddresses = DriverAddresses.Unknown
        '''<summary>The default value for the EncoderRatio property.</summary>
        Public Const DefaultEncoderRatio As Integer = 10266
        '''<summary>The default value for the HoldingCurrent property.</summary>
        Public Const DefaultHoldingCurrent As Integer = 5
        '''<summary>The default value for the HomePolarity property.</summary>
        Public Const DefaultHomePolarity As Integer = 0
        '''<summary>The default value for the MaxPosition property.</summary>
        Public Const DefaultMaxPosition As Integer = 500000
        '''<summary>The default value for the MotorPolarity property.</summary>
        Public Const DefaultMotorPolarity As Integer = 1
        '''<summary>The default value for the Portname property.</summary>
        Public Const DefaultPortname As String = ""
        '''<summary>The default value for the PositionCorrectionRetries property.</summary>
        Public Const DefaultPositionCorrectionRetries As Integer = 5
        '''<summary>The default value for the PositionCorrectionTolerance property.</summary>
        Public Const DefaultPositionCorrectionTolerance As Integer = 5
        '''<summary>The default value for the PositionUpdaterInterval property.</summary>
        Public Const DefaultPositionUpdaterInterval As Integer = 200
        '''<summary>The default value for the RunningCurrent property.</summary>
        Public Const DefaultRunningCurrent As Integer = 50
        '''<summary>The default value for the Velocity property.</summary>
        Public Const DefaultVelocity As Integer = 230000

        'Public Properties
        '''<summary>Gets or sets the acceleration for the motor. Call ResendMotorSettings() to apply any changes to this property.</summary>
        Public Property Acceleration() As Integer
            Get
                Return m_acceleration
            End Get
            Set(ByVal value As Integer)
                m_acceleration = value
            End Set
        End Property
        '''<summary>Gets the baud rate this SilverpakManager is connected through, or sets the baud rate prior to connection</summary>
        Public Property BaudRate() As Integer
            Get
                Return m_baudRate
            End Get
            Set(ByVal value As Integer)
                m_baudRate = value
            End Set
        End Property
        '''<summary>Gets the driver address this SilverpakManager is connected through, or sets the driver address prior to connection</summary>
        Public Property DriverAddress() As DriverAddresses
            Get
                Return m_driverAddress
            End Get
            Set(ByVal value As DriverAddresses)
                m_driverAddress = value
            End Set
        End Property
        '''<summary>Gets or sets the encoder ratio for the motor. Call ResendMotorSettings() to apply any changes to this property.</summary>
        Public Property EncoderRatio() As Integer
            Get
                Return m_encoderRatio
            End Get
            Set(ByVal value As Integer)
                m_encoderRatio = value
            End Set
        End Property
        '''<summary>Gets or sets the holding current for the motor. Call ResendMotorSettings() to apply any changes to this property.</summary>
        Public Property HoldingCurrent() As Integer
            Get
                Return m_holdingCurrent
            End Get
            Set(ByVal value As Integer)
                m_holdingCurrent = value
            End Set
        End Property
        '''<summary>Gets or sets the home polarity for the motor. Call ResendMotorSettings() to apply any changes to this property.</summary>
        Public Property HomePolarity() As Integer
            Get
                Return m_homePolarity
            End Get
            Set(ByVal value As Integer)
                m_homePolarity = value
            End Set
        End Property
        '''<summary>Returns a value indicating whether this SilverpakManager is actively connected to a Silverpak23CE.</summary>
        Public Overridable ReadOnly Property IsActive() As Boolean
            Get
                SyncLock m_motor_lock
                    Return m_motorState_motor <> MotorStates.Disconnected
                End SyncLock
            End Get
        End Property
        '''<summary>Returns a value indicating whether the motor is ready to accept a command (i.e. connected, initialized, and stopped).</summary>
        Public Overridable ReadOnly Property IsReady() As Boolean
            Get
                SyncLock m_motor_lock
                    Return (m_motorState_motor = MotorStates.Ready)
                End SyncLock
            End Get
        End Property
        '''<summary>Gets or sets the expected maximum position the motor will ever reach. This value is the positive limit to the GoInfinite() command.</summary>
        Public Property MaxPosition() As Integer
            Get
                Return m_maxPosition
            End Get
            Set(ByVal value As Integer)
                m_maxPosition = value
            End Set
        End Property
        '''<summary>Gets or sets the motor polarity for the motor. Call ResendMotorSettings() to apply any changes to this property.</summary>
        Public Property MotorPolarity() As Integer
            Get
                Return m_motorPolarity
            End Get
            Set(ByVal value As Integer)
                m_motorPolarity = value
            End Set
        End Property
        '''<summary>Gets the port name this SilverpakManager is connected through, or sets the port name prior to connection</summary>
        Public Property PortName() As String
            Get
                Return m_portName
            End Get
            Set(ByVal value As String)
                m_portName = value
            End Set
        End Property
        '''<summary>Returns the position of the motor.</summary>
        Public Overridable ReadOnly Property Position() As Integer
            Get
                Return m_position
            End Get
        End Property
        '''<summary>Gets or sets the position correction retries for the motor. Call ResendMotorSettings() to apply any changes to this property.</summary>
        Public Property PositionCorrectionRetries() As Integer
            Get
                Return m_positionCorrectionRetries
            End Get
            Set(ByVal value As Integer)
                m_positionCorrectionRetries = value
            End Set
        End Property
        '''<summary>Gets or sets the position correction tolerance for the motor. Call ResendMotorSettings() to apply any changes to this property.</summary>
        Public Property PositionCorrectionTolerance() As Integer
            Get
                Return m_positionCorrectionTolerance
            End Get
            Set(ByVal value As Integer)
                m_positionCorrectionTolerance = value
            End Set
        End Property
        '''<summary>Gets or sets the interval in milliseconds between position update queries by the automatic position updater.</summary>
        Public Property PositionUpdaterInterval() As Integer
            Get
                Return m_positionUpdaterInterval
            End Get
            Set(ByVal value As Integer)
                m_positionUpdaterInterval = value
            End Set
        End Property
        '''<summary>Gets or sets the running current for the motor. Call ResendMotorSettings() to apply any changes to this property.</summary>
        Public Property RunningCurrent() As Integer
            Get
                Return m_runningCurrent
            End Get
            Set(ByVal value As Integer)
                m_runningCurrent = value
            End Set
        End Property
        '''<summary>Gets or sets the maximum velocity for the motor. Call ResendMotorSettings() to apply any changes to this property.</summary>
        Public Property Velocity() As Integer
            Get
                Return m_velocity
            End Get
            Set(ByVal value As Integer)
                m_velocity = value
            End Set
        End Property

        ''' <summary>Gets or sets a callback delegate for handling unexpected exceptions on internal threads.</summary>
        Public Shared Property ErrorCallback() As Action(Of Exception)
            Get
                Return s_errorCallback
            End Get
            Set(ByVal value As Action(Of Exception))
                s_errorCallback = value
            End Set
        End Property


        'Public Events
        '''<summary>Raised when the connection to the Silverpak23CE is lost.</summary>
        Public Event ConnectionLost As EventHandler
        ''''<summary>Raised once the InitializeCoordinates() command has completed without being interrupted.</summary>
        'Public Event CoordinatesInitialized As EventHandler
        ''''<summary>Raised when the InitializeCoordinates() command is aborted by calling the StopMotor() method.</summary>
        'Public Event CoordinatesInitializationAborted(ByVal sender As Object, ByVal e As EventArgs)
        '''<summary>Raised when the motor stops moving.</summary>
        Public Event StoppedMoving As EventHandler(Of StoppedMovingEventArgs)
        '''<summary>Raised when the motor's position changes. Read the Position property to get the new position.</summary>
        Public Event PositionChanged As EventHandler


        'Public Constructors
        '''<summary>Creates a new instance of the SilverpakManager class. This overload is provided for Windows.Forms Class Composition Designer support.</summary>
        Public Sub New(ByVal container As System.ComponentModel.IContainer)
            MyClass.New()
            If container IsNot Nothing Then container.Add(Me) 'Required for Windows.Forms Class Composition Designer support
        End Sub
        '''<summary>Creates a new instance of the SilverpakManager class.</summary>
        Public Sub New()
            MyBase.New()
            m_components = New Container()
            m_connectionManager_motor = New SilverpakConnectionManager(m_components)
        End Sub


        'Public methods
        '''<summary>Attempts to connect to a Silverpak23CE. 
        ''' The PortName, BaudRate, and DriverAddress properties must be set or an ArgumentException will be thrown. 
        ''' To auto-detect these properties, see FindAndConnect(). 
        ''' The IsActive property must return False when calling this method or an InvalidSilverpakOperationException will be thrown.</summary>
        Public Overridable Function Connect() As Boolean
            SyncLock m_motor_lock
                'Validate state and connection properties
                If m_motorState_motor <> MotorStates.Disconnected Then Throw New InvalidSilverpakOperationException("Connection is already active. Make sure the IsActive property returns False before calling this method.")
                If m_portName = DefaultPortname Then Throw New ArgumentException("PortName property must be set before calling this method. See also FindAndConnect().")
                If m_baudRate = DefaultBaudRate Then Throw New ArgumentException("BaudRate property must be set before calling this method. See also FindAndConnect().")
                If m_driverAddress = DefaultDriverAddress Then Throw New ArgumentException("DriverAddress property must be set before calling this method. See also FindAndConnect().")

                'Initialize connection manager's properties
                m_connectionManager_motor.PortName = m_portName
                m_connectionManager_motor.BaudRate = m_baudRate
                m_connectionManager_motor.DriverAddress = m_driverAddress
                'Attempt to connect
                If m_connectionManager_motor.Connect() Then
                    'Connection succeeded
                    m_motorState_motor = MotorStates.Connected
                    Return True
                Else
                    'Connection failed
                    Return False
                End If
            End SyncLock
        End Function

        '''<summary>Attempts to find and connect to a Silverpak23CE. 
        ''' If any of the properties PortName, BaudRate, and DriverAddress are set to their defaults, all of their possible values will be searched. 
        ''' After a successful connection, these properties will be set to their discovered values. 
        ''' The IsActive property must return False when calling this method or an InvalidSilverpakOperationException will be thrown.</summary>
        Public Overridable Function FindAndConnect() As Boolean
            SyncLock m_motor_lock
                'Validate state
                If m_motorState_motor <> MotorStates.Disconnected Then Throw New InvalidSilverpakOperationException("Connection is already active. Make sure the IsActive property returns False before calling this method.")

                'Get information for all COM ports being searched
                Dim portInfos() As PortInformation = SearchComPorts(m_portName, m_baudRate, m_driverAddress)
                'Search the list of information for an available Silverpak23CE
                For Each iPI As PortInformation In portInfos
                    If iPI.PortStatus = PortStatuses.AvailableSilverpak Then
                        'Listed available Silverpak23CE found
                        'Initialize connection manager's properties
                        m_connectionManager_motor.PortName = iPI.PortName
                        m_connectionManager_motor.BaudRate = iPI.BaudRate
                        m_connectionManager_motor.DriverAddress = iPI.DriverAddress
                        'Attempt to connect
                        If m_connectionManager_motor.Connect() Then 'This should only evaluate to Flase in the event that the Silverpak23CE was disconnected between the call to SearchComPorts and now
                            'Connection succeeded
                            'Save connection properties
                            m_portName = iPI.PortName
                            m_baudRate = iPI.BaudRate
                            m_driverAddress = iPI.DriverAddress
                            m_motorState_motor = MotorStates.Connected
                            Return True
                        End If 'In the rare occasion that block is skipped, try the next iPI
                    End If
                Next
                'End of list was reached and no available Silverpak23CE was found
                Return False
            End SyncLock
        End Function

        '''<summary>Initialization Step 1. 
        ''' This method will set the motor settings as specified by this SilverpakManager's properties.
        ''' This method does not cause the motor to move.
        ''' The next step is InitializeSmoothMotion().
        ''' Calling this method out of order will throw an InvalidSilverpakOperationException.</summary>
        Public Overridable Sub InitializeMotorSettings()
            SyncLock m_motor_lock
                'Validate state
                If m_motorState_motor <> MotorStates.Connected Then Throw New InvalidSilverpakOperationException("Initialization methods must be called in the proper order.")

                'Send settings-initialization command
                m_connectionManager_motor.Write(GenerateMessage(m_driverAddress, generateFullInitCommandList()), 4.0!)
                'Update state
                m_motorState_motor = MotorStates.InitializedSettings
            End SyncLock
        End Sub

        '''<summary>Call this method if any changes to the motor settings properties need to be applied.
        ''' This method does not cause the motor to move.
        ''' Calling this method will throw an InvalidSilverpakOperationException if the IsActive property returns False or if the motor is moving.</summary>
        Public Overridable Sub ResendMotorSettings()
            SyncLock m_motor_lock
                'Validate state
                If m_motorState_motor = MotorStates.Disconnected Then Throw New InvalidSilverpakOperationException("Connection is not active.")
                If m_motorState_motor = MotorStates.InitializingCoordinates_moveToZero Or _
                   m_motorState_motor = MotorStates.InitializingCoordinates_calibrateHome Or _
                   m_motorState_motor = MotorStates.Moving Then Throw New InvalidSilverpakOperationException("Cannot resend motor settings while the motor is moving.")

                'Send settings command
                m_connectionManager_motor.Write(GenerateMessage(m_driverAddress, generateResendInitCommandList()), 4.0!)
            End SyncLock
        End Sub

        '''<summary>Initialization Step 2. 
        ''' This method will send a small motion command five times to bypass any initialization quirks that some motors are prone to exhibit.
        ''' This method causes the motor to move up to 5 microsteps in the positive direction and causes the motor to briefly produce a rapid tapping sound.
        ''' The next step is InitializeCoordinates().
        ''' Calling this method out of order will throw an InvalidSilverpakOperationException.</summary>
        Public Overridable Sub InitializeSmoothMotion()
            SyncLock m_motor_lock
                'Validate state
                If m_motorState_motor <> MotorStates.InitializedSettings Then Throw New InvalidSilverpakOperationException("Initialization methods must be called in the proper order.")

                'Send a small motion command five times
                For i As Integer = 1 To 5
                    Static s_smoothMotionInitMsg As String = GenerateMessage(m_driverAddress, GenerateCommand(Commands.GoPositive, "1"))
                    m_connectionManager_motor.Write(s_smoothMotionInitMsg, 3.0!)
                Next
                'Update state
                m_motorState_motor = MotorStates.InitializedSmoothMotion
            End SyncLock
        End Sub

        '''<summary>Initialization Step 3. 
        ''' This method will send the motor looking for its upper limit switch so it can zero its coordinate system.
        ''' This method causes the motor to move in the negative direction until it trips the upper limit switch.
        ''' The next initialization step is to wait for the CoordinatesInitialized event or to wait for the IsReady property to return True.
        ''' Calling this method out of order will throw an InvalidSilverpakOperationException.</summary>
        Public Overridable Sub InitializeCoordinates()
            SyncLock m_motor_lock
                'Validate state
                If m_motorState_motor <> MotorStates.InitializedSmoothMotion Then Throw New InvalidSilverpakOperationException("Initialization methods must be called in the proper order.")

                moveToZero()
            End SyncLock
            'Now that the motor is moving, begin listening for position changes
            startPositionUpdater()
        End Sub
        Private Sub moveToZero()
            'move to zero in preparation for home calibration
            Dim cmd As String = GenerateCommand(Commands.SetPosition, CInt(m_maxPosition * (m_encoderRatio / 1000)))
            cmd &= GenerateCommand(Commands.SetEncoderRatio, m_encoderRatio)
            cmd &= GenerateCommand(Commands.GoAbsolute, 0)
            Dim message = GenerateMessage(m_driverAddress, cmd)
            m_connectionManager_motor.Write(message, 2.0!)

            'Update state
            m_motorState_motor = MotorStates.InitializingCoordinates_moveToZero
        End Sub

        '''<summary>Sends the motor either to position 0 or to the position specified by the MaxPosition property.
        ''' Calling this method before the motor has been fully initialized will throw an InvalidSilverpakOperationException.</summary>
        Public Sub GoInfinite(ByVal positive As Boolean)
            GoToPosition(If(positive, m_maxPosition, 0))
        End Sub

        '''<summary>Stops the motor.
        ''' Calling this method when the IsActive property returns False will throw an InvalidSilverpakOperationException.</summary>
        Public Overridable Sub StopMotor()
            SyncLock m_motor_lock
                'Validate state
                If m_motorState_motor = MotorStates.Disconnected Then Throw New InvalidSilverpakOperationException("Connection is not active.")

                'Send stop command
                Static s_stopMessage As String = GenerateMessage(m_driverAddress, GenerateCommand(Commands.TerminateCommand))
                m_connectionManager_motor.Write(s_stopMessage, 1.0!)
                'Update state if applicable
                If m_motorState_motor = MotorStates.InitializingCoordinates_moveToZero Or _
                   m_motorState_motor = MotorStates.InitializingCoordinates_calibrateHome Then
                    m_motorState_motor = MotorStates.AbortingCoordinateInitialization
                End If
            End SyncLock
        End Sub

        '''<summary>Sends the motor to the passed position.
        ''' Calling this method before the motor has been fully initialized will throw an InvalidSilverpakOperationException.</summary>
        Public Overridable Sub GoToPosition(ByVal position As Integer)
            SyncLock m_motor_lock
                'Validate state
                If Not (m_motorState_motor = MotorStates.Ready Or m_motorState_motor = MotorStates.Moving) Then
                    Throw New InvalidSilverpakOperationException("Motor is not fully initialized")
                End If

                'Send absolute motion command
                m_connectionManager_motor.Write(GenerateMessage(m_driverAddress, GenerateCommand(Commands.GoAbsolute, position)), 1.0!)
                'Update state
                m_motorState_motor = MotorStates.Moving
            End SyncLock
        End Sub

        '''<summary>Terminates the connection to the Silverpak23CE and closes the COM port.
        ''' Calling this method will throw an InvalidSilverpakOperationException if the IsActive property returns False or if the motor is moving.</summary>
        Public Overridable Sub Disconnect()
            SyncLock m_motor_lock
                'Validate state
                If m_motorState_motor = MotorStates.Disconnected Then Throw New InvalidSilverpakOperationException("Connection is not active.")
                If m_motorState_motor = MotorStates.InitializingCoordinates_moveToZero Or _
                   m_motorState_motor = MotorStates.InitializingCoordinates_calibrateHome Or _
                   m_motorState_motor = MotorStates.Moving Then
                    Throw New InvalidSilverpakOperationException("Disconnecting while the motor is moving is not allowed.")
                End If

                'Disconnect
                m_connectionManager_motor.Disconnect()
                'Update state
                m_motorState_motor = MotorStates.Disconnected
            End SyncLock
        End Sub



        'Private Fields

        ''' <summary>Contains sup-components.</summary>
        Private m_components As System.ComponentModel.IContainer

        ''' <summary>Field behind the Acceleration property.</summary>
        Private m_acceleration As Integer = DefaultAcceleration
        ''' <summary>Field behind the BaudRate property.</summary>
        Private m_baudRate As Integer = DefaultBaudRate
        ''' <summary>Field behind the DriverAddress property.</summary>
        Private m_driverAddress As DriverAddresses = DefaultDriverAddress
        ''' <summary>Field behind the EncoderRatio property.</summary>
        Private m_encoderRatio As Integer = DefaultEncoderRatio
        ''' <summary>Field behind the HoldingCurrent property.</summary>
        Private m_holdingCurrent As Integer = DefaultHoldingCurrent
        ''' <summary>Field behind the HomePolarity property.</summary>
        Private m_homePolarity As Integer = DefaultHomePolarity
        ''' <summary>Field behind the MaxPosition property.</summary>
        Private m_maxPosition As Integer = DefaultMaxPosition
        ''' <summary>Field behind the MotorPolarity property.</summary>
        Private m_motorPolarity As Integer = DefaultMotorPolarity
        ''' <summary>Field behind the Portname property.</summary>
        Private m_portName As String = DefaultPortname
        ''' <summary>Field behind the PositionCorrectionRetries property.</summary>
        Private m_positionCorrectionRetries As Integer = DefaultPositionCorrectionRetries
        ''' <summary>Field behind the PositionCorrectionTolerance property.</summary>
        Private m_positionCorrectionTolerance As Integer = DefaultPositionCorrectionTolerance
        ''' <summary>Field behind the PositionUpdaterInterval property.</summary>
        Private m_positionUpdaterInterval As Integer = DefaultPositionUpdaterInterval
        ''' <summary>Field behind the RunningCurrent property.</summary>
        Private m_runningCurrent As Integer = DefaultRunningCurrent
        ''' <summary>Field behind the Velocity property.</summary>
        Private m_velocity As Integer = DefaultVelocity

        ''' <summary>Field behind the ErrorCallback property.</summary>
        Private Shared s_errorCallback As Action(Of Exception)

        ''' <summary>Field behind the Position property.</summary>
        Private m_position As Integer = 0

        'Fields in the SyncLock group: motor
        ''' <summary>Lock object for the SyncLock group: motor.</summary>
        Private m_motor_lock As New Object
        ''' <summary>Connection manager component. Part of the SyncLock group: motor.</summary>
        Private m_connectionManager_motor As SilverpakConnectionManager
        ''' <summary>The present state of the motor. Part of the SyncLock group: motor.</summary>
        Private m_motorState_motor As MotorStates = MotorStates.Disconnected

        'Fields in the SyncLock group: posUpd
        ''' <summary>Lock object for the SyncLock group: posUpd.</summary>
        Private m_posUpd_lock As New Object 'SyncLock object for this group
        ''' <summary>Used to cancel the position updater thread. Part of the SyncLock group: posUpd.</summary>
        Private m_keepPositionUpdaterRunning_posUpd As Boolean
        ''' <summary>Thread that periodically gets the position of the motor. Part of the SyncLock group: posUpd.</summary>
        Private m_positionUpdaterThread_posUpd As New Thread(AddressOf positionUpdater_run)

        'Event raisers
        ''' <summary>Method that raises the event ConnectionLost.</summary>
        Protected Sub OnConnectionLost()
            RaiseEvent ConnectionLost(Me, New EventArgs)
        End Sub
        ''' <summary>Method that raises the event CoordinatesInitializationAborted.</summary>
        Protected Sub OnCoordinatesInitializationAborted()
            RaiseEvent StoppedMoving(Me, New StoppedMovingEventArgs(StoppedMovingReason.InitializationAborted))
        End Sub
        ''' <summary>Method that raises the event CoordinatesInitialized.</summary>
        Protected Sub OnCoordinatesInitialized()
            RaiseEvent StoppedMoving(Me, New StoppedMovingEventArgs(StoppedMovingReason.Initialized))
        End Sub
        ''' <summary>Method that raises the event PositionChanged.</summary>
        Protected Sub OnPositionChanged()
            RaiseEvent PositionChanged(Me, New EventArgs)
        End Sub
        ''' <summary>Method that raises the event StoppedMoving.</summary>
        Protected Sub OnStoppedMoving()
            RaiseEvent StoppedMoving(Me, New StoppedMovingEventArgs(StoppedMovingReason.Normal))
        End Sub

        'Private methods
        ''' <summary>Disposes this component.</summary>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso m_components IsNot Nothing Then
                    m_components.Dispose()
                    stopPositionUpdater()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        ''' <summary>Makes sure the position updater thread is running.</summary>
        Private Sub startPositionUpdater()
            SyncLock m_posUpd_lock
                m_keepPositionUpdaterRunning_posUpd = True 'make sure the position updater thread doesn't cancel
                If Not m_positionUpdaterThread_posUpd.IsAlive Then 'only activate it when it's not active
                    If m_positionUpdaterThread_posUpd.ThreadState = ThreadState.Stopped Then 'if it's previously completed running
                        m_positionUpdaterThread_posUpd = New Thread(AddressOf positionUpdater_run) 'reinstantiate the thread
                    End If
                    m_positionUpdaterThread_posUpd.Start() 'start the thread
                End If
            End SyncLock
        End Sub

        ''' <summary>Stops the position updater thread and makes sure it dies.</summary>
        Private Sub stopPositionUpdater()
            If Thread.CurrentThread Is m_positionUpdaterThread_posUpd Then
                'the position updater thread cannot stop itself; a thread can never see itself die.
                Dim t As New Thread(AddressOf stopPositionUpdater_not_positionUpdaterThread)
                t.Start() 'stop the position updater thread on a seperate thread
            Else
                stopPositionUpdater_not_positionUpdaterThread()
            End If
        End Sub
        ''' <summary>Stops the position updater thread and makes sure it dies. This method cannot be called on the position updater thread.</summary>
        Private Sub stopPositionUpdater_not_positionUpdaterThread()
            Try
                SyncLock m_posUpd_lock
                    m_keepPositionUpdaterRunning_posUpd = False 'cancel the position updater
                    If m_positionUpdaterThread_posUpd IsNot Nothing Then
                        Dim timeoutTime As Integer = Environment.TickCount + 1000
                        While m_positionUpdaterThread_posUpd.IsAlive
                            If Environment.TickCount >= timeoutTime Then Exit While
                        End While 'make sure the position updater thread dies before releasing the SyncLock
                    End If
                End SyncLock
            Catch ex As Exception
                invokeErrorCallback(ex)
            End Try
        End Sub

        ''' <summary>Method that the position getter thread runs.</summary>
        Private Sub positionUpdater_run()
            Try
                While m_keepPositionUpdaterRunning_posUpd 'check for cancelation
                    'Keep time according to Environment.TickCount
                    Dim nextIterationTime As Integer = Environment.TickCount + m_positionUpdaterInterval
                    'Update postion
                    updatePosition()
                    'Wait for the next iteration time
                    Thread.Sleep(Math.Max(0, nextIterationTime - Environment.TickCount))
                End While
            Catch ex As Exception
                invokeErrorCallback(ex)
            End Try
        End Sub
        ''' <summary>Updates the Position property by querying the position of the motor.</summary>
        Private Sub updatePosition()
            Dim callbackAction As Action = Nothing 'store a function to call after the SyncLock has been released
            SyncLock m_motor_lock
                Static s_getPositionCmd As String = GenerateCommand(Commands.QueryMotorPosition)
                Dim getPositionMessage As String = GenerateMessage(m_driverAddress, s_getPositionCmd)
                Dim newPosition As Integer = Integer.MinValue 'make sure we know whether it's been set by starting it at an unlikely value
                Dim response As String
                Static s_homeCalibrationSteps As Integer
                Try
                    response = m_connectionManager_motor.WriteAndGetResponse(getPositionMessage, 1.0!)
                Catch ex As InvalidSilverpakOperationException 'the SilverpakManager's been disconnected
                    callbackAction = New Action(AddressOf stopPositionUpdater) 'shut down updater thread
                    GoTo ExitAndCallback
                End Try
                'Serial Port is still active
                If response IsNot Nothing AndAlso IsNumeric(response) Then
                    'Got a valid response
                    newPosition = response
                End If
                Static s_failCount As Integer = 0 'track the number of times we didn't receive a valid response
                If m_position = newPosition Then
                    'motor stopped moving
                    s_failCount = 0
                    Select Case m_motorState_motor
                        Case MotorStates.InitializingCoordinates_moveToZero
                            'wait! sometimes the motor will stop at 5000000 and lie about being at the top (stupid old firmware)
                            If Math.Abs(m_position - 5000000) < 100 Then
                                moveToZero()
                            Else
                                m_motorState_motor = MotorStates.InitializingCoordinates_calibrateHome
                                'Send the homing message
                                Dim initCoordMessage As String = GenerateMessage(m_driverAddress, GenerateCommand(Commands.GoHome, CInt(m_maxPosition * (m_encoderRatio / 1000))))
                                m_connectionManager_motor.Write(initCoordMessage, 1.0!)
                                s_homeCalibrationSteps = 0
                            End If
                        Case MotorStates.InitializingCoordinates_calibrateHome
                            m_motorState_motor = MotorStates.Ready
                            callbackAction = New Action(AddressOf OnCoordinatesInitialized)
                        Case MotorStates.AbortingCoordinateInitialization
                            m_motorState_motor = MotorStates.InitializedSmoothMotion
                            callbackAction = New Action(AddressOf OnCoordinatesInitializationAborted)
                        Case MotorStates.Moving
                            m_motorState_motor = MotorStates.Ready
                            callbackAction = New Action(AddressOf OnStoppedMoving)
                    End Select
                ElseIf newPosition <> Integer.MinValue Then
                    'motor changed position
                    s_failCount = 0
                    m_position = newPosition
                    callbackAction = New Action(AddressOf OnPositionChanged)
                    'make sure the home calibration isn't sneaking away
                    If m_motorState_motor = MotorStates.InitializingCoordinates_calibrateHome Then
                        s_homeCalibrationSteps += 1
                        If s_homeCalibrationSteps > 5 Then
                            'Calling shenanigans on initialization
                            'stop the motor damnit
                            For i = 0 To 3 - 1
                                Static s_stopMessage As String = GenerateMessage(m_driverAddress, GenerateCommand(Commands.TerminateCommand))
                                m_connectionManager_motor.Write(s_stopMessage, 1.0!)
                            Next
                            MsgBox("Motor shenanigans detected! This is a quirk resulting from using outdated motor firmware." & vbCrLf & "Please restart the program.", MsgBoxStyle.Critical, MsgBoxTitle)
                            End ' crash program
                        End If
                    End If
                    Else
                        'failed to get a valid position
                        s_failCount += 1
                        If s_failCount >= 5 Then
                            'failed 5 times in a row. Silverpak23CE must no longer be available.
                            s_failCount = 0
                            'disconnect
                            m_motorState_motor = MotorStates.Disconnected
                            m_connectionManager_motor.Disconnect()
                            'raise LostConnection event
                            callbackAction = New Action(AddressOf OnConnectionLost)
                        End If
                    End If
            End SyncLock
ExitAndCallback:
            'invoke callback sub if any
            If callbackAction IsNot Nothing Then callbackAction.Invoke()
        End Sub

        ''' <summary>Produces a command list to initialize the motor from scratch.</summary>
        Private Function generateFullInitCommandList() As String
            Static s_initMotorSettingsProgramHeader As String = GenerateCommand(Commands.SetPosition, "0")
            Static s_initMotorSettingsProgramFooter As String = GenerateCommand(Commands.SetMode, "10") 'Position Correction + Optical Limit Switches

            Return s_initMotorSettingsProgramHeader & generateResendInitCommandList() & s_initMotorSettingsProgramFooter
        End Function
        ''' <summary>Produces a command list to set the adjustable motor settings.</summary>
        Private Function generateResendInitCommandList() As String
            Return GenerateCommand(Commands.SetHoldCurrent, m_holdingCurrent) _
                & GenerateCommand(Commands.SetRunningCurrent, m_runningCurrent) _
                & GenerateCommand(Commands.SetMotorPolarity, m_motorPolarity) _
                & GenerateCommand(Commands.SetHomePolarity, m_homePolarity) _
                & GenerateCommand(Commands.SetPositionCorrectionTolerance, m_positionCorrectionTolerance) _
                & GenerateCommand(Commands.SetPositionCorrectionRetries, m_positionCorrectionRetries) _
                & GenerateCommand(Commands.SetEncoderRatio, "1000") _
                & GenerateCommand(Commands.SetVelocity, m_velocity) _
                & GenerateCommand(Commands.SetAcceleration, m_acceleration) _
                & GenerateCommand(Commands.SetEncoderRatio, m_encoderRatio)
        End Function

        ''' <summary>Invokes the ErrorCalback delegate if it has been set. Otherwise, re-throws the exception so that the program crashes.</summary>
        Private Shared Sub invokeErrorCallback(ByVal ex As Exception)
            If s_errorCallback IsNot Nothing Then s_errorCallback.Invoke(ex) Else Throw ex
        End Sub

    End Class

    ''' <summary> The exception that is thrown when a method call in namespace Silverpak23CE is invalid for the object's current state. </summary>
    Public Class InvalidSilverpakOperationException
        Inherits InvalidOperationException
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(ByVal message As String, ByVal innerException As Exception)
            MyBase.New(message, innerException)
        End Sub
        Protected Sub New(ByVal info As Runtime.Serialization.SerializationInfo, ByVal context As Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub
    End Class

    '''<summary>Represents a collection of data for reporting the status of a COM port</summary>
    Public Class PortInformation
        '''<summary>The name of the COM port</summary>
        Public Property PortName() As String
            Get
                Return m_portName
            End Get
            Set(ByVal value As String)
                m_portName = value
            End Set
        End Property
        '''<summary>The baud rate of the COM port</summary>
        Public Property BaudRate() As Integer
            Get
                Return m_baudRate
            End Get
            Set(ByVal value As Integer)
                m_baudRate = value
            End Set
        End Property
        '''<summary>The status of the COM port</summary>
        Public Property PortStatus() As PortStatuses
            Get
                Return m_portStatus
            End Get
            Set(ByVal value As PortStatuses)
                m_portStatus = value
            End Set
        End Property
        '''<summary>The driver address of the active Silverpak23CE if there is one</summary>
        Public Property DriverAddress() As DriverAddresses
            Get
                Return m_driverAddress
            End Get
            Set(ByVal value As DriverAddresses)
                m_driverAddress = value
            End Set
        End Property

        ''' <summary>Field behind the PortName property</summary>
        Private m_portName As String
        ''' <summary>Field behind the BaudRate property</summary>
        Private m_baudRate As Integer
        ''' <summary>Field behind the PortStatu property</summary>
        Private m_portStatus As PortStatuses
        ''' <summary>Field behind the DriverAddress property</summary>
        Private m_driverAddress As DriverAddresses
    End Class

    Public Class StoppedMovingEventArgs
        Inherits EventArgs
        Private m_reason
        ''' <summary>The reason that the motor stopped moving.</summary>
        Public Property Reason() As StoppedMovingReason
            Get
                Return m_reason
            End Get
            Set(ByVal value As StoppedMovingReason)
                m_reason = value
            End Set
        End Property

        Public Sub New(ByVal reason As StoppedMovingReason)
            m_reason = reason
        End Sub
    End Class

    'Public enums
    '''<summary>Represents a driver address.</summary>
    Public Enum DriverAddresses As Byte
        Unknown = 0
        Driver1 = Asc("1")
        Driver2 = Asc("2")
        Driver3 = Asc("3")
        Driver4 = Asc("4")
        Driver5 = Asc("5")
        Driver6 = Asc("6")
        Driver7 = Asc("7")
        Driver8 = Asc("8")
        Driver9 = Asc("9")
        DriverA = Asc(":")
        DriverB = Asc(";")
        DriverC = Asc("<")
        DriverD = Asc("=")
        DriverE = Asc(">")
        DriverF = Asc("?")
        Driver0 = Asc("@")
        Drivers1And2 = Asc("A")
        Drivers3And4 = Asc("C")
        Drivers5And6 = Asc("E")
        Drivers7And8 = Asc("G")
        Drivers9And10 = Asc("I")
        Drivers11And12 = Asc("K")
        Drivers13And14 = Asc("M")
        Drivers15And16 = Asc("O")
        Drivers1And2And3And4 = Asc("Q")
        Drivers5And6And7And8 = Asc("U")
        Drivers9And10And11And12 = Asc("Y")
        Drivers13And14And15And16 = Asc("]")
        AllDrivers = Asc("_")
    End Enum

    '''<summary>Represents the status of a COM port.</summary>
    Public Enum PortStatuses
        '''<summary>Indicates that there is an active, available Silverpak on this COM port</summary>
        AvailableSilverpak
        '''<summary>Indicates that this COM port does not have an active Silverpak</summary>
        Empty
        '''<summary>Indicates that this COM port could not be read from or written to</summary>
        Invalid
        '''<summary>Indicates that this COM port is already open by another resource</summary>
        Busy
    End Enum

    '''<summary>Represents the reason that the motor stopped moving.</summary>
    Public Enum StoppedMovingReason
        '''<summary>The motor stopped after a GoInfinite() or GoToPosition() command.</summary>
        Normal
        '''<summary>The InitializeCoordinates() command has completed without being interrupted.</summary>
        Initialized
        '''<summary>The InitializeCoordinates() command is aborted by calling the StopMotor() method.</summary>
        InitializationAborted
    End Enum

    'Friend classes
    ''' <summary>Manages the connection to a Silverpak23CE through a serial port.</summary>
    <ToolboxItem(False)> _
    Friend Class SilverpakConnectionManager
        Inherits Component

        'Public fields
        ''' <summary>The command string for a safe query.</summary>
        Public Shared ReadOnly SafeQueryCommandStr As String = GenerateCommand(m_safeQueryCommand, m_safeQueryOperand)
        ''' <summary>The delay factor for a safe query.</summary>
        Public Const SafeQueryDelayFactor As Single = 3.0!
        ''' <summary>The minimum amount of time in milliseconds to wait for the Silverpak23CE to respond to a command.</summary>
        Public Const PortDelayUnit As Integer = 50

        'Public properties
        ''' <summary>The name of the COM port to connect to.</summary>
        Public Property PortName() As String
            Get
                Return m_portName
            End Get
            Set(ByVal value As String)
                m_portName = value
            End Set
        End Property
        ''' <summary>The baud rate of the COM port to connect to.</summary>
        Public Property BaudRate() As Integer
            Get
                Return m_baudRate
            End Get
            Set(ByVal value As Integer)
                m_baudRate = value
            End Set
        End Property
        ''' <summary>The driver address of the Silverpak23CE to connect to.</summary>
        Public Property DriverAddress() As DriverAddresses
            Get
                Return m_driverAddress
            End Get
            Set(ByVal value As DriverAddresses)
                m_driverAddress = value
            End Set
        End Property

        'Public constructors
        '''<summary>Creates a new instance of the SilverpakConnectionManager class. This overload is provided for Windows.Forms Class Composition Designer support.</summary>
        Public Sub New(ByVal container As System.ComponentModel.IContainer)
            MyClass.New()
            If container IsNot Nothing Then container.Add(Me) 'Required for Windows.Forms Class Composition Designer support
        End Sub
        '''<summary>Creates a new instance of the SilverpakConnectionManager class.</summary>
        Public Sub New()
            MyBase.New()
            components = New Container()
            m_serialPortInterface_srlPort = InitializeSerialPort(New SerialPort(components))
        End Sub

        'Public methods
        ''' <summary>Attempts to connect to a Silverpak23CE using the PortName, BaudRate, and DriverAddress properties. 
        ''' Returns True if successful.
        ''' Throws an InvalidSilverpakOperationException if already connected.</summary>
        Public Function Connect() As Boolean
            SyncLock m_srlPort_lock
                'Validate SerialPort state
                If m_serialPortInterface_srlPort.IsOpen Then Throw New InvalidSilverpakOperationException("Already connected.")
                Try 'Catch all expected exceptions
                    'apply serial port settings
                    m_serialPortInterface_srlPort.PortName = m_portName
                    m_serialPortInterface_srlPort.BaudRate = m_baudRate
                    'Attempt to connect
                    m_serialPortInterface_srlPort.Open()
                    'Check for a Silverpak23CE
                    Dim response As String = writeAndGetResponse_srlPort(GenerateMessage(m_driverAddress, SafeQueryCommandStr), SafeQueryDelayFactor)
                    If (response IsNot Nothing) Then
                        Return True
                    Else
                        closeSerialPort_srlPort()
                        Return False
                    End If
                Catch ex As ArgumentOutOfRangeException '.BaudRate
                Catch ex As ArgumentNullException '.PortName
                Catch ex As ArgumentException '.PortName
                Catch ex As UnauthorizedAccessException '.Open
                Catch ex As IO.IOException '.Write (called from within writeAndGetResponse_srlPort())
                End Try
                'Failed to connect. Make sure the SerialPort is closed
                closeSerialPort_srlPort()
                Return False
            End SyncLock
        End Function
        '''<summary>Makes sure there is no active connection to a Silverpak23CE.</summary>
        Public Sub Disconnect()
            SyncLock m_srlPort_lock
                closeSerialPort_srlPort()
            End SyncLock
        End Sub

        '''<summary>Writes the passed complete message to the Silverpak23CE.
        ''' Throws an InvalidSilverpakOperationException if not connected.</summary>
        ''' <param name="completeMessage">Recommended use generateMessage() to generate this parameter.</param>
        ''' <param name="delayFactor">How long the the Silverpak23CE is expected to take to process the message, 
        ''' expressed as a multiple of PortDelatUnit, typically in the range 1.0 to 3.0.</param>
        Public Sub Write(ByVal completeMessage As String, ByVal delayFactor As Single)
            SyncLock m_srlPort_lock
                'Validate state
                If Not m_serialPortInterface_srlPort.IsOpen Then Throw New InvalidSilverpakOperationException()
                'write message
                write_srlPort(completeMessage, delayFactor)
            End SyncLock
        End Sub

        '''<summary>Writes the passed message to and returns the body of the response from the Silverpak23CE.
        ''' If no response was received, returns Nothing.
        ''' Throws an InvalidSilverpakOperationException if not connected.</summary>
        ''' <param name="completeMessage">Recommended use generateMessage() to generate this parameter.</param>
        ''' <param name="delayFactor">How long the the Silverpak23CE is expected to take to process the message, 
        ''' expressed as a multiple of PortDelatUnit, typically in the range 1.0 to 3.0.</param>
        Public Function WriteAndGetResponse(ByVal completeMessage As String, ByVal delayFactor As Single) As String
            SyncLock m_srlPort_lock
                'Validate state
                If Not m_serialPortInterface_srlPort.IsOpen Then Throw New InvalidSilverpakOperationException()
                'write messag and get response
                Return writeAndGetResponse_srlPort(completeMessage, delayFactor)
            End SyncLock
        End Function


        'Private fields
        ''' <summary>Contains sup-components.</summary>
        Private components As System.ComponentModel.IContainer

        ''' <summary>Field behind the PortName property.</summary>
        Private m_portName As String
        ''' <summary>Field behind the BaudRate property.</summary>
        Private m_baudRate As Integer
        ''' <summary>Field behind the DriverAddress property.</summary>
        Private m_driverAddress As DriverAddresses

        'Fields in the SyncLock group: srlPort
        ''' <summary>Lock object for the SyncLock group: srlPort.</summary>
        Private m_srlPort_lock As New Object
        ''' <summary>The SerialPort object used to communicate with a Silverpak23CE. Part of the SyncLock group: srlPort.</summary>
        Private m_serialPortInterface_srlPort As SerialPort

        ''' <summary>The command for a safe query.</summary>
        Private Const m_safeQueryCommand As Commands = Commands.QueryControllerStatus
        ''' <summary>The operand for a safe query.</summary>
        Private Const m_safeQueryOperand As String = ""

        'Private methods
        ''' <summary>Disposes this component.</summary>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        Private Sub closeSerialPort_srlPort()
            If m_serialPortInterface_srlPort.IsOpen Then
                Try
                    m_serialPortInterface_srlPort.Close() 'Close the serial port.
                Catch 'Ignore any exceptions that occure while closing.
                End Try
            End If
        End Sub


        '''<summary>Writes the passed message to and returns the body of the response from the Silverpak23CE.
        ''' If no response was received, returns Nothing.
        ''' Part of the SyncLock group: srlPort.</summary>
        ''' <param name="completeMessage">Recommended use generateMessage() to generate this parameter.</param>
        ''' <param name="delayFactor">How long the the Silverpak23CE is expected to take to process the message, 
        ''' expressed as a multiple of PortDelatUnit, typically in the range 1.0 to 3.0.</param>
        Private Function writeAndGetResponse_srlPort(ByVal completeMessage As String, ByVal delayFactor As Single) As String
            safeReadExisting_srlPort(0.0!) 'Clear the read buffer.
            safeWrite_srlPort(completeMessage, delayFactor) 'Write the message.
            Dim totalRx As String = "" 'accumulates chunks of RX data
            Do 'Read the response from the Silverpak23CE in chunks until the accumulated message is complete.
                Dim rxStr As String = safeReadExisting_srlPort(1.0!) 'Read a chunk.
                If rxStr Is Nothing OrElse rxStr = "" Then Return Nothing 'If nothing came through, return nothing in lieu of an infinite loop.
                totalRx &= rxStr 'Append chunk to accumulated RX data.
            Loop While Not IsRxDataComplete(totalRx) 'check to see if the accumulated RX data is complete
            Dim trimResponse As String = TrimRxData(totalRx) 'Trim the RX data. Garunteed to succeed because IsRxDataComplete(totalRx) returned True
            Return trimResponse.Substring(1) 'Return only the return data (not the Status Char).
        End Function

        '''<summary>Writes the passed message to the Silverpak23CE.
        ''' Part of the SyncLock group: srlPort.</summary>
        ''' <param name="completeMessage">Recommended use generateMessage() to generate this parameter.</param>
        ''' <param name="delayFactor">How long the the Silverpak23CE is expected to take to process the message, 
        ''' expressed as a multiple of PortDelatUnit, typically in the range 1.0 to 3.0.</param>
        Private Sub write_srlPort(ByVal completeMessage As String, ByVal delayFactor As Single)
            safeWrite_srlPort(completeMessage, delayFactor)
        End Sub

        '''<summary>Reads the existing data on the read buffer from the Silverpak23CE after calling waitForSafeReadWrite_srlPort.
        ''' In the event of an unexcepted exception from SerialPort.ReadExisting(), returns Nothing.
        ''' Part of the SyncLock group: srlPort.</summary>
        ''' <param name="delayFactor">How long to wait after reading from the Silverpak23CE,
        ''' expressed as a multiple of PortDelatUnit, typically 1.0.</param>
        Private Function safeReadExisting_srlPort(ByVal delayFactor As Single) As String
            'wait for safe read/write
            waitForSafeReadWrite_srlPort(delayFactor)
            Try 'catch any undocumented exceptions from SerialPort.ReadExisting()
                Return m_serialPortInterface_srlPort.ReadExisting
            Catch
                Return Nothing
            End Try
        End Function

        '''<summary>Writes the passed message to the Silverpak23CE after calling waitForSafeReadWrite_srlPort.
        ''' Catches all exceptions from SerialPort.Write().
        ''' Part of the SyncLock group: srlPort.</summary>
        ''' <param name="completeMessage">Recommended use generateMessage() to generate this parameter.</param>
        ''' <param name="delayFactor">How long the the Silverpak23CE is expected to take to process the message, 
        ''' expressed as a multiple of PortDelatUnit, typically in the range 1.0 to 3.0.</param>
        Private Sub safeWrite_srlPort(ByVal completeMessage As String, ByVal delayFactor As Single)
            'wait for safe read/write
            waitForSafeReadWrite_srlPort(delayFactor)
            Try 'catch any undocumented exceptions from SerialPort.Write()
                m_serialPortInterface_srlPort.Write(completeMessage)
            Catch
            End Try
        End Sub

        ''' <summary>Waits until the time passed by the last call to this method passes.
        ''' Part of the SyncLock group: srlPort.</summary>
        ''' <param name="incrementFactor">How long to wait after this call to this method,
        ''' expressed as a multiple of PortDelatUnit, typically 1.0.</param>
        Private Sub waitForSafeReadWrite_srlPort(ByVal incrementFactor As Single)
            Static s_nextReadWriteTime As Integer = Environment.TickCount + incrementFactor * PortDelayUnit  'stores the next time that interaction with the Silverpak23CE is safe
            'wait until s_nextReadWriteTime
            Thread.Sleep(Math.Max(0, s_nextReadWriteTime - Environment.TickCount))
            'increment s_nextReadWriteTime by ReadWriteInterval number of milliseconds
            s_nextReadWriteTime = Environment.TickCount + PortDelayUnit * incrementFactor
        End Sub
    End Class

    'Friend modules
    '''<summary>Consts and Functions for internal use</summary>
    Friend Module SilverpakUtils

        ''' <summary>The beginning of a sent message to a Silverpak23CE.</summary>
        Friend Const DTProtocolTxStartStr As String = "/"
        ''' <summary>The end of a sent message to a Silverpak23CE.</summary>
        Friend Const DTProtocolTxEndStr As String = "R" & vbCr
        ''' <summary>The beginning of a received message from a Silverpak23CE.</summary>
        Friend Const DTProtocolRxStartStr As String = "/0"
        ''' <summary>The end of a received message from a Silverpak23CE.</summary>
        Friend Const DTProtocolRxEndStr As String = Chr(3)

        ''' <summary>DataBits setting for operating a Silverpak23CE over a serial port.</summary>
        Friend Const DTProtocolComDataBits As Integer = 8
        ''' <summary>Parity setting for operating a Silverpak23CE over a serial port.</summary>
        Friend Const DTProtocolComParity As IO.Ports.Parity = IO.Ports.Parity.None
        ''' <summary>StopBits setting for operating a Silverpak23CE over a serial port.</summary>
        Friend Const DTProtocolComStopBits As IO.Ports.StopBits = IO.Ports.StopBits.One
        ''' <summary>Handshake setting for operating a Silverpak23CE over a serial port.</summary>
        Friend Const DTProtocolComHandshake As IO.Ports.Handshake = IO.Ports.Handshake.None

        ''' <summary>Returns a complete message to write to the Silverpak23CE.</summary>
        ''' <param name="commandList">Recommended use GenerateCommand() to generate this parameter. Multiple commands can be concatenated and passed as this argument.</param>
        Public Function GenerateMessage(ByVal recipient As DriverAddresses, ByVal commandList As String) As String
            Return DTProtocolTxStartStr & GetDriverAddressStr(recipient) & commandList & DTProtocolTxEndStr
        End Function
        '''<summary>Returns a command to pass to GenerateMessage()</summary>
        Public Function GenerateCommand(ByVal cmnd As Commands, Optional ByVal operand As String = "") As String
            Return GetCommandStr(cmnd) & operand
        End Function

        '''<summary>Returns the character to use in GenerateMessage()</summary>
        Public Function GetDriverAddressStr(ByVal driver As DriverAddresses) As String
            Return Chr(driver)
        End Function

        '''<summary>Returns the string used in GenerateCommand()</summary>
        Public Function GetCommandStr(ByVal command As Commands) As String
            Select Case command
                'Homing and Positioning
                Case Commands.GoHome : Return "Z"
                Case Commands.SetPosition : Return "z"
                Case Commands.GoAbsolute : Return "A"
                Case Commands.SetHomePolarity : Return "f"
                Case Commands.GoPositive : Return "P"
                Case Commands.GoNegative : Return "D"
                Case Commands.SetPulseJogDistance : Return "B"
                Case Commands.TerminateCommand : Return "T"
                Case Commands.SetMotorPolarity : Return "F"

                    'Velocity and Acceleration
                Case Commands.SetVelocity : Return "V"
                Case Commands.SetAcceleration : Return "L"

                    'Setting Current
                Case Commands.SetRunningCurrent : Return "m"
                Case Commands.SetHoldCurrent : Return "h"

                    'Looping and Branching
                Case Commands.BeginLoop : Return "g"
                Case Commands.EndLoop : Return "G"
                Case Commands.Delay : Return "M"
                Case Commands.HaltUntil : Return "H"
                Case Commands.SkipIf : Return "S"
                Case Commands.SetMode : Return "n"

                    'Position Correction - Encoder Option Only
                Case Commands.SetEncoderMode : Return "N"
                Case Commands.SetPositionCorrectionTolerance : Return "aC"
                Case Commands.SetEncoderRatio : Return "aE"
                Case Commands.SetPositionCorrectionRetries : Return "au"
                Case Commands.RecoverEncoderTimeout : Return "r"

                    'Program Stroage and Recall
                Case Commands.StoreProgram : Return "s"
                Case Commands.ExecuteStoredProgram : Return "e"

                    'Program Execution
                Case Commands.RunCurrentCommand : Return "R"
                Case Commands.RepeatCurrentCommand : Return "X"

                    'Microstepping
                Case Commands.SetMicrostepResolution : Return "j"
                Case Commands.SetMicrostepAdjust : Return "o"

                    'On/Off Drivers (Outputs)
                Case Commands.SetOutputOnOff : Return "J"

                    'Query Commands
                Case Commands.QueryMotorPosition : Return "?0"
                Case Commands.QueryStartVelocity : Return "?1"
                Case Commands.QuerySlewSpeed : Return "?2"
                Case Commands.QueryStopSpeed : Return "?3"
                Case Commands.QueryInputs : Return "?4"
                Case Commands.QueryCurrentVelocityModeSpeed : Return "?5"
                Case Commands.QueryMicrostepSize : Return "?6"
                Case Commands.QueryMicrostepAdjust : Return "?7"
                Case Commands.QueryEncoderPosition : Return "?8"
                Case Commands.ClearMemory : Return "?9"

                Case Commands.QueryCurrentCommand : Return "$"
                Case Commands.QueryFirmwareVersion : Return "&"
                Case Commands.QueryControllerStatus : Return "Q"
                Case Commands.TerminateCommands : Return "T"
                Case Commands.EchoNumber : Return "p"

                    'Baud Control
                Case Commands.SetBaudRate : Return "b"

                Case Else : Throw New ArgumentException("Unknown Enum Value", "command")
            End Select
        End Function

        ''' <summary>Evaluates an RX string received from the Silverpak and returns whether the RX message is complete and valid.</summary>
        Public Function IsRxDataComplete(ByVal rxData As String) As Boolean
            If rxData Is Nothing Then Return False 'rxData is Nothing
            If Not rxData.Contains(DTProtocolRxStartStr) Then Return False 'rxData does not include Start
            Return rxData.Substring(rxData.IndexOf(DTProtocolRxStartStr) + DTProtocolRxStartStr.Length).Contains(DTProtocolRxEndStr)
        End Function

        '''<summary>Returns just the status char and data from the passed RX message. Returns Nothing if RX data is incomplete or invalid.</summary>
        Public Function TrimRxData(ByVal rxData As String) As String
            If rxData Is Nothing Then Return Nothing 'rxData cannot be Nothing
            Dim iStart As Integer = rxData.IndexOf(DTProtocolRxStartStr)
            If iStart < 0 Then Return Nothing 'rxData must include DTPROTOCOL_RX_STARTCHAR
            Dim fstTrim As String = rxData.Substring(iStart + DTProtocolRxStartStr.Length)
            Dim iLen As Integer = fstTrim.IndexOf(DTProtocolRxEndStr)
            If iLen < 0 Then Return Nothing 'rxData must include DTPROTOCOL_RX_ENDCHAR after the DTPROTOCOL_RX_STARTCHAR
            Return fstTrim.Substring(0, iLen)
        End Function

        '''<summary>Sets the DataBits, Parity, StopBits, and Handshake properties of the passed SerialPort object in accordance with DT Protocol.</summary>
        Public Function InitializeSerialPort(ByVal srlPort As SerialPort) As SerialPort
            With srlPort
                .DataBits = DTProtocolComDataBits
                .Parity = DTProtocolComParity
                .StopBits = DTProtocolComStopBits
                .Handshake = DTProtocolComHandshake
            End With
            Return srlPort
        End Function

        '''<summary>Searches for available Silverpak23CE's and returns a PortInformation class for every serached COM port.
        ''' If any parameters are not set, all possible values for the parameters will be attempted.
        ''' This method can throw an ArgumentOutOfRangeException or an ArgumentException if passed values are invalid.</summary>
        Public Function SearchComPorts(Optional ByVal portName As String = SilverpakManager.DefaultPortname, Optional ByVal baudRate As Integer = SilverpakManager.DefaultBaudRate, _
                                       Optional ByVal driverAddress As DriverAddresses = SilverpakManager.DefaultDriverAddress) As PortInformation()
            If portName = SilverpakManager.DefaultPortname Then
                'Search all COM ports
                Dim allPortNames() As String = SerialPort.GetPortNames()
                Dim rtnAry(allPortNames.Length - 1) As PortInformation
                For i As Integer = 0 To allPortNames.Length - 1
                    'Search this COM port
                    rtnAry(i) = SearchBaudRates(allPortNames(i), baudRate, driverAddress)
                Next
                Return rtnAry
            Else
                'Search a specific COM port
                Return New PortInformation() {SearchBaudRates(portName, baudRate, driverAddress)}
            End If
        End Function
        '''<summary>Searches for an available Silverpak23CE at the specified COM port.
        ''' If any parameters are not set, all possible values for the parameters will be attempted.
        ''' This method can throw an ArgumentOutOfRangeException or an ArgumentException if passed values are invalid.</summary>
        Public Function SearchBaudRates(ByVal portName As String, Optional ByVal baudRate As Integer = SilverpakManager.DefaultBaudRate, _
                                        Optional ByVal driverAddress As DriverAddresses = SilverpakManager.DefaultDriverAddress) As PortInformation
            Dim portInfo As PortInformation = Nothing
            If baudRate = SilverpakManager.DefaultBaudRate Then
                'Search all baud rates
                For Each iBaudRate As Integer In New Integer() {9600, 19200, 38400}
                    portInfo = SearchDriverAddresses(portName, iBaudRate, driverAddress)
                    If portInfo IsNot Nothing Then Exit For
                Next
            Else
                'Search specific baud rate
                portInfo = SearchDriverAddresses(portName, baudRate, driverAddress)
            End If
            If portInfo Is Nothing Then portInfo = New PortInformation With {.PortName = portName, .PortStatus = PortStatuses.Empty}
            Return portInfo
        End Function
        '''<summary>Searches for an available Silverpak23CE at the specified COM port with the specified baud rate.
        ''' If any parameters are not set, all possible values for the parameters will be attempted.
        ''' Returns Nothing instead of a new PortInformation with .PortStatus = Empty.
        ''' This method can throw an ArgumentOutOfRangeException or an ArgumentException if passed values are invalid.</summary>
        Public Function SearchDriverAddresses(ByVal portName As String, ByVal baudRate As Integer, _
                                              Optional ByVal driverAddress As DriverAddresses = SilverpakManager.DefaultDriverAddress) As PortInformation
            If driverAddress = SilverpakManager.DefaultDriverAddress Then
                'Search all driver addresses
                Dim allDriverAddresses() As DriverAddresses = [Enum].GetValues(GetType(DriverAddresses))
                Dim portInfo As PortInformation = Nothing
                For i As Integer = 1 To 16 'from Driver1 to Driver0 (includes Driver2 - Driver9, DriverA - DriverF)
                    portInfo = GetSilverpakPortInfo(portName, baudRate, allDriverAddresses(i))
                    If portInfo IsNot Nothing Then Exit For
                Next
                Return portInfo
            Else
                'Search specified driver address
                Return GetSilverpakPortInfo(portName, baudRate, driverAddress)
            End If
        End Function
        '''<summary>Searches for an available Silverpak23CE at the specified COM port with the specified baud rate and driver address.
        ''' Returns Nothing instead of a new PortInformation with .PortStatus = Empty.
        ''' This method can throw an ArgumentOutOfRangeException or an ArgumentException if passed values are invalid.</summary>
        Public Function GetSilverpakPortInfo(ByVal portName As String, ByVal baudRate As Integer, _
                                             ByVal driverAddress As DriverAddresses) As PortInformation
            Using sp As SerialPort = InitializeSerialPort(New SerialPort())
                'set SerialPort parameters and allow exceptions to bubble out
                sp.PortName = portName
                sp.BaudRate = baudRate

                'delay if this method has been called recently
                Static s_nextSerialPortTime As Integer = Environment.TickCount + SilverpakConnectionManager.PortDelayUnit
                Thread.Sleep(Math.Max(0, s_nextSerialPortTime - Environment.TickCount))
                s_nextSerialPortTime = Environment.TickCount + SilverpakConnectionManager.PortDelayUnit

                'test the COM port
                Try
                    'Open the serial port
                    sp.Open() 'can throw UnauthorizedAccessException
                    'Write a safe query
                    sp.Write(GenerateMessage(driverAddress, SilverpakConnectionManager.SafeQueryCommandStr)) 'can throw IOException
                    'read response
                    Dim totalRx As String = "" 'accumulates chunks of RX data
                    Do
                        Thread.Sleep(SilverpakConnectionManager.PortDelayUnit) 'wait for a chunk to be written to the read buffer
                        Dim newRx As String = sp.ReadExisting 'retrieve any data from the read buffer
                        If newRx = "" Then Return Nothing 'abort if no data was written
                        totalRx &= newRx
                    Loop While Not IsRxDataComplete(totalRx) 'check to see if the RX data is complete
                    'success
                    Return New PortInformation With {.PortName = portName, .BaudRate = baudRate, .DriverAddress = driverAddress, .PortStatus = PortStatuses.AvailableSilverpak}
                Catch ex As UnauthorizedAccessException 'thrown by .Open
                    'Port was already open
                    Return New PortInformation With {.PortName = portName, .PortStatus = PortStatuses.Busy}
                Catch ex As IO.IOException 'thrown by .Write
                    'Port was invalid (such as a Bluetooth virtual COM port)
                    Return New PortInformation With {.PortName = portName, .PortStatus = PortStatuses.Invalid}
                Finally
                    'make sure the port is closed
                    Try
                        If sp.IsOpen Then sp.Close()
                    Catch
                    End Try
                End Try
            End Using
        End Function

    End Module

    'Friend enums
    '''<summary>All available commands. See Specification Commands for more information.</summary>
    Friend Enum Commands
        'Homing and Positioning
        ''' <summary>"Z"</summary>
        GoHome = 1
        ''' <summary>"z"</summary>
        SetPosition = 2
        ''' <summary>"A"</summary>
        GoAbsolute = 3
        ''' <summary>"f"</summary>
        SetHomePolarity = 4
        ''' <summary>"P"</summary>
        GoPositive = 5
        ''' <summary>"D"</summary>
        GoNegative = 6
        ''' <summary>"B"</summary>
        SetPulseJogDistance = 7
        ''' <summary>"T"</summary>
        TerminateCommand = 8
        ''' <summary>"F"</summary>
        SetMotorPolarity = 9

        'Velocity and Acceleration
        ''' <summary>"V"</summary>
        SetVelocity = 10
        ''' <summary>"A"</summary>
        SetAcceleration = 11

        'Setting Current
        ''' <summary>"m"</summary>
        SetRunningCurrent = 12
        ''' <summary>"h"</summary>
        SetHoldCurrent = 13

        'Looping and Branching
        ''' <summary>"g"</summary>
        BeginLoop = 14
        ''' <summary>"G"</summary>
        EndLoop = 15
        ''' <summary>"M"</summary>
        Delay = 16
        ''' <summary>"H"</summary>
        HaltUntil = 17
        ''' <summary>"S"</summary>
        SkipIf = 18
        ''' <summary>"n"</summary>
        SetMode = 19

        'Position Correction - Encoder Option Only
        ''' <summary>"N"</summary>
        SetEncoderMode = 20
        ''' <summary>"aC"</summary>
        SetPositionCorrectionTolerance = 21
        ''' <summary>"aE"</summary>
        SetEncoderRatio = 22
        ''' <summary>"au"</summary>
        SetPositionCorrectionRetries = 23
        ''' <summary>"r"</summary>
        RecoverEncoderTimeout = 24

        'Program Stroage and Recall
        ''' <summary>"s"</summary>
        StoreProgram = 25
        ''' <summary>"e"</summary>
        ExecuteStoredProgram = 26

        'Program Execution
        ''' <summary>"R"</summary>
        RunCurrentCommand = 27
        ''' <summary>"X"</summary>
        RepeatCurrentCommand = 28

        'Microstepping
        ''' <summary>"j"</summary>
        SetMicrostepResolution = 29
        ''' <summary>"o"</summary>
        SetMicrostepAdjust = 30

        'On/Off Drivers (Outputs)
        ''' <summary>"J"</summary>
        SetOutputOnOff = 31

        'Query Commands
        ''' <summary>"?0"</summary>
        QueryMotorPosition = 32
        ''' <summary>"?1"</summary>
        QueryStartVelocity = 33
        ''' <summary>"?2"</summary>
        QuerySlewSpeed = 34
        ''' <summary>"?3"</summary>
        QueryStopSpeed = 35
        ''' <summary>"?4"</summary>
        QueryInputs = 36
        ''' <summary>"?5"</summary>
        QueryCurrentVelocityModeSpeed = 37
        ''' <summary>"?6"</summary>
        QueryMicrostepSize = 38
        ''' <summary>"?7"</summary>
        QueryMicrostepAdjust = 39
        ''' <summary>"?8"</summary>
        QueryEncoderPosition = 40
        ''' <summary>"?9"</summary>
        ClearMemory = 41

        ''' <summary>"$"</summary>
        QueryCurrentCommand = 42
        ''' <summary>"&amp;"</summary>
        QueryFirmwareVersion = 43
        ''' <summary>"Q"</summary>
        QueryControllerStatus = 44
        ''' <summary>"T"</summary>
        TerminateCommands = 45
        ''' <summary>"p"</summary>
        EchoNumber = 46

        'Baud Control
        ''' <summary>"b"</summary>
        SetBaudRate = 47
    End Enum

    '''<summary>States for the motor</summary>
    Friend Enum MotorStates
        ''' <summary>Serial Port is closed.</summary>
        Disconnected
        ''' <summary>Serial Port is just open.</summary>
        Connected
        ''' <summary>Motor settings have been written to the Silverpak23CE.</summary>
        InitializedSettings
        ''' <summary>Small movements have been issued to the Silverpak23CE to clear initialization quirks.</summary>
        InitializedSmoothMotion
        ''' <summary>In the process of moving to the zero position.</summary>
        InitializingCoordinates_moveToZero
        ''' <summary>The "official" homing command. should complete very quickly.</summary>
        InitializingCoordinates_calibrateHome
        ''' <summary>In the process of aborting coordinate initialization.</summary>
        AbortingCoordinateInitialization
        ''' <summary>Fully initialized and stopped.</summary>
        Ready
        ''' <summary>In the process of moving.</summary>
        Moving
    End Enum

End Namespace

