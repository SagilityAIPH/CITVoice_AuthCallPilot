Imports System.Data.SQLite
Imports System.IO

Public NotInheritable Class TrackingDatabaseManager

    Private Sub New()
    End Sub

    'Public Shared ReadOnly Property DatabasePath As String
    '    Get
    '        'Local Laptop
    '        'Return "D:\Office Works\VS\CITVoice_AuthCallPilot\AuthCallPilot\Database\CallPilotTracking.db"
    '        'Live
    '        Return "X:\HGSL CIT Faxes\13_CIT - Web Queue - EOD\Client\CallPilot\CallPilotTracking.db"
    '    End Get
    'End Property
    Public Shared ReadOnly Property DatabasePath As String
        Get
            Return AppSession.TrackingDatabasePath
        End Get
    End Property

    Public Shared Function GetConnection() As SQLiteConnection
        If String.IsNullOrWhiteSpace(DatabasePath) Then Throw New InvalidOperationException("No tracking database has been selected.")
        If Not File.Exists(DatabasePath) Then Throw New FileNotFoundException("CallPilot tracking database was not found.", DatabasePath)
        Return New SQLiteConnection("Data Source=" & DatabasePath & ";Version=3;FailIfMissing=True;Busy Timeout=10000;")
    End Function
    Public Shared Sub SaveTracking(userName As String, startTime As DateTime, endTime As DateTime, memberId As String, authNumber As String, concern As String, providerDetails As String, questionAnswers As String, callNotes As String)

        Using connection As SQLiteConnection = GetConnection()
            connection.Open()

            Const sql As String = "INSERT INTO CallTracking (UserName, StartTime, EndTime, MemberId, AuthNumber, Concern, ProviderDetails, QuestionAnswers, CallNotes) VALUES (@UserName, @StartTime, @EndTime, @MemberId, @AuthNumber, @Concern, @ProviderDetails, @QuestionAnswers, @CallNotes);"

            Using command As New SQLiteCommand(sql, connection)
                command.Parameters.AddWithValue("@UserName", userName)
                command.Parameters.AddWithValue("@StartTime", startTime)
                command.Parameters.AddWithValue("@EndTime", endTime)
                command.Parameters.AddWithValue("@MemberId", If(memberId, String.Empty))
                command.Parameters.AddWithValue("@AuthNumber", If(authNumber, String.Empty))
                command.Parameters.AddWithValue("@Concern", If(concern, String.Empty))
                command.Parameters.AddWithValue("@ProviderDetails", If(providerDetails, String.Empty))
                command.Parameters.AddWithValue("@QuestionAnswers", If(questionAnswers, String.Empty))
                command.Parameters.AddWithValue("@CallNotes", If(callNotes, String.Empty))
                command.ExecuteNonQuery()
            End Using
        End Using

    End Sub
End Class