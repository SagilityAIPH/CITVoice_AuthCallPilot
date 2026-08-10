Imports System.Text
Imports System.Linq
Public NotInheritable Class RecommendationEngine

    Private Sub New()
    End Sub
    Private Shared Function Normalize(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then
            Return String.Empty
        End If
        Return value.Trim().ToUpperInvariant()
    End Function
    Private Shared Function DisplayValue(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then
            Return "Not available"
        End If
        Return value.Trim()
    End Function
    Public Shared Function Analyze(context As CallContext, lookups As LookupResult) As RecommendationResult
        If context Is Nothing Then
            Throw New ArgumentNullException(NameOf(context))
        End If

        If lookups Is Nothing Then
            Throw New ArgumentNullException(NameOf(lookups))
        End If

        Dim result As New RecommendationResult()
        result.OverallOutput = BuildOverallOutput(context, lookups)
        Select Case Normalize(context.Scenario)

            Case "UPDATING AUTHORIZATION"
                Return AnalyzeUpdatingAuthorization(context, lookups, result)

            Case "NEW AUTHORIZATION"
                result.NextBestAction = "The New Authorization scenario has not yet been configured."

            Case "CHECKING STATUS OF THE AUTHORIZATION"
                result.NextBestAction = "The Checking Status of the Authorization scenario has not yet been configured."

            Case Else
                result.NextBestAction = "Select a supported scenario."

        End Select

        Return result
    End Function

    Private Shared Function BuildOverallOutput(context As CallContext, lookup As LookupResult) As String

        Dim output As New StringBuilder()

        output.AppendLine("CALL INFORMATION")
        output.AppendLine(New String("-"c, 40))

        output.AppendLine("Caller: " & (context.CallerName).Trim())
        output.AppendLine("Member ID: " & context.MemberId)
        output.AppendLine("Scenario: " & context.Scenario)

        output.AppendLine()
        output.AppendLine("CGX INFORMATION")
        output.AppendLine(New String("-"c, 40))

        output.AppendLine("Product: " & context.Product)
        output.AppendLine("Group Number: " & context.GroupNumber)
        output.AppendLine("Health Type: " & context.HealthType)
        output.AppendLine("Care Setting: " & context.CareSetting)
        output.AppendLine("Authorization Status: " & context.AuthorizationStatus)

        output.AppendLine()
        output.AppendLine("LOOKUP RESULTS")
        output.AppendLine(New String("-"c, 40))

        output.AppendLine("Out of Scope: " & FormatBoolean(lookup.IsOutOfScope))
        output.AppendLine(lookup.OutOfScopeMessage)

        output.AppendLine()

        output.AppendLine("Market Guide: " & lookup.MarketGuideMessage)
        output.AppendLine()
        output.AppendLine("PAL:")

        For Each palResult As String In
            lookup.PalResults
            output.AppendLine("• " & palResult)
        Next

        Return output.ToString()

    End Function

    Private Shared Function BuildStandardAction(lookup As LookupResult) As String

        Return "NEXT BEST ACTION" &
            Environment.NewLine &
            New String("-"c, 40) &
            Environment.NewLine &
            "1. Proceed with the standard update process." &
            Environment.NewLine &
            Environment.NewLine &
            "2. Request the required clinical information." &
            Environment.NewLine &
            Environment.NewLine &
            "3. Follow Market Guide " &            'lookup.MarketGuideMessage &
            Environment.NewLine &
            Environment.NewLine &
            "4. Review PAL results." &
            Environment.NewLine &
            Environment.NewLine &
            "5. Complete documentation."
    End Function

    Private Shared Function BuildExpeditedAction(context As CallContext, lookup As LookupResult) As String

        Return "NEXT BEST ACTION" &
            Environment.NewLine &
            New String("-"c, 40) &
            Environment.NewLine &
            "Caller Type: " &
            context.CallerType &
            Environment.NewLine &
            Environment.NewLine &
            "1. Proceed with expedited handling." &
            Environment.NewLine &
            Environment.NewLine &
            "2. Request required clinicals." &
            Environment.NewLine &
            Environment.NewLine &
            "3. Follow Market Guide " &            'lookup.MarketGuideMessage &
            Environment.NewLine &
            Environment.NewLine &
            "4. Review PAL results." &
            Environment.NewLine &
            Environment.NewLine &
            "5. Confirm queue assignment and turnaround time." &
            Environment.NewLine &
            Environment.NewLine &
            "6. Complete documentation."
    End Function

    Private Shared Function FormatBoolean(value As Boolean?) As String

        If Not value.HasValue Then
            Return "Unable to determine"
        End If

        Return If(value.Value, "YES", "NO")
    End Function
    Private Shared Function AnalyzeUpdatingAuthorization(context As CallContext, lookup As LookupResult, result As RecommendationResult) As RecommendationResult
        'Health Type is not currently scraped from CGX.
        If String.IsNullOrWhiteSpace(context.HealthType) Then
            Return AskQuestion(result, "HEALTH_TYPE", "Is this for Physical Health or Behavioral Health?", "PHYSICAL HEALTH", "BEHAVIORAL HEALTH")
        End If

        'Care setting is not currently scraped from CGX.
        If String.IsNullOrWhiteSpace(context.CareSetting) Then
            Return AskQuestion(result, "CARE_SETTING", "Is this for Inpatient or Outpatient?", "INPATIENT", "OUTPATIENT")
        End If

        Dim totalDays As Integer
        If Not TryGetTotalDays(context.TotalDays, totalDays) Then
            result.NextBestAction = "Unable to determine Total Days from CGX."
            Return result
        End If

        Dim healthType As String = Normalize(context.HealthType)
        Dim careSetting As String = Normalize(context.CareSetting)
        Dim authorizationStatus As String = Normalize(context.AuthorizationStatus)

        Dim expirationLimit As Integer

        If careSetting = "INPATIENT" Then
            expirationLimit = 90
        ElseIf careSetting = "OUTPATIENT" Then
            expirationLimit = 30
        Else
            result.NextBestAction = "Unable to determine whether the authorization is Inpatient or Outpatient."
            Return result
        End If

        'Expired threshold branch.
        Dim authEndDate As DateTime
        If DateTime.TryParse(context.AuthorizationEndDate, authEndDate) Then
            Dim expirationDate As DateTime = authEndDate.Date
            Dim expirationWindowEnd As DateTime = expirationDate.AddDays(30)

            If Date.Today <= expirationDate And Date.Today <= expirationWindowEnd Then
                result.NextBestAction = BuildExpiredAuthorizationAction()
                Return result
            End If
        End If

        'If totalDays >= expirationLimit Then
        '    result.NextBestAction = BuildExpiredAuthorizationAction()
        '    Return result
        'End If

        'Denied branch.
        If IsDeniedStatus(authorizationStatus) Then
            result.NextBestAction = BuildDeniedAuthorizationAction()
            Return result
        End If

        'Approved branch.
        If IsApprovedStatus(authorizationStatus) Then
            result.NextBestAction = BuildApprovedAuthorizationAction(careSetting, healthType)
            Return result
        End If

        'Pending authorization requires agent decisions.
        If IsPendingStatus(authorizationStatus) Then
            Return AnalyzePendingAuthorization(context, result, healthType, careSetting)
        End If

        result.NextBestAction = "Unable to determine the Updating Authorization process for status: " & DisplayValue(context.AuthorizationStatus)
        Return result
    End Function
    Private Shared Function AnalyzePendingAuthorization(context As CallContext, result As RecommendationResult, healthType As String, careSetting As String) As RecommendationResult

        If Not context.IsExpedited.HasValue Then
            Return AskQuestion(result, "EXPEDITED_REQUEST", "Is this an expedited request?", "YES", "NO")
        End If

        If Not context.IsExpedited.Value Then
            result.NextBestAction = BuildStandardPendingAction(careSetting, healthType)
            Return result
        End If

        If String.IsNullOrWhiteSpace(context.CallerType) Then
            Return AskQuestion(result, "CALLER_TYPE", "Is the caller a Specialist or PCP?", "SPECIALIST", "PCP")
        End If

        If Normalize(context.CallerType) = "SPECIALIST" Then
            result.NextBestAction = BuildSpecialistExpeditedAction(careSetting, healthType)
        ElseIf Normalize(context.CallerType) = "PCP" Then
            result.NextBestAction = BuildPcpExpeditedAction(careSetting, healthType)
        Else
            result.NextBestAction = "Unable to determine whether the caller is a Specialist or PCP."
        End If

        Return result
    End Function
    Private Shared Function AskQuestion(result As RecommendationResult, questionId As String, questionText As String, ParamArray options() As String) As RecommendationResult

        result.RequiresAgentInput = True
        result.QuestionId = questionId
        result.QuestionText = questionText
        result.QuestionOptions = New List(Of String)(options)
        result.NextBestAction = "Additional information is required."
        Return result

    End Function
    Private Shared Function BuildExpiredAuthorizationAction() As String
        Return FormatActions("Do not update the authorization.",
            "Redirect the caller to Claims for provider dispute.", "Complete the required documentation.",
            "https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ed1e15&dl=0&searchID=VI-8d964baa0b8ecb7&row=0&mode=Mentor", "END OF THE PROCESS.")
    End Function
    Private Shared Function BuildDeniedAuthorizationAction() As String
        Return FormatActions(
            "Do not update the authorization.",
            "If it is within 65 days of the denial, redirect the caller to Appeals. Appeal information is included in the denial letter.",
            "If the authorization was denied more than 65 days ago, advise the caller to submit a new authorization.",
            "Complete the required documentation.",
            "https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ed1e15&dl=0&searchID=VI-8d964baa0b8ecb7&row=0&mode=Mentor",
            "END OF THE PROCESS.")
    End Function
    Private Shared Function BuildApprovedAuthorizationAction(careSetting As String, healthType As String) As String
        Dim actions As New List(Of String)
        If careSetting = "INPATIENT" AndAlso healthType = "PHYSICAL HEALTH" Then
            actions.Add("Check for a possible duplicate case or authorization.")
            actions.Add("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982bc03a9&dl=0&searchID=VI-8d8558edf4ef58a&row=0&mode=Mentor")
        Else
            actions.Add("Check whether there is a duplicate request.")
            actions.Add("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982bc03a9&dl=0&searchID=VI-8d8558edf4ef58a&row=0&mode=Mentor")

        End If
        actions.Add("Ask the caller what they want to update on the authorization.")
        actions.Add("Proceed with the applicable authorization update process.")
        actions.Add("Advise the requestor that the authorization is still approved and provide the applicable disclaimer.")
        actions.Add("Provide the reference number, which is the same as the authorization number.")
        actions.Add("Deliver the closing script and transfer to VOC.")
        actions.Add("Complete the required documentation.")
        actions.Add("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ed1e15&dl=0&searchID=VI-8d964baa0b8ecb7&row=0&mode=Mentor")
        actions.Add("END OF THE PROCESS.")

        Return FormatActions(actions)

    End Function
    Private Shared Function BuildStandardPendingAction(careSetting As String, healthType As String) As String
        Dim actions As New List(Of String)
        If careSetting = "INPATIENT" AndAlso healthType = "PHYSICAL HEALTH" Then
            actions.Add("Check for a possible duplicate case or authorization.")
        Else
            actions.Add("Check whether there is a duplicate request.")
        End If
        actions.Add("Ask the caller what they want to update on the authorization.")
        actions.Add("Proceed with the applicable authorization update process.")
        actions.Add("Proceed with the standard process.")
        Return FormatActions(actions)
    End Function
    Private Shared Function BuildSpecialistExpeditedAction(careSetting As String, healthType As String) As String
        Dim actions As New List(Of String)

        actions.Add("Proceed with expediting the authorization.")
        If healthType = "BEHAVIORAL HEALTH" Then
            actions.Add("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=09000929877eda0b&dl=0&searchID=VI-8de6a4a0200a0d6&row=0&mode=Mentor&launchId=1770927524924")
        Else
            actions.Add("https://dctm.humana.com/mentor/xweb/ViewTopic.aspx?schronicleID=0900092987745d15&searchID=link")
        End If

        actions.Add("Request the required clinical information.")
        actions.Add("https://dctm.humana.com/mentor/web/v.aspx/CO-Overview%20Common%20Records%20Request%20Verbiage?chronicleID=090009298687b587&dl=0&searchID=wdkLink")

        If healthType = "BEHAVIORAL HEALTH" Then
            actions.Add("Request clinicals to be faxed to 469-913-6941 and check the BH Expedited queue assignment.")
            actions.Add("Provide the applicable turnaround timeframe.")
            actions.Add("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982b310f5&dl=0&searchID=VI-8de6a48b4b09948&row=0&mode=Mentor&launchId=1770926965607")
            actions.Add("Enter the NRD as the next business day.")
            actions.Add("Obtain and document the following in Free Text Notes: Reason for Admission, Discharge Plan, Discharge Planner Name, Phone Number, Concurrent UR Contact Name, Phone Number, and Fax Number.")
        ElseIf careSetting = "OUTPATIENT" Then
            actions.Add("Provide fax number 1-888-200-7440. For Genetic requests, use 855-227-0677.")
            actions.Add("Manually queue the authorization to CIT ALL Medicare Expedited. For Genetic requests, use Genetic Expedited.")
            actions.Add("Provide the applicable turnaround timeframe.")
            actions.Add("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982b310f5&dl=0&searchID=VI-8de26a7262643f8&row=0&mode=Mentor&launchId=1763490851475")
            actions.Add("Provide the reference number, which is the same as the authorization number.")
        Else
            actions.Add("Provide the applicable fax number.")
            actions.Add("https://dctm.humana.com/Mentor/Web/v.aspx/HSO%20Regional%20Map%20and%20Contact%20Information?chronicleID=0900092980bc4220&dl=0&searchID=link&row=0")
            actions.Add("Check the applicable queue assignment.")
            actions.Add("https://dctm.humana.com/Mentor/Web/v.aspx/ClinicalDirectory?chronicleID=09000929813a2f13&dl=1&searchID=VI-8dd33d5130cee9d&row=0")
            actions.Add("Provide the applicable turnaround timeframe.")
            actions.Add("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982b310f5&dl=0&searchID=VI-8de26a7262643f8&row=0&mode=Mentor&launchId=1763490851475")
            actions.Add("Provide the reference number, which is the same as the authorization number.")

        End If
        actions.Add("Deliver the closing script and transfer to VOC.")
        actions.Add("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=090009298a27e33e&dl=0&searchID=VI-8de6a47157e8c45&row=0&mode=Mentor&launchId=1770926270025")
        actions.Add("Complete the required documentation.")
        actions.Add("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ed1e15&dl=0&searchID=VI-8d964baa0b8ecb7&row=0&mode=Mentor")
        actions.Add("END OF THE PROCESS.")

        Return FormatActions(actions)

    End Function
    Private Shared Function BuildPcpExpeditedAction(careSetting As String, healthType As String) As String
        Dim actions As New List(Of String)
        actions.Add("Request the required clinical information.")
        If healthType = "BEHAVIORAL HEALTH" Then
            actions.Add("Provide fax number 469-913-6941 and check the applicable queue assignment.")
        Else
            actions.Add("Provide the applicable fax number.")
            actions.Add("Check the applicable queue assignment.")
        End If
        actions.Add("Provide the applicable turnaround timeframe.")
        actions.Add("Provide the reference number, which is the same as the authorization number.")
        actions.Add("Deliver the closing script and transfer to VOC.")
        actions.Add("Complete the required documentation.")
        actions.Add("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ed1e15&dl=0&searchID=VI-8d964baa0b8ecb7&row=0&mode=Mentor")
        actions.Add("END OF THE PROCESS.")

        Return FormatActions(actions)
    End Function
    Private Shared Function FormatActions(ParamArray actions() As String) As String
        Return FormatActions(CType(actions, IEnumerable(Of String)))
    End Function
    Private Shared Function FormatActions(actions As IEnumerable(Of String)) As String

        Dim output As New Text.StringBuilder()

        output.AppendLine("NEXT BEST ACTION")
        output.AppendLine(New String("-"c, 45))

        Dim stepNumber As Integer = 1
        For Each actionText As String In actions
            If String.IsNullOrWhiteSpace(actionText) Then
                Continue For
            End If
            output.AppendLine(stepNumber.ToString() & ". " & actionText.Trim())
            output.AppendLine()
            stepNumber += 1
        Next
        Return output.ToString().TrimEnd()

    End Function
    Private Shared Function TryGetTotalDays(rawValue As String, ByRef totalDays As Integer) As Boolean
        totalDays = 0
        If String.IsNullOrWhiteSpace(rawValue) Then
            Return False
        End If

        If Integer.TryParse(rawValue.Trim(), totalDays) Then
            Return True
        End If

        'Handles text such as "90 Days".
        Dim digits As String = New String(rawValue.Where(Function(character)
                                                             Return Char.IsDigit(character)
                                                         End Function).ToArray())

        Return Integer.TryParse(digits, totalDays)

    End Function
    Private Shared Function IsDeniedStatus(status As String) As Boolean
        Dim normalizedStatus As String = Normalize(status)
        Return normalizedStatus.Contains("DENIED") Or normalizedStatus.Contains("DENY")
    End Function
    Private Shared Function IsApprovedStatus(status As String) As Boolean
        Dim normalizedStatus As String = Normalize(status)
        Return normalizedStatus.Contains("APPROVED") Or normalizedStatus.Contains("APPROVE")
    End Function
    Private Shared Function IsPendingStatus(status As String) As Boolean
        Dim normalizedStatus As String = Normalize(status)
        Return normalizedStatus.Contains("PENDING") Or normalizedStatus.Contains("PENDED")
    End Function
End Class