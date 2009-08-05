Imports VisionaryDigital.SmartSteps
Imports VisionaryDigital.Settings


Namespace CamliftController
    Public Class PositionManager

        Private m_memReg As Integer?()

        Private m_smartStepsManager As SmartStepsManager

        Private m_settings As PositionManagerSettings

        Public Sub New(ByVal settings As PositionManagerSettings, ByVal smartStepsManager As SmartStepsManager)

            m_settings = If(settings, New PositionManagerSettings(Nothing))
            m_smartStepsManager = smartStepsManager

            ReDim m_memReg(settings.MemRegSize - 1)
        End Sub

        Public Sub SaveSettings()
            m_settings.MemRegSize = m_memReg.Length
        End Sub

        Public Function MakeStoreMenu(ByVal value As Integer) As ContextMenuStrip
            Return New StoreContextMenuStrip(Me, value)
        End Function
        Public Function MakeLoadMenu(ByVal f_loadPosition As Action(Of Integer)) As ContextMenuStrip
            Return New LoadContextMenuStrip(Me, f_loadPosition)
        End Function

        Private MustInherit Class StoreLoadContextMenuStrip
            Inherits ContextMenuStrip
            Protected _nest As PositionManager

            Protected WithEvents tsmiSavedPosition As ToolStripMenuItem
            Protected WithEvents tsmiMemManage As ToolStripMenuItem

            Protected Sub New(ByVal nest As PositionManager)
                MyBase.New()
                _nest = nest

                'Saved Position...
                tsmiSavedPosition = New ToolStripMenuItem("Saved Position...")
                Me.Items.Add(tsmiSavedPosition)

                Me.Items.Add(New ToolStripSeparator) ' -----------------------------

                'Mem1
                Dim i = 1
                For Each mem In nest.m_memReg
                    Me.Items.Add(initValueMenuItem_safe("Mem" & i, mem, MemHandler))
                    i += 1
                Next

                Me.Items.Add(New ToolStripSeparator) ' -----------------------------

                'Manage Memory Registers...
                tsmiMemManage = New ToolStripMenuItem("Manage Memory Registers...")
                Me.Items.Add(tsmiMemManage)

                Me.Items.Add(New ToolStripSeparator) ' -----------------------------

                Me.Items.Add(initValueMenuItem_safe("Autorun Start", _nest.m_smartStepsManager.LastAutorunRun.AutorunStart, AutorunStartHandler))
                Me.Items.Add(initValueMenuItem_safe("Autorun Stop", _nest.m_smartStepsManager.LastAutorunRun.AutorunStop, AutorunStopHandler))

            End Sub
            Private Sub tsmiMemManage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiMemManage.Click
                MsgBox("Manually set the values of each register," & vbCrLf & "adjust the number of registers," & vbCrLf & "ect...")
            End Sub

            Private Function initValueMenuItem_safe(ByVal baseTitle As String, ByVal value As Object, ByVal clickHandler As EventHandler) As ToolStripMenuItem
                Dim tempText = baseTitle
                Dim valueIsNull = value Is Nothing OrElse value.ToString = ""
                If Not valueIsNull Then tempText &= " = " & value
                Dim tsmi = New ToolStripMenuItem(tempText)
                tsmi.Enabled = Not (valueIsNull And DisableNull)
                AddHandler tsmi.Click, clickHandler
                Return tsmi
            End Function

            Protected MustOverride ReadOnly Property DisableNull() As Boolean
            Protected MustOverride ReadOnly Property MemHandler() As EventHandler
            Protected MustOverride ReadOnly Property AutorunStartHandler() As EventHandler
            Protected MustOverride ReadOnly Property AutorunStopHandler() As EventHandler

        End Class
        Private Class LoadContextMenuStrip
            Inherits StoreLoadContextMenuStrip

            Private m_f_loadPosition As Action(Of Integer)

            Public Sub New(ByVal nest As PositionManager, ByVal f_loadPosition As Action(Of Integer))
                MyBase.New(nest)

                m_f_loadPosition = f_loadPosition
            End Sub

            Private Sub tsmiLoadSavedPosition_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiSavedPosition.Click
                MsgBox("Coming soon!", Title:=MsgBoxTitle)
            End Sub
            Private Sub tsmiLoadMem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
                Dim tsmi As ToolStripMenuItem = sender
                Dim number As Integer = tsmi.Text.Substring(3, 1) - 1
                m_f_loadPosition.Invoke(_nest.m_memReg(number))
            End Sub
            Private Sub tsmiLoadAutorunStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
                m_f_loadPosition.Invoke(_nest.m_smartStepsManager.LastAutorunRun.AutorunStart)
            End Sub
            Private Sub tsmiLoadAutorunStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
                m_f_loadPosition.Invoke(_nest.m_smartStepsManager.LastAutorunRun.AutorunStop)
            End Sub

            Protected Overrides ReadOnly Property AutorunStartHandler() As System.EventHandler
                Get
                    Return AddressOf tsmiLoadAutorunStart_Click
                End Get
            End Property
            Protected Overrides ReadOnly Property AutorunStopHandler() As System.EventHandler
                Get
                    Return AddressOf tsmiLoadAutorunStop_Click
                End Get
            End Property
            Protected Overrides ReadOnly Property DisableNull() As Boolean
                Get
                    Return True
                End Get
            End Property
            Protected Overrides ReadOnly Property MemHandler() As System.EventHandler
                Get
                    Return AddressOf tsmiLoadMem_Click
                End Get
            End Property
        End Class
        Private Class StoreContextMenuStrip
            Inherits StoreLoadContextMenuStrip

            Private m_value As Integer

            Public Sub New(ByVal nest As PositionManager, ByVal value As Integer)
                MyBase.New(nest)

                m_value = value
            End Sub

            Private Sub tsmiStoreSavedPosition_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiSavedPosition.Click
                MsgBox("Coming soon!", Title:=MsgBoxTitle)
            End Sub
            Private Sub tsmiStoreMem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
                Dim tsmi As ToolStripMenuItem = sender
                Dim number As Integer = tsmi.Text.Substring(3, 1) - 1
                _nest.m_memReg(number) = m_value
            End Sub
            Private Sub tsmiStoreAutorunStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
                _nest.m_smartStepsManager.LastAutorunRun.AutorunStart = m_value
            End Sub
            Private Sub tsmiStoreAutorunStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
                _nest.m_smartStepsManager.LastAutorunRun.AutorunStop = m_value
            End Sub

            Protected Overrides ReadOnly Property AutorunStartHandler() As System.EventHandler
                Get
                    Return AddressOf tsmiStoreAutorunStart_Click
                End Get
            End Property
            Protected Overrides ReadOnly Property AutorunStopHandler() As System.EventHandler
                Get
                    Return AddressOf tsmiStoreAutorunStop_Click
                End Get
            End Property
            Protected Overrides ReadOnly Property DisableNull() As Boolean
                Get
                    Return False
                End Get
            End Property
            Protected Overrides ReadOnly Property MemHandler() As System.EventHandler
                Get
                    Return AddressOf tsmiStoreMem_Click
                End Get
            End Property
        End Class

    End Class


    Public Class LoadPositionEventArgs
        Inherits EventArgs

        Private m_NewPosition As Integer
        Public Property NewPosition() As Integer
            Get
                Return m_NewPosition
            End Get
            Set(ByVal value As Integer)
                m_NewPosition = value
            End Set
        End Property

        Public Sub New(ByVal newPosition As Integer)
            m_NewPosition = newPosition
        End Sub
    End Class
End Namespace
