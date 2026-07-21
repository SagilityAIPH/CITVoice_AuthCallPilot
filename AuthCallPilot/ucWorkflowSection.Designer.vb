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
        Me.cardSection = New Guna.UI2.WinForms.Guna2Panel()
        Me.flowSteps = New System.Windows.Forms.FlowLayoutPanel()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.lblTitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.lblExpand = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.cardSection.SuspendLayout()
        Me.pnlHeader.SuspendLayout()
        Me.SuspendLayout()
        '
        'cardSection
        '
        Me.cardSection.BorderRadius = 15
        Me.cardSection.BorderThickness = 1
        Me.cardSection.Controls.Add(Me.flowSteps)
        Me.cardSection.Controls.Add(Me.pnlHeader)
        Me.cardSection.Dock = System.Windows.Forms.DockStyle.Top
        Me.cardSection.FillColor = System.Drawing.Color.White
        Me.cardSection.Location = New System.Drawing.Point(0, 0)
        Me.cardSection.Name = "cardSection"
        Me.cardSection.Padding = New System.Windows.Forms.Padding(10)
        Me.cardSection.Size = New System.Drawing.Size(235, 100)
        Me.cardSection.TabIndex = 0
        '
        'flowSteps
        '
        Me.flowSteps.AutoSize = True
        Me.flowSteps.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowSteps.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flowSteps.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flowSteps.Location = New System.Drawing.Point(10, 50)
        Me.flowSteps.Margin = New System.Windows.Forms.Padding(0)
        Me.flowSteps.Name = "flowSteps"
        Me.flowSteps.Padding = New System.Windows.Forms.Padding(5)
        Me.flowSteps.Size = New System.Drawing.Size(215, 40)
        Me.flowSteps.TabIndex = 1
        Me.flowSteps.WrapContents = False
        '
        'pnlHeader
        '
        Me.pnlHeader.Controls.Add(Me.lblTitle)
        Me.pnlHeader.Controls.Add(Me.lblExpand)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(10, 10)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(215, 40)
        Me.pnlHeader.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(35, 10)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(108, 22)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "Authentication"
        '
        'lblExpand
        '
        Me.lblExpand.BackColor = System.Drawing.Color.Transparent
        Me.lblExpand.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExpand.Location = New System.Drawing.Point(10, 10)
        Me.lblExpand.Name = "lblExpand"
        Me.lblExpand.Size = New System.Drawing.Size(17, 23)
        Me.lblExpand.TabIndex = 0
        Me.lblExpand.Text = "▼"
        '
        'ucWorkflowSection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.cardSection)
        Me.Name = "ucWorkflowSection"
        Me.Size = New System.Drawing.Size(235, 150)
        Me.cardSection.ResumeLayout(False)
        Me.cardSection.PerformLayout()
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents cardSection As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents pnlHeader As Panel
    Friend WithEvents lblTitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents lblExpand As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents flowSteps As FlowLayoutPanel
End Class
