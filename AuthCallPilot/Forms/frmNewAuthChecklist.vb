Imports System.Drawing
Imports System.Windows.Forms

Public Class frmNewAuthChecklist
    Inherits Form

    Private ReadOnly pnlMain As New FlowLayoutPanel()

    Private ReadOnly lblContactMethod As Label = CreateStatusLabel("Contact Method")
    Private ReadOnly lblContactName As Label = CreateStatusLabel("Contact Name")
    Private ReadOnly lblContactType As Label = CreateStatusLabel("Contact Type")
    Private ReadOnly lblServiceDate As Label = CreateStatusLabel("Date of Service")

    Private ReadOnly lblRequestingProvider As Label = CreateStatusLabel("Requesting Provider")
    Private ReadOnly lblTreatingProvider As Label = CreateStatusLabel("Treating Provider")
    Private ReadOnly lblFacilityProvider As Label = CreateStatusLabel("Facility Provider")

    Private ReadOnly lblNotificationDate As Label = CreateStatusLabel("Notification Date")
    Private ReadOnly lblProgramManagement As Label = CreateStatusLabel("Program Management")
    Private ReadOnly lblAuthType As Label = CreateStatusLabel("Authorization Type")

    Private ReadOnly lblRequestType As Label = CreateStatusLabel("Request Type")
    Private ReadOnly lblAdmissionType As Label = CreateStatusLabel("Admission Type")
    Private ReadOnly lblServiceType As Label = CreateStatusLabel("Service Type")
    Private ReadOnly lblTotalDays As Label = CreateStatusLabel("Total Days")

    Private ReadOnly lblPrimaryDiagnosis As Label = CreateStatusLabel("Primary Diagnosis")
    Private ReadOnly lblProcedureCodes As Label = CreateStatusLabel("Procedure Codes")

    Public Sub New()
        Me.Text = "New Authorization Checklist"
        Me.Width = 390
        Me.Height = 720
        Me.MinimumSize = New Size(350, 500)
        Me.StartPosition = FormStartPosition.Manual
        Me.TopMost = True
        Me.ShowInTaskbar = False
        Me.FormBorderStyle = FormBorderStyle.SizableToolWindow
        Me.BackColor = Color.White
        Me.Font = New Font("Segoe UI", 9.0F)

        pnlMain.Dock = DockStyle.Fill
        pnlMain.FlowDirection = FlowDirection.TopDown
        pnlMain.WrapContents = False
        pnlMain.AutoScroll = True
        pnlMain.Padding = New Padding(12)
        pnlMain.BackColor = Color.White

        pnlMain.Controls.Add(CreateHeader("GENERAL"))
        pnlMain.Controls.Add(lblContactMethod)
        pnlMain.Controls.Add(lblContactName)
        pnlMain.Controls.Add(lblContactType)
        pnlMain.Controls.Add(lblServiceDate)

        pnlMain.Controls.Add(CreateHeader("PROVIDERS"))
        pnlMain.Controls.Add(lblRequestingProvider)
        pnlMain.Controls.Add(lblTreatingProvider)
        pnlMain.Controls.Add(lblFacilityProvider)

        pnlMain.Controls.Add(CreateHeader("AUTHORIZATION"))
        pnlMain.Controls.Add(lblNotificationDate)
        pnlMain.Controls.Add(lblProgramManagement)
        pnlMain.Controls.Add(lblAuthType)

        pnlMain.Controls.Add(CreateHeader("INPATIENT / OUTPATIENT"))
        pnlMain.Controls.Add(lblRequestType)
        pnlMain.Controls.Add(lblAdmissionType)
        pnlMain.Controls.Add(lblServiceType)
        pnlMain.Controls.Add(lblTotalDays)

        pnlMain.Controls.Add(CreateHeader("CLINICAL"))
        pnlMain.Controls.Add(lblPrimaryDiagnosis)
        pnlMain.Controls.Add(lblProcedureCodes)

        Me.Controls.Add(pnlMain)

        PositionAtRightSide()
    End Sub

    Private Sub PositionAtRightSide()
        Dim area As Rectangle = Screen.PrimaryScreen.WorkingArea
        Me.Left = Math.Max(area.Left, area.Right - Me.Width - 20)
        Me.Top = area.Top + 40
    End Sub

    Private Shared Function CreateHeader(text As String) As Label
        Return New Label With {
            .Text = text,
            .Font = New Font("Segoe UI", 9.5F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(62, 119, 35),
            .BackColor = Color.FromArgb(238, 247, 230),
            .Width = 335,
            .Height = 28,
            .TextAlign = ContentAlignment.MiddleLeft,
            .Padding = New Padding(8, 0, 0, 0),
            .Margin = New Padding(0, 8, 0, 3)
        }
    End Function

    Private Shared Function CreateStatusLabel(text As String) As Label
        Return New Label With {
            .Text = "✗ " & text,
            .Tag = text,
            .Font = New Font("Segoe UI", 9.0F),
            .ForeColor = Color.FromArgb(170, 55, 55),
            .BackColor = Color.FromArgb(252, 235, 235),
            .Width = 335,
            .Height = 28,
            .TextAlign = ContentAlignment.MiddleLeft,
            .Padding = New Padding(8, 0, 0, 0),
            .Margin = New Padding(0, 2, 0, 2)
        }
    End Function

    Private Shared Sub SetStatus(label As Label, completed As Boolean)
        Dim itemName As String = Convert.ToString(label.Tag)

        If completed Then
            label.Text = "✓ " & itemName
            label.ForeColor = Color.FromArgb(48, 112, 45)
            label.BackColor = Color.FromArgb(235, 247, 232)
        Else
            label.Text = "✗ " & itemName
            label.ForeColor = Color.FromArgb(170, 55, 55)
            label.BackColor = Color.FromArgb(252, 235, 235)
        End If
    End Sub

    Public Sub UpdateChecklist(result As NewAuthChecklistResult)
        If result Is Nothing OrElse Me.IsDisposed Then Return

        SetStatus(lblContactMethod, result.ContactMethod)
        SetStatus(lblContactName, result.ContactName)
        SetStatus(lblContactType, result.ContactType)
        SetStatus(lblServiceDate, result.ServiceDate)

        SetStatus(lblRequestingProvider, result.RequestingProvider)
        SetStatus(lblTreatingProvider, result.TreatingProvider)
        SetStatus(lblFacilityProvider, result.FacilityProvider)

        SetStatus(lblNotificationDate, result.NotificationDate)
        SetStatus(lblProgramManagement, result.ProgramManagement)
        SetStatus(lblAuthType, result.AuthType)

        SetStatus(lblPrimaryDiagnosis, result.PrimaryDiagnosis)
        SetStatus(lblProcedureCodes, result.ProcedureCodes)

        UpdateConditionalFields(result)
    End Sub

    Private Sub UpdateConditionalFields(result As NewAuthChecklistResult)
        Dim isInpatient As Boolean = String.Equals(result.CareSetting, "INPATIENT", StringComparison.OrdinalIgnoreCase)
        Dim isOutpatient As Boolean = String.Equals(result.CareSetting, "OUTPATIENT", StringComparison.OrdinalIgnoreCase)

        lblRequestType.Visible = isInpatient OrElse isOutpatient
        lblAdmissionType.Visible = isInpatient
        lblServiceType.Visible = isOutpatient
        lblTotalDays.Visible = isOutpatient

        If isInpatient Then
            SetStatus(lblRequestType, result.RequestType)
            SetStatus(lblAdmissionType, result.AdmissionType)
        ElseIf isOutpatient Then
            SetStatus(lblRequestType, result.RequestType)
            SetStatus(lblServiceType, result.ServiceType)
            SetStatus(lblTotalDays, result.TotalDays)
        End If
    End Sub
End Class