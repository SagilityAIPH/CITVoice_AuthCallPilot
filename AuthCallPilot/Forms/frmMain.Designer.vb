<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits MaterialSkin.Controls.MaterialForm

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
        Me.TableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.pnlTopInputs = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.txtFaxRef = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Guna2TextBox1 = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtCallback = New Guna.UI2.WinForms.Guna2TextBox()
        Me.pnlMiddleChecklist = New System.Windows.Forms.Panel()
        Me.Guna2Panel1 = New Guna.UI2.WinForms.Guna2Panel()
        Me.pnlBottomBanners = New System.Windows.Forms.Panel()
        Me.flowWorkflow = New System.Windows.Forms.FlowLayoutPanel()
        Me.TableLayoutPanel.SuspendLayout()
        Me.pnlTopInputs.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.pnlMiddleChecklist.SuspendLayout()
        Me.Guna2Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel
        '
        Me.TableLayoutPanel.ColumnCount = 1
        Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel.Controls.Add(Me.pnlTopInputs, 0, 0)
        Me.TableLayoutPanel.Controls.Add(Me.pnlMiddleChecklist, 0, 1)
        Me.TableLayoutPanel.Controls.Add(Me.pnlBottomBanners, 0, 2)
        Me.TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel.Location = New System.Drawing.Point(3, 64)
        Me.TableLayoutPanel.Name = "TableLayoutPanel"
        Me.TableLayoutPanel.RowCount = 3
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110.0!))
        Me.TableLayoutPanel.Size = New System.Drawing.Size(428, 383)
        Me.TableLayoutPanel.TabIndex = 0
        '
        'pnlTopInputs
        '
        Me.pnlTopInputs.Controls.Add(Me.TableLayoutPanel1)
        Me.pnlTopInputs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlTopInputs.Location = New System.Drawing.Point(3, 3)
        Me.pnlTopInputs.Name = "pnlTopInputs"
        Me.pnlTopInputs.Size = New System.Drawing.Size(422, 114)
        Me.pnlTopInputs.TabIndex = 0
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334!))
        Me.TableLayoutPanel1.Controls.Add(Me.txtFaxRef, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Guna2TextBox1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.txtCallback, 1, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.Padding = New System.Windows.Forms.Padding(10)
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 94.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(422, 114)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'txtFaxRef
        '
        Me.txtFaxRef.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFaxRef.BorderColor = System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(214, Byte), Integer), CType(CType(219, Byte), Integer))
        Me.txtFaxRef.BorderRadius = 6
        Me.txtFaxRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFaxRef.DefaultText = ""
        Me.txtFaxRef.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtFaxRef.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtFaxRef.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtFaxRef.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtFaxRef.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(95, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.txtFaxRef.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtFaxRef.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtFaxRef.Location = New System.Drawing.Point(280, 39)
        Me.txtFaxRef.Name = "txtFaxRef"
        Me.txtFaxRef.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.txtFaxRef.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(150, Byte), Integer))
        Me.txtFaxRef.PlaceholderText = "Fax #"
        Me.txtFaxRef.SelectedText = ""
        Me.txtFaxRef.Size = New System.Drawing.Size(129, 36)
        Me.txtFaxRef.TabIndex = 5
        Me.txtFaxRef.TextOffset = New System.Drawing.Point(8, 0)
        '
        'Guna2TextBox1
        '
        Me.Guna2TextBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Guna2TextBox1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(214, Byte), Integer), CType(CType(219, Byte), Integer))
        Me.Guna2TextBox1.BorderRadius = 6
        Me.Guna2TextBox1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Guna2TextBox1.DefaultText = ""
        Me.Guna2TextBox1.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Guna2TextBox1.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Guna2TextBox1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Guna2TextBox1.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Guna2TextBox1.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(95, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.Guna2TextBox1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Guna2TextBox1.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Guna2TextBox1.Location = New System.Drawing.Point(13, 39)
        Me.Guna2TextBox1.Name = "Guna2TextBox1"
        Me.Guna2TextBox1.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.Guna2TextBox1.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(150, Byte), Integer))
        Me.Guna2TextBox1.PlaceholderText = "Caller Name"
        Me.Guna2TextBox1.SelectedText = ""
        Me.Guna2TextBox1.Size = New System.Drawing.Size(127, 36)
        Me.Guna2TextBox1.TabIndex = 3
        Me.Guna2TextBox1.TextOffset = New System.Drawing.Point(8, 0)
        '
        'txtCallback
        '
        Me.txtCallback.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCallback.BorderColor = System.Drawing.Color.FromArgb(CType(CType(210, Byte), Integer), CType(CType(214, Byte), Integer), CType(CType(219, Byte), Integer))
        Me.txtCallback.BorderRadius = 6
        Me.txtCallback.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCallback.DefaultText = ""
        Me.txtCallback.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtCallback.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtCallback.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtCallback.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtCallback.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(95, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.txtCallback.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtCallback.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtCallback.Location = New System.Drawing.Point(146, 39)
        Me.txtCallback.Name = "txtCallback"
        Me.txtCallback.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.txtCallback.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(150, Byte), Integer))
        Me.txtCallback.PlaceholderText = "Callback"
        Me.txtCallback.SelectedText = ""
        Me.txtCallback.Size = New System.Drawing.Size(128, 36)
        Me.txtCallback.TabIndex = 4
        Me.txtCallback.TextOffset = New System.Drawing.Point(8, 0)
        '
        'pnlMiddleChecklist
        '
        Me.pnlMiddleChecklist.AutoScroll = True
        Me.pnlMiddleChecklist.Controls.Add(Me.Guna2Panel1)
        Me.pnlMiddleChecklist.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMiddleChecklist.Location = New System.Drawing.Point(3, 123)
        Me.pnlMiddleChecklist.Name = "pnlMiddleChecklist"
        Me.pnlMiddleChecklist.Size = New System.Drawing.Size(422, 114)
        Me.pnlMiddleChecklist.TabIndex = 1
        '
        'Guna2Panel1
        '
        Me.Guna2Panel1.BorderRadius = 16
        Me.Guna2Panel1.Controls.Add(Me.flowWorkflow)
        Me.Guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Guna2Panel1.FillColor = System.Drawing.Color.White
        Me.Guna2Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Guna2Panel1.Name = "Guna2Panel1"
        Me.Guna2Panel1.Size = New System.Drawing.Size(422, 114)
        Me.Guna2Panel1.TabIndex = 0
        '
        'pnlBottomBanners
        '
        Me.pnlBottomBanners.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlBottomBanners.Location = New System.Drawing.Point(3, 243)
        Me.pnlBottomBanners.Name = "pnlBottomBanners"
        Me.pnlBottomBanners.Size = New System.Drawing.Size(422, 137)
        Me.pnlBottomBanners.TabIndex = 2
        '
        'flowWorkflow
        '
        Me.flowWorkflow.AutoScroll = True
        Me.flowWorkflow.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flowWorkflow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flowWorkflow.Location = New System.Drawing.Point(0, 0)
        Me.flowWorkflow.Name = "flowWorkflow"
        Me.flowWorkflow.Padding = New System.Windows.Forms.Padding(15)
        Me.flowWorkflow.Size = New System.Drawing.Size(422, 114)
        Me.flowWorkflow.TabIndex = 0
        Me.flowWorkflow.WrapContents = False
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(434, 450)
        Me.Controls.Add(Me.TableLayoutPanel)
        Me.Name = "frmMain"
        Me.Text = "AuthCallPilot"
        Me.TableLayoutPanel.ResumeLayout(False)
        Me.pnlTopInputs.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.pnlMiddleChecklist.ResumeLayout(False)
        Me.Guna2Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel As TableLayoutPanel
    Friend WithEvents pnlTopInputs As Panel
    Friend WithEvents pnlMiddleChecklist As Panel
    Friend WithEvents Guna2Panel1 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents pnlBottomBanners As Panel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Guna2TextBox1 As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtFaxRef As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtCallback As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents flowWorkflow As FlowLayoutPanel
End Class
