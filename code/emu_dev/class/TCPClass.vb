Imports System.Net.Sockets

Public Class TCPClass

    Const BUFFER_SIZE As Integer = 32
    Dim ENTITY_STR As String

    Dim _ip As String
    Dim _port As Int32

    Dim client As TcpClient
    Dim IQPStream As NetworkStream
    'Dim DataBuffer(1024) As Byte
    Dim DataBuffer(BUFFER_SIZE) As Byte
    Dim ByteRead As Integer

    Dim evtConnEstb As New AsyncCallback(AddressOf onConnectionEstablished)
    Dim evtDataArrival As New AsyncCallback(AddressOf DataProcessing)

    Public Event OnResponse(ByVal nLog As Object, ByVal value As Common.ReceiveEvents)
    Public Event OnEvent(ByVal nLog As Object, ByVal value As Common.TRX)

    Private _replyTimer As System.Timers.Timer
    Private REPLY_TYPE As Common.ReplyType
    Private WAITING_REPLY_FLAG As Boolean

    Private DOWNLOAD_FLAG As Boolean

    Dim cmmn As New Common

    Public ReadOnly Property IP() As String
        Get
            Return Me._ip
        End Get
    End Property

    Public ReadOnly Property Port() As String
        Get
            Return Me._port
        End Get
    End Property

    Public Sub New(ByVal dev As Common.Device)
        Select Case dev
            Case Common.Device.DriverConsole
                ENTITY_STR = ENTITY_DC
            Case Common.Device.RearSeatMonitor
                ENTITY_STR = ENTITY_RSM
            Case Else
                ENTITY_STR = String.Empty
        End Select
        _replyTimer = New System.Timers.Timer(RESPONSE_TIME)
        AddHandler _replyTimer.Elapsed, AddressOf replyHandler
    End Sub

    Public Sub connect(ByVal IP As String, ByVal Port As Int32)
        Me._ip = IP
        Me._port = Port
        client = New TcpClient
        client.BeginConnect(IP, Port, evtConnEstb, Nothing)
    End Sub

    Public Sub SendHello()
        Dim data() As Byte = System.Text.Encoding.ASCII.GetBytes("Hello World")
        Try
            If client.Connected Then
                Dim stream As NetworkStream = client.GetStream()
                stream.Write(data, 0, data.Length)
                'stream.Close()
                'stream.Dispose()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub Test()
        Try
            If client.Connected Then
                Dim stream As NetworkStream = client.GetStream()
                stream.Write(DC_ACK_HEX, 0, DC_ACK_HEX.Length)
                'stream.Close()
                'stream.Dispose()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub SendAcknowledge()
        Try
            If client.Connected Then
                Dim stream As NetworkStream = client.GetStream()
                stream.Write(DC_ACK_HEX, 0, DC_ACK_HEX.Length)
                RaiseEvent OnResponse(ENTITY_STR & "ACK  " & cmmn.ByteArrayToString(cmmn.TrimBytesArray(DC_ACK_HEX)), Common.ReceiveEvents.ERRORS)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub SendNAcknowledge()
        Try
            If client.Connected Then
                Dim stream As NetworkStream = client.GetStream()
                stream.Write(DC_NACK_HEX, 0, DC_NACK_HEX.Length)
                RaiseEvent OnResponse(ENTITY_STR & "NACK  " & cmmn.ByteArrayToString(cmmn.TrimBytesArray(DC_NACK_HEX)), Common.ReceiveEvents.ERRORS)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub SendTaxiValidateReq()
        Try
            If client.Connected Then
                Dim stream As NetworkStream = client.GetStream()
                stream.Write(DC_TAXI_VALIDATE_REQ, 0, DC_TAXI_VALIDATE_REQ.Length)
                RaiseEvent OnResponse(ENTITY_STR & "Taxi validate request  " & cmmn.ByteArrayToString(cmmn.TrimBytesArray(DC_TAXI_VALIDATE_REQ)), Common.ReceiveEvents.ERRORS)
                RaiseEvent OnEvent("Taxi validation request sent.", Common.TRX.SEND)
            End If
        Catch ex As Exception
            RaiseEvent OnResponse(ENTITY_STR & "SendTaxiValidateReq: " & ex.Message, Common.ReceiveEvents.ERRORS)
        End Try
    End Sub

    Public Sub SendUpdateTaxiProfile(ByVal trackerID As String, ByVal plateNo As String)
        Try
            If client.Connected Then
                Dim stream As NetworkStream = client.GetStream()
                Dim DataBytes(-1) As Byte
                Dim command_str As String = String.Empty

                Dim drv() As Byte = System.Text.Encoding.ASCII.GetBytes(trackerID)
                ReDim Preserve DataBytes(UBound(drv))
                drv.CopyTo(DataBytes, 0)
                ReDim Preserve DataBytes(UBound(DataBytes) + 1)
                DataBytes(UBound(DataBytes)) = SEPARATOR
                Dim pwd() As Byte = System.Text.Encoding.ASCII.GetBytes(plateNo)
                Dim offset As Integer = UBound(DataBytes) + 1
                ReDim Preserve DataBytes(UBound(DataBytes) + UBound(pwd) + 1)
                pwd.CopyTo(DataBytes, offset)
                ReDim Preserve DataBytes(UBound(DataBytes) + 1)
                DataBytes(UBound(DataBytes)) = SEPARATOR

                Dim CS As Byte = cmmn.GetCheckSum(DataBytes)
                ReDim Preserve DataBytes(UBound(DataBytes) + 1)
                DataBytes(UBound(DataBytes)) = CS

                Dim newData(0) As Byte
                newData(0) = STX
                ReDim Preserve newData(UBound(newData) + 1)
                newData(UBound(newData)) = COMMAND_UPDATE_PROFILE_DC

                offset = UBound(newData) + 1
                ReDim Preserve newData(UBound(newData) + UBound(DataBytes) + 1)
                DataBytes.CopyTo(newData, offset)

                ReDim Preserve newData(UBound(newData) + 1)
                newData(UBound(newData)) = EOT

                stream.Write(newData, 0, newData.Length)
                RaiseEvent OnResponse(ENTITY_STR & "SendUpdateTaxiProfile  " & cmmn.ByteArrayToString(cmmn.TrimBytesArray(newData)), Common.ReceiveEvents.ERRORS)
                RaiseEvent OnEvent("Taxi profile sent.", Common.TRX.SEND)
            End If
        Catch ex As Exception
            RaiseEvent OnResponse(ENTITY_STR & "SendUpdateTaxiProfile " & ex.Message, Common.ReceiveEvents.ERRORS)
        End Try
    End Sub

    Public Sub SendLogin(ByVal drvname As String, ByVal drvpwd As String)
        Try
            If client.Connected Then
                Dim stream As NetworkStream = client.GetStream()
                Dim DataBytes(-1) As Byte
                Dim command_str As String = String.Empty

                Dim drv() As Byte = System.Text.Encoding.ASCII.GetBytes(drvname)
                ReDim Preserve DataBytes(UBound(drv))
                drv.CopyTo(DataBytes, 0)
                ReDim Preserve DataBytes(UBound(DataBytes) + 1)
                DataBytes(UBound(DataBytes)) = SEPARATOR
                Dim pwd() As Byte = System.Text.Encoding.ASCII.GetBytes(drvpwd)
                Dim offset As Integer = UBound(DataBytes) + 1
                ReDim Preserve DataBytes(UBound(DataBytes) + UBound(pwd) + 1)
                pwd.CopyTo(DataBytes, offset)
                ReDim Preserve DataBytes(UBound(DataBytes) + 1)
                DataBytes(UBound(DataBytes)) = SEPARATOR

                Dim CS As Byte = cmmn.GetCheckSum(DataBytes)
                ReDim Preserve DataBytes(UBound(DataBytes) + 1)
                DataBytes(UBound(DataBytes)) = CS

                Dim newData(0) As Byte
                newData(0) = STX
                ReDim Preserve newData(UBound(newData) + 1)
                newData(UBound(newData)) = COMMAND_DRV_LOGIN_DC

                offset = UBound(newData) + 1
                ReDim Preserve newData(UBound(newData) + UBound(DataBytes) + 1)
                DataBytes.CopyTo(newData, offset)

                ReDim Preserve newData(UBound(newData) + 1)
                newData(UBound(newData)) = EOT

                stream.Write(newData, 0, newData.Length)
                RaiseEvent OnResponse(ENTITY_STR & "SendLogin  " & cmmn.ByteArrayToString(cmmn.TrimBytesArray(newData)), Common.ReceiveEvents.ERRORS)
                RaiseEvent OnEvent("Driver Login sent.", Common.TRX.SEND)
            End If
        Catch ex As Exception
            RaiseEvent OnResponse(ENTITY_STR & "SendLogin " & ex.Message, Common.ReceiveEvents.ERRORS)
        End Try
    End Sub

    Public Sub SendRequestUpdate(ByVal device As Common.Device, ByVal Request As Common.RequestUpdateType)
        Try
            If client.Connected Then
                Dim valid As Boolean = False
                Dim stream As NetworkStream = client.GetStream()
                Dim DataBytes(-1) As Byte
                Dim command_str As String = String.Empty

                ReDim Preserve DataBytes(UBound(DataBytes) + 1)
                DataBytes(UBound(DataBytes)) = STX

                Select Case device
                    Case Common.Device.DriverConsole 'DRIVER CONSOLE
                        If Request = Common.RequestUpdateType.ContextUpdate Then
                            ReDim Preserve DataBytes(UBound(DataBytes) + 1)
                            DataBytes(UBound(DataBytes)) = COMMAND_CONTEXT_UPDATE_REQUEST_REPLY_RSM
                            command_str = "Context update request"
                            valid = True
                        Else
                            ReDim Preserve DataBytes(UBound(DataBytes) + 1)
                            DataBytes(UBound(DataBytes)) = COMMAND_DRIVER_INFORMATION_UPDATE_REQUEST_DC
                            command_str = "Driver information update request"
                            valid = True
                        End If

                        'FIX DATA STRING (NULL AND CHECKSUM)
                        ReDim Preserve DataBytes(UBound(DataBytes) + 1)
                        DataBytes(UBound(DataBytes)) = &H0
                        ReDim Preserve DataBytes(UBound(DataBytes) + 1)
                        DataBytes(UBound(DataBytes)) = &H1

                    Case Common.Device.RearSeatMonitor 'REAR SEAT MONITOR
                        If Request = Common.RequestUpdateType.ContextUpdate Then
                            ReDim Preserve DataBytes(UBound(DataBytes) + 1)
                            DataBytes(UBound(DataBytes)) = COMMAND_CONTEXT_UPDATE_REQUEST_RSM
                            command_str = "Context update request"
                            valid = True
                        Else
                            ReDim Preserve DataBytes(UBound(DataBytes) + 1)
                            DataBytes(UBound(DataBytes)) = COMMAND_DRIVER_INFORMATION_CONTEXT_UPDATE_REQUEST_RSM
                            command_str = "Driver information update request"
                            valid = True
                        End If

                        'FIX DATA STRING (NULL AND CHECKSUM)
                        ReDim Preserve DataBytes(UBound(DataBytes) + 1)
                        DataBytes(UBound(DataBytes)) = &H0
                        ReDim Preserve DataBytes(UBound(DataBytes) + 1)
                        DataBytes(UBound(DataBytes)) = &H1

                End Select

                ReDim Preserve DataBytes(UBound(DataBytes) + 1)
                DataBytes(UBound(DataBytes)) = EOT

                If valid Then
                    stream.Write(DataBytes, 0, DataBytes.Length)
                    RaiseEvent OnEvent("Context Update", Common.TRX.SEND)
                    RaiseEvent OnResponse(ENTITY_STR & command_str & " " & cmmn.ByteArrayToString(cmmn.TrimBytesArray(DataBytes)), Common.ReceiveEvents.ERRORS)
                Else
                    'RaiseEvent OnResponse(command_str & " " & cmmn.ByteArrayToString(cmmn.TrimBytesArray(DataBytes)), Common.ReceiveEvents.ERRORS)
                End If
            End If
        Catch ex As Exception
            RaiseEvent OnResponse(ENTITY_STR & "SendRequestUpdate " & ex.Message, Common.ReceiveEvents.ERRORS)
        End Try
    End Sub

    Public Sub SendSOS(ByVal Activate As Boolean, ByVal Device As Common.Device)
        Try
            If Not (Device = Common.Device.DriverConsole Or Device = Common.Device.RearSeatMonitor) Then Exit Sub
            If client.Connected Then
                Dim command As Byte = IIf(Device = Common.Device.DriverConsole, COMMAND_SOS_DC, COMMAND_SOS_RSM)
                Dim value As Byte = IIf(Activate, &H1, &H0)
                Dim s As New ArrayList
                s.Add(value)

                Dim msg() As Byte = {STX, command, value, cmmn.GetCheckSum(s.ToArray()), EOT}
                Dim stream As NetworkStream = client.GetStream()
                stream.Write(msg, 0, msg.Length)
                RaiseEvent OnEvent("SOS " & IIf(Activate, "Activated", "Deactivated"), Common.TRX.SEND)
                RaiseEvent OnResponse(ENTITY_STR & "SOS  " & cmmn.ByteArrayToString(cmmn.TrimBytesArray(msg)), Common.ReceiveEvents.ERRORS)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub disconnect()
        Try
            IQPStream.Flush()
            IQPStream.Close()
            client.Client.Disconnect(False)
            client.Client.Close()
            client.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ReConnect()
        If client.Connected Then disconnect()
        client.Connect(Me._ip, Me._port)
    End Sub

