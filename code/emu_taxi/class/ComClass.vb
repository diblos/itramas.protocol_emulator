Public Class ComClass
    Dim COM As System.IO.Ports.SerialPort

    Private _connection As ConnectionStatus

    Private _errortitle As String

    Public arrLastReceived() As Byte
    Private buffer() As Byte

    Private _replyTimer As System.Timers.Timer
    Private _timeoutTimer As System.Timers.Timer
    Private _control As System.Timers.Timer
    Private Const TIMEOUT As Integer = 120 '(seconds)
    Private TIMEOUT_COUNTER As Integer = 0

    Private REPLY_FLAG As Boolean = False
    Private REPLY_TYPE As Common.ReplyType

    Private comn As New Common

    Private _devicemode As Common.Device = Common.Device.TaxiMeter

    Private CONTROL_FLAG As Boolean = False

#Region "ENUM"
    Private Enum ConnectionStatus
        Connected = 1
        Disconnected = 0
    End Enum
#End Region
#Region "PROPERTIES"
    Public ReadOnly Property PortName() As String
        Get
            Return COM.PortName
        End Get
    End Property
    Public ReadOnly Property IsConnected() As Boolean
        Get
            Return COM.IsOpen
        End Get
    End Property
    Public Property DeviceMode() As Common.Device
        Get
            Return Me._devicemode
        End Get
        Set(ByVal value As Common.Device)
            Me._devicemode = value
        End Set
    End Property
#End Region
#Region "PUBLIC METHODS"
    Public Sub New()
        InitiateComPort(1, 115200)
    End Sub

    Public Sub New(ByVal ComPort As Integer, Optional ByVal BaudRate As Integer = 115200)
        InitiateComPort(ComPort, BaudRate)
    End Sub

    Public Function Connect(ByRef comPort As Integer, Optional ByVal BaudRate As Integer = 115200) As String
        _connection = ConnectionStatus.Disconnected
        If ComparePortname(comPort) = False Then
            InitiateComPort(comPort, BaudRate)
        End If
        Try
            RaiseEvent OnResponse("COM" & comPort & ", Baudrate " & BaudRate & " selected.", emu_common.Common.ReceiveEvents.LOG)
            COM.Open()
            _connection = ConnectionStatus.Connected
        Catch ex As Exception
            If COM.IsOpen = False Then
                _connection = ConnectionStatus.Disconnected
            End If
            Throw ex
        End Try
        Return _connection
    End Function

    Public Function Disconnect() As String
        _connection = ConnectionStatus.Disconnected
        Try
            If IsNothing(COM) Then
                _connection = ConnectionStatus.Disconnected
            Else
                If COM.IsOpen = True Then
                    COM.Close()
                    _connection = ConnectionStatus.Disconnected
                ElseIf COM.IsOpen = False Then
                    _connection = ConnectionStatus.Disconnected
                End If
            End If
        Catch ex As Exception
            _connection = ConnectionStatus.Disconnected
            Throw ex
        End Try
        Return _connection
    End Function

    Public Sub SendByte(ByVal message As Byte(), ByVal textString As String)
        Try
            RaiseEvent OnEvent(textString, Common.TRX.SEND)
            buffer = Nothing 'clear buffer for next retrieve
            COM.Write(message, 0, message.Length)
            RaiseEvent OnResponse(IIf(_devicemode = Common.Device.TaxiMeter, ENTITY_TM, ENTITY_CT) & textString & ": " & comn.ByteArrayToString(message), Common.ReceiveEvents.LOG)
        Catch ex As Exception
            Reset()
            RaiseEvent OnResponse(IIf(_devicemode = Common.Device.TaxiMeter, ENTITY_TM, ENTITY_CT) & textString & ": " & ex.Message, Common.ReceiveEvents.ERRORS)
        End Try
    End Sub

    Public Sub SendSaleTransaction()
        Try
            RaiseEvent OnEvent("Sale Transaction", Common.TRX.SEND)
            '02 00 35 36 30 30 30 30 30 30 30 30 30 31 30 32 30 30 30 30 1C 34 30 00 12 30 30 30 30 30 30 30 30 30 33 30 30 1C 03 16
            Dim tmp() As Byte = {&H2, &H0, &H35, &H36, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H31, &H30, &H32, &H30, &H30, &H30, &H30, &H1C, &H34, &H30, &H0, &H12, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H33, &H30, &H30, &H1C, &H3, &H16}
            '_finished_load = False
            buffer = Nothing 'clear buffer for next retrieve
            '_timeoutTimer.Start()
            COM.Write(tmp, 0, tmp.Length)
            RaiseEvent OnResponse(IIf(_devicemode = Common.Device.TaxiMeter, ENTITY_TM, ENTITY_CT) & "SaleTransaction: " & comn.ByteArrayToString(tmp), Common.ReceiveEvents.LOG)
        Catch ex As Exception
            Reset()
            RaiseEvent OnResponse(IIf(_devicemode = Common.Device.TaxiMeter, ENTITY_TM, ENTITY_CT) & "SaleTransaction:" & ex.Message, Common.ReceiveEvents.ERRORS)
        End Try
    End Sub

    Private Sub SendMessage()
        Try
            Dim tmp() As Byte = PRESERVED_ADHOC
            tmp(1) = &H84
            '_finished_load = False
            buffer = Nothing 'clear buffer for next retrieve
            '_timeoutTimer.Start()
            COM.Write(tmp, 0, tmp.Length)
            RaiseEvent OnResponse(ENTITY_RSC & "SendMessage: " & comn.ByteArrayToString(tmp), Common.ReceiveEvents.LOG)
        Catch ex As Exception
            Reset()
            RaiseEvent OnResponse(ENTITY_RSC & "SendMessage:" & ex.Message, Common.ReceiveEvents.ERRORS)
        End Try
    End Sub

    Private Sub SendAcknowledge()
        Try
            '_finished_load = False
            buffer = Nothing 'clear buffer for next retrieve
            '_timeoutTimer.Start()
            COM.Write(COM_ACK_HEX, 0, COM_ACK_HEX.Length)
            RaiseEvent OnResponse(ENTITY_RSC & "SendAcknowledge: " & comn.ByteArrayToString(COM_ACK_HEX), Common.ReceiveEvents.LOG)
            RaiseEvent OnEvent("SendAcknowledge", Common.TRX.SEND)
        Catch ex As Exception
            Reset()
            RaiseEvent OnResponse(ENTITY_RSC & "SendAcknowledge:" & ex.Message, Common.ReceiveEvents.ERRORS)
        End Try
    End Sub

    Private Sub SendNAcknowledge()
        Try
            '_finished_load = False
            buffer = Nothing 'clear buffer for next retrieve
            '_timeoutTimer.Start()
            COM.Write(COM_NACK_HEX, 0, COM_NACK_HEX.Length)
            RaiseEvent OnResponse(ENTITY_RSC & "Send NAcknowledge: " & comn.ByteArrayToString(COM_NACK_HEX), Common.ReceiveEvents.LOG)
            RaiseEvent OnEvent("NAcknowledge", Common.TRX.SEND)
        Catch ex As Exception
            Reset()
            RaiseEvent OnResponse(ENTITY_RSC & "SendAcknowledge:" & ex.Message, Common.ReceiveEvents.ERRORS)
        End Try
    End Sub

    Public Sub CurrentMessage()
        Try

            '_finished_load = False
            buffer = Nothing 'clear buffer for next retrieve
            '_timeoutTimer.Start()
            'COM.Write(COM_ACK_HEX, 0, COM_ACK_HEX.Length)
            'RaiseEvent OnResponse("Send Acknowledge: " & comn.ByteArrayToString(COM_ACK_HEX), Common.ReceiveEvents.log)
            'RaiseEvent OnEvent("Acknowledge", Common.TRX.SEND)
            '===========================================================================================================

            Dim DataBytes(-1) As Byte
            Dim command_str As String = String.Empty

            Dim announce() As Byte = System.Text.Encoding.ASCII.GetBytes(RSC_DATA.Message)
            Dim CLR() As Byte = System.BitConverter.GetBytes(RSC_DATA.ColorID)
            Dim FX() As Byte = System.BitConverter.GetBytes(RSC_DATA.EffectID)
            Dim DURATION() As Byte = System.BitConverter.GetBytes(RSC_DATA.Duration)

            Array.Reverse(DURATION)
            Array.Resize(CLR, 1)
            Array.Resize(FX, 1)

            ReDim Preserve DataBytes(UBound(announce))
            announce.CopyTo(DataBytes, 0)
            ReDim Preserve DataBytes(UBound(DataBytes) + CLR.Length)
            CLR.CopyTo(DataBytes, UBound(DataBytes))

            ReDim Preserve DataBytes(UBound(DataBytes) + FX.Length)
            FX.CopyTo(DataBytes, UBound(DataBytes))

            ReDim Preserve DataBytes(UBound(DataBytes) + DURATION.Length)
            DURATION.CopyTo(DataBytes, UBound(DataBytes) - UBound(DURATION))

            Dim length() As Byte = System.BitConverter.GetBytes(DataBytes.Length)
            Array.Resize(length, 1)

            Dim CS As Byte = comn.GetCheckSum(DataBytes)
            ReDim Preserve DataBytes(UBound(DataBytes) + 1)
            DataBytes(UBound(DataBytes)) = CS

            Dim newData(0) As Byte
            newData(0) = STX
            ReDim Preserve newData(UBound(newData) + 1)
            newData(UBound(newData)) = COMMAND_READ

            ReDim Preserve newData(UBound(newData) + length.Length)
            length.CopyTo(newData, UBound(newData) - UBound(length))

            ReDim Preserve newData(UBound(newData) + DataBytes.Length)
            DataBytes.CopyTo(newData, UBound(newData) - UBound(DataBytes))

            ReDim Preserve newData(UBound(newData) + 1)
            newData(UBound(newData)) = EOT

            COM.Write(newData, 0, newData.Length)
            RaiseEvent OnResponse(ENTITY_RSC & "Read Message  " & comn.ByteArrayToString(newData), Common.ReceiveEvents.ERRORS)
            RaiseEvent OnEvent("Read Message", Common.TRX.SEND)

            '===========================================================================================================
        Catch ex As Exception
            Reset()
            RaiseEvent OnResponse(ENTITY_RSC & "Read Message:" & ex.Message, Common.ReceiveEvents.ERRORS)
        End Try
    End Sub

    Public Event OnResponse(ByVal nLog As Object, ByVal value As Common.ReceiveEvents)
    Public Event OnEvent(ByVal nLog As Object, ByVal value As Common.TRX)

