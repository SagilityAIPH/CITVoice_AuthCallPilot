Imports System.Data.SQLite
Imports System.IO

Public NotInheritable Class DatabaseManager

    Private Sub New()
    End Sub

    Public Shared ReadOnly Property DatabasePath As String
        'Test
        'Get
        '    Dim currentFolder As New DirectoryInfo(
        '        Application.StartupPath)

        '    While currentFolder IsNot Nothing

        '        Dim candidatePath As String =
        '            Path.Combine(
        '                currentFolder.FullName,
        '                "Database",
        '                "CallPilot.db")

        '        If File.Exists(candidatePath) Then
        '            Return candidatePath
        '        End If

        '        currentFolder = currentFolder.Parent
        '    End While

        '    Throw New FileNotFoundException(
        '        "Unable to find Database\CallPilot.db.")
        'End Get
        'Prod
        Get
            Return Path.Combine(
                Application.StartupPath,
                "Database",
                "CallPilot.db")
        End Get
    End Property

    Public Shared Function GetConnection() As SQLiteConnection
        If Not File.Exists(DatabasePath) Then
            Throw New FileNotFoundException(
                "CallPilot database was not found.",
                DatabasePath)
        End If

        Dim connectionString As String =
            "Data Source=" & DatabasePath & ";" &
            "Version=3;" &
            "Read Only=True;" &
            "FailIfMissing=True;"

        Return New SQLiteConnection(connectionString)

    End Function
    Public Shared Sub VerifyDatabase()

        If Not File.Exists(DatabasePath) Then
            Throw New FileNotFoundException(
                "Database is missing from the deployed application." &
                Environment.NewLine &
                DatabasePath)
        End If

        Using connection As SQLiteConnection = GetConnection()
            connection.Open()

            Using command As New SQLiteCommand(
                "SELECT COUNT(*) FROM sqlite_master WHERE type='table';",
                connection)

                Dim tableCount As Integer =
                    Convert.ToInt32(command.ExecuteScalar())

                If tableCount = 0 Then
                    Throw New InvalidOperationException(
                        "The database opened, but it contains no tables.")
                End If
            End Using
        End Using

    End Sub
End Class