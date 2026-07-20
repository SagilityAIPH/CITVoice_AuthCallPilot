<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucWorkflowStep
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
        Me.cardStep = New Guna.UI2.WinForms.Guna2Panel()
        Me.tgAnswer = New Guna.UI2.WinForms.Guna2ToggleSwitch()
        Me.lblQuestion = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.btnStepNumber = New Guna.UI2.WinForms.Guna2CircleButton()
        Me.tblHeader = New System.Windows.Forms.TableLayoutPanel()
        Me.flowInstructions = New System.Windows.Forms.FlowLayoutPanel()
        Me.cardStep.SuspendLayout()
        Me.tblHeader.SuspendLayout()
        Me.SuspendLayout()
        '
        'cardStep
        '
        Me.cardStep.BackColor = System.Drawing.Color.Transparent
        Me.cardStep.BorderColor = System.Drawing.Color.Gainsboro
        Me.cardStep.BorderRadius = 15
        Me.cardStep.BorderThickness = 1
        Me.cardStep.Controls.Add(Me.flowInstructions)
        Me.cardStep.Controls.Add(Me.tblHeader)
        Me.cardStep.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cardStep.FillColor = System.Drawing.Color.White
        Me.cardStep.Location = New System.Drawing.Point(0, 0)
        Me.cardStep.Margin = New System.Windows.Forms.Padding(0)
        Me.cardStep.Name = "cardStep"
        Me.cardStep.Padding = New System.Windows.Forms.Padding(15)
        Me.cardStep.ShadowDecoration.Depth = 3
        Me.cardStep.ShadowDecoration.Enabled = True
        Me.cardStep.Size = New System.Drawing.Size(530, 120)
        Me.cardStep.TabIndex = 0
        '
        'tgAnswer
        '
        Me.tgAnswer.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.tgAnswer.CheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.tgAnswer.CheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.tgAnswer.CheckedState.InnerBorderColor = System.Drawing.Color.White
        Me.tgAnswer.CheckedState.InnerColor = System.Drawing.Color.White
        Me.tgAnswer.Cursor = System.Windows.Forms.Cursors.Hand
        Me.tgAnswer.Location = New System.Drawing.Point(453, 17)
        Me.tgAnswer.Margin = New System.Windows.Forms.Padding(0)
        Me.tgAnswer.Name = "tgAnswer"
        Me.tgAnswer.Size = New System.Drawing.Size(38, 20)
        Me.tgAnswer.TabIndex = 2
        Me.tgAnswer.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(125, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(149, Byte), Integer))
        Me.tgAnswer.UncheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(125, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(149, Byte), Integer))
        Me.tgAnswer.UncheckedState.InnerBorderColor = System.Drawing.Color.White
        Me.tgAnswer.UncheckedState.InnerColor = System.Drawing.Color.White
        '
        'lblQuestion
        '
        Me.lblQuestion.AutoSize = False
        Me.lblQuestion.BackColor = System.Drawing.Color.Transparent
        Me.lblQuestion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblQuestion.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblQuestion.Location = New System.Drawing.Point(55, 0)
        Me.lblQuestion.Margin = New System.Windows.Forms.Padding(0)
        Me.lblQuestion.Name = "lblQuestion"
        Me.lblQuestion.Size = New System.Drawing.Size(390, 55)
        Me.lblQuestion.TabIndex = 1
        Me.lblQuestion.Text = "Was there a response?"
        Me.lblQuestion.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnStepNumber
        '
        Me.btnStepNumber.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnStepNumber.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnStepNumber.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnStepNumber.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnStepNumber.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnStepNumber.Enabled = False
        Me.btnStepNumber.FillColor = System.Drawing.Color.Silver
        Me.btnStepNumber.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.btnStepNumber.ForeColor = System.Drawing.Color.White
        Me.btnStepNumber.Location = New System.Drawing.Point(7, 7)
        Me.btnStepNumber.Margin = New System.Windows.Forms.Padding(0)
        Me.btnStepNumber.Name = "btnStepNumber"
        Me.btnStepNumber.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle
        Me.btnStepNumber.Size = New System.Drawing.Size(40, 40)
        Me.btnStepNumber.TabIndex = 0
        Me.btnStepNumber.Text = "1"
        '
        'tblHeader
        '
        Me.tblHeader.ColumnCount = 3
        Me.tblHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55.0!))
        Me.tblHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55.0!))
        Me.tblHeader.Controls.Add(Me.btnStepNumber, 0, 0)
        Me.tblHeader.Controls.Add(Me.lblQuestion, 1, 0)
        Me.tblHeader.Controls.Add(Me.tgAnswer, 2, 0)
        Me.tblHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.tblHeader.Location = New System.Drawing.Point(15, 15)
        Me.tblHeader.Margin = New System.Windows.Forms.Padding(0)
        Me.tblHeader.Name = "tblHeader"
        Me.tblHeader.RowCount = 1
        Me.tblHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblHeader.Size = New System.Drawing.Size(500, 55)
        Me.tblHeader.TabIndex = 3
        '
        'flowInstructions
        '
        Me.flowInstructions.AutoSize = True
        Me.flowInstructions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowInstructions.Dock = System.Windows.Forms.DockStyle.Top
        Me.flowInstructions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flowInstructions.Location = New System.Drawing.Point(15, 15)
        Me.flowInstructions.Margin = New System.Windows.Forms.Padding(0)
        Me.flowInstructions.Name = "flowInstructions"
        Me.flowInstructions.Size = New System.Drawing.Size(500, 0)
        Me.flowInstructions.TabIndex = 4
        Me.flowInstructions.WrapContents = False
        '
        'ucWorkflowStep
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.cardStep)
        Me.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.MinimumSize = New System.Drawing.Size(0, 80)
        Me.Name = "ucWorkflowStep"
        Me.Size = New System.Drawing.Size(530, 120)
        Me.cardStep.ResumeLayout(False)
        Me.cardStep.PerformLayout()
        Me.tblHeader.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents cardStep As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents btnStepNumber As Guna.UI2.WinForms.Guna2CircleButton
    Friend WithEvents tgAnswer As Guna.UI2.WinForms.Guna2ToggleSwitch
    Friend WithEvents lblQuestion As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents tblHeader As TableLayoutPanel
    Friend WithEvents flowInstructions As FlowLayoutPanel
End Class
