Public Class WorkflowInstruction
    Public Property Text As String
    Public Property Type As InstructionType
    Public Sub New(text As String, type As InstructionType)
        Me.Text = text
        Me.Type = type
    End Sub

End Class

Public Enum InstructionType
    Normal
    Action
    Warning
    Reference
    EndProcess
End Enum