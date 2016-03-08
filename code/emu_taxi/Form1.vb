Public Class Form1

    Dim nTitle As String
    Dim nSize As New Size(600, 400)
    Dim cmn As New Common
    Dim CLEARLIST As Boolean = True

    Dim WithEvents nCOM As New ComClass

    Public Delegate Sub AddItemsToListBoxDelegate(ByVal ToListBox As ListBox, ByVal AddText As String)
    Public Delegate Sub updateItemsToListBoxDelegate(ByVal ToListBox As ListBox, ByVal AddText As String)
    Public Delegate Sub updateStatusDelegate(ByVal ToListBox As StatusStrip, ByVal AddText As String)
    Public Delegate Sub UpdateDataGridDelegate(ByVal DataGridView As DataGridView, ByVal DataSource As Object)
    Public Delegate Sub disableFormDelegate(ByVal mForm As Form1, ByVal bool As Boolean)

    Dim RESET_INTERVAL As Integer = 500

    Dim EMUMODE As EmulatorMode = EmulatorMode.TaxiMeter
    Dim EMUSTART As Boolean = False

    Dim EMUTimer As System.Timers.Timer
    Dim EMUCOUNTER As Integer

#Region "Enums"
    Private Enum connectionStat
        connected
        disconnected
    End Enum
    Private Enum EmulatorMode
        TaxiMeter
        CashlessTerminal
    End Enum
#End Region
#Region "Delegates"
    Private Sub AddItemsToListBox(ByVal ToListBox As ListBox, ByVal AddText As String)
        If ToListBox.Items.Count > 1000 And CLEARLIST = True Then
            ToListBox.Items.Clear()
        End If
        ToListBox.Items.Add(AddText)
        ToListBox.SetSelected(ListBox1.Items.Count - 1, True)
        ToListBox.SetSelected(ListBox1.Items.Count - 1, False)
    End Sub

    Public Sub lstMsgs(ByVal item As Object, Optional ByVal source As Object = Nothing)
        If (ListBox1.InvokeRequired) Then
            ListBox1.Invoke(New AddItemsToListBoxDelegate(AddressOf AddItemsToListBox), New Object() {ListBox1, CStr(item)})
        Else
            If Me.ListBox1.Items.Count > 1000 And CLEARLIST = True Then
                ListBox1.Items.Clear()
            End If
            Me.ListBox1.Items.Add(CStr(item))
            Me.ListBox1.SetSelected(ListBox1.Items.Count - 1, True)
            Me.ListBox1.SetSelected(ListBox1.Items.Count - 1, False)
        End If
    End Sub

    Private Sub AddItemsToStatus(ByVal StatusStrips As StatusStrip, ByVal AddText As String)
        StatusStrips.Items.Clear()
        StatusStrips.Items.Add(AddText)
    End Sub

    Private Sub updateListBox(ByVal ToListBox As ListBox, ByVal AddText As String)
        ToListBox.Items(ToListBox.Items.Count - 1) = ToListBox.Items(ToListBox.Items.Count - 1) & " " & AddText
        ToListBox.SetSelected(ListBox1.Items.Count - 1, True)
        ToListBox.SetSelected(ListBox1.Items.Count - 1, False)
    End Sub

    Public Sub UpdateMsgs(ByVal item As Object, Optional ByVal source As Object = Nothing)
        If (ListBox1.InvokeRequired) Then
            ListBox1.Invoke(New updateItemsToListBoxDelegate(AddressOf updateListBox), New Object() {ListBox1, CStr(item)})
        Else
            Me.ListBox1.Items(Me.ListBox1.Items.Count - 1) = Me.ListBox1.Items(Me.ListBox1.Items.Count - 1) & " " & item
            Me.ListBox1.SetSelected(ListBox1.Items.Count - 1, True)
            Me.ListBox1.SetSelected(ListBox1.Items.Count - 1, False)
        End If
    End Sub

    Private Sub UpdateStatus(ByVal AddText As String)
        If (StatusStrip1.InvokeRequired) Then
            StatusStrip1.Invoke(New updateStatusDelegate(AddressOf AddItemsToStatus), New Object() {StatusStrip1, CStr(AddText)})
        Else
            Me.StatusStrip1.Items.Clear()
            Me.StatusStrip1.Items.Add(AddText)
        End If
    End Sub

    Private Sub disableFormElements(ByVal mForm As Form1, _
                         ByVal bool As Boolean)

        mForm.Cursor = IIf(bool, Cursors.Default, Cursors.WaitCursor)
        mForm.btnCOM.Enabled = bool
        mForm.btnTest.Enabled = bool

    End Sub

    Private Sub DisableForm(ByVal bool As Boolean)

        If (Me.InvokeRequired) Then
            Me.Invoke( _
                    New disableFormDelegate(AddressOf disableFormElements), _
                    New Object() {Me, bool})
        Else

            Me.Cursor = IIf(bool, Cursors.Default, Cursors.WaitCursor)
            Me.btnCOM.Enabled = bool
            Me.btnTest.Enabled = bool

        End If

    End Sub
