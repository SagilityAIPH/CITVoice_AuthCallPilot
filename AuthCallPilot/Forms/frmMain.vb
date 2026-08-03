Imports MaterialSkin
Imports MaterialSkin.Controls
Imports System.Linq
Public Class frmMain
    Private _currentContext As CallContext
    Private _currentLookup As LookupResult
    Private _currentQuestionId As String

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        'txtOverAllOutput.ReadOnly = True
        txtNextBestAction.ReadOnly = True
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

    Private Sub btnAnalyze_Click(sender As Object, e As EventArgs) Handles btnAnalyze.Click
        Try
            _currentContext = BuildCallContextFromUI()

            'Temporary test values.
            'Later these will come from Selenium/CGX.
            _currentContext.Product = "Medicare HMO"
            _currentContext.Conso = "CONSOL - FLORIDA"
            _currentContext.IssueState = "FL"
            _currentContext.GroupNumber = "333569"
            _currentContext.HealthType = "PHYSICAL HEALTH"
            _currentContext.CareSetting = "INPATIENT"
            _currentContext.AuthorizationStatus = "PENDING"

            _currentContext.ProcedureCodes.Clear()
            _currentContext.ProcedureCodes.Add("11971")

            _currentLookup =
                CallPilotRepository.RunLookups(
                    _currentContext)

            RunRecommendation()

        Catch ex As Exception

            MessageBox.Show(
                ex.ToString(),
                "Analyze Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

        End Try
    End Sub
    Private Function BuildCallContextFromUI() As CallContext

        Dim dob As DateTime
        Dim parsedDob As DateTime? = Nothing

        If DateTime.TryParse(txtDOB.Text.Trim(), dob) Then
            parsedDob = dob
        End If

        Return New CallContext With {
            .CallerFirstName = txtCallerFirstName.Text.Trim(),
            .CallerLastName = txtCallerLastName.Text.Trim(),
            .MemberId = txtMemberId.Text.Trim(),
            .DateOfBirth = parsedDob,
            .Scenario = Convert.ToString(cmbScenario.SelectedItem)
        }
    End Function
    Private Sub RunRecommendation()

        If _currentContext Is Nothing Then Exit Sub
        If _currentLookup Is Nothing Then Exit Sub

        Dim result As RecommendationResult = RecommendationEngine.Analyze(_currentContext, _currentLookup)

        txtOverAllOutput.Text = result.OverallOutput
        txtNextBestAction.Text = result.NextBestAction
        If result.RequiresAgentInput Then
            ShowActionQuestion(result.QuestionId, result.QuestionText, result.QuestionOptions)
        Else
            ClearActionsPanel()
        End If
    End Sub
    Private Sub ShowActionQuestion(questionId As String, questionText As String, options As IEnumerable(Of String))
        ClearActionsPanel()
        _currentQuestionId = questionId
        pnlActions.SuspendLayout()

        Dim questionLabel As New Label With {
            .Text = questionText,
            .AutoSize = False,
            .Font = New Font("Segoe UI", 10.0!, FontStyle.Bold),
            .Left = 12,
            .Top = 10,
            .Width = Math.Max(150, pnlActions.ClientSize.Width - 24),
            .Height = 32
        }

        pnlActions.Controls.Add(questionLabel)

        Dim y As Integer =
            questionLabel.Bottom + 5

        For Each optionText As String In options

            Dim radio As New Guna.UI2.WinForms.Guna2RadioButton With
            {
            .Text = optionText,
            .Tag = optionText,
            .AutoSize = True,
            .Left = 16,
            .Top = y,
            .Font = New Font(
                "Segoe UI",
                9.5!,
                FontStyle.Regular)
        }

            AddHandler radio.CheckedChanged, AddressOf DynamicAction_CheckedChanged
            pnlActions.Controls.Add(radio)
            y += radio.Height + 8

        Next

        pnlActions.Height = y + 10
        pnlActions.Visible = True
        pnlActions.ResumeLayout(True)

    End Sub
    Private Sub DynamicAction_CheckedChanged(sender As Object, e As EventArgs)

        Dim radio As Guna.UI2.WinForms.Guna2RadioButton = TryCast(sender, Guna.UI2.WinForms.Guna2RadioButton)

        If radio Is Nothing OrElse Not radio.Checked Then
            Exit Sub
        End If

        Dim selectedValue As String = Convert.ToString(radio.Tag)

        Select Case _currentQuestionId
            Case "EXPEDITED_REQUEST"
                _currentContext.IsExpedited = String.Equals(selectedValue, "YES", StringComparison.OrdinalIgnoreCase)

            Case "CALLER_TYPE"
                _currentContext.CallerType = selectedValue

        End Select
        RunRecommendation()

    End Sub
    Private Sub ClearActionsPanel()
        pnlActions.SuspendLayout()

        For Each control As Control In pnlActions.Controls.Cast(Of Control)().ToList()
            pnlActions.Controls.Remove(control)
            control.Dispose()
        Next

        pnlActions.Visible = False
        _currentQuestionId = Nothing
        pnlActions.ResumeLayout(True)

    End Sub
End Class