#End Region

#Region "PRIVATE METHODS"
    Private Function reConnect() As String
        _connection = ConnectionStatus.Disconnected
        Try
            If COM.IsOpen Then
                COM.Close()
            End If

            COM.Open()
            _connection = ConnectionStatus.Connected

        Catch ex As Exception
            If COM.IsOpen = False Then
                _connection = ConnectionStatus.Disconnected
            End If
            Throw ex
        End Try
        Return _connection
    End Function

    Private Sub Reset()
        '_finished_load = True
        buffer = Nothing 'clear buffer for next retrieve
        _timeoutTimer.Stop()
        reConnect()
    End Sub

    Private Function ComparePortname(ByVal newPort As Integer) As Boolean
        If COM.PortName = "COM" & CStr(newPort) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub InitiateComPort(ByVal ComPort As Integer, ByVal BaudRate As Integer)
        ' Create a new SerialPort object with default settings.
        If IsNothing(COM) Then
            COM = New System.IO.Ports.SerialPort
        ElseIf COM.IsOpen = True Then
            COM.Close()
        End If

        ' Allow the user to set the appropriate properties.
        COM.PortName = "COM" & CStr(ComPort)
        COM.BaudRate = BaudRate
        COM.Parity = System.IO.Ports.Parity.None
        COM.DataBits = 8
        COM.StopBits = System.IO.Ports.StopBits.One
        COM.Handshake = System.IO.Ports.Handshake.None

        ' Set the read/write timeouts
        'COM.ReadTimeout = 10000
        'COM.WriteTimeout = 10000

        COM.ReadTimeout = 20000
        COM.WriteTimeout = 20000

        COM.ReadBufferSize = 8192

        'Debug.Print(COM.ReadBufferSize)
        'Add handler to event that process messages received from the serial port
        AddHandler COM.DataReceived, AddressOf DataReceivedHandler

        _replyTimer = New System.Timers.Timer(RESPONSE_TIME) '1s interval
        'Set TimeOut Timer
        _timeoutTimer = New System.Timers.Timer(1000) '1s interval
        _control = New System.Timers.Timer(1000)
        AddHandler _replyTimer.Elapsed, AddressOf replyHandler
        AddHandler _timeoutTimer.Elapsed, AddressOf TimeOutHandler
        AddHandler _control.Elapsed, AddressOf ControlHandler
    End Sub

    Private Sub replyHandler(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
        _replyTimer.Stop()
        If REPLY_TYPE = Common.ReplyType.Acknowledge Then
            SendAcknowledge()
        ElseIf REPLY_TYPE = Common.ReplyType.NAcknowledge Then
            SendNAcknowledge()
        Else
            'SendMessage()
            CurrentMessage()
        End If
        RaiseEvent OnEvent("Acknowledge sent.", Common.TRX.SEND)
        Debug.Print("REPLY_COUNTER END")
    End Sub

    Private Sub TimeOutHandler(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
        If TIMEOUT_COUNTER = TIMEOUT Then
            'reset counter
            Debug.Print(COM.BytesToRead)
            TIMEOUT_COUNTER = 0
            Reset()
            RaiseEvent OnResponse(Nothing, Common.ReceiveEvents.TIMEOUT)
            _timeoutTimer.Stop()
        Else
            TIMEOUT_COUNTER += 1
        End If
        Debug.Print("TIMEOUT_COUNTER:" & TIMEOUT_COUNTER)
    End Sub

    Private Sub ControlHandler(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
        CONTROL_FLAG = False
        _control.Stop()
    End Sub

    Private Sub DataReceivedHandler(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs)

        Dim intLenData As Integer
        Dim arrByte() As Byte
        Dim tempBuffer() As Byte
        Dim intI As Integer
        Dim intJ As Integer
        Dim intOffset As Integer
        Dim arrDeValueLen(1) As Byte
        Dim blnETX As Boolean
        Dim blnChkInput As Boolean
        Dim sp As System.IO.Ports.SerialPort = CType(sender, System.IO.Ports.SerialPort)
        Dim intBytesToRead As Integer = sp.BytesToRead
        Dim inData(intBytesToRead - 1) As Byte

        blnETX = False
        blnChkInput = False

        If intBytesToRead > 0 Then
            sp.Read(inData, 0, intBytesToRead)
            If Not IsNothing(buffer) Then
                intOffset = buffer.Length
                ReDim tempBuffer(buffer.Length - 1)
                buffer.CopyTo(tempBuffer, 0)
                ReDim buffer(buffer.Length + inData.Length - 1)
                tempBuffer.CopyTo(buffer, 0)
                inData.CopyTo(buffer, intOffset)
            Else
                ReDim buffer(inData.Length - 1)
                buffer = inData
            End If
        End If

        If Not IsNothing(buffer) Then
            ReDim arrLastReceived(UBound(buffer))
            arrLastReceived = buffer
            blnChkInput = True
            intLenData = buffer.Length
            ReDim arrByte(intLenData - 1)
            arrByte = buffer

            Try
                If arrByte(intLenData - 1) = ETX Then
                    '*************************************
                    'The data received is verified for the complete pacakge
                    '*************************************
                    If intLenData > 0 And intLenData < 17521 Then '17521 for TYPEA PID 39421 for TYPEB and TYPEC refer to PIS Protocol document to derive
                        blnETX = True
                    Else
                        buffer = Nothing
                    End If
                End If
            Catch ex As Exception
                blnETX = False
            End Try
        End If

        If blnChkInput And blnETX Then
            blnETX = False
            ReDim arrLastReceived(UBound(arrByte))
            arrLastReceived = arrByte
            buffer = Nothing 'Clear the inData after reassign array

           If UBound(arrByte) > 3 Then

                refine(arrByte, intLenData)

                'Debug.Print(arrByte.Length)
                'Debug.Print(comn.ByteArrayToString(arrByte))

                'Package verification
                If (arrByte(0) = STX) And (arrByte(intLenData - 1) = ETX) Then

                    If arrByte.Length = 1 And arrByte(0) = &H6 Then
                        RaiseEvent OnResponse(IIf(_devicemode = emu_common.Common.Device.TaxiMeter, ENTITY_CT, ENTITY_TM) & "Acknowledge" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                        RaiseEvent OnEvent("Acknowledge", Common.TRX.RECEIVE)
                        Array.Resize(buffer, 0)
                    Else
                        Select Case _devicemode
                            Case emu_common.Common.Device.TaxiMeter

                                '2A 5E 7E
                                Dim Write_Datetime_to_TaxiMeter As Byte() = {&H2A, &H5E, &H7E}

                                '2A646E0105FE00
                                Dim Send_Report As Byte() = {&H2A, &H64, &H6E, &H1, &H5, &HFE, &H0} 'HARDCODED

                                '2A646E05050102
                                Dim Get_Meter_Info As Byte() = {&H2A, &H64, &H6E, &H5, &H5, &H1, &H2} 'HARDCODED

                                '2A646E0305FC00
                                Dim Get_Accumulated_Statistics As Byte() = {&H2A, &H64, &H6E, &H3, &H5, &HFC, &H0}

                                '2A646E06050101
                                Dim Get_Daily_Accumulated_Statistics As Byte() = {&H2A, &H64, &H6E, &H6, &H5, &H1, &H1}

                                If IsDatetimeWrite(arrByte, Write_Datetime_to_TaxiMeter) Then
                                    RaiseEvent OnResponse(ENTITY_OBU & "Write Datetime to TaxiMeter" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                                    RaiseEvent OnEvent("WRITE DATETIME TO TAXIMETER", Common.TRX.RECEIVE)
                                    Array.Resize(buffer, 0)
                                ElseIf AreArraysEqual(arrByte, Get_Meter_Info) Then
                                    RaiseEvent OnResponse(ENTITY_OBU & "Get Meter Info" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                                    RaiseEvent OnEvent("GET METER INFO", Common.TRX.RECEIVE)
                                    Array.Resize(buffer, 0)
                                ElseIf AreArraysEqual(arrByte, Send_Report) Then
                                    RaiseEvent OnResponse(ENTITY_OBU & "Send Report" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                                    RaiseEvent OnEvent("SEND REPORT", Common.TRX.RECEIVE)
                                    Array.Resize(buffer, 0)
                                ElseIf AreArraysEqual(arrByte, Get_Accumulated_Statistics) Then
                                    RaiseEvent OnResponse(ENTITY_OBU & "Get Accumulated Statistics" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                                    RaiseEvent OnEvent("GET ACCUMULATED STATISTICS", Common.TRX.RECEIVE)
                                    Array.Resize(buffer, 0)
                                ElseIf AreArraysEqual(arrByte, Get_Daily_Accumulated_Statistics) Then
                                    RaiseEvent OnResponse(ENTITY_OBU & "Get Daily Accumulated Statistics" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                                    RaiseEvent OnEvent("GET DAILY ACCUMULATED STATISTICS", Common.TRX.RECEIVE)
                                    Array.Resize(buffer, 0)
                                Else
                                    Try
                                        '02 03 16 36 30
                                        If SaleFilter(arrByte) Then 'HOWTO FILTER OUT SALE TRANSACTION IN TAXIMETER MODE

                                            If Not IsApproval(arrByte) Then Throw New Exception("Not Sale Approval")

                                            RaiseEvent OnResponse(ENTITY_CT & "Sale Approval" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                                            RaiseEvent OnEvent("SALE APPROVAL", Common.TRX.RECEIVE)

                                            Breakdowns(arrByte)
                                            Array.Resize(buffer, 0)
                                            Array.Resize(arrByte, 0)
                                        ElseIf arrByte(0) <> STX Then
                                            Throw New Exception("Unknown Message")
                                        End If
                                    Catch ex As Exception
                                        If Not CONTROL_FLAG Then
                                            RaiseEvent OnResponse(ENTITY_CT & "Unknown" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                                            RaiseEvent OnEvent("UNKNOWN", Common.TRX.RECEIVE)
                                            Array.Resize(buffer, 0)
                                        End If
                                    End Try
                                End If

                            Case emu_common.Common.Device.CashlessTerminal
                                'Debug.Print("RATES")

                                'RaiseEvent OnResponse(ENTITY_CT & "Unknown" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                                'RaiseEvent OnEvent("UNKNOWN", Common.TRX.RECEIVE)
                                'Array.Resize(buffer, 0)

                                Try
                                    '02003536303030303030303030313032303030301C343000123030303030303030303330301C0316
                                    Dim Sale_Transaction As Byte() = {&H2, &H0, &H35, &H36, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H31, &H30, &H32, &H30, &H30, &H30, &H30, &H1C, &H34, &H30, &H0, &H12, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H33, &H30, &H30, &H1C, &H3, &H16}
                                    'If AreArraysEqual(arrByte, Sale_Transaction) Then
                                    If (arrByte(0) = STX) And (arrByte(intLenData - 1) = ETX) Then
                                        'If arrByte(0) = STX And arrByte(UBound(arrByte) - 1) = ETX Then

                                        If IsApproval(arrByte) Then Throw New Exception("Not Sale Transaction")

                                        RaiseEvent OnResponse(ENTITY_TM & "Sale Transaction" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                                        RaiseEvent OnEvent("Sale Transaction", Common.TRX.RECEIVE)

                                        Breakdowns(arrByte)
                                        'Array.Resize(buffer, 0)
                                    ElseIf arrByte(0) <> STX Then
                                        Throw New Exception("Unknown Message")
                                    End If
                                Catch ex As Exception
                                    RaiseEvent OnResponse(ENTITY_TM & "Unknown" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                                    RaiseEvent OnEvent("UNKNOWN", Common.TRX.RECEIVE)
                                    'If Not CONTROL_FLAG Then Array.Resize(buffer, 0)
                                End Try

                        End Select
                    End If


                End If
            Else
                intI = UBound(arrByte)
                intJ = intI
            End If
        Else

            If arrByte.Length = 0 Then Exit Sub
            If arrByte.Length = 1 And arrByte(UBound(arrByte)) = &H6 Then
                RaiseEvent OnResponse(IIf(_devicemode = emu_common.Common.Device.TaxiMeter, ENTITY_CT, ENTITY_TM) & "Acknowledge" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                RaiseEvent OnEvent("Acknowledge", Common.TRX.RECEIVE)
                Array.Resize(buffer, 0)
            Else
                Select Case _devicemode
                    Case emu_common.Common.Device.TaxiMeter
                        '2A 5E 7E
                        Dim write_datetime_to_taximeter As Byte() = {&H2A, &H5E, &H7E}

                        '2A646E0105FE00
                        Dim send_report As Byte() = {&H2A, &H64, &H6E, &H1, &H5, &HFE, &H0} 'HARDCODED

                        '2A646E05050102
                        Dim get_meter_info As Byte() = {&H2A, &H64, &H6E, &H5, &H5, &H1, &H2} 'HARDCODED

                        '2A646E0305FC00
                        Dim get_accumulated_statistics As Byte() = {&H2A, &H64, &H6E, &H3, &H5, &HFC, &H0}

                        '2A646E06050101
                        Dim get_daily_accumulated_statistics As Byte() = {&H2A, &H64, &H6E, &H6, &H5, &H1, &H1}

                        If AreArraysEqual(arrByte, get_meter_info) Then
                            RaiseEvent OnResponse(ENTITY_OBU & "get meter info" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                            RaiseEvent OnEvent("get meter info", Common.TRX.RECEIVE)
                            Array.Resize(buffer, 0)
                        ElseIf AreArraysEqual(arrByte, send_report) Then
                            RaiseEvent OnResponse(ENTITY_OBU & "send report" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                            RaiseEvent OnEvent("send report", Common.TRX.RECEIVE)
                            Array.Resize(buffer, 0)
                        ElseIf AreArraysEqual(arrByte, get_accumulated_statistics) Then
                            RaiseEvent OnResponse(ENTITY_OBU & "get accumulated statistics" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                            RaiseEvent OnEvent("get accumulated statistics", Common.TRX.RECEIVE)
                            Array.Resize(buffer, 0)
                        ElseIf AreArraysEqual(arrByte, get_daily_accumulated_statistics) Then
                            RaiseEvent OnResponse(ENTITY_OBU & "get daily accumulated statistics" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                            RaiseEvent OnEvent("get daily accumulated statistics", Common.TRX.RECEIVE)
                            Array.Resize(buffer, 0)
                        Else
                            Try
                                If IsDatetimeWrite(arrByte, write_datetime_to_taximeter) Then
                                    RaiseEvent OnResponse(ENTITY_OBU & "write datetime to taximeter" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                                    RaiseEvent OnEvent("write datetime to taximeter", Common.TRX.RECEIVE)
                                    Array.Resize(buffer, 0)
                                ElseIf arrByte.Length > 15 Then
                                    'If arrByte.Length > 400 Then Throw New Exception("unknown message")
                                End If

                            Catch ex As Exception
                                RaiseEvent OnResponse(ENTITY_TM & "unknown" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                                RaiseEvent OnEvent("unknown", Common.TRX.RECEIVE)
                                If Not CONTROL_FLAG Then Array.Resize(buffer, 0)
                            End Try
                        End If
                    Case emu_common.Common.Device.CashlessTerminal

                End Select
            End If
        End If

        If Not IsNothing(arrByte) Then Debug.Print(intBytesToRead & " | " & comn.ByteArrayToString(arrByte))

    End Sub

    Private Sub refine(ByRef refBytes As Byte(), ByRef len As Integer)
        Dim index As Integer = 0
        If Not IsNothing(refBytes) Then
            For i = 0 To UBound(refBytes)
                If refBytes(i) = STX Then
                    index = i
                    Exit For
                End If
            Next

            Array.Reverse(refBytes)
            Array.Resize(refBytes, UBound(refBytes) - (index) + 1)
            Array.Reverse(refBytes)

            len = refBytes.Length

        End If
    End Sub

    Private Sub DataReceivedHandler_OLD(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs)

        Dim arrByte() As Byte
        Dim tempBuffer() As Byte
        Dim intOffset As Integer
        Dim arrDeValueLen(1) As Byte
        Dim sp As System.IO.Ports.SerialPort = CType(sender, System.IO.Ports.SerialPort)
        Dim intBytesToRead As Integer = sp.BytesToRead
        Dim inData(intBytesToRead - 1) As Byte

        Dim msglength As Integer
        Dim blnEOT As Boolean
        'Reset Timeout Counter
        _timeoutTimer.Stop()
        'Dim dsNew As DataSet = Nothing
        'Dim drNewRow As DataRow = Nothing

        'Debug.Print(sp.BytesToRead)

        If intBytesToRead > 0 Then
            If Not CONTROL_FLAG Then
                CONTROL_FLAG = True
                _control.Start()
            End If
            sp.Read(inData, 0, intBytesToRead)
            If Not IsNothing(buffer) Then
                intOffset = buffer.Length
                ReDim tempBuffer(buffer.Length - 1)
                buffer.CopyTo(tempBuffer, 0)
                ReDim buffer(buffer.Length + inData.Length - 1)
                tempBuffer.CopyTo(buffer, 0)
                inData.CopyTo(buffer, intOffset)
            Else
                ReDim buffer(inData.Length - 1)
                buffer = inData
            End If

            Exit Sub '<<<<<<<<<<<<
            'Else '<<<<<<<<<<<<
            'CONTROL_FLAG = False
            'Array.Resize(buffer, 0)
        End If



        If IsNothing(buffer) Then Exit Sub
        '====================================================================================================================================================================================================================
        'TEST CODES
        '====================================================================================================================================================================================================================
        If buffer.Length <= 0 Then Exit Sub

        arrByte = buffer

        'Debug.Print(comn.ByteArrayToString(arrByte))

        If arrByte.Length = 1 And arrByte(0) = &H6 Then
            RaiseEvent OnResponse(IIf(_devicemode = emu_common.Common.Device.TaxiMeter, ENTITY_CT, ENTITY_TM) & "Acknowledge" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
            RaiseEvent OnEvent("Acknowledge", Common.TRX.RECEIVE)
            Array.Resize(buffer, 0)
        Else
            Select Case _devicemode
                Case emu_common.Common.Device.TaxiMeter

                    'Try
                    '    Dim a(1) As Byte
                    '    a(0) = arrByte(1)
                    '    a(1) = arrByte(2)
                    '    'msglength = comn.ByteArrayToString(a)
                    '    Debug.Print("LENGTH:" & comn.ByteArrayToString(a))
                    'Catch ex As Exception

                    'End Try

                    '2A 5E 7E
                    Dim Write_Datetime_to_TaxiMeter As Byte() = {&H2A, &H5E, &H7E}

                    '2A646E0105FE00
                    Dim Send_Report As Byte() = {&H2A, &H64, &H6E, &H1, &H5, &HFE, &H0} 'HARDCODED

                    '2A646E05050102
                    Dim Get_Meter_Info As Byte() = {&H2A, &H64, &H6E, &H5, &H5, &H1, &H2} 'HARDCODED

                    '2A646E0305FC00
                    Dim Get_Accumulated_Statistics As Byte() = {&H2A, &H64, &H6E, &H3, &H5, &HFC, &H0}

                    '2A646E06050101
                    Dim Get_Daily_Accumulated_Statistics As Byte() = {&H2A, &H64, &H6E, &H6, &H5, &H1, &H1}

                    If IsDatetimeWrite(arrByte, Write_Datetime_to_TaxiMeter) Then
                        RaiseEvent OnResponse(ENTITY_OBU & "Write Datetime to TaxiMeter" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                        RaiseEvent OnEvent("WRITE DATETIME TO TAXIMETER", Common.TRX.RECEIVE)
                        Array.Resize(buffer, 0)
                    ElseIf AreArraysEqual(arrByte, Get_Meter_Info) Then
                        RaiseEvent OnResponse(ENTITY_OBU & "Get Meter Info" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                        RaiseEvent OnEvent("GET METER INFO", Common.TRX.RECEIVE)
                        Array.Resize(buffer, 0)
                    ElseIf AreArraysEqual(arrByte, Send_Report) Then
                        RaiseEvent OnResponse(ENTITY_OBU & "Send Report" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                        RaiseEvent OnEvent("SEND REPORT", Common.TRX.RECEIVE)
                        Array.Resize(buffer, 0)
                    ElseIf AreArraysEqual(arrByte, Get_Accumulated_Statistics) Then
                        RaiseEvent OnResponse(ENTITY_OBU & "Get Accumulated Statistics" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                        RaiseEvent OnEvent("GET ACCUMULATED STATISTICS", Common.TRX.RECEIVE)
                        Array.Resize(buffer, 0)
                    ElseIf AreArraysEqual(arrByte, Get_Daily_Accumulated_Statistics) Then
                        RaiseEvent OnResponse(ENTITY_OBU & "Get Daily Accumulated Statistics" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                        RaiseEvent OnEvent("GET DAILY ACCUMULATED STATISTICS", Common.TRX.RECEIVE)
                        Array.Resize(buffer, 0)
                    Else
                        Try
                            '02 03 16 36 30
                            If SaleFilter(arrByte) Then 'HOWTO FILTER OUT SALE TRANSACTION IN TAXIMETER MODE

                                'Debug.Print(arrByte.Length)

                                If Not IsApproval(arrByte) Then Throw New Exception("Not Sale Approval")

                                RaiseEvent OnResponse(ENTITY_CT & "Sale Approval" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                                RaiseEvent OnEvent("SALE APPROVAL", Common.TRX.RECEIVE)

                                'Debug.Print(comn.ByteArrayToString(arrByte))
                                Breakdowns(arrByte)
                                Array.Resize(buffer, 0)
                            ElseIf arrByte(0) <> STX Then
                                Throw New Exception("Unknown Message")
                            End If
                        Catch ex As Exception
                            If Not CONTROL_FLAG Then
                                RaiseEvent OnResponse(ENTITY_CT & "Unknown" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                                RaiseEvent OnEvent("UNKNOWN", Common.TRX.RECEIVE)
                                Array.Resize(buffer, 0)
                            End If
                        End Try
                    End If

                Case emu_common.Common.Device.CashlessTerminal
                    'Debug.Print("RATES")

                    'RaiseEvent OnResponse(ENTITY_CT & "Unknown" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                    'RaiseEvent OnEvent("UNKNOWN", Common.TRX.RECEIVE)
                    'Array.Resize(buffer, 0)

                    Try
                        '02003536303030303030303030313032303030301C343000123030303030303030303330301C0316
                        Dim Sale_Transaction As Byte() = {&H2, &H0, &H35, &H36, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H31, &H30, &H32, &H30, &H30, &H30, &H30, &H1C, &H34, &H30, &H0, &H12, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H33, &H30, &H30, &H1C, &H3, &H16}
                        'If AreArraysEqual(arrByte, Sale_Transaction) Then
                        If arrByte(0) = STX And arrByte(UBound(arrByte) - 1) = ETX Then

                            If IsApproval(arrByte) Then Throw New Exception("Not Sale Transaction")

                            RaiseEvent OnResponse(ENTITY_TM & "Sale Transaction" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                            RaiseEvent OnEvent("Sale Transaction", Common.TRX.RECEIVE)

                            Breakdowns(arrByte)
                            Array.Resize(buffer, 0)
                        ElseIf arrByte(0) <> STX Then
                            Throw New Exception("Unknown Message")
                        End If
                    Catch ex As Exception
                        RaiseEvent OnResponse(ENTITY_TM & "Unknown" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
                        RaiseEvent OnEvent("UNKNOWN", Common.TRX.RECEIVE)
                        'If Not CONTROL_FLAG Then Array.Resize(buffer, 0)
                    End Try

            End Select
        End If

        'RaiseEvent OnResponse("INFO " & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.LOG)
        '====================================================================================================================================================================================================================
        'OLD CODES
        '====================================================================================================================================================================================================================
        'If blnEOT = False Then
        '    If buffer.Length <= 0 Then Exit Sub
        '    arrByte = buffer
        '    'If (arrByte(0) = STX) AndAlso (arrByte(UBound(arrByte) - 1) = EOT) Then
        '    If Checking(arrByte) = Common.Result.FAIL Then
        '        'RaiseEvent OnEvent("Acknowledge", Common.TRX.RECEIVE)
        '        RaiseEvent OnResponse(ENTITY_OBU & "Invalid Checksum" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.ERRORS)
        '        REPLY_TYPE = Common.ReplyType.NAcknowledge
        '        _replyTimer.Start()
        '    Else

        '        Select Case _devicemode
        '            Case Common.Device.TaxiMeter
        '                If arrByte.Length = 1 And arrByte(0) = &H6 Then
        '                    RaiseEvent OnResponse(ENTITY_CT & "Acknowledge" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
        '                    RaiseEvent OnEvent("Acknowledge", Common.TRX.RECEIVE)
        '                    Array.Resize(buffer, 0)
        '                Else
        '                    '========================================================================================
        '                    '2A 5E 7E
        '                    Dim Write_Datetime_to_TaxiMeter As Byte() = {&H2A, &H5E, &H7E}

        '                    '2A646E0105FE00
        '                    Dim Send_Report As Byte() = {&H2A, &H64, &H6E, &H1, &H5, &HFE, &H0} 'HARDCODED

        '                    '2A646E05050102
        '                    Dim Get_Meter_Info As Byte() = {&H2A, &H64, &H6E, &H5, &H5, &H1, &H2} 'HARDCODED

        '                    '2A646E0305FC00
        '                    Dim Get_Accumulated_Statistics As Byte() = {&H2A, &H64, &H6E, &H3, &H5, &HFC, &H0}

        '                    '2A646E06050101
        '                    Dim Get_Daily_Accumulated_Statistics As Byte() = {&H2A, &H64, &H6E, &H6, &H5, &H1, &H1}
        '                    '========================================================================================
        '                    If IsDatetimeWrite(arrByte, Write_Datetime_to_TaxiMeter) Then
        '                        RaiseEvent OnResponse(ENTITY_OBU & "Write Datetime to TaxiMeter" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
        '                        RaiseEvent OnEvent("WRITE DATETIME TO TAXIMETER", Common.TRX.RECEIVE)
        '                        Array.Resize(buffer, 0)
        '                    ElseIf AreArraysEqual(arrByte, Get_Meter_Info) Then
        '                        RaiseEvent OnResponse(ENTITY_OBU & "Get Meter Info" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
        '                        RaiseEvent OnEvent("GET METER INFO", Common.TRX.RECEIVE)
        '                        Array.Resize(buffer, 0)
        '                    ElseIf AreArraysEqual(arrByte, Send_Report) Then
        '                        RaiseEvent OnResponse(ENTITY_OBU & "Send Report" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
        '                        RaiseEvent OnEvent("SEND REPORT", Common.TRX.RECEIVE)
        '                        Array.Resize(buffer, 0)
        '                    ElseIf AreArraysEqual(arrByte, Get_Accumulated_Statistics) Then
        '                        RaiseEvent OnResponse(ENTITY_OBU & "Get Accumulated Statistics" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
        '                        RaiseEvent OnEvent("GET ACCUMULATED STATISTICS", Common.TRX.RECEIVE)
        '                        Array.Resize(buffer, 0)
        '                    ElseIf AreArraysEqual(arrByte, Get_Daily_Accumulated_Statistics) Then
        '                        RaiseEvent OnResponse(ENTITY_OBU & "Get Daily Accumulated Statistics" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
        '                        RaiseEvent OnEvent("GET DAILY ACCUMULATED STATISTICS", Common.TRX.RECEIVE)
        '                        Array.Resize(buffer, 0)
        '                    Else
        '                        Try
        '                            '02 03 16 36 30
        '                            If arrByte(0) = STX And (arrByte(3) = &H36 And arrByte(4) = &H30) Then 'HOWTO FILTER OUT SALE TRANSACTION IN TAXIMETER MODE

        '                                If Not IsApproval(arrByte) Then Throw New Exception("Not Sale Approval")

        '                                RaiseEvent OnResponse(ENTITY_CT & "Sale Approval" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
        '                                RaiseEvent OnEvent("SALE APPROVAL", Common.TRX.RECEIVE)

        '                                Breakdowns(arrByte)
        '                                Array.Resize(buffer, 0)
        '                            ElseIf arrByte(0) <> STX Then
        '                                Throw New Exception("Unknown Message")
        '                                Array.Resize(buffer, 0)
        '                            End If
        '                        Catch ex As Exception
        '                            RaiseEvent OnResponse(ENTITY_CT & "Unknown" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
        '                            RaiseEvent OnEvent("UNKNOWN", Common.TRX.RECEIVE)
        '                            Array.Resize(buffer, 0)
        '                        End Try
        '                    End If
        '                End If

        '            Case Common.Device.CashlessTerminal
        '                If arrByte.Length = 1 And arrByte(0) = &H6 Then
        '                    RaiseEvent OnResponse(ENTITY_TM & "Acknowledge" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
        '                    RaiseEvent OnEvent("Acknowledge", Common.TRX.RECEIVE)
        '                    Array.Resize(buffer, 0)
        '                Else
        '                    Try
        '                        '02003536303030303030303030313032303030301C343000123030303030303030303330301C0316
        '                        Dim Sale_Transaction As Byte() = {&H2, &H0, &H35, &H36, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H31, &H30, &H32, &H30, &H30, &H30, &H30, &H1C, &H34, &H30, &H0, &H12, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H33, &H30, &H30, &H1C, &H3, &H16}
        '                        'If AreArraysEqual(arrByte, Sale_Transaction) Then
        '                        If arrByte(0) = STX And arrByte(UBound(arrByte) - 1) = ETX Then

        '                            If IsApproval(arrByte) Then Throw New Exception("Not Sale Transaction")

        '                            RaiseEvent OnResponse(ENTITY_TM & "Sale Transaction" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
        '                            RaiseEvent OnEvent("Sale Transaction", Common.TRX.RECEIVE)

        '                            Breakdowns(arrByte)
        '                            Array.Resize(buffer, 0)
        '                        ElseIf arrByte(0) <> STX Then
        '                            Throw New Exception("Unknown Message")
        '                            Array.Resize(buffer, 0)
        '                        End If
        '                    Catch ex As Exception
        '                        RaiseEvent OnResponse(ENTITY_TM & "Unknown" & DATA_DELIMITER & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.STRING)
        '                        RaiseEvent OnEvent("UNKNOWN", Common.TRX.RECEIVE)
        '                        Array.Resize(buffer, 0)
        '                    End Try
        '                End If

        '        End Select

        '    End If
        '    '_finished_load = True
        '    'End If
        'End If
        '====================================================================================================================================================================================================================

        '_timeoutTimer.Start()

    End Sub

    Private Sub ProcessAdhoc(ByVal arrbyte() As Byte)

        Dim len As Integer = Convert.ToInt32(arrbyte(2))

        Dim data(len - 1) As Byte
        Dim dur(3) As Byte

        Array.ConstrainedCopy(arrbyte, 3, data, 0, len)
        Array.ConstrainedCopy(data, UBound(data) - 3, dur, 0, 4)

        Dim str As String = String.Empty

        For i = 0 To UBound(data)
            If i <= UBound(data) - 6 Then
                str &= Convert.ToChar(data(i))
            Else
                Select Case i
                    Case UBound(data) - 5
                        RSC_DATA.ColorID = Convert.ToInt32(data(i))
                    Case UBound(data) - 4
                        RSC_DATA.EffectID = Convert.ToInt32(data(i))
                End Select
            End If
        Next

        Array.Reverse(dur)
        RSC_DATA.Message = str
        RSC_DATA.Duration = System.BitConverter.ToInt32(dur, 0)

    End Sub

#End Region
#Region "COMMON"

    'SALE DATA FILTER - 1ST LAYER
    Private Function SaleFilter(ByVal data As Byte()) As Boolean
        Try
            If data(0) = STX And (data(3) = &H36 And data(4) = &H30) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    'SALE APPROVAL OR SALE TRANSACTION DATA
    Private Function IsApproval(ByVal data As Byte()) As Boolean
        Dim encounter As Integer = 0
        Dim counter As Integer = 0

        For Each item As Byte In data
            If item = SEP Then
                encounter = encounter + 1
            End If
        Next
        Return IIf(encounter > 2, True, False)
    End Function

    'WRITE DATETIME TO TAXIMETER
    Private Function IsDatetimeWrite(ByVal data As Byte(), ByVal signature As Byte()) As Boolean
        Dim CompareBytes(UBound(signature)) As Byte
        Try
            Array.Copy(data, CompareBytes, 3)
            If data.Length = 15 Then
                Return AreArraysEqual(CompareBytes, signature)
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub TimeBreakdowns(ByVal arrByte() As Byte)
        If _devicemode = emu_common.Common.Device.TaxiMeter Then
            '2A5E7E 300E01002522060116 20 F101
            '*^~||0x20|CS

        End If
    End Sub

    Private Sub Breakdowns(ByVal arrByte() As Byte)
        Select Case _devicemode
            Case emu_common.Common.Device.TaxiMeter
                'TAXIMETER  TAXIMETER   TAXIMETER

                'TEST
                Dim tmpStr_1 As String
                Dim tmpStr_2 As String
                Dim tmpStr_3 As String
                Dim tmpStr_4 As String
                Dim tmpStr_5 As String
                Dim tmpStr_6 As String
                Dim tmpStr_7 As String
                Dim tmpStr_8 As String
                Dim tmpStr_9 As String

                Dim tmpStr_10 As String
                Dim tmpStr_11 As String
                Dim tmpStr_12 As String
                Dim tmpStr_13 As String
                Dim tmpStr_14 As String
                Dim tmpStr_15 As String
                Dim tmpStr_16 As String

                RaiseEvent OnResponse("-- BREAKDOWN START --", Common.ReceiveEvents.LOG)
                Dim k(0) As Byte
                k(0) = arrByte(13)
                tmpStr_1 = "Format version - " & comn.ByteArrayToString(k)
                RaiseEvent OnResponse(tmpStr_1, Common.ReceiveEvents.LOG)

                k(0) = arrByte(14)
                tmpStr_1 = "Request Response Indicator - " & comn.ByteArrayToString(k)
                RaiseEvent OnResponse(tmpStr_1, Common.ReceiveEvents.LOG)

                ReDim k(1)
                k(0) = arrByte(15)
                k(1) = arrByte(16)
                tmpStr_1 = "Transaction code - " & comn.ByteArrayToString(k)
                RaiseEvent OnResponse(tmpStr_1, Common.ReceiveEvents.LOG)

                k(0) = arrByte(17)
                k(1) = arrByte(18)
                tmpStr_1 = "Response code - " & comn.ByteArrayToString(k)
                RaiseEvent OnResponse(tmpStr_1, Common.ReceiveEvents.LOG)

                ReDim k(0)
                k(0) = arrByte(19)
                tmpStr_1 = "More indicator - " & comn.ByteArrayToString(k)
                RaiseEvent OnResponse(tmpStr_1, Common.ReceiveEvents.LOG)

                Try
                    Dim encounter As Integer = 0
                    Dim counter As Integer = 0
                    tmpStr_1 = String.Empty
                    tmpStr_2 = String.Empty
                    tmpStr_3 = String.Empty
                    tmpStr_4 = String.Empty
                    tmpStr_5 = String.Empty
                    tmpStr_6 = String.Empty
                    tmpStr_7 = String.Empty
                    tmpStr_8 = String.Empty
                    tmpStr_9 = String.Empty

                    tmpStr_10 = String.Empty
                    tmpStr_11 = String.Empty
                    tmpStr_12 = String.Empty
                    tmpStr_13 = String.Empty
                    tmpStr_14 = String.Empty
                    tmpStr_15 = String.Empty
                    tmpStr_16 = String.Empty

                    For Each item As Byte In arrByte
                        Select Case encounter
                            Case 1
                                tmpStr_1 = tmpStr_1 & ConvertDecToHex(item)
                            Case 2
                                tmpStr_2 = tmpStr_2 & ConvertDecToHex(item)
                            Case 3
                                tmpStr_3 = tmpStr_3 & ConvertDecToHex(item)
                            Case 4
                                tmpStr_4 = tmpStr_4 & ConvertDecToHex(item)
                            Case 5
                                tmpStr_5 = tmpStr_5 & ConvertDecToHex(item)
                            Case 6
                                tmpStr_6 = tmpStr_6 & ConvertDecToHex(item)
                            Case 7
                                tmpStr_7 = tmpStr_7 & ConvertDecToHex(item)
                            Case 8
                                tmpStr_8 = tmpStr_8 & ConvertDecToHex(item)
                            Case 9
                                tmpStr_9 = tmpStr_9 & ConvertDecToHex(item)
                            Case 10
                                tmpStr_10 = tmpStr_10 & ConvertDecToHex(item)
                            Case 11
                                tmpStr_11 = tmpStr_11 & ConvertDecToHex(item)
                            Case 12
                                tmpStr_12 = tmpStr_12 & ConvertDecToHex(item)
                            Case 13
                                tmpStr_13 = tmpStr_13 & ConvertDecToHex(item)
                            Case 14
                                tmpStr_14 = tmpStr_14 & ConvertDecToHex(item)
                            Case 15
                                tmpStr_15 = tmpStr_15 & ConvertDecToHex(item)
                            Case 16
                                tmpStr_16 = tmpStr_16 & ConvertDecToHex(item)

                        End Select
                        If item = SEP Then
                            encounter = encounter + 1
                        End If
                    Next


                    If encounter >= 1 Then
                        tmpStr_1 = tmpStr_1.Remove(tmpStr_1.Length - 2, 2)
                        RaiseEvent OnResponse("Field Element 1", emu_common.Common.ReceiveEvents.LOG)
                        'RaiseEvent OnResponse(tmpStr_1, emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Type - " & tmpStr_1.Substring(0, 4), Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Length - " & tmpStr_1.Substring(4, 4), emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Data - " & tmpStr_1.Substring(8, tmpStr_1.Length - 8) & _
                                              " [" & hex2ascii(tmpStr_1.Substring(8, tmpStr_1.Length - 8)) & "]", emu_common.Common.ReceiveEvents.LOG)
                    End If

                    If encounter >= 2 Then
                        tmpStr_2 = tmpStr_2.Remove(tmpStr_2.Length - 2, 2)
                        RaiseEvent OnResponse("Field Element 2", emu_common.Common.ReceiveEvents.LOG)
                        'RaiseEvent OnResponse(tmpStr_2, emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Type - " & tmpStr_2.Substring(0, 4), Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Length - " & tmpStr_2.Substring(4, 4), emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Data - " & tmpStr_2.Substring(8, tmpStr_2.Length - 8) & _
                                              " [" & hex2ascii(tmpStr_2.Substring(8, tmpStr_2.Length - 8)) & "]", emu_common.Common.ReceiveEvents.LOG)
                    End If

                    If encounter >= 3 Then
                        tmpStr_3 = tmpStr_3.Remove(tmpStr_3.Length - 2, 2)
                        RaiseEvent OnResponse("Field Element 3", emu_common.Common.ReceiveEvents.LOG)
                        'RaiseEvent OnResponse(tmpStr_3, emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Type - " & tmpStr_3.Substring(0, 4), Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Length - " & tmpStr_3.Substring(4, 4), emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Data - " & tmpStr_3.Substring(8, tmpStr_3.Length - 8) & _
                                              " [" & hex2ascii(tmpStr_3.Substring(8, tmpStr_3.Length - 8)) & "]", emu_common.Common.ReceiveEvents.LOG)
                    End If

                    '==============================================================================
                    If encounter >= 4 Then
                        tmpStr_4 = tmpStr_4.Remove(tmpStr_4.Length - 2, 2)
                        RaiseEvent OnResponse("Field Element 4", emu_common.Common.ReceiveEvents.LOG)
                        'RaiseEvent OnResponse(tmpStr_4, emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Type - " & tmpStr_4.Substring(0, 4), Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Length - " & tmpStr_4.Substring(4, 4), emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Data - " & tmpStr_4.Substring(8, tmpStr_4.Length - 8) & _
                                              " [" & hex2ascii(tmpStr_4.Substring(8, tmpStr_4.Length - 8)) & "]", emu_common.Common.ReceiveEvents.LOG)
                    End If

                    If encounter >= 5 Then
                        tmpStr_5 = tmpStr_5.Remove(tmpStr_5.Length - 2, 2)
                        RaiseEvent OnResponse("Field Element 5", emu_common.Common.ReceiveEvents.LOG)
                        'RaiseEvent OnResponse(tmpStr_5, emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Type - " & tmpStr_5.Substring(0, 4), Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Length - " & tmpStr_5.Substring(4, 4), emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Data - " & tmpStr_5.Substring(8, tmpStr_5.Length - 8) & _
                                              " [" & hex2ascii(tmpStr_5.Substring(8, tmpStr_5.Length - 8)) & "]", emu_common.Common.ReceiveEvents.LOG)
                    End If

                    If encounter >= 6 Then
                        tmpStr_6 = tmpStr_6.Remove(tmpStr_6.Length - 2, 2)
                        RaiseEvent OnResponse("Field Element 6", emu_common.Common.ReceiveEvents.LOG)
                        'RaiseEvent OnResponse(tmpStr_6, emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Type - " & tmpStr_6.Substring(0, 4), Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Length - " & tmpStr_6.Substring(4, 4), emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Data - " & tmpStr_6.Substring(8, tmpStr_6.Length - 8) & _
                                              " [" & hex2ascii(tmpStr_6.Substring(8, tmpStr_6.Length - 8)) & "]", emu_common.Common.ReceiveEvents.LOG)
                    End If

                    If encounter >= 7 Then
                        tmpStr_7 = tmpStr_7.Remove(tmpStr_7.Length - 2, 2)
                        RaiseEvent OnResponse("Field Element 7", emu_common.Common.ReceiveEvents.LOG)
                        'RaiseEvent OnResponse(tmpStr_7, emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Type - " & tmpStr_7.Substring(0, 4), Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Length - " & tmpStr_7.Substring(4, 4), emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Data - " & tmpStr_7.Substring(8, tmpStr_7.Length - 8) & _
                                              " [" & hex2ascii(tmpStr_7.Substring(8, tmpStr_7.Length - 8)) & "]", emu_common.Common.ReceiveEvents.LOG)
                    End If

                    If encounter >= 8 Then
                        tmpStr_8 = tmpStr_8.Remove(tmpStr_8.Length - 2, 2)
                        RaiseEvent OnResponse("Field Element 8", emu_common.Common.ReceiveEvents.LOG)
                        'RaiseEvent OnResponse(tmpStr_8, emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Type - " & tmpStr_8.Substring(0, 4), Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Length - " & tmpStr_8.Substring(4, 4), emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Data - " & tmpStr_8.Substring(8, tmpStr_8.Length - 8) & _
                                              " [" & hex2ascii(tmpStr_8.Substring(8, tmpStr_8.Length - 8)) & "]", emu_common.Common.ReceiveEvents.LOG)
                    End If

                    If encounter >= 9 Then
                        tmpStr_9 = tmpStr_9.Remove(tmpStr_9.Length - 2, 2)
                        RaiseEvent OnResponse("Field Element 9", emu_common.Common.ReceiveEvents.LOG)
                        'RaiseEvent OnResponse(tmpStr_9, emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Type - " & tmpStr_9.Substring(0, 4), Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Length - " & tmpStr_9.Substring(4, 4), emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Data - " & tmpStr_9.Substring(8, tmpStr_9.Length - 8) & _
                                              " [" & hex2ascii(tmpStr_9.Substring(8, tmpStr_9.Length - 8)) & "]", emu_common.Common.ReceiveEvents.LOG)
                    End If

                    If encounter >= 10 Then
                        tmpStr_10 = tmpStr_10.Remove(tmpStr_10.Length - 2, 2)
                        RaiseEvent OnResponse("Field Element 10", emu_common.Common.ReceiveEvents.LOG)
                        'RaiseEvent OnResponse(tmpStr_10, emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Type - " & tmpStr_10.Substring(0, 4), Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Length - " & tmpStr_10.Substring(4, 4), emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Data - " & tmpStr_10.Substring(8, tmpStr_10.Length - 8) & _
                                              " [" & hex2ascii(tmpStr_10.Substring(8, tmpStr_10.Length - 8)) & "]", emu_common.Common.ReceiveEvents.LOG)
                    End If

                    If encounter >= 11 Then
                        tmpStr_11 = tmpStr_11.Remove(tmpStr_11.Length - 2, 2)
                        RaiseEvent OnResponse("Field Element 11", emu_common.Common.ReceiveEvents.LOG)
                        'RaiseEvent OnResponse(tmpStr_11, emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Type - " & tmpStr_11.Substring(0, 4), Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Length - " & tmpStr_11.Substring(4, 4), emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Data - " & tmpStr_11.Substring(8, tmpStr_11.Length - 8) & _
                                              " [" & hex2ascii(tmpStr_11.Substring(8, tmpStr_11.Length - 8)) & "]", emu_common.Common.ReceiveEvents.LOG)
                    End If

                    If encounter >= 12 Then
                        tmpStr_12 = tmpStr_12.Remove(tmpStr_12.Length - 2, 2)
                        RaiseEvent OnResponse("Field Element 12", emu_common.Common.ReceiveEvents.LOG)
                        'RaiseEvent OnResponse(tmpStr_12, emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Type - " & tmpStr_12.Substring(0, 4), Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Length - " & tmpStr_12.Substring(4, 4), emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Data - " & tmpStr_12.Substring(8, tmpStr_12.Length - 8) & _
                                              " [" & hex2ascii(tmpStr_12.Substring(8, tmpStr_12.Length - 8)) & "]", emu_common.Common.ReceiveEvents.LOG)
                    End If

                    If encounter >= 13 Then
                        tmpStr_13 = tmpStr_13.Remove(tmpStr_13.Length - 2, 2)
                        RaiseEvent OnResponse("Field Element 13", emu_common.Common.ReceiveEvents.LOG)
                        'RaiseEvent OnResponse(tmpStr_13, emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Type - " & tmpStr_13.Substring(0, 4), Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Length - " & tmpStr_13.Substring(4, 4), emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Data - " & tmpStr_13.Substring(8, tmpStr_13.Length - 8) & _
                                              " [" & hex2ascii(tmpStr_13.Substring(8, tmpStr_13.Length - 8)) & "]", emu_common.Common.ReceiveEvents.LOG)
                    End If

                    If encounter >= 14 Then
                        tmpStr_14 = tmpStr_14.Remove(tmpStr_14.Length - 2, 2)
                        RaiseEvent OnResponse("Field Element 14", emu_common.Common.ReceiveEvents.LOG)
                        'RaiseEvent OnResponse(tmpStr_14, emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Type - " & tmpStr_14.Substring(0, 4), Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Length - " & tmpStr_14.Substring(4, 4), emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Data - " & tmpStr_14.Substring(8, tmpStr_14.Length - 8) & _
                                              " [" & hex2ascii(tmpStr_14.Substring(8, tmpStr_14.Length - 8)) & "]", emu_common.Common.ReceiveEvents.LOG)
                    End If

                    If encounter >= 15 Then
                        tmpStr_15 = tmpStr_15.Remove(tmpStr_15.Length - 2, 2)
                        RaiseEvent OnResponse("Field Element 15", emu_common.Common.ReceiveEvents.LOG)
                        'RaiseEvent OnResponse(tmpStr_15, emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Type - " & tmpStr_15.Substring(0, 4), Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Length - " & tmpStr_15.Substring(4, 4), emu_common.Common.ReceiveEvents.LOG)
                        RaiseEvent OnResponse("  Field Data - " & tmpStr_15.Substring(8, tmpStr_15.Length - 8) & _
                                              " [" & hex2ascii(tmpStr_15.Substring(8, tmpStr_15.Length - 8)) & "]", emu_common.Common.ReceiveEvents.LOG)
                    End If

                    'If encounter >= 16 Then
                    '    tmpStr_16 = tmpStr_16.Remove(tmpStr_16.Length - 2, 2)
                    '    RaiseEvent OnResponse("Field Element 16", emu_common.Common.ReceiveEvents.LOG)
                    '    'RaiseEvent OnResponse(tmpStr_16, emu_common.Common.ReceiveEvents.LOG)
                    '    RaiseEvent OnResponse("  Field Type - " & tmpStr_16.Substring(0, 4), Common.ReceiveEvents.LOG)
                    '    RaiseEvent OnResponse("  Field Length - " & tmpStr_16.Substring(4, 4), emu_common.Common.ReceiveEvents.LOG)
                    '    RaiseEvent OnResponse("  Field Data - " & tmpStr_16.Substring(8, tmpStr_16.Length - 8) & _
                    '                          " [" & hex2ascii(tmpStr_16.Substring(8, tmpStr_16.Length - 8)) & "]", emu_common.Common.ReceiveEvents.LOG)
                    'End If
                    '==============================================================================

                    'Debug.Print(encounter)

                    RaiseEvent OnResponse("-- BREAKDOWN END --", Common.ReceiveEvents.LOG)
                Catch ex As Exception

                End Try

                'TEST

            Case emu_common.Common.Device.CashlessTerminal
                'CASHLESSTERMINAL   CASHLESSTERMINAL    CASHLESSTERMINAL

                'TEST
                Dim tmpStr As String
                RaiseEvent OnResponse("-- BREAKDOWN START --", Common.ReceiveEvents.LOG)
                Dim k(0) As Byte
                k(0) = arrByte(13)
                tmpStr = "Format version - " & comn.ByteArrayToString(k)
                RaiseEvent OnResponse(tmpStr, Common.ReceiveEvents.LOG)

                k(0) = arrByte(14)
                tmpStr = "Request Response Indicator - " & comn.ByteArrayToString(k)
                RaiseEvent OnResponse(tmpStr, Common.ReceiveEvents.LOG)

                ReDim k(1)
                k(0) = arrByte(15)
                k(1) = arrByte(16)
                tmpStr = "Transaction code - " & comn.ByteArrayToString(k)
                RaiseEvent OnResponse(tmpStr, Common.ReceiveEvents.LOG)

                k(0) = arrByte(17)
                k(1) = arrByte(18)
                tmpStr = "Response code - " & comn.ByteArrayToString(k)
                RaiseEvent OnResponse(tmpStr, Common.ReceiveEvents.LOG)

                ReDim k(0)
                k(0) = arrByte(19)
                tmpStr = "More indicator - " & comn.ByteArrayToString(k)
                RaiseEvent OnResponse(tmpStr, Common.ReceiveEvents.LOG)

                Try
                    Dim encounter As Integer = 0
                    Dim counter As Integer = 0
                    tmpStr = String.Empty
                    For Each item As Byte In arrByte
                        If encounter = 1 Then tmpStr = tmpStr & ConvertDecToHex(item)
                        If item = SEP Then
                            encounter = encounter + 1
                        End If
                    Next

                    tmpStr = tmpStr.Remove(tmpStr.Length - 2, 2)
                    RaiseEvent OnResponse("Field Type - " & tmpStr.Substring(0, 4), Common.ReceiveEvents.LOG)
                    RaiseEvent OnResponse("Field Length - " & tmpStr.Substring(4, 4), emu_common.Common.ReceiveEvents.LOG)
                    RaiseEvent OnResponse("Field Data - " & tmpStr.Substring(8, tmpStr.Length - 8) & " [" & hex2ascii(tmpStr.Substring(8, tmpStr.Length - 8)) & "]", emu_common.Common.ReceiveEvents.LOG)

                    RaiseEvent OnResponse("-- BREAKDOWN END --", Common.ReceiveEvents.LOG)
                Catch ex As Exception

                End Try

                'TEST

        End Select
    End Sub

    Private Function AreArraysEqual(Of T)(ByVal a As T(), ByVal b() As T) As Boolean

        'IF 2 NULL REFERENCES WERE PASSED IN, THEN RETURN TRUE, YOU MAY WANT TO RETURN FALSE
        If a Is Nothing AndAlso b Is Nothing Then Return True

        'CHECK THAT THERE IS NOT 1 NULL REFERENCE ARRAY
        If a Is Nothing Or b Is Nothing Then Return False

        'AT THIS POINT NEITHER ARRAY IS NULL
        'IF LENGTHS DON'T MATCH, THEY ARE NOT EQUAL
        If a.Length <> b.Length Then Return False

        'LOOP ARRAYS TO COMPARE CONTENTS
        For i As Integer = 0 To a.GetUpperBound(0)
            'RETURN FALSE AS SOON AS THERE IS NO MATCH
            If Not a(i).Equals(b(i)) Then Return False
        Next

        'IF WE GOT HERE, THE ARRAYS ARE EQUAL
        Return True

    End Function

    Private Function Checking(ByVal arrByte() As Byte) As Common.Result
        Try
            'Dim nSize As Integer = ConvertHexToDec(ConvertDecToHex(arrByte(2)))
            'Dim newarr(nSize - 1) As Byte
            'Dim nCS As Byte = arrByte(2 + nSize + 1)

            'Array.Copy(arrByte, 3, newarr, 0, nSize)

            'Debug.Print(nSize)
            'Debug.Print(nCS & vbTab & comn.GetCheckSum(newarr))

            'If nCS = comn.GetCheckSum(newarr) Then
            Return Common.Result.SUCCESS
            'Else
            '    Return Common.Result.FAIL
            'End If

        Catch ex As Exception
            Debug.Print("CS ERROR")
            Return Common.Result.FAIL
        End Try
    End Function

    Private Function ConvertDecToHex(ByVal decData As Integer) As String
        Dim tmpHex As String = Hex(decData)
        Try
            Return IIf(tmpHex.Length = 1, "0" & tmpHex, tmpHex)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Function ConvertHexToBinary(ByVal hexData As String) As Integer
        Dim decData As Integer = Nothing

        Try
            decData = ConvertHexToDec(hexData)
            Return Convert.ToString(decData, 2)
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Function ConvertHexToDec(ByVal hexData As String) As Integer
        Try
            Return Convert.ToInt32(hexData, 16)
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Function ConvertHexToASC(ByVal hexData As String) As String
        Try
            ConvertHexToASC = Convert.ToChar(System.Convert.ToUInt32(hexData, 16)).ToString
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Function ConvertHexToSingle(ByVal hexValue As String) As Single
        Dim iInputIndex As Integer = 0
        Dim iOutputIndex As Integer = 0
        Dim bArray(3) As Byte

        Try
            For iInputIndex = 0 To hexValue.Length - 1 Step 2
                bArray(iOutputIndex) = Byte.Parse(hexValue.Chars(iInputIndex) & hexValue.Chars(iInputIndex + 1), Globalization.NumberStyles.HexNumber)
                iOutputIndex += 1
            Next

            Array.Reverse(bArray)

            Return BitConverter.ToSingle(bArray, 0)
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Function hex2ascii(ByVal hextext As String) As String
        Dim Y As Long
        Dim num As String
        Dim value As String = String.Empty

        For Y = 1 To Len(hextext)
            num = Mid(hextext, Y, 2)
            value = value & Chr(Val("&h" & num))
            Y = Y + 1
        Next Y

        hex2ascii = value
    End Function

    Private Function ConvertHexStringToASCII(ByVal hexDataString As String) As String
        Try
            ConvertHexStringToASCII = "" 'Convert.ToChar(System.Convert.ToUInt32(hexData, 16)).ToString
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Function BCD_to_Int(ByVal LowW As Int16, ByVal HighW As Int16) As Int32

        Dim tempH As Int32 = 0
        Dim tempL As Int32 = 0
        Dim RunH As Int32 = 0
        Dim RunL As Int32 = 0

        For i As Integer = 0 To 3
            tempH = (HighW And &HF000) >> 12    'get the high byte
            RunH *= 10
            RunH = RunH + tempH
            HighW <<= 4

            tempL = (LowW And &HF000) >> 12     'get the high byte
            RunL *= 10
            RunL = RunL + tempL
            LowW <<= 4
        Next i

        Return (RunH * 10000) + RunL

    End Function

    Public Sub dispose()
        _timeoutTimer.Dispose()
        buffer = Nothing
        Disconnect()
        COM.Dispose()
    End Sub
#End Region

End Class
