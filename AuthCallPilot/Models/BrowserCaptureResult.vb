Public Enum CgxPageType
    Other
    MemberInformation
    ViewAuthorization
End Enum

Public Class BrowserCaptureResult
    Public Property PageType As CgxPageType
    Public Property Url As String
    Public Property Title As String
    Public Property Context As CallContext

End Class