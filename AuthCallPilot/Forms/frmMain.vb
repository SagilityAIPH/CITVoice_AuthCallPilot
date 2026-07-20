Imports MaterialSkin
Imports MaterialSkin.Controls

Public Class frmMain
    Inherits MaterialForm
    Private Session As WorkflowSession
    Private _scrollToStep As Integer = 0
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
        pnlWorkflow.AutoScrollPosition = New Point(0, 0)

        Dim y As Integer = 10
        Dim stepNumber As Integer = 1
        Dim availableWidth As Integer =
        pnlWorkflow.ClientSize.Width -
        pnlWorkflow.Padding.Left -
        pnlWorkflow.Padding.Right

        For Each state In Session.Path
            Dim ctrl As New ucWorkflowStep()
            'MessageBox.Show("1. New Control = " & ctrl.Width)
            ctrl.StepNumber = stepNumber
            'ctrl.Width = pnlWorkflow.ClientSize.Width - 20
            'ctrl.Width = pnlWorkflow.ClientSize.Width _
            ' - pnlWorkflow.Padding.Left _
            ' - pnlWorkflow.Padding.Right _
            ' - 2
            'MessageBox.Show("2. After Width = " & ctrl.Width)

            ctrl.LoadNode(state.Node)
            If state.Answer.HasValue Then
                ctrl.Answer = state.Answer
            End If
            'MessageBox.Show("3. After LoadNode = " & ctrl.Width)

            ctrl.Left = 10
            ctrl.Top = y

            AddHandler ctrl.AnswerSelected, AddressOf StepAnswered
            pnlWorkflow.Controls.Add(ctrl)
            ctrl.Width = availableWidth
            'MessageBox.Show("After Reassign = " & ctrl.Width)

            'MessageBox.Show("4. After Add = " & ctrl.Width)

            y += ctrl.Height + 4
            stepNumber += 1
        Next
        pnlWorkflow.AutoScrollMinSize = New Size(0, y)
        pnlWorkflow.ResumeLayout(True)

        If _scrollToStep > 0 Then
            For Each ctrl As ucWorkflowStep In pnlWorkflow.Controls.OfType(Of ucWorkflowStep)()
                If ctrl.StepNumber = _scrollToStep Then
                    pnlWorkflow.ScrollControlIntoView(ctrl)
                    Exit For
                End If
            Next
        End If

    End Sub
    Private Sub StepAnswered(stepControl As ucWorkflowStep)
        'MessageBox.Show(stepControl.Answer.ToString())
        Dim state As WorkflowNodeState = Session.Path.FirstOrDefault(Function(x) x.Node Is stepControl.CurrentNode)

        If state Is Nothing Then Exit Sub
        state.Answer = stepControl.Answer
        Dim index As Integer = Session.Path.IndexOf(state)
        While Session.Path.Count > index + 1
            Session.Path.RemoveAt(Session.Path.Count - 1)
        End While

        Dim nextNode As ChecklistNode = Nothing
        If state.Answer.Value Then
            nextNode = state.Node.YesNode
        Else
            nextNode = state.Node.NoNode
        End If

        If nextNode IsNot Nothing Then
            Session.Path.Add(New WorkflowNodeState With {.Node = nextNode})
        End If

        _scrollToStep = index + 2
        RenderWorkflow()
    End Sub
    Private Sub frmMain_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Dim y As Integer = 10
        Dim availableWidth As Integer =
        pnlWorkflow.ClientSize.Width -
        pnlWorkflow.Padding.Left -
        pnlWorkflow.Padding.Right - 2

        For Each ctrl As ucWorkflowStep In pnlWorkflow.Controls.OfType(Of ucWorkflowStep)()
            ctrl.Width = availableWidth
            ctrl.Left = pnlWorkflow.Padding.Left
            ctrl.Top = y

            y += ctrl.Height + 4
        Next
    End Sub

End Class
