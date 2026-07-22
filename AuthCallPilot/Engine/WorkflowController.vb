Public Class WorkflowController
    Private _session As WorkflowSession
    Public ReadOnly Property Session As WorkflowSession
        Get
            Return _session
        End Get
    End Property
    Public Sub New(session As WorkflowSession)
        _session = session
    End Sub
    Public Function GetState(node As ChecklistNode) As WorkflowNodeState
        Return _session.Path.FirstOrDefault(
            Function(x) x.Node Is node)
    End Function
    Public Function GetNextNode(state As WorkflowNodeState) As ChecklistNode
        If state Is Nothing Then Return Nothing
        If Not state.Answer.HasValue Then Return Nothing
        If state.Answer.Value Then
            Return state.Node.YesNode
        Else
            Return state.Node.NoNode
        End If
    End Function
    Public Function AdvanceWorkflow(state As WorkflowNodeState) As Integer
        Dim index As Integer = _session.Path.IndexOf(state)
        While _session.Path.Count > index + 1
            _session.Path.RemoveAt(_session.Path.Count - 1)
        End While

        Dim nextNode As ChecklistNode = GetNextNode(state)
        If nextNode IsNot Nothing Then
            _session.Path.Add(New WorkflowNodeState With {
                .Node = nextNode
            })
        End If
        Return index + 2

    End Function
End Class