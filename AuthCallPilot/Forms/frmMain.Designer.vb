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
        Me.btnLaunchBrowser = New Guna.UI2.WinForms.Guna2Button()
        Me.Guna2Panel1 = New Guna.UI2.WinForms.Guna2Panel()
        Me.pnlOverallActions = New Guna.UI2.WinForms.Guna2Panel()
        Me.pnlActions = New Guna.UI2.WinForms.Guna2Panel()
        Me.cmbScenario = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.txtOverAllOutput = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtNextBestAction = New Guna.UI2.WinForms.Guna2TextBox()
        Me.btnAnalyze = New Guna.UI2.WinForms.Guna2Button()
        Me.txtMemberId = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtDOB = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtCallerLastName = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtCallerFirstName = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Guna2HtmlLabel1 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2HtmlLabel2 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2Panel1.SuspendLayout()
        Me.pnlOverallActions.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnLaunchBrowser
        '
        Me.btnLaunchBrowser.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnLaunchBrowser.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnLaunchBrowser.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnLaunchBrowser.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnLaunchBrowser.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLaunchBrowser.ForeColor = System.Drawing.Color.White
        Me.btnLaunchBrowser.Location = New System.Drawing.Point(255, 33)
        Me.btnLaunchBrowser.Name = "btnLaunchBrowser"
        Me.btnLaunchBrowser.Size = New System.Drawing.Size(119, 25)
        Me.btnLaunchBrowser.TabIndex = 1
        Me.btnLaunchBrowser.Text = "Launch Browser"
        '
        'Guna2Panel1
        '
        Me.Guna2Panel1.Controls.Add(Me.Guna2HtmlLabel2)
        Me.Guna2Panel1.Controls.Add(Me.Guna2HtmlLabel1)
        Me.Guna2Panel1.Controls.Add(Me.pnlOverallActions)
        Me.Guna2Panel1.Controls.Add(Me.txtOverAllOutput)
        Me.Guna2Panel1.Controls.Add(Me.txtNextBestAction)
        Me.Guna2Panel1.Controls.Add(Me.btnAnalyze)
        Me.Guna2Panel1.Controls.Add(Me.txtMemberId)
        Me.Guna2Panel1.Controls.Add(Me.txtDOB)
        Me.Guna2Panel1.Controls.Add(Me.txtCallerLastName)
        Me.Guna2Panel1.Controls.Add(Me.txtCallerFirstName)
        Me.Guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Guna2Panel1.Location = New System.Drawing.Point(3, 64)
        Me.Guna2Panel1.Name = "Guna2Panel1"
        Me.Guna2Panel1.Size = New System.Drawing.Size(386, 602)
        Me.Guna2Panel1.TabIndex = 2
        '
        'pnlOverallActions
        '
        Me.pnlOverallActions.Controls.Add(Me.pnlActions)
        Me.pnlOverallActions.Controls.Add(Me.cmbScenario)
        Me.pnlOverallActions.Location = New System.Drawing.Point(19, 99)
        Me.pnlOverallActions.Name = "pnlOverallActions"
        Me.pnlOverallActions.Size = New System.Drawing.Size(340, 223)
        Me.pnlOverallActions.TabIndex = 8
        '
        'pnlActions
        '
        Me.pnlActions.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.pnlActions.Location = New System.Drawing.Point(4, 53)
        Me.pnlActions.Name = "pnlActions"
        Me.pnlActions.Size = New System.Drawing.Size(333, 167)
        Me.pnlActions.TabIndex = 5
        '
        'cmbScenario
        '
        Me.cmbScenario.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbScenario.AutoRoundedCorners = True
        Me.cmbScenario.BackColor = System.Drawing.Color.Transparent
        Me.cmbScenario.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cmbScenario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbScenario.FocusedColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cmbScenario.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cmbScenario.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.cmbScenario.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(112, Byte), Integer))
        Me.cmbScenario.ItemHeight = 30
        Me.cmbScenario.Items.AddRange(New Object() {"New Authorization", "Updating Authorization", "Checking Status Of The Authorization"})
        Me.cmbScenario.Location = New System.Drawing.Point(29, 11)
        Me.cmbScenario.Name = "cmbScenario"
        Me.cmbScenario.Size = New System.Drawing.Size(271, 36)
        Me.cmbScenario.TabIndex = 4
        '
        'txtOverAllOutput
        '
        Me.txtOverAllOutput.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOverAllOutput.DefaultText = ""
        Me.txtOverAllOutput.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtOverAllOutput.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtOverAllOutput.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtOverAllOutput.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtOverAllOutput.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtOverAllOutput.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtOverAllOutput.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtOverAllOutput.Location = New System.Drawing.Point(19, 476)
        Me.txtOverAllOutput.Multiline = True
        Me.txtOverAllOutput.Name = "txtOverAllOutput"
        Me.txtOverAllOutput.PlaceholderText = ""
        Me.txtOverAllOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtOverAllOutput.SelectedText = ""
        Me.txtOverAllOutput.Size = New System.Drawing.Size(340, 114)
        Me.txtOverAllOutput.TabIndex = 7
        '
        'txtNextBestAction
        '
        Me.txtNextBestAction.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNextBestAction.DefaultText = ""
        Me.txtNextBestAction.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtNextBestAction.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtNextBestAction.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtNextBestAction.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtNextBestAction.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtNextBestAction.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtNextBestAction.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtNextBestAction.Location = New System.Drawing.Point(19, 349)
        Me.txtNextBestAction.Multiline = True
        Me.txtNextBestAction.Name = "txtNextBestAction"
        Me.txtNextBestAction.PlaceholderText = ""
        Me.txtNextBestAction.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtNextBestAction.SelectedText = ""
        Me.txtNextBestAction.Size = New System.Drawing.Size(340, 100)
        Me.txtNextBestAction.TabIndex = 6
        '
        'btnAnalyze
        '
        Me.btnAnalyze.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnAnalyze.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnAnalyze.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnAnalyze.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnAnalyze.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnAnalyze.ForeColor = System.Drawing.Color.White
        Me.btnAnalyze.Location = New System.Drawing.Point(240, 68)
        Me.btnAnalyze.Name = "btnAnalyze"
        Me.btnAnalyze.Size = New System.Drawing.Size(119, 25)
        Me.btnAnalyze.TabIndex = 5
        Me.btnAnalyze.Text = "Analyze"
        '
        'txtMemberId
        '
        Me.txtMemberId.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMemberId.DefaultText = ""
        Me.txtMemberId.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtMemberId.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtMemberId.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtMemberId.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtMemberId.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtMemberId.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMemberId.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtMemberId.Location = New System.Drawing.Point(216, 12)
        Me.txtMemberId.Name = "txtMemberId"
        Me.txtMemberId.PlaceholderText = "Member ID"
        Me.txtMemberId.SelectedText = ""
        Me.txtMemberId.Size = New System.Drawing.Size(143, 22)
        Me.txtMemberId.TabIndex = 3
        '
        'txtDOB
        '
        Me.txtDOB.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDOB.DefaultText = ""
        Me.txtDOB.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtDOB.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtDOB.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtDOB.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtDOB.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDOB.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDOB.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDOB.Location = New System.Drawing.Point(216, 40)
        Me.txtDOB.Name = "txtDOB"
        Me.txtDOB.PlaceholderText = "Date of Birth (MMddYY)"
        Me.txtDOB.SelectedText = ""
        Me.txtDOB.Size = New System.Drawing.Size(143, 22)
        Me.txtDOB.TabIndex = 2
        '
        'txtCallerLastName
        '
        Me.txtCallerLastName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCallerLastName.DefaultText = ""
        Me.txtCallerLastName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtCallerLastName.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtCallerLastName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtCallerLastName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtCallerLastName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtCallerLastName.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCallerLastName.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtCallerLastName.Location = New System.Drawing.Point(19, 40)
        Me.txtCallerLastName.Name = "txtCallerLastName"
        Me.txtCallerLastName.PlaceholderText = "Last Name"
        Me.txtCallerLastName.SelectedText = ""
        Me.txtCallerLastName.Size = New System.Drawing.Size(143, 22)
        Me.txtCallerLastName.TabIndex = 1
        '
        'txtCallerFirstName
        '
        Me.txtCallerFirstName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCallerFirstName.DefaultText = ""
        Me.txtCallerFirstName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtCallerFirstName.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtCallerFirstName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtCallerFirstName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtCallerFirstName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtCallerFirstName.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCallerFirstName.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtCallerFirstName.Location = New System.Drawing.Point(19, 12)
        Me.txtCallerFirstName.Name = "txtCallerFirstName"
        Me.txtCallerFirstName.PlaceholderText = "First Name"
        Me.txtCallerFirstName.SelectedText = ""
        Me.txtCallerFirstName.Size = New System.Drawing.Size(143, 22)
        Me.txtCallerFirstName.TabIndex = 0
        '
        'Guna2HtmlLabel1
        '
        Me.Guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel1.Location = New System.Drawing.Point(19, 455)
        Me.Guna2HtmlLabel1.Name = "Guna2HtmlLabel1"
        Me.Guna2HtmlLabel1.Size = New System.Drawing.Size(71, 15)
        Me.Guna2HtmlLabel1.TabIndex = 9
        Me.Guna2HtmlLabel1.Text = "Overall Output"
        '
        'Guna2HtmlLabel2
        '
        Me.Guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel2.Location = New System.Drawing.Point(19, 328)
        Me.Guna2HtmlLabel2.Name = "Guna2HtmlLabel2"
        Me.Guna2HtmlLabel2.Size = New System.Drawing.Size(82, 15)
        Me.Guna2HtmlLabel2.TabIndex = 10
        Me.Guna2HtmlLabel2.Text = "Next Best Action"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(392, 669)
        Me.Controls.Add(Me.Guna2Panel1)
        Me.Controls.Add(Me.btnLaunchBrowser)
        Me.Name = "frmMain"
        Me.Text = "AuthCallPilot"
        Me.Guna2Panel1.ResumeLayout(False)
        Me.Guna2Panel1.PerformLayout()
        Me.pnlOverallActions.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnLaunchBrowser As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2Panel1 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents txtCallerLastName As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtCallerFirstName As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtMemberId As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtDOB As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents cmbScenario As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents btnAnalyze As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents pnlOverallActions As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents pnlActions As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents txtOverAllOutput As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtNextBestAction As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Guna2HtmlLabel2 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel1 As Guna.UI2.WinForms.Guna2HtmlLabel
End Class
