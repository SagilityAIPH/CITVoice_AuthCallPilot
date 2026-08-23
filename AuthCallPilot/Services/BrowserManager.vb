Imports OpenQA.Selenium
Imports OpenQA.Selenium.Edge
Imports OpenQA.Selenium.Support.UI
Public Class BrowserManager
    Private Shared _driver As EdgeDriver
    Private Shared ReadOnly _driverLock As New Object()

    Public Shared ReadOnly Property Driver As EdgeDriver
        Get
            Return _driver
        End Get
    End Property

    Public Shared Function IsRunning() As Boolean

        Try
            If _driver Is Nothing Then
                Return False
            End If

            Dim temp As String = _driver.Title
            Return True
        Catch
            Return False
        End Try

    End Function

    Public Shared Function IsBrowserAvailable() As Boolean

        If _driver Is Nothing Then
            Return False
        End If

        Try
            Return _driver.WindowHandles IsNot Nothing AndAlso _driver.WindowHandles.Count > 0
        Catch
            Return False
        End Try

    End Function

    Public Shared Sub Launch()

        If IsRunning() Then
            Exit Sub
        End If

        Dim options As New EdgeOptions()
        options.AddArgument("--start-maximized")
        _driver = New EdgeDriver(options)
        _driver.Navigate().GoToUrl("https://carehub.humana.com/cgx/Search")

    End Sub

    Public Shared Sub SearchCgxMember(firstName As String, lastName As String, memberId As String, dateOfBirth As String)

        If Not IsBrowserAvailable() Then
            Throw New InvalidOperationException("The CGX browser is not open. Click Launch Browser first.")
        End If

        Const cgxSearchUrl As String = "https://carehub.humana.com/cgx/Search"

        If String.IsNullOrWhiteSpace(_driver.Url) OrElse Not _driver.Url.StartsWith(cgxSearchUrl, StringComparison.OrdinalIgnoreCase) Then
            _driver.Navigate().GoToUrl(cgxSearchUrl)
        End If

        Dim wait As New WebDriverWait(_driver, TimeSpan.FromSeconds(20))

        WaitForVisibleElement(wait, By.Id("FirstName"))
        FillTextBox(wait, By.Id("FirstName"), firstName)
        FillTextBox(wait, By.Id("LastName"), lastName)
        FillTextBox(wait, By.Id("SubscriberId"), memberId)
        FillTextBox(wait, By.Id("DateOfBirth"), dateOfBirth)

        Dim searchButton As IWebElement = WaitForClickableElement(wait, By.Id("btnSearch"))
        searchButton.Click()

    End Sub
    Public Shared Sub OpenFirstSearchResultAndPopulateContext(context As CallContext)

        If context Is Nothing Then
            Throw New ArgumentNullException(NameOf(context))
        End If

        If Not IsBrowserAvailable() Then
            Throw New InvalidOperationException(
            "The CGX browser is unavailable.")
        End If

        Dim wait As New WebDriverWait(_driver, TimeSpan.FromSeconds(25))

        'The exact XPath supplied for the first result row.
        Dim firstResultXPath As String = "/html/body/div[3]/div/div[3]/div/div/div[2]/div[5]/div[2]/div[1]/div/div[4]/div/div/div[2]/div/div[1]/table/tbody/tr"

        Dim firstResult As IWebElement = WaitForClickableElement(wait, By.XPath(firstResultXPath))
        firstResult.Click()

        'Wait for the member landing page.
        WaitForMemberPage(wait)

        'Expand the member-information banner when needed.
        ExpandMemberInformationIfNeeded(wait)

        'Read and normalize CGX values.
        'Dim rawProduct As String = ReadCgxFieldValue(wait, "Product/MTV or CAS")
        Dim rawProduct As String = ReadCgxFieldValue(wait, "ProductDesc")
        Dim rawConso As String = ReadCgxFieldValue(wait, "Consolidated Selling Market")
        Dim rawGroup As String = ReadCgxFieldValue(wait, "Group Name/ID")
        Dim rawIssueState As String = ReadCgxFieldValue(wait, "State of Issue")
        context.Product = GetTextBeforeSlash(rawProduct)
        context.Conso = GetTextAfterSlash(rawConso)
        context.GroupNumber = GetTextAfterSlash(rawGroup)

        context.IssueState = rawIssueState.Trim()

    End Sub
    Private Shared Sub WaitForMemberPage(wait As WebDriverWait)

        wait.Until(
            Function(driver As IWebDriver) As Boolean

                Try
                    'The landing page should have either the
                    'member banner or the member information area.
                    Dim expandButtons As IReadOnlyCollection(Of IWebElement) = driver.FindElements(By.Id("expand-member-information"))

                    If expandButtons.Count > 0 Then
                        Return True
                    End If

                    Dim pageText As String = driver.FindElement(By.TagName("body")).Text
                    Return pageText.IndexOf("Member Information", StringComparison.OrdinalIgnoreCase) >= 0

                Catch ex As WebDriverException
                    Return False
                End Try

            End Function)
    End Sub
    Private Shared Sub ExpandMemberInformationIfNeeded(wait As WebDriverWait)
        Try
            Dim expandButton As IWebElement = wait.Until(
                Function(driver As IWebDriver) As IWebElement

                    Try
                        Dim elements As IReadOnlyCollection(Of IWebElement) = driver.FindElements(By.Id("expand-member-information"))

                        If elements.Count = 0 Then
                            Return Nothing
                        End If

                        Dim element As IWebElement = elements.First()
                        If element.Displayed AndAlso element.Enabled Then
                            Return element
                        End If

                    Catch ex As WebDriverException
                    End Try

                    Return Nothing

                End Function)

            If expandButton IsNot Nothing Then
                expandButton.Click()
            End If

        Catch ex As WebDriverTimeoutException
            'The banner may already be expanded.
            'Continue and attempt to read the fields.
        End Try
    End Sub
    Private Shared Function ReadCgxFieldValue(wait As WebDriverWait, fieldName As String) As String

        Return wait.Until(
        Function(driver As IWebDriver) As String

            'Attempt 1:
            'Treat the supplied field name as an element ID.
            Try
                Dim elements As IReadOnlyCollection(Of IWebElement) = driver.FindElements(By.Id(fieldName))
                For Each element As IWebElement In elements
                    Dim value As String = GetElementValue(element)
                    If Not String.IsNullOrWhiteSpace(value) Then
                        Return value.Trim()
                    End If
                Next

            Catch ex As WebDriverException
            End Try

            'Attempt 2:
            'Find visible text matching the label and read
            'the value from a nearby sibling element.
            Try
                Dim escapedLabel As String = EscapeXPathLiteral(fieldName)
                Dim labelXPath As String = "//*[normalize-space(text())=" & escapedLabel & "]"
                Dim labels As IReadOnlyCollection(Of IWebElement) = driver.FindElements(By.XPath(labelXPath))

                For Each labelElement As IWebElement In labels
                    Dim nearbyValue As String = ReadNearbyValue(labelElement)
                    If Not String.IsNullOrWhiteSpace(
                        nearbyValue) Then

                        Return nearbyValue.Trim()
                    End If

                Next

            Catch ex As WebDriverException
            End Try

            Return Nothing
        End Function)
    End Function
    Private Shared Function GetElementValue(element As IWebElement) As String

        If element Is Nothing Then
            Return String.Empty
        End If

        Dim tagName As String =
        element.TagName.ToLowerInvariant()

        If tagName = "input" OrElse tagName = "textarea" OrElse tagName = "select" Then
            Dim valueAttribute As String = element.GetAttribute("value")
            If Not String.IsNullOrWhiteSpace(valueAttribute) Then
                Return valueAttribute.Trim()
            End If

        End If
        Return element.Text.Trim()

    End Function
    Private Shared Function ReadNearbyValue(labelElement As IWebElement) As String

        If labelElement Is Nothing Then
            Return String.Empty
        End If

        Dim relativeXPaths As String() = {
        "following-sibling::*[1]",
        "../following-sibling::*[1]",
        "../*[last()]",
        "../../*[last()]",
        "following::*[1]"
    }

        For Each relativeXPath As String In relativeXPaths
            Try
                Dim valueElement As IWebElement = labelElement.FindElement(By.XPath(relativeXPath))
                Dim value As String = GetElementValue(valueElement)

                If Not String.IsNullOrWhiteSpace(value) AndAlso Not String.Equals(value, labelElement.Text, StringComparison.OrdinalIgnoreCase) Then

                    Return value.Trim()
                End If

            Catch ex As NoSuchElementException
            Catch ex As StaleElementReferenceException
            End Try

        Next
        Return String.Empty
    End Function
    Private Shared Function GetTextBeforeSlash(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then
            Return String.Empty
        End If

        Dim slashIndex As Integer = value.IndexOf("/"c)

        If slashIndex < 0 Then
            Return value.Trim()
        End If

        Return value.Substring(0, slashIndex).Trim()
    End Function
    Private Shared Function GetTextAfterSlash(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then
            Return String.Empty
        End If

        Dim slashIndex As Integer = value.IndexOf("/"c)

        If slashIndex < 0 OrElse slashIndex = value.Length - 1 Then
            Return value.Trim()
        End If

        Return value.Substring(slashIndex + 1).Trim()

    End Function
    Private Shared Sub FillTextBox(wait As WebDriverWait, locator As By, value As String)

        If String.IsNullOrWhiteSpace(value) Then
            Return
        End If

        Dim element As IWebElement = WaitForVisibleElement(wait, locator)
        element.Click()
        element.Clear()
        element.SendKeys(value.Trim())

    End Sub
    Private Shared Function EscapeXPathLiteral(value As String) As String
        If Not value.Contains("'") Then
            Return "'" & value & "'"
        End If

        If Not value.Contains("""") Then
            Return """" & value & """"
        End If

        Dim parts As String() = value.Split("'"c)
        Return "concat('" & String.Join("', ""'"", '", parts) & "')"
    End Function
    Public Shared Sub Close()
        Try

            If _driver Is Nothing Then
                Exit Sub
            End If

            _driver.Quit()

        Catch
        Finally
            _driver = Nothing
        End Try

    End Sub
    Private Shared Function WaitForVisibleElement(wait As WebDriverWait, locator As By) As IWebElement
        Return wait.Until(
            Function(driver As IWebDriver) As IWebElement
                Try
                    Dim element As IWebElement = driver.FindElement(locator)

                    If element.Displayed Then
                        Return element
                    End If

                Catch ex As NoSuchElementException
                    Return Nothing

                Catch ex As StaleElementReferenceException
                    Return Nothing
                End Try
                Return Nothing
            End Function)
    End Function
    Private Shared Function WaitForClickableElement(wait As WebDriverWait, locator As By) As IWebElement
        Return wait.Until(
            Function(driver As IWebDriver) As IWebElement
                Try
                    Dim element As IWebElement = driver.FindElement(locator)
                    If element.Displayed AndAlso element.Enabled Then
                        Return element
                    End If

                Catch ex As NoSuchElementException
                    Return Nothing

                Catch ex As StaleElementReferenceException
                    Return Nothing
                End Try

                Return Nothing
            End Function)
    End Function
    Public Shared Sub SearchAuthorizationAndPopulateContext(context As CallContext)
        If context Is Nothing Then
            Throw New ArgumentNullException(NameOf(context))
        End If

        If String.IsNullOrWhiteSpace(context.AuthorizationNumber) Then
            Return
        End If

        If Not IsBrowserAvailable() Then
            Throw New InvalidOperationException("The CGX browser is unavailable.")
        End If

        Dim wait As New WebDriverWait(_driver, TimeSpan.FromSeconds(30))
        Debug.WriteLine("Opening authorization search...")

        Const authorizationMenuXPath As String = "/html/body/div[3]/div/div[2]/div[2]/nav/section/ul[2]/li[6]/a"

        Dim authorizationMenu As IWebElement = WaitForClickableElement(wait, By.XPath(authorizationMenuXPath))

        authorizationMenu.Click()
        Debug.WriteLine("Opening authorization record search...")

        Const recordLinkXPath As String = "/html/body/div[3]/div/div[3]/div/div/div[1]/div/div/dl/dd[2]/a"

        Dim recordLink As IWebElement = WaitForClickableElement(wait, By.XPath(recordLinkXPath))

        recordLink.Click()

        Debug.WriteLine("Entering authorization number...")

        FillTextBox(wait, By.Id("authIdField"), context.AuthorizationNumber)
        Dim searchButton As IWebElement = WaitForClickableElement(wait, By.Id("btnSearch"))

        searchButton.Click()
        WaitForAuthorizationDetails(wait)
        PopulateAuthorizationDetails(wait, context)

    End Sub
    Private Shared Sub WaitForAuthorizationDetails(wait As WebDriverWait)
        wait.Until(
            Function(driver As IWebDriver) As Boolean
                Try
                    Dim elements As IReadOnlyCollection(Of IWebElement) = driver.FindElements(By.Id("OverallAuthStatusForBanner"))
                    Return elements.Count > 0 AndAlso
                           elements.First().Displayed

                Catch ex As WebDriverException
                    Return False
                End Try
            End Function)
    End Sub
    Private Shared Sub ApplyAuthTypeClassification(context As CallContext, authType As String)
        If context Is Nothing Then
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(authType) Then
            Exit Sub
        End If

        Dim normalized As String = authType.Trim().ToUpper()

        '========================================
        ' HEALTH TYPE
        '========================================
        If normalized.StartsWith("BH") Then
            context.HealthType = "BEHAVIORAL HEALTH"
        Else
            context.HealthType = "PHYSICAL HEALTH"
        End If

        '========================================
        ' CARE SETTING
        '========================================
        If normalized.Contains("INPATIENT") Then
            context.CareSetting = "INPATIENT"
        ElseIf normalized.Contains("OUTPATIENT") Then
            context.CareSetting = "OUTPATIENT"
        Else
            context.CareSetting = Nothing
        End If
    End Sub
    Private Shared Sub PopulateAuthorizationDetails(wait As WebDriverWait, context As CallContext)

        Debug.WriteLine("Reading authorization details...")
        context.AuthorizationStatus = ReadElementTextSafely(wait, By.Id("OverallAuthStatusForBanner"))
        context.RequestingProvider = ReadElementTextSafely(wait, By.Id("requesting-provider-panel"))
        context.TreatingProvider = ReadElementTextSafely(wait, By.Id("treating-provider-panel"))
        context.FacilityProvider = ReadElementTextSafely(wait, By.Id("facility-provider-panel"))
        Dim authType As String = ReadElementTextSafely(wait, By.Id("AuthType"))
        context.AuthType = authType
        ApplyAuthTypeClassification(context, authType)
        context.AuthorizationStartDate = ReadElementValueSafely(wait, By.Id("FirstDay"))
        context.AuthorizationEndDate = ReadElementValueSafely(wait, By.Id("LastDay"))
        context.AuthorizationStatus = ReadElementTextSafely(wait, By.Id("OverallAuthStatusForBanner"))

        'Const totalDaysXPath As String = "/html/body/div[3]/div/div[3]/div[2]/div[6]/div/div[13]/div[2]/div[18]/div[3]/div/div[2]"
        'context.TotalDays = ReadElementTextSafely(wait, By.XPath(totalDaysXPath))
        context.TotalDays = GetTotalDays()

        Const primaryDiagnosisXPath As String = "/html/body/div[3]/div/div[3]/div[2]/div[6]/div/div[15]/div/div[2]/div[1]/div/div[3]/div/div/table/tbody/tr/td[1]"
        context.PrimaryDiagnosisCode = ReadElementTextSafely(wait, By.XPath(primaryDiagnosisXPath))
        context.SecondaryDiagnosisCodes.Clear()

        Const secondaryDiagnosisRowsXPath As String = "/html/body/div[3]/div/div[3]/div[2]/div[6]/div/div[15]/div/div[2]/div[2]/div/div[3]/div/div/div[2]/div/table/tbody/tr"
        context.SecondaryDiagnosisCodes.AddRange(ReadTableCodes(By.XPath(secondaryDiagnosisRowsXPath)))
        context.ProcedureCodes.Clear()


        'Const procedureRowsXPath As String = "/html/body/div[3]/div/div[3]/div[2]/div[6]/div/div[15]/div/div[2]/div[3]/div/div[3]/div/div/table/tbody/tr"
        'context.ProcedureCodes.AddRange(ReadTableCodes(By.XPath(procedureRowsXPath)))
        If String.Equals(context.CareSetting, "INPATIENT", StringComparison.OrdinalIgnoreCase) Then
            context.ProcedureCodes.Clear()
            context.ProcedureCodes.AddRange(ReadInpatientProcedureCodes())
        Else
            context.ProcedureCodes.Clear()
            context.ProcedureCodes.AddRange(ReadProcedureCodes())
        End If
        'context.ProcedureCodes.AddRange(ReadProcedureCodes())


        Debug.WriteLine("Authorization Status: " & context.AuthorizationStatus)
        Debug.WriteLine("Primary Diagnosis: " & context.PrimaryDiagnosisCode)
        Debug.WriteLine("Secondary Diagnosis Count: " & context.SecondaryDiagnosisCodes.Count.ToString())
        Debug.WriteLine("Procedure Code Count: " & context.ProcedureCodes.Count.ToString())

        Dim summary As New Text.StringBuilder()

        summary.AppendLine("AUTHORIZATION DETAILS CAPTURED")
        summary.AppendLine(New String("-"c, 45))
        summary.AppendLine("Authorization Number: " & context.AuthorizationNumber)
        summary.AppendLine("Authorization Status: " & context.AuthorizationStatus)
        summary.AppendLine("Start Date: " & context.AuthorizationStartDate)
        summary.AppendLine("End Date: " & context.AuthorizationEndDate)
        summary.AppendLine("Total Days: " & context.TotalDays)
        summary.AppendLine()
        summary.AppendLine("Requesting Provider:")
        summary.AppendLine(context.RequestingProvider)
        summary.AppendLine()
        summary.AppendLine("Treating Provider:")
        summary.AppendLine(context.TreatingProvider)
        summary.AppendLine()
        summary.AppendLine("Facility Provider:")
        summary.AppendLine(context.FacilityProvider)
        summary.AppendLine()
        summary.AppendLine("Primary Diagnosis: " & context.PrimaryDiagnosisCode)
        summary.AppendLine("Secondary Diagnoses: " & If(context.SecondaryDiagnosisCodes IsNot Nothing AndAlso context.SecondaryDiagnosisCodes.Count > 0, String.Join(", ", context.SecondaryDiagnosisCodes), "Not found"))
        summary.AppendLine("Procedure Codes: " & If(context.ProcedureCodes IsNot Nothing AndAlso context.ProcedureCodes.Count > 0, String.Join(", ", context.ProcedureCodes), "Not found"))

        'MessageBox.Show(summary.ToString(), "Authorization Capture Test", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub
    Private Shared Function ReadInpatientProcedureCodes() As List(Of String)
        Dim results As New List(Of String)
        Try
            Dim grid As IWebElement = _driver.FindElement(By.Id("AuthDirectProcedureCodeGrid-gridContent"))
            Dim rows = grid.FindElements(By.XPath(".//tr"))
            For Each row As IWebElement In rows
                Dim cells = row.FindElements(By.XPath(".//td"))
                For Each cell As IWebElement In cells
                    Dim value As String = cell.Text.Trim()
                    If String.IsNullOrWhiteSpace(value) Then
                        Continue For
                    End If

                    If Not results.Contains(value, StringComparer.OrdinalIgnoreCase) Then
                        results.Add(value)
                    End If
                Next
            Next
        Catch ex As NoSuchElementException
        Catch ex As WebDriverException
        End Try
        Return results
    End Function
    Private Shared Function GetTotalDays() As String
        Try

            Dim totalDaysValue As IWebElement = _driver.FindElement(By.XPath(
                    "//div[contains(normalize-space(.), 'Total Days')]" &
                    "/following-sibling::div[1]"))

            Return totalDaysValue.Text.Trim()

        Catch ex As NoSuchElementException
            Return String.Empty

        Catch ex As WebDriverException
            Return String.Empty
        End Try

    End Function
    Private Shared Function ReadElementTextSafely(wait As WebDriverWait, locator As By) As String
        Try
            Dim element As IWebElement = WaitForVisibleElement(wait, locator)
            Return GetElementValue(element)
        Catch ex As WebDriverTimeoutException
            Return String.Empty
        Catch ex As NoSuchElementException
            Return String.Empty
        Catch ex As StaleElementReferenceException
            Return String.Empty
        End Try
    End Function
    Private Shared Function ReadElementValueSafely(wait As WebDriverWait, locator As By) As String
        Try
            Dim element As IWebElement = WaitForVisibleElement(wait, locator)
            Dim value As String = element.GetAttribute("value")

            If Not String.IsNullOrWhiteSpace(value) Then
                Return value.Trim()
            End If

            Return element.Text.Trim()

        Catch ex As WebDriverException
            Return String.Empty
        End Try
    End Function
    Private Shared Function ReadTableCodes(rowsLocator As By) As List(Of String)
        Dim codes As New List(Of String)
        Try
            Dim rows As IReadOnlyCollection(Of IWebElement) = _driver.FindElements(rowsLocator)

            For Each row As IWebElement In rows
                Try
                    Dim firstCell As IWebElement = row.FindElement(By.XPath("./td[1]"))
                    Dim code As String = firstCell.Text.Trim()

                    If Not String.IsNullOrWhiteSpace(code) AndAlso
                       Not codes.Any(
                           Function(existingCode)
                               Return String.Equals(existingCode, code, StringComparison.OrdinalIgnoreCase)
                           End Function) Then
                        codes.Add(code)
                    End If

                Catch ex As NoSuchElementException
                    Continue For

                Catch ex As StaleElementReferenceException
                    Continue For
                End Try
            Next

        Catch ex As WebDriverException
            Debug.WriteLine("Unable to read table codes: " & ex.Message)
        End Try
        Return codes

    End Function
    Public Shared Function CaptureCurrentCgxPage() As BrowserCaptureResult
        SyncLock _driverLock
            If Not IsBrowserAvailable() Then
                Return Nothing
            End If

            Dim currentUrl As String = Convert.ToString(_driver.Url)
            Dim currentTitle As String = Convert.ToString(_driver.Title)

            Dim result As New BrowserCaptureResult With {
                .Url = currentUrl,
                .Title = currentTitle
            }



            If IsMemberInformationPage(currentUrl) Then
                result.PageType = CgxPageType.MemberInformation
                result.Context = CaptureMemberInformation()

            ElseIf String.Equals(currentTitle.Trim(), "View Authorization", StringComparison.OrdinalIgnoreCase) Or currentUrl.IndexOf("/cgx/Authorization/Authorization/ViewAuth", StringComparison.OrdinalIgnoreCase) >= 0 Then
                result.PageType = CgxPageType.ViewAuthorization
                result.Context = CaptureAuthorizationInformation()
            Else
                result.PageType = CgxPageType.Other
            End If

            Return result

        End SyncLock
    End Function
    Private Shared Function IsMemberInformationPage(currentUrl As String) As Boolean
        If String.IsNullOrWhiteSpace(currentUrl) Then
            Return False
        End If
        Return currentUrl.IndexOf("/cgx/MemberCentral/MemberInfo/Index", StringComparison.OrdinalIgnoreCase) >= 0
    End Function
    Private Shared Function CaptureMemberInformation() As CallContext
        Dim wait As New WebDriverWait(_driver, TimeSpan.FromSeconds(10))
        ExpandMemberInformationIfNeeded(wait)
        Dim context As New CallContext()
        context.MemberId = ReadElementTextSafely(wait, By.Id("MaskedMemberId"))
        context.MemberName = ReadElementTextSafely(wait, By.Id("MaskedSubscriber"))
        Const dateOfBirthXPath As String = "/html/body/div[3]/div/div[2]/div[3]/div/div[1]/div[3]"
        context.DateOfBirth = ReadElementTextSafely(wait, By.XPath(dateOfBirthXPath))
        Dim rawProduct As String = ReadCgxFieldValue(wait, "Product/MTV or CAS")
        Dim rawConso As String = ReadCgxFieldValue(wait, "Consolidated Selling Market")
        Dim rawGroup As String = ReadCgxFieldValue(wait, "Group Name/ID")
        context.Product = GetTextBeforeSlash(rawProduct)
        context.Conso = GetTextAfterSlash(rawConso)
        context.GroupNumber = GetTextAfterSlash(rawGroup)
        context.IssueState = ReadCgxFieldValue(wait, "State of Issue")

        Return context
    End Function
    Private Shared Function CaptureAuthorizationInformation() As CallContext
        'MessageBox.Show("CaptureAuthorizationInformation() started", "Authorization")
        Dim wait As New WebDriverWait(_driver, TimeSpan.FromSeconds(5))
        wait.Until(
        Function(driver As IWebDriver) As Boolean

            Try
                Dim elements =
                    driver.FindElements(By.Id("OverallAuthStatusForBanner"))
                Return elements.Count > 0
            Catch
                Return False
            End Try

        End Function)
        Dim context As New CallContext()
        context.AuthorizationNumber = ReadElementTextSafely(wait, By.Id("AuthId"))

        If String.IsNullOrWhiteSpace(context.AuthorizationNumber) Then
            context.AuthorizationNumber = GetQueryStringValue(_driver.Url, "authId")
        End If

        context.AuthorizationStatus = ReadElementTextSafely(wait, By.Id("OverallAuthStatusForBanner"))
        context.RequestingProvider = ReadElementTextSafely(wait, By.Id("requesting-provider-panel"))
        context.TreatingProvider = ReadElementTextSafely(wait, By.Id("treating-provider-panel"))
        context.FacilityProvider = ReadElementTextSafely(wait, By.Id("facility-provider-panel"))

        Dim authType As String = ReadElementTextSafely(wait, By.Id("AuthType"))
        context.AuthType = authType
        ApplyAuthTypeClassification(context, authType)

        context.AuthorizationStartDate = ReadElementValueSafely(wait, By.Id("FirstDay"))
        context.AuthorizationEndDate = ReadElementValueSafely(wait, By.Id("LastDay"))
        context.ClaimPaymentNotes = String.Empty
        context.TotalDays = GetTotalDays()

        context.PrimaryDiagnosisCode = ReadElementTextSafely(wait, By.XPath("/html/body/div[3]/div/div[3]/div[2]/div[6]/div/div[15]/div/div[2]/div[1]/div/div[3]/div/div/table/tbody/tr/td[1]"))

        context.SecondaryDiagnosisCodes.Clear()
        context.SecondaryDiagnosisCodes.AddRange(ReadTableCodes(By.XPath("/html/body/div[3]/div/div[3]/div[2]/div[6]/div/div[15]/div/div[2]/div[2]/div/div[3]/div/div/div[2]/div/table/tbody/tr")))

        context.ProcedureCodes.Clear()
        context.ProcedureCodes.AddRange(ReadProcedureCodes())
        'context.ProcedureCodes.AddRange(ReadTableCodes(By.XPath("/html/body/div[3]/div/div[3]/div[2]/div[6]/div/div[15]/div/div[2]/div[3]/div/div[3]/div/div/table/tbody/tr")))

        '    MessageBox.Show(
        '"Authorization Number: " &
        'If(
        '    String.IsNullOrWhiteSpace(
        '        context.AuthorizationNumber),
        '    "[blank]",
        '    context.AuthorizationNumber) &
        'Environment.NewLine &
        'Environment.NewLine &
        '"Status: " &
        'If(
        '    String.IsNullOrWhiteSpace(
        '        context.AuthorizationStatus),
        '    "[blank]",
        '    context.AuthorizationStatus) &
        'Environment.NewLine &
        'Environment.NewLine &
        '"Requesting Provider: " &
        'If(
        '    String.IsNullOrWhiteSpace(
        '        context.RequestingProvider),
        '    "[blank]",
        '    context.RequestingProvider),
        '"Authorization Capture Result",
        'MessageBoxButtons.OK,
        'MessageBoxIcon.Information)
        Return context
    End Function
    Private Shared Function ReadProcedureCodes() As List(Of String)
        Dim codes As New List(Of String)
        Try
            Dim gridContent As IWebElement = _driver.FindElement(By.Id("AuthDirectOpProcedureCodeGrid-gridContent"))

            Dim rows As IReadOnlyCollection(Of IWebElement) =
            gridContent.FindElements(
                By.XPath(".//table/tbody/tr"))

            For Each row As IWebElement In rows
                Try
                    Dim cells As IReadOnlyCollection(Of IWebElement) = row.FindElements(By.TagName("td"))

                    If cells.Count = 0 Then
                        Continue For
                    End If

                    Dim code As String = cells.First().Text.Trim()
                    If String.IsNullOrWhiteSpace(code) Then
                        Continue For
                    End If

                    If Not codes.Any(Function(existing)
                                         Return String.Equals(existing, code, StringComparison.OrdinalIgnoreCase)
                                     End Function) Then

                        codes.Add(code)
                    End If

                Catch ex As StaleElementReferenceException
                    Continue For
                End Try
            Next

        Catch ex As NoSuchElementException
            Debug.WriteLine("Procedure Code grid was not found.")

        Catch ex As WebDriverException
            Debug.WriteLine("Procedure Code read error: " &
            ex.Message)

        End Try

        Return codes

    End Function
    Public Shared Function GetCurrentPageLocation(ByRef currentUrl As String, ByRef currentTitle As String) As Boolean
        currentUrl = String.Empty
        currentTitle = String.Empty
        SyncLock _driverLock

            If Not IsBrowserAvailable() Then
                Return False
            End If

            Try
                currentUrl = Convert.ToString(_driver.Url).Trim()
                currentTitle = Convert.ToString(_driver.Title).Trim()
                Return True

            Catch ex As WebDriverException
                Return False
            End Try
        End SyncLock
    End Function
    Private Shared Function GetQueryStringValue(url As String, parameterName As String) As String
        If String.IsNullOrWhiteSpace(url) OrElse
           String.IsNullOrWhiteSpace(parameterName) Then

            Return String.Empty
        End If

        Try
            Dim uri As New Uri(url)
            Dim query As String =
                uri.Query.TrimStart("?"c)
            For Each pair As String In query.Split("&"c)
                Dim parts As String() = pair.Split(New Char() {"="c}, 2)

                If parts.Length = 2 And String.Equals(parts(0), parameterName, StringComparison.OrdinalIgnoreCase) Then
                    Return Uri.UnescapeDataString(parts(1)).Trim()
                End If
            Next
        Catch ex As Exception
            Return String.Empty
        End Try

        Return String.Empty
    End Function
    Private Shared Function ReadElementTextImmediate(locator As By) As String
        Try
            Dim elements As IReadOnlyCollection(Of IWebElement) = _driver.FindElements(locator)
            If elements.Count = 0 Then
                Return String.Empty
            End If

            Dim element As IWebElement = elements.First()
            Return GetElementValue(element)

        Catch ex As WebDriverException
            Return String.Empty
        End Try

    End Function
    Private Shared Function ReadElementValueImmediate(locator As By) As String
        Try
            Dim elements As IReadOnlyCollection(Of IWebElement) = _driver.FindElements(locator)

            If elements.Count = 0 Then
                Return String.Empty
            End If

            Dim element As IWebElement = elements.First()
            Dim value As String = element.GetAttribute("value")

            If Not String.IsNullOrWhiteSpace(value) Then
                Return value.Trim()
            End If

            Return element.Text.Trim()

        Catch ex As WebDriverException
            Return String.Empty
        End Try

    End Function
End Class