Public Class ComClass
    Dim COM As System.IO.Ports.SerialPort

    Private _connection As ConnectionStatus

    Private _errortitle As String

    Public arrLastReceived() As Byte
    Private buffer() As Byte

    Private _replyTimer As System.Timers.Timer
    Private _timeoutTimer As System.Timers.Timer
    Private Const TIMEOUT As Integer = 120 '(seconds)
    Private TIMEOUT_COUNTER As Integer = 0

    Private REPLY_FLAG As Boolean = False
    Private REPLY_TYPE As Common.ReplyType

    Private comn As New Common

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
    'Public ReadOnly Property Loaded() As Boolean
    '    Get
    '        Return _finished_load
    '    End Get
    'End Property
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

    Private Sub SendMessage()
        Try
            Dim tmp() As Byte = PRESERVED_ADHOC
            tmp(1) = &H84
            '_finished_load = False
            buffer = Nothing 'clear buffer for next retrieve
            '_timeoutTimer.Start()
            COM.Write(tmp, 0, tmp.Length)
            RaiseEvent OnResponse(ENTITY_RSC & "SendMessage: " & comn.ByteArrayToString(tmp), Common.ReceiveEvents.DTO)
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
            RaiseEvent OnResponse(ENTITY_RSC & "SendAcknowledge: " & comn.ByteArrayToString(COM_ACK_HEX), Common.ReceiveEvents.DTO)
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
            RaiseEvent OnResponse(ENTITY_RSC & "Send NAcknowledge: " & comn.ByteArrayToString(COM_NACK_HEX), Common.ReceiveEvents.DTO)
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
            'RaiseEvent OnResponse("Send Acknowledge: " & comn.ByteArrayToString(COM_ACK_HEX), Common.ReceiveEvents.DTO)
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
        COM.ReadTimeout = 10000
        COM.WriteTimeout = 10000

        COM.ReadBufferSize = 8192

        Debug.Print(COM.ReadBufferSize)
        'Add handler to event that process messages received from the serial port
        AddHandler COM.DataReceived, AddressOf DataReceivedHandler

        _replyTimer = New System.Timers.Timer(RESPONSE_TIME) '1s interval
        'Set TimeOut Timer
        _timeoutTimer = New System.Timers.Timer(1000) '1s interval
        AddHandler _replyTimer.Elapsed, AddressOf replyHandler
        AddHandler _timeoutTimer.Elapsed, AddressOf TimeOutHandler
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

    Private Sub DataReceivedHandler(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs)
        Dim arrByte() As Byte
        Dim tempBuffer() As Byte
        Dim intOffset As Integer
        Dim arrDeValueLen(1) As Byte
        Dim sp As System.IO.Ports.SerialPort = CType(sender, System.IO.Ports.SerialPort)
        Dim intBytesToRead As Integer = sp.BytesToRead
        Dim inData(intBytesToRead - 1) As Byte

        Dim blnEOT As Boolean
        'Reset Timeout Counter
        _timeoutTimer.Stop()
        'Dim dsNew As DataSet = Nothing
        'Dim drNewRow As DataRow = Nothing

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

        If blnEOT = False Then
            arrByte = buffer
            If (arrByte(0) = STX) AndAlso (arrByte(UBound(arrByte)) = EOT) Then
                If Checking(arrByte) = Common.Result.FAIL Then
                    'RaiseEvent OnEvent("Acknowledge", Common.TRX.RECEIVE)
                    RaiseEvent OnResponse(ENTITY_OBU & "Invalid Checksum " & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.ERRORS)
                    REPLY_TYPE = Common.ReplyType.NAcknowledge
                    _replyTimer.Start()
                Else

                    If arrByte.Length <= 6 Then
                        If (arrByte(0) = STX) Then
                            Select Case arrByte(1)
                                Case COMMAND_ACK
                                    If REPLY_FLAG = False Then
                                        Select Case arrByte(3)
                                            Case &H0
                                                RaiseEvent OnEvent("NAcknowledge", Common.TRX.RECEIVE)
                                                RaiseEvent OnResponse(ENTITY_OBU & "NACK " & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.ERRORS)
                                                REPLY_TYPE = Common.ReplyType.Acknowledge
                                                _replyTimer.Start()
                                            Case &H1
                                                RaiseEvent OnEvent("Acknowledge", Common.TRX.RECEIVE)
                                                RaiseEvent OnResponse(ENTITY_OBU & "ACK " & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.ERRORS)
                                                REPLY_TYPE = Common.ReplyType.Acknowledge
                                                _replyTimer.Start()
                                        End Select
                                    End If
                                Case COMMAND_END_ADHOC
                                    If REPLY_FLAG = False Then
                                        'ConvertHexToDec(arrByte(2))
                                        RaiseEvent OnEvent("Exit ADHOC", Common.TRX.RECEIVE)
                                        RaiseEvent OnResponse(ENTITY_OBU & "END " & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.ERRORS)
                                        REPLY_TYPE = Common.ReplyType.Acknowledge
                                        _replyTimer.Start()
                                    End If
                                Case COMMAND_READ
                                    If REPLY_FLAG = False Then
                                        RaiseEvent OnEvent("Read", Common.TRX.RECEIVE)
                                        RaiseEvent OnResponse(ENTITY_OBU & "READ " & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.ERRORS)
                                        REPLY_TYPE = Common.ReplyType.Message
                                        _replyTimer.Start()
                                    End If
                            End Select
                        End If
                    Else
                        If arrByte(1) = COMMAND_START_ADHOC Then
                            If REPLY_FLAG = False Then

                                ProcessAdhoc(arrByte)

                                PRESERVED_ADHOC = arrByte
                                RaiseEvent OnEvent("ADHOC", Common.TRX.RECEIVE)
                                RaiseEvent OnResponse(ENTITY_OBU & "ADHOC " & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.DTO)
                                REPLY_TYPE = Common.ReplyType.Acknowledge
                                _replyTimer.Start()
                            End If
                        Else
                            RaiseEvent OnEvent("UNKNOWN", Common.TRX.RECEIVE)
                            RaiseEvent OnResponse(ENTITY_OBU & "Unknown " & comn.ByteArrayToString(arrByte), Common.ReceiveEvents.DTO)
                            REPLY_TYPE = Common.ReplyType.NAcknowledge
                            _replyTimer.Start()
                        End If
                    End If

                End If
                '_finished_load = True
            End If
        End If
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

    Private Function Checking(ByVal arrByte() As Byte) As Common.Result
        Try
            Dim nSize As Integer = ConvertHexToDec(ConvertDecToHex(arrByte(2)))
            Dim newarr(nSize - 1) As Byte
            Dim nCS As Byte = arrByte(2 + nSize + 1)

            Array.Copy(arrByte, 3, newarr, 0, nSize)

            Debug.Print(nSize)
            Debug.Print(nCS & vbTab & comn.GetCheckSum(newarr))

            If nCS = comn.GetCheckSum(newarr) Then
                Return Common.Result.SUCCESS
            Else
                Return Common.Result.FAIL
            End If

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

    Public Sub dispose()
        _timeoutTimer.Dispose()
        buffer = Nothing
        Disconnect()
        COM.Dispose()
    End Sub
#End Region

End Class
