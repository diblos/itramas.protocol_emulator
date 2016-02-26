Imports System.Net.NetworkInformation

Public Class Form3

    Dim nSize As New Size(800, 400)
    Dim CLEARLIST As Boolean = True
    Dim dtTableSize As DataTable

    'Private WithEvents _server As Server
    Dim WithEvents nCOM As New ComClass
    Dim WithEvents nTCP_1 As New TCPClass(Common.Device.DriverConsole)
    Dim WithEvents nTCP_2 As New TCPClass(Common.Device.RearSeatMonitor)

    Dim counter As Integer = 0

    Dim cmn As New Common


#Region "Enums"
    Private Enum connectionStat
        connected
        disconnected
    End Enum
#End Region

    Public Delegate Sub AddItemsToListBoxDelegate( _
                     ByVal ToListBox As ListBox, _
                     ByVal AddText As String)

    Public Delegate Sub updateItemsToListBoxDelegate( _
                 ByVal ToListBox As ListBox, _
                 ByVal AddText As String)

    Public Delegate Sub updateStatusDelegate( _
             ByVal ToListBox As StatusStrip, _
             ByVal AddText As String)

    Public Delegate Sub UpdateDataGridDelegate(ByVal DataGridView As DataGridView, ByVal DataSource As Object)

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

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If IsNothing(nCOM) = False Then nCOM.dispose()
            If IsNothing(dtTableSize) = False Then dtTableSize.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            End
        End Try
    End Sub

    Private Sub DisableForm(ByVal bool As Boolean)

        'ListTable.Enabled = bool
        DataGridView1.Enabled = bool
        'Button1.Enabled = bool

        If bool Then
            Me.Cursor = Cursors.Default
        Else
            Me.Cursor = Cursors.WaitCursor
        End If

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Initialization()
        Me.Text = APPLICATION_NAME & " ver " & cmn.ShowVersion
        Me.Size = nSize
        Me.MinimumSize = nSize
        Me.MaximumSize = nSize

        With Me.ListBox1
            .BackColor = Color.Black
            .ForeColor = Color.Lime
        End With

        Dim minToTray As New clsMinToTray(Me, APPLICATION_NAME & " is running...", Me.Icon)

        SetComboValues()

        DataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically
        DataGridView1.AllowUserToAddRows = False

        Button1.Enabled = False
        '=================================================
        RSC_DATA.ColorID = 1
        RSC_DATA.EffectID = 12
        RSC_DATA.Duration = 1000
        RSC_DATA.Message = "TEKS1M"
        UpdateDataGrid(RSC_DATA.DataCollection)

        'COM
        AddHandler nCOM.OnResponse, AddressOf DataReceived
        '=================================================
        'Dim cmmn As New Common
        'cmmn.DownloadFtpFiles()
        'cmmn.DownloadFtpFile("", DUMMY)

        'Dim data() As Byte = {&H1}
        'Dim h As Byte = cmn.GetCheckSum(data)

        'EASTER
        Dim x, y, z As Byte : x = &H14 : y = &H3 : z = x Xor y

        Debug.Print(x)
        Debug.Print(y)
        Debug.Print(z)
        'EASTER

        UpdateStatus("Ready")

    End Sub

    Private Sub SetComboValues()
        With ComboCOM
            For i = 1 To 10
                .Items.Add("COM" & i)
            Next
            .SelectedIndex = 2
        End With

        With ComboBaud
            .Items.Add("600")
            .Items.Add("1200")
            .Items.Add("2400")
            .Items.Add("4800")
            .Items.Add("9600")
            .Items.Add("14400")
            .Items.Add("19200")
            .Items.Add("38400")
            .Items.Add("56000")
            .Items.Add("57600")
            .Items.Add("115200")
            .SelectedIndex = .Items.Count - 1
        End With

        txtIP.Text = OBU_IP
        txtIP_RSM.Text = OBU_IP
        txtPort.Text = DRIVER_CONSOLE_PORT
        txtPort_RSM.Text = REAR_SEAT_MONITOR_PORT

        txtUID.Text = UID
        txtPWD.Text = PWD

        txtDriver.Text = DEFAULT_DRIVER_NAME
        txtDrvPwd.Text = DEFAULT_DRIVER_PASSWORD

        txtTrackerID_DC.Text = DEFAULT_TRACKERID
        txtPlate_DC.Text = DEFAULT_PLATENO

    End Sub

    Private Sub StartSimulator()
        If StartEmulator() Then
            'UpdateStatus("Emulator Started")
        Else
            'UpdateStatus("Emulator Not Started")
        End If
    End Sub

    Private Function StartEmulator() As Boolean
        Try
            UID = txtUID.Text
            PWD = txtPWD.Text
            OBU_IP = txtIP.Text
            OBU_IP = txtIP_RSM.Text
            DRIVER_CONSOLE_PORT = txtPort.Text
            REAR_SEAT_MONITOR_PORT = txtPort_RSM.Text
            Return True
        Catch ex As Exception
            lstMsgs("Start Emulator: " & ex.Message)
            Return False
        End Try
    End Function

    Private Function getTimeString(ByVal seconds As Integer) As String
        Dim t As TimeSpan = TimeSpan.FromSeconds(seconds)
        'Return String.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", t.Hours, t.Minutes, t.Seconds, t.Milliseconds)
        Return String.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds)
    End Function

    Private Sub Form3_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Dim Top_Height As Double = 0.5
        Dim Bottom_Height As Double = 0.5
        Dim Width_Margin As Double = IIf(My.Computer.Info.OSVersion.StartsWith("5"), 3, 5) 'WIN7++ ADJUSTMENT

        Dim Left_Width As Double = 0.5
        Dim Right_Width As Double = 0.5

        With Group1
            .Location = New Point(Width_Margin, 0)
            .Height = Me.Height * Top_Height
            .Width = (Me.Width * Left_Width) - (2 * Width_Margin)
        End With

        With TabControl1
            .Height = Group1.Height - (btnCOM.Height + 25)
        End With

        With Group2
            .Location = New Point((Me.Width * Left_Width), 0)
            .Height = Me.Height * Top_Height
            .Width = (Me.Width * Right_Width) - (4 * Width_Margin)
        End With

        With TabControl2
            '.Height = Group2.Height - (Button1.Height + 25)
        End With

        With GroupVerbose
            .Location = New Point(Width_Margin, Me.Height * Top_Height)
            .Width = Me.Width - (5 * Width_Margin)
            .Height = (Me.Height * Bottom_Height) - (30 + StatusStrip1.Height)
        End With

        With GroupSerial
            '.Width = (Group1.Width * 0.5) - 4
            .Width = (Group1.Width) - 8
        End With

        With GroupTCP
            .Width = (Group1.Width * 0.5) - 4
        End With

        btnContextUpdate_RSM.Location = New Point(TabControl2.Width * 0.2, TabControl2.Height * 0.35)

        txtIP.Left = GroupTCP.Width * 0.4
        txtIP.Width = (GroupTCP.Width * 0.9) - 5
        txtPort.Left = GroupTCP.Width * 0.4
        txtPort.Width = (GroupTCP.Width * 0.9) - 5

        txtIP_RSM.Left = txtIP.Left
        txtIP_RSM.Width = txtIP.Width
        txtPort_RSM.Left = txtPort.Left
        txtPort_RSM.Width = txtPort.Width

        gbDriver.Width = (TabControl2.Width * 0.5) - 10
        gbTaxi.Width = (TabControl2.Width * 0.5) - 10

        txtDriver.Width = (gbDriver.Width * 0.5) - 5
        txtDrvPwd.Width = (gbDriver.Width * 0.5) - 5

        txtTrackerID_DC.Width = (gbTaxi.Width * 0.5) - 5
        txtPlate_DC.Width = (gbTaxi.Width * 0.5) - 5

        DataGridView1.Height = Button1.Top - 8
    End Sub

    Private Sub ListTable_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DisableForm(False)
        Dim dListBox As ListBox = DirectCast(sender, ListBox)
        Dim str As String = dListBox.Items(dListBox.SelectedIndex)
        If str <> "" Then
            readSizeDT(str)
        End If
        GC.Collect()
        DisableForm(True)
    End Sub

    Private Sub readSizeDT(ByVal TableName As String)
        If IsNothing(dtTableSize) Then StatusStrip1.Text = "Table: " & TableName & " - info Not available."
        Dim row() As DataRow = dtTableSize.Select("TableName='" & TableName & "'")
        If row.Count > 0 Then
            UpdateStatus("Table: " & TableName & ", count: " & row(0)("NumberOfRows") & " row(s), size: " & row(0)("SizeinKB") & " KB")
        Else
            UpdateStatus("Table: " & TableName & " - info Not available.")
        End If
    End Sub

    Private Sub btnCOM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCOM.Click
        Try
            StartSimulator()
            If Not nCOM.IsConnected Then
                nCOM.Connect(ComboCOM.SelectedItem.Replace("COM", ""))
                nTCP_1.connect(OBU_IP, DRIVER_CONSOLE_PORT)
                nTCP_2.connect(OBU_IP, REAR_SEAT_MONITOR_PORT)
                If nCOM.IsConnected = True Then
                    setBehavior(connectionStat.connected)
                Else
                    setBehavior(connectionStat.disconnected)
                End If
            Else
                nCOM.Disconnect()
                nTCP_1.disconnect()
                nTCP_2.disconnect()
                If nCOM.IsConnected = True Then
                    setBehavior(connectionStat.connected)
                Else
                    setBehavior(connectionStat.disconnected)
                End If
            End If
            lstMsgs(nCOM.PortName & " is " & IIf(nCOM.IsConnected = True, "Connected", "Disconnected"))
        Catch ex As Exception
            lstMsgs(ex.Message)
        End Try
    End Sub

    Private Sub DataReceived(ByVal nData As Object, ByVal e As Common.ReceiveEvents)
        Try
            Select Case e
                Case Common.ReceiveEvents.ERRORS
                    lstMsgs(nData)
                Case Common.ReceiveEvents.TIMEOUT
                    lstMsgs("Request time out!")
                Case Common.ReceiveEvents.DTO
                    'Dim newRow As DataRow
                    'newRow = tmpData.NewRow
                    'newRow("NO") = tmpData.Rows.Count + 1
                    'newRow("GPSTIME") = nData.GPSTime
                    'newRow("FLOW") = nData.DataFlow
                    'newRow("LOG") = nData.LogData
                    'newRow("RETRIEVEDTIME") = nData.Received_Datetime
                    'tmpData.Rows.Add(newRow)

                    'newRow = Nothing

                    lstMsgs(nData)

            End Select

        Catch ex As Exception
            lstMsgs("DataReceived: " & ex.Message)
        End Try
    End Sub

    Private Sub setBehavior(ByVal status As connectionStat)
        Select Case status
            Case connectionStat.connected
                'OSStatus.FillColor = Color.LimeGreen
                'btnCOM.Text = "Disconnect"
                btnCOM.Text = "Stop"
                UpdateStatus("Emulator Started")
                Button1.Enabled = True
                GroupSerial.Enabled = False
                GroupTCP.Enabled = False
                GroupTCP2.Enabled = False
                GroupFTP.Enabled = False
                TabControl2.Enabled = True
            Case connectionStat.disconnected
                'OSStatus.FillColor = Color.Red
                'btnCOM.Text = "Connect"
                btnCOM.Text = "Start"
                UpdateStatus("Emulator Stopped")
                Button1.Enabled = False
                GroupSerial.Enabled = True
                GroupTCP.Enabled = True
                GroupTCP2.Enabled = True
                GroupFTP.Enabled = True
                TabControl2.Enabled = False
        End Select
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            nCOM.CurrentMessage()
        Catch ex As Exception
            lstMsgs(ex.Message)
        End Try
    End Sub

    Private Sub nCOM_OnEvent(ByVal nLog As Object, ByVal value As Common.TRX) Handles nCOM.OnEvent
        UpdateStatus("[" & nCOM.PortName & "] " & value.ToString & ": " & nLog)
        If CStr(nLog).StartsWith("ADHOC") Then
            UpdateDataGrid(RSC_DATA.DataCollection)
        End If
    End Sub

    Private Sub nTCP_1_OnEvent(ByVal nLog As Object, ByVal value As Common.TRX) Handles nTCP_1.OnEvent
        UpdateStatus("[TCP:" & nTCP_1.Port & "] " & value.ToString & ": " & nLog)
    End Sub

    Private Sub nTCP_2_OnEvent(ByVal nLog As Object, ByVal value As Common.TRX) Handles nTCP_2.OnEvent
        UpdateStatus("[TCP:" & nTCP_2.Port & "] " & value.ToString & ": " & nLog)
    End Sub

    Private Sub nTCP_OnResponse(ByVal nLog As Object, ByVal value As Common.ReceiveEvents) Handles nTCP_1.OnResponse, nTCP_2.OnResponse
        Select Case value
            Case Common.ReceiveEvents.ERRORS
                lstMsgs(nLog)
            Case Else
                lstMsgs(nLog)
        End Select
    End Sub

    Private Sub cbSOS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSOS_DC.CheckedChanged, cbSOS_RSM.CheckedChanged
        Dim cbSOS As CheckBox = DirectCast(sender, CheckBox)
        Select Case cbSOS.Name
            Case "cbSOS_RSM"
                If (cbSOS.Checked) Then
                    nTCP_2.SendSOS(True, Common.Device.RearSeatMonitor)
                Else
                    nTCP_2.SendSOS(False, Common.Device.RearSeatMonitor)
                End If
            Case "cbSOS_DC"
                If (cbSOS.Checked) Then
                    nTCP_1.SendSOS(True, Common.Device.DriverConsole)
                Else
                    nTCP_1.SendSOS(False, Common.Device.DriverConsole)
                End If
        End Select
    End Sub

    Private Sub btnContextUpdate_RSM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContextUpdate_RSM.Click
        nTCP_2.SendRequestUpdate(Common.Device.RearSeatMonitor, Common.RequestUpdateType.ContextUpdate)
    End Sub

    Private Sub btnLoginDrv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoginDrv.Click
        DEFAULT_DRIVER_NAME = IIf(txtDriver.Text.Trim = String.Empty, DEFAULT_DRIVER_NAME, txtDriver.Text.Trim)
        DEFAULT_DRIVER_PASSWORD = IIf(txtDrvPwd.Text.Trim = String.Empty, DEFAULT_DRIVER_PASSWORD, txtDrvPwd.Text.Trim)
        nTCP_1.SendLogin(DEFAULT_DRIVER_NAME, DEFAULT_DRIVER_PASSWORD)
    End Sub

    Private Sub btnValidatetaxi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnValidatetaxi.Click
        nTCP_1.SendTaxiValidateReq()
    End Sub

    Private Sub btnUpdateTaxi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateTaxi.Click
        DEFAULT_TRACKERID = IIf(txtTrackerID_DC.Text.Trim = String.Empty, DEFAULT_TRACKERID, txtTrackerID_DC.Text.Trim)
        DEFAULT_PLATENO = IIf(txtPlate_DC.Text.Trim = String.Empty, DEFAULT_PLATENO, txtPlate_DC.Text.Trim)
        nTCP_1.SendUpdateTaxiProfile(DEFAULT_TRACKERID, DEFAULT_PLATENO)
    End Sub

End Class
