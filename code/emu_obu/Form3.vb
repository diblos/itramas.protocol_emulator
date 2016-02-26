Imports System.Net.NetworkInformation

Public Class Form3

    Dim nSize As New Size(800, 350)
    Dim CLEARLIST As Boolean = True
    Dim dtTableSize As DataTable

    Dim WithEvents nCOM_TM As New ComClass
    Dim WithEvents nCOM_CT As New ComClass
    Dim WithEvents nCOM_RTS As New ComClass

    Dim counter As Integer = 0

    Dim cmn As New Common

    Dim DEVICE_COUNTER As Integer

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

        'If (DataGridView1.InvokeRequired) Then
        '    DataGridView1.Invoke( _
        '            New UpdateDataGridDelegate(AddressOf updateDGV), _
        '            New Object() {DataGridView1, DataSource})
        'Else
        '    DataGridView1.DataSource = DataSource
        '    DataGridView1.AutoResizeColumns()
        'End If

    End Sub

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If IsNothing(nCOM_TM) = False Then nCOM_TM.dispose()
            If IsNothing(dtTableSize) = False Then dtTableSize.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            End
        End Try
    End Sub

    Private Sub DisableForm(ByVal bool As Boolean)

        'ListTable.Enabled = bool
        'DataGridView1.Enabled = bool
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

        SetLabelStatus(Label_Stat_TM, DEVICE_LABEL_OFFLINE)
        SetLabelStatus(Label_Stat_CT, DEVICE_LABEL_OFFLINE)
        SetLabelStatus(Label_Stat_RTS, DEVICE_LABEL_OFFLINE)

        Dim minToTray As New clsMinToTray(Me, APPLICATION_NAME & " is running...", Me.Icon)

        SetComboValues()

        'DataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically
        'DataGridView1.AllowUserToAddRows = False

        'Button1.Enabled = False
        '=================================================
        'COM
        AddHandler nCOM_TM.OnResponse, AddressOf DataReceived_TM
        AddHandler nCOM_CT.OnResponse, AddressOf DataReceived_CT
        AddHandler nCOM_RTS.OnResponse, AddressOf DataReceived_RTS

        AddHandler btnCOM_TM.Click, AddressOf btnCOM_Click
        AddHandler btnCOM_CT.Click, AddressOf btnCOM_Click
        AddHandler btnCOM_RTS.Click, AddressOf btnCOM_Click

        AddHandler nCOM_TM.OnForwarding, AddressOf nCOM_OnForwarding
        AddHandler nCOM_CT.OnForwarding, AddressOf nCOM_OnForwarding
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

        SetCOM(ComboCOM_TM)
        SetCOM(ComboCOM_CT)
        SetCOM(ComboCOM_RTS)
        SetBaudRate(ComboBaud_TM)
        SetBaudRate(ComboBaud_CT)
        SetBaudRate(ComboBaud_RTS)

    End Sub

    Private Sub SetCOM(ByRef cb As ComboBox)
        With cb
            For i = 1 To 10
                .Items.Add("COM" & i)
            Next
            .SelectedIndex = 2
        End With
    End Sub

    Private Sub SetBaudRate(ByRef cb As ComboBox)
        With cb
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
            .SelectedIndex = 4
        End With
    End Sub

    Private Function getTimeString(ByVal seconds As Integer) As String
        Dim t As TimeSpan = TimeSpan.FromSeconds(seconds)
        'Return String.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", t.Hours, t.Minutes, t.Seconds, t.Milliseconds)
        Return String.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds)
    End Function

    Private Sub Form3_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Dim Top_Height As Double = 0.45
        Dim Bottom_Height As Double = 0.55
        Dim Width_Margin As Double = IIf(My.Computer.Info.OSVersion.StartsWith("5"), 3, 5) 'WIN7++ ADJUSTMENT

        Dim Left_Width As Double = 0.6
        Dim Right_Width As Double = 0.4

        With Group1
            .Location = New Point(Width_Margin, 0)
            .Height = Me.Height * Top_Height
            .Width = (Me.Width * Left_Width) - (2 * Width_Margin)
        End With

        With TabControl1
            '.Height = Group1.Height - (btnCOM_TM.Height + 25)
            .Dock = DockStyle.Fill
        End With

        With Group2
            .Location = New Point((Me.Width * Left_Width), 0)
            .Height = Me.Height * Top_Height
            .Width = (Me.Width * Right_Width) - (4 * Width_Margin)
        End With


        With GroupVerbose
            .Location = New Point(Width_Margin, Me.Height * Top_Height)
            .Width = Me.Width - (5 * Width_Margin)
            .Height = (Me.Height * Bottom_Height) - (30 + StatusStrip1.Height)
        End With

        With GroupSerial_TM
            .Width = (Group1.Width) - 8
            .Height = (TabTM.Height - btnCOM_TM.Height) - 10
        End With

        GroupBox_CT.Height = GroupSerial_TM.Height
        GroupBox_RTS.Height = GroupSerial_TM.Height

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

    Private Sub btnCOM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim baudrate As Integer
        Try
            Select Case btn.Name
                Case "btnCOM_TM"
                    If Not nCOM_TM.IsConnected Then
                        baudrate = CInt(ComboBaud_TM.Text)
                        nCOM_TM.DeviceMode = Common.Device.TaxiMeter
                        nCOM_TM.Connect(ComboCOM_TM.SelectedItem.Replace("COM", ""), baudrate)

                        If nCOM_TM.IsConnected = True Then
                            SetLabelStatus(Label_Stat_TM, DEVICE_LABEL_ONLINE)
                            setBehavior(connectionStat.connected, Common.Device.TaxiMeter)
                        Else
                            SetLabelStatus(Label_Stat_TM, DEVICE_LABEL_OFFLINE)
                            setBehavior(connectionStat.disconnected, Common.Device.TaxiMeter)
                        End If
                    Else
                        nCOM_TM.Disconnect()

                        If nCOM_TM.IsConnected = True Then
                            SetLabelStatus(Label_Stat_TM, DEVICE_LABEL_ONLINE)
                            setBehavior(connectionStat.connected, Common.Device.TaxiMeter)
                        Else
                            SetLabelStatus(Label_Stat_TM, DEVICE_LABEL_OFFLINE)
                            setBehavior(connectionStat.disconnected, Common.Device.TaxiMeter)
                        End If
                    End If
                    lstMsgs(nCOM_TM.PortName & " is " & IIf(nCOM_TM.IsConnected = True, "Connected", "Disconnected"))

                Case "btnCOM_CT"
                    If Not nCOM_CT.IsConnected Then
                        baudrate = CInt(ComboBaud_CT.Text)
                        nCOM_CT.DeviceMode = Common.Device.CashlessTerminal
                        nCOM_CT.Connect(ComboCOM_CT.SelectedItem.Replace("COM", ""), baudrate)

                        If nCOM_CT.IsConnected = True Then
                            SetLabelStatus(Label_Stat_CT, DEVICE_LABEL_ONLINE)
                            setBehavior(connectionStat.connected, Common.Device.CashlessTerminal)
                        Else
                            SetLabelStatus(Label_Stat_CT, DEVICE_LABEL_OFFLINE)
                            setBehavior(connectionStat.disconnected, Common.Device.CashlessTerminal)
                        End If
                    Else
                        nCOM_CT.Disconnect()

                        If nCOM_CT.IsConnected = True Then
                            SetLabelStatus(Label_Stat_CT, DEVICE_LABEL_ONLINE)
                            setBehavior(connectionStat.connected, Common.Device.CashlessTerminal)
                        Else
                            SetLabelStatus(Label_Stat_CT, DEVICE_LABEL_OFFLINE)
                            setBehavior(connectionStat.disconnected, Common.Device.CashlessTerminal)
                        End If
                    End If
                    lstMsgs(nCOM_CT.PortName & " is " & IIf(nCOM_CT.IsConnected = True, "Connected", "Disconnected"))

                Case "btnCOM_RTS"
                    If Not nCOM_RTS.IsConnected Then
                        baudrate = CInt(ComboBaud_RTS.Text)
                        nCOM_RTS.DeviceMode = Common.Device.RoofTopSignage
                        nCOM_RTS.Connect(ComboCOM_RTS.SelectedItem.Replace("COM", ""), baudrate)

                        If nCOM_RTS.IsConnected = True Then
                            SetLabelStatus(Label_Stat_RTS, DEVICE_LABEL_ONLINE)
                            setBehavior(connectionStat.connected, Common.Device.RoofTopSignage)
                        Else
                            SetLabelStatus(Label_Stat_RTS, DEVICE_LABEL_OFFLINE)
                            setBehavior(connectionStat.disconnected, Common.Device.RoofTopSignage)
                        End If
                    Else
                        nCOM_RTS.Disconnect()

                        If nCOM_RTS.IsConnected = True Then
                            SetLabelStatus(Label_Stat_RTS, DEVICE_LABEL_ONLINE)
                            setBehavior(connectionStat.connected, Common.Device.RoofTopSignage)
                        Else
                            SetLabelStatus(Label_Stat_RTS, DEVICE_LABEL_OFFLINE)
                            setBehavior(connectionStat.disconnected, Common.Device.RoofTopSignage)
                        End If
                    End If
                    lstMsgs(nCOM_RTS.PortName & " is " & IIf(nCOM_RTS.IsConnected = True, "Connected", "Disconnected"))

            End Select


        Catch ex As Exception
            lstMsgs(ex.Message)
        End Try
    End Sub

