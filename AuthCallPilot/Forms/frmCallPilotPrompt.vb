Imports System.Drawing
Imports System.Windows.Forms

Public NotInheritable Class frmCallPilotPrompt
    Inherits Form

    Private ReadOnly lblMessage As Label
    Private ReadOnly pnlButtons As FlowLayoutPanel
    Private _result As DialogResult = DialogResult.None

    Private Sub New(title As String, message As String, buttons As MessageBoxButtons, icon As MessageBoxIcon)
        Text = title
        FormBorderStyle = FormBorderStyle.FixedDialog
        StartPosition = FormStartPosition.CenterParent
        ShowInTaskbar = False
        MaximizeBox = False
        MinimizeBox = False
        TopMost = True
        ClientSize = New Size(520, 230)
        Font = New Font("Segoe UI", 9.0!)

        lblMessage = New Label With {
            .Text = message,
            .AutoSize = False,
            .Location = New Point(25, 25),
            .Size = New Size(470, 135),
            .Font = New Font("Segoe UI", 10.0!),
            .TextAlign = ContentAlignment.MiddleLeft
        }

        pnlButtons = New FlowLayoutPanel With {
            .FlowDirection = FlowDirection.RightToLeft,
            .Location = New Point(25, 175),
            .Size = New Size(470, 40)
        }

        Controls.Add(lblMessage)
        Controls.Add(pnlButtons)

        AddPromptButtons(buttons)
    End Sub

    Private Sub AddPromptButtons(buttons As MessageBoxButtons)
        Select Case buttons
            Case MessageBoxButtons.OK
                AddButton("OK", DialogResult.OK)

            Case MessageBoxButtons.OKCancel
                AddButton("Cancel", DialogResult.Cancel)
                AddButton("OK", DialogResult.OK)

            Case MessageBoxButtons.YesNo
                AddButton("No", DialogResult.No)
                AddButton("Yes", DialogResult.Yes)

            Case MessageBoxButtons.YesNoCancel
                AddButton("Cancel", DialogResult.Cancel)
                AddButton("No", DialogResult.No)
                AddButton("Yes", DialogResult.Yes)

            Case Else
                AddButton("OK", DialogResult.OK)
        End Select
    End Sub

    Private Sub AddButton(text As String, result As DialogResult)
        Dim button As New Button With {
            .Text = text,
            .Width = 95,
            .Height = 32,
            .Tag = result
        }

        AddHandler button.Click, AddressOf PromptButton_Click
        pnlButtons.Controls.Add(button)
    End Sub

    Private Sub PromptButton_Click(sender As Object, e As EventArgs)
        Dim button As Button = TryCast(sender, Button)
        If button Is Nothing Then Return

        _result = DirectCast(button.Tag, DialogResult)
        DialogResult = _result
        Close()
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        TopMost = True
        BringToFront()
        Activate()
    End Sub

    Public Shared Function ShowPrompt(owner As IWin32Window, title As String, message As String, buttons As MessageBoxButtons, icon As MessageBoxIcon) As DialogResult
        Using prompt As New frmCallPilotPrompt(title, message, buttons, icon)
            Return prompt.ShowDialog(owner)
        End Using
    End Function
End Class