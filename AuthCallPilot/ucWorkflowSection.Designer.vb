<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucWorkflowSection
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.tblMain = New System.Windows.Forms.TableLayoutPanel()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.tblHeader = New System.Windows.Forms.TableLayoutPanel()
        Me.lblExpand = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.lblTitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.flowSteps = New System.Windows.Forms.FlowLayoutPanel()
        Me.tblMain.SuspendLayout()
        Me.pnlHeader.SuspendLayout()
        Me.tblHeader.SuspendLayout()
        Me.SuspendLayout()
        '
        'tblMain
        '
        Me.tblMain.AutoSize = True
        Me.tblMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tblMain.ColumnCount = 1
        Me.tblMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblMain.Controls.Add(Me.pnlHeader, 0, 0)
        Me.tblMain.Controls.Add(Me.flowSteps, 0, 1)
        Me.tblMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblMain.Location = New System.Drawing.Point(0, 0)
        Me.tblMain.Margin = New System.Windows.Forms.Padding(0)
        Me.tblMain.Name = "tblMain"
        Me.tblMain.RowCount = 2
        Me.tblMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45.0!))
        Me.tblMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tblMain.Size = New System.Drawing.Size(200, 55)
        Me.tblMain.TabIndex = 0
        '
        'pnlHeader
        '
        Me.pnlHeader.Controls.Add(Me.tblHeader)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Padding = New System.Windows.Forms.Padding(10, 5, 10, 5)
        Me.pnlHeader.Size = New System.Drawing.Size(200, 45)
        Me.pnlHeader.TabIndex = 0
        '
        'tblHeader
        '
        Me.tblHeader.ColumnCount = 3
        Me.tblHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.tblHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.tblHeader.Controls.Add(Me.lblExpand, 0, 0)
        Me.tblHeader.Controls.Add(Me.lblTitle, 1, 0)
        Me.tblHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblHeader.Location = New System.Drawing.Point(10, 5)
        Me.tblHeader.Name = "tblHeader"
        Me.tblHeader.RowCount = 1
        Me.tblHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tblHeader.Size = New System.Drawing.Size(180, 35)
        Me.tblHeader.TabIndex = 0
        '
        'lblExpand
        '
        Me.lblExpand.BackColor = System.Drawing.Color.Transparent
        Me.lblExpand.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExpand.Location = New System.Drawing.Point(3, 3)
        Me.lblExpand.Name = "lblExpand"
        Me.lblExpand.Size = New System.Drawing.Size(17, 23)
        Me.lblExpand.TabIndex = 0
        Me.lblExpand.Text = "▼"
        Me.lblExpand.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(33, 3)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(108, 22)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "Authentication"
        Me.lblTitle.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        '
        'flowSteps
        '
        Me.flowSteps.AutoSize = True
        Me.flowSteps.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowSteps.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flowSteps.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flowSteps.Location = New System.Drawing.Point(0, 45)
        Me.flowSteps.Margin = New System.Windows.Forms.Padding(0)
        Me.flowSteps.Name = "flowSteps"
        Me.flowSteps.Padding = New System.Windows.Forms.Padding(5)
        Me.flowSteps.Size = New System.Drawing.Size(200, 10)
        Me.flowSteps.TabIndex = 1
        Me.flowSteps.WrapContents = False
        '
        'ucWorkflowSection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.tblMain)
        Me.Margin = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.Name = "ucWorkflowSection"
        Me.Size = New System.Drawing.Size(200, 55)
        Me.tblMain.ResumeLayout(False)
        Me.tblMain.PerformLayout()
        Me.pnlHeader.ResumeLayout(False)
        Me.tblHeader.ResumeLayout(False)
        Me.tblHeader.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tblMain As TableLayoutPanel
    Friend WithEvents pnlHeader As Panel
    Friend WithEvents tblHeader As TableLayoutPanel
    Friend WithEvents lblExpand As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents lblTitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents flowSteps As FlowLayoutPanel
End Class
