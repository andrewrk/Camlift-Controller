Namespace CanonCamera

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmLiveView
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
            Me.btnGrid = New System.Windows.Forms.Button
            Me.lblZoom = New System.Windows.Forms.Label
            Me.btnZoomIn = New System.Windows.Forms.Button
            Me.cboWhiteBalance = New System.Windows.Forms.ComboBox
            Me.btnZoomOut = New System.Windows.Forms.Button
            Me.btnClose = New System.Windows.Forms.Button
            Me.picLiveView = New System.Windows.Forms.PictureBox
            Me.Label1 = New System.Windows.Forms.Label
            CType(Me.picLiveView, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'btnGrid
            '
            Me.btnGrid.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnGrid.Location = New System.Drawing.Point(3, 402)
            Me.btnGrid.Name = "btnGrid"
            Me.btnGrid.Size = New System.Drawing.Size(75, 23)
            Me.btnGrid.TabIndex = 6
            Me.btnGrid.Text = "&Grid"
            Me.btnGrid.UseVisualStyleBackColor = True
            '
            'lblZoom
            '
            Me.lblZoom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblZoom.Location = New System.Drawing.Point(165, 399)
            Me.lblZoom.Name = "lblZoom"
            Me.lblZoom.Size = New System.Drawing.Size(40, 29)
            Me.lblZoom.TabIndex = 9
            Me.lblZoom.Text = "100%"
            Me.lblZoom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnZoomIn
            '
            Me.btnZoomIn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnZoomIn.Location = New System.Drawing.Point(84, 402)
            Me.btnZoomIn.Name = "btnZoomIn"
            Me.btnZoomIn.Size = New System.Drawing.Size(75, 23)
            Me.btnZoomIn.TabIndex = 10
            Me.btnZoomIn.Text = "Zoom &In"
            Me.btnZoomIn.UseVisualStyleBackColor = True
            '
            'cboWhiteBalance
            '
            Me.cboWhiteBalance.Anchor = System.Windows.Forms.AnchorStyles.Bottom
            Me.cboWhiteBalance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboWhiteBalance.FormattingEnabled = True
            Me.cboWhiteBalance.Items.AddRange(New Object() {"<Coordinate Based>", "Auto", "Daylight", "Cloudy", "Tungsten", "Fluorescent", "Flash", "Shade", "Color Temperature", "Custom White Balance: PC-1", "Custom White Balance: PC-2", "Custom White Balance: PC-3"})
            Me.cboWhiteBalance.Location = New System.Drawing.Point(392, 404)
            Me.cboWhiteBalance.Name = "cboWhiteBalance"
            Me.cboWhiteBalance.Size = New System.Drawing.Size(121, 21)
            Me.cboWhiteBalance.TabIndex = 4
            '
            'btnZoomOut
            '
            Me.btnZoomOut.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnZoomOut.Location = New System.Drawing.Point(211, 402)
            Me.btnZoomOut.Name = "btnZoomOut"
            Me.btnZoomOut.Size = New System.Drawing.Size(75, 23)
            Me.btnZoomOut.TabIndex = 7
            Me.btnZoomOut.Text = "Zoom &Out"
            Me.btnZoomOut.UseVisualStyleBackColor = True
            '
            'btnClose
            '
            Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnClose.Location = New System.Drawing.Point(557, 402)
            Me.btnClose.Name = "btnClose"
            Me.btnClose.Size = New System.Drawing.Size(75, 23)
            Me.btnClose.TabIndex = 5
            Me.btnClose.Text = "&Close"
            Me.btnClose.UseVisualStyleBackColor = True
            '
            'picLiveView
            '
            Me.picLiveView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.picLiveView.Cursor = System.Windows.Forms.Cursors.SizeAll
            Me.picLiveView.Location = New System.Drawing.Point(3, 3)
            Me.picLiveView.Name = "picLiveView"
            Me.picLiveView.Size = New System.Drawing.Size(629, 393)
            Me.picLiveView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.picLiveView.TabIndex = 8
            Me.picLiveView.TabStop = False
            '
            'Label1
            '
            Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(309, 407)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(80, 13)
            Me.Label1.TabIndex = 11
            Me.Label1.Text = "White B&alance:"
            '
            'frmLiveView
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(644, 434)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.btnGrid)
            Me.Controls.Add(Me.lblZoom)
            Me.Controls.Add(Me.btnZoomIn)
            Me.Controls.Add(Me.cboWhiteBalance)
            Me.Controls.Add(Me.btnZoomOut)
            Me.Controls.Add(Me.btnClose)
            Me.Controls.Add(Me.picLiveView)
            Me.MinimumSize = New System.Drawing.Size(660, 470)
            Me.Name = "frmLiveView"
            Me.ShowIcon = False
            Me.Text = "Live View"
            CType(Me.picLiveView, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents btnGrid As System.Windows.Forms.Button
        Friend WithEvents lblZoom As System.Windows.Forms.Label
        Friend WithEvents btnZoomIn As System.Windows.Forms.Button
        Friend WithEvents cboWhiteBalance As System.Windows.Forms.ComboBox
        Friend WithEvents btnZoomOut As System.Windows.Forms.Button
        Friend WithEvents btnClose As System.Windows.Forms.Button
        Friend WithEvents picLiveView As System.Windows.Forms.PictureBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
    End Class
End Namespace