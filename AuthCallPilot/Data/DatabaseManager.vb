Imports System.Data.SQLite
Imports System.IO

Public NotInheritable Class DatabaseManager

    Private Sub New()
    End Sub

    Public Shared ReadOnly Property DatabasePath As String
        Get
            Dim currentFolder As New DirectoryInfo(
                Application.StartupPath)

            While currentFolder IsNot Nothing

                Dim candidatePath As String =
                    Path.Combine(
                        currentFolder.FullName,
                        "Database",
                        "CallPilot.db")

                If File.Exists(candidatePath) Then
                    Return candidatePath
                End If

                currentFolder = currentFolder.Parent
            End While

            Throw New FileNotFoundException(
                "Unable to find Database\CallPilot.db.")
        End Get
    End Property

    Public Shared Function GetConnection() As SQLiteConnection

        Dim connectionString As String =
            "Data Source=" & DatabasePath & ";" &
            "Version=3;" &
            "Read Only=True;" &
            "FailIfMissing=True;"

        Return New SQLiteConnection(connectionString)

    End Function

End Class