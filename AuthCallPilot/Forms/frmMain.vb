Imports MaterialSkin
Imports MaterialSkin.Controls

Public Class frmMain
    Inherits MaterialForm
    Private Session As WorkflowSession
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
        Session.Root = root
        Session.Current = root
        Session.Path.Add(New WorkflowNodeState With {
            .Node = root
        })

        'RenderWorkflow()
    End Sub
    Private Sub frmMain_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        'BeginInvoke(New MethodInvoker(AddressOf RenderWorkflow))
        RenderWorkflow()
    End Sub
    Private Sub RenderWorkflow()
        pnlWorkflow.SuspendLayout()
        pnlWorkflow.Controls.Clear()

        Dim y As Integer = 10
        Dim stepNumber As Integer = 1

        For Each state In Session.Path
            Dim ctrl As New ucWorkflowStep()
            ctrl.StepNumber = stepNumber
            ctrl.Width = pnlWorkflow.ClientSize.Width - 20
            ctrl.LoadNode(state.Node)
            ctrl.Left = 10
            ctrl.Top = y

            AddHandler ctrl.AnswerSelected, AddressOf StepAnswered
            pnlWorkflow.Controls.Add(ctrl)
            y += ctrl.Height + 10
            stepNumber += 1
        Next
        pnlWorkflow.ResumeLayout(True)

    End Sub
    Private Sub StepAnswered(workflowStep As ucWorkflowStep, answer As Boolean)
        MessageBox.Show(answer.ToString())
        'MessageBox.Show("Question: " & workflowStep.CurrentNode.Question & vbCrLf & "Answer: " & answer.ToString())
    End Sub
    Private Sub frmMain_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Dim y As Integer = 10
        For Each ctrl As ucWorkflowStep In pnlWorkflow.Controls.OfType(Of ucWorkflowStep)()
            ctrl.Width = pnlWorkflow.ClientSize.Width - 25
            ctrl.Top = y
            y += ctrl.Height + 10
        Next
    End Sub
End Class
