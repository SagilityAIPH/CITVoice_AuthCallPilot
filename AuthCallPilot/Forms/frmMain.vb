Imports MaterialSkin
Imports MaterialSkin.Controls

Public Class frmMain
    Inherits MaterialForm
    Private Session As WorkflowSession
    Private Controller As WorkflowController
    Private _scrollToStep As Integer = 0
    Dim section As New ucWorkflowSection()


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

        Dim root As ChecklistNode = WorkflowEngine.InitializeCallPilotTree()
        Session = New WorkflowSession()
        Controller = New WorkflowController(Session)
        Session.Root = root
        Session.Current = root
        Session.Path.Add(New WorkflowNodeState With {
            .Node = root
        })

        'RenderWorkflow()
    End Sub
    Private Sub frmMain_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        'working
        RenderWorkflow()

        'Testing
        'pnlWorkflow.Controls.Clear()
        'Dim section As New ucWorkflowSection()
        'section.Dock = DockStyle.Top
        'Dim workflowStep As New ucWorkflowStep()
        'workflowStep.LoadNode(Session.Root)
        'section.AddWorkflowStep(workflowStep)
        'pnlWorkflow.Controls.Add(section)
    End Sub
    Private Sub RenderWorkflow()

        pnlWorkflow.SuspendLayout()
        pnlWorkflow.Controls.Clear()

        section = New ucWorkflowSection()
        section.SectionTitle = "Authentication"
        section.Dock = DockStyle.Top

        Dim stepNumber As Integer = 1

        For Each state In Session.Path
            Dim workflowStep As New ucWorkflowStep()
            workflowStep.StepNumber = stepNumber
            workflowStep.LoadNode(state.Node)
            If state.Answer.HasValue Then
                workflowStep.Answer = state.Answer
            End If
            AddHandler workflowStep.AnswerSelected, AddressOf StepAnswered
            section.AddWorkflowStep(workflowStep)
            stepNumber += 1

        Next
        pnlWorkflow.Controls.Add(section)
        pnlWorkflow.ResumeLayout()

    End Sub
    Private Sub StepAnswered(stepControl As ucWorkflowStep)
        Dim state As WorkflowNodeState = Controller.GetState(stepControl.CurrentNode)
        If state Is Nothing Then Exit Sub
        state.Answer = stepControl.Answer
        _scrollToStep = Controller.AdvanceWorkflow(state)
        RenderWorkflow()
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
