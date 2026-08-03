Public Class CallContext

    'Agent input
    Public Property CallerFirstName As String
    Public Property CallerLastName As String
    Public Property MemberId As String
    Public Property DateOfBirth As DateTime?
    Public Property Scenario As String

    'CGX values
    Public Property Product As String
    Public Property Conso As String
    Public Property IssueState As String

    Public Property GroupNumber As String
    Public Property GroupName As String

    Public Property HealthType As String
    Public Property CareSetting As String

    Public Property AuthorizationStatus As String

    Public Property ProcedureCodes As New List(Of String)

    'Agent answers from pnlActions
    Public Property IsExpedited As Boolean?
    Public Property CallerType As String

End Class