Imports System.Linq
Imports MaterialSkin
Imports MaterialSkin.Controls
Imports OpenQA.Selenium
Imports System.Threading.Tasks
Imports System.Diagnostics
Public Class frmMain
    Private _currentContext As CallContext
    Private _currentLookup As LookupResult
    Private _currentQuestionId As String
    Private _currentQuestionText As String
    Private ReadOnly _questionHistory As New Stack(Of QuestionState)

    Private WithEvents _browserMonitorTimer As New Timer()
    Private _browserMonitorBusy As Boolean = False
    Private _ignoredMemberId As String
    Private _ignoredAuthorizationId As String

    Private _lastProcessedUrl As String = String.Empty
    Private _lastProcessedTitle As String = String.Empty

    Private _nextBestActionLinks As New Dictionary(Of String, String)

    '========================================
    ' CALLPILOT COLOR PALETTE
    '========================================
    Private ReadOnly ColorPrimaryGreen As Color = Color.FromArgb(111, 180, 44)
    Private ReadOnly ColorDarkGreen As Color = Color.FromArgb(62, 119, 35)
    Private ReadOnly ColorHoverGreen As Color = Color.FromArgb(91, 157, 36)
    Private ReadOnly ColorLightGreen As Color = Color.FromArgb(238, 247, 230)
    Private ReadOnly ColorAppBackground As Color = Color.FromArgb(245, 247, 245)
    Private ReadOnly ColorCardBackground As Color = Color.White
    Private ReadOnly ColorTextPrimary As Color = Color.FromArgb(45, 55, 50)
    Private ReadOnly ColorTextSecondary As Color = Color.FromArgb(115, 125, 120)
    Private ReadOnly ColorBorder As Color = Color.FromArgb(220, 226, 220)
    'General tool-generated outcome
    Private ReadOnly ColorOutcomeBackground As Color = Color.FromArgb(248, 246, 226)   'Soft pastel yellow
    Private ReadOnly ColorOutcomeBorder As Color = Color.FromArgb(226, 215, 150)
    'Positive / Out of Scope = NO
    Private ReadOnly ColorSuccessBackground As Color = Color.FromArgb(235, 247, 232)
    Private ReadOnly ColorSuccessBorder As Color = Color.FromArgb(125, 186, 105)
    Private ReadOnly ColorSuccessText As Color = Color.FromArgb(48, 112, 45)
    'Negative / Out of Scope = YES
    Private ReadOnly ColorDangerBackground As Color = Color.FromArgb(252, 235, 235)
    Private ReadOnly ColorDangerBorder As Color = Color.FromArgb(220, 125, 125)
    Private ReadOnly ColorDangerText As Color = Color.FromArgb(170, 55, 55)
    Private ReadOnly ColorAgentActionBackground As Color = Color.FromArgb(245, 250, 240)
    Private ReadOnly ColorAgentActionBorder As Color = Color.FromArgb(190, 215, 170)

    Private ReadOnly ColorAutoPopulatedBackground As Color = Color.FromArgb(255, 245, 230) 'soft orange
    Private ReadOnly ColorAutoPopulatedBorder As Color = Color.FromArgb(235, 170, 90)
    Private ReadOnly ColorAutoPopulatedText As Color = Color.FromArgb(120, 75, 20)
    Private ReadOnly _oosLinks As New Dictionary(Of String, String)

    '#URLS
    Private Const DelegatedGrouperSearchUrl As String =
    "https://app.powerbi.com/groups/me/reports/1d9cdc40-1454-455e-98d4-399ede4a15e7/ReportSectioneaef45f0db640833762b?experience=power-bi"
    Private Const PcodsotUrl As String =
    "https://apps.powerapps.com/play/e/default-56c62bbe-8598-4b85-9e51-1ca753fa50f2/a/8c7fa451-51fc-4840-930c-9f832a1cdba0?tenantId=56c62bbe-8598-4b85-9e51-1ca753fa50f2&hint=a7eca138-56c6-4316-919d-fb8cad463df2&sourcetime=1706564912682"
    Private Sub RenderOutOfScope()
        rtbOutOfScope.Clear()
        If _currentLookup Is Nothing Then
            Return
        End If

        '========================================
        ' NORMAL OOS OUTPUT
        '========================================
        Dim oosText As String =
        OutputFormatter.BuildOutOfScope(_currentLookup)
        rtbOutOfScope.AppendText(oosText)
        rtbOutOfScope.AppendText(Environment.NewLine)
        rtbOutOfScope.AppendText("Validate Grouper on the Following Links:" & Environment.NewLine)

        '========================================
        ' DELEGATED GROUPER SEARCH
        '========================================
        AppendHyperlink(rtbOutOfScope, "Open Delegated Grouper Search", DelegatedGrouperSearchUrl)
        rtbOutOfScope.AppendText(Environment.NewLine)

        '========================================
        ' PCODSOT
        '========================================
        AppendHyperlink(rtbOutOfScope, "Open PCODSOT", PcodsotUrl)
    End Sub
    Private Sub AppendHyperlink(box As RichTextBox, displayText As String, url As String)
        Dim start As Integer = box.TextLength
        box.AppendText(displayText)
        box.Select(start, displayText.Length)
        box.SelectionColor = Color.FromArgb(0, 102, 204)
        box.SelectionFont = New Font(box.Font, FontStyle.Underline)
        box.Select(box.TextLength, 0)
        _oosLinks(displayText) = url
    End Sub
    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        _browserMonitorTimer.Stop()
        BrowserManager.Close()
    End Sub
    Private Sub SetAuthorizationWaitingState()
        pnlAuthCard.FillColor = Color.White
        pnlAuthCard.BackColor = Color.White
        pnlAuthCard.BorderColor = ColorBorder

        txtAuthInfo.FillColor = Color.White
        txtAuthInfo.ForeColor = ColorTextSecondary

    End Sub
    Private Sub SetAuthorizationPopulatedState()
        pnlAuthCard.FillColor = ColorAutoPopulatedBackground
        pnlAuthCard.BackColor = ColorAutoPopulatedBackground
        pnlAuthCard.BorderColor = ColorAutoPopulatedBorder

        txtAuthInfo.FillColor = ColorAutoPopulatedBackground
        txtAuthInfo.ForeColor = ColorAutoPopulatedText

    End Sub
    Private Async Sub BrowserMonitorTimer_Tick(sender As Object, e As EventArgs) Handles _browserMonitorTimer.Tick
        If _browserMonitorBusy Then
            Exit Sub
        End If

        If Not BrowserManager.IsBrowserAvailable() Then
            SetCgxStatus("OFFLINE")
            Exit Sub
        End If

        Dim currentUrl As String = String.Empty
        Dim currentTitle As String = String.Empty

        If Not BrowserManager.GetCurrentPageLocation(currentUrl, currentTitle) Then
            Exit Sub
        End If

        'Do nothing while CGX remains on the same page.
        If String.Equals(currentUrl, _lastProcessedUrl, StringComparison.OrdinalIgnoreCase) And String.Equals(currentTitle, _lastProcessedTitle, StringComparison.OrdinalIgnoreCase) Then
            Exit Sub
        End If

        SetCgxStatus("READING")
        _browserMonitorBusy = True

        Try
            Dim captured As BrowserCaptureResult =
                Await Task.Run(
                    Function()
                        Return BrowserManager.CaptureCurrentCgxPage()
                    End Function)

            If captured Is Nothing Then
                Exit Sub
            End If

            Select Case captured.PageType

                Case CgxPageType.MemberInformation
                    If captured.Context Is Nothing Then
                        SetCgxStatus("WAITING")
                        Exit Sub
                    End If

                    ProcessDetectedMember(captured.Context)
                    ShowMemberInformationPanel()
                    SetCgxStatus("MEMBER")
                    _lastProcessedUrl = currentUrl
                    _lastProcessedTitle = currentTitle

                Case CgxPageType.ViewAuthorization
                    If captured.Context Is Nothing Then
                        Exit Sub
                    End If

                    ProcessDetectedAuthorization(captured.Context)
                    ShowAuthorizationInformationPanel()
                    SetCgxStatus("AUTH")
                    _lastProcessedUrl = currentUrl
                    _lastProcessedTitle = currentTitle

                Case CgxPageType.Other
                    SetCgxStatus("WAITING")
                    _lastProcessedUrl = currentUrl
                    _lastProcessedTitle = currentTitle

            End Select
        Catch ex As OpenQA.Selenium.WebDriverException
            SetCgxStatus("ERROR")
            Debug.WriteLine("CGX listener error: " & ex.Message)

        Catch ex As Exception
            SetCgxStatus("ERROR")
            Debug.WriteLine("CGX listener error: " & ex.ToString())
        Finally
            _browserMonitorBusy = False
        End Try
    End Sub
    Private Sub ApplyModernUI()
        '========================================
        ' MAIN FORM
        '========================================
        Me.BackColor = Color.FromArgb(245, 247, 250)
        lblCgxStatus.Font = New Font("Segoe UI Semibold", 9.0!)
        lblCgxStatus.BackColor = Color.Transparent
        Guna2Panel1.FillColor = Color.FromArgb(245, 247, 250)
        Guna2Panel1.BackColor = Color.FromArgb(245, 247, 250)

        '========================================
        ' PRIMARY BUTTONS
        '========================================
        btnLaunchBrowser.FillColor = ColorPrimaryGreen
        btnLaunchBrowser.ForeColor = Color.White
        btnLaunchBrowser.BackColor = Color.FromArgb(55, 142, 60)
        btnLaunchBrowser.BorderThickness = 0
        btnLaunchBrowser.BorderRadius = 8
        btnLaunchBrowser.HoverState.FillColor = ColorHoverGreen
        btnLaunchBrowser.Height = 28

        StylePrimaryButton(btnLaunchBrowser)
        '========================================
        ' Secondary BUTTONS
        '========================================
        StyleSecondaryButton(btnRefreshCGX)
        StyleSecondaryButton(btnCopyDocumentation)
        StyleSecondaryButton(btnTest)

        '========================================
        ' INPUT CONTROLS
        '========================================
        StyleInput(txtCallerName)
        StyleInput(txtCallbackNum)
        StyleInput(txtSecuredFax)
        StyleInput(txtCallingFrom)
        StyleInput(txtDOS)

        '========================================
        ' SCENARIO COMBOBOX
        '========================================
        cmbScenario.BorderRadius = 8
        cmbScenario.Font = New Font("Segoe UI", 9.0!)
        cmbScenario.BackColor = Color.White
        cmbScenario.FillColor = Color.White
        cmbScenario.BorderColor = Color.FromArgb(210, 216, 225)
        cmbScenario.ForeColor = Color.FromArgb(45, 55, 72)

        '========================================
        ' NEXT BEST ACTION
        '========================================
        rtbNextBestAction.BorderStyle = BorderStyle.None
        rtbNextBestAction.BackColor = ColorOutcomeBackground
        rtbNextBestAction.Font = New Font("Segoe UI", 9.0!)
        rtbNextBestAction.ForeColor = Color.FromArgb(45, 55, 72)

        '========================================
        ' INFORMATION / OUTPUT BOXES
        '========================================
        StyleOutputBox(txtMemberInfo)
        StyleOutputBox(txtAuthInfo)

        '========================================
        ' DOCUMENTATION
        '========================================
        StyleDocumentationBox(txtOverAllOutput)

        '========================================
        ' CARDS
        '========================================
        StyleCard(pnlCallDetailsCard)
        pnlCallDetailsCard.FillColor = ColorAgentActionBackground
        pnlCallDetailsCard.BackColor = ColorAgentActionBackground
        pnlCallDetailsCard.BorderColor = ColorPrimaryGreen
        pnlCallDetailsCard.BorderThickness = 1
        StyleCard(pnlMemberCard)
        StyleCard(pnlAuthCard)
        StyleCard(pnlLookupCard)
        StyleCard(pnlScenarioCard)
        pnlScenarioCard.FillColor = ColorAgentActionBackground
        pnlScenarioCard.BackColor = ColorAgentActionBackground
        pnlScenarioCard.BorderColor = ColorPrimaryGreen
        pnlScenarioCard.BorderThickness = 1
        StyleCard(pnlNextBestActionCard)
        StyleCard(pnlDocumentationCard)
        pnlNextBestActionCard.FillColor = ColorOutcomeBackground
        pnlNextBestActionCard.BackColor = ColorOutcomeBackground
        pnlNextBestActionCard.BorderColor = ColorOutcomeBorder

        '========================================
        ' CARD HEADERS
        '========================================
        StyleCardHeader(lblCallDetailsHeader)
        StyleCardHeader(lblScenarioHeader)
        StyleCardHeader(lblMemberHeader)
        StyleCardHeader(lblAuthHeader)
        StyleCardHeader(lblDocumentation)
        StyleCardHeader(lblNextBestAction)

        StyleLookupHeader(lblLookupOOS)
        StyleLookupHeader(lblLookupMarketGuide)
        StyleLookupHeader(lblLookupPAL)

        lblCallDetailsHeader.Text = "CALL DETAILS"
        lblScenarioHeader.Text = "CALL SCENARIO"
        lblMemberHeader.Text = "MEMBER INFORMATION"
        lblAuthHeader.Text = "AUTHORIZATION"
        lblDocumentation.Text = "DOCUMENTATION"
        lblNextBestAction.Text = "NEXT BEST ACTION"

        lblLookupOOS.Text = "OUT OF SCOPE"
        lblLookupMarketGuide.Text = "MARKET GUIDE"
        lblLookupPAL.Text = "PAL"

        '========================================
        ' DYNAMIC ACTION PANEL
        '========================================
        pnlActions.BackColor = Color.White
        pnlActions.FillColor = Color.White
        pnlActions.BorderThickness = 0
        pnlActions.AutoScroll = True

        '========================================
        ' OUTPUT RICH TEXT BOXES
        '========================================
        StyleLookupRichTextBox(rtbOutOfScope)
        StyleLookupRichTextBox(rtbMarketGuide)
        StyleLookupRichTextBox(rtbPAL)

        '========================================
        ' OUT OF SCOPE SECTION
        '========================================
        pnlOutOfScopeSection.BorderRadius = 8
        pnlOutOfScopeSection.BorderThickness = 1

        '========================================
        ' LOOKUP / OUTCOME SECTIONS
        '========================================
        StyleOutcomeSection(pnlOutOfScopeSection)
        StyleOutcomeSection(pnlMarketGuideSection)
        StyleOutcomeSection(pnlPALSection)

        SetOutOfScopeWaitingState()
        SetMarketGuideWaitingState()
        SetPALWaitingState()

        StyleCard(pnlVerificationCard)
        pnlVerificationCard.FillColor = ColorAgentActionBackground
        pnlVerificationCard.BackColor = ColorAgentActionBackground
        pnlVerificationCard.BorderColor = ColorPrimaryGreen
        pnlVerificationCard.BorderThickness = 1
        StyleCardHeader(lblVerificationHeader)

        '========================================
        ' VERIFICATION CHECKBOXES
        '========================================
        lblVerificationHeader.Text = "VERIFICATION"
        StyleVerificationCheckBox(chkGenesysVerified)
        StyleVerificationCheckBox(chkProviderAuthenticated)
        StyleVerificationCheckBox(chkMailingAddressVerified)
    End Sub
    Private Sub StyleVerificationCheckBox(chk As Guna.UI2.WinForms.Guna2CheckBox)
        chk.Font = New Font("Segoe UI", 9.0!, FontStyle.Regular)
        chk.ForeColor = ColorTextPrimary
        chk.BackColor = Color.Transparent

        chk.CheckedState.BorderColor = ColorPrimaryGreen
        chk.CheckedState.BorderRadius = 2
        chk.CheckedState.BorderThickness = 0
        chk.CheckedState.FillColor = ColorPrimaryGreen

        chk.UncheckedState.BorderColor = ColorPrimaryGreen
        chk.UncheckedState.BorderRadius = 2
        chk.UncheckedState.BorderThickness = 1
        chk.UncheckedState.FillColor = Color.White
        chk.Cursor = Cursors.Hand
    End Sub
    Private Sub SetMarketGuideWaitingState()
        pnlMarketGuideSection.FillColor = ColorOutcomeBackground
        pnlMarketGuideSection.BackColor = ColorOutcomeBackground
        pnlMarketGuideSection.BorderColor = ColorOutcomeBorder

        rtbMarketGuide.BackColor = ColorOutcomeBackground
        rtbMarketGuide.ForeColor = ColorTextPrimary

        lblLookupMarketGuide.BackColor = Color.Transparent
        lblLookupMarketGuide.ForeColor = ColorTextPrimary
    End Sub
    Private Sub SetPALWaitingState()
        pnlPALSection.FillColor = ColorOutcomeBackground
        pnlPALSection.BackColor = ColorOutcomeBackground
        pnlPALSection.BorderColor = ColorOutcomeBorder

        rtbPAL.BackColor = ColorOutcomeBackground
        rtbPAL.ForeColor = ColorTextPrimary

        lblLookupPAL.BackColor = Color.Transparent
        lblLookupPAL.ForeColor = ColorTextPrimary
    End Sub
    Private Sub StyleOutcomeSection(panel As Guna.UI2.WinForms.Guna2Panel)
        panel.BorderRadius = 8
        panel.BorderThickness = 1
        panel.FillColor = ColorOutcomeBackground
        panel.BackColor = ColorOutcomeBackground
        panel.BorderColor = ColorOutcomeBorder
    End Sub
    Private Sub SetOutOfScopeWaitingState()
        pnlOutOfScopeSection.FillColor = ColorOutcomeBackground
        pnlOutOfScopeSection.BackColor = ColorOutcomeBackground
        pnlOutOfScopeSection.BorderColor = ColorOutcomeBorder
        rtbOutOfScope.BackColor = ColorOutcomeBackground
        rtbOutOfScope.ForeColor = ColorTextPrimary
        lblLookupOOS.BackColor = Color.Transparent
        lblLookupOOS.ForeColor = ColorTextPrimary
    End Sub
    Private Sub ApplyOutOfScopeVisualState()
        'No lookup yet
        If _currentLookup Is Nothing Then
            SetOutOfScopeWaitingState()
            Return
        End If

        'Result could not be determined
        If Not _currentLookup.IsOutOfScope.HasValue Then
            SetOutOfScopeWaitingState()
            Return
        End If

        '========================================
        ' OUT OF SCOPE = YES
        ' RED
        '========================================
        If _currentLookup.IsOutOfScope.Value Then
            pnlOutOfScopeSection.FillColor = ColorDangerBackground
            pnlOutOfScopeSection.BackColor = ColorDangerBackground
            pnlOutOfScopeSection.BorderColor = ColorDangerBorder
            rtbOutOfScope.BackColor = ColorDangerBackground
            rtbOutOfScope.ForeColor = ColorDangerText
            lblLookupOOS.ForeColor = ColorDangerText
            Return
        End If

        '========================================
        ' OUT OF SCOPE = NO
        ' GREEN
        '========================================
        pnlOutOfScopeSection.FillColor = ColorSuccessBackground
        pnlOutOfScopeSection.BackColor = ColorSuccessBackground
        pnlOutOfScopeSection.BorderColor = ColorSuccessBorder
        rtbOutOfScope.BackColor = ColorSuccessBackground
        rtbOutOfScope.ForeColor = ColorSuccessText
        lblLookupOOS.ForeColor = ColorSuccessText
    End Sub
    Private Sub StyleLookupRichTextBox(box As RichTextBox)
        box.BorderStyle = BorderStyle.None
        box.BackColor = Color.White
        box.ForeColor = Color.FromArgb(55, 65, 81)

        box.Font = New Font("Segoe UI", 9.0!)
        box.ReadOnly = True
        box.DetectUrls = True
    End Sub
    Private Sub StylePrimaryButton(button As Guna.UI2.WinForms.Guna2Button)
        button.BorderRadius = 8
        button.FillColor = ColorPrimaryGreen
        button.ForeColor = Color.White
        button.Font = New Font("Segoe UI Semibold", 9.0!, FontStyle.Bold)
        button.Cursor = Cursors.Hand
    End Sub
    Private Sub StyleInput(txt As Guna.UI2.WinForms.Guna2TextBox)
        txt.BorderRadius = 8
        txt.BorderThickness = 1
        txt.BorderColor = ColorPrimaryGreen
        txt.FillColor = Color.White
        txt.Font = New Font("Segoe UI", 9.5!)
        txt.FocusedState.BorderColor = Color.FromArgb(76, 129, 255)
    End Sub
    Private Sub StyleWaitingOutput(txt As Guna.UI2.WinForms.Guna2TextBox)
        txt.ForeColor = Color.FromArgb(150, 158, 170)
    End Sub
    Private Sub StyleOutputBox(txt As Guna.UI2.WinForms.Guna2TextBox)
        txt.BorderRadius = 0
        txt.BorderThickness = 0
        txt.FillColor = Color.White
        txt.ForeColor = Color.FromArgb(55, 65, 81)
        txt.Font = New Font("Segoe UI", 9.0!)
        txt.ReadOnly = True
        txt.BorderColor = ColorPrimaryGreen
        txt.Cursor = Cursors.Default
    End Sub
    Private Sub StyleDocumentationBox(txt As Guna.UI2.WinForms.Guna2TextBox)
        txt.BorderRadius = 0
        txt.BorderThickness = 0
        txt.FillColor = Color.White
        txt.ForeColor = Color.FromArgb(45, 55, 72)
        txt.Font = New Font("Segoe UI", 9.0!)
    End Sub
    Private Sub StyleCard(card As Guna.UI2.WinForms.Guna2Panel)
        card.BackColor = Color.White
        card.FillColor = Color.White
        card.BorderRadius = 12
        card.BorderThickness = 1
        card.BorderColor = Color.FromArgb(230, 233, 239)
        card.ShadowDecoration.Enabled = True
        card.ShadowDecoration.Depth = 2
        card.ShadowDecoration.Shadow = New Padding(1)
    End Sub
    Private Sub StyleCardHeader(lbl As Label)
        lbl.Font = New Font("Segoe UI Semibold", 9.5!, FontStyle.Bold)
        lbl.ForeColor = Color.FromArgb(45, 55, 72)
        lbl.BackColor = Color.Transparent
    End Sub
    Private Sub StyleSecondaryButton(button As Guna.UI2.WinForms.Guna2Button)
        button.BorderRadius = 8
        button.BorderThickness = 1
        button.BorderColor = ColorPrimaryGreen
        button.FillColor = Color.White
        button.ForeColor = ColorDarkGreen
        button.Font = New Font("Segoe UI Semibold", 9.0!)
        button.Cursor = Cursors.Hand
    End Sub
    Private Sub StyleLookupHeader(lbl As Label)
        lbl.Font = New Font("Segoe UI Semibold", 8.5!, FontStyle.Bold)
        lbl.ForeColor = Color.FromArgb(90, 100, 115)
        lbl.BackColor = Color.Transparent
    End Sub
    Private Sub SetOutputWaiting(txt As Guna.UI2.WinForms.Guna2TextBox, message As String)
        txt.Text = message
        txt.ForeColor = Color.FromArgb(150, 158, 170)
    End Sub
    Private Sub SetOutputWaiting(txt As RichTextBox, message As String)
        txt.Text = message
        txt.ForeColor = Color.FromArgb(150, 158, 170)
    End Sub
    Private Sub SetOutputValue(txt As Guna.UI2.WinForms.Guna2TextBox, value As String)
        txt.Text = value
        txt.ForeColor = Color.FromArgb(55, 65, 81)
    End Sub
    Private Sub SetCgxStatus(status As String)
        Select Case status.ToUpperInvariant()
            Case "WAITING"
                lblCgxStatus.Text = "● CGX Waiting"
                lblCgxStatus.ForeColor = Color.FromArgb(220, 225, 220)

            Case "READING"
                lblCgxStatus.Text = "● Reading CGX..."
                lblCgxStatus.ForeColor = Color.FromArgb(255, 193, 7)

            Case "MEMBER"
                lblCgxStatus.Text = "● Member Detected"
                lblCgxStatus.ForeColor = Color.FromArgb(76, 175, 80)

            Case "AUTH"
                lblCgxStatus.Text = "● Authorization Detected"
                lblCgxStatus.ForeColor = Color.FromArgb(76, 175, 80)

            Case "ERROR"
                lblCgxStatus.Text = "● CGX Error"
                lblCgxStatus.ForeColor = Color.FromArgb(220, 70, 70)

            Case "OFFLINE"
                lblCgxStatus.Text = "● Browser Not Connected"
                lblCgxStatus.ForeColor = Color.FromArgb(160, 165, 170)
        End Select

    End Sub
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            DatabaseManager.VerifyDatabase()

            txtOverAllOutput.Text =
        "Database loaded successfully:" &
        Environment.NewLine &
        DatabaseManager.DatabasePath

        Catch ex As Exception

            MessageBox.Show(
        ex.Message,
        "Database Error",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error)

        End Try

        '_browserMonitorTimer.Interval = 1500
        _browserMonitorTimer.Interval = 750
        _browserMonitorTimer.Start()


        Dim skinManager As MaterialSkinManager = MaterialSkinManager.Instance
        skinManager.AddFormToManage(Me)

        skinManager.Theme = MaterialSkinManager.Themes.LIGHT
        skinManager.ColorScheme = New ColorScheme(
            Primary.Green700,
            Primary.Green800,
            Primary.Green500,
            Accent.LightGreen400,
            TextShade.WHITE
        )

        Me.Text = "CallPilot V1.0"
        ApplyModernUI()
        SetCgxStatus("OFFLINE")

        cmbScenario.Items.Clear()
        cmbScenario.Items.Add("NEW AUTHORIZATION")
        cmbScenario.Items.Add("UPDATING AUTHORIZATION")
        cmbScenario.Items.Add("CHECKING STATUS OF THE AUTHORIZATION")
        cmbScenario.SelectedIndex = -1

        txtOverAllOutput.Clear()
        rtbNextBestAction.Clear()
        'SetWaitingOutputStates()
        SetOutputWaiting(txtMemberInfo, "Waiting for member information...")
        SetOutputWaiting(txtAuthInfo, "Waiting for authorization information...")
        SetOutputWaiting(rtbOutOfScope, "Waiting for member lookup...")
        SetOutputWaiting(rtbMarketGuide, "Waiting for member lookup...")
        SetOutputWaiting(rtbPAL, "Waiting for authorization...")

        pnlActions.Controls.Clear()
        pnlActions.Visible = False

        txtMemberInfo.ReadOnly = True
        txtAuthInfo.ReadOnly = True
        rtbOutOfScope.ReadOnly = True
        rtbMarketGuide.ReadOnly = True
        rtbPAL.ReadOnly = True
        rtbNextBestAction.ReadOnly = True
        txtOverAllOutput.ReadOnly = False

        lblCgxStatus.BackColor = Color.Transparent
        SetAuthorizationWaitingState()
        ShowMemberInformationPanel()
    End Sub
    Private Sub SetWaitingOutputStates()
        txtMemberInfo.Text = "Waiting for member information..."
        txtAuthInfo.Text = "Waiting for authorization information..."
        rtbOutOfScope.Text = "Waiting for member lookup..."
        rtbMarketGuide.Text = "Waiting for member lookup..."
        rtbPAL.Text = "Waiting for authorization..."
    End Sub
    Private Sub frmMain_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        'working
        'RenderWorkflow()

        'tsting
        'RenderAllWorkflowSections()

    End Sub

    Private Sub btnLaunchBrowser_Click(sender As Object, e As EventArgs) Handles btnLaunchBrowser.Click
        Try
            SetCgxStatus("READING")
            BrowserManager.Launch()
            SetCgxStatus("WAITING")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Browser Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Async Sub btnAnalyze_Click(sender As Object, e As EventArgs) Handles btnRefreshCGX.Click
        If Not BrowserManager.IsBrowserAvailable() Then
            SetCgxStatus("OFFLINE")
            MessageBox.Show("Launch the CGX browser first.", "Browser Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not ValidateCgxSearchInput() Then
            Exit Sub
        End If

        Try
            btnRefreshCGX.Enabled = False
            btnRefreshCGX.Text = "Refreshing..."
            SetCgxStatus("READING")
            _lastProcessedUrl = String.Empty
            _lastProcessedTitle = String.Empty

            Dim captured As BrowserCaptureResult =
                Await Task.Run(
                    Function()
                        Return BrowserManager.CaptureCurrentCgxPage()
                    End Function)

            If captured Is Nothing Or captured.Context Is Nothing Then
                SetCgxStatus("WAITING")
                MessageBox.Show("Navigate to the Member Information or View Authorization page.", "Refresh CGX", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            Select Case captured.PageType
                Case CgxPageType.MemberInformation
                    ProcessDetectedMember(captured.Context)
                    ShowMemberInformationPanel()
                    SetCgxStatus("MEMBER")
                Case CgxPageType.ViewAuthorization
                    ProcessDetectedAuthorization(captured.Context)
                    ShowAuthorizationInformationPanel()
                    SetCgxStatus("AUTH")
                Case Else
                    SetCgxStatus("WAITING")
                    MessageBox.Show("The current page is not a supported CGX page.", "Refresh CGX", MessageBoxButtons.OK,
                        MessageBoxIcon.Information)
            End Select

            Dim currentUrl As String = String.Empty
            Dim currentTitle As String = String.Empty
            If BrowserManager.GetCurrentPageLocation(currentUrl, currentTitle) Then
                _lastProcessedUrl = currentUrl
                _lastProcessedTitle = currentTitle
            End If
        Catch ex As Exception
            SetCgxStatus("ERROR")
            MessageBox.Show(ex.ToString(), "Refresh CGX Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnRefreshCGX.Enabled = True
            btnRefreshCGX.Text = "Refresh CGX"
        End Try
    End Sub
    Private Function BuildCallContextFromUI() As CallContext
        Return New CallContext With {
        .CallerName = txtCallerName.Text.Trim(),
        .CallbackNumber = txtCallbackNum.Text.Trim(),
        .SecuredFax = txtSecuredFax.Text.Trim(),
        .CallingFrom = txtCallingFrom.Text.Trim(),
        .DateOfService = ParseOptionalDos(txtDOS.Text),
        .Scenario = If(cmbScenario.SelectedItem Is Nothing,
                       String.Empty,
                       cmbScenario.SelectedItem.ToString())
    }
    End Function
    Private Function ParseOptionalDate(input As String) As DateTime?
        If String.IsNullOrWhiteSpace(input) Then
            Return Nothing
        End If
        Dim parsedDate As DateTime

        If DateTime.TryParseExact(input.Trim(), "MM/dd/yyyy", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, parsedDate) Then
            Return parsedDate
        End If
        Return Nothing
    End Function
    Private Sub RunRecommendation()
        If _currentContext Is Nothing Then Exit Sub
        If _currentLookup Is Nothing Then Exit Sub

        Dim result As RecommendationResult = RecommendationEngine.Analyze(_currentContext, _currentLookup)
        RenderNextBestAction(result.NextBestAction)
        If result.RequiresAgentInput Then
            ShowActionQuestion(result.QuestionId, result.QuestionText, result.QuestionOptions)
        ElseIf _questionHistory.Count > 0 Then
            ShowBackOnly()
        Else
            ClearActionsPanel()
        End If
        RefreshOutputs()
    End Sub
    Private Sub ShowActionQuestion(questionId As String, questionText As String, options As IEnumerable(Of String))
        ClearActionsPanel()
        _currentQuestionId = questionId
        _currentQuestionText = questionText
        pnlActions.SuspendLayout()

        Dim panelWidth As Integer = Math.Max(200, pnlActions.ClientSize.Width)
        Dim questionLabel As New Label With {
        .Name = "lblDynamicQuestion",
        .Text = questionText,
        .AutoSize = False,
        .Font = New Font(
            "Segoe UI",
            10.0!,
            FontStyle.Bold),
        .ForeColor = Color.FromArgb(45, 55, 72),
        .BackColor = Color.White,
        .Left = 12,
        .Top = 10,
        .Width = Math.Max(150, panelWidth - 24),
        .Height = 40,
        .TextAlign = ContentAlignment.MiddleLeft
    }

        pnlActions.Controls.Add(questionLabel)
        Dim y As Integer = questionLabel.Bottom + 5
        For Each optionText As String In options

            Dim radio As New Guna.UI2.WinForms.Guna2RadioButton With {
            .Name = "rbAction_" & optionText.Replace(" ", "_"),
            .Text = optionText,
            .Tag = optionText,
            .AutoSize = False,
            .Left = 16,
            .Top = y,
            .Width = Math.Max(150, panelWidth - 32),
            .Height = 32, .Font = New Font("Segoe UI", 9.5!, FontStyle.Regular),
            .ForeColor = Color.FromArgb(55, 65, 81),
            .BackColor = Color.White,
            .Cursor = Cursors.Hand
        }
            'Green radio button styling
            radio.CheckedState.BorderColor = ColorPrimaryGreen
            radio.CheckedState.FillColor = ColorPrimaryGreen
            radio.CheckedState.InnerColor = Color.White
            radio.UncheckedState.BorderColor = Color.FromArgb(130, 145, 155)
            radio.UncheckedState.FillColor = Color.White

            AddHandler radio.CheckedChanged,
            AddressOf DynamicAction_CheckedChanged

            pnlActions.Controls.Add(radio)
            y += radio.Height + 6
        Next

        If _questionHistory.Count > 0 Then
            Dim backButton As Guna.UI2.WinForms.Guna2Button = CreateBackButton()
            backButton.Left = 12
            backButton.Top = y + 5
            pnlActions.Controls.Add(backButton)
            y = backButton.Bottom + 5
        End If

        pnlActions.Height = y + 10
        pnlActions.Visible = True
        pnlActions.ResumeLayout(True)
    End Sub
    Private Function CreateBackButton() As Guna.UI2.WinForms.Guna2Button

        Dim button As New Guna.UI2.WinForms.Guna2Button With {
        .Name = "btnDynamicBack",
        .Text = "← Back",
        .Width = 95,
        .Height = 32,
        .BorderRadius = 4,
        .Font = New Font("Segoe UI", 9.0!, FontStyle.Regular),
        .Cursor = Cursors.Hand,
        .FillColor = Color.FromArgb(108, 117, 125),
        .ForeColor = Color.White
    }

        AddHandler button.Click, AddressOf DynamicBackButton_Click
        Return button
    End Function
    Private Sub DynamicAction_CheckedChanged(sender As Object, e As EventArgs)

        Dim radio As Guna.UI2.WinForms.Guna2RadioButton = TryCast(sender, Guna.UI2.WinForms.Guna2RadioButton)
        If radio Is Nothing OrElse Not radio.Checked Then
            Exit Sub
        End If

        Dim selectedValue As String = Convert.ToString(radio.Tag)

        _questionHistory.Push(New QuestionState With {
            .QuestionId = _currentQuestionId,
            .QuestionText = _currentQuestionText,
            .SelectedAnswer = selectedValue
        })

        ApplyQuestionAnswer(_currentQuestionId, selectedValue)
        RunRecommendation()
    End Sub
    Private Sub ApplyQuestionAnswer(questionId As String, selectedValue As String)

        Select Case questionId
            Case "HEALTH_TYPE"
                _currentContext.HealthType = selectedValue
                _currentContext.CareSetting = Nothing
                _currentContext.IsExpedited = Nothing
                _currentContext.CallerType = Nothing
                _currentContext.ClinicalReviewNeeded = Nothing

            Case "CARE_SETTING"
                _currentContext.CareSetting = selectedValue
                _currentContext.IsExpedited = Nothing
                _currentContext.CallerType = Nothing
                _currentContext.ClinicalReviewNeeded = Nothing

            Case "EXPEDITED_REQUEST"
                _currentContext.IsExpedited = String.Equals(selectedValue, "YES", StringComparison.OrdinalIgnoreCase)
                _currentContext.CallerType = Nothing

            Case "CALLER_TYPE"
                _currentContext.CallerType = selectedValue

            Case "CLINICAL_REVIEW"
                _currentContext.ClinicalReviewNeeded = String.Equals(selectedValue, "YES", StringComparison.OrdinalIgnoreCase)

                _currentContext.IsExpedited = Nothing
                _currentContext.CallerType = Nothing

            Case "REQUEST_APPROVED_AUTH_COPY"
                _currentContext.ProviderRequestingApprovedAuthCopy = String.Equals(selectedValue, "YES", StringComparison.OrdinalIgnoreCase)
                _currentContext.ProviderRequestingLoaCopy = Nothing

            Case "PENDING_CLINICAL_REVIEW"
                _currentContext.PendingClinicalReview = String.Equals(selectedValue, "YES", StringComparison.OrdinalIgnoreCase)

            Case "REQUEST_LOA_COPY"
                _currentContext.ProviderRequestingLoaCopy = String.Equals(selectedValue, "YES", StringComparison.OrdinalIgnoreCase)

            Case "NEEDS_CLINICAL_ADVISOR"
                _currentContext.NeedsClinicalAdvisor = String.Equals(selectedValue, "YES", StringComparison.OrdinalIgnoreCase)
                _currentContext.ClinicalAttached = Nothing

            Case "CLINICAL_ATTACHED"
                _currentContext.ClinicalAttached = String.Equals(selectedValue, "YES", StringComparison.OrdinalIgnoreCase)

            Case "REQUEST_DENIAL_LETTER"
                _currentContext.RequestingDenialLetter = String.Equals(selectedValue, "YES", StringComparison.OrdinalIgnoreCase)
            Case "AUTH_REQUEST_FOUND"

                _currentContext.AuthRequestFound = String.Equals(selectedValue, "YES", StringComparison.OrdinalIgnoreCase)
                'Clear all downstream Check Status answers
                _currentContext.ProviderRequestingApprovedAuthCopy = Nothing
                _currentContext.ProviderRequestingLoaCopy = Nothing
                _currentContext.PendingClinicalReview = Nothing
                _currentContext.NeedsClinicalAdvisor = Nothing
                _currentContext.ClinicalAttached = Nothing
                _currentContext.RequestingDenialLetter = Nothing
                _currentContext.WantsToInitiateNewAuth = Nothing

            Case "INITIATE_NEW_AUTH"
                _currentContext.WantsToInitiateNewAuth = String.Equals(selectedValue, "YES", StringComparison.OrdinalIgnoreCase)
        End Select
    End Sub
    Private Sub DynamicBackButton_Click(sender As Object, e As EventArgs)

        If _questionHistory.Count = 0 Then
            Exit Sub
        End If

        Dim previousDecision As QuestionState = _questionHistory.Pop()
        ClearAnswerForQuestion(previousDecision.QuestionId)
        RunRecommendation()
    End Sub
    Private Sub ClearAnswerForQuestion(questionId As String)

        Select Case questionId
            Case "HEALTH_TYPE"
                _currentContext.HealthType = Nothing
                _currentContext.CareSetting = Nothing
                _currentContext.IsExpedited = Nothing
                _currentContext.CallerType = Nothing
                _currentContext.ClinicalReviewNeeded = Nothing
            Case "CARE_SETTING"
                _currentContext.CareSetting = Nothing
                _currentContext.IsExpedited = Nothing
                _currentContext.CallerType = Nothing
                _currentContext.ClinicalReviewNeeded = Nothing
            Case "EXPEDITED_REQUEST"
                _currentContext.IsExpedited = Nothing
                _currentContext.CallerType = Nothing
            Case "CALLER_TYPE"
                _currentContext.CallerType = Nothing
            Case "CLINICAL_REVIEW"
                _currentContext.ClinicalReviewNeeded = Nothing
                _currentContext.IsExpedited = Nothing
                _currentContext.CallerType = Nothing
            Case "REQUEST_APPROVED_AUTH_COPY"
                _currentContext.ProviderRequestingApprovedAuthCopy = Nothing
                _currentContext.ProviderRequestingLoaCopy = Nothing
            Case "PENDING_CLINICAL_REVIEW"
                _currentContext.PendingClinicalReview = Nothing
            Case "REQUEST_LOA_COPY"
                _currentContext.ProviderRequestingLoaCopy = Nothing
            Case "NEEDS_CLINICAL_ADVISOR"
                _currentContext.NeedsClinicalAdvisor = Nothing
                _currentContext.ClinicalAttached = Nothing
            Case "CLINICAL_ATTACHED"
                _currentContext.ClinicalAttached = Nothing
            Case "REQUEST_DENIAL_LETTER"
                _currentContext.RequestingDenialLetter = Nothing
            Case "AUTH_REQUEST_FOUND"
                _currentContext.AuthRequestFound = Nothing
                _currentContext.ProviderRequestingApprovedAuthCopy = Nothing
                _currentContext.ProviderRequestingLoaCopy = Nothing
                _currentContext.PendingClinicalReview = Nothing
                _currentContext.NeedsClinicalAdvisor = Nothing
                _currentContext.ClinicalAttached = Nothing
                _currentContext.RequestingDenialLetter = Nothing
                _currentContext.WantsToInitiateNewAuth = Nothing

            Case "INITIATE_NEW_AUTH"

                _currentContext.WantsToInitiateNewAuth = Nothing
        End Select
    End Sub
    Private Sub ShowBackOnly()

        ClearActionsPanel()
        pnlActions.SuspendLayout()

        Dim completedLabel As New Label With {
        .Text = "Decision completed. Use Back to change the previous answer.",
        .AutoSize = False,
        .Font = New Font("Segoe UI", 9.0!, FontStyle.Regular),
        .ForeColor = Color.FromArgb(75, 75, 75),
        .Left = 12,
        .Top = 10,
        .Width = Math.Max(180, pnlActions.ClientSize.Width - 24), .Height = 35
        }

        Dim backButton As Guna.UI2.WinForms.Guna2Button = CreateBackButton()
        backButton.Left = 12
        backButton.Top = completedLabel.Bottom + 5

        pnlActions.Controls.Add(completedLabel)
        pnlActions.Controls.Add(backButton)

        pnlActions.Height =
        backButton.Bottom + 12

        pnlActions.Visible = True

        pnlActions.ResumeLayout(True)

    End Sub
    Private Sub ClearActionsPanel()
        pnlActions.SuspendLayout()
        For Each existingControl As Control In pnlActions.Controls.Cast(Of Control)().ToList()
            pnlActions.Controls.Remove(existingControl)
            existingControl.Dispose()
        Next

        pnlActions.Visible = False
        _currentQuestionId = Nothing
        _currentQuestionText = Nothing
        pnlActions.ResumeLayout(True)
    End Sub
    Private Function ValidateCgxSearchInput() As Boolean
        If Not String.IsNullOrWhiteSpace(txtDOS.Text) Then
            Dim parsedDos As DateTime
            If Not DateTime.TryParseExact(txtDOS.Text.Trim(), "MMddyy", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, parsedDos) Then
                MessageBox.Show("Date of Service must use MMddyy format. Example: 080526.", "Invalid Date of Service", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtDOS.Focus()
                Return False
            End If
        End If
        Return True
    End Function
    Private Function FormatDateOfBirthForCgx(input As String) As String
        If String.IsNullOrWhiteSpace(input) Then
            Return String.Empty
        End If

        Dim parsedDate As DateTime
        If DateTime.TryParseExact(input.Trim(), "MM/dd/yyyy", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, parsedDate) Then
            Return parsedDate.ToString("MM/dd/yyyy")
        End If

        Return input.Trim()
    End Function
    Private Function BuildExtractedCgxOutput(context As CallContext) As String

        Dim output As New Text.StringBuilder()
        output.AppendLine("CGX INFORMATION")
        output.AppendLine(New String("-"c, 40))
        output.AppendLine("Product: " & DisplayTestValue(context.Product))
        output.AppendLine("Consolidated Selling Market: " & DisplayTestValue(context.Conso))
        output.AppendLine("State of Issue: " & DisplayTestValue(context.IssueState))
        output.AppendLine("Group Number: " & DisplayTestValue(context.GroupNumber))

        Return output.ToString()
    End Function
    Private Function BuildLookupTestOutput(context As CallContext, lookup As LookupResult) As String
        Dim output As New Text.StringBuilder()

        output.AppendLine("CGX INFORMATION")
        output.AppendLine(New String("-"c, 40))
        output.AppendLine("Product: " & DisplayTestValue(context.Product))
        output.AppendLine("Consolidated Selling Market: " & DisplayTestValue(context.Conso))
        output.AppendLine("State of Issue: " & DisplayTestValue(context.IssueState))
        output.AppendLine("Group Number: " & DisplayTestValue(context.GroupNumber))
        output.AppendLine()
        output.AppendLine()

        output.AppendLine("AUTHORIZATION INFORMATION")
        output.AppendLine(New String("-"c, 40))
        output.AppendLine("Authorization Number: " & DisplayTestValue(context.AuthorizationNumber))
        output.AppendLine("Authorization Status: " & DisplayTestValue(context.AuthorizationStatus))
        output.AppendLine("Start Date: " & DisplayTestValue(context.AuthorizationStartDate))
        output.AppendLine("End Date: " & DisplayTestValue(context.AuthorizationEndDate))
        output.AppendLine("Total Days: " & DisplayTestValue(context.TotalDays))
        output.AppendLine()
        output.AppendLine("Requesting Provider:")
        output.AppendLine(DisplayTestValue(context.RequestingProvider))
        output.AppendLine()
        output.AppendLine("Treating Provider:")
        output.AppendLine(DisplayTestValue(context.TreatingProvider))
        output.AppendLine()
        output.AppendLine("Facility Provider:")
        output.AppendLine(DisplayTestValue(context.FacilityProvider))
        output.AppendLine()
        output.AppendLine("Primary Diagnosis: " & DisplayTestValue(context.PrimaryDiagnosisCode))
        output.AppendLine("Secondary Diagnoses: " & If(context.SecondaryDiagnosisCodes IsNot Nothing AndAlso context.SecondaryDiagnosisCodes.Count > 0, String.Join(", ", context.SecondaryDiagnosisCodes), "Not found"))
        output.AppendLine("Procedure Codes: " & If(context.ProcedureCodes IsNot Nothing AndAlso context.ProcedureCodes.Count > 0, String.Join(", ", context.ProcedureCodes), "Not found"))

        output.AppendLine("DATABASE LOOKUPS")
        output.AppendLine(New String("-"c, 40))
        output.AppendLine("OUT OF SCOPE")
        output.AppendLine("Result: " & FormatNullableBoolean(lookup.IsOutOfScope, "OUT OF SCOPE", "NOT OUT OF SCOPE"))
        output.AppendLine("Details: " & DisplayTestValue(lookup.OutOfScopeMessage))
        output.AppendLine("Restriction: " & DisplayTestValue(lookup.RestrictionType))
        output.AppendLine()
        output.AppendLine("MARKET GUIDE")
        output.AppendLine("Found: " & If(lookup.MarketGuideFound, "YES", "NO"))
        output.AppendLine("Reference: " & DisplayTestValue(lookup.MarketGuideReference))
        output.AppendLine("Details: " & DisplayTestValue(lookup.MarketGuideMessage))
        output.AppendLine()
        output.AppendLine("PAL")

        output.AppendLine("Found: " & If(lookup.PalFound, "YES", "NO"))
        If context.ProcedureCodes Is Nothing OrElse context.ProcedureCodes.Count = 0 Then
            output.AppendLine("Procedure Codes: Not yet extracted from CGX")
        Else
            output.AppendLine("Procedure Codes: " & String.Join(", ", context.ProcedureCodes))
        End If

        If lookup.PalResults Is Nothing OrElse lookup.PalResults.Count = 0 Then
            output.AppendLine("Result: No PAL result returned.")
        Else
            For Each palResult As String In lookup.PalResults
                output.AppendLine("• " & palResult)
            Next
        End If

        Return output.ToString()
    End Function
    Private Function BuildDocumentation(context As CallContext) As String
        Dim output As New Text.StringBuilder()
        output.AppendLine("DOCUMENTATION")
        output.AppendLine(New String("-"c, 45))
        output.AppendLine("Name: " & DisplayDocumentationValue(context.CallerName))
        output.AppendLine("Direct #: " & DisplayDocumentationValue(context.CallbackNumber))
        output.AppendLine("Secured Fax: " & DisplayDocumentationValue(context.SecuredFax))
        output.AppendLine("Calling From: " & DisplayDocumentationValue(context.CallingFrom))
        output.AppendLine()
        output.AppendLine("Member ID: " & DisplayDocumentationValue(context.MemberId))
        output.AppendLine("Date of Birth: " & DisplayDocumentationValue(context.DateOfBirth))
        output.AppendLine()
        output.AppendLine("Genesys Verification: " & If(context.GenesysVerified, "Yes", "No"))
        output.AppendLine("Provider/Member Authenticated: " & If(context.ProviderMemberAuthenticated, "Yes", "No"))
        output.AppendLine("Mailing Address Verified: " & If(context.MailingAddressVerified, "Yes", "No"))
        output.AppendLine()
        output.AppendLine("Concern: " & DisplayDocumentationValue(context.Scenario))
        output.AppendLine("Date of Service: " & FormatDocumentationDate(context.DateOfService))
        output.AppendLine()
        output.AppendLine("Requesting Provider: " & DisplayDocumentationValue(context.RequestingProvider))
        output.AppendLine()
        output.AppendLine("Treating Provider: " & DisplayDocumentationValue(context.TreatingProvider))
        output.AppendLine()
        output.AppendLine("Facility Provider: " & DisplayDocumentationValue(context.FacilityProvider))
        output.AppendLine()
        output.AppendLine("DX: " & BuildDiagnosisText(context))
        output.AppendLine("PX: " & BuildProcedureText(context))
        output.AppendLine()
        output.AppendLine("Addt'l Notes: " & DisplayDocumentationValue(context.ClaimPaymentNotes))
        output.AppendLine(Environment.UserName & " / ManilaCIT")

        Return output.ToString()
    End Function
    Private Function CombineName(firstName As String, lastName As String) As String
        Dim fullName As String = (firstName & " " & lastName).Trim()
        Return DisplayDocumentationValue(fullName)
    End Function
    Private Function DisplayTestValue(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then
            Return "Not found"
        End If
        Return value.Trim()
    End Function
    Private Function DisplayDocumentationValue(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then
            Return String.Empty
        End If
        Return value.Trim()
    End Function
    Private Function BuildDiagnosisText(context As CallContext) As String
        Dim diagnosisCodes As New List(Of String)
        If Not String.IsNullOrWhiteSpace(context.PrimaryDiagnosisCode) Then
            diagnosisCodes.Add(context.PrimaryDiagnosisCode.Trim())
        End If
        If context.SecondaryDiagnosisCodes IsNot Nothing Then
            For Each code As String In context.SecondaryDiagnosisCodes
                If String.IsNullOrWhiteSpace(code) Then
                    Continue For
                End If
                If Not diagnosisCodes.Any(
                Function(existingCode)
                    Return String.Equals(
                        existingCode,
                        code.Trim(),
                        StringComparison.OrdinalIgnoreCase)
                End Function) Then
                    diagnosisCodes.Add(
                    code.Trim())
                End If
            Next
        End If
        Return String.Join(", ", diagnosisCodes)
    End Function
    Private Function BuildProcedureText(context As CallContext) As String
        If context.ProcedureCodes Is Nothing Then
            Return String.Empty
        End If

        Dim procedureCodes As List(Of String) = context.ProcedureCodes.
            Where(
                Function(code)
                    Return Not String.IsNullOrWhiteSpace(code)
                End Function).
            Select(
                Function(code)
                    Return code.Trim()
                End Function).
            Distinct(
                StringComparer.OrdinalIgnoreCase).
            ToList()
        Return String.Join(", ", procedureCodes)
    End Function
    Private Function FormatNullableBoolean(value As Boolean?, trueText As String, falseText As String) As String
        If Not value.HasValue Then
            Return "Unable to determine"
        End If

        Return If(value.Value, trueText, falseText)
    End Function
    Private Sub ClearScenarioDecisionState()
        _questionHistory.Clear()
        _currentQuestionId = Nothing
        _currentQuestionText = Nothing

        ClearActionsPanel()
        If _currentContext Is Nothing Then Exit Sub
        '_currentContext.HealthType = Nothing
        '_currentContext.CareSetting = Nothing
        _currentContext.IsExpedited = Nothing
        _currentContext.CallerType = Nothing
        _currentContext.ClinicalReviewNeeded = Nothing

        'Checking Status decisions
        _currentContext.ProviderRequestingApprovedAuthCopy = Nothing
        _currentContext.PendingClinicalReview = Nothing
        _currentContext.ProviderRequestingLoaCopy = Nothing
        _currentContext.NeedsClinicalAdvisor = Nothing
        _currentContext.ClinicalAttached = Nothing
        _currentContext.RequestingDenialLetter = Nothing
        _currentContext.AuthRequestFound = Nothing
        _currentContext.WantsToInitiateNewAuth = Nothing
    End Sub
    Private Sub ProcessDetectedMember(detected As CallContext)
        Dim detectedMemberId As String = NormalizeIdentifier(detected.MemberId)
        If String.IsNullOrWhiteSpace(detectedMemberId) Then
            Exit Sub
        End If

        Dim currentMemberId As String = If(_currentContext Is Nothing, String.Empty, NormalizeIdentifier(_currentContext.MemberId))

        'Same member already loaded.
        If String.Equals(currentMemberId, detectedMemberId, StringComparison.OrdinalIgnoreCase) Then
            Exit Sub
        End If

        'User already rejected this same detected member.
        If String.Equals(_ignoredMemberId, detectedMemberId, StringComparison.OrdinalIgnoreCase) Then
            Exit Sub
        End If

        If Not String.IsNullOrWhiteSpace(currentMemberId) Then

            Dim response As DialogResult =
                MessageBox.Show(
                    "CGX is showing a different member." &
                    Environment.NewLine &
                    Environment.NewLine &
                    "Current Member ID: " &
                    currentMemberId &
                    Environment.NewLine &
                    "Detected Member ID: " &
                    detectedMemberId &
                    Environment.NewLine &
                    Environment.NewLine &
                    "Use the detected member information?" &
                    Environment.NewLine &
                    Environment.NewLine &
                    "Selecting Yes will scrape the member information again and clear the current authorization and scenario results.",
                    "Different Member Detected",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question)

            If response <> DialogResult.Yes Then
                _ignoredMemberId =
                    detectedMemberId
                Exit Sub
            End If
        End If

        _ignoredMemberId = Nothing
        _ignoredAuthorizationId = Nothing
        ApplyDetectedMember(detected)

    End Sub
    Private Sub ApplyDetectedMember(detected As CallContext)
        If detected Is Nothing Then
            Exit Sub
        End If

        If _currentContext Is Nothing Then
            _currentContext = New CallContext()
        End If

        'Agent-entered documentation values
        _currentContext.CallerName = txtCallerName.Text.Trim()
        _currentContext.CallbackNumber = txtCallbackNum.Text.Trim()
        _currentContext.SecuredFax = txtSecuredFax.Text.Trim()
        _currentContext.CallingFrom = txtCallingFrom.Text.Trim()
        _currentContext.DateOfService = ParseOptionalDos(txtDOS.Text)
        _currentContext.Extension = txtExtension.Text.Trim()
        'Member information from CGX
        _currentContext.MemberId = detected.MemberId
        _currentContext.MemberName = detected.MemberName
        _currentContext.DateOfBirth = detected.DateOfBirth
        _currentContext.Product = NormalizeProductForLookup(detected.Product)
        _currentContext.Conso = detected.Conso
        _currentContext.IssueState = detected.IssueState
        _currentContext.GroupNumber = detected.GroupNumber

        'New/refreshed member means old Auth data
        'must not remain in memory.
        ClearAuthorizationInformation()
        ClearScenarioDecisionState()

        'Run member-level lookups.
        _currentLookup = CallPilotRepository.RunLookups(_currentContext)
        'Populate member information.
        SetOutputValue(txtMemberInfo, OutputFormatter.BuildMemberInformation(_currentContext))
        ShowMemberInformationPanel()
        txtAuthInfo.Clear()
        rtbPAL.Clear()
        'Documentation only.
        txtOverAllOutput.Text = OutputFormatter.BuildDocumentation(_currentContext)
        RenderNextBestAction("Member information refreshed. Open an authorization in CGX or select a scenario when ready.")
        RefreshOutputs()
    End Sub
    Private Sub ClearAuthorizationInformation()
        If _currentContext Is Nothing Then
            Exit Sub
        End If

        _currentContext.AuthorizationNumber = Nothing
        _currentContext.AuthorizationStatus = Nothing

        _currentContext.RequestingProvider = Nothing
        _currentContext.TreatingProvider = Nothing
        _currentContext.FacilityProvider = Nothing

        _currentContext.AuthorizationStartDate = Nothing
        _currentContext.AuthorizationEndDate = Nothing
        _currentContext.TotalDays = Nothing

        _currentContext.PrimaryDiagnosisCode = Nothing
        _currentContext.ClaimPaymentNotes = Nothing

        _currentContext.AuthType = Nothing
        _currentContext.HealthType = Nothing
        _currentContext.CareSetting = Nothing

        _currentContext.SecondaryDiagnosisCodes.Clear()
        _currentContext.ProcedureCodes.Clear()
    End Sub
    Private Sub ProcessDetectedAuthorization(detected As CallContext)
        Dim detectedAuthId As String = NormalizeIdentifier(detected.AuthorizationNumber)
        If String.IsNullOrWhiteSpace(detectedAuthId) Then
            Exit Sub
        End If

        If _currentContext Is Nothing Then
            MessageBox.Show("Open the Member Information page first so CallPilot can associate this authorization with a member.", "Member Information Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim currentAuthId As String = NormalizeIdentifier(_currentContext.AuthorizationNumber)
        If String.Equals(currentAuthId, detectedAuthId, StringComparison.OrdinalIgnoreCase) Then
            Exit Sub
        End If

        If String.Equals(_ignoredAuthorizationId, detectedAuthId, StringComparison.OrdinalIgnoreCase) Then
            Exit Sub
        End If

        If Not String.IsNullOrWhiteSpace(currentAuthId) Then
            Dim response As DialogResult =
                MessageBox.Show(
                    "CGX is showing a different authorization." &
                    Environment.NewLine &
                    Environment.NewLine &
                    "Current Authorization: " &
                    currentAuthId &
                    Environment.NewLine &
                    "Detected Authorization: " &
                    detectedAuthId &
                    Environment.NewLine &
                    Environment.NewLine &
                    "Use the detected authorization information?" &
                    Environment.NewLine &
                    Environment.NewLine &
                    "Selecting Yes will scrape the authorization information again and reset the current scenario decisions.",
                    "Different Authorization Detected",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question)

            If response <> DialogResult.Yes Then
                _ignoredAuthorizationId = detectedAuthId
                Exit Sub
            End If
        End If
        _ignoredAuthorizationId = Nothing
        ApplyDetectedAuthorization(detected)
    End Sub
    Private Sub ApplyDetectedAuthorization(detected As CallContext)

        _currentContext.AuthorizationNumber = detected.AuthorizationNumber
        _currentContext.AuthorizationStatus = detected.AuthorizationStatus
        _currentContext.AuthType = detected.AuthType
        _currentContext.HealthType = detected.HealthType
        _currentContext.CareSetting = detected.CareSetting
        _currentContext.RequestingProvider = detected.RequestingProvider
        _currentContext.TreatingProvider = detected.TreatingProvider
        _currentContext.FacilityProvider = detected.FacilityProvider
        _currentContext.AuthorizationStartDate = detected.AuthorizationStartDate
        _currentContext.AuthorizationEndDate = detected.AuthorizationEndDate
        _currentContext.TotalDays = detected.TotalDays
        _currentContext.PrimaryDiagnosisCode = detected.PrimaryDiagnosisCode
        _currentContext.ClaimPaymentNotes = detected.ClaimPaymentNotes
        _currentContext.SecondaryDiagnosisCodes.Clear()
        _currentContext.AdmissionDate = detected.AdmissionDate
        _currentContext.DischargeDate = detected.DischargeDate

        If detected.SecondaryDiagnosisCodes IsNot Nothing Then
            _currentContext.SecondaryDiagnosisCodes.AddRange(detected.SecondaryDiagnosisCodes)
        End If

        _currentContext.ProcedureCodes.Clear()
        If detected.ProcedureCodes IsNot Nothing Then
            _currentContext.ProcedureCodes.AddRange(detected.ProcedureCodes)
        End If

        'Run again because PAL now has procedure codes.
        _currentLookup = CallPilotRepository.RunLookups(_currentContext)

        ClearScenarioDecisionState()
        SetAuthorizationPopulatedState()
        RenderNextBestAction("Authorization information detected. Select a scenario and click Select.")
        ShowAuthorizationInformationPanel()
        RefreshOutputs()
    End Sub
    Private Function NormalizeIdentifier(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then
            Return String.Empty
        End If

        Return value.Replace(" ", String.Empty).Replace("-", String.Empty).Trim().ToUpperInvariant()
    End Function
    Private Function ParseOptionalDos(input As String) As DateTime?
        If String.IsNullOrWhiteSpace(input) Then
            Return Nothing
        End If

        Dim parsedDate As DateTime
        If DateTime.TryParseExact(input.Trim(), "MMddyy", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, parsedDate) Then
            Return parsedDate
        End If

        Return Nothing
    End Function
    Private Function FormatDocumentationDate(value As DateTime?) As String
        If Not value.HasValue Then
            Return String.Empty
        End If
        Return value.Value.ToString("MM/dd/yyyy")
    End Function
    Private Sub RefreshOutputs()
        If _currentContext Is Nothing Then
            Return
        End If
        SyncVerificationToContext()

        If _currentContext Is Nothing Then
            txtMemberInfo.Clear()
            txtAuthInfo.Clear()
            txtOverAllOutput.Clear()
        Else
            SetOutputValue(txtMemberInfo, OutputFormatter.BuildMemberInformation(_currentContext))
            '========================================
            ' AUTH INFORMATION
            '========================================
            If ShouldHideAuthorizationOutput() Then
                txtAuthInfo.Clear()
            Else
                SetOutputValue(txtAuthInfo, OutputFormatter.BuildAuthorizationInformation(_currentContext))
            End If
            txtOverAllOutput.Text = OutputFormatter.BuildDocumentation(_currentContext)

        End If
        '========================================
        ' LOOKUPS
        '========================================
        If _currentLookup Is Nothing Then
            rtbOutOfScope.Clear()
            rtbMarketGuide.Clear()
            rtbPAL.Clear()
        Else
            'rtbOutOfScope.Text = OutputFormatter.BuildOutOfScope(_currentLookup)
            RenderOutOfScope()
            rtbMarketGuide.Text = OutputFormatter.BuildMarketGuide(_currentLookup)
            If ShouldHideAuthorizationOutput() Then
                rtbPAL.Clear()
                SetPALWaitingState()
            Else
                rtbPAL.Text = OutputFormatter.BuildPal(_currentContext, _currentLookup)
                ApplyPALVisualState()
            End If
            'IMPORTANT - do this after assigning the text
            ApplyOutOfScopeVisualState()
            ApplyMarketGuideVisualState()
        End If
    End Sub
    Private Function ShouldHideAuthorizationOutput() As Boolean
        If _currentContext Is Nothing Then
            Return True
        End If

        If Not String.Equals(_currentContext.Scenario, "CHECKING STATUS OF THE AUTHORIZATION", StringComparison.OrdinalIgnoreCase) Then
            Return False
        End If
        'Only hide after the agent explicitly answered NO.
        Return _currentContext.AuthRequestFound.HasValue AndAlso Not _currentContext.AuthRequestFound.Value

    End Function
    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        'TestUpdatingAuthorizationOffline()
        TestCurrentMarketGuideLookup()
    End Sub
    Private Sub TestLouisianaMarketGuide()

        Try

            '========================================
            ' TEST CGX MEMBER DATA
            '========================================
            Dim testContext As New CallContext With {
            .CallerName = "TEST CALLER",
            .MemberId = "TEST123",
            .MemberName = "TEST MEMBER",
            .DateOfBirth = "01/01/1980",
            .Product = "Medicare HMO",
            .Conso = "CONSOL - LOUISIANA",
            .IssueState = "LA",
            .GroupNumber = ""
        }


            '========================================
            ' RUN THE REAL DATABASE LOOKUP
            '========================================
            Dim testLookup As LookupResult =
            CallPilotRepository.RunLookups(testContext)


            If testLookup Is Nothing Then

                MessageBox.Show(
                "RunLookups returned Nothing.",
                "Market Guide Test",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)

                Return

            End If


            '========================================
            ' SHOW RAW LOOKUP RESULT
            '========================================
            Dim output As New Text.StringBuilder()

            output.AppendLine("MARKET GUIDE DATABASE TEST")
            output.AppendLine(New String("-"c, 45))
            output.AppendLine()

            output.AppendLine(
            "Product: " &
            testContext.Product)

            output.AppendLine(
            "CSM: " &
            testContext.Conso)

            output.AppendLine(
            "State: " &
            testContext.IssueState)

            output.AppendLine()

            output.AppendLine(
            "Market Guide Found: " &
            If(testLookup.MarketGuideFound,
               "YES",
               "NO"))

            output.AppendLine()

            output.AppendLine(
            "Reference:")

            output.AppendLine(
            If(String.IsNullOrWhiteSpace(
                   testLookup.MarketGuideReference),
               "[blank]",
               testLookup.MarketGuideReference))

            output.AppendLine()

            output.AppendLine(
            "Database Details:")

            output.AppendLine(
            If(String.IsNullOrWhiteSpace(
                   testLookup.MarketGuideMessage),
               "[blank]",
               testLookup.MarketGuideMessage))


            MessageBox.Show(
            output.ToString(),
            "Louisiana Market Guide Test",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)


            '========================================
            ' ALSO SHOW RESULT IN MARKET GUIDE UI
            '========================================
            rtbMarketGuide.Text =
            OutputFormatter.BuildMarketGuide(
                testLookup)

            pnlMarketGuideSection.Visible = True

        Catch ex As Exception

            MessageBox.Show(
            ex.ToString(),
            "Market Guide Test Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub TestCurrentMarketGuideLookup()

        Try

            If _currentContext Is Nothing Then

                MessageBox.Show(
                "No live member context is loaded.",
                "Market Guide Test",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)

                Return

            End If


            Dim lookup As LookupResult =
            CallPilotRepository.RunLookups(_currentContext)


            Dim output As New Text.StringBuilder()

            output.AppendLine("LIVE MARKET GUIDE TEST")
            output.AppendLine(New String("-"c, 45))
            output.AppendLine()

            output.AppendLine(
            "Product: [" &
            _currentContext.Product &
            "]")

            output.AppendLine(
            "CSM: [" &
            _currentContext.Conso &
            "]")

            output.AppendLine(
            "State: [" &
            _currentContext.IssueState &
            "]")

            output.AppendLine(
            "Group: [" &
            _currentContext.GroupNumber &
            "]")

            output.AppendLine()

            output.AppendLine(
            "Market Guide Found: " &
            If(lookup.MarketGuideFound, "YES", "NO"))

            output.AppendLine()

            output.AppendLine(
            "Reference: " &
            If(String.IsNullOrWhiteSpace(
                   lookup.MarketGuideReference),
               "[blank]",
               lookup.MarketGuideReference))

            output.AppendLine()

            output.AppendLine(
            "Details: " &
            If(String.IsNullOrWhiteSpace(
                   lookup.MarketGuideMessage),
               "[blank]",
               lookup.MarketGuideMessage))


            MessageBox.Show(
            output.ToString(),
            "Live Market Guide Lookup",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

        Catch ex As Exception

            MessageBox.Show(
            ex.ToString(),
            "Market Guide Test Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub TestUpdatingAuthorizationOffline()

        Try

            Dim testContext As New CallContext With {
            .CallerName = "TEST CALLER",
            .CallbackNumber = "5551234567",
            .SecuredFax = "5559876543",
            .CallingFrom = "TEST PROVIDER",
            .MemberId = "TEST123",
            .MemberName = "TEST MEMBER",
            .DateOfBirth = "01/01/1980",
            .Product = "Medicare HMO",
            .Conso = "TEST MARKET",
            .IssueState = "FL",
            .GroupNumber = "TESTGROUP",
            .Scenario = "UPDATING AUTHORIZATION",
            .HealthType = "PHYSICAL HEALTH",
            .CareSetting = "OUTPATIENT",
            .AuthorizationNumber = "TESTAUTH123",
            .AuthorizationStatus = "APPROVED",
            .AuthorizationStartDate = "08/01/2026",
            .AuthorizationEndDate =
                "10/4/2026" &
                Environment.NewLine &
                "8/10/2026",
            .TotalDays = "30",
            .RequestingProvider = "TEST REQUESTING PROVIDER",
            .TreatingProvider = "TEST TREATING PROVIDER",
            .FacilityProvider = "TEST FACILITY",
            .PrimaryDiagnosisCode = "Z00.00",
            .ClaimPaymentNotes = "TEST NOTES"
        }

            testContext.SecondaryDiagnosisCodes.Add("Z01.89")
            testContext.SecondaryDiagnosisCodes.Add("Z02.9")

            testContext.ProcedureCodes.Add("99213")
            testContext.ProcedureCodes.Add("99214")

            Dim testLookup As New LookupResult With
                {
            .IsOutOfScope = False,
            .OutOfScopeMessage = "Test member is in scope.",
            .MarketGuideFound = True,
            .MarketGuideMessage = "TEST MARKET GUIDE",
            .PalFound = True
                }

            testLookup.PalResults.Add(
            "TEST PAL RESULT")
            Dim result As RecommendationResult = RecommendationEngine.Analyze(testContext, testLookup)

            MessageBox.Show(
                "TEST COMPLETED SUCCESSFULLY" &
                Environment.NewLine &
                Environment.NewLine &
                "Next Best Action:" &
                Environment.NewLine &
                result.NextBestAction,
                "Offline Recommendation Test",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

        Catch ex As Exception

            MessageBox.Show(
                ex.ToString(),
                "Offline Test Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub rtbNextBestAction_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles rtbNextBestAction.LinkClicked
        Try
            Process.Start(New ProcessStartInfo With {.FileName = e.LinkText, .UseShellExecute = True})
        Catch ex As Exception
            MessageBox.Show(
                "Unable to open the link." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Open Link",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
        End Try
    End Sub
    Private Sub RenderNextBestAction(rawText As String)
        rtbNextBestAction.Clear()
        _nextBestActionLinks.Clear()

        If String.IsNullOrWhiteSpace(rawText) Then
            Exit Sub
        End If

        Dim lines As String() = rawText.Replace(vbCrLf, vbLf).Replace(vbCr, vbLf).Split(ControlChars.Lf)
        For Each line As String In lines

            Dim cleanLine As String = line.Trim()
            If String.IsNullOrWhiteSpace(cleanLine) Then
                rtbNextBestAction.AppendText(Environment.NewLine)
                Continue For
            End If

            'Guide link
            If cleanLine.StartsWith("[LINK]", StringComparison.OrdinalIgnoreCase) Then
                RenderGuideLink(cleanLine)
                Continue For
            End If

            'End of process
            If cleanLine.IndexOf("END OF THE PROCESS", StringComparison.OrdinalIgnoreCase) >= 0 Then
                RenderEndOfProcess()
                Continue For
            End If

            'Header
            If String.Equals(cleanLine, "NEXT BEST ACTION", StringComparison.OrdinalIgnoreCase) Then
                RenderNextBestActionHeader()
                Continue For
            End If

            'Skip old separator lines
            If cleanLine.All(
            Function(c)
                Return c = "-"c
            End Function) Then
                Continue For
            End If

            'Numbered action
            If Char.IsDigit(cleanLine(0)) And cleanLine.Contains(".") Then
                RenderActionStep(cleanLine)
                Continue For
            End If
            'Normal text
            AppendNormalActionText(cleanLine)
        Next
        rtbNextBestAction.SelectionStart = 0
        rtbNextBestAction.ScrollToCaret()
    End Sub
    Private Sub RenderNextBestActionHeader()
        Dim startPosition As Integer = rtbNextBestAction.TextLength
        'rtbNextBestAction.AppendText("NEXT BEST ACTION" & Environment.NewLine & Environment.NewLine)
        'rtbNextBestAction.Select(startPosition, "NEXT BEST ACTION".Length)
        rtbNextBestAction.SelectionFont = New Font("Segoe UI Semibold", 11.0!, FontStyle.Bold)
        rtbNextBestAction.SelectionColor = ColorDarkGreen
        ResetNextBestActionFormatting()
    End Sub
    Private Sub RenderActionStep(actionText As String)
        Dim separatorIndex As Integer = actionText.IndexOf("."c)

        If separatorIndex <= 0 Then
            AppendNormalActionText(actionText)
            Exit Sub
        End If

        Dim stepNumber As String = actionText.Substring(0, separatorIndex + 1)
        Dim description As String = actionText.Substring(separatorIndex + 1).Trim()
        Dim numberStart As Integer = rtbNextBestAction.TextLength
        rtbNextBestAction.AppendText(stepNumber & " ")
        rtbNextBestAction.Select(numberStart, stepNumber.Length)
        rtbNextBestAction.SelectionFont = New Font("Segoe UI Semibold", 10.0!, FontStyle.Bold)
        rtbNextBestAction.SelectionColor = ColorPrimaryGreen
        ResetNextBestActionFormatting()
        rtbNextBestAction.AppendText(description & Environment.NewLine & Environment.NewLine)
    End Sub
    Private Sub AppendNormalActionText(text As String)
        rtbNextBestAction.SelectionFont = New Font("Segoe UI", 9.5!, FontStyle.Regular)
        rtbNextBestAction.SelectionColor = ColorDarkGreen
        rtbNextBestAction.AppendText(text & Environment.NewLine & Environment.NewLine)
    End Sub
    Private Sub RenderEndOfProcess()
        Dim startPosition As Integer = rtbNextBestAction.TextLength
        Dim endText As String = "✓ END OF THE PROCESS"
        rtbNextBestAction.AppendText(endText & Environment.NewLine)
        rtbNextBestAction.Select(startPosition, endText.Length)
        rtbNextBestAction.SelectionFont = New Font("Segoe UI Semibold", 10.0!, FontStyle.Bold)
        rtbNextBestAction.SelectionColor = ColorDarkGreen
        ResetNextBestActionFormatting()
    End Sub
    Private Sub ResetNextBestActionFormatting()
        rtbNextBestAction.Select(rtbNextBestAction.TextLength, 0)
        rtbNextBestAction.SelectionFont = New Font("Segoe UI", 9.5!, FontStyle.Regular)
        rtbNextBestAction.SelectionColor = ColorDarkGreen
    End Sub
    Private Sub RenderGuideLink(linkLine As String)
        Dim content As String = linkLine.Substring(6)
        Dim separatorIndex As Integer = content.IndexOf("|"c)

        If separatorIndex < 0 Then
            Return
        End If

        Dim displayText As String = content.Substring(0, separatorIndex).Trim()
        Dim url As String = content.Substring(separatorIndex + 1).Trim()

        If String.IsNullOrWhiteSpace(displayText) Or String.IsNullOrWhiteSpace(url) Then
            Return
        End If

        Dim startPosition As Integer = rtbNextBestAction.TextLength
        rtbNextBestAction.AppendText("↗ " & displayText)
        Dim visibleLength As Integer = ("↗ " & displayText).Length
        rtbNextBestAction.Select(startPosition, visibleLength)
        rtbNextBestAction.SelectionColor = ColorDarkGreen
        rtbNextBestAction.SelectionFont = New Font("Segoe UI Semibold", 9.5!, FontStyle.Underline)
        Dim key As String = startPosition.ToString() & ":" & visibleLength.ToString()
        _nextBestActionLinks(key) = url
        ResetNextBestActionFormatting()
        rtbNextBestAction.AppendText(Environment.NewLine & Environment.NewLine)
    End Sub
    Private Sub rtbNextBestAction_MouseUp(sender As Object, e As MouseEventArgs) Handles rtbNextBestAction.MouseUp
        Dim characterIndex As Integer = rtbNextBestAction.GetCharIndexFromPosition(e.Location)

        For Each pair In _nextBestActionLinks
            Dim parts As String() = pair.Key.Split(":"c)
            If parts.Length <> 2 Then
                Continue For
            End If

            Dim startPosition As Integer = Integer.Parse(parts(0))
            Dim length As Integer = Integer.Parse(parts(1))
            If characterIndex >= startPosition And characterIndex < startPosition + length Then
                OpenGuideLink(pair.Value)
                Exit For
            End If
        Next
    End Sub
    Private Sub OpenGuideLink(url As String)
        Try

            Process.Start(
                New ProcessStartInfo With {
                    .FileName = url,
                    .UseShellExecute = True
                })

        Catch ex As Exception
            MessageBox.Show(
                "Unable to open the guide." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Open Guide",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub btnCopyDocumentation_Click(sender As Object, e As EventArgs) Handles btnCopyDocumentation.Click
        If String.IsNullOrWhiteSpace(txtOverAllOutput.Text) Then
            Exit Sub
        End If
        Clipboard.SetText(txtOverAllOutput.Text)
    End Sub

    Private Sub chkGenesysVerified_CheckedChanged(sender As Object, e As EventArgs)
        If _currentContext Is Nothing Then
            Exit Sub
        End If

        _currentContext.GenesysVerified = chkGenesysVerified.Checked
        RefreshOutputs()
    End Sub
    Private Sub chkProviderMemberAuthenticated_CheckedChanged(sender As Object, e As EventArgs)
        If _currentContext Is Nothing Then
            Exit Sub
        End If
        _currentContext.ProviderMemberAuthenticated = chkProviderAuthenticated.Checked
        RefreshOutputs()
    End Sub
    Private Sub chkMailingAddressVerified_CheckedChanged(sender As Object, e As EventArgs)
        If _currentContext Is Nothing Then
            Exit Sub
        End If
        _currentContext.MailingAddressVerified = chkMailingAddressVerified.Checked
        RefreshOutputs()
    End Sub
    Private Sub UpdateScenarioVisibility()
        Dim scenario As String = If(cmbScenario.SelectedItem Is Nothing, String.Empty, cmbScenario.SelectedItem.ToString().Trim().ToUpperInvariant())

        Select Case scenario
            Case "UPDATING AUTHORIZATION"
                pnlOutOfScopeSection.Visible = True
                pnlMarketGuideSection.Visible = True
                pnlPALSection.Visible = True

            Case "CHECKING STATUS OF THE AUTHORIZATION"
                pnlOutOfScopeSection.Visible = True
                pnlMarketGuideSection.Visible = True
                pnlPALSection.Visible = True

            Case "NEW AUTHORIZATION"
                pnlOutOfScopeSection.Visible = False
                pnlMarketGuideSection.Visible = False
                pnlPALSection.Visible = False

            Case Else
                pnlOutOfScopeSection.Visible = False
                pnlMarketGuideSection.Visible = False
                pnlPALSection.Visible = False
        End Select

    End Sub
    Private Sub cmbScenario_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbScenario.SelectedIndexChanged
        If cmbScenario.SelectedIndex < 0 Then
            Exit Sub
        End If

        UpdateScenarioVisibility()
        If _currentContext Is Nothing Or String.IsNullOrWhiteSpace(_currentContext.MemberId) Then
            Return
        End If

        Try
            _currentContext.CallerName = txtCallerName.Text.Trim()
            _currentContext.CallbackNumber = txtCallbackNum.Text.Trim()
            _currentContext.SecuredFax = txtSecuredFax.Text.Trim()
            _currentContext.CallingFrom = txtCallingFrom.Text.Trim()
            _currentContext.DateOfService = ParseOptionalDos(txtDOS.Text)
            _currentContext.Extension = txtExtension.Text.Trim()
            ClearScenarioDecisionState()
            _currentContext.Scenario = Convert.ToString(cmbScenario.SelectedItem)
            RunRecommendation()
        Catch ex As Exception
            MessageBox.Show(ex.ToString(), "Scenario Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub ApplyMarketGuideVisualState()

        If _currentLookup Is Nothing Then
            SetMarketGuideWaitingState()
            Return
        End If

        If _currentLookup.MarketGuideFound Then
            'FOUND = GREEN
            pnlMarketGuideSection.FillColor = ColorSuccessBackground
            pnlMarketGuideSection.BackColor = ColorSuccessBackground
            pnlMarketGuideSection.BorderColor = ColorSuccessBorder

            rtbMarketGuide.BackColor = ColorSuccessBackground
            rtbMarketGuide.ForeColor = ColorSuccessText

            lblLookupMarketGuide.ForeColor = ColorSuccessText

        Else
            'NOT FOUND = RED
            pnlMarketGuideSection.FillColor = ColorDangerBackground
            pnlMarketGuideSection.BackColor = ColorDangerBackground
            pnlMarketGuideSection.BorderColor = ColorDangerBorder

            rtbMarketGuide.BackColor = ColorDangerBackground
            rtbMarketGuide.ForeColor = ColorDangerText

            lblLookupMarketGuide.ForeColor = ColorDangerText

        End If

    End Sub
    Private Sub ApplyPALVisualState()

        If _currentLookup Is Nothing Then
            SetPALWaitingState()
            Return
        End If

        If _currentLookup.PalFound Then
            'PAL FOUND = RED / ATTENTION
            pnlPALSection.FillColor = ColorDangerBackground
            pnlPALSection.BackColor = ColorDangerBackground
            pnlPALSection.BorderColor = ColorDangerBorder

            rtbPAL.BackColor = ColorDangerBackground
            rtbPAL.ForeColor = ColorDangerText

            lblLookupPAL.ForeColor = ColorDangerText

        Else
            'NO PAL = GREEN
            pnlPALSection.FillColor = ColorSuccessBackground
            pnlPALSection.BackColor = ColorSuccessBackground
            pnlPALSection.BorderColor = ColorSuccessBorder

            rtbPAL.BackColor = ColorSuccessBackground
            rtbPAL.ForeColor = ColorSuccessText

            lblLookupPAL.ForeColor = ColorSuccessText

        End If
    End Sub
    Private Function NormalizeProductForLookup(product As String) As String
        If String.IsNullOrWhiteSpace(product) Then
            Return String.Empty
        End If

        Dim value As String = product.Trim().ToUpperInvariant()
        Select Case value
            Case "MER"
                Return "Medicare HMO"

            Case Else
                Return product.Trim()

        End Select
    End Function
    Private Sub ShowMemberInformationPanel()
        pnlMemberCard.Visible = True
        pnlAuthCard.Visible = False
        pnlMemberCard.BringToFront()
    End Sub
    Private Sub ShowAuthorizationInformationPanel()
        pnlMemberCard.Visible = False
        pnlAuthCard.Visible = True
        pnlAuthCard.BringToFront()
    End Sub
    Private Sub Verification_CheckedChanged(sender As Object, e As EventArgs) Handles chkGenesysVerified.CheckedChanged, chkProviderAuthenticated.CheckedChanged, chkMailingAddressVerified.CheckedChanged
        If _currentContext Is Nothing Then
            Return
        End If
        SyncVerificationToContext()
        'Update context from the current checkbox states
        _currentContext.GenesysVerified = chkGenesysVerified.Checked
        _currentContext.ProviderMemberAuthenticated = chkProviderAuthenticated.Checked
        _currentContext.MailingAddressVerified = chkMailingAddressVerified.Checked
        'Immediately refresh documentation
        txtOverAllOutput.Text = OutputFormatter.BuildDocumentation(_currentContext)
    End Sub
    Private Sub SyncVerificationToContext()
        If _currentContext Is Nothing Then
            Return
        End If
        _currentContext.GenesysVerified = chkGenesysVerified.Checked
        _currentContext.ProviderMemberAuthenticated = chkProviderAuthenticated.Checked
        _currentContext.MailingAddressVerified = chkMailingAddressVerified.Checked
    End Sub

    Private Sub rtbOutOfScope_MouseUp(sender As Object, e As MouseEventArgs) Handles rtbOutOfScope.MouseUp
        Dim charIndex As Integer = rtbOutOfScope.GetCharIndexFromPosition(e.Location)
        For Each link In _oosLinks
            Dim startIndex As Integer = rtbOutOfScope.Text.IndexOf(link.Key, StringComparison.Ordinal)
            If startIndex < 0 Then
                Continue For
            End If

            Dim endIndex As Integer = startIndex + link.Key.Length
            If charIndex >= startIndex And charIndex <= endIndex Then
                OpenExternalLink(link.Value)
                Exit Sub
            End If
        Next
    End Sub
    Private Sub OpenExternalLink(url As String)
        Try
            Process.Start(New ProcessStartInfo With {.FileName = url, .UseShellExecute = True})
        Catch ex As Exception
            MessageBox.Show(
                "Unable to open the link." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Open Link",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
        End Try

    End Sub
    Private Function GetDigitCount(value As String) As Integer
        If String.IsNullOrWhiteSpace(value) Then
            Return 0
        End If

        Return value.Count(Function(c) Char.IsDigit(c))
    End Function
    Private Sub ValidatePhoneAndFax()
        ValidateTenDigitField(txtCallbackNum)
        ValidateTenDigitField(txtSecuredFax)
    End Sub
    Private Sub ValidateTenDigitField(textBox As Guna.UI2.WinForms.Guna2TextBox)
        Dim digitCount As Integer = GetDigitCount(textBox.Text)
        'Blank is allowed.
        If digitCount = 0 Then
            textBox.BorderColor = ColorPrimaryGreen
            textBox.FocusedState.BorderColor = ColorPrimaryGreen
            Return
        End If
        'Entered but less than 10 digits = invalid.
        If digitCount < 10 Then
            textBox.BorderColor = Color.FromArgb(220, 53, 69)
            textBox.FocusedState.BorderColor = Color.FromArgb(220, 53, 69)
        Else
            textBox.BorderColor = ColorPrimaryGreen
            textBox.FocusedState.BorderColor = ColorPrimaryGreen
        End If
    End Sub
    Private Sub PhoneFax_TextChanged(sender As Object, e As EventArgs) Handles txtCallbackNum.TextChanged, txtSecuredFax.TextChanged
        Dim textBox = TryCast(sender, Guna.UI2.WinForms.Guna2TextBox)
        If textBox Is Nothing Then
            Return
        End If
        ValidateTenDigitField(textBox)
    End Sub
End Class
