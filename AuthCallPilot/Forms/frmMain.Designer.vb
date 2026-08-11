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
        Me.rtbNextBestAction = New System.Windows.Forms.RichTextBox()
        Me.btnTest = New Guna.UI2.WinForms.Guna2Button()
        Me.txtPAL = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtMarketGuide = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtOutOfScope = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtAuthInfo = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtMemberInfo = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtDOS = New Guna.UI2.WinForms.Guna2TextBox()
        Me.pnlScenarioCard = New Guna.UI2.WinForms.Guna2Panel()
        Me.btnSelectScenario = New Guna.UI2.WinForms.Guna2Button()
        Me.pnlActions = New Guna.UI2.WinForms.Guna2Panel()
        Me.cmbScenario = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.txtOverAllOutput = New Guna.UI2.WinForms.Guna2TextBox()
        Me.btnRefreshCGX = New Guna.UI2.WinForms.Guna2Button()
        Me.txtSecuredFax = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtCallingFrom = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtCallbackNum = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtCallerName = New Guna.UI2.WinForms.Guna2TextBox()
        Me.pnlCallDetailsCard = New Guna.UI2.WinForms.Guna2Panel()
        Me.pnlMemberCard = New Guna.UI2.WinForms.Guna2Panel()
        Me.pnlAuthCard = New Guna.UI2.WinForms.Guna2Panel()
        Me.pnlLookupCard = New Guna.UI2.WinForms.Guna2Panel()
        Me.pnlDocumentationCard = New Guna.UI2.WinForms.Guna2Panel()
        Me.pnlNextBestActionCard = New Guna.UI2.WinForms.Guna2Panel()
        Me.lblCgxStatus = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.btnCopyDocumentation = New Guna.UI2.WinForms.Guna2Button()
        Me.lblCallDetailsHeader = New System.Windows.Forms.Label()
        Me.lblScenarioHeader = New System.Windows.Forms.Label()
        Me.lblNextBestAction = New System.Windows.Forms.Label()
        Me.lblMemberHeader = New System.Windows.Forms.Label()
        Me.lblLookupOOS = New System.Windows.Forms.Label()
        Me.lblLookupMarketGuide = New System.Windows.Forms.Label()
        Me.lblLookupPAL = New System.Windows.Forms.Label()
        Me.lblDocumentation = New System.Windows.Forms.Label()
        Me.lblAuthHeader = New System.Windows.Forms.Label()
        Me.Guna2Panel1.SuspendLayout()
        Me.pnlScenarioCard.SuspendLayout()
        Me.pnlCallDetailsCard.SuspendLayout()
        Me.pnlMemberCard.SuspendLayout()
        Me.pnlAuthCard.SuspendLayout()
        Me.pnlLookupCard.SuspendLayout()
        Me.pnlDocumentationCard.SuspendLayout()
        Me.pnlNextBestActionCard.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnLaunchBrowser
        '
        Me.btnLaunchBrowser.BackColor = System.Drawing.Color.Transparent
        Me.btnLaunchBrowser.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnLaunchBrowser.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnLaunchBrowser.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnLaunchBrowser.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnLaunchBrowser.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLaunchBrowser.ForeColor = System.Drawing.Color.White
        Me.btnLaunchBrowser.Location = New System.Drawing.Point(869, 33)
        Me.btnLaunchBrowser.Name = "btnLaunchBrowser"
        Me.btnLaunchBrowser.Size = New System.Drawing.Size(186, 25)
        Me.btnLaunchBrowser.TabIndex = 1
        Me.btnLaunchBrowser.Text = "Launch Browser"
        '
        'Guna2Panel1
        '
        Me.Guna2Panel1.Controls.Add(Me.pnlNextBestActionCard)
        Me.Guna2Panel1.Controls.Add(Me.pnlDocumentationCard)
        Me.Guna2Panel1.Controls.Add(Me.pnlLookupCard)
        Me.Guna2Panel1.Controls.Add(Me.pnlAuthCard)
        Me.Guna2Panel1.Controls.Add(Me.pnlMemberCard)
        Me.Guna2Panel1.Controls.Add(Me.pnlCallDetailsCard)
        Me.Guna2Panel1.Controls.Add(Me.pnlScenarioCard)
        Me.Guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Guna2Panel1.Location = New System.Drawing.Point(3, 64)
        Me.Guna2Panel1.Name = "Guna2Panel1"
        Me.Guna2Panel1.Size = New System.Drawing.Size(1055, 631)
        Me.Guna2Panel1.TabIndex = 2
        '
        'rtbNextBestAction
        '
        Me.rtbNextBestAction.BackColor = System.Drawing.Color.White
        Me.rtbNextBestAction.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbNextBestAction.DetectUrls = False
        Me.rtbNextBestAction.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbNextBestAction.Location = New System.Drawing.Point(3, 24)
        Me.rtbNextBestAction.Name = "rtbNextBestAction"
        Me.rtbNextBestAction.ReadOnly = True
        Me.rtbNextBestAction.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
        Me.rtbNextBestAction.Size = New System.Drawing.Size(360, 214)
        Me.rtbNextBestAction.TabIndex = 24
        Me.rtbNextBestAction.TabStop = False
        Me.rtbNextBestAction.Text = ""
        '
        'btnTest
        '
        Me.btnTest.BorderRadius = 10
        Me.btnTest.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnTest.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnTest.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnTest.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnTest.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnTest.ForeColor = System.Drawing.Color.White
        Me.btnTest.Location = New System.Drawing.Point(117, 114)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(119, 25)
        Me.btnTest.TabIndex = 23
        Me.btnTest.Text = "Test"
        '
        'txtPAL
        '
        Me.txtPAL.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPAL.DefaultText = ""
        Me.txtPAL.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtPAL.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtPAL.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtPAL.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtPAL.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtPAL.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtPAL.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtPAL.Location = New System.Drawing.Point(5, 302)
        Me.txtPAL.Multiline = True
        Me.txtPAL.Name = "txtPAL"
        Me.txtPAL.PlaceholderText = ""
        Me.txtPAL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtPAL.SelectedText = ""
        Me.txtPAL.Size = New System.Drawing.Size(289, 112)
        Me.txtPAL.TabIndex = 21
        '
        'txtMarketGuide
        '
        Me.txtMarketGuide.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMarketGuide.DefaultText = ""
        Me.txtMarketGuide.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtMarketGuide.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtMarketGuide.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtMarketGuide.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtMarketGuide.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtMarketGuide.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtMarketGuide.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtMarketGuide.Location = New System.Drawing.Point(5, 163)
        Me.txtMarketGuide.Multiline = True
        Me.txtMarketGuide.Name = "txtMarketGuide"
        Me.txtMarketGuide.PlaceholderText = ""
        Me.txtMarketGuide.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMarketGuide.SelectedText = ""
        Me.txtMarketGuide.Size = New System.Drawing.Size(289, 112)
        Me.txtMarketGuide.TabIndex = 19
        '
        'txtOutOfScope
        '
        Me.txtOutOfScope.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOutOfScope.DefaultText = ""
        Me.txtOutOfScope.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtOutOfScope.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtOutOfScope.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtOutOfScope.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtOutOfScope.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtOutOfScope.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtOutOfScope.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtOutOfScope.Location = New System.Drawing.Point(5, 24)
        Me.txtOutOfScope.Multiline = True
        Me.txtOutOfScope.Name = "txtOutOfScope"
        Me.txtOutOfScope.PlaceholderText = ""
        Me.txtOutOfScope.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtOutOfScope.SelectedText = ""
        Me.txtOutOfScope.Size = New System.Drawing.Size(289, 112)
        Me.txtOutOfScope.TabIndex = 17
        '
        'txtAuthInfo
        '
        Me.txtAuthInfo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAuthInfo.DefaultText = ""
        Me.txtAuthInfo.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtAuthInfo.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtAuthInfo.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtAuthInfo.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtAuthInfo.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtAuthInfo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtAuthInfo.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtAuthInfo.Location = New System.Drawing.Point(3, 30)
        Me.txtAuthInfo.Multiline = True
        Me.txtAuthInfo.Name = "txtAuthInfo"
        Me.txtAuthInfo.PlaceholderText = ""
        Me.txtAuthInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtAuthInfo.SelectedText = ""
        Me.txtAuthInfo.Size = New System.Drawing.Size(311, 160)
        Me.txtAuthInfo.TabIndex = 15
        '
        'txtMemberInfo
        '
        Me.txtMemberInfo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMemberInfo.DefaultText = ""
        Me.txtMemberInfo.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtMemberInfo.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtMemberInfo.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtMemberInfo.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtMemberInfo.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtMemberInfo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtMemberInfo.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtMemberInfo.Location = New System.Drawing.Point(3, 29)
        Me.txtMemberInfo.Multiline = True
        Me.txtMemberInfo.Name = "txtMemberInfo"
        Me.txtMemberInfo.PlaceholderText = ""
        Me.txtMemberInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMemberInfo.SelectedText = ""
        Me.txtMemberInfo.Size = New System.Drawing.Size(311, 160)
        Me.txtMemberInfo.TabIndex = 13
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
        Me.txtDOS.Location = New System.Drawing.Point(9, 81)
        Me.txtDOS.Name = "txtDOS"
        Me.txtDOS.PlaceholderText = "Date of Service(MMddYY)"
        Me.txtDOS.SelectedText = ""
        Me.txtDOS.Size = New System.Drawing.Size(164, 22)
        Me.txtDOS.TabIndex = 12
        '
        'pnlScenarioCard
        '
        Me.pnlScenarioCard.Controls.Add(Me.lblScenarioHeader)
        Me.pnlScenarioCard.Controls.Add(Me.btnSelectScenario)
        Me.pnlScenarioCard.Controls.Add(Me.pnlActions)
        Me.pnlScenarioCard.Controls.Add(Me.cmbScenario)
        Me.pnlScenarioCard.Location = New System.Drawing.Point(15, 154)
        Me.pnlScenarioCard.Name = "pnlScenarioCard"
        Me.pnlScenarioCard.Size = New System.Drawing.Size(368, 230)
        Me.pnlScenarioCard.TabIndex = 8
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
        Me.btnSelectScenario.Location = New System.Drawing.Point(261, 45)
        Me.btnSelectScenario.Name = "btnSelectScenario"
        Me.btnSelectScenario.Size = New System.Drawing.Size(104, 25)
        Me.btnSelectScenario.TabIndex = 6
        Me.btnSelectScenario.Text = "Select"
        '
        'pnlActions
        '
        Me.pnlActions.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.pnlActions.Location = New System.Drawing.Point(7, 80)
        Me.pnlActions.Name = "pnlActions"
        Me.pnlActions.Size = New System.Drawing.Size(358, 137)
        Me.pnlActions.TabIndex = 5
        '
        'cmbScenario
        '
        Me.cmbScenario.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbScenario.BackColor = System.Drawing.Color.Transparent
        Me.cmbScenario.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cmbScenario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbScenario.FocusedColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cmbScenario.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cmbScenario.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.cmbScenario.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(112, Byte), Integer))
        Me.cmbScenario.ItemHeight = 30
        Me.cmbScenario.Items.AddRange(New Object() {"New Authorization", "Updating Authorization", "Checking Status Of The Authorization"})
        Me.cmbScenario.Location = New System.Drawing.Point(7, 38)
        Me.cmbScenario.Name = "cmbScenario"
        Me.cmbScenario.Size = New System.Drawing.Size(252, 36)
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
        Me.txtOverAllOutput.Location = New System.Drawing.Point(3, 34)
        Me.txtOverAllOutput.Multiline = True
        Me.txtOverAllOutput.Name = "txtOverAllOutput"
        Me.txtOverAllOutput.PlaceholderText = ""
        Me.txtOverAllOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtOverAllOutput.SelectedText = ""
        Me.txtOverAllOutput.Size = New System.Drawing.Size(348, 383)
        Me.txtOverAllOutput.TabIndex = 7
        '
        'btnRefreshCGX
        '
        Me.btnRefreshCGX.BorderRadius = 10
        Me.btnRefreshCGX.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnRefreshCGX.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnRefreshCGX.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnRefreshCGX.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnRefreshCGX.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnRefreshCGX.ForeColor = System.Drawing.Color.White
        Me.btnRefreshCGX.Location = New System.Drawing.Point(242, 114)
        Me.btnRefreshCGX.Name = "btnRefreshCGX"
        Me.btnRefreshCGX.Size = New System.Drawing.Size(119, 25)
        Me.btnRefreshCGX.TabIndex = 5
        Me.btnRefreshCGX.Text = "Refresh CGX"
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
        Me.txtSecuredFax.Location = New System.Drawing.Point(197, 25)
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
        Me.txtCallingFrom.Location = New System.Drawing.Point(197, 53)
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
        Me.txtCallbackNum.Location = New System.Drawing.Point(9, 53)
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
        Me.txtCallerName.Location = New System.Drawing.Point(9, 25)
        Me.txtCallerName.Name = "txtCallerName"
        Me.txtCallerName.PlaceholderText = "Caller Name"
        Me.txtCallerName.SelectedText = ""
        Me.txtCallerName.Size = New System.Drawing.Size(164, 22)
        Me.txtCallerName.TabIndex = 0
        '
        'pnlCallDetailsCard
        '
        Me.pnlCallDetailsCard.Controls.Add(Me.lblCallDetailsHeader)
        Me.pnlCallDetailsCard.Controls.Add(Me.txtCallerName)
        Me.pnlCallDetailsCard.Controls.Add(Me.txtCallbackNum)
        Me.pnlCallDetailsCard.Controls.Add(Me.btnTest)
        Me.pnlCallDetailsCard.Controls.Add(Me.txtCallingFrom)
        Me.pnlCallDetailsCard.Controls.Add(Me.txtSecuredFax)
        Me.pnlCallDetailsCard.Controls.Add(Me.btnRefreshCGX)
        Me.pnlCallDetailsCard.Controls.Add(Me.txtDOS)
        Me.pnlCallDetailsCard.Location = New System.Drawing.Point(15, 3)
        Me.pnlCallDetailsCard.Name = "pnlCallDetailsCard"
        Me.pnlCallDetailsCard.Size = New System.Drawing.Size(368, 145)
        Me.pnlCallDetailsCard.TabIndex = 25
        '
        'pnlMemberCard
        '
        Me.pnlMemberCard.Controls.Add(Me.lblMemberHeader)
        Me.pnlMemberCard.Controls.Add(Me.txtMemberInfo)
        Me.pnlMemberCard.Location = New System.Drawing.Point(393, 4)
        Me.pnlMemberCard.Name = "pnlMemberCard"
        Me.pnlMemberCard.Size = New System.Drawing.Size(320, 196)
        Me.pnlMemberCard.TabIndex = 26
        '
        'pnlAuthCard
        '
        Me.pnlAuthCard.Controls.Add(Me.lblAuthHeader)
        Me.pnlAuthCard.Controls.Add(Me.txtAuthInfo)
        Me.pnlAuthCard.Location = New System.Drawing.Point(719, 3)
        Me.pnlAuthCard.Name = "pnlAuthCard"
        Me.pnlAuthCard.Size = New System.Drawing.Size(321, 197)
        Me.pnlAuthCard.TabIndex = 27
        '
        'pnlLookupCard
        '
        Me.pnlLookupCard.Controls.Add(Me.lblLookupPAL)
        Me.pnlLookupCard.Controls.Add(Me.lblLookupMarketGuide)
        Me.pnlLookupCard.Controls.Add(Me.lblLookupOOS)
        Me.pnlLookupCard.Controls.Add(Me.txtOutOfScope)
        Me.pnlLookupCard.Controls.Add(Me.txtMarketGuide)
        Me.pnlLookupCard.Controls.Add(Me.txtPAL)
        Me.pnlLookupCard.Location = New System.Drawing.Point(392, 204)
        Me.pnlLookupCard.Name = "pnlLookupCard"
        Me.pnlLookupCard.Size = New System.Drawing.Size(295, 419)
        Me.pnlLookupCard.TabIndex = 28
        '
        'pnlDocumentationCard
        '
        Me.pnlDocumentationCard.Controls.Add(Me.lblDocumentation)
        Me.pnlDocumentationCard.Controls.Add(Me.btnCopyDocumentation)
        Me.pnlDocumentationCard.Controls.Add(Me.txtOverAllOutput)
        Me.pnlDocumentationCard.Location = New System.Drawing.Point(693, 206)
        Me.pnlDocumentationCard.Name = "pnlDocumentationCard"
        Me.pnlDocumentationCard.Size = New System.Drawing.Size(359, 422)
        Me.pnlDocumentationCard.TabIndex = 29
        '
        'pnlNextBestActionCard
        '
        Me.pnlNextBestActionCard.Controls.Add(Me.lblNextBestAction)
        Me.pnlNextBestActionCard.Controls.Add(Me.rtbNextBestAction)
        Me.pnlNextBestActionCard.Location = New System.Drawing.Point(15, 390)
        Me.pnlNextBestActionCard.Name = "pnlNextBestActionCard"
        Me.pnlNextBestActionCard.Size = New System.Drawing.Size(368, 238)
        Me.pnlNextBestActionCard.TabIndex = 30
        '
        'lblCgxStatus
        '
        Me.lblCgxStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblCgxStatus.Location = New System.Drawing.Point(636, 39)
        Me.lblCgxStatus.Name = "lblCgxStatus"
        Me.lblCgxStatus.Size = New System.Drawing.Size(74, 15)
        Me.lblCgxStatus.TabIndex = 3
        Me.lblCgxStatus.Text = "● CGX Waiting"
        '
        'btnCopyDocumentation
        '
        Me.btnCopyDocumentation.BorderRadius = 10
        Me.btnCopyDocumentation.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnCopyDocumentation.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnCopyDocumentation.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnCopyDocumentation.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnCopyDocumentation.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnCopyDocumentation.ForeColor = System.Drawing.Color.White
        Me.btnCopyDocumentation.Location = New System.Drawing.Point(229, 5)
        Me.btnCopyDocumentation.Name = "btnCopyDocumentation"
        Me.btnCopyDocumentation.Size = New System.Drawing.Size(119, 25)
        Me.btnCopyDocumentation.TabIndex = 24
        Me.btnCopyDocumentation.Text = "Copy "
        '
        'lblCallDetailsHeader
        '
        Me.lblCallDetailsHeader.AutoSize = True
        Me.lblCallDetailsHeader.Location = New System.Drawing.Point(10, 8)
        Me.lblCallDetailsHeader.Name = "lblCallDetailsHeader"
        Me.lblCallDetailsHeader.Size = New System.Drawing.Size(81, 13)
        Me.lblCallDetailsHeader.TabIndex = 24
        Me.lblCallDetailsHeader.Text = "CALL DETAILS"
        '
        'lblScenarioHeader
        '
        Me.lblScenarioHeader.AutoSize = True
        Me.lblScenarioHeader.Location = New System.Drawing.Point(10, 12)
        Me.lblScenarioHeader.Name = "lblScenarioHeader"
        Me.lblScenarioHeader.Size = New System.Drawing.Size(81, 13)
        Me.lblScenarioHeader.TabIndex = 25
        Me.lblScenarioHeader.Text = "CALL DETAILS"
        '
        'lblNextBestAction
        '
        Me.lblNextBestAction.AutoSize = True
        Me.lblNextBestAction.Location = New System.Drawing.Point(10, 6)
        Me.lblNextBestAction.Name = "lblNextBestAction"
        Me.lblNextBestAction.Size = New System.Drawing.Size(81, 13)
        Me.lblNextBestAction.TabIndex = 26
        Me.lblNextBestAction.Text = "CALL DETAILS"
        '
        'lblMemberHeader
        '
        Me.lblMemberHeader.AutoSize = True
        Me.lblMemberHeader.Location = New System.Drawing.Point(13, 8)
        Me.lblMemberHeader.Name = "lblMemberHeader"
        Me.lblMemberHeader.Size = New System.Drawing.Size(81, 13)
        Me.lblMemberHeader.TabIndex = 27
        Me.lblMemberHeader.Text = "CALL DETAILS"
        '
        'lblLookupOOS
        '
        Me.lblLookupOOS.AutoSize = True
        Me.lblLookupOOS.Location = New System.Drawing.Point(14, 8)
        Me.lblLookupOOS.Name = "lblLookupOOS"
        Me.lblLookupOOS.Size = New System.Drawing.Size(81, 13)
        Me.lblLookupOOS.TabIndex = 28
        Me.lblLookupOOS.Text = "CALL DETAILS"
        '
        'lblLookupMarketGuide
        '
        Me.lblLookupMarketGuide.AutoSize = True
        Me.lblLookupMarketGuide.Location = New System.Drawing.Point(14, 146)
        Me.lblLookupMarketGuide.Name = "lblLookupMarketGuide"
        Me.lblLookupMarketGuide.Size = New System.Drawing.Size(81, 13)
        Me.lblLookupMarketGuide.TabIndex = 29
        Me.lblLookupMarketGuide.Text = "CALL DETAILS"
        '
        'lblLookupPAL
        '
        Me.lblLookupPAL.AutoSize = True
        Me.lblLookupPAL.Location = New System.Drawing.Point(14, 285)
        Me.lblLookupPAL.Name = "lblLookupPAL"
        Me.lblLookupPAL.Size = New System.Drawing.Size(81, 13)
        Me.lblLookupPAL.TabIndex = 30
        Me.lblLookupPAL.Text = "CALL DETAILS"
        '
        'lblDocumentation
        '
        Me.lblDocumentation.AutoSize = True
        Me.lblDocumentation.Location = New System.Drawing.Point(13, 12)
        Me.lblDocumentation.Name = "lblDocumentation"
        Me.lblDocumentation.Size = New System.Drawing.Size(81, 13)
        Me.lblDocumentation.TabIndex = 31
        Me.lblDocumentation.Text = "CALL DETAILS"
        '
        'lblAuthHeader
        '
        Me.lblAuthHeader.AutoSize = True
        Me.lblAuthHeader.Location = New System.Drawing.Point(16, 9)
        Me.lblAuthHeader.Name = "lblAuthHeader"
        Me.lblAuthHeader.Size = New System.Drawing.Size(81, 13)
        Me.lblAuthHeader.TabIndex = 32
        Me.lblAuthHeader.Text = "CALL DETAILS"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1061, 698)
        Me.Controls.Add(Me.lblCgxStatus)
        Me.Controls.Add(Me.Guna2Panel1)
        Me.Controls.Add(Me.btnLaunchBrowser)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMain"
        Me.Text = "AuthCallPilot"
        Me.Guna2Panel1.ResumeLayout(False)
        Me.pnlScenarioCard.ResumeLayout(False)
        Me.pnlScenarioCard.PerformLayout()
        Me.pnlCallDetailsCard.ResumeLayout(False)
        Me.pnlCallDetailsCard.PerformLayout()
        Me.pnlMemberCard.ResumeLayout(False)
        Me.pnlMemberCard.PerformLayout()
        Me.pnlAuthCard.ResumeLayout(False)
        Me.pnlAuthCard.PerformLayout()
        Me.pnlLookupCard.ResumeLayout(False)
        Me.pnlLookupCard.PerformLayout()
        Me.pnlDocumentationCard.ResumeLayout(False)
        Me.pnlDocumentationCard.PerformLayout()
        Me.pnlNextBestActionCard.ResumeLayout(False)
        Me.pnlNextBestActionCard.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnLaunchBrowser As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2Panel1 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents txtCallbackNum As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtCallerName As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtSecuredFax As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtCallingFrom As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents cmbScenario As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents btnRefreshCGX As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents pnlScenarioCard As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents pnlActions As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents txtOverAllOutput As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtDOS As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents btnSelectScenario As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents txtMemberInfo As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtPAL As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtMarketGuide As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtOutOfScope As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtAuthInfo As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents btnTest As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents rtbNextBestAction As RichTextBox
    Friend WithEvents pnlAuthCard As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents pnlMemberCard As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents pnlCallDetailsCard As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents pnlNextBestActionCard As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents pnlDocumentationCard As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents pnlLookupCard As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents lblCgxStatus As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents btnCopyDocumentation As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents lblCallDetailsHeader As Label
    Friend WithEvents lblNextBestAction As Label
    Friend WithEvents lblLookupPAL As Label
    Friend WithEvents lblLookupMarketGuide As Label
    Friend WithEvents lblLookupOOS As Label
    Friend WithEvents lblMemberHeader As Label
    Friend WithEvents lblScenarioHeader As Label
    Friend WithEvents lblDocumentation As Label
    Friend WithEvents lblAuthHeader As Label
End Class
