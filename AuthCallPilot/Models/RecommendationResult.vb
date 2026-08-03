Public Class RecommendationResult

    Public Property OverallOutput As String
    Public Property NextBestAction As String

    Public Property RequiresAgentInput As Boolean

    Public Property QuestionId As String
    Public Property QuestionText As String
    Public Property QuestionOptions As New List(Of String)

End Class