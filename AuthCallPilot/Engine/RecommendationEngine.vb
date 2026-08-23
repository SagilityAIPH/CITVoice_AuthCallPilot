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
                Return AnalyzeCheckingAuthorizationStatus(context, lookups, result)

            Case Else
                result.NextBestAction = "Select a supported scenario."

        End Select

        Return result
    End Function
    Private Shared Function AnalyzeCheckingAuthorizationStatus(context As CallContext, lookup As LookupResult, result As RecommendationResult) As RecommendationResult
        '========================================
        ' FIRST QUESTION:
        ' WAS THE AUTH REQUEST FOUND?
        '========================================
        If Not context.AuthRequestFound.HasValue Then
            Return AskQuestion(result, "AUTH_REQUEST_FOUND", "Is the request found on the system?", "YES", "NO")
        End If

        '========================================
        ' REQUEST NOT FOUND
        '========================================
        If Not context.AuthRequestFound.Value Then
            Return AnalyzeAuthorizationNotFound(context, result)
        End If

        '========================================
        ' REQUEST FOUND
        ' USE STATUS CAPTURED FROM CGX
        '========================================

        Dim authorizationStatus As String = Normalize(context.AuthorizationStatus)
        '========================================
        ' APPROVED
        '========================================
        If IsApprovedStatus(authorizationStatus) Then
            Return AnalyzeApprovedAuthorizationStatus(context, result)
        End If


        '========================================
        ' PENDING OR DENIED
        '========================================
        If IsPendingStatus(authorizationStatus) Or IsDeniedStatus(authorizationStatus) Then
            Return AnalyzeNonApprovedAuthorizationStatus(context, result)
        End If


        '========================================
        ' STATUS NOT RECOGNIZED
        '========================================
        result.NextBestAction = "Unable to determine authorization status from CGX: " & DisplayValue(context.AuthorizationStatus)
        Return result

    End Function
    Private Shared Function AnalyzeAuthorizationNotFound(context As CallContext, result As RecommendationResult) As RecommendationResult

        result.NextBestAction = FormatActions("Agent will offer to submit a new authorization to the caller.")

        If Not context.WantsToInitiateNewAuth.HasValue Then
            Return AskQuestion(result, "INITIATE_NEW_AUTH", "Is the caller wishing to initiate a new auth?", "YES", "NO")
        End If

        '========================================
        ' YES - GO TO PROVIDER TRIAGE
        '========================================
        If context.WantsToInitiateNewAuth.Value Then
            result.NextBestAction = FormatActions("Proceed to Provider Triage.")
            Return result
        End If

        '========================================
        ' NO - CLOSE THE CALL
        '========================================
        result.NextBestAction = FormatActions(
            "Agent will save COR.",
            "Agent will deliver the closing script and offer VOC.",
            GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=090009298a27e33e&dl=0&searchID=VI-8de6a47157e8c45&row=0&mode=Mentor&launchId=1770926270025"),
            "Agent will complete documentation.",
            GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ed1e15&dl=0&searchID=VI-8d964baa0b8ecb7&row=0&mode=Mentor"),
            "END OF THE PROCESS.")
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

    Private Shared Function FormatBoolean(value As Boolean?) As String

        If Not value.HasValue Then
            Return "Unable to determine"
        End If

        Return If(value.Value, "YES", "NO")
    End Function
    Private Shared Function TryGetAuthorizationEndDate(rawValue As String, ByRef endDate As DateTime) As Boolean
        endDate = Date.MinValue
        If String.IsNullOrWhiteSpace(rawValue) Then
            Return False
        End If
        Dim normalized As String = rawValue.Replace(vbCrLf, vbLf).Replace(vbCr, vbLf).Trim()
        Dim parts As String() = normalized.Split(New Char() {ControlChars.Lf}, StringSplitOptions.RemoveEmptyEntries)
        For Each part As String In parts
            Dim candidate As String = part.Trim()
            Dim parsedDate As DateTime
            If DateTime.TryParseExact(candidate,
            New String() {
                "M/d/yyyy",
                "MM/dd/yyyy",
                "M/dd/yyyy",
                "MM/d/yyyy"
            }, Globalization.CultureInfo.GetCultureInfo("en-US"), Globalization.DateTimeStyles.None, parsedDate) Then
                endDate = parsedDate.Date
                Return True
            End If
        Next
        Return False
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

        Dim healthType As String = Normalize(context.HealthType)
        Dim careSetting As String = Normalize(context.CareSetting)
        Dim authorizationStatus As String = Normalize(context.AuthorizationStatus)

        'Expired threshold branch.
        Dim expirationLimit As Integer
        If careSetting = "INPATIENT" Then
            expirationLimit = 90
        ElseIf careSetting = "OUTPATIENT" Then
            expirationLimit = 30
        Else
            result.NextBestAction = "Unable to determine whether the authorization is Inpatient or Outpatient."
            Return result
        End If

        Dim dateToEvaluate As String
        If careSetting = "INPATIENT" Then
            dateToEvaluate = context.AdmissionDate
        Else
            dateToEvaluate = context.AuthorizationEndDate
        End If

        Dim expirationDate As DateTime
        If Not TryGetAuthorizationEndDate(dateToEvaluate, expirationDate) Then
            result.NextBestAction = "Unable to determine the authorization end date."
            Return result

        End If

        Dim expirationWindowEnd As DateTime = expirationDate.AddDays(expirationLimit)
        Dim todayDate As DateTime = Date.Today
        Dim isExpired As Boolean = todayDate >= expirationDate
        Dim isWithinExpirationWindow As Boolean = todayDate <= expirationWindowEnd

        If isExpired And isWithinExpirationWindow Then
            result.NextBestAction = BuildExpiredAuthorizationAction()
            Return result
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

        'Approved or Pended both continue through
        'the authorization update process.
        If IsApprovedStatus(authorizationStatus) Or IsPendingStatus(authorizationStatus) Then
            Return AnalyzeActiveAuthorizationUpdate(context, result, healthType, careSetting)
        End If

        result.NextBestAction = "Unable to determine the Updating Authorization process for status: " & DisplayValue(context.AuthorizationStatus)
        Return result
    End Function
    Private Shared Function AnalyzeActiveAuthorizationUpdate(context As CallContext, result As RecommendationResult, healthType As String, careSetting As String) As RecommendationResult

        'First ask whether clinical review is needed.
        If Not context.ClinicalReviewNeeded.HasValue Then
            result.NextBestAction = BuildUpdatePreparationAction(careSetting, healthType)
            Return AskQuestion(result, "CLINICAL_REVIEW", "Is Clinical Review Needed?", "YES", "NO")
        End If

        'No clinical review needed.
        If Not context.ClinicalReviewNeeded.Value Then
            result.NextBestAction = BuildNoClinicalReviewAction()
            Return result
        End If

        'Clinical review is needed.
        If Not context.IsExpedited.HasValue Then
            Return AskQuestion(result, "EXPEDITED_REQUEST", "Is this an expedited request?", "YES", "NO")
        End If

        'Not expedited.
        If Not context.IsExpedited.Value Then
            result.NextBestAction = FormatActions("Agent will proceed with the standard process.")
            Return result
        End If

        'Expedited requires caller type.
        If String.IsNullOrWhiteSpace(context.CallerType) Then
            Return AskQuestion(result, "CALLER_TYPE", "Is the caller Specialist or PCP?", "SPECIALIST", "PCP")
        End If

        Select Case Normalize(context.CallerType)
            Case "PCP"
                result.NextBestAction = BuildPcpExpeditedAction(careSetting, healthType)
            Case "SPECIALIST"
                result.NextBestAction = BuildSpecialistExpeditedAction(careSetting, healthType)
            Case Else
                result.NextBestAction = "Unable to determine whether the caller is a Specialist or PCP."
        End Select
        Return result
    End Function
    Private Shared Function BuildUpdatePreparationAction(careSetting As String, healthType As String) As String

        Dim actions As New List(Of String)
        If careSetting = "INPATIENT" And healthType = "PHYSICAL HEALTH" Then
            actions.Add("Agent will check possible duplicate case/authorization.")
        Else
            actions.Add("Agent will check if there is a duplicate request.")
        End If
        actions.Add(GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982bc03a9&dl=0&searchID=VI-8d8558edf4ef58a&row=0&mode=Mentor"))
        actions.Add("Agent will ask the caller what they want to update on the authorization.")

        If healthType = "BEHAVIORAL HEALTH" And careSetting = "INPATIENT" Then
            actions.Add("Agent will proceed to the update. Refer to Updating an Existing Authorization")
            actions.Add(GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ff0c9a&dl=0&searchID=VI-8dc4fd9456799a2&row=0&mode=Mentor"))
            actions.Add("Refer to Updating BH Inpatient Authorizations.")
            actions.Add(GuideLink("https://dctm.humana.com/mentor/xweb/ViewTopic.aspx?schronicleID=09000929830e3c5b&searchID=link"))
        Else
            actions.Add("Agent will proceed to the update. Refer to the applicable Updating Medicare Authorization guide.")
            actions.Add(GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982f23fb4&dl=0&searchID=VI-8d871249e0ac098&row=0&mode=Mentor"))
        End If
        Return FormatActions(actions)
    End Function
    Private Shared Function BuildNoClinicalReviewAction() As String
        Return FormatActions(
        "Agent will advise/notify the requestor that the authorization is still approved and provide the applicable disclaimer.",
        GuideLink("https://dctm.humana.com/mentor/xweb/ViewTopic.aspx?schronicleID=090009298315b928&searchID=link"),
        "Agent will provide the reference number, which is the same as the authorization number.",
        "Agent will deliver the closing script and transfer to VOC.",
        GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=090009298a27e33e&dl=0&searchID=VI-8de6a47157e8c45&row=0&mode=Mentor&launchId=1770926270025"),
        "Agent will complete documentation.",
        GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ff0c9a&dl=0&searchID=VI-8d924ecb96a6436&row=0&mode=Mentor"),
        "END OF THE PROCESS.")

    End Function
    Private Shared Function AskQuestion(result As RecommendationResult, questionId As String, questionText As String, ParamArray options() As String) As RecommendationResult

        result.RequiresAgentInput = True
        result.QuestionId = questionId
        result.QuestionText = questionText
        result.QuestionOptions = New List(Of String)(options)
        If String.IsNullOrWhiteSpace(result.NextBestAction) Then
            result.NextBestAction = "Additional information is required."
        End If
        Return result

    End Function
    Private Shared Function BuildExpiredAuthorizationAction() As String

        Return FormatActions(
        "Do not update the authorization.",
        "Redirect the caller to Claims for provider dispute.",
        "Complete the required documentation.",
        GuideLink(
            "https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ed1e15&dl=0&searchID=VI-8d964baa0b8ecb7&row=0&mode=Mentor"),
        "END OF THE PROCESS.")

    End Function
    Private Shared Function BuildDeniedAuthorizationAction() As String

        Return FormatActions(
        "Do not update the authorization.",
        "If it is within 65 days of the denial, redirect the caller to Appeals. Appeal information is included in the denial letter.",
        "If the authorization was denied more than 65 days ago, advise the caller to submit a new authorization.",
        "Complete the required documentation.",
        GuideLink(
            "https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ed1e15&dl=0&searchID=VI-8d964baa0b8ecb7&row=0&mode=Mentor"),
        "END OF THE PROCESS.")

    End Function
    Private Shared Function BuildSpecialistExpeditedAction(careSetting As String, healthType As String) As String

        Dim actions As New List(Of String)
        If healthType = "BEHAVIORAL HEALTH" Then
            actions.Add(GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=09000929877eda0b&dl=0&searchID=VI-8de6a4a0200a0d6&row=0&mode=Mentor&launchId=1770927524924"))
        Else
            actions.Add(GuideLink("https://dctm.humana.com/mentor/xweb/ViewTopic.aspx?schronicleID=0900092987745d15&searchID=link"))
        End If

        actions.Add("Request the required clinical information.")
        actions.Add(GuideLink("https://dctm.humana.com/mentor/web/v.aspx/CO-Overview%20Common%20Records%20Request%20Verbiage?chronicleID=090009298687b587&dl=0&searchID=wdkLink"))
        If healthType = "BEHAVIORAL HEALTH" Then
            actions.Add("Request clinicals to be faxed to 469-913-6941 and check the BH Expedited queue assignment.")
            actions.Add("Provide the applicable turnaround timeframe.")
            actions.Add(GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982b310f5&dl=0&searchID=VI-8de6a48b4b09948&row=0&mode=Mentor&launchId=1770926965607"))
            actions.Add("Enter the NRD as the next business day.")
            actions.Add("Obtain and document the following in Free Text Notes: Reason for Admission, Discharge Plan, Discharge Planner Name, Phone Number, Concurrent UR Contact Name, Phone Number, and Fax Number.")

        ElseIf careSetting = "OUTPATIENT" Then
            actions.Add("Provide fax number 1-888-200-7440. For Genetic requests, use 855-227-0677.")
            actions.Add("Manually queue the authorization to CIT ALL Medicare Expedited. For Genetic requests, use Genetic Expedited.")
            actions.Add("Provide the applicable turnaround timeframe.")
            actions.Add(GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982b310f5&dl=0&searchID=VI-8de26a7262643f8&row=0&mode=Mentor&launchId=1763490851475"))
            actions.Add("Provide the reference number, which is the same as the authorization number.")
        Else
            actions.Add("Provide the applicable fax number.")
            actions.Add(GuideLink("https://dctm.humana.com/Mentor/Web/v.aspx/HSO%20Regional%20Map%20and%20Contact%20Information?chronicleID=0900092980bc4220&dl=0&searchID=link&row=0"))
            actions.Add("Check the applicable queue assignment.")
            actions.Add(GuideLink("https://dctm.humana.com/Mentor/Web/v.aspx/ClinicalDirectory?chronicleID=09000929813a2f13&dl=1&searchID=VI-8dd33d5130cee9d&row=0"))
            actions.Add("Provide the applicable turnaround timeframe.")
            actions.Add(GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982b310f5&dl=0&searchID=VI-8de26a7262643f8&row=0&mode=Mentor&launchId=1763490851475"))
            actions.Add("Provide the reference number, which is the same as the authorization number.")
        End If

        actions.Add("Deliver the closing script and transfer to VOC.")
        actions.Add(GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=090009298a27e33e&dl=0&searchID=VI-8de6a47157e8c45&row=0&mode=Mentor&launchId=1770926270025"))
        actions.Add("Complete the required documentation.")
        actions.Add(GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ed1e15&dl=0&searchID=VI-8d964baa0b8ecb7&row=0&mode=Mentor"))
        actions.Add("END OF THE PROCESS.")
        Return FormatActions(actions)
    End Function
    Private Shared Function BuildPcpExpeditedAction(careSetting As String, healthType As String) As String
        Dim actions As New List(Of String)
        actions.Add("Proceed with expediting the authorization.")
        actions.Add(GuideLink("https://dctm.humana.com/mentor/xweb/ViewTopic.aspx?schronicleID=0900092987745d15&searchID=link"))
        actions.Add("Request the required clinical information.")
        actions.Add(GuideLink("https://dctm.humana.com/mentor/web/v.aspx/CO-Overview%20Common%20Records%20Request%20Verbiage?chronicleID=090009298687b587&dl=0&searchID=wdkLink"))

        If healthType = "BEHAVIORAL HEALTH" Then
            actions.Add("Provide fax number 469-913-6941 and check the applicable queue assignment.")
        Else
            actions.Add("Provide the applicable fax number.")
            actions.Add(GuideLink("https://dctm.humana.com/Mentor/Web/v.aspx/HSO%20Regional%20Map%20and%20Contact%20Information?chronicleID=0900092980bc4220&dl=0&searchID=link&row=0"))
            actions.Add("Check the applicable queue assignment.")
            actions.Add(GuideLink("https://dctm.humana.com/Mentor/Web/v.aspx/ClinicalDirectory?chronicleID=09000929813a2f13&dl=1&searchID=VI-8dd33d5130cee9d&row=0"))
        End If

        actions.Add("Provide the applicable turnaround timeframe.")
        actions.Add(GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982b310f5&dl=0&searchID=VI-8de26a7262643f8&row=0&mode=Mentor&launchId=1763490851475"))
        actions.Add("Provide the reference number, which is the same as the authorization number.")
        actions.Add("Deliver the closing script and transfer to VOC.")
        actions.Add(GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=090009298a27e33e&dl=0&searchID=VI-8de6a47157e8c45&row=0&mode=Mentor&launchId=1770926270025"))
        actions.Add("Complete the required documentation.")
        actions.Add(GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ed1e15&dl=0&searchID=VI-8d964baa0b8ecb7&row=0&mode=Mentor"))
        actions.Add("END OF THE PROCESS.")

        Return FormatActions(actions)
    End Function
    Private Shared Function FormatActions(ParamArray actions() As String) As String
        Return FormatActions(CType(actions, IEnumerable(Of String)))
    End Function
    Private Shared Function GuideLink(url As String) As String
        If String.IsNullOrWhiteSpace(url) Then
            Return String.Empty
        End If

        Return "[LINK]Open Guide|" & url.Trim()
    End Function
    Private Shared Function FormatActions(actions As IEnumerable(Of String)) As String

        Dim output As New StringBuilder()
        'output.AppendLine("NEXT BEST ACTION")
        'output.AppendLine(New String("-"c, 45))
        'output.AppendLine()

        Dim stepNumber As Integer = 1
        For Each actionText As String In actions
            If String.IsNullOrWhiteSpace(actionText) Then
                Continue For
            End If

            Dim cleanText As String = actionText.Trim()
            If cleanText.StartsWith("[LINK]", StringComparison.OrdinalIgnoreCase) Then
                output.AppendLine(cleanText)
                'output.AppendLine()

            ElseIf String.Equals(cleanText, "END OF THE PROCESS.", StringComparison.OrdinalIgnoreCase) Then
                output.AppendLine("✓ END OF THE PROCESS")
                'output.AppendLine()
            Else
                output.AppendLine(stepNumber.ToString() & ". " & cleanText)
                'output.AppendLine()
                stepNumber += 1
            End If
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
        Return normalizedStatus.Contains("PENDING") Or normalizedStatus.Contains("PENDED") Or normalizedStatus.Contains("PEND")
    End Function
    Private Shared Function AnalyzeNonApprovedAuthorizationStatus(context As CallContext, result As RecommendationResult) As RecommendationResult

        result.NextBestAction = FormatActions(
            "Agent will provide the status and disclaims (if applicable).",
            GuideLink(
                "https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=090009298315b927&dl=0&searchID=VI-8d98fec070a0cb4&row=0&mode=Mentor"))

        If Not context.PendingClinicalReview.HasValue Then
            Return AskQuestion(result, "PENDING_CLINICAL_REVIEW", "Is the auth request pending for clinical review?", "YES", "NO")
        End If

        If context.PendingClinicalReview.Value Then
            Return AnalyzePendingAuthorizationStatus(context, result)
        End If

        Return AnalyzeDeniedAuthorizationStatus(context, result)
    End Function
    Private Shared Function AnalyzeApprovedAuthorizationStatus(context As CallContext, result As RecommendationResult) As RecommendationResult

        result.NextBestAction = FormatActions("Agent will notify the requestor of the Approval Determination.",
            GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982f0e2da&dl=0&searchID=VI-8d869339f5da589&row=0&mode=Mentor")
        )

        If Not context.ProviderRequestingApprovedAuthCopy.HasValue Then
            Return AskQuestion(result, "REQUEST_APPROVED_AUTH_COPY", "Is the provider requesting for a copy of approved authorization?", "YES", "NO")
        End If

        If Not context.ProviderRequestingApprovedAuthCopy.Value Then
            result.NextBestAction = BuildCheckStatusClosingAction()
            Return result
        End If

        If Not context.ProviderRequestingLoaCopy.HasValue Then
            Return AskQuestion(result, "REQUEST_LOA_COPY", "Is the provider requesting a copy of the LOA?", "YES", "NO")
        End If

        If context.ProviderRequestingLoaCopy.Value Then
            result.NextBestAction = FormatActions("Agent will access the LOA coversheet.",
                                                  "Agent will fill out/complete the section 1.",
                                                  GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=090009298a4960a1&dl=0&searchID=VI-8de6a468b581890&row=0&mode=Mentor&launchId=1770926037812"),
                                                  "Agent will process and send it thru email to the Director of Contracting aligned to the region.",
                                                  GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092980cb8e51&dl=0&searchID=link&row=0&mode=Mentor&launchId=1770917691831"),
                                                  "Agent will deliver the closing script and offer VOC.",
                                                  GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=090009298a27e33e&dl=0&searchID=VI-8de6a47157e8c45&row=0&mode=Mentor&launchId=1770926270025"),
                                                  "Agent will complete documentation.",
                                                  GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ed1e15&dl=0&searchID=VI-8d964baa0b8ecb7&row=0&mode=Mentor"),
                                                  "END OF THE PROCESS."
            )
            Return result
        End If

        result.NextBestAction = FormatActions("Agent will fill out Fax to CGX.",
                                              GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982a46d2d&dl=0&searchID=VI-8de6a3331f81552&row=0&mode=Mentor&launchId=1770917730574"),
                                              "Agent will fill out the needed details and send the fax.",
                                              GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982a46d2d&dl=0&searchID=VI-8de6a3331f81552&row=0&mode=Mentor&launchId=1770917730574"),
                                              "Agent will deliver the closing script and offer VOC.",
                                              GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=090009298a27e33e&dl=0&searchID=VI-8de6a47157e8c45&row=0&mode=Mentor&launchId=1770926270025"),
                                              "Agent will complete documentation.",
                                              GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ed1e15&dl=0&searchID=VI-8d964baa0b8ecb7&row=0&mode=Mentor"),
                                              "END OF THE PROCESS."
        )
        Return result
    End Function
    Private Shared Function AnalyzePendingAuthorizationStatus(context As CallContext, result As RecommendationResult) As RecommendationResult
        If Not context.NeedsClinicalAdvisor.HasValue Then
            Return AskQuestion(result, "NEEDS_CLINICAL_ADVISOR", "Is there a need to speak with a Clinical Advisor regarding determination P2P, LOA, verbal clinical, adverse determination, or an inquiry about HBH subsequent review?", "YES", "NO")
        End If

        If context.NeedsClinicalAdvisor.Value Then
            result.NextBestAction = FormatActions(
                    "Agent will triage a request for transfer to a clinical advisor.",
                    "Agent will transfer the caller to a Clinician.",
                    GuideLink("https://dctm.humana.com/mentor/xweb/ViewTopic.aspx?schronicleID=090009298305e52c&searchID=link"),
                    "Agent will deliver the closing script and offer VOC.",
                    GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=090009298a27e33e&dl=0&searchID=VI-8de6a47157e8c45&row=0&mode=Mentor&launchId=1770926270025"),
                    "Agent will complete documentation.",
                    GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ed1e15&dl=0&searchID=VI-8d964baa0b8ecb7&row=0&mode=Mentor"),
                    "END OF THE PROCESS.")
            Return result
        End If

        If Not context.ClinicalAttached.HasValue Then
            Return AskQuestion(result, "CLINICAL_ATTACHED", "Is there a clinical attached to the authorization?", "YES", "NO")
        End If

        If context.ClinicalAttached.Value Then
            result.NextBestAction = FormatActions(
                    "Do not request clinicals.",
                    "Agent will deliver the closing script and offer VOC.",
                    GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=090009298a27e33e&dl=0&searchID=VI-8de6a47157e8c45&row=0&mode=Mentor&launchId=1770926270025"),
                    "Agent will complete documentation.",
                    GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ed1e15&dl=0&searchID=VI-8d964baa0b8ecb7&row=0&mode=Mentor"),
                    "END OF THE PROCESS.")
            Return result
        End If

        result.NextBestAction = FormatActions(
            "Agent needs to request clinicals.",
            GuideLink("https://dctm.humana.com/mentor/web/v.aspx/CO-Overview%20Common%20Records%20Request%20Verbiage?chronicleID=090009298687b587&dl=0&searchID=wdkLink"),
                "Agent will provide the applicable fax number.",
                GuideLink("https://dctm.humana.com/Mentor/Web/v.aspx/ClinicalDirectory?chronicleID=09000929813a2f13&dl=1&searchID=VI-8dd33d5130cee9d&row=0"),
                "Agent will provide the determination timeframe.",
                GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982b310f5&dl=0&searchID=VI-8de6a48b4b09948&row=0&mode=Mentor&launchId=1770926965607"),
                "Agent will deliver the closing script and offer VOC.",
                GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=090009298a27e33e&dl=0&searchID=VI-8de6a47157e8c45&row=0&mode=Mentor&launchId=1770926270025"),
                "Agent will complete documentation.",
                GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ed1e15&dl=0&searchID=VI-8d964baa0b8ecb7&row=0&mode=Mentor"),
                "END OF THE PROCESS.")
        Return result
    End Function
    Private Shared Function AnalyzeDeniedAuthorizationStatus(context As CallContext, result As RecommendationResult) As RecommendationResult

        result.NextBestAction = FormatActions("Agent will provide the authorization status and applicable disclaimer.")
        If Not context.RequestingDenialLetter.HasValue Then
            Return AskQuestion(result, "REQUEST_DENIAL_LETTER", "Is the caller asking for the denial letter?", "YES", "NO")
        End If

        If context.RequestingDenialLetter.Value Then
            result.NextBestAction = FormatActions(
                    "Agent will check whether there is a denial letter on the Letters tab.",
                    "Agent will send the information to the TL via email",
                    "Advise the caller that they will receive the letter within the day.",
                    "Agent will deliver the closing script and offer VOC.",
                    GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=090009298a27e33e&dl=0&searchID=VI-8de6a47157e8c45&row=0&mode=Mentor&launchId=1770926270025"),
                    "Agent will complete documentation.",
                    GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ed1e15&dl=0&searchID=VI-8d964baa0b8ecb7&row=0&mode=Mentor"),
                    "END OF THE PROCESS.")
            Return result
        End If
        result.NextBestAction = BuildCheckStatusClosingAction()
        Return result
    End Function
    Private Shared Function BuildCheckStatusClosingAction() As String
        Return FormatActions(
            "Agent will deliver the closing script and offer VOC.",
            GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=090009298a27e33e&dl=0&searchID=VI-8de6a47157e8c45&row=0&mode=Mentor&launchId=1770926270025"),
            "Agent will complete documentation.",
            GuideLink("https://dctm.humana.com/Mentor/xWeb/viewtopic.aspx?sChronicleID=0900092982ed1e15&dl=0&searchID=VI-8d964baa0b8ecb7&row=0&mode=Mentor"),
            "END OF THE PROCESS.")
    End Function
End Class