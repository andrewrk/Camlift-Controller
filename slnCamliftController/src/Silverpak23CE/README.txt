================================================================================
======================      Silverpak23CE.vb  README      ======================
======================       Version 1.1  8/20/2008       ======================
======================  Copyright 2008 Visionary Digital  ======================
======================  http://www.visionarydigital.com/  ======================
======================          Visual Basic 9.0          ======================
================================================================================


This namespace provides functionality for interfacing with a Lin Engineering 
Silverpak23CE stepper motor connected locally to a COM port. Documentation for 
this namespace and its members is primarilly integrated into xml comments 
accessible through IntelliSense. 

Using the SilverpakManager Class:

1. Declare and instantiate a SilverpakManager in a Using statement or in a 
         designer interface
2. Set any properties of the SilverpakManager applicable to your application
3. Call the SilverpakManager.FindAndConnect() function and use it's result to 
         determine if the connection process succeed.
4. Initialize the motor by calling the following three methods in order:
   1. SilverpakManager.InitializeMotorSettings()
   2. SilverpakManager.InitializeSmoothMotion()
   3. SilverpakManager.InitializeCoordinates()
5. Wait for the motor to move to its "home" position either by handling the 
         SilverpakManager.CoordinatesInitialized event or by reading the 
         SilverpakManager.IsReady property until it returns True. 
6. Operate the Silverpak23CE stepper motor with the following methods:
   - SilverpakManager.GoInfinite(Boolean)
   - SilverpakManager.GoToPosition(Integer)
   - SilverpakManager.StopMotor()
7. Get the position of the motor either by handling the 
         SilverpakManager.PositionChanged event or by reading the 
         SilverpakManager.Position property.
8. Detect when the connection to the motor is lost either by handling the 
         SilverpakManager.ConnectionLost event or by reading the 
         SilverpakManager.IsActive property.


Notes on Synchronization and Member Names:

In the SilverpakManager and SilverpakConnectionManager classes, some fields and 
methods are not threadsafe and require that a SyncLock be put on a particular 
object before accessing them. For every group of non-threadsafe members, there 
is an object named "m_[identifier]_lock" where [identifier] is a name given to 
the group of non-threadsafe members. This object is the argument for the 
Synclock statement that begins a block of code in which it is safe to use the 
non-threadsafe members. The non-threadsafe members are named 
"m_[varname]_[identifier]" to indicate that they are not threadsafe and to 
identify the group they belong to.


Links:

Visionary Digital (Owner): 
         http://www.visionarydigital.com/
Josh Wolfe (Programmer): 
         thejoshwolfe@gmail.com
Lin Engineering Silverpak23CE Specification Manual: 
         http://www.linengineering.com/LinE/contents/stepmotors/pdf/SilverPak23C_CE_Manual.pdf
Lin Engineering Silverpak23CE Specification Commands: 
         http://www.linengineering.com/LinE/contents/stepmotors/pdf/Silverpak23C-256Commands.pdf

