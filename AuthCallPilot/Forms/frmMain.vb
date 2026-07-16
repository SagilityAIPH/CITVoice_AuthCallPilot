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

        RenderWorkflow()
    End Sub
    Private Sub RenderWorkflow()
        flowWorkflow.Controls.Clear()
        Dim stepNumber As Integer = 1
        For Each state In Session.Path
            Dim ctrl As New ucWorkflowStep()
            ctrl.LoadStep(state.Node)
            ctrl.StepNumber = stepNumber
            AddHandler ctrl.AnswerSelected, AddressOf StepAnswered
            flowWorkflow.Controls.Add(ctrl)
            stepNumber += 1
        Next
    End Sub
    Private Sub StepAnswered(workflowStep As ucWorkflowStep, answer As Boolean)
        MessageBox.Show("Question: " & workflowStep.CurrentNode.Question & vbCrLf & "Answer: " & answer.ToString())
    End Sub
End Class
