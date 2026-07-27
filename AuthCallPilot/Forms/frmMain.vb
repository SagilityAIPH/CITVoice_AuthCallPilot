Imports MaterialSkin
Imports MaterialSkin.Controls

Public Class frmMain
    Inherits MaterialForm
    'Private Session As WorkflowSession
    'Private Controller As WorkflowController
    'Private _scrollToStep As Integer = 0
    'Dim section As New ucWorkflowSection()
    Private _authenticationSession As WorkflowSession
    Private _outOfScopeSession As WorkflowSession
    Private _providerTriageSession As WorkflowSession

    Private _authenticationController As WorkflowController
    Private _outOfScopeController As WorkflowController
    Private _providerTriageController As WorkflowController

    Private _authenticationSection As ucWorkflowSection
    Private _outOfScopeSection As ucWorkflowSection
    Private _providerTriageSection As ucWorkflowSection
    Private Function CreateSession(rootNode As ChecklistNode) As WorkflowSession
        If rootNode Is Nothing Then
            Throw New ArgumentNullException(NameOf(rootNode))
        End If

        Dim workflowSession As New WorkflowSession With {
        .Root = rootNode,
        .Current = rootNode
    }

        workflowSession.Path.Add(New WorkflowNodeState With {
        .Node = rootNode,
        .StepNumber = 1
    })

        Return workflowSession
    End Function

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

        Dim authenticationRoot As ChecklistNode = AuthenticationWorkflow.CreateWorkflow()
        Dim outOfScopeRoot As ChecklistNode = CheckIfOutOfScopeWorkflow.CreateWorkflow()
        Dim providerTriageRoot As ChecklistNode = ProviderTriageWorkflow.CreateWorkflow()

        _authenticationSession = CreateSession(authenticationRoot)
        _outOfScopeSession = CreateSession(outOfScopeRoot)
        _providerTriageSession = CreateSession(providerTriageRoot)

        _authenticationController = New WorkflowController(_authenticationSession)
        _outOfScopeController = New WorkflowController(_outOfScopeSession)
        _providerTriageController = New WorkflowController(_providerTriageSession)
    End Sub
    Private Sub frmMain_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        'working
        'RenderWorkflow()

        'tsting
        RenderAllWorkflowSections()

    End Sub
    Private Sub RenderAllWorkflowSections()

        pnlWorkflow.SuspendLayout()
        pnlWorkflow.Controls.Clear()
        pnlWorkflow.AutoScrollPosition = Point.Empty

        _authenticationSection = CreateWorkflowSection("Authentication", _authenticationSession, AddressOf AuthenticationStepAnswered)
        _outOfScopeSection = CreateWorkflowSection("Check If Out Of Scope", _outOfScopeSession, AddressOf OutOfScopeStepAnswered)
        _providerTriageSection = CreateWorkflowSection("Provider Triage", _providerTriageSession, AddressOf ProviderTriageStepAnswered)

        _authenticationSection.Expanded = True
        _outOfScopeSection.Expanded = False
        _providerTriageSection.Expanded = False

        'DockStyle.Top stacking is reverse-order sensitive.
        pnlWorkflow.Controls.Add(_providerTriageSection)
        pnlWorkflow.Controls.Add(_outOfScopeSection)
        pnlWorkflow.Controls.Add(_authenticationSection)

        pnlWorkflow.ResumeLayout(True)

    End Sub
    Private Function CreateWorkflowSection(title As String, workflowSession As WorkflowSession, answerHandler As Action(Of ucWorkflowStep)) As ucWorkflowSection

        Dim newSection As New ucWorkflowSection With {
        .SectionTitle = title,
        .Dock = DockStyle.Top,
        .Margin = New Padding(0, 0, 0, 8)
    }

        Dim stepNumber As Integer = 1

        For Each state As WorkflowNodeState In workflowSession.Path

            Dim stepControl As New ucWorkflowStep With {
            .StepNumber = stepNumber,
            .SelectedResponse = state.SelectedResponse
        }

            stepControl.LoadNode(state.Node)

            If state.Answer.HasValue Then
                stepControl.Answer = state.Answer
            End If

            AddHandler stepControl.AnswerSelected,
            Sub(senderStep As ucWorkflowStep)
                answerHandler(senderStep)
            End Sub

            newSection.AddWorkflowStep(stepControl)

            stepNumber += 1

        Next

        Return newSection

    End Function
    Private Sub AuthenticationStepAnswered(stepControl As ucWorkflowStep)
        HandleWorkflowAnswer(stepControl, _authenticationController, _authenticationSession, _authenticationSection, AddressOf AuthenticationStepAnswered)
    End Sub
    Private Sub OutOfScopeStepAnswered(stepControl As ucWorkflowStep)
        HandleWorkflowAnswer(stepControl, _outOfScopeController, _outOfScopeSession, _outOfScopeSection, AddressOf OutOfScopeStepAnswered)
    End Sub
    Private Sub ProviderTriageStepAnswered(stepControl As ucWorkflowStep)
        HandleWorkflowAnswer(stepControl, _providerTriageController, _providerTriageSession, _providerTriageSection, AddressOf ProviderTriageStepAnswered)
    End Sub
    Private Sub HandleWorkflowAnswer(stepControl As ucWorkflowStep, controller As WorkflowController, workflowSession As WorkflowSession, workflowSection As ucWorkflowSection, answerHandler As Action(Of ucWorkflowStep)
)
        Dim state As WorkflowNodeState = controller.GetState(stepControl.CurrentNode)
        If state Is Nothing Then Exit Sub

        state.Answer = stepControl.Answer
        state.SelectedResponse = stepControl.SelectedResponse

        controller.AdvanceWorkflow(state)
        RefreshWorkflowSection(workflowSection, workflowSession, answerHandler)
    End Sub
    Private Sub RefreshWorkflowSection(workflowSection As ucWorkflowSection, workflowSession As WorkflowSession, answerHandler As Action(Of ucWorkflowStep)
)

        If workflowSection Is Nothing Then Exit Sub
        If workflowSession Is Nothing Then Exit Sub

        Dim wasExpanded As Boolean = workflowSection.Expanded

        workflowSection.SuspendLayout()
        workflowSection.ClearWorkflowSteps()

        Dim stepNumber As Integer = 1
        Dim newestStep As ucWorkflowStep = Nothing

        For Each state As WorkflowNodeState In workflowSession.Path

            Dim stepControl As New ucWorkflowStep With {
            .StepNumber = stepNumber,
            .SelectedResponse = state.SelectedResponse
        }

            stepControl.LoadNode(state.Node)

            If state.Answer.HasValue Then
                stepControl.Answer = state.Answer
            End If

            AddHandler stepControl.AnswerSelected,
            Sub(senderStep As ucWorkflowStep)
                answerHandler(senderStep)
            End Sub

            workflowSection.AddWorkflowStep(stepControl)

            newestStep = stepControl
            stepNumber += 1

        Next

        workflowSection.Expanded = wasExpanded
        workflowSection.ResumeLayout(True)

        If IsHandleCreated Then

            BeginInvoke(
        New MethodInvoker(
            Sub()

                workflowSection.PerformLayout()
                workflowSection.ResizeWorkflowSteps()

                If newestStep IsNot Nothing AndAlso wasExpanded Then
                    pnlWorkflow.ScrollControlIntoView(newestStep)
                End If

            End Sub))

        End If

    End Sub
    Private Sub btnLaunchBrowser_Click(sender As Object, e As EventArgs) Handles btnLaunchBrowser.Click
        Try
            BrowserManager.Launch()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Browser Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    'Private Sub frmMain_Resize(sender As Object, e As EventArgs) Handles Me.Resize
    '    Dim y As Integer = 10
    '    Dim availableWidth As Integer =
    '    pnlWorkflow.ClientSize.Width -
    '    pnlWorkflow.Padding.Left -
    '    pnlWorkflow.Padding.Right - 2

    '    For Each ctrl As ucWorkflowStep In pnlWorkflow.Controls.OfType(Of ucWorkflowStep)()
    '        ctrl.Width = availableWidth
    '        ctrl.Left = pnlWorkflow.Padding.Left
    '        ctrl.Top = y

    '        y += ctrl.Height + 4
    '    Next
    'End Sub

End Class
