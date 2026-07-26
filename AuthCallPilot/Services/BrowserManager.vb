Imports OpenQA.Selenium
Imports OpenQA.Selenium.Edge

Public Class BrowserManager
    Private Shared _driver As EdgeDriver
    Public Shared ReadOnly Property Driver As EdgeDriver
        Get
            Return _driver
        End Get
    End Property
    Public Shared Function IsRunning() As Boolean
        Try
            If _driver Is Nothing Then Return False
            Dim temp = _driver.Title
            Return True
        Catch
            Return False
        End Try
    End Function

    Public Shared Sub Launch()
        If IsRunning() Then Exit Sub
        Dim options As New EdgeOptions()
        options.AddArgument("--start-maximized")
        _driver = New EdgeDriver(options)
        _driver.Navigate().GoToUrl("https://www.google.com")
    End Sub

    Public Shared Sub Close()
        Try
            If _driver Is Nothing Then Exit Sub
            _driver.Quit()
        Catch
        Finally
            _driver = Nothing
        End Try
    End Sub
End Class