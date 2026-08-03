Public Class LookupResult

    Public Property IsOutOfScope As Boolean?
    Public Property OutOfScopeMessage As String
    Public Property RestrictionType As String

    Public Property MarketGuideFound As Boolean
    Public Property MarketGuideReference As String
    Public Property MarketGuideMessage As String

    Public Property PalFound As Boolean
    Public Property PalResults As New List(Of String)

    Public Property Errors As New List(Of String)

End Class