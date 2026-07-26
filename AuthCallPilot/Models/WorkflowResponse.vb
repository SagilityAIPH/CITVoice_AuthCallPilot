Public Class WorkflowResponse
    Public Property Id As String
    Public Property Text As String
    Public Property NextNode As ChecklistNode
    Public Sub New(text As String, nextNode As ChecklistNode)
        Me.Text = text
        Me.NextNode = nextNode
    End Sub
End Class