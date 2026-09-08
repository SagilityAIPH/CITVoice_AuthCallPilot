Imports System.Threading
Imports System.Threading.Tasks

Public NotInheritable Class ParSteerageService

    Private Sub New()
    End Sub

    Public Shared Async Function NavigateToPhysicianFinderAsync(memberId As String, dateOfBirth As String, memberZip As String, nonParNpi As String) As Task
        Dim nppesResponse As NppesResponse = Await NppesService.LookupNpiAsync(nonParNpi, CancellationToken.None)
        If nppesResponse Is Nothing OrElse nppesResponse.Results Is Nothing OrElse nppesResponse.Results.Count = 0 Then Throw New InvalidOperationException("NPI Registry did not return the Non-PAR provider.")

        Dim nppesProvider As NppesResult = nppesResponse.Results(0)
        Dim classification As NppesRequestClassificationResult = NppesRequestClassifier.Classify(nppesProvider)
        If classification Is Nothing OrElse classification.Classification = FindCareRequestClassification.UnableToDetermine OrElse classification.SelectedTaxonomy Is Nothing Then Throw New InvalidOperationException("Unable to determine the provider taxonomy.")

        Dim taxonomy As NppesTaxonomy = classification.SelectedTaxonomy
        Dim providerType As String = NppesRequestClassifier.GetFindCareProviderType(classification.Classification, nppesProvider.EnumerationType)

        Await Task.Run(Sub() BrowserManager.OpenPhysicianFinder(memberId, dateOfBirth, memberZip, providerType, taxonomy.Code, taxonomy.Description))
    End Function

End Class