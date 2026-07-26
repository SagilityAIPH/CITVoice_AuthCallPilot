Public Class ucWorkflowStep
    Private _loading As Boolean = False
    Public Sub New()
        InitializeComponent()
    End Sub
    Private Sub ArrangeControls()

        btnStepNumber.Location = New Point(15, 15)

        pnlResponse.Width = 120
        pnlResponse.Height = 35

        pnlResponse.Left = Me.ClientSize.Width - pnlResponse.Width - 15
        pnlResponse.Top = 15

        lblQuestion.Left = 70
        lblQuestion.Top = 18
        lblQuestion.Width = pnlResponse.Left - lblQuestion.Left - 15

        flowInstructions.Left = 70
        flowInstructions.Top = 50
        flowInstructions.MaximumSize = New Size(lblQuestion.Width, 0)

    End Sub
    'Public Event AnswerSelected(sender As ucWorkflowStep, answer As Boolean)
    Public Event AnswerSelected(stepControl As ucWorkflowStep)
    Public Property CurrentNode As ChecklistNode
    Public Sub LoadNode(node As ChecklistNode)
        _loading = True
        CurrentNode = node
        lblQuestion.Text = node.Question
        LoadInstructions(node)
        RenderResponses(node)
        ArrangeControls()
        flowInstructions.PerformLayout()

        Dim requiredHeight As Integer =
            tblHeader.Height +
            flowInstructions.PreferredSize.Height +
            20

        If requiredHeight < 90 Then
            requiredHeight = 90
        End If

        Me.Height = requiredHeight

        _loading = False
    End Sub
    Private Sub LoadInstructions(node As ChecklistNode)

        flowInstructions.Controls.Clear()
        flowInstructions.SuspendLayout()

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

        flowInstructions.ResumeLayout(True)
        flowInstructions.PerformLayout()

    End Sub
    Public Property StepNumber As Integer
        Get
            Return CInt(btnStepNumber.Text)
        End Get

        Set(value As Integer)
            btnStepNumber.Text = value.ToString()
        End Set
    End Property
    Private _selectedResponse As WorkflowResponse
    Public Property SelectedResponse As WorkflowResponse
        Get
            Return _selectedResponse
        End Get

        Set(value As WorkflowResponse)
            _selectedResponse = value
        End Set
    End Property
    Private _answer As Boolean?
    Public Property Answer As Boolean?
        Get
            Return _answer
        End Get
        Set(value As Boolean?)
            _loading = True
            _answer = value

            If value.HasValue Then
                tgAnswer.Checked = value.Value
            Else
                tgAnswer.Checked = False
            End If

            _loading = False
        End Set
    End Property
    Private Sub tgAnswer_CheckedChanged_1(sender As Object, e As EventArgs) Handles tgAnswer.CheckedChanged
        If _loading Then Exit Sub
        Answer = tgAnswer.Checked
        RaiseEvent AnswerSelected(Me)
    End Sub
    Private Sub RenderResponses(node As ChecklistNode)
        pnlResponse.Controls.Clear()
        If node.Responses.Count <= 2 Then
            RenderToggle(node)
        Else
            RenderRadioButtons(node)
        End If
    End Sub
    Private Sub RenderToggle(node As ChecklistNode)
        tgAnswer.Tag = node
        tgAnswer.Visible = True
        pnlResponse.Controls.Add(tgAnswer)
    End Sub
    Private Sub RenderRadioButtons(node As ChecklistNode)
        Dim y As Integer = 5
        For Each response As WorkflowResponse In node.Responses
            Dim rb As New RadioButton()
            rb.Text = response.Text
            rb.Tag = response

            rb.AutoSize = True
            rb.Left = 5
            rb.Top = y

            AddHandler rb.CheckedChanged, AddressOf RadioButton_CheckedChanged
            pnlResponse.Controls.Add(rb)
            y += rb.Height + 5
        Next
    End Sub
    Private Sub RadioButton_CheckedChanged(sender As Object, e As EventArgs)
        Dim rb As RadioButton = CType(sender, RadioButton)
        If Not rb.Checked Then Exit Sub
        SelectedResponse = CType(rb.Tag, WorkflowResponse)
        If CurrentNode IsNot Nothing AndAlso CurrentNode.Responses.Count >= 2 Then
            If tgAnswer.Checked Then
                SelectedResponse = CurrentNode.Responses(0)
            Else
                SelectedResponse = CurrentNode.Responses(1)
            End If
        End If
        RaiseEvent AnswerSelected(Me)
    End Sub
End Class
