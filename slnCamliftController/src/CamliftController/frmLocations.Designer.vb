Namespace CamliftController

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmLocations
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
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.lsvLocations = New System.Windows.Forms.ListView
            Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
            Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
            Me.Panel2 = New System.Windows.Forms.Panel
            Me.btnDelete = New System.Windows.Forms.Button
            Me.btnLoad = New System.Windows.Forms.Button
            Me.btnClose = New System.Windows.Forms.Button
            Me.TableLayoutPanel1.SuspendLayout()
            Me.Panel1.SuspendLayout()
            Me.Panel2.SuspendLayout()
            Me.SuspendLayout()
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.ColumnCount = 1
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.Panel2, 0, 1)
            Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 2
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(292, 266)
            Me.TableLayoutPanel1.TabIndex = 0
            '
            'Panel1
            '
            Me.Panel1.Controls.Add(Me.lsvLocations)
            Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Panel1.Location = New System.Drawing.Point(3, 3)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Padding = New System.Windows.Forms.Padding(10)
            Me.Panel1.Size = New System.Drawing.Size(286, 225)
            Me.Panel1.TabIndex = 0
            '
            'lsvLocations
            '
            Me.lsvLocations.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
            Me.lsvLocations.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lsvLocations.FullRowSelect = True
            Me.lsvLocations.Location = New System.Drawing.Point(10, 10)
            Me.lsvLocations.MultiSelect = False
            Me.lsvLocations.Name = "lsvLocations"
            Me.lsvLocations.Size = New System.Drawing.Size(266, 205)
            Me.lsvLocations.TabIndex = 0
            Me.lsvLocations.UseCompatibleStateImageBehavior = False
            Me.lsvLocations.View = System.Windows.Forms.View.Details
            '
            'ColumnHeader1
            '
            Me.ColumnHeader1.Text = "Name"
            Me.ColumnHeader1.Width = 176
            '
            'ColumnHeader2
            '
            Me.ColumnHeader2.Text = "Value"
            Me.ColumnHeader2.Width = 67
            '
            'Panel2
            '
            Me.Panel2.Controls.Add(Me.btnDelete)
            Me.Panel2.Controls.Add(Me.btnLoad)
            Me.Panel2.Controls.Add(Me.btnClose)
            Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Panel2.Location = New System.Drawing.Point(3, 234)
            Me.Panel2.Name = "Panel2"
            Me.Panel2.Size = New System.Drawing.Size(286, 29)
            Me.Panel2.TabIndex = 1
            '
            'btnDelete
            '
            Me.btnDelete.Enabled = False
            Me.btnDelete.Location = New System.Drawing.Point(91, 3)
            Me.btnDelete.Name = "btnDelete"
            Me.btnDelete.Size = New System.Drawing.Size(75, 23)
            Me.btnDelete.TabIndex = 2
            Me.btnDelete.Text = "Delete"
            Me.btnDelete.UseVisualStyleBackColor = True
            '
            'btnLoad
            '
            Me.btnLoad.Enabled = False
            Me.btnLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnLoad.Location = New System.Drawing.Point(10, 3)
            Me.btnLoad.Name = "btnLoad"
            Me.btnLoad.Size = New System.Drawing.Size(75, 23)
            Me.btnLoad.TabIndex = 1
            Me.btnLoad.Text = "Load"
            Me.btnLoad.UseVisualStyleBackColor = True
            '
            'btnClose
            '
            Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnClose.Location = New System.Drawing.Point(201, 3)
            Me.btnClose.Name = "btnClose"
            Me.btnClose.Size = New System.Drawing.Size(75, 23)
            Me.btnClose.TabIndex = 0
            Me.btnClose.Text = "Close"
            Me.btnClose.UseVisualStyleBackColor = True
            '
            'frmLocations
            '
            Me.AcceptButton = Me.btnLoad
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnClose
            Me.ClientSize = New System.Drawing.Size(292, 266)
            Me.Controls.Add(Me.TableLayoutPanel1)
            Me.Name = "frmLocations"
            Me.Text = "Saved Locations"
            Me.TableLayoutPanel1.ResumeLayout(False)
            Me.Panel1.ResumeLayout(False)
            Me.Panel2.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents lsvLocations As System.Windows.Forms.ListView
        Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
        Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents btnClose As System.Windows.Forms.Button
        Friend WithEvents btnDelete As System.Windows.Forms.Button
        Friend WithEvents btnLoad As System.Windows.Forms.Button
    End Class

End Namespace
