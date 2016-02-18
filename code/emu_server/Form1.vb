Public Class Form1

    Dim nSize As New Size(700, 400)
    Dim CLEARLIST As Boolean = True

    Private WithEvents _server As Server
    Dim kommon As New Common

#Region "Form Delegates"

    Public Delegate Sub AddItemsToListBoxDelegate(ByVal ToListBox As ListBox, ByVal AddText As String)
    Public Delegate Sub updateItemsToListBoxDelegate(ByVal ToListBox As ListBox, ByVal AddText As String)
    Public Delegate Sub updateStatusDelegate(ByVal ToListBox As StatusStrip, ByVal AddText As String)
    Public Delegate Sub UpdateDataGridDelegate(ByVal DataGridView As DataGridView, ByVal DataSource As Object)
    Public Delegate Sub SelectRowDataGridDelegate(ByVal DataGridView As DataGridView, ByVal SelectIndex As Object)

    Private Sub AddItemsToListBox(ByVal ToListBox As ListBox, _
                                 ByVal AddText As String)
        If ToListBox.Items.Count > 1000 And CLEARLIST = True Then
            ToListBox.Items.Clear()
        End If

        ToListBox.Items.Add(AddText)
        ToListBox.SetSelected(ListBox1.Items.Count - 1, True)
        ToListBox.SetSelected(ListBox1.Items.Count - 1, False)
    End Sub

    Public Sub lstMsgs(ByVal item As Object, Optional ByVal source As Object = Nothing)
        If (ListBox1.InvokeRequired) Then
            ListBox1.Invoke( _
                    New AddItemsToListBoxDelegate(AddressOf AddItemsToListBox), _
                    New Object() {ListBox1, CStr(item)})
        Else
            If Me.ListBox1.Items.Count > 1000 And CLEARLIST = True Then
                ListBox1.Items.Clear()
            End If

            Me.ListBox1.Items.Add(CStr(item))
            Me.ListBox1.SetSelected(ListBox1.Items.Count - 1, True)
            Me.ListBox1.SetSelected(ListBox1.Items.Count - 1, False)
        End If

    End Sub

    Private Sub AddItemsToStatus(ByVal StatusStrips As StatusStrip, _
                             ByVal AddText As String)
        StatusStrips.Items.Clear()
        StatusStrips.Items.Add(AddText)
    End Sub

    Private Sub updateListBox(ByVal ToListBox As ListBox, _
                             ByVal AddText As String)
        ToListBox.Items(ToListBox.Items.Count - 1) = ToListBox.Items(ToListBox.Items.Count - 1) & " " & AddText
        ToListBox.SetSelected(ListBox1.Items.Count - 1, True)
        ToListBox.SetSelected(ListBox1.Items.Count - 1, False)
    End Sub

    Public Sub UpdateMsgs(ByVal item As Object, Optional ByVal source As Object = Nothing)
        If (ListBox1.InvokeRequired) Then
            ListBox1.Invoke( _
                    New updateItemsToListBoxDelegate(AddressOf updateListBox), _
                    New Object() {ListBox1, CStr(item)})
        Else
            Me.ListBox1.Items(Me.ListBox1.Items.Count - 1) = Me.ListBox1.Items(Me.ListBox1.Items.Count - 1) & " " & item
            Me.ListBox1.SetSelected(ListBox1.Items.Count - 1, True)
            Me.ListBox1.SetSelected(ListBox1.Items.Count - 1, False)
        End If

    End Sub

    Private Sub UpdateStatus(ByVal AddText As String)
        If (StatusStrip1.InvokeRequired) Then
            StatusStrip1.Invoke( _
                    New updateStatusDelegate(AddressOf AddItemsToStatus), _
                    New Object() {StatusStrip1, CStr(AddText)})
        Else
            Me.StatusStrip1.Items.Clear()
            Me.StatusStrip1.Items.Add(AddText)
        End If
    End Sub

    Private Sub updateDGV(ByVal dgv As DataGridView, _
                     ByVal DataSource As Object)
        dgv.DataSource = DataSource
        dgv.AutoResizeColumns()
    End Sub

    Private Sub UpdateDataGrid(ByVal DataSource As Object)

        If (DataGridView1.InvokeRequired) Then
            DataGridView1.Invoke( _
                    New UpdateDataGridDelegate(AddressOf updateDGV), _
                    New Object() {DataGridView1, DataSource})
        Else
            DataGridView1.DataSource = DataSource
            DataGridView1.AutoResizeColumns()
        End If

    End Sub

    Private Sub selectRowDGV(ByVal dgv As DataGridView, _
                 ByVal selectIndex As Object)
        For Each row As DataGridViewRow In dgv.Rows
            If row.Index = selectIndex Then
                row.Selected = True
                Exit For
            End If
        Next
        dgv.Focus()
    End Sub

    Private Sub DGVSelectRow(ByVal selectIndex As Integer)
        If (DataGridView1.InvokeRequired) Then
            DataGridView1.Invoke( _
                    New SelectRowDataGridDelegate(AddressOf selectRowDGV), _
                    New Object() {DataGridView1, selectIndex})
        Else
            Button1.Focus()
            For Each row As DataGridViewRow In DataGridView1.Rows
                If row.Index = selectIndex Then
                    row.Selected = True
                    Exit For
                End If
            Next
            DataGridView1.Focus()
        End If
    End Sub

