Public Class ChecklistNode

    Public Property ID As String
    Public Property Question As String
    Public Property Instructions As New List(Of WorkflowInstruction)
    Public Property YesNode As ChecklistNode
    Public Property NoNode As ChecklistNode

    Public Sub New(id As String, question As String)
        Me.ID = id
        Me.Question = question

    End Sub

End Class