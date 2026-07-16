Public Class WorkflowSession
    Public Property Root As ChecklistNode
    Public Property Current As ChecklistNode
    Public Property Path As New List(Of WorkflowNodeState)
End Class

Public Class WorkflowNodeState
    Public Property Node As ChecklistNode
    Public Property Answer As Boolean?

End Class