#End Region

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If IsNothing(_server) = False Then
                _server.Stop()
                _server = Nothing
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            GC.Collect()
            End
        End Try
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Initialization()
        Me.Text = APPLICATION_NAME & " - server version " & kommon.ShowVersion
        Me.Size = nSize
        Me.MinimumSize = nSize
        Me.MaximumSize = nSize

        With Me.ListBox1
            .BackColor = Color.Black
            .ForeColor = Color.Lime
        End With

        DataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically
        DataGridView1.AllowUserToAddRows = False

        Dim minToTray As New clsMinToTray(Me, APPLICATION_NAME & " is running...", Me.Icon)

        GetIPAddress()
        _server = New Server(Convert.ToInt64(SERVER_PORT))

        UpdateStatus("Ready")

        UpdateDataGrid(ClientGroup)

    End Sub

    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Dim Top_Height As Double = 0.45
        Dim Bottom_Height As Double = 0.55
        Dim Width_Margin As Double = IIf(My.Computer.Info.OSVersion.StartsWith("5"), 3, 5) 'WIN7++ ADJUSTMENT

        Dim Left_Width As Double = 0.5
        Dim Right_Width As Double = 0.5

        With TabControl2
            .Location = New Point(Width_Margin, 0)
            .Width = Me.Width - (5 * Width_Margin)
            '.Height = (Me.Height * Bottom_Height) - (30 + StatusStrip1.Height)
            .Height = (Me.Height * Top_Height) - 5
        End With

        With GroupVerbose
            .Location = New Point(Width_Margin, Me.Height * Top_Height)
            .Width = Me.Width - (5 * Width_Margin)
            .Height = (Me.Height * Bottom_Height) - (30 + StatusStrip1.Height)
        End With

        DataGridView1.Height = TabClient.Height - (Button1.Height + 10)

    End Sub

    Private Sub _server_ClientEvent(ByVal client As Client) Handles _server.ClientEvent
        updateDT(client)
        UpdateDataGrid(ClientGroup)
    End Sub

    Private Sub _server_OnEvent(ByVal nLog As Object, ByVal value As Common.TRX) Handles _server.OnEvent
        lstMsgs(nLog)
    End Sub

    Private Sub _server_ReceiveData(ByVal data As String) Handles _server.ReceiveData
        If data.StartsWith("22:00:") Then 'AVL DATA

            'lstMsgs(kommon.ParseData(data).GetTrackerID & vbTab & data.ToString)
            lstMsgs(ENTITY_OBU & "AVL " & data.ToString)

        ElseIf data.StartsWith("08:") Then 'OBU ACKNOWLEDGE

            lstMsgs(ENTITY_OBU & "ACK " & data.ToString)

        ElseIf data.StartsWith("23:") Then 'OBU Reply Rooftop Signage Message Request

            lstMsgs(ENTITY_OBU & "Rooftop " & data.ToString)

        ElseIf data.StartsWith("31:") Then 'OBU Send SALE Transaction Record to Server (Generic Protocol)

            lstMsgs(ENTITY_OBU & "Sale Transaction " & data.ToString)

        ElseIf data.StartsWith("32:") Then 'OBU Send VOID Transaction Record to Server (Generic Protocol)

            lstMsgs(ENTITY_OBU & "Void Transaction " & data.ToString)

        ElseIf data.StartsWith("41:") Then 'OBU Request Driver Information to Server

            lstMsgs(ENTITY_OBU & "Driver Info Request " & data.ToString)

        ElseIf data.StartsWith("42:") Then 'OBU Update Driver Information via FTP

            lstMsgs(ENTITY_OBU & "Driver Info " & data.ToString)

        ElseIf data.StartsWith("51:") Then 'OBU Send Taxi Profile Data to Server

            lstMsgs(ENTITY_OBU & "Taxi Profile " & data.ToString)

        ElseIf data.StartsWith("61:") Then 'OBU Send Driver Behaviour Data to Server

            lstMsgs(ENTITY_OBU & "Driver Behaviour " & data.ToString)

        ElseIf data.StartsWith("01:") Then 'OBU Send Connection Request

            lstMsgs(ENTITY_OBU & "PING " & data.ToString)

        ElseIf data.StartsWith("02:") Then 'OBU Send Ping

            lstMsgs(ENTITY_OBU & "PING " & data.ToString)

        Else 'UNKNOWN

            lstMsgs(ENTITY_OBU & "UNKNOWN " & data.ToString)

        End If
    End Sub

