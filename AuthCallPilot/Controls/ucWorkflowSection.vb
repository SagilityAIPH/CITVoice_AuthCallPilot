Public Class ucWorkflowSection

    Private _expanded As Boolean = True

    Public Sub New()
        InitializeComponent()
        lblTitle.Text = "Authentication"
        Expanded = True
    End Sub
    Public Property SectionTitle As String

        Get
            Return lblTitle.Text
        End Get

        Set(value As String)
            lblTitle.Text = value
        End Set

    End Property
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
    Private Sub Header_Click(sender As Object, e As EventArgs) Handles pnlHeader.Click, lblTitle.Click, lblExpand.Click
        Expanded = Not Expanded
    End Sub
    Public Sub AddWorkflowStep(stepControl As ucWorkflowStep)
        stepControl.Margin = New Padding(0, 0, 0, 8)
        stepControl.Width =
            flowSteps.ClientSize.Width -
            flowSteps.Padding.Left -
            flowSteps.Padding.Right
        flowSteps.Controls.Add(stepControl)
    End Sub
    Private Sub flowSteps_Resize(sender As Object, e As EventArgs) Handles flowSteps.Resize

        Dim availableWidth As Integer =
            flowSteps.ClientSize.Width -
            flowSteps.Padding.Left -
            flowSteps.Padding.Right

        For Each stepControl As ucWorkflowStep In flowSteps.Controls.OfType(Of ucWorkflowStep)()
            stepControl.Width = availableWidth
        Next
    End Sub
End Class