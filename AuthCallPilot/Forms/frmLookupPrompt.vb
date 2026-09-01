Public Class frmLookupPrompt
    Inherits Form

    Private ReadOnly lblTitle As New Label()
    Private ReadOnly txtMessage As New TextBox()
    Private ReadOnly btnClose As New Button()

    Public Sub New(
        title As String,
        message As String)

        Me.Text = title
        Me.Width = 430
        Me.Height = 300

        Me.StartPosition =
            FormStartPosition.CenterScreen

        Me.TopMost = True
        Me.ShowInTaskbar = False

        Me.FormBorderStyle =
            FormBorderStyle.FixedToolWindow

        lblTitle.Text = title
        lblTitle.Font =
            New Font("Segoe UI", 11.0!, FontStyle.Bold)

        lblTitle.Left = 15
        lblTitle.Top = 15
        lblTitle.Width = 380
        lblTitle.Height = 30

        txtMessage.Left = 15
        txtMessage.Top = 50
        txtMessage.Width = 385
        txtMessage.Height = 170
        txtMessage.Multiline = True
        txtMessage.ReadOnly = True
        txtMessage.ScrollBars =
            ScrollBars.Vertical

        btnClose.Text = "Close"
        btnClose.Width = 90
        btnClose.Height = 30
        btnClose.Left = 310
        btnClose.Top = 230

        AddHandler btnClose.Click,
            Sub()
                Me.Close()
            End Sub

        Me.Controls.Add(lblTitle)
        Me.Controls.Add(txtMessage)
        Me.Controls.Add(btnClose)

    End Sub

End Class