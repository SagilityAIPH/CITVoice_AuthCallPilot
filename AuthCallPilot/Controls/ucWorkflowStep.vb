Public Class ucWorkflowStep
    Public Event AnswerSelected(sender As ucWorkflowStep, answer As Boolean)
    Public Property CurrentNode As ChecklistNode
    Public Sub LoadStep(node As ChecklistNode)
        CurrentNode = node
        lblQuestion.Text = node.Question
        flowInstructions.Controls.Clear()
        For Each instruction As WorkflowInstruction In node.Instructions
            Dim lbl As New Guna.UI2.WinForms.Guna2HtmlLabel()
            lbl.AutoSize = True
            lbl.Font = New Font("Segoe UI", 10)
            lbl.ForeColor = Color.FromArgb(70, 70, 70)
            lbl.Text = "• " & instruction.Text
            lbl.AutoSize = True
            flowInstructions.Controls.Add(lbl)
        Next

    End Sub
    Public Property StepNumber As Integer
        Get
            Return CInt(btnStepNumber.Text)
        End Get

        Set(value As Integer)
            btnStepNumber.Text = value.ToString()
        End Set
    End Property
    Public Property StepStatus As WorkflowStepStatus

        Get
            Return _status
        End Get

        Set(value As WorkflowStepStatus)
            _status = value
            Select Case value
                Case WorkflowStepStatus.Current
                    btnStepNumber.FillColor = Color.RoyalBlue
                    btnStepNumber.ForeColor = Color.White
                    tgAnswer.Enabled = True
                Case WorkflowStepStatus.Completed
                    btnStepNumber.FillColor = Color.ForestGreen
                    btnStepNumber.ForeColor = Color.White
                    tgAnswer.Enabled = False
            End Select
        End Set
    End Property

    Private _status As WorkflowStepStatus
    Public Property Answer As Boolean?
    Private Sub tgAnswer_CheckedChanged(sender As Object, e As EventArgs)
        Answer = tgAnswer.Checked
        RaiseEvent AnswerSelected(Me, Answer.Value)
    End Sub
End Class
