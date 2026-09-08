
Imports Newtonsoft.Json

    Public Class NppesResponse
        <JsonProperty("result_count")>
        Public Property ResultCount As Integer

        <JsonProperty("results")>
        Public Property Results As List(Of NppesResult)
    End Class

    Public Class NppesResult
        <JsonProperty("number")>
        Public Property Number As String

        <JsonProperty("enumeration_type")>
        Public Property EnumerationType As String

        <JsonProperty("basic")>
        Public Property Basic As NppesBasic

        <JsonProperty("addresses")>
        Public Property Addresses As List(Of NppesAddress)

        <JsonProperty("practiceLocations")>
        Public Property PracticeLocations As List(Of NppesAddress)

        <JsonProperty("taxonomies")>
        Public Property Taxonomies As List(Of NppesTaxonomy)

        <JsonProperty("identifiers")>
        Public Property Identifiers As List(Of NppesIdentifier)

        <JsonProperty("other_names")>
        Public Property OtherNames As List(Of NppesOtherName)
    End Class

    Public Class NppesBasic
        <JsonProperty("first_name")>
        Public Property FirstName As String

        <JsonProperty("middle_name")>
        Public Property MiddleName As String

        <JsonProperty("last_name")>
        Public Property LastName As String

        <JsonProperty("credential")>
        Public Property Credential As String

        <JsonProperty("sex")>
        Public Property Sex As String

        <JsonProperty("organization_name")>
        Public Property OrganizationName As String

        <JsonProperty("authorized_official_first_name")>
        Public Property AuthorizedOfficialFirstName As String

        <JsonProperty("authorized_official_last_name")>
        Public Property AuthorizedOfficialLastName As String

        <JsonProperty("authorized_official_title_or_position")>
        Public Property AuthorizedOfficialTitle As String

        <JsonProperty("status")>
        Public Property Status As String

        <JsonProperty("sole_proprietor")>
        Public Property SoleProprietor As String

        <JsonProperty("enumeration_date")>
        Public Property EnumerationDate As String

        <JsonProperty("last_updated")>
        Public Property LastUpdated As String
    End Class

    Public Class NppesAddress
        <JsonProperty("address_purpose")>
        Public Property AddressPurpose As String

        <JsonProperty("address_type")>
        Public Property AddressType As String

        <JsonProperty("address_1")>
        Public Property Address1 As String

        <JsonProperty("address_2")>
        Public Property Address2 As String

        <JsonProperty("city")>
        Public Property City As String

        <JsonProperty("state")>
        Public Property State As String

        <JsonProperty("postal_code")>
        Public Property PostalCode As String

        <JsonProperty("country_name")>
        Public Property CountryName As String

        <JsonProperty("telephone_number")>
        Public Property TelephoneNumber As String

        <JsonProperty("fax_number")>
        Public Property FaxNumber As String
    End Class

    Public Class NppesTaxonomy
        <JsonProperty("code")>
        Public Property Code As String

        <JsonProperty("desc")>
        Public Property Description As String

        <JsonProperty("license")>
        Public Property License As String

        <JsonProperty("state")>
        Public Property State As String

        <JsonProperty("primary")>
        Public Property Primary As Boolean
    End Class

    Public Class NppesIdentifier
        <JsonProperty("identifier")>
        Public Property Identifier As String

        <JsonProperty("desc")>
        Public Property Description As String

        <JsonProperty("state")>
        Public Property State As String

        <JsonProperty("issuer")>
        Public Property Issuer As String
    End Class

Public Class NppesOtherName
    <JsonProperty("organization_name")>
    Public Property OrganizationName As String

    <JsonProperty("first_name")>
    Public Property FirstName As String

    <JsonProperty("last_name")>
    Public Property LastName As String

    <JsonProperty("credential")>
    Public Property Credential As String
End Class
