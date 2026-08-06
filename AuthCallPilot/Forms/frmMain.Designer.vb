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
        Me.txtDOS = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtAuthNumber = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Guna2HtmlLabel2 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2HtmlLabel1 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.pnlOverallActions = New Guna.UI2.WinForms.Guna2Panel()
        Me.btnSelectScenario = New Guna.UI2.WinForms.Guna2Button()
        Me.pnlActions = New Guna.UI2.WinForms.Guna2Panel()
        Me.cmbScenario = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.txtOverAllOutput = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtNextBestAction = New Guna.UI2.WinForms.Guna2TextBox()
        Me.btnAnalyze = New Guna.UI2.WinForms.Guna2Button()
        Me.txtSecuredFax = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtCallingFrom = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtCallbackNum = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtCallerName = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Guna2Panel1.SuspendLayout()
        Me.pnlOverallActions.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnLaunchBrowser
        '
        Me.btnLaunchBrowser.BackColor = System.Drawing.Color.Transparent
        Me.btnLaunchBrowser.BorderRadius = 10
        Me.btnLaunchBrowser.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnLaunchBrowser.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnLaunchBrowser.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnLaunchBrowser.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnLaunchBrowser.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLaunchBrowser.ForeColor = System.Drawing.Color.White
        Me.btnLaunchBrowser.Location = New System.Drawing.Point(613, 33)
        Me.btnLaunchBrowser.Name = "btnLaunchBrowser"
        Me.btnLaunchBrowser.Size = New System.Drawing.Size(119, 25)
        Me.btnLaunchBrowser.TabIndex = 1
        Me.btnLaunchBrowser.Text = "Launch Browser"
        '
        'Guna2Panel1
        '
        Me.Guna2Panel1.Controls.Add(Me.txtDOS)
        Me.Guna2Panel1.Controls.Add(Me.txtAuthNumber)
        Me.Guna2Panel1.Controls.Add(Me.Guna2HtmlLabel2)
        Me.Guna2Panel1.Controls.Add(Me.Guna2HtmlLabel1)
        Me.Guna2Panel1.Controls.Add(Me.pnlOverallActions)
        Me.Guna2Panel1.Controls.Add(Me.txtOverAllOutput)
        Me.Guna2Panel1.Controls.Add(Me.txtNextBestAction)
        Me.Guna2Panel1.Controls.Add(Me.btnAnalyze)
        Me.Guna2Panel1.Controls.Add(Me.txtSecuredFax)
        Me.Guna2Panel1.Controls.Add(Me.txtCallingFrom)
        Me.Guna2Panel1.Controls.Add(Me.txtCallbackNum)
        Me.Guna2Panel1.Controls.Add(Me.txtCallerName)
        Me.Guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Guna2Panel1.Location = New System.Drawing.Point(3, 64)
        Me.Guna2Panel1.Name = "Guna2Panel1"
        Me.Guna2Panel1.Size = New System.Drawing.Size(740, 463)
        Me.Guna2Panel1.TabIndex = 2
        '
        'txtDOS
        '
        Me.txtDOS.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDOS.DefaultText = ""
        Me.txtDOS.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtDOS.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtDOS.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtDOS.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtDOS.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDOS.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDOS.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDOS.Location = New System.Drawing.Point(207, 68)
        Me.txtDOS.Name = "txtDOS"
        Me.txtDOS.PlaceholderText = "Date of Service(MMddYY)"
        Me.txtDOS.SelectedText = ""
        Me.txtDOS.Size = New System.Drawing.Size(164, 22)
        Me.txtDOS.TabIndex = 12
        '
        'txtAuthNumber
        '
        Me.txtAuthNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAuthNumber.DefaultText = ""
        Me.txtAuthNumber.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtAuthNumber.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtAuthNumber.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtAuthNumber.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtAuthNumber.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtAuthNumber.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAuthNumber.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtAuthNumber.Location = New System.Drawing.Point(19, 68)
        Me.txtAuthNumber.Name = "txtAuthNumber"
        Me.txtAuthNumber.PlaceholderText = "Auth Number"
        Me.txtAuthNumber.SelectedText = ""
        Me.txtAuthNumber.Size = New System.Drawing.Size(164, 22)
        Me.txtAuthNumber.TabIndex = 11
        '
        'Guna2HtmlLabel2
        '
        Me.Guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel2.Location = New System.Drawing.Point(389, 12)
        Me.Guna2HtmlLabel2.Name = "Guna2HtmlLabel2"
        Me.Guna2HtmlLabel2.Size = New System.Drawing.Size(82, 15)
        Me.Guna2HtmlLabel2.TabIndex = 10
        Me.Guna2HtmlLabel2.Text = "Next Best Action"
        '
        'Guna2HtmlLabel1
        '
        Me.Guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel1.Location = New System.Drawing.Point(389, 159)
        Me.Guna2HtmlLabel1.Name = "Guna2HtmlLabel1"
        Me.Guna2HtmlLabel1.Size = New System.Drawing.Size(71, 15)
        Me.Guna2HtmlLabel1.TabIndex = 9
        Me.Guna2HtmlLabel1.Text = "Overall Output"
        '
        'pnlOverallActions
        '
        Me.pnlOverallActions.Controls.Add(Me.btnSelectScenario)
        Me.pnlOverallActions.Controls.Add(Me.pnlActions)
        Me.pnlOverallActions.Controls.Add(Me.cmbScenario)
        Me.pnlOverallActions.Location = New System.Drawing.Point(19, 150)
        Me.pnlOverallActions.Name = "pnlOverallActions"
        Me.pnlOverallActions.Size = New System.Drawing.Size(364, 310)
        Me.pnlOverallActions.TabIndex = 8
        '
        'btnSelectScenario
        '
        Me.btnSelectScenario.BorderRadius = 10
        Me.btnSelectScenario.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnSelectScenario.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnSelectScenario.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnSelectScenario.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnSelectScenario.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnSelectScenario.ForeColor = System.Drawing.Color.White
        Me.btnSelectScenario.Location = New System.Drawing.Point(257, 18)
        Me.btnSelectScenario.Name = "btnSelectScenario"
        Me.btnSelectScenario.Size = New System.Drawing.Size(104, 25)
        Me.btnSelectScenario.TabIndex = 6
        Me.btnSelectScenario.Text = "Select"
        '
        'pnlActions
        '
        Me.pnlActions.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.pnlActions.Location = New System.Drawing.Point(3, 53)
        Me.pnlActions.Name = "pnlActions"
        Me.pnlActions.Size = New System.Drawing.Size(358, 254)
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
        Me.cmbScenario.Location = New System.Drawing.Point(3, 11)
        Me.cmbScenario.Name = "cmbScenario"
        Me.cmbScenario.Size = New System.Drawing.Size(248, 36)
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
        Me.txtOverAllOutput.Location = New System.Drawing.Point(389, 180)
        Me.txtOverAllOutput.Multiline = True
        Me.txtOverAllOutput.Name = "txtOverAllOutput"
        Me.txtOverAllOutput.PlaceholderText = ""
        Me.txtOverAllOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtOverAllOutput.SelectedText = ""
        Me.txtOverAllOutput.Size = New System.Drawing.Size(348, 280)
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
        Me.txtNextBestAction.Location = New System.Drawing.Point(389, 33)
        Me.txtNextBestAction.Multiline = True
        Me.txtNextBestAction.Name = "txtNextBestAction"
        Me.txtNextBestAction.PlaceholderText = ""
        Me.txtNextBestAction.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtNextBestAction.SelectedText = ""
        Me.txtNextBestAction.Size = New System.Drawing.Size(348, 120)
        Me.txtNextBestAction.TabIndex = 6
        '
        'btnAnalyze
        '
        Me.btnAnalyze.BorderRadius = 10
        Me.btnAnalyze.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnAnalyze.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnAnalyze.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnAnalyze.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnAnalyze.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnAnalyze.ForeColor = System.Drawing.Color.White
        Me.btnAnalyze.Location = New System.Drawing.Point(252, 105)
        Me.btnAnalyze.Name = "btnAnalyze"
        Me.btnAnalyze.Size = New System.Drawing.Size(119, 25)
        Me.btnAnalyze.TabIndex = 5
        Me.btnAnalyze.Text = "Refresh CGX"
        '
        'txtSecuredFax
        '
        Me.txtSecuredFax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSecuredFax.DefaultText = ""
        Me.txtSecuredFax.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtSecuredFax.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtSecuredFax.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtSecuredFax.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtSecuredFax.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtSecuredFax.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSecuredFax.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtSecuredFax.Location = New System.Drawing.Point(207, 12)
        Me.txtSecuredFax.Name = "txtSecuredFax"
        Me.txtSecuredFax.PlaceholderText = "Secured Fax"
        Me.txtSecuredFax.SelectedText = ""
        Me.txtSecuredFax.Size = New System.Drawing.Size(164, 22)
        Me.txtSecuredFax.TabIndex = 3
        '
        'txtCallingFrom
        '
        Me.txtCallingFrom.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCallingFrom.DefaultText = ""
        Me.txtCallingFrom.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtCallingFrom.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtCallingFrom.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtCallingFrom.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtCallingFrom.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtCallingFrom.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCallingFrom.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtCallingFrom.Location = New System.Drawing.Point(207, 40)
        Me.txtCallingFrom.Name = "txtCallingFrom"
        Me.txtCallingFrom.PlaceholderText = "Calling From"
        Me.txtCallingFrom.SelectedText = ""
        Me.txtCallingFrom.Size = New System.Drawing.Size(164, 22)
        Me.txtCallingFrom.TabIndex = 2
        '
        'txtCallbackNum
        '
        Me.txtCallbackNum.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCallbackNum.DefaultText = ""
        Me.txtCallbackNum.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtCallbackNum.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtCallbackNum.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtCallbackNum.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtCallbackNum.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtCallbackNum.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCallbackNum.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtCallbackNum.Location = New System.Drawing.Point(19, 40)
        Me.txtCallbackNum.Name = "txtCallbackNum"
        Me.txtCallbackNum.PlaceholderText = "Callback #"
        Me.txtCallbackNum.SelectedText = ""
        Me.txtCallbackNum.Size = New System.Drawing.Size(164, 22)
        Me.txtCallbackNum.TabIndex = 1
        '
        'txtCallerName
        '
        Me.txtCallerName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCallerName.DefaultText = ""
        Me.txtCallerName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtCallerName.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtCallerName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtCallerName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtCallerName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtCallerName.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCallerName.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtCallerName.Location = New System.Drawing.Point(19, 12)
        Me.txtCallerName.Name = "txtCallerName"
        Me.txtCallerName.PlaceholderText = "Caller Name"
        Me.txtCallerName.SelectedText = ""
        Me.txtCallerName.Size = New System.Drawing.Size(164, 22)
        Me.txtCallerName.TabIndex = 0
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(746, 530)
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
    Friend WithEvents txtCallbackNum As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtCallerName As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtSecuredFax As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtCallingFrom As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents cmbScenario As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents btnAnalyze As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents pnlOverallActions As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents pnlActions As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents txtOverAllOutput As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtNextBestAction As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Guna2HtmlLabel2 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel1 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents txtAuthNumber As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtDOS As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents btnSelectScenario As Guna.UI2.WinForms.Guna2Button
End Class
