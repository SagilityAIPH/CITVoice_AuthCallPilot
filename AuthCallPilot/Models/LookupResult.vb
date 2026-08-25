Public Class LookupResult

    Public Property IsOutOfScope As Boolean?
    Public Property OutOfScopeMessage As String
    Public Property RestrictionType As String

    Public Property MarketGuideFound As Boolean
    Public Property MarketGuideReference As String
    Public Property MarketGuideMessage As String

    Public Property PalFound As Boolean
    Public Property PalResults As New List(Of String)
    Public Property PalMatchedProcedureCodes As New List(Of String)

    '========================================
    ' IPA / PCODSOT LOOKUP
    '========================================
    Public Property IpaFound As Boolean
    Public Property IpaGrouperId As String
    Public Property IpaContractedEntityName As String

    '========================================
    ' DELEGATED GROUPER / POWER BI
    '========================================
    Public Property DelegatedGrouper As DelegatedGrouperResult
    Public Property Errors As New List(Of String)
    Public Sub New()
        DelegatedGrouper = New DelegatedGrouperResult()
    End Sub
End Class