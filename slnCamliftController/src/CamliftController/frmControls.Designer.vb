Namespace CamliftController

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmControls
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmControls))
            Me.stsStatusStrip = New System.Windows.Forms.StatusStrip
            Me.tslStatus = New System.Windows.Forms.ToolStripStatusLabel
            Me.tkbPos = New System.Windows.Forms.TrackBar
            Me.btnGo = New System.Windows.Forms.Button
            Me.txtPos = New System.Windows.Forms.TextBox
            Me.cmsPosition = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.tsmLoad = New System.Windows.Forms.ToolStripMenuItem
            Me.tsmStore = New System.Windows.Forms.ToolStripMenuItem
            Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
            Me.ManageMemoryRegistersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
            Me.SetAutorunStartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.SetAutorunStopHereToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.grpDist = New System.Windows.Forms.GroupBox
            Me.lblCurrentDist = New System.Windows.Forms.Label
            Me.pnlDist = New System.Windows.Forms.Panel
            Me.lblDist1 = New System.Windows.Forms.Label
            Me.Label1 = New System.Windows.Forms.Label
            Me.lblDist9 = New System.Windows.Forms.Label
            Me.lblDist2 = New System.Windows.Forms.Label
            Me.lblDist8 = New System.Windows.Forms.Label
            Me.lblDist3 = New System.Windows.Forms.Label
            Me.lblDist7 = New System.Windows.Forms.Label
            Me.lblDist4 = New System.Windows.Forms.Label
            Me.lblDist6 = New System.Windows.Forms.Label
            Me.lblDist5 = New System.Windows.Forms.Label
            Me.tkbDist = New System.Windows.Forms.TrackBar
            Me.btnStop = New System.Windows.Forms.Button
            Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
            Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.PreferencesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.AdvancedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.SavePicturesFolderMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
            Me.AlwaysOnTopToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.UsersGuideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
            Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
            Me.btnAutorun = New System.Windows.Forms.Button
            Me.btnAutoStart = New System.Windows.Forms.Button
            Me.btnAutoStop = New System.Windows.Forms.Button
            Me.GroupBox1 = New System.Windows.Forms.GroupBox
            Me.btnDown = New System.Windows.Forms.Button
            Me.btnUp = New System.Windows.Forms.Button
            Me.GroupBox2 = New System.Windows.Forms.GroupBox
            Me.lblAutoStop = New System.Windows.Forms.Label
            Me.lblAutoStart = New System.Windows.Forms.Label
            Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
            Me.btnLiveView = New System.Windows.Forms.Button
            Me.btnTakePic = New System.Windows.Forms.Button
            Me.stsStatusStrip.SuspendLayout()
            CType(Me.tkbPos, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.cmsPosition.SuspendLayout()
            Me.grpDist.SuspendLayout()
            Me.pnlDist.SuspendLayout()
            CType(Me.tkbDist, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.MenuStrip1.SuspendLayout()
            Me.FlowLayoutPanel1.SuspendLayout()
            Me.GroupBox1.SuspendLayout()
            Me.GroupBox2.SuspendLayout()
            Me.FlowLayoutPanel2.SuspendLayout()
            Me.TableLayoutPanel1.SuspendLayout()
            Me.SuspendLayout()
            '
            'stsStatusStrip
            '
            Me.stsStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tslStatus})
            Me.stsStatusStrip.Location = New System.Drawing.Point(0, 601)
            Me.stsStatusStrip.Name = "stsStatusStrip"
            Me.stsStatusStrip.Size = New System.Drawing.Size(186, 22)
            Me.stsStatusStrip.SizingGrip = False
            Me.stsStatusStrip.TabIndex = 0
            Me.stsStatusStrip.Text = "StatusStrip1"
            '
            'tslStatus
            '
            Me.tslStatus.Name = "tslStatus"
            Me.tslStatus.Size = New System.Drawing.Size(70, 17)
            Me.tslStatus.Text = "Initializing..."
            '
            'tkbPos
            '
            Me.tkbPos.Dock = System.Windows.Forms.DockStyle.Left
            Me.tkbPos.Enabled = False
            Me.tkbPos.LargeChange = 0
            Me.tkbPos.Location = New System.Drawing.Point(0, 24)
            Me.tkbPos.Maximum = 1000000
            Me.tkbPos.Name = "tkbPos"
            Me.tkbPos.Orientation = System.Windows.Forms.Orientation.Vertical
            Me.tkbPos.Size = New System.Drawing.Size(45, 577)
            Me.tkbPos.SmallChange = 10000
            Me.tkbPos.TabIndex = 9
            Me.tkbPos.TickFrequency = 10000
            Me.tkbPos.Value = 1000000
            '
            'btnGo
            '
            Me.btnGo.Enabled = False
            Me.btnGo.Location = New System.Drawing.Point(60, 3)
            Me.btnGo.Name = "btnGo"
            Me.btnGo.Size = New System.Drawing.Size(31, 23)
            Me.btnGo.TabIndex = 3
            Me.btnGo.Text = "&Go"
            Me.btnGo.UseVisualStyleBackColor = True
            '
            'txtPos
            '
            Me.txtPos.ContextMenuStrip = Me.cmsPosition
            Me.txtPos.Enabled = False
            Me.txtPos.Location = New System.Drawing.Point(3, 4)
            Me.txtPos.Margin = New System.Windows.Forms.Padding(3, 4, 3, 3)
            Me.txtPos.Name = "txtPos"
            Me.txtPos.Size = New System.Drawing.Size(51, 20)
            Me.txtPos.TabIndex = 2
            '
            'cmsPosition
            '
            Me.cmsPosition.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmLoad, Me.tsmStore, Me.ToolStripMenuItem1, Me.ManageMemoryRegistersToolStripMenuItem, Me.ToolStripMenuItem2, Me.SetAutorunStartToolStripMenuItem, Me.SetAutorunStopHereToolStripMenuItem})
            Me.cmsPosition.Name = "cmsPosition"
            Me.cmsPosition.Size = New System.Drawing.Size(225, 126)
            '
            'tsmLoad
            '
            Me.tsmLoad.Name = "tsmLoad"
            Me.tsmLoad.Size = New System.Drawing.Size(224, 22)
            Me.tsmLoad.Text = "Load"
            '
            'tsmStore
            '
            Me.tsmStore.Name = "tsmStore"
            Me.tsmStore.Size = New System.Drawing.Size(224, 22)
            Me.tsmStore.Text = "Store"
            '
            'ToolStripMenuItem1
            '
            Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
            Me.ToolStripMenuItem1.Size = New System.Drawing.Size(221, 6)
            '
            'ManageMemoryRegistersToolStripMenuItem
            '
            Me.ManageMemoryRegistersToolStripMenuItem.Name = "ManageMemoryRegistersToolStripMenuItem"
            Me.ManageMemoryRegistersToolStripMenuItem.Size = New System.Drawing.Size(224, 22)
            Me.ManageMemoryRegistersToolStripMenuItem.Text = "Manage Memory Registers..."
            '
            'ToolStripMenuItem2
            '
            Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
            Me.ToolStripMenuItem2.Size = New System.Drawing.Size(221, 6)
            '
            'SetAutorunStartToolStripMenuItem
            '
            Me.SetAutorunStartToolStripMenuItem.Name = "SetAutorunStartToolStripMenuItem"
            Me.SetAutorunStartToolStripMenuItem.Size = New System.Drawing.Size(224, 22)
            Me.SetAutorunStartToolStripMenuItem.Text = "Set Autorun Start Here"
            '
            'SetAutorunStopHereToolStripMenuItem
            '
            Me.SetAutorunStopHereToolStripMenuItem.Name = "SetAutorunStopHereToolStripMenuItem"
            Me.SetAutorunStopHereToolStripMenuItem.Size = New System.Drawing.Size(224, 22)
            Me.SetAutorunStopHereToolStripMenuItem.Text = "Set Autorun Stop Here"
            '
            'grpDist
            '
            Me.grpDist.Controls.Add(Me.lblCurrentDist)
            Me.grpDist.Controls.Add(Me.pnlDist)
            Me.grpDist.Controls.Add(Me.tkbDist)
            Me.grpDist.Location = New System.Drawing.Point(14, 165)
            Me.grpDist.Name = "grpDist"
            Me.grpDist.Size = New System.Drawing.Size(108, 204)
            Me.grpDist.TabIndex = 3
            Me.grpDist.TabStop = False
            Me.grpDist.Text = "Distance"
            '
            'lblCurrentDist
            '
            Me.lblCurrentDist.AutoSize = True
            Me.lblCurrentDist.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblCurrentDist.Location = New System.Drawing.Point(6, 16)
            Me.lblCurrentDist.Name = "lblCurrentDist"
            Me.lblCurrentDist.Size = New System.Drawing.Size(38, 13)
            Me.lblCurrentDist.TabIndex = 1
            Me.lblCurrentDist.Text = "Infinite"
            '
            'pnlDist
            '
            Me.pnlDist.Controls.Add(Me.lblDist1)
            Me.pnlDist.Controls.Add(Me.Label1)
            Me.pnlDist.Controls.Add(Me.lblDist9)
            Me.pnlDist.Controls.Add(Me.lblDist2)
            Me.pnlDist.Controls.Add(Me.lblDist8)
            Me.pnlDist.Controls.Add(Me.lblDist3)
            Me.pnlDist.Controls.Add(Me.lblDist7)
            Me.pnlDist.Controls.Add(Me.lblDist4)
            Me.pnlDist.Controls.Add(Me.lblDist6)
            Me.pnlDist.Controls.Add(Me.lblDist5)
            Me.pnlDist.Location = New System.Drawing.Point(46, 35)
            Me.pnlDist.Name = "pnlDist"
            Me.pnlDist.Size = New System.Drawing.Size(56, 163)
            Me.pnlDist.TabIndex = 11
            '
            'lblDist1
            '
            Me.lblDist1.AutoSize = True
            Me.lblDist1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDist1.Location = New System.Drawing.Point(3, 143)
            Me.lblDist1.Name = "lblDist1"
            Me.lblDist1.Size = New System.Drawing.Size(24, 13)
            Me.lblDist1.TabIndex = 10
            Me.lblDist1.Text = "20x"
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label1.Location = New System.Drawing.Point(3, 8)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(38, 13)
            Me.Label1.TabIndex = 2
            Me.Label1.Text = "Infinite"
            '
            'lblDist9
            '
            Me.lblDist9.AutoSize = True
            Me.lblDist9.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDist9.Location = New System.Drawing.Point(3, 21)
            Me.lblDist9.Name = "lblDist9"
            Me.lblDist9.Size = New System.Drawing.Size(44, 13)
            Me.lblDist9.TabIndex = 2
            Me.lblDist9.Text = "4.00mm"
            '
            'lblDist2
            '
            Me.lblDist2.AutoSize = True
            Me.lblDist2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDist2.Location = New System.Drawing.Point(3, 127)
            Me.lblDist2.Name = "lblDist2"
            Me.lblDist2.Size = New System.Drawing.Size(33, 13)
            Me.lblDist2.TabIndex = 9
            Me.lblDist2.Text = "10x +"
            '
            'lblDist8
            '
            Me.lblDist8.AutoSize = True
            Me.lblDist8.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDist8.Location = New System.Drawing.Point(3, 36)
            Me.lblDist8.Name = "lblDist8"
            Me.lblDist8.Size = New System.Drawing.Size(44, 13)
            Me.lblDist8.TabIndex = 3
            Me.lblDist8.Text = "2.00mm"
            '
            'lblDist3
            '
            Me.lblDist3.AutoSize = True
            Me.lblDist3.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDist3.Location = New System.Drawing.Point(3, 111)
            Me.lblDist3.Name = "lblDist3"
            Me.lblDist3.Size = New System.Drawing.Size(24, 13)
            Me.lblDist3.TabIndex = 8
            Me.lblDist3.Text = "10x"
            '
            'lblDist7
            '
            Me.lblDist7.AutoSize = True
            Me.lblDist7.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDist7.Location = New System.Drawing.Point(3, 51)
            Me.lblDist7.Name = "lblDist7"
            Me.lblDist7.Size = New System.Drawing.Size(44, 13)
            Me.lblDist7.TabIndex = 4
            Me.lblDist7.Text = "1.00mm"
            '
            'lblDist4
            '
            Me.lblDist4.AutoSize = True
            Me.lblDist4.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDist4.Location = New System.Drawing.Point(3, 96)
            Me.lblDist4.Name = "lblDist4"
            Me.lblDist4.Size = New System.Drawing.Size(26, 13)
            Me.lblDist4.TabIndex = 7
            Me.lblDist4.Text = "CF4"
            '
            'lblDist6
            '
            Me.lblDist6.AutoSize = True
            Me.lblDist6.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDist6.Location = New System.Drawing.Point(3, 66)
            Me.lblDist6.Name = "lblDist6"
            Me.lblDist6.Size = New System.Drawing.Size(26, 13)
            Me.lblDist6.TabIndex = 5
            Me.lblDist6.Text = "CF2"
            '
            'lblDist5
            '
            Me.lblDist5.AutoSize = True
            Me.lblDist5.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDist5.Location = New System.Drawing.Point(3, 81)
            Me.lblDist5.Name = "lblDist5"
            Me.lblDist5.Size = New System.Drawing.Size(26, 13)
            Me.lblDist5.TabIndex = 6
            Me.lblDist5.Text = "CF3"
            '
            'tkbDist
            '
            Me.tkbDist.BackColor = System.Drawing.Color.Black
            Me.tkbDist.Enabled = False
            Me.tkbDist.LargeChange = 1
            Me.tkbDist.Location = New System.Drawing.Point(9, 36)
            Me.tkbDist.Minimum = 1
            Me.tkbDist.Name = "tkbDist"
            Me.tkbDist.Orientation = System.Windows.Forms.Orientation.Vertical
            Me.tkbDist.Size = New System.Drawing.Size(45, 160)
            Me.tkbDist.TabIndex = 3
            Me.tkbDist.TabStop = False
            Me.tkbDist.Value = 10
            '
            'btnStop
            '
            Me.btnStop.BackColor = System.Drawing.Color.Red
            Me.btnStop.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnStop.Location = New System.Drawing.Point(14, 125)
            Me.btnStop.Name = "btnStop"
            Me.btnStop.Size = New System.Drawing.Size(97, 35)
            Me.btnStop.TabIndex = 8
            Me.btnStop.Text = "ST&OP !"
            Me.btnStop.UseVisualStyleBackColor = False
            '
            'MenuStrip1
            '
            Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SettingsToolStripMenuItem, Me.HelpToolStripMenuItem})
            Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
            Me.MenuStrip1.Name = "MenuStrip1"
            Me.MenuStrip1.Size = New System.Drawing.Size(186, 24)
            Me.MenuStrip1.TabIndex = 10
            Me.MenuStrip1.Text = "MenuStrip1"
            '
            'SettingsToolStripMenuItem
            '
            Me.SettingsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PreferencesToolStripMenuItem, Me.AdvancedToolStripMenuItem, Me.SavePicturesFolderMenuItem, Me.ToolStripSeparator2, Me.AlwaysOnTopToolStripMenuItem})
            Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
            Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
            Me.SettingsToolStripMenuItem.Text = "&Settings"
            '
            'PreferencesToolStripMenuItem
            '
            Me.PreferencesToolStripMenuItem.Enabled = False
            Me.PreferencesToolStripMenuItem.Name = "PreferencesToolStripMenuItem"
            Me.PreferencesToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
            Me.PreferencesToolStripMenuItem.Text = "Edit &Step Settings..."
            '
            'AdvancedToolStripMenuItem
            '
            Me.AdvancedToolStripMenuItem.Enabled = False
            Me.AdvancedToolStripMenuItem.Name = "AdvancedToolStripMenuItem"
            Me.AdvancedToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
            Me.AdvancedToolStripMenuItem.Text = "&Advanced..."
            '
            'SavePicturesFolderMenuItem
            '
            Me.SavePicturesFolderMenuItem.Enabled = False
            Me.SavePicturesFolderMenuItem.Name = "SavePicturesFolderMenuItem"
            Me.SavePicturesFolderMenuItem.Size = New System.Drawing.Size(188, 22)
            Me.SavePicturesFolderMenuItem.Text = "&Save Pictures Folder..."
            '
            'ToolStripSeparator2
            '
            Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
            Me.ToolStripSeparator2.Size = New System.Drawing.Size(185, 6)
            '
            'AlwaysOnTopToolStripMenuItem
            '
            Me.AlwaysOnTopToolStripMenuItem.CheckOnClick = True
            Me.AlwaysOnTopToolStripMenuItem.Name = "AlwaysOnTopToolStripMenuItem"
            Me.AlwaysOnTopToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
            Me.AlwaysOnTopToolStripMenuItem.Text = "Always On Top"
            '
            'HelpToolStripMenuItem
            '
            Me.HelpToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
            Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UsersGuideToolStripMenuItem, Me.ToolStripSeparator1, Me.AboutToolStripMenuItem})
            Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
            Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
            Me.HelpToolStripMenuItem.Text = "&Help"
            '
            'UsersGuideToolStripMenuItem
            '
            Me.UsersGuideToolStripMenuItem.Name = "UsersGuideToolStripMenuItem"
            Me.UsersGuideToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
            Me.UsersGuideToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
            Me.UsersGuideToolStripMenuItem.Text = "&Users Guide"
            '
            'ToolStripSeparator1
            '
            Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
            Me.ToolStripSeparator1.Size = New System.Drawing.Size(152, 6)
            '
            'AboutToolStripMenuItem
            '
            Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
            Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
            Me.AboutToolStripMenuItem.Text = "&About"
            '
            'FlowLayoutPanel1
            '
            Me.FlowLayoutPanel1.AutoSize = True
            Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.FlowLayoutPanel1.Controls.Add(Me.txtPos)
            Me.FlowLayoutPanel1.Controls.Add(Me.btnGo)
            Me.FlowLayoutPanel1.Location = New System.Drawing.Point(11, 16)
            Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
            Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
            Me.FlowLayoutPanel1.Size = New System.Drawing.Size(94, 29)
            Me.FlowLayoutPanel1.TabIndex = 12
            '
            'btnAutorun
            '
            Me.btnAutorun.Location = New System.Drawing.Point(14, 19)
            Me.btnAutorun.Name = "btnAutorun"
            Me.btnAutorun.Size = New System.Drawing.Size(97, 35)
            Me.btnAutorun.TabIndex = 0
            Me.btnAutorun.Text = "&Autorun..."
            Me.btnAutorun.UseVisualStyleBackColor = True
            '
            'btnAutoStart
            '
            Me.btnAutoStart.Location = New System.Drawing.Point(3, 60)
            Me.btnAutoStart.Name = "btnAutoStart"
            Me.btnAutoStart.Size = New System.Drawing.Size(73, 20)
            Me.btnAutoStart.TabIndex = 17
            Me.btnAutoStart.Text = "Sta&rt Here"
            Me.btnAutoStart.UseVisualStyleBackColor = True
            '
            'btnAutoStop
            '
            Me.btnAutoStop.Location = New System.Drawing.Point(3, 82)
            Me.btnAutoStop.Name = "btnAutoStop"
            Me.btnAutoStop.Size = New System.Drawing.Size(73, 20)
            Me.btnAutoStop.TabIndex = 18
            Me.btnAutoStop.Text = "Sto&p Here"
            Me.btnAutoStop.UseVisualStyleBackColor = True
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.FlowLayoutPanel1)
            Me.GroupBox1.Controls.Add(Me.grpDist)
            Me.GroupBox1.Controls.Add(Me.btnStop)
            Me.GroupBox1.Controls.Add(Me.btnDown)
            Me.GroupBox1.Controls.Add(Me.btnUp)
            Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(128, 375)
            Me.GroupBox1.TabIndex = 19
            Me.GroupBox1.TabStop = False
            Me.GroupBox1.Text = "Motor"
            '
            'btnDown
            '
            Me.btnDown.Enabled = False
            Me.btnDown.Image = Global.VisionaryDigital.My.Resources.Resources.Down
            Me.btnDown.Location = New System.Drawing.Point(14, 87)
            Me.btnDown.Name = "btnDown"
            Me.btnDown.Size = New System.Drawing.Size(97, 32)
            Me.btnDown.TabIndex = 7
            Me.btnDown.UseVisualStyleBackColor = True
            '
            'btnUp
            '
            Me.btnUp.Enabled = False
            Me.btnUp.Image = Global.VisionaryDigital.My.Resources.Resources.Up
            Me.btnUp.Location = New System.Drawing.Point(14, 49)
            Me.btnUp.Name = "btnUp"
            Me.btnUp.Size = New System.Drawing.Size(97, 32)
            Me.btnUp.TabIndex = 6
            Me.btnUp.UseVisualStyleBackColor = True
            '
            'GroupBox2
            '
            Me.GroupBox2.Controls.Add(Me.lblAutoStop)
            Me.GroupBox2.Controls.Add(Me.lblAutoStart)
            Me.GroupBox2.Controls.Add(Me.btnAutorun)
            Me.GroupBox2.Controls.Add(Me.btnAutoStart)
            Me.GroupBox2.Controls.Add(Me.btnAutoStop)
            Me.GroupBox2.Location = New System.Drawing.Point(3, 384)
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.Size = New System.Drawing.Size(128, 109)
            Me.GroupBox2.TabIndex = 20
            Me.GroupBox2.TabStop = False
            Me.GroupBox2.Text = "Autorun"
            '
            'lblAutoStop
            '
            Me.lblAutoStop.AutoSize = True
            Me.lblAutoStop.Location = New System.Drawing.Point(82, 86)
            Me.lblAutoStop.Name = "lblAutoStop"
            Me.lblAutoStop.Size = New System.Drawing.Size(0, 13)
            Me.lblAutoStop.TabIndex = 19
            '
            'lblAutoStart
            '
            Me.lblAutoStart.AutoSize = True
            Me.lblAutoStart.Location = New System.Drawing.Point(82, 64)
            Me.lblAutoStart.Name = "lblAutoStart"
            Me.lblAutoStart.Size = New System.Drawing.Size(0, 13)
            Me.lblAutoStart.TabIndex = 19
            '
            'FlowLayoutPanel2
            '
            Me.FlowLayoutPanel2.AutoSize = True
            Me.FlowLayoutPanel2.Controls.Add(Me.GroupBox1)
            Me.FlowLayoutPanel2.Controls.Add(Me.GroupBox2)
            Me.FlowLayoutPanel2.Controls.Add(Me.TableLayoutPanel1)
            Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
            Me.FlowLayoutPanel2.Location = New System.Drawing.Point(45, 24)
            Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
            Me.FlowLayoutPanel2.Size = New System.Drawing.Size(141, 577)
            Me.FlowLayoutPanel2.TabIndex = 21
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.ColumnCount = 3
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.Controls.Add(Me.btnLiveView, 1, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.btnTakePic, 1, 1)
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 499)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 2
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(128, 71)
            Me.TableLayoutPanel1.TabIndex = 21
            '
            'btnLiveView
            '
            Me.btnLiveView.Image = Global.VisionaryDigital.My.Resources.Resources.icon_reel
            Me.btnLiveView.Location = New System.Drawing.Point(15, 3)
            Me.btnLiveView.Name = "btnLiveView"
            Me.btnLiveView.Size = New System.Drawing.Size(97, 28)
            Me.btnLiveView.TabIndex = 16
            Me.btnLiveView.Text = "&Live View..."
            Me.btnLiveView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.btnLiveView.UseVisualStyleBackColor = True
            '
            'btnTakePic
            '
            Me.btnTakePic.Image = Global.VisionaryDigital.My.Resources.Resources.InsertPictureHS
            Me.btnTakePic.Location = New System.Drawing.Point(15, 37)
            Me.btnTakePic.Name = "btnTakePic"
            Me.btnTakePic.Size = New System.Drawing.Size(97, 28)
            Me.btnTakePic.TabIndex = 15
            Me.btnTakePic.Text = "&Take Picture"
            Me.btnTakePic.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.btnTakePic.UseVisualStyleBackColor = True
            '
            'frmControls
            '
            Me.AcceptButton = Me.btnGo
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoSize = True
            Me.ClientSize = New System.Drawing.Size(186, 623)
            Me.Controls.Add(Me.FlowLayoutPanel2)
            Me.Controls.Add(Me.tkbPos)
            Me.Controls.Add(Me.stsStatusStrip)
            Me.Controls.Add(Me.MenuStrip1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.KeyPreview = True
            Me.Location = New System.Drawing.Point(0, 100)
            Me.MainMenuStrip = Me.MenuStrip1
            Me.MaximizeBox = False
            Me.Name = "frmControls"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
            Me.Text = "Camlift"
            Me.stsStatusStrip.ResumeLayout(False)
            Me.stsStatusStrip.PerformLayout()
            CType(Me.tkbPos, System.ComponentModel.ISupportInitialize).EndInit()
            Me.cmsPosition.ResumeLayout(False)
            Me.grpDist.ResumeLayout(False)
            Me.grpDist.PerformLayout()
            Me.pnlDist.ResumeLayout(False)
            Me.pnlDist.PerformLayout()
            CType(Me.tkbDist, System.ComponentModel.ISupportInitialize).EndInit()
            Me.MenuStrip1.ResumeLayout(False)
            Me.MenuStrip1.PerformLayout()
            Me.FlowLayoutPanel1.ResumeLayout(False)
            Me.FlowLayoutPanel1.PerformLayout()
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            Me.GroupBox2.ResumeLayout(False)
            Me.GroupBox2.PerformLayout()
            Me.FlowLayoutPanel2.ResumeLayout(False)
            Me.TableLayoutPanel1.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents stsStatusStrip As System.Windows.Forms.StatusStrip
        Friend WithEvents tkbPos As System.Windows.Forms.TrackBar
        Friend WithEvents btnGo As System.Windows.Forms.Button
        Friend WithEvents txtPos As System.Windows.Forms.TextBox
        Friend WithEvents tslStatus As System.Windows.Forms.ToolStripStatusLabel
        Friend WithEvents grpDist As System.Windows.Forms.GroupBox
        Friend WithEvents tkbDist As System.Windows.Forms.TrackBar
        Friend WithEvents lblCurrentDist As System.Windows.Forms.Label
        Friend WithEvents lblDist9 As System.Windows.Forms.Label
        Friend WithEvents lblDist8 As System.Windows.Forms.Label
        Friend WithEvents lblDist7 As System.Windows.Forms.Label
        Friend WithEvents lblDist4 As System.Windows.Forms.Label
        Friend WithEvents lblDist5 As System.Windows.Forms.Label
        Friend WithEvents lblDist6 As System.Windows.Forms.Label
        Friend WithEvents lblDist1 As System.Windows.Forms.Label
        Friend WithEvents lblDist2 As System.Windows.Forms.Label
        Friend WithEvents lblDist3 As System.Windows.Forms.Label
        Friend WithEvents btnUp As System.Windows.Forms.Button
        Friend WithEvents btnDown As System.Windows.Forms.Button
        Friend WithEvents btnStop As System.Windows.Forms.Button
        Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
        Friend WithEvents SettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents PreferencesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AdvancedToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents UsersGuideToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents pnlDist As System.Windows.Forms.Panel
        Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents AlwaysOnTopToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents btnAutorun As System.Windows.Forms.Button
        Friend WithEvents btnTakePic As System.Windows.Forms.Button
        Friend WithEvents btnLiveView As System.Windows.Forms.Button
        Friend WithEvents SavePicturesFolderMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents btnAutoStart As System.Windows.Forms.Button
        Friend WithEvents btnAutoStop As System.Windows.Forms.Button
        Friend WithEvents cmsPosition As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents tsmLoad As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsmStore As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents FlowLayoutPanel2 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents ManageMemoryRegistersToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents SetAutorunStartToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents SetAutorunStopHereToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents lblAutoStop As System.Windows.Forms.Label
        Friend WithEvents lblAutoStart As System.Windows.Forms.Label
        Friend WithEvents Label1 As System.Windows.Forms.Label
    End Class

End Namespace
