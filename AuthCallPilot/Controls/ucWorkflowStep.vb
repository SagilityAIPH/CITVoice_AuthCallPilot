Public Class ucWorkflowStep
    Private _loading As Boolean = False
    Public Sub New()
        InitializeComponent()
    End Sub
    Private Sub ucWorkflowStep_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        btnStepNumber.Location = New Point(15, 15)

        tgAnswer.Location = New Point(Me.Width - tgAnswer.Width - 20, 18)

        lblQuestion.Location = New Point(70, 18)
        lblQuestion.Width = tgAnswer.Left - lblQuestion.Left - 20

        flowInstructions.Location = New Point(70, 55)

    End Sub
    Public Event AnswerSelected(sender As ucWorkflowStep, answer As Boolean)
    Public Property CurrentNode As ChecklistNode
    Public Sub LoadNode(node As ChecklistNode)
        _loading = True
        CurrentNode = node
        lblQuestion.Text = node.Question
        LoadInstructions(node)
        'Me.Height = Math.Max(95, 60 + flowInstructions.PreferredSize.Height)
        flowInstructions.PerformLayout()
        Dim cardHeight As Integer =
        flowInstructions.Bottom + 15

        Dim requiredHeight As Integer =
        flowInstructions.Bottom + 15

        If requiredHeight < 100 Then
            requiredHeight = 100
        End If

        cardStep.Height = requiredHeight
        Me.Height = cardStep.Height
        _loading = False
    End Sub
    Private Sub LoadInstructions(node As ChecklistNode)

        flowInstructions.SuspendLayout()
        flowInstructions.Controls.Clear()

        For Each instruction As WorkflowInstruction In node.Instructions

            Dim lbl As New Guna.UI2.WinForms.Guna2HtmlLabel()

            lbl.AutoSize = True
            lbl.BackColor = Color.Transparent
            lbl.Font = New Font("Segoe UI", 9.5!, FontStyle.Regular)
            lbl.ForeColor = Color.FromArgb(70, 70, 70)
            lbl.Margin = New Padding(0, 0, 0, 6)

            lbl.Text = "• " & instruction.Text

            flowInstructions.Controls.Add(lbl)

        Next

        flowInstructions.ResumeLayout()
        flowInstructions.ResumeLayout(True)

    End Sub
    Public Property StepNumber As Integer
        Get
            Return CInt(btnStepNumber.Text)
        End Get

        Set(value As Integer)
            btnStepNumber.Text = value.ToString()
        End Set
    End Property
    Private _answer As Boolean?
    Public Property Answer As Boolean?
        Get
            Return _answer
        End Get

        Set(value As Boolean?)
            _answer = value
            If value.HasValue Then
                tgAnswer.Checked = value.Value
            End If
        End Set
    End Property

    Private Sub tgAnswer_CheckedChanged_1(sender As Object, e As EventArgs) Handles tgAnswer.CheckedChanged
        If _loading Then Exit Sub
        Answer = tgAnswer.Checked
        RaiseEvent AnswerSelected(Me, Answer.Value)
    End Sub
End Class
