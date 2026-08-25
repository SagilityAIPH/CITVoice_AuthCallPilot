Public Class CallContext

    'Documentation values entered by the agent.
    Public Property CallerName As String
    Public Property CallbackNumber As String
    Public Property SecuredFax As String
    Public Property CallingFrom As String
    Public Property DateOfService As DateTime?

    'Member information scraped from CGX.
    Public Property MemberId As String
    Public Property MemberName As String
    Public Property DateOfBirth As String

    Public Property Product As String
    Public Property Conso As String
    Public Property IssueState As String
    Public Property GroupNumber As String

    'Authorization information scraped from CGX.
    Public Property AuthorizationNumber As String
    Public Property AuthorizationStatus As String
    Public Property AuthType As String

    Public Property RequestingProvider As String
    Public Property TreatingProvider As String
    Public Property FacilityProvider As String

    Public Property AuthorizationStartDate As String
    Public Property AuthorizationEndDate As String
    Public Property TotalDays As String

    Public Property PrimaryDiagnosisCode As String
    Public Property SecondaryDiagnosisCodes As New List(Of String)
    Public Property ProcedureCodes As New List(Of String)

    Public Property ClaimPaymentNotes As String

    'Selected scenario and dynamic answers.
    Public Property Scenario As String
    Public Property HealthType As String
    Public Property CareSetting As String
    Public Property IsExpedited As Boolean?
    Public Property CallerType As String
    Public Property ClinicalReviewNeeded As Boolean?

    'Checking Authorization Status
    Public Property ProviderRequestingApprovedAuthCopy As Boolean?
    Public Property ProviderRequestingLoaCopy As Boolean?

    Public Property NeedsClinicalAdvisor As Boolean?
    Public Property ClinicalAttached As Boolean?

    Public Property RequestingDenialLetter As Boolean?
    Public Property PendingClinicalReview As Boolean?

    'Checking Status of Authorization
    Public Property AuthRequestFound As Boolean?
    Public Property WantsToInitiateNewAuth As Boolean?

    'Agent verification
    Public Property GenesysVerified As Boolean
    Public Property ProviderMemberAuthenticated As Boolean
    Public Property MailingAddressVerified As Boolean
    Public Property Extension As String
    Public Property AdmissionDate As String
    Public Property DischargeDate As String

    Public Property GrouperId As String

End Class