#Region "On Data Received Handlers"

    'TAXI METER
    Private Sub DataReceived_TM(ByVal nData As Object, ByVal e As Common.ReceiveEvents)
        Try
            Select Case e
                Case Common.ReceiveEvents.ERRORS
                    lstMsgs(nData)
                Case Common.ReceiveEvents.TIMEOUT
                    lstMsgs("Request time out!")
                Case Common.ReceiveEvents.DTO

                Case Common.ReceiveEvents.LOg
                    lstMsgs(nData)

            End Select

        Catch ex As Exception
            lstMsgs("DataReceived: " & ex.Message)
        End Try
    End Sub

    'CASHLESS TERMINAL
    Private Sub DataReceived_CT(ByVal nData As Object, ByVal e As Common.ReceiveEvents)
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

                Case Common.ReceiveEvents.LOG
                    lstMsgs(nData)

            End Select

        Catch ex As Exception
            lstMsgs("DataReceived: " & ex.Message)
        End Try
    End Sub

    'ROOF TOP SIGNAGE
    Private Sub DataReceived_RTS(ByVal nData As Object, ByVal e As Common.ReceiveEvents)
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

                Case Common.ReceiveEvents.LOG
                    lstMsgs(nData)

            End Select

        Catch ex As Exception
            lstMsgs("DataReceived: " & ex.Message)
        End Try
    End Sub