#Region "Private Methods"
    Private Sub GetIPAddress()
        Dim strHostName, strIPAddress As String
        strHostName = System.Net.Dns.GetHostName()
        strIPAddress = System.Net.Dns.GetHostEntry(strHostName).AddressList(0).ToString()
        lstMsgs("Host Name: " & strHostName & "; IP Address: " & strIPAddress)
        txtIPAddress.Text = strIPAddress
        txtPort.Text = SERVER_PORT
    End Sub

    Private Function updateDT(ByVal Client As Client) As IncomingStatus
        Dim row() As DataRow = ClientGroup.Select("TrackerID='" & Client.TrackerID & "'")
        If row.Count > 0 Then
            row(0)("IPAddress") = Client.IPAddress
            row(0)("Timestamp") = Client.Timestamp
            Return IncomingStatus.Updated
        Else
            Dim _row As DataRow
            _row = ClientGroup.NewRow
            _row("TrackerID") = Client.TrackerID
            _row("IPAddress") = Client.IPAddress
            _row("Timestamp") = Client.Timestamp
            ClientGroup.Rows.Add(_row)
            Return IncomingStatus.Inserted
        End If
    End Function
#End Region

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            test()
        Catch ex As Exception
            lstMsgs(ex.Message)
        End Try
    End Sub

    Private Sub test()
        'Console.WriteLine(DataGridView1.SelectedRows.Count)
        '_server.TestSend("192.168.9.178")
        'lstMsgs(_server.SendRoofTopMessage("TEKS1M", emu_common.Common.RooftopSignageColor.Green, emu_common.Common.RooftopSignageEffect.Blink, 1000))

        lstMsgs(_server.SendAcknowledge("123", emu_common.Common.Acknowledgement.Success))

    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 Then
            DGVSelectRow(e.RowIndex)
        End If
    End Sub

    Private Sub DataGridView1_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles DataGridView1.RowsAdded
        Dim dgv As DataGridView = DirectCast(sender, DataGridView)
        If dgv.RowCount <= 0 Then
            Exit Sub
        Else
            Dim index As Integer = 0
            For Each row As DataGridViewRow In dgv.Rows
                For Each cell As DataGridViewCell In row.Cells
                    If cell.Selected = True Then
                        index = row.Index
                        DGVSelectRow(index)
                        Exit Sub
                    End If
                Next
            Next
        End If
    End Sub
End Class
