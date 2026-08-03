Imports System.Text

Public NotInheritable Class RecommendationEngine

    Private Sub New()
    End Sub

    Public Shared Function Analyze(context As CallContext, lookups As LookupResult) As RecommendationResult

        Dim result As New RecommendationResult()
        result.OverallOutput = BuildOverallOutput(context, lookups)

        If Not String.Equals(context.Scenario, "UPDATING AUTHORIZATION", StringComparison.OrdinalIgnoreCase) Then
            result.NextBestAction = "This scenario has not yet been configured."
            Return result
        End If

        Dim clinicalReviewNeeded As Boolean = String.Equals(context.AuthorizationStatus, "PENDING", StringComparison.OrdinalIgnoreCase)
        If Not clinicalReviewNeeded Then
            result.NextBestAction = "Clinical review is not required based on the current authorization status."
            Return result
        End If

        If Not context.IsExpedited.HasValue Then
            result.RequiresAgentInput = True
            result.QuestionId = "EXPEDITED_REQUEST"
            result.QuestionText = "Is this an expedited request?"
            result.QuestionOptions = New List(Of String) From {"YES", "NO"}
            result.NextBestAction = "Clinical review is required."
            Return result
        End If

        If Not context.IsExpedited.Value Then
            result.NextBestAction = BuildStandardAction(lookups)
            Return result
        End If

        If String.IsNullOrWhiteSpace(context.CallerType) Then
            result.RequiresAgentInput = True
            result.QuestionId = "CALLER_TYPE"
            result.QuestionText = "Is the caller a Specialist or PCP?"
            result.QuestionOptions = New List(Of String) From {"SPECIALIST", "PCP"}
            result.NextBestAction = "Expedited request selected."
            Return result
        End If

        result.NextBestAction = BuildExpeditedAction(context, lookups)
        Return result
    End Function

    Private Shared Function BuildOverallOutput(context As CallContext, lookup As LookupResult) As String

        Dim output As New StringBuilder()

        output.AppendLine("CALL INFORMATION")
        output.AppendLine(New String("-"c, 40))

        output.AppendLine("Caller: " & (context.CallerFirstName & " " & context.CallerLastName).Trim())
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
End Class