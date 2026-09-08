Imports System.Net.Http
Imports System.Threading
Imports Newtonsoft.Json

Public Class NppesService

    Private Shared ReadOnly Client As New HttpClient() With {
            .Timeout = TimeSpan.FromSeconds(15)
        }

    Private Const ApiUrl As String =
            "https://npiregistry.cms.hhs.gov/api/?number={0}&version=2.1"

    Public Shared Async Function LookupNpiAsync(
            npi As String,
            Optional cancellationToken As CancellationToken = Nothing
        ) As Task(Of NppesResponse)

        If String.IsNullOrWhiteSpace(npi) Then
            Throw New ArgumentException("Please enter an NPI number.")
        End If

        npi = npi.Trim()

        If npi.Length <> 10 OrElse Not npi.All(AddressOf Char.IsDigit) Then
            Throw New ArgumentException(
                    "The NPI number must contain exactly 10 digits."
                )
        End If

        Dim requestUrl As String =
                String.Format(ApiUrl, Uri.EscapeDataString(npi))

        Using response As HttpResponseMessage =
                Await Client.GetAsync(requestUrl, cancellationToken)

            response.EnsureSuccessStatusCode()

            Dim json As String =
                    Await response.Content.ReadAsStringAsync()

            Dim result As NppesResponse =
                    JsonConvert.DeserializeObject(Of NppesResponse)(json)

            If result Is Nothing Then
                Throw New Exception(
                        "The NPPES API returned an invalid response."
                    )
            End If

            Return result
        End Using
    End Function

End Class
