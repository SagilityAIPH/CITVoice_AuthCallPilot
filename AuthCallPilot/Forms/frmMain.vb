Imports System.Linq
Imports MaterialSkin
Imports MaterialSkin.Controls
Imports OpenQA.Selenium
Imports System.Threading.Tasks
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
    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        _browserMonitorTimer.Stop()
        BrowserManager.Close()
    End Sub
    Private Async Sub BrowserMonitorTimer_Tick(sender As Object, e As EventArgs) Handles _browserMonitorTimer.Tick
        If _browserMonitorBusy Then
            Exit Sub
        End If

        If Not BrowserManager.IsBrowserAvailable() Then
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
                        Exit Sub
                    End If

                    ProcessDetectedMember(captured.Context)
                    _lastProcessedUrl = currentUrl
                    _lastProcessedTitle = currentTitle

                Case CgxPageType.ViewAuthorization
                    If captured.Context Is Nothing Then
                        Exit Sub
                    End If

                    ProcessDetectedAuthorization(captured.Context)
                    _lastProcessedUrl = currentUrl
                    _lastProcessedTitle = currentTitle

                Case CgxPageType.Other
                    _lastProcessedUrl = currentUrl
                    _lastProcessedTitle = currentTitle

            End Select
        Catch ex As OpenQA.Selenium.WebDriverException
            Debug.WriteLine("CGX listener error: " & ex.Message)

        Catch ex As Exception
            Debug.WriteLine("CGX listener error: " & ex.ToString())
        Finally
            _browserMonitorBusy = False
        End Try
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

        Me.Text = "CallPilot V1.0"

        Dim skinManager As MaterialSkinManager = MaterialSkinManager.Instance
        skinManager.AddFormToManage(Me)

        skinManager.Theme = MaterialSkinManager.Themes.LIGHT
        skinManager.ColorScheme = New ColorScheme(
            Primary.BlueGrey800,
            Primary.BlueGrey900,
            Primary.BlueGrey500,
            Accent.LightBlue200,
            TextShade.WHITE
        )

        cmbScenario.Items.Clear()
        cmbScenario.Items.Add("NEW AUTHORIZATION")
        cmbScenario.Items.Add("UPDATING AUTHORIZATION")
        cmbScenario.Items.Add("CHECKING STATUS OF THE AUTHORIZATION")
        cmbScenario.SelectedIndex = -1

        txtOverAllOutput.Clear()
        txtNextBestAction.Clear()

        pnlActions.Controls.Clear()
        pnlActions.Visible = False

        txtMemberInfo.ReadOnly = True
        txtAuthInfo.ReadOnly = True
        txtOutOfScope.ReadOnly = True
        txtMarketGuide.ReadOnly = True
        txtPAL.ReadOnly = True
        txtNextBestAction.ReadOnly = True
        txtOverAllOutput.ReadOnly = False
    End Sub
    Private Sub frmMain_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        'working
        'RenderWorkflow()

        'tsting
        'RenderAllWorkflowSections()

    End Sub

    Private Sub btnLaunchBrowser_Click(sender As Object, e As EventArgs) Handles btnLaunchBrowser.Click
        Try
            BrowserManager.Launch()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Browser Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Async Sub btnAnalyze_Click(sender As Object, e As EventArgs) Handles btnAnalyze.Click
        If Not BrowserManager.IsBrowserAvailable() Then
            MessageBox.Show("Launch the CGX browser first.", "Browser Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not ValidateCgxSearchInput() Then
            Exit Sub
        End If

        Try
            btnAnalyze.Enabled = False
            btnAnalyze.Text = "Refreshing..."

            _lastProcessedUrl = String.Empty
            _lastProcessedTitle = String.Empty

            Dim captured As BrowserCaptureResult =
                Await Task.Run(
                    Function()
                        Return BrowserManager.CaptureCurrentCgxPage()
                    End Function)

            If captured Is Nothing Or captured.Context Is Nothing Then
                MessageBox.Show("Navigate to the Member Information or View Authorization page.", "Refresh CGX", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            Select Case captured.PageType
                Case CgxPageType.MemberInformation
                    ProcessDetectedMember(captured.Context)
                Case CgxPageType.ViewAuthorization
                    ProcessDetectedAuthorization(captured.Context)
                Case Else
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
            MessageBox.Show(ex.ToString(), "Refresh CGX Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnAnalyze.Enabled = True
            btnAnalyze.Text = "Refresh CGX"
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

        txtNextBestAction.Text = result.NextBestAction
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
        .ForeColor = Color.FromArgb(45, 45, 45),
        .BackColor = Color.Transparent,
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
            .ForeColor = Color.FromArgb(55, 55, 55),
            .Cursor = Cursors.Hand
        }

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

            Case "CARE_SETTING"
                _currentContext.CareSetting = selectedValue
                _currentContext.IsExpedited = Nothing
                _currentContext.CallerType = Nothing

            Case "EXPEDITED_REQUEST"
                _currentContext.IsExpedited = String.Equals(selectedValue, "YES", StringComparison.OrdinalIgnoreCase)
                _currentContext.CallerType = Nothing

            Case "CALLER_TYPE"
                _currentContext.CallerType = selectedValue
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
            Case "CARE_SETTING"
                _currentContext.CareSetting = Nothing
                _currentContext.IsExpedited = Nothing
                _currentContext.CallerType = Nothing
            Case "EXPEDITED_REQUEST"
                _currentContext.IsExpedited = Nothing
                _currentContext.CallerType = Nothing
            Case "CALLER_TYPE"
                _currentContext.CallerType = Nothing
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
        output.AppendLine("Genesys Verification: Yes/No")
        output.AppendLine("Provider/Member Authenticated: Yes/No")
        output.AppendLine("Mailing Address Verified: Yes/No")
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
    Private Sub btnSelectScenario_Click(sender As Object, e As EventArgs) Handles btnSelectScenario.Click
        If Not ValidateCgxSearchInput() Then
            Exit Sub
        End If
        Try
            If cmbScenario.SelectedIndex < 0 Then
                MessageBox.Show("Select a scenario first.", "Scenario Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If _currentContext Is Nothing Or String.IsNullOrWhiteSpace(_currentContext.MemberId) Then
                MessageBox.Show("Navigate to the CGX Member Information page first.", "Member Information Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            _currentContext.CallerName = txtCallerName.Text.Trim()
            _currentContext.CallbackNumber = txtCallbackNum.Text.Trim()
            _currentContext.SecuredFax = txtSecuredFax.Text.Trim()
            _currentContext.CallingFrom = txtCallingFrom.Text.Trim()
            _currentContext.DateOfService = ParseOptionalDos(txtDOS.Text)
            ClearScenarioDecisionState()
            _currentContext.Scenario = Convert.ToString(cmbScenario.SelectedItem)
            RunRecommendation()
        Catch ex As Exception
            MessageBox.Show(ex.ToString(), "Scenario Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub ClearScenarioDecisionState()
        _questionHistory.Clear()
        _currentQuestionId = Nothing
        _currentQuestionText = Nothing
        ClearActionsPanel()
        If _currentContext Is Nothing Then Exit Sub
        _currentContext.HealthType = Nothing
        _currentContext.CareSetting = Nothing
        _currentContext.IsExpedited = Nothing
        _currentContext.CallerType = Nothing
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
        'Member information from CGX
        _currentContext.MemberId = detected.MemberId
        _currentContext.MemberName = detected.MemberName
        _currentContext.DateOfBirth = detected.DateOfBirth
        _currentContext.Product = detected.Product
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
        txtMemberInfo.Text = OutputFormatter.BuildMemberInformation(_currentContext)
        'Populate member-level lookup results.
        txtOutOfScope.Text = OutputFormatter.BuildOutOfScope(_currentLookup)
        txtMarketGuide.Text = OutputFormatter.BuildMarketGuide(_currentLookup)

        'Authorization and PAL stay completely blank.
        txtAuthInfo.Clear()
        txtPAL.Clear()

        'Documentation only.
        txtOverAllOutput.Text = OutputFormatter.BuildDocumentation(_currentContext)
        txtNextBestAction.Text = "Member information refreshed. Open an authorization in CGX or select a scenario when ready."

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
        _currentContext.RequestingProvider = detected.RequestingProvider
        _currentContext.TreatingProvider = detected.TreatingProvider
        _currentContext.FacilityProvider = detected.FacilityProvider
        _currentContext.AuthorizationStartDate = detected.AuthorizationStartDate
        _currentContext.AuthorizationEndDate = detected.AuthorizationEndDate
        _currentContext.TotalDays = detected.TotalDays
        _currentContext.PrimaryDiagnosisCode = detected.PrimaryDiagnosisCode
        _currentContext.ClaimPaymentNotes = detected.ClaimPaymentNotes
        _currentContext.SecondaryDiagnosisCodes.Clear()

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
        txtNextBestAction.Text = "Authorization information detected. Select a scenario and click Select."
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
            txtMemberInfo.Clear()
            txtAuthInfo.Clear()
            txtOverAllOutput.Clear()
        Else
            txtMemberInfo.Text = OutputFormatter.BuildMemberInformation(_currentContext)
            txtAuthInfo.Text = OutputFormatter.BuildAuthorizationInformation(_currentContext)
            txtOverAllOutput.Text = OutputFormatter.BuildDocumentation(_currentContext)
        End If

        If _currentLookup Is Nothing Then
            txtOutOfScope.Clear()
            txtMarketGuide.Clear()
            txtPAL.Clear()
        Else
            txtOutOfScope.Text = OutputFormatter.BuildOutOfScope(_currentLookup)
            txtMarketGuide.Text = OutputFormatter.BuildMarketGuide(_currentLookup)
            txtPAL.Text = OutputFormatter.BuildPal(_currentContext, _currentLookup)
        End If
    End Sub
End Class
