Public Class Workflow
    Public Property ID As String
    Public Property Name As String
    Public Property RootNode As ChecklistNode
    Public Sub New(id As String, name As String, root As ChecklistNode)
        Me.ID = id
        Me.Name = name
        Me.RootNode = root
    End Sub
End Class