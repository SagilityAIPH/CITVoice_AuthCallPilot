Imports Guna.UI2.WinForms
Imports System.Drawing
Imports System.IO

Public Class frmLogin
    Inherits Form

    Private ReadOnly ColorPrimaryGreen As Color = Color.FromArgb(55, 142, 60)
    Private ReadOnly ColorDarkGreen As Color = Color.FromArgb(25, 105, 55)

    Private ReadOnly txtUserId As New Guna2TextBox()
    Private ReadOnly cmbTeam As New Guna2ComboBox()
    Private ReadOnly btnLogin As New Guna2Button()

    Public Sub New()
        BuildForm()
    End Sub

    Private Sub BuildForm()
        Me.Text = "Auth Call Pilot"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.None
        Me.ClientSize = New Size(430, 410)
        Me.BackColor = Color.FromArgb(245, 248, 245)

        Dim pnlHeader As New Panel With {
        .Dock = DockStyle.Top,
        .Height = 46,
        .BackColor = ColorPrimaryGreen
    }

        Dim lblHeader As New Label With {
        .Text = "Auth Call Pilot V1.0",
        .ForeColor = Color.White,
        .Font = New Font("Segoe UI Semibold", 12.0!),
        .AutoSize = True,
        .Location = New Point(15, 12)
    }

        Dim lblClose As New Label With {
        .Text = "×",
        .ForeColor = Color.White,
        .Font = New Font("Segoe UI", 18.0!),
        .AutoSize = True,
        .Cursor = Cursors.Hand,
        .Location = New Point(398, 7)
    }

        AddHandler lblClose.Click, Sub() Me.Close()

        pnlHeader.Controls.Add(lblHeader)
        pnlHeader.Controls.Add(lblClose)
        Me.Controls.Add(pnlHeader)

        Dim card As New Guna2Panel With {
        .Location = New Point(38, 65),
        .Size = New Size(350, 335),
        .FillColor = Color.White,
        .BorderRadius = 15,
        .BorderThickness = 1,
        .BorderColor = Color.FromArgb(200, 225, 200)
    }

        Me.Controls.Add(card)

        Dim picLogo As New PictureBox With {
        .Location = New Point(55, 15),
        .Size = New Size(240, 80),
        .SizeMode = PictureBoxSizeMode.Zoom,
        .BackColor = Color.Transparent
    }

        Dim logoPath As String = Path.Combine(Application.StartupPath, "Data", "AuthCallPilot.png")

        If File.Exists(logoPath) Then
            picLogo.Image = Image.FromFile(logoPath)
        End If

        card.Controls.Add(picLogo)

        Dim lblUserId As New Label With {
    .Text = "USER ID",
    .ForeColor = ColorDarkGreen,
    .Font = New Font("Segoe UI Semibold", 8.5!),
    .AutoSize = True,
    .Location = New Point(30, 110)
}

        txtUserId.Location = New Point(30, 122)
        txtUserId.Size = New Size(290, 30)
        txtUserId.Text = Environment.UserName
        txtUserId.Enabled = False
        txtUserId.BorderRadius = 8
        txtUserId.BorderThickness = 1
        txtUserId.BorderColor = Color.FromArgb(190, 210, 195)
        txtUserId.FillColor = Color.FromArgb(245, 247, 245)
        txtUserId.ForeColor = Color.FromArgb(70, 80, 70)
        txtUserId.Font = New Font("Segoe UI", 9.5!)

        txtUserId.DisabledState.FillColor = Color.FromArgb(245, 247, 245)
        txtUserId.DisabledState.ForeColor = Color.FromArgb(70, 80, 70)
        txtUserId.DisabledState.BorderColor = Color.FromArgb(190, 210, 195)

        Dim lblTeam As New Label With {
            .Text = "TEAM",
            .ForeColor = ColorDarkGreen,
            .Font = New Font("Segoe UI Semibold", 8.5!),
            .AutoSize = True,
            .Location = New Point(30, 190)
        }

        cmbTeam.Location = New Point(30, 212)
        cmbTeam.Size = New Size(290, 40)
        cmbTeam.BorderRadius = 8
        cmbTeam.BorderThickness = 1
        cmbTeam.BorderColor = ColorPrimaryGreen
        cmbTeam.FocusedState.BorderColor = ColorDarkGreen
        cmbTeam.FillColor = Color.White
        cmbTeam.Font = New Font("Segoe UI", 9.5!)
        cmbTeam.ForeColor = Color.FromArgb(45, 55, 50)
        cmbTeam.DropDownStyle = ComboBoxStyle.DropDownList

        cmbTeam.Items.Clear()
        cmbTeam.Items.Add("Select Team")
        cmbTeam.Items.Add("TEAM 1")
        cmbTeam.Items.Add("TEAM 2")
        cmbTeam.Items.Add("TEAM 3")
        cmbTeam.Items.Add("TEAM 4")
        cmbTeam.Items.Add("TEAM 5")
        cmbTeam.SelectedIndex = 0



        btnLogin.Text = "CONTINUE"
        btnLogin.Location = New Point(30, 275)
        btnLogin.Size = New Size(290, 38)
        btnLogin.BorderRadius = 9
        btnLogin.BorderThickness = 0
        btnLogin.FillColor = ColorPrimaryGreen
        btnLogin.ForeColor = Color.White
        btnLogin.Font = New Font("Segoe UI Semibold", 10.0!)
        btnLogin.Cursor = Cursors.Hand
        btnLogin.HoverState.FillColor = ColorDarkGreen

        AddHandler btnLogin.Click, AddressOf btnLogin_Click

        card.Controls.Add(lblUserId)
        card.Controls.Add(txtUserId)
        card.Controls.Add(lblTeam)
        card.Controls.Add(cmbTeam)
        card.Controls.Add(btnLogin)

        Me.AcceptButton = btnLogin
    End Sub

    Private Sub StyleInput(textBox As Guna2TextBox)
        textBox.BorderRadius = 8
        textBox.BorderColor = Color.FromArgb(190, 210, 195)
        textBox.FocusedState.BorderColor = ColorPrimaryGreen
        textBox.FillColor = Color.White
        textBox.Font = New Font("Segoe UI", 9.5!)
        textBox.ForeColor = Color.FromArgb(45, 55, 50)
        textBox.PlaceholderForeColor = Color.FromArgb(150, 160, 155)

        textBox.DisabledState.FillColor = Color.FromArgb(245, 247, 245)
        textBox.DisabledState.ForeColor = Color.FromArgb(80, 90, 80)
        textBox.DisabledState.BorderColor = Color.FromArgb(200, 215, 200)
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs)
        If cmbTeam.SelectedIndex < 0 Then
            MessageBox.Show("Please select your team.", "Auth Call Pilot", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim team As String = cmbTeam.SelectedItem.ToString()
        Dim databasePath As String = GetTeamDatabasePath(team)

        If String.IsNullOrWhiteSpace(databasePath) Then
            MessageBox.Show("No database has been configured for " & team & ".", "Auth Call Pilot", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If Not File.Exists(databasePath) Then
            MessageBox.Show(
                "The tracking database for " & team & " was not found." &
                Environment.NewLine &
                Environment.NewLine &
                databasePath,
                "Database Not Found",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)
            Return
        End If

        AppSession.UserId = Environment.UserName
        AppSession.Team = team
        AppSession.TrackingDatabasePath = databasePath

        Me.Hide()

        Using mainForm As New frmMain()
            mainForm.ShowDialog()
        End Using

        Me.Close()
    End Sub

    Private Function GetTeamDatabasePath(team As String) As String
        Select Case team.Trim().ToUpperInvariant()

            Case "TEAM 1"
                Return "D:\CallPilotTracking\Team1\CallPilotTracking.db"

            Case "TEAM 2"
                Return "D:\CallPilotTracking\Team2\CallPilotTracking.db"

            Case "TEAM 3"
                Return "D:\CallPilotTracking\Team3\CallPilotTracking.db"

            Case "TEAM 4"
                Return "D:\CallPilotTracking\Team4\CallPilotTracking.db"

            Case "TEAM 5"
                Return "D:\CallPilotTracking\Team5\CallPilotTracking.db"

            Case Else
                Return String.Empty

        End Select
    End Function

End Class