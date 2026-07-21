Public Class ucWorkflowSection
    Private _expanded As Boolean = True
    Public Property Expanded As Boolean

        Get
            Return _expanded
        End Get

        Set(value As Boolean)

            _expanded = value

            flowSteps.Visible = value

            If value Then
                lblExpand.Text = "▼"
            Else
                lblExpand.Text = "▶"
            End If

        End Set

    End Property
    Public Sub New()

        InitializeComponent()

        Expanded = True

    End Sub

    Private Sub pnlHeader_Click(sender As Object, e As EventArgs) Handles pnlHeader.Click
        Expanded = Not Expanded
    End Sub

    Private Sub lblTitle_Click(sender As Object, e As EventArgs) Handles lblTitle.Click
        Expanded = Not Expanded
    End Sub

    Private Sub lblExpand_Click(sender As Object, e As EventArgs) Handles lblExpand.Click
        Expanded = Not Expanded
    End Sub
    Public Sub AddWorkflowStep(workflowStep As ucWorkflowStep)

        workflowStep.Width = flowSteps.ClientSize.Width - 10
        flowSteps.Controls.Add(workflowStep)
        flowSteps.PerformLayout()

        cardSection.Height =
            pnlHeader.Height +
            flowSteps.Height +
            20

        Me.Height = cardSection.Height
    End Sub
    Private Sub ucWorkflowSection_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.AutoSize = True
        Me.AutoSizeMode = AutoSizeMode.GrowAndShrink

    End Sub
End Class
