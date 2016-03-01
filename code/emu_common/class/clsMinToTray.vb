Imports System.Windows.Forms
Imports System.Drawing
'''''''''''''''''''''''''''''''
'   Created By: Chris Renfrow '
'   Date:       07/01/2005    '
'''''''''''''''''''''''''''''''

Public Class clsMinToTray
    'Var for context menu to add to icon in system tray
    'Placed here to be accessible from calling form (to add menu items)
    Public WithEvents trayMenu As New ContextMenu
    'Var for optional property of window state on restore (default normal)
    'Var for icon to be in system tray
    Private WithEvents trayIcon As New NotifyIcon
    'Var to hold calling app's main form
    Private WithEvents mainForm As New Form
    'Var for determining if app is visible (whether in tray or not)
    Private visible As Boolean = True

    'Constructor
    'Takes three parameters
    '1)The calling main form of application (me)            --mandatory
    '2)The text to display when you mouse over icon in tray --optional
    '3)A custom icon for the tray (defaults to apps icon)   --optional
    Sub New(ByVal theForm As Form, Optional ByVal iconText As String = "", _
    Optional ByVal icon As Icon = Nothing)
        'assign passed form to global var
        mainForm = theForm
        'add two main menu items to traymenu and event handlers
        trayMenu.MenuItems.Add("Restore", AddressOf restore)
        trayMenu.MenuItems.Add("Close", AddressOf close)
        'add event handler for passed main form to execute
        AddHandler mainForm.SizeChanged, AddressOf execute
        'add event handler for double clicking the icon in the tray (same as menu item) 
        AddHandler trayIcon.DoubleClick, AddressOf restore
        'properties for trayicon
        '-hide
        '-if icon passed, assign - otherwise assign default icon
        '-assign passed text - if none then will be blank
        '-assign context menu
        With trayIcon
            .Visible = False
            If IsNothing(icon) Then
                .Icon = mainForm.Icon
            Else
                .Icon = icon
            End If
            .Text = iconText
            .ContextMenu = trayMenu
        End With
    End Sub

    Private Sub restore(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'on restore
        'show show in taskbar (this can be buggy in .NET 2.0 - not sure about others
        mainForm.ShowInTaskbar = True
        'application is now visible
        visible = True
        'hide the icon in the system tray 
        trayIcon.Visible = False
        'restore window to user's defined state (or default normal)
        mainForm.WindowState = FormWindowState.Normal  'restoreWindowState
    End Sub

    Private Sub close(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Hide icon in tray
        trayIcon.Visible = False
        'Close passed main form (should trigger main forms closing event)
        mainForm.Close()
    End Sub

    Private Sub execute(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'This fires on form size changine so...
        'If form is visible and is being minimized, then 
        '-hide app in taskbar
        '-show icon in system tray
        If visible And mainForm.WindowState = FormWindowState.Minimized Then
            visible = False
            mainForm.ShowInTaskbar = False
            trayIcon.Visible = True
        End If
    End Sub

End Class