#Region "CallBacks"
    Public Sub DataProcessing(ByVal dr As IAsyncResult)

        If DataBuffer.Length > 3 Then

            Dim result As Common.Result = Checking(cmmn.TrimBytesArray(DataBuffer))

            If result = Common.Result.FAIL Then
                RaiseEvent OnResponse(ENTITY_STR & "Invalid checksum  " & cmmn.ByteArrayToString(cmmn.TrimBytesArray(DataBuffer)), Common.ReceiveEvents.ERRORS)
                REPLY_TYPE = Common.ReplyType.NAcknowledge
                _replyTimer.Start()
            ElseIf result = Common.Result.NULL Then
                'do nothing
            Else

                Select Case DataBuffer(1)
                    Case COMMAND_DRIVER_INFORMATION_UPDATE_REQUEST_DC
                        RaiseEvent OnEvent("Driver Info Context Update Request", Common.TRX.RECEIVE)
                        RaiseEvent OnResponse(ENTITY_OBU & "Driver Information  " & cmmn.ByteArrayToString(cmmn.TrimBytesArray(DataBuffer)), Common.ReceiveEvents.ERRORS)
                        'check
                        REPLY_TYPE = Common.ReplyType.Acknowledge
                        _replyTimer.Start()
                        'Case COMMAND_DRV_LOGIN_DC

                    Case COMMAND_PROFILE_VALIDATION_REPLY_DC
                        RaiseEvent OnEvent("Validation:", Common.TRX.RECEIVE)
                        RaiseEvent OnResponse(ENTITY_OBU & "Taxi profile validation: " & cmmn.ByteArrayToString(cmmn.TrimBytesArray(DataBuffer)), Common.ReceiveEvents.ERRORS)
                        'check
                        REPLY_TYPE = Common.ReplyType.Acknowledge
                        _replyTimer.Start()
                    Case COMMAND_ACK
                        Select Case DataBuffer(2)
                            Case &H0
                                RaiseEvent OnEvent("NAcknowledge", Common.TRX.RECEIVE)
                                RaiseEvent OnResponse(ENTITY_OBU & "NACK  " & cmmn.ByteArrayToString(cmmn.TrimBytesArray(DataBuffer)), Common.ReceiveEvents.ERRORS)
                            Case &H1
                                RaiseEvent OnEvent("Acknowledge", Common.TRX.RECEIVE)
                                RaiseEvent OnResponse(ENTITY_OBU & "ACK  " & cmmn.ByteArrayToString(cmmn.TrimBytesArray(DataBuffer)), Common.ReceiveEvents.ERRORS)
                        End Select
                    Case COMMAND_CONTEXT_UPDATE_REQUEST_REPLY_RSM
                        RaiseEvent OnEvent("Context Update Reply", Common.TRX.RECEIVE)
                        RaiseEvent OnResponse(ENTITY_OBU & "Context update reply:  " & cmmn.ByteArrayToString(cmmn.TrimBytesArray(DataBuffer)), Common.ReceiveEvents.ERRORS)

                        'check


                        If DataBuffer(2) = &H1 Then
                            RaiseEvent OnResponse("Downloading updated content.", Common.ReceiveEvents.ERRORS)
                            'if exist initiate FTP download
                            Try
                                If Not FTP_CONTENT_FILE = "*.*" Then
                                    cmmn.DownloadFtpFile("", FTP_CONTENT_FILE)
                                    RaiseEvent OnResponse("FTP download: " & FTP_CONTENT_FILE & " success.", Common.ReceiveEvents.ERRORS)
                                Else
                                    cmmn.DownloadFtpFiles()
                                    RaiseEvent OnResponse("FTP download success.", Common.ReceiveEvents.ERRORS)
                                End If
                            Catch ex As Exception
                                RaiseEvent OnResponse("FTP download: " & ex.Message, Common.ReceiveEvents.ERRORS)
                            End Try
                        Else
                            RaiseEvent OnResponse("No updates.", Common.ReceiveEvents.ERRORS)
                        End If


                        REPLY_TYPE = Common.ReplyType.Acknowledge
                        _replyTimer.Start()

                    Case COMMAND_DRIVER_INFORMATION_CONTEXT_UPDATE_REQUEST_RSM 'Rear
                        RaiseEvent OnEvent("Context Update Request:", Common.TRX.RECEIVE)
                        RaiseEvent OnResponse(ENTITY_OBU & "Rear seat monitor: " & cmmn.ByteArrayToString(cmmn.TrimBytesArray(DataBuffer)), Common.ReceiveEvents.ERRORS)
                        'check
                        REPLY_TYPE = Common.ReplyType.Acknowledge
                        _replyTimer.Start()
                    Case Else
                        RaiseEvent OnEvent("Unknown", Common.TRX.RECEIVE)
                        RaiseEvent OnResponse(ENTITY_OBU & "Unknown " & cmmn.ByteArrayToString(cmmn.TrimBytesArray(DataBuffer)), Common.ReceiveEvents.ERRORS)
                        REPLY_TYPE = Common.ReplyType.NAcknowledge
                        _replyTimer.Start()
                End Select
            End If
        End If

        IQPStream.Flush()
        StartListening()
    End Sub

    Public Sub onConnectionEstablished(ByVal dr As IAsyncResult)
        Debug.Print("Connect Test Eventzd")
        RaiseEvent OnResponse("Connected to " & Me._ip & ":" & Me._port, Common.ReceiveEvents.ERRORS)
        StartListening()
    End Sub

    Private Sub StartListening()
        If client.Connected Then
            IQPStream = client.GetStream
            'IQPStream.BeginRead(DataBuffer, 0, 1024, evtDataArrival, Nothing)
            IQPStream.BeginRead(DataBuffer, 0, BUFFER_SIZE, evtDataArrival, Nothing)
        End If
    End Sub

    Private Sub replyHandler()
        _replyTimer.Stop()
        Select Case REPLY_TYPE
            Case Common.ReplyType.Acknowledge
                SendAcknowledge()
                RaiseEvent OnEvent("Acknowledge sent.", Common.TRX.SEND)
            Case Common.ReplyType.NAcknowledge
                SendNAcknowledge()
                RaiseEvent OnEvent("NAcknowledge sent.", Common.TRX.SEND)
            Case Common.ReplyType.Message
                'SendMessage()
                RaiseEvent OnEvent("Message sent.", Common.TRX.SEND)
        End Select

    End Sub

    Private Function Checking(ByVal arrByte() As Byte) As Common.Result
        Try
            If arrByte.Length = 0 Then Return Common.Result.NULL

            Dim nSize As Integer = UBound(arrByte) - 3
            Dim newarr(nSize - 1) As Byte
            Dim nCS As Byte = arrByte(2 + nSize)

            Array.Copy(arrByte, 2, newarr, 0, nSize)

            Debug.Print(nSize)
            Debug.Print(nCS & vbTab & cmmn.GetCheckSum(newarr))

            If nCS = cmmn.GetCheckSum(newarr) Then
                Return Common.Result.SUCCESS
            Else
                Return Common.Result.FAIL
            End If

        Catch ex As Exception
            Debug.Print("CS ERROR")
            Return Common.Result.FAIL
        End Try
    End Function

#End Region

#Region "Commons"

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
#End Region

End Class
