Public Class ucWorkflowStep
    Private _loading As Boolean = False
    Public Sub New()
        InitializeComponent()
    End Sub
    Private Sub ArrangeControls()

        If Me.ClientSize.Width <= 0 Then Exit Sub

        btnStepNumber.Location = New Point(15, 15)

        If CurrentNode IsNot Nothing AndAlso
       CurrentNode.Responses.Count = 0 Then

            pnlResponse.Visible = False

            lblQuestion.Left = 70
            lblQuestion.Top = 18
            lblQuestion.Width =
            Math.Max(100, Me.ClientSize.Width - 85)

            flowInstructions.Left = 70
            flowInstructions.Top = 50
            flowInstructions.MaximumSize =
            New Size(Math.Max(100, Me.ClientSize.Width - 85), 0)

            Exit Sub

        End If

        pnlResponse.Visible = True

        If CurrentNode IsNot Nothing AndAlso (CurrentNode.ResponseType = WorkflowResponseType.RadioButton OrElse CurrentNode.Responses.Count > 2) Then
            'Radio-button area spans beneath the question.
            lblQuestion.Left = 70
            lblQuestion.Top = 18
            lblQuestion.Width = Math.Max(100, Me.ClientSize.Width - 90)

            pnlResponse.Left = 70
            pnlResponse.Top = lblQuestion.Bottom + 8
            pnlResponse.Width = Math.Max(100, Me.ClientSize.Width - 85)

            flowInstructions.Left = 70
            flowInstructions.Top = pnlResponse.Bottom + 5
            flowInstructions.MaximumSize = New Size(Math.Max(100, Me.ClientSize.Width - 85), 0)

        Else
            'Toggle stays at the upper-right.
            pnlResponse.Width = 65
            pnlResponse.Height = 35

            pnlResponse.Left = Math.Max(70, Me.ClientSize.Width - pnlResponse.Width - 15)
            pnlResponse.Top = 15

            lblQuestion.Left = 70
            lblQuestion.Top = 18
            lblQuestion.Width = Math.Max(100, pnlResponse.Left - lblQuestion.Left - 15)

            flowInstructions.Left = 70
            flowInstructions.Top = 50
            flowInstructions.MaximumSize =
            New Size(Math.Max(100, Me.ClientSize.Width - 85), 0)

            If tgAnswer.Parent Is pnlResponse AndAlso
           tgAnswer.Visible Then

                tgAnswer.Left = Math.Max(0, pnlResponse.ClientSize.Width - tgAnswer.Width - 5)
                tgAnswer.Top = Math.Max(0, (pnlResponse.ClientSize.Height - tgAnswer.Height) \ 2)

            End If

        End If
        ResizeRadioButtons()
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
        pnlResponse.PerformLayout()

        Dim responseBottom As Integer = 0

        If pnlResponse.Visible Then
            responseBottom = pnlResponse.Bottom
        End If

        Dim contentBottom As Integer =
        Math.Max(
            flowInstructions.Bottom,
            responseBottom)

        Dim requiredHeight As Integer =
        contentBottom + 15

        If requiredHeight < 90 Then
            requiredHeight = 90
        End If

        Me.Height = requiredHeight

        _loading = False

        If IsHandleCreated Then
            BeginInvoke(
            New MethodInvoker(
                Sub()
                    ArrangeControls()
                    ResizeRadioButtons()
                    flowInstructions.PerformLayout()
                    pnlResponse.PerformLayout()
                    Invalidate()
                End Sub))
        End If

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
        _answer = tgAnswer.Checked

        If CurrentNode IsNot Nothing AndAlso
       CurrentNode.Responses.Count >= 2 Then

            If tgAnswer.Checked Then
                SelectedResponse = CurrentNode.Responses(0)
            Else
                SelectedResponse = CurrentNode.Responses(1)
            End If
        Else
            SelectedResponse = Nothing
        End If
        RaiseEvent AnswerSelected(Me)
    End Sub
    Private Sub RenderResponses(node As ChecklistNode)

        pnlResponse.SuspendLayout()
        pnlResponse.Controls.Clear()

        If node.Responses.Count = 0 Then
            tgAnswer.Visible = False
            pnlResponse.Visible = False
            pnlResponse.ResumeLayout(True)
            Exit Sub
        End If

        pnlResponse.Visible = True

        If node.ResponseType = WorkflowResponseType.RadioButton OrElse node.Responses.Count > 2 Then
            RenderRadioButtons(node)
        Else
            RenderToggle(node)
        End If

        pnlResponse.ResumeLayout(True)

    End Sub
    Private Sub RenderToggle(node As ChecklistNode)

        tgAnswer.Tag = node
        tgAnswer.Visible = True

        pnlResponse.Controls.Add(tgAnswer)
        tgAnswer.Left = Math.Max(0, pnlResponse.ClientSize.Width - tgAnswer.Width - 5)
        tgAnswer.Top = Math.Max(0, (pnlResponse.ClientSize.Height - tgAnswer.Height) \ 2)

    End Sub
    Private Sub RenderRadioButtons(node As ChecklistNode)

        tgAnswer.Visible = False

        Dim y As Integer = 5

        For Each response As WorkflowResponse In node.Responses

            Dim rb As New Guna.UI2.WinForms.Guna2RadioButton With {
            .Text = response.Text,
            .Tag = response,
            .AutoSize = False,
            .Width = Math.Max(100, pnlResponse.ClientSize.Width - 10),
            .Height = 32,
            .Left = 5,
            .Top = y,
            .Font = New Font("Segoe UI", 9.5!, FontStyle.Regular),
            .ForeColor = Color.FromArgb(45, 45, 45),
            .BackColor = Color.Transparent,
            .Cursor = Cursors.Hand
        }

            rb.CheckedState.BorderColor = Color.FromArgb(94, 148, 255)
            rb.CheckedState.BorderThickness = 0
            rb.CheckedState.FillColor = Color.FromArgb(94, 148, 255)
            rb.CheckedState.InnerColor = Color.White
            rb.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149)
            rb.UncheckedState.BorderThickness = 2
            rb.UncheckedState.FillColor = Color.Transparent
            rb.UncheckedState.InnerColor = Color.Transparent

            If SelectedResponse Is response OrElse
           (SelectedResponse IsNot Nothing AndAlso
            String.Equals(
                SelectedResponse.Text,
                response.Text,
                StringComparison.OrdinalIgnoreCase)) Then

                rb.Checked = True

            End If

            AddHandler rb.CheckedChanged,
            AddressOf RadioButton_CheckedChanged

            pnlResponse.Controls.Add(rb)

            y += rb.Height + 5

        Next

        pnlResponse.Height = Math.Max(35, y + 5)

    End Sub
    Private Sub RadioButton_CheckedChanged(sender As Object, e As EventArgs)
        If _loading Then Exit Sub

        Dim rb As Guna.UI2.WinForms.Guna2RadioButton =
        TryCast(sender, Guna.UI2.WinForms.Guna2RadioButton)

        If rb Is Nothing OrElse Not rb.Checked Then Exit Sub

        SelectedResponse = TryCast(rb.Tag, WorkflowResponse)

        If SelectedResponse Is Nothing Then Exit Sub
        _answer = Nothing
        RaiseEvent AnswerSelected(Me)
    End Sub
    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        If IsHandleCreated Then
            ArrangeControls()
            pnlResponse.PerformLayout()
            Invalidate()
        End If
    End Sub
    Protected Overrides Sub OnVisibleChanged(e As EventArgs)
        MyBase.OnVisibleChanged(e)
        ArrangeControls()
    End Sub
    Private Sub ResizeRadioButtons()
        Dim availableWidth As Integer = Math.Max(100, pnlResponse.ClientSize.Width - 10)
        For Each rb As Guna.UI2.WinForms.Guna2RadioButton In pnlResponse.Controls.OfType(Of Guna.UI2.WinForms.Guna2RadioButton)()
            rb.Width = availableWidth
        Next
    End Sub
End Class
