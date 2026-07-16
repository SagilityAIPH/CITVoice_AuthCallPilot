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
        Me.btnStepNumber = New Guna.UI2.WinForms.Guna2CircleButton()
        Me.flowInstructions = New System.Windows.Forms.FlowLayoutPanel()
        Me.tblMain = New System.Windows.Forms.TableLayoutPanel()
        Me.lblQuestion = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.tgAnswer = New Guna.UI2.WinForms.Guna2ToggleSwitch()
        Me.cardStep.SuspendLayout()
        Me.tblMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'cardStep
        '
        Me.cardStep.BorderRadius = 15
        Me.cardStep.Controls.Add(Me.tblMain)
        Me.cardStep.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cardStep.FillColor = System.Drawing.Color.White
        Me.cardStep.Location = New System.Drawing.Point(0, 0)
        Me.cardStep.Name = "cardStep"
        Me.cardStep.Padding = New System.Windows.Forms.Padding(15)
        Me.cardStep.Size = New System.Drawing.Size(850, 180)
        Me.cardStep.TabIndex = 0
        '
        'btnStepNumber
        '
        Me.btnStepNumber.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnStepNumber.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnStepNumber.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnStepNumber.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnStepNumber.Enabled = False
        Me.btnStepNumber.FillColor = System.Drawing.Color.DarkSeaGreen
        Me.btnStepNumber.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStepNumber.ForeColor = System.Drawing.Color.White
        Me.btnStepNumber.Location = New System.Drawing.Point(3, 3)
        Me.btnStepNumber.Name = "btnStepNumber"
        Me.btnStepNumber.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle
        Me.btnStepNumber.Size = New System.Drawing.Size(42, 29)
        Me.btnStepNumber.TabIndex = 0
        Me.btnStepNumber.Text = "1"
        '
        'flowInstructions
        '
        Me.flowInstructions.AutoSize = True
        Me.flowInstructions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flowInstructions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flowInstructions.Location = New System.Drawing.Point(63, 38)
        Me.flowInstructions.Name = "flowInstructions"
        Me.flowInstructions.Size = New System.Drawing.Size(644, 109)
        Me.flowInstructions.TabIndex = 2
        '
        'tblMain
        '
        Me.tblMain.ColumnCount = 3
        Me.tblMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
        Me.tblMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110.0!))
        Me.tblMain.Controls.Add(Me.btnStepNumber, 0, 0)
        Me.tblMain.Controls.Add(Me.flowInstructions, 1, 2)
        Me.tblMain.Controls.Add(Me.lblQuestion, 1, 0)
        Me.tblMain.Controls.Add(Me.tgAnswer, 2, 0)
        Me.tblMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblMain.Location = New System.Drawing.Point(15, 15)
        Me.tblMain.Name = "tblMain"
        Me.tblMain.RowCount = 3
        Me.tblMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.tblMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tblMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tblMain.Size = New System.Drawing.Size(820, 150)
        Me.tblMain.TabIndex = 0
        '
        'lblQuestion
        '
        Me.lblQuestion.AutoSize = False
        Me.lblQuestion.BackColor = System.Drawing.Color.Transparent
        Me.lblQuestion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblQuestion.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblQuestion.ForeColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer))
        Me.lblQuestion.Location = New System.Drawing.Point(60, 0)
        Me.lblQuestion.Margin = New System.Windows.Forms.Padding(0)
        Me.lblQuestion.Name = "lblQuestion"
        Me.lblQuestion.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
        Me.lblQuestion.Size = New System.Drawing.Size(650, 35)
        Me.lblQuestion.TabIndex = 3
        Me.lblQuestion.TabStop = False
        Me.lblQuestion.Text = "Was there a response?"
        Me.lblQuestion.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tgAnswer
        '
        Me.tgAnswer.CheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.tgAnswer.CheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.tgAnswer.CheckedState.InnerBorderColor = System.Drawing.Color.White
        Me.tgAnswer.CheckedState.InnerColor = System.Drawing.Color.White
        Me.tgAnswer.Location = New System.Drawing.Point(713, 3)
        Me.tgAnswer.Name = "tgAnswer"
        Me.tgAnswer.Size = New System.Drawing.Size(52, 29)
        Me.tgAnswer.TabIndex = 4
        Me.tgAnswer.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(125, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(149, Byte), Integer))
        Me.tgAnswer.UncheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(125, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(149, Byte), Integer))
        Me.tgAnswer.UncheckedState.InnerBorderColor = System.Drawing.Color.White
        Me.tgAnswer.UncheckedState.InnerColor = System.Drawing.Color.White
        '
        'ucWorkflowStep
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.cardStep)
        Me.Margin = New System.Windows.Forms.Padding(0, 0, 0, 15)
        Me.MinimumSize = New System.Drawing.Size(850, 180)
        Me.Name = "ucWorkflowStep"
        Me.Size = New System.Drawing.Size(850, 180)
        Me.cardStep.ResumeLayout(False)
        Me.tblMain.ResumeLayout(False)
        Me.tblMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents cardStep As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents btnStepNumber As Guna.UI2.WinForms.Guna2CircleButton
    Friend WithEvents tblMain As TableLayoutPanel
    Friend WithEvents flowInstructions As FlowLayoutPanel
    Friend WithEvents lblQuestion As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents tgAnswer As Guna.UI2.WinForms.Guna2ToggleSwitch
End Class
