Imports System.Linq
Imports System.Text

Public NotInheritable Class OutputFormatter

    Private Sub New()
    End Sub

    Public Shared Function BuildMemberInformation(context As CallContext) As String
        If context Is Nothing Then
            Return String.Empty
        End If

        Dim output As New StringBuilder()
        output.AppendLine("Member ID: " & DisplayValue(context.MemberId))
        output.AppendLine("Member Name: " & DisplayValue(context.MemberName))
        output.AppendLine("Date of Birth: " & DisplayValue(context.DateOfBirth))
        output.AppendLine()
        output.AppendLine("Product: " & DisplayValue(context.Product))
        output.AppendLine("Consolidated Selling Market: " & DisplayValue(context.Conso))
        output.AppendLine("Group Number: " & DisplayValue(context.GroupNumber))
        output.AppendLine("Grouper ID: " & DisplayValue(context.GrouperId))
        output.AppendLine("State of Issue: " & DisplayValue(context.IssueState))
        Return output.ToString().TrimEnd()
    End Function

    Public Shared Function BuildAuthorizationInformation(context As CallContext) As String
        If context Is Nothing Then
            Return String.Empty
        End If

        Dim output As New StringBuilder()
        output.AppendLine("Authorization ID: " & DisplayValue(context.AuthorizationNumber))
        output.AppendLine("Authorization Status: " & DisplayValue(context.AuthorizationStatus))
        output.AppendLine()
        If String.Equals(context.CareSetting, "INPATIENT", StringComparison.OrdinalIgnoreCase) Then
            output.AppendLine("Admission Date: " & DisplayValue(context.AdmissionDate))
            output.AppendLine("Discharge Date: " & DisplayValue(context.DischargeDate))
        Else
            output.AppendLine("Start Date: " & DisplayValue(context.AuthorizationStartDate))
            output.AppendLine("End Date: " & DisplayValue(context.AuthorizationEndDate))
        End If
        output.AppendLine("Total Days: " & DisplayValue(context.TotalDays))
        output.AppendLine()
        output.AppendLine("Requesting Provider:")
        output.AppendLine(DisplayValue(context.RequestingProvider))
        output.AppendLine()
        output.AppendLine("Treating Provider:")
        output.AppendLine(DisplayValue(context.TreatingProvider))
        output.AppendLine()
        output.AppendLine("Facility Provider:")
        output.AppendLine(DisplayValue(context.FacilityProvider))
        output.AppendLine()
        output.AppendLine("Primary Diagnosis: " & DisplayValue(context.PrimaryDiagnosisCode))
        output.AppendLine("Secondary Diagnoses: " & JoinValues(context.SecondaryDiagnosisCodes))
        output.AppendLine("Procedure Codes: " & JoinValues(context.ProcedureCodes))



        Return output.ToString().TrimEnd()
    End Function

    Public Shared Function BuildOutOfScope(lookup As LookupResult) As String
        If lookup Is Nothing Then
            Return String.Empty
        End If

        Dim output As New StringBuilder()
        output.AppendLine("Out of Scope: " & FormatNullableBoolean(lookup.IsOutOfScope))

        If Not String.IsNullOrWhiteSpace(lookup.OutOfScopeMessage) Then
            output.AppendLine()
            output.AppendLine(lookup.OutOfScopeMessage.Trim())
        End If

        If Not String.IsNullOrWhiteSpace(lookup.RestrictionType) Then
            output.AppendLine()
            output.AppendLine("Restriction: " & lookup.RestrictionType.Trim())
        End If
        Return output.ToString().TrimEnd()
    End Function
    Public Shared Function BuildMarketGuide(lookup As LookupResult) As String
        If lookup Is Nothing Then
            Return String.Empty
        End If

        Dim output As New Text.StringBuilder()
        output.AppendLine("Found: " & If(lookup.MarketGuideFound, "YES", "NO"))
        If Not lookup.MarketGuideFound Then
            If Not String.IsNullOrWhiteSpace(lookup.MarketGuideMessage) Then
                output.AppendLine()
                output.AppendLine(
                lookup.MarketGuideMessage.Trim())
            End If
            Return output.ToString().TrimEnd()
        End If


        Dim reference As String = If(lookup.MarketGuideReference, String.Empty).Trim()
        Dim message As String = If(lookup.MarketGuideMessage, String.Empty).Trim()

        'Show reference
        If Not String.IsNullOrWhiteSpace(reference) Then
            output.AppendLine()
            output.AppendLine(reference)
        End If

        'Only show message if it contains different information
        If Not String.IsNullOrWhiteSpace(message) And Not String.Equals(NormalizeMarketGuideText(reference), NormalizeMarketGuideText(message), StringComparison.OrdinalIgnoreCase) Then
            output.AppendLine()
            output.AppendLine(message)
        End If
        Return output.ToString().TrimEnd()
    End Function
    Private Shared Function NormalizeMarketGuideText(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then
            Return String.Empty
        End If

        Return String.Join(" ", value.Replace(vbCr, " ").Replace(vbLf, " ").Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)).Trim().ToUpperInvariant()
    End Function
    Public Shared Function BuildPal(context As CallContext, lookup As LookupResult) As String
        If lookup Is Nothing Then
            Return String.Empty
        End If

        Dim output As New StringBuilder()
        output.AppendLine("Found: " & If(lookup.PalFound, "YES", "NO"))
        '========================================
        ' ALL PX CODES FROM CGX
        '========================================
        If context Is Nothing OrElse context.ProcedureCodes Is Nothing OrElse context.ProcedureCodes.Count = 0 Then
            output.AppendLine("Procedure Codes: Not yet extracted from CGX")
        Else
            output.AppendLine("Procedure Codes: " & String.Join(", ", context.ProcedureCodes))
        End If
        '========================================
        ' PX CODES THAT HAVE PAL
        '========================================
        If lookup.PalMatchedProcedureCodes Is Nothing OrElse lookup.PalMatchedProcedureCodes.Count = 0 Then
            output.AppendLine("Found PX: None")
        Else
            output.AppendLine("Found PX: " & String.Join(", ", lookup.PalMatchedProcedureCodes))
        End If
        '========================================
        ' PAL RESULTS
        '========================================
        output.AppendLine()
        If lookup.PalResults Is Nothing OrElse lookup.PalResults.Count = 0 Then
            output.AppendLine("No matching PAL result found.")
        Else
            For Each palResult As String In lookup.PalResults
                If Not String.IsNullOrWhiteSpace(palResult) Then
                    output.AppendLine("• " & palResult.Trim())
                End If
            Next
        End If
        Return output.ToString().TrimEnd()
    End Function
    Public Shared Function BuildDocumentation(context As CallContext) As String
        If context Is Nothing Then
            Return String.Empty
        End If

        Dim output As New StringBuilder()
        output.AppendLine("Name: " & DocumentationValue(context.CallerName))
        output.AppendLine("Direct #: " & DocumentationValue(context.CallbackNumber))
        output.AppendLine("Secured Fax: " & DocumentationValue(context.SecuredFax))

        Dim callbackDisplay As String = If(context.CallbackNumber, String.Empty).Trim()
        Dim extension As String = If(context.Extension, String.Empty).Trim()
        If Not String.IsNullOrWhiteSpace(extension) Then
            callbackDisplay &= " Ext. " & extension
        End If
        output.AppendLine("Direct #: " & callbackDisplay)

        output.AppendLine("Calling From: " & DocumentationValue(context.CallingFrom))
        output.AppendLine()
        output.AppendLine("Member ID: " & DocumentationValue(context.MemberId))
        output.AppendLine("Date of Birth: " & DocumentationValue(context.DateOfBirth))
        output.AppendLine()
        output.AppendLine("Genesys Verification: " & If(context.GenesysVerified, "Yes", "No"))
        output.AppendLine("Provider/Member Authenticated: " & If(context.ProviderMemberAuthenticated, "Yes", "No"))
        output.AppendLine("Mailing Address Verified: " & If(context.MailingAddressVerified, "Yes", "No"))
        output.AppendLine()
        output.AppendLine("Concern: " & DocumentationValue(context.Scenario))
        output.AppendLine("Date of Service: " & FormatDate(context.DateOfService))
        output.AppendLine()
        output.AppendLine("Authorization ID: " & DocumentationValue(context.AuthorizationNumber))
        output.AppendLine()
        output.AppendLine("Requesting Provider: " & DocumentationValue(context.RequestingProvider))
        output.AppendLine()
        output.AppendLine("Treating Provider: " & DocumentationValue(context.TreatingProvider))
        output.AppendLine()
        output.AppendLine("Facility Provider: " & DocumentationValue(context.FacilityProvider))
        output.AppendLine()
        output.AppendLine("DX: " & BuildDiagnosisText(context))
        output.AppendLine("PX: " & JoinValues(context.ProcedureCodes, String.Empty))
        output.AppendLine()
        output.AppendLine("Addt'l Notes: " & DocumentationValue(context.ClaimPaymentNotes))
        output.AppendLine(Environment.UserName & " / ManilaCIT")

        Return output.ToString().TrimEnd()
    End Function

    Private Shared Function BuildDiagnosisText(context As CallContext) As String

        Dim codes As New List(Of String)
        If Not String.IsNullOrWhiteSpace(context.PrimaryDiagnosisCode) Then
            codes.Add(context.PrimaryDiagnosisCode.Trim())
        End If

        If context.SecondaryDiagnosisCodes IsNot Nothing Then
            For Each code As String In
                context.SecondaryDiagnosisCodes
                If String.IsNullOrWhiteSpace(code) Then
                    Continue For
                End If
                If Not codes.Any(
                    Function(existing)
                        Return String.Equals(
                            existing,
                            code.Trim(),
                            StringComparison.OrdinalIgnoreCase)
                    End Function) Then

                    codes.Add(code.Trim())
                End If
            Next
        End If
        Return String.Join(", ", codes)
    End Function
    Private Shared Function JoinValues(values As IEnumerable(Of String), Optional emptyText As String = "Not found") As String
        If values Is Nothing Then
            Return emptyText
        End If

        Dim cleanedValues As List(Of String) =
            values.
                Where(
                    Function(value)
                        Return Not String.IsNullOrWhiteSpace(
                            value)
                    End Function).
                Select(
                    Function(value)
                        Return value.Trim()
                    End Function).
                Distinct(
                    StringComparer.OrdinalIgnoreCase).
                ToList()

        If cleanedValues.Count = 0 Then
            Return emptyText
        End If
        Return String.Join(", ", cleanedValues)
    End Function

    Private Shared Function DisplayValue(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then
            Return "Not found"
        End If
        Return value.Trim()
    End Function
    Private Shared Function DocumentationValue(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then
            Return String.Empty
        End If
        Return value.Trim()
    End Function
    Private Shared Function FormatNullableBoolean(value As Boolean?) As String
        If Not value.HasValue Then
            Return "Unable to determine"
        End If
        Return If(value.Value, "YES", "NO")
    End Function
    Private Shared Function FormatDate(value As DateTime?) As String
        If Not value.HasValue Then
            Return String.Empty
        End If

        Return value.Value.ToString("MM/dd/yyyy")
    End Function

End Class