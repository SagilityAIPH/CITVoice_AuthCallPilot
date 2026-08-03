Imports System.Data.SQLite
Imports System.Linq
Public NotInheritable Class CallPilotRepository

    Private Sub New()
    End Sub

    Public Shared Function RunLookups(context As CallContext) As LookupResult

        If context Is Nothing Then
            Throw New ArgumentNullException(NameOf(context))
        End If

        Dim result As New LookupResult()
        Using connection As SQLiteConnection = DatabaseManager.GetConnection()
            connection.Open()
            CheckOutOfScope(connection, context, result)
            CheckMarketGuide(connection, context, result)
            CheckPal(connection, context, result)
        End Using
        Return result

    End Function

    Private Shared Sub CheckOutOfScope(connection As SQLiteConnection, context As CallContext, result As LookupResult)

        If String.IsNullOrWhiteSpace(context.GroupNumber) Then

            result.IsOutOfScope = Nothing
            result.OutOfScopeMessage = "Group number is unavailable."

            Return
        End If

        Const sql As String = "SELECT NumGroup, GroupStat, TypeofRestriction FROM PALAndOOS_tbOOS WHERE NumGroup = @groupNumber;"

        Using command As New SQLiteCommand(sql, connection)

            command.Parameters.AddWithValue("@groupNumber", context.GroupNumber.Trim())

            Using reader As SQLiteDataReader = command.ExecuteReader()

                Dim foundActive As Boolean = False

                While reader.Read()

                    Dim status As String = SafeGet(reader, "GroupStat")

                    If String.Equals(status, "Active", StringComparison.OrdinalIgnoreCase) Then

                        foundActive = True
                        result.IsOutOfScope = True
                        result.RestrictionType = SafeGet(reader, "TypeofRestriction")

                        result.OutOfScopeMessage = context.GroupNumber & " - Active OOS group."

                        Exit While
                    End If

                End While

                If Not foundActive Then
                    result.IsOutOfScope = False
                    result.OutOfScopeMessage = context.GroupNumber & " - Not an active OOS group."
                End If

            End Using
        End Using

    End Sub

    Private Shared Sub CheckMarketGuide(connection As SQLiteConnection, context As CallContext, result As LookupResult)

        Dim product As String = Normalize(context.Product)

        If product <> "MEDICARE HMO" AndAlso product <> "MEDICARE MRO" Then
            result.MarketGuideFound = False
            result.MarketGuideMessage = "Market Guide lookup is not applicable for this product."
            Return
        End If

        If String.IsNullOrWhiteSpace(context.Conso) Then
            result.MarketGuideMessage = "Conso value is unavailable."
            Return
        End If

        Dim cleanConso As String = RemoveSpacesAndHyphens(context.Conso)

        Const sql As String = "SELECT AuthRef1, AuthRef2, AuthRef3, RefModel1, RefModel2, RefModel3 " &
            "FROM MarketGuide_tblMarketGuide " &
            "WHERE instr(upper(replace(replace(CSM, '-', ''), ' ', '')), @conso) > 0;"

        Using command As New SQLiteCommand(sql, connection)
            command.Parameters.AddWithValue("@conso", cleanConso)
            Using reader As SQLiteDataReader = command.ExecuteReader()
                Dim cleanState As String = Normalize(context.IssueState)
                While reader.Read()

                    Dim authRef1 As String = SafeGet(reader, "AuthRef1")
                    Dim authRef2 As String = SafeGet(reader, "AuthRef2")
                    Dim authRef3 As String = SafeGet(reader, "AuthRef3")
                    Dim refModel1 As String = SafeGet(reader, "RefModel1")
                    Dim refModel2 As String = SafeGet(reader, "RefModel2")
                    Dim refModel3 As String = SafeGet(reader, "RefModel3")

                    If Normalize(authRef1).Contains(cleanState) Then
                        result.MarketGuideReference = refModel1
                    ElseIf Normalize(authRef2).Contains(cleanState) Then
                        result.MarketGuideReference = refModel2
                    ElseIf Normalize(authRef3).Contains(cleanState) Then
                        result.MarketGuideReference = refModel3
                    Else
                        result.MarketGuideReference = refModel1
                    End If

                    result.MarketGuideFound = Not String.IsNullOrWhiteSpace(result.MarketGuideReference)

                    result.MarketGuideMessage =
                        If(result.MarketGuideFound, result.MarketGuideReference, "No matching Market Guide found.")
                    Exit While
                End While
            End Using
        End Using

    End Sub
    Private Shared Sub CheckPal(connection As SQLiteConnection, context As CallContext, result As LookupResult)
        If context.ProcedureCodes Is Nothing OrElse context.ProcedureCodes.Count = 0 Then

            result.PalFound = False
            Return
        End If

        For Each procedureCode As String In context.ProcedureCodes

            If String.IsNullOrWhiteSpace(procedureCode) Then
                Continue For
            End If

            Const sql As String = "SELECT PALMedHMO, PALMedPPO, PALMedPFFS, PALResponseCode " &
                "FROM PALAndOOS_tbPAL " &
                "WHERE PALCode = @palCode;"

            Using command As New SQLiteCommand(sql, connection)

                command.Parameters.AddWithValue("@palCode", procedureCode.Trim())

                Using reader As SQLiteDataReader = command.ExecuteReader()

                    While reader.Read()
                        Dim palText As String = GetPalTextByProduct(reader, context.Product)
                        Dim responseCode As String = SafeGet(reader, "PALResponseCode")
                        AddPalClassification(result, palText, responseCode)
                    End While
                End Using
            End Using

        Next

        result.PalFound = result.PalResults.Count > 0

        If Not result.PalFound Then
            result.PalResults.Add("No matching PAL result found.")
        End If
    End Sub

    Private Shared Function GetPalTextByProduct(
        reader As SQLiteDataReader,
        product As String
    ) As String

        Select Case Normalize(product)

            Case "MEDICARE HMO",
                 "MEDICARE MRO"

                Return SafeGet(reader, "PALMedHMO")

            Case "MEDICARE PPO"

                Return SafeGet(reader, "PALMedPPO")

            Case "MEDICARE PFFS"

                Return SafeGet(reader, "PALMedPFFS")

            Case Else
                Return String.Empty

        End Select

    End Function

    Private Shared Sub AddPalClassification(result As LookupResult, palText As String, responseCode As String)
        Dim normalizedPal As String = NormalizePalText(palText)
        Dim normalizedResponse As String = NormalizePalText(responseCode)
        If normalizedPal.Contains("REQUIRED") And normalizedPal.Contains("CLINICAL") And normalizedPal.Contains("MEDICAL NECESSITY REVIEW") Then
            AddUnique(result.PalResults, "Required - Clinical Medical Necessity Review")
        ElseIf normalizedPal.Contains("NON-CLINICAL") And normalizedPal.Contains("PREAUTH: REQUIRED") Then
            AddUnique(result.PalResults, "Non-Clinical - Preauth Required")
        ElseIf normalizedPal.Contains("COHERE") OrElse normalizedResponse.Contains("COHERE") Then
            AddUnique(result.PalResults, "Follow current process for transferring to Cohere")
        ElseIf normalizedPal.Contains("EVOLENT") OrElse normalizedResponse.Contains("EVOLENT") Then
            AddUnique(result.PalResults, "Follow current process for transferring to Evolent")
        End If

    End Sub

    Private Shared Sub AddUnique(values As List(Of String), value As String)
        If Not values.Any(
            Function(existingValue)
                Return String.Equals(existingValue, value, StringComparison.OrdinalIgnoreCase)
            End Function) Then

            values.Add(value)
        End If
    End Sub

    Private Shared Function SafeGet(reader As SQLiteDataReader, columnName As String) As String

        Dim ordinal As Integer
        Try
            ordinal = reader.GetOrdinal(columnName)
        Catch
            Return String.Empty
        End Try

        If reader.IsDBNull(ordinal) Then
            Return String.Empty
        End If

        Return Convert.ToString(reader.GetValue(ordinal)).Trim()

    End Function

    Private Shared Function Normalize(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then
            Return String.Empty
        End If
        Return value.Trim().ToUpperInvariant()
    End Function

    Private Shared Function NormalizePalText(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then
            Return String.Empty
        End If
        Return value.Replace(vbCr, " ").Replace(vbLf, " ").Trim().ToUpperInvariant()
    End Function

    Private Shared Function RemoveSpacesAndHyphens(value As String) As String
        Return Normalize(value).Replace("-", "").Replace(" ", "")
    End Function

End Class