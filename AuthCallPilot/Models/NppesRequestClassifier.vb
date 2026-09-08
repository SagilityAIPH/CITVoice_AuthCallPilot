Imports System
Imports System.Collections.Generic
Imports System.Linq

Public Enum FindCareRequestClassification
    UnableToDetermine = 0
    DoctorSpecialist = 1
    Clinic = 2
    Facility = 3
End Enum

Public NotInheritable Class NppesRequestClassificationResult
    Public Sub New(
        classification As FindCareRequestClassification,
        selectedTaxonomy As NppesTaxonomy
    )
        Me.Classification = classification
        Me.SelectedTaxonomy = selectedTaxonomy
    End Sub

    Public ReadOnly Property Classification As FindCareRequestClassification
    Public ReadOnly Property SelectedTaxonomy As NppesTaxonomy

    Public ReadOnly Property DisplayName As String
        Get
            Return NppesRequestClassifier.GetDisplayName(Classification)
        End Get
    End Property
End Class

Public NotInheritable Class NppesRequestClassifier
    Private Enum OrganizationTaxonomyKind
        Unknown = 0
        Clinic = 1
        Facility = 2
    End Enum

    Private Sub New()
    End Sub

    Public Shared Function Classify(
        provider As NppesResult
    ) As NppesRequestClassificationResult
        If provider Is Nothing Then
            Return UnableResult(Nothing)
        End If

        Dim availableTaxonomies As List(Of NppesTaxonomy) =
            GetAvailableTaxonomies(provider.Taxonomies)
        If availableTaxonomies.Count = 0 Then
            Return UnableResult(Nothing)
        End If

        Dim primaryTaxonomies As List(Of NppesTaxonomy) =
            availableTaxonomies.Where(
                Function(taxonomy As NppesTaxonomy)
                    Return taxonomy.Primary
                End Function
            ).ToList()
        Dim taxonomiesToInspect As List(Of NppesTaxonomy) = If(
            primaryTaxonomies.Count > 0,
            primaryTaxonomies,
            availableTaxonomies
        )
        Dim selectedTaxonomy As NppesTaxonomy = taxonomiesToInspect(0)
        Dim entityType As String =
            If(provider.EnumerationType, String.Empty).
                Trim().
                ToUpperInvariant()

        If entityType = "NPI-1" Then
            For Each taxonomy As NppesTaxonomy In taxonomiesToInspect
                If ClassifyOrganizationTaxonomy(taxonomy) <>
                    OrganizationTaxonomyKind.Unknown Then

                    Return UnableResult(selectedTaxonomy)
                End If
            Next

            Return New NppesRequestClassificationResult(
                FindCareRequestClassification.DoctorSpecialist,
                selectedTaxonomy
            )
        End If

        If entityType <> "NPI-2" Then
            Return UnableResult(selectedTaxonomy)
        End If

        Dim resolvedKind As OrganizationTaxonomyKind =
            OrganizationTaxonomyKind.Unknown
        For Each taxonomy As NppesTaxonomy In taxonomiesToInspect
            Dim currentKind As OrganizationTaxonomyKind =
                ClassifyOrganizationTaxonomy(taxonomy)
            If currentKind = OrganizationTaxonomyKind.Unknown Then
                Return UnableResult(selectedTaxonomy)
            End If
            If resolvedKind = OrganizationTaxonomyKind.Unknown Then
                resolvedKind = currentKind
            ElseIf resolvedKind <> currentKind Then
                Return UnableResult(selectedTaxonomy)
            End If
        Next

        Select Case resolvedKind
            Case OrganizationTaxonomyKind.Clinic
                Return New NppesRequestClassificationResult(
                    FindCareRequestClassification.Clinic,
                    selectedTaxonomy
                )
            Case OrganizationTaxonomyKind.Facility
                Return New NppesRequestClassificationResult(
                    FindCareRequestClassification.Facility,
                    selectedTaxonomy
                )
            Case Else
                Return UnableResult(selectedTaxonomy)
        End Select
    End Function

    Public Shared Function GetFindCareProviderType(
        classification As FindCareRequestClassification,
        enumerationType As String
    ) As String
        Select Case classification
            Case FindCareRequestClassification.DoctorSpecialist
                Return "doctor"
            Case FindCareRequestClassification.Clinic,
                 FindCareRequestClassification.Facility
                Return "facility"
        End Select

        Select Case If(enumerationType, String.Empty).
            Trim().
            ToUpperInvariant()
            Case "NPI-1"
                Return "doctor"
            Case "NPI-2"
                Return "facility"
            Case Else
                Return String.Empty
        End Select
    End Function

    Public Shared Function GetDisplayName(
        classification As FindCareRequestClassification
    ) As String
        Select Case classification
            Case FindCareRequestClassification.DoctorSpecialist
                Return "Doctor / Specialist"
            Case FindCareRequestClassification.Clinic
                Return "Clinic"
            Case FindCareRequestClassification.Facility
                Return "Facility"
            Case Else
                Return "UnableToDetermine"
        End Select
    End Function

    Private Shared Function GetAvailableTaxonomies(
        taxonomies As IEnumerable(Of NppesTaxonomy)
    ) As List(Of NppesTaxonomy)
        If taxonomies Is Nothing Then
            Return New List(Of NppesTaxonomy)()
        End If

        Return taxonomies.Where(
            Function(taxonomy As NppesTaxonomy)
                Return taxonomy IsNot Nothing AndAlso (
                    Not String.IsNullOrWhiteSpace(taxonomy.Code) OrElse
                    Not String.IsNullOrWhiteSpace(taxonomy.Description)
                )
            End Function
        ).ToList()
    End Function

    Private Shared Function ClassifyOrganizationTaxonomy(
        taxonomy As NppesTaxonomy
    ) As OrganizationTaxonomyKind
        Dim code As String =
            If(taxonomy.Code, String.Empty).Trim().ToUpperInvariant()
        Dim description As String =
            If(taxonomy.Description, String.Empty).Trim().ToLowerInvariant()

        If code.StartsWith("261Q", StringComparison.Ordinal) OrElse
            code = "193200000X" OrElse
            code = "193400000X" OrElse
            ContainsAny(
                description,
                "clinic/center",
                "clinic",
                "group practice",
                "medical group",
                "health center",
                "health centre",
                "urgent care"
            ) Then

            Return OrganizationTaxonomyKind.Clinic
        End If

        If code.StartsWith("282", StringComparison.Ordinal) OrElse
            code.StartsWith("283", StringComparison.Ordinal) OrElse
            code.StartsWith("284", StringComparison.Ordinal) OrElse
            ContainsAny(
                description,
                "hospital",
                "institutional",
                "durable medical equipment",
                "medical supplies",
                "dme supplier",
                "laboratory",
                "imaging center",
                "imaging centre",
                "facility"
            ) Then

            Return OrganizationTaxonomyKind.Facility
        End If

        Return OrganizationTaxonomyKind.Unknown
    End Function

    Private Shared Function ContainsAny(
        source As String,
        ParamArray values As String()
    ) As Boolean
        For Each value As String In values
            If source.IndexOf(value, StringComparison.Ordinal) >= 0 Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Shared Function UnableResult(
        selectedTaxonomy As NppesTaxonomy
    ) As NppesRequestClassificationResult
        Return New NppesRequestClassificationResult(
            FindCareRequestClassification.UnableToDetermine,
            selectedTaxonomy
        )
    End Function
End Class
