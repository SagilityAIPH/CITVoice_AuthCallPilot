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
        flowSteps.Controls.Add(stepControl)

        ResizeWorkflowSteps()
    End Sub
    Public Sub ResizeWorkflowSteps()

        If flowSteps.ClientSize.Width <= 0 Then Exit Sub

        Dim availableWidth As Integer = flowSteps.ClientSize.Width - flowSteps.Padding.Left - flowSteps.Padding.Right

        'Leave a small allowance so a vertical scrollbar
        'does not cause horizontal clipping.
        availableWidth -= 4

        If availableWidth < 100 Then Exit Sub

        For Each stepControl As ucWorkflowStep In
            flowSteps.Controls.OfType(Of ucWorkflowStep)()
            stepControl.Width = availableWidth
        Next

    End Sub
    Private Sub flowSteps_Resize(sender As Object, e As EventArgs) Handles flowSteps.Resize
        ResizeWorkflowSteps()
    End Sub
    Public Sub ClearWorkflowSteps()
        flowSteps.SuspendLayout()
        For Each control As Control In flowSteps.Controls.Cast(Of Control)().ToList()
            flowSteps.Controls.Remove(control)
            control.Dispose()
        Next
        flowSteps.ResumeLayout(True)
    End Sub
    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        ResizeWorkflowSteps()
    End Sub
End Class