#End Region

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Initialization()
        nTitle = APPLICATION_NAME & " ver " & cmn.ShowVersion
        Me.Text = nTitle & " - " & TabControl1.SelectedTab.Text
        Me.Size = nSize
        Me.MinimumSize = nSize
        Me.MaximumSize = nSize

        With Me.ListBox1
            .BackColor = Color.Black
            .ForeColor = Color.Lime
        End With
        SetComboValues()

        EMUTimer = New System.Timers.Timer(RESPONSE_TIME)
        AddHandler EMUTimer.Elapsed, AddressOf EmulatorRoutine

        AddHandler btnCOM.Click, AddressOf btnCOM_Click
        AddHandler btnCOM2.Click, AddressOf btnCOM_Click
        AddHandler btnTest.Click, AddressOf btnTest_Click
        AddHandler btnTest2.Click, AddressOf btnTest_Click

        AddHandler TabControl1.SelectedIndexChanged, AddressOf TabControl1_SelectedIndexChanged
        AddHandler nCOM.OnResponse, AddressOf DataReceived

        AddHandler ListBox1.DoubleClick, AddressOf ListBox1_DoubleClick

        UpdateStatus("Ready")
    End Sub

    Private Sub SetComboValues()

        SetCOM(ComboCOM)
        SetCOM(ComboCOM2)
        SetBaudRate(ComboBaud)
        SetBaudRate(ComboBaud2)

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

    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
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
            .Height = Group1.Height - 25
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

    End Sub

    Private Sub btnCOM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim baudrate As Integer
        Try
            If Not nCOM.IsConnected Then
                If EMUMODE = EmulatorMode.TaxiMeter Then
                    baudrate = CInt(ComboBaud.Text)
                    nCOM.Connect(ComboCOM.SelectedItem.Replace("COM", ""), baudrate)
                Else
                    baudrate = CInt(ComboBaud2.Text)
                    nCOM.Connect(ComboCOM2.SelectedItem.Replace("COM", ""), baudrate)
                End If

                If nCOM.IsConnected = True Then
                    setBehavior(connectionStat.connected)
                Else
                    setBehavior(connectionStat.disconnected)
                End If
            Else
                nCOM.Disconnect()
                If nCOM.IsConnected = True Then
                    setBehavior(connectionStat.connected)
                Else
                    setBehavior(connectionStat.disconnected)
                End If
            End If

            If EMUMODE = EmulatorMode.TaxiMeter Then
                nCOM.DeviceMode = Common.Device.TaxiMeter
            Else
                nCOM.DeviceMode = Common.Device.CashlessTerminal
            End If

            lstMsgs(nCOM.PortName & " is " & IIf(nCOM.IsConnected = True, "Connected", "Disconnected"))
            If nCOM.IsConnected = True Then lstMsgs("Emulator mode: " & EMUMODE.ToString)
        Catch ex As Exception
            lstMsgs(ex.Message)
        End Try
    End Sub

    'Start emulator test
    Private Sub btnTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        lstMsgs("Test started.")
        DisableForm(False)

        Select Case EMUMODE
            Case EmulatorMode.TaxiMeter
                '        1.	A button so simulate and send sale transaction from Taxi Meter (emulator) to Cashless Terminal (via OBU as repeater)
                '02003536303030303030303030313032303030301C343000123030303030303030303330301C0316

                EMUTimer.Start()

                '                Taxi Meter (Emulator) expect to receive acknowledgment 06

                '                If not receive acknowledgment within 2 sec, resend once.

                '                If 2nd resend still not receive acknowledgment, discard.

                '2.	Reply 2A5452503423281606011629162C010000000000000000000000000000000000000000000000000001000100010000000000000A2307
                'after read “Send Report” command 2A646E05050102 from OBU

            Case EmulatorMode.CashlessTerminal
                '                1.	Reply acknowledgement 06
                'After receive sale transaction from Taxi Meter (via OBU as repeater)
                '02003536303030303030303030313032303030301C343000123030303030303030303330301C0316
                System.Threading.Thread.Sleep(5000)
                EMUTimer.Start()
                '2.	Create a time 5 sec. After 5 sec send “Approval” data to Taxi Meter (via OBU)
                '02031636303030303030303030313132303030301C30320040415050524F56414C20202020202058585858585820202020202020202020202020202020202020201C4430006054455354205445524D494E414C202020202020204C494E45203120202020202020202020202020204C494E45203220202020202020202020202020201C303300063136303132371C303400063132333635321C303100065858585858581C363500063939343033351C3136000831303031303030311C443100153939393939303030303031323334351C44320010564953412020202020201C333000162A2A2A2A2A2A2A2A2A2A2A2A393531321C33310004313930361C353000063030303030311C443300120000000000000000000000001C4434000230341C4435002654414E20575549204B49415420202020202020202020202000001C0309

                '3.	If Cashless Terminal (Emulator) not receive acknowledgment from Taxi Meter (via OBU) within 2 sec,
                'Resend “Approval” data

                'Resend attempt 9 times if no receive acknowledgment.

                'If receive acknowledgment, discard.

        End Select

    End Sub

    Private Sub EmulatorRoutine(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
        EMUTimer.Stop()
        Select Case EMUMODE
            Case EmulatorMode.TaxiMeter
                If EMUCOUNTER < 2 Then
                    lstMsgs(IIf(EMUCOUNTER > 0, "Resending", "Sending") & " sale transaction.")
                    nCOM.SendSaleTransaction()
                    EMUCOUNTER = EMUCOUNTER + 1
                    EMUTimer.Interval = 2000
                    EMUTimer.Start()
                Else
                    EMUTimer.Interval = RESET_INTERVAL
                    EMUCOUNTER = 0
                    lstMsgs("Test finished.")
                    UpdateStatus("Emulator Ready")
                    DisableForm(True)
                    Exit Sub
                End If

            Case EmulatorMode.CashlessTerminal
                If EMUCOUNTER < 9 Then
                    lstMsgs(IIf(EMUCOUNTER > 0, "Resending", "Sending") & " sale approval.")
                    '02031636303030303030303030313132303030301C30320040415050524F56414C20202020202058585858585820202020202020202020202020202020202020201C4430006054455354205445524D494E414C202020202020204C494E45203120202020202020202020202020204C494E45203220202020202020202020202020201C303300063136303132371C303400063132333635321C303100065858585858581C363500063939343033351C3136000831303031303030311C443100153939393939303030303031323334351C44320010564953412020202020201C333000162A2A2A2A2A2A2A2A2A2A2A2A393531321C33310004313930361C353000063030303030311C443300120000000000000000000000001C4434000230341C4435002654414E20575549204B49415420202020202020202020202000001C0309
                    Dim approval_data As Byte() = _
                    {&H2, &H3, &H16, &H36, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H31, &H31, &H32, &H30, &H30, &H30, &H30, &H1C, &H30, &H32, &H0, &H40, &H41, &H50, &H50, &H52, &H4F, &H56, &H41, &H4C, &H20, &H20, &H20, &H20, &H20, &H20, &H58, &H58, &H58, &H58, &H58, &H58, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H1C, &H44, &H30, &H0, &H60, &H54, &H45, &H53, &H54, &H20, &H54, &H45, &H52, &H4D, &H49, &H4E, &H41, &H4C, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H4C, &H49, &H4E, &H45, &H20, &H31, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H4C, &H49, &H4E, &H45, &H20, &H32, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H1C, &H30, &H33, &H0, &H6, &H31, &H36, &H30, &H31, &H32, &H37, &H1C, &H30, &H34, &H0, &H6, &H31, &H32, &H33, &H36, &H35, &H32, &H1C, &H30, &H31, &H0, &H6, &H58, &H58, &H58, &H58, &H58, &H58, &H1C, &H36, &H35, &H0, &H6, &H39, &H39, &H34, &H30, &H33, &H35, &H1C, &H31, &H36, &H0, &H8, &H31, &H30, &H30, &H31, &H30, &H30, &H30, &H31, &H1C, &H44, &H31, &H0, &H15, &H39, &H39, &H39, &H39, &H39, &H30, &H30, &H30, &H30, &H30, &H31, &H32, &H33, &H34, &H35, &H1C, &H44, &H32, &H0, &H10, &H56, &H49, &H53, &H41, &H20, &H20, &H20, &H20, &H20, &H20, &H1C, &H33, &H30, &H0, &H16, &H2A, &H2A, &H2A, &H2A, &H2A, &H2A, &H2A, &H2A, &H2A, &H2A, &H2A, &H2A, &H39, &H35, &H31, &H32, &H1C, &H33, &H31, &H0, &H4, &H31, &H39, &H30, &H36, &H1C, &H35, &H30, &H0, &H6, &H30, &H30, &H30, &H30, &H30, &H31, &H1C, &H44, &H33, &H0, &H12, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H1C, &H44, &H34, &H0, &H2, &H30, &H34, &H1C, &H44, &H35, &H0, &H26, &H54, &H41, &H4E, &H20, &H57, &H55, &H49, &H20, &H4B, &H49, &H41, &H54, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H0, &H0, &H1C, &H3, &H9}
                    nCOM.SendByte(approval_data, "Sale approval")
                    EMUCOUNTER = EMUCOUNTER + 1
                    EMUTimer.Interval = 2000
                    EMUTimer.Start()
                Else
                    EMUTimer.Interval = RESET_INTERVAL
                    EMUCOUNTER = 0
                    lstMsgs("Test finished.")
                    UpdateStatus("Emulator Ready")
                    DisableForm(True)
                    Exit Sub
                End If


            Case Else
                lstMsgs("undefined")
        End Select

    End Sub

    Private Sub setBehavior(ByVal status As connectionStat)
        Select Case status
            Case connectionStat.connected
                EMUSTART = True
                btnCOM.Text = "Stop"
                btnCOM2.Text = "Stop"
                btnTest.Enabled = True
                btnTest2.Enabled = True
                UpdateStatus("Emulator Started")
                GroupSerial.Enabled = False
                GroupSerial2.Enabled = False
            Case connectionStat.disconnected
                EMUSTART = False
                btnCOM.Text = "Start"
                btnCOM2.Text = "Start"
                btnTest.Enabled = False
                btnTest2.Enabled = False
                UpdateStatus("Emulator Stopped")
                GroupSerial.Enabled = True
                GroupSerial2.Enabled = True
        End Select
    End Sub

    Private Sub nCOM_OnEvent(ByVal nLog As Object, ByVal value As Common.TRX) Handles nCOM.OnEvent
        UpdateStatus("[" & nCOM.PortName & "] " & value.ToString & ": " & nLog)

        Try

            Select Case UCase(nLog)
                Case "ACKNOWLEDGE"
                    If EMUTimer.Enabled Then
                        Select Case EMUMODE
                            Case EmulatorMode.TaxiMeter
                                EMUTimer.Interval = RESET_INTERVAL
                                EMUTimer.Stop()
                                EMUCOUNTER = 0
                                DisableForm(True)
                                lstMsgs("Test finished.")
                                UpdateStatus("Emulator Ready")
                            Case EmulatorMode.CashlessTerminal
                                EMUTimer.Interval = RESET_INTERVAL
                                EMUTimer.Stop()
                                EMUCOUNTER = 0
                                DisableForm(True)
                                lstMsgs("Test finished.")
                                UpdateStatus("Emulator Ready")
                        End Select

                    End If
                Case "SEND REPORT"
                    'If EMUTimer.Enabled Then
                    Select Case EMUMODE
                        Case EmulatorMode.TaxiMeter
                            Dim reply() As Byte = {&H2A, &H54, &H52, &H50, &H34, &H23, &H28, &H16, &H6, &H1, &H16, &H29, &H16, &H2C, &H1, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H1, &H0, &H1, &H0, &H1, &H0, &H0, &H0, &H0, &H0, &H0, &HA, &H23, &H7}
                            nCOM.SendByte(reply, "Send Report Response")
                        Case EmulatorMode.CashlessTerminal

                    End Select
                    'End If

                Case "SALE TRANSACTION"
                    Select Case EMUMODE
                        Case EmulatorMode.TaxiMeter

                        Case EmulatorMode.CashlessTerminal
                            Dim reply() As Byte = {&H6} 'ACK
                            nCOM.SendByte(reply, "Acknowledge")
                    End Select

                Case "SALE APPROVAL"
                    Select Case EMUMODE
                        Case EmulatorMode.TaxiMeter
                            Dim reply() As Byte = {&H6} 'ACK
                            nCOM.SendByte(reply, "Acknowledge")

                        Case EmulatorMode.CashlessTerminal

                    End Select

                Case Else

            End Select

        Catch ex As Exception

        End Try
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim iam As TabControl = DirectCast(sender, TabControl)

        If EMUSTART = True Then
            If EMUMODE = EmulatorMode.TaxiMeter Then
                iam.SelectTab(0)
            Else
                iam.SelectTab(1)
            End If
            Exit Sub
        End If

        Select Case iam.SelectedIndex
            Case 0
                EMUMODE = EmulatorMode.TaxiMeter
            Case 1
                EMUMODE = EmulatorMode.CashlessTerminal
        End Select

        Me.Text = nTitle & " - " & TabControl1.SelectedTab.Text

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
                Case emu_common.Common.ReceiveEvents.STRING
                    Try
                        Split(nData, DATA_DELIMITER)
                        lstMsgs(Split(nData, DATA_DELIMITER)(0) & " : " & Split(nData, DATA_DELIMITER)(1))

                    Catch ex As Exception
                        lstMsgs(nData)
                    End Try
                Case Common.ReceiveEvents.LOG
                    lstMsgs(nData)

            End Select

        Catch ex As Exception
            lstMsgs("DataReceived: " & ex.Message)
        End Try
    End Sub

    Private Sub ListBox1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim list As ListBox = DirectCast(sender, ListBox)
        If list.Items.Count <= 0 Then Exit Sub
        If list.SelectedItem <> "" Then MsgBox(list.SelectedItem)
    End Sub

End Class