#End Region

    Private Sub setBehavior(ByVal status As connectionStat, ByVal Device As Common.Device)
        Select Case status
            Case connectionStat.connected
                Select Case Device
                    Case Common.Device.TaxiMeter
                        btnCOM_TM.Text = "Stop"
                        GroupSerial_TM.Enabled = False

                    Case Common.Device.CashlessTerminal
                        btnCOM_CT.Text = "Stop"
                        GroupBox_CT.Enabled = False

                    Case Common.Device.RoofTopSignage
                        btnCOM_RTS.Text = "Stop"
                        GroupBox_RTS.Enabled = False

                End Select
                DEVICE_COUNTER = +1

            Case connectionStat.disconnected
                Select Case Device
                    Case Common.Device.TaxiMeter
                        btnCOM_TM.Text = "Start"
                        GroupSerial_TM.Enabled = True

                    Case Common.Device.CashlessTerminal
                        btnCOM_CT.Text = "Start"
                        GroupBox_CT.Enabled = True

                    Case Common.Device.RoofTopSignage
                        btnCOM_RTS.Text = "Start"
                        GroupBox_RTS.Enabled = True

                End Select
                DEVICE_COUNTER = -1

        End Select

        UpdateStatus(IIf(DEVICE_COUNTER > 0, "Emulator Started", "Emulator Stopped"))

    End Sub

    Private Sub nCOM_OnEvent(ByVal nLog As Object, ByVal value As Common.TRX) Handles nCOM_TM.OnEvent
        UpdateStatus("[" & nCOM_TM.PortName & "] " & value.ToString & ": " & nLog)
        If CStr(nLog).StartsWith("ADHOC") Then

        End If
    End Sub

    Public Sub SetLabelStatus(ByRef LabelName As Label, ByVal devStatus As LabeldeviceStatus)
        With LabelName
            .Font = New Font("system", 8, FontStyle.Bold)
            .BackColor = devStatus.BackColor
            .ForeColor = devStatus.ForeColor
            .Text = devStatus.Text
        End With
    End Sub

    Private Sub nCOM_OnForwarding(ByVal Data() As Byte, ByVal destination As Common.Device)
        Try
            Select Case destination
                Case Common.Device.TaxiMeter
                    If nCOM_TM.IsConnected Then nCOM_TM.SendByte(Data, "Forwarding")
                Case Common.Device.CashlessTerminal
                    If nCOM_CT.IsConnected Then nCOM_CT.SendByte(Data, "Forwarding")
            End Select
        Catch ex As Exception

        End Try
    End Sub
End Class

Public Class LabeldeviceStatus
    Private _id As deviceStatus
    Private _backcolor As Color
    Private _forecolor As Color
    Private _text As String
    Public ReadOnly Property ID() As deviceStatus
        Get
            Return Me._id
        End Get
    End Property
    Public ReadOnly Property BackColor() As Color
        Get
            Return Me._backcolor
        End Get
    End Property
    Public ReadOnly Property ForeColor() As Color
        Get
            Return Me._forecolor
        End Get
    End Property
    Public ReadOnly Property Text() As String
        Get
            Return Me._text
        End Get
    End Property
    Public Sub New(ByVal ID As deviceStatus, ByVal BackColor As Color, ByVal ForeColor As Color, ByVal Text As String)
        Me._id = ID
        Me._backcolor = BackColor
        Me._forecolor = ForeColor
        Me._text = Text
    End Sub
End Class

Public Enum deviceStatus
    [Online]
    [Offline]
    [Error]
End Enum