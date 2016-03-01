Imports System.Text
Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Class Server
    Private _tcpListener As TcpListener
    Private _listenThread As Thread

    Public Event ReceiveData(ByVal data As String)
    Public Event ClientEvent(ByVal client As emu_common.Client)
    Public Event OnEvent(ByVal nLog As Object, ByVal value As Common.TRX)

    Dim kommon As New Common

    Dim ClientList As New List(Of TcpClient)
    Dim MessageQueue As New Queue(Of Message)

#Region "Properties"

    Public ReadOnly Property getClientList() As Object
        Get
            Dim IP As New ArrayList
            For Each x As TcpClient In Me.ClientList
                Dim ipend = (IPAddress.Parse(CType(x.Client.RemoteEndPoint, IPEndPoint).Address.ToString()))
                IP.Add(ipend.ToString)
            Next

            Return IP.ToArray
        End Get
    End Property

#End Region

#Region "Private Methods"

    Private Sub AddToClientList(ByVal dClient As TcpClient)
        ClientList.Add(dClient)
        'pending list
    End Sub

    Private Sub RemoveFromClientList(ByVal dClient As TcpClient)
        Try
            For Each x As TcpClient In Me.ClientList
                Dim ipend = (IPAddress.Parse(CType(x.Client.RemoteEndPoint, IPEndPoint).Address.ToString()))
                If IPAddress.Parse(CType(dClient.Client.RemoteEndPoint, IPEndPoint).Address.ToString()).ToString = ipend.ToString Then
                    Debug.Print("remove" & vbTab & ClientList.Remove(x))
                    Exit For
                End If
            Next
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Sub

    Private Sub ListenForClients()
        Me._tcpListener.Start()

        Dim client As TcpClient
        Dim clientThread As Thread

        Try
            While True
                'blocks until a client has connected to the server
                client = Me._tcpListener.AcceptTcpClient()

                'create a thread to handle communication 
                'with connected client
                clientThread = New Thread(New ParameterizedThreadStart(AddressOf HandleClientComm))
                clientThread.Start(client)

            End While
        Catch ex As Exception
            WriteToEventLog("ServerError01 : " & ex.ToString)
            Throw ex
        Finally
            client = Nothing
            clientThread = Nothing
        End Try
    End Sub

    Private Sub HandleClientComm(ByVal client As Object)
        Dim tcpClient As TcpClient
        Dim clientStream As NetworkStream
        Dim message As Byte()
        Dim bytesRead As Integer
        Dim aClient As emu_common.Client

        Try
            tcpClient = DirectCast(client, TcpClient)

            AddToClientList(tcpClient)

            clientStream = tcpClient.GetStream()
            message = New Byte(4095) {}

            Dim ipend = (IPAddress.Parse(CType(tcpClient.Client.RemoteEndPoint, IPEndPoint).Address.ToString()))

            Debug.Print(ipend.ToString)

            While True
                bytesRead = 0

                Try
                    'blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096)
                Catch
                    'a socket error has occured
                    Exit Try
                End Try

                If bytesRead = 0 Then
                    'the client has disconnected from the server
                    Exit While
                End If

                Try
                    aClient = New emu_common.Client(kommon.ParseData(ByteToHex(message, 0, bytesRead)).GetTrackerID, ipend.ToString)
                    RaiseEvent ClientEvent(aClient)
                Catch ex As Exception

                End Try

                'message has successfully been received
                RaiseEvent ReceiveData(ByteToHex(message, 0, bytesRead))

                'RESPONSE ACKNOWLEDGE
                clientStream.BeginWrite(DC_ACK_HEX, 0, DC_ACK_HEX.Length, Nothing, Nothing)
                RaiseEvent OnEvent(ENTITY_SERVER & kommon.ByteArrayToString(DC_ACK_HEX), Common.TRX.SEND)

            End While

            RemoveFromClientList(tcpClient)
            tcpClient.Close()

        Catch ex As Exception
            WriteToEventLog("ServerError02 : " & ex.ToString)
            Throw ex
        Finally
            tcpClient = Nothing
            clientStream = Nothing
            message = Nothing
            bytesRead = Nothing
            If Not IsNothing(aClient) Then
                aClient.IPAddress = "NULL"
                RaiseEvent ClientEvent(aClient)
            End If
        End Try
    End Sub

    Private Function ByteToHex(ByVal Bytes() As Byte) As String
        ByteToHex = String.Empty

        Dim objSB As StringBuilder
        Dim intIndex As Integer

        Try
            objSB = New StringBuilder(UBound(Bytes) + 1)

            With objSB
                For intIndex = 0 To UBound(Bytes)
                    'If (intIndex > 33) Then
                    '    Exit For
                    'End If

                    .AppendFormat("{0:X2}", Bytes(intIndex))

                    If (intIndex <> UBound(Bytes)) Then
                        .Append(":")
                    End If
                Next

                ByteToHex = .ToString

                'If (.ToString.Trim.Length >= 101) Then
                '    ByteToHex = .ToString.Substring(0, 101)
                'Else
                '    ByteToHex = .ToString.Substring(0, .ToString.Trim.Length)
                'End If
            End With
        Catch ex As Exception
            WriteToEventLog("ServerError03 : " & ex.ToString)
            Throw ex
        Finally
            objSB = Nothing
            intIndex = Nothing
        End Try
    End Function

    Private Function ByteToHex(ByVal Bytes() As Byte, ByVal offset As Integer, ByVal count As Integer) As String
        ByteToHex = String.Empty

        Dim objSB As StringBuilder
        Dim intIndex As Integer

        Try
            objSB = New StringBuilder(count)

            With objSB
                For intIndex = offset To offset + count - 1
                    'If (intIndex > 33) Then
                    '    Exit For
                    'End If

                    .AppendFormat("{0:X2}", Bytes(intIndex))

                    If (intIndex <> (offset + count - 1)) Then
                        .Append(":")
                    End If
                Next

                ByteToHex = .ToString

                'If (.ToString.Trim.Length >= 101) Then
                '    ByteToHex = .ToString.Substring(0, 101)
                'Else
                '    ByteToHex = .ToString.Substring(0, .ToString.Trim.Length)
                'End If
            End With
        Catch ex As Exception
            WriteToEventLog("ServerError04 : " & ex.ToString)
            Throw ex
        Finally
            objSB = Nothing
            intIndex = Nothing
        End Try
    End Function

    Private Function WriteToEventLog(ByVal Entry As String)
        Dim appName As String = "AVLReceiver"
        Dim eventType As EventLogEntryType = EventLogEntryType.Error
        Dim logName = "Application"
        Dim objEventLog As New EventLog()

        Try
            'Register the App as an Event Source
            If Not EventLog.SourceExists(appName) Then
                EventLog.CreateEventSource(appName, logName)
            End If

            objEventLog.Source = appName

            'WriteEntry is overloaded; this is one
            'of 10 ways to call it
            objEventLog.WriteEntry(Entry, eventType)
            objEventLog.Dispose()
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub GetMessageFrame(ByRef MessageContent() As Byte, ByVal MessageID As Byte)
        Dim tempData() As Byte = MessageContent.Clone

        Array.Clear(MessageContent, 0, MessageContent.Length)

        ReDim Preserve MessageContent(UBound(MessageContent) + 2)
        MessageContent(0) = SOF
        MessageContent(1) = MessageID

        tempData.CopyTo(MessageContent, 2)

        ReDim Preserve MessageContent(UBound(MessageContent) + 1)
        MessageContent(UBound(MessageContent)) = EOF
    End Sub

#End Region

#Region "Public Methods"

    Public Sub New(ByVal portNo As Integer)
        Me._tcpListener = New TcpListener(IPAddress.Any, portNo)
        Me._listenThread = New Thread(New ThreadStart(AddressOf ListenForClients))
        Me._listenThread.Start()
    End Sub

    Public Sub TestSend(ByVal dIPAddress As String)
        Dim clientStream As NetworkStream
        Dim sent As Boolean = False
        Try
            For Each x As TcpClient In ClientList
                If IPAddress.Parse(CType(x.Client.RemoteEndPoint, IPEndPoint).Address.ToString()).ToString = dIPAddress Then
                    clientStream = x.GetStream()
                    clientStream.BeginWrite(DC_ACK_HEX, 0, DC_ACK_HEX.Length, Nothing, Nothing)
                    RaiseEvent OnEvent(ENTITY_SERVER & kommon.ByteArrayToString(DC_ACK_HEX), Common.TRX.SEND)
                    sent = True
                    Exit For
                End If
            Next
        Catch ex As Exception
            Debug.Print("TestSend " & ex.Message)
        Finally
            clientStream = Nothing
            If sent = False Then
                'ADD TO QUEUE
            End If
        End Try
    End Sub

    Public Sub [Stop]()
        If (_listenThread.IsAlive) Then
            _listenThread.Suspend()
            _listenThread.Abort()
        End If

        If (_tcpListener IsNot Nothing) Then
            _tcpListener.Stop()
            _tcpListener = Nothing
        End If
    End Sub

    Public Sub RequestRoofTopMessage()

    End Sub

    Public Function SendRoofTopMessage(ByVal Message As String, ByVal Color As emu_common.Common.RooftopSignageColor, ByVal Effect As emu_common.Common.RooftopSignageEffect, ByVal Duration As Integer) As String

        Dim MessageID As Byte = MESSAGEID_ROOFTOP_SIGNAGE
        Dim DataBytes(-1) As Byte

        Dim announce() As Byte = System.Text.Encoding.ASCII.GetBytes(Message)
        Dim CLR() As Byte = System.BitConverter.GetBytes(Color)
        Dim FX() As Byte = System.BitConverter.GetBytes(Effect)
        'Dim DUR() As Byte = System.BitConverter.GetBytes(Duration)

        'Array.Reverse(DUR)
        Array.Resize(CLR, 1)
        Array.Resize(FX, 1)

        ReDim Preserve DataBytes(UBound(announce))
        announce.CopyTo(DataBytes, 0)
        ReDim Preserve DataBytes(UBound(DataBytes) + CLR.Length)
        CLR.CopyTo(DataBytes, UBound(DataBytes))

        ReDim Preserve DataBytes(UBound(DataBytes) + FX.Length)
        FX.CopyTo(DataBytes, UBound(DataBytes))

        'ReDim Preserve DataBytes(UBound(DataBytes) + DUR.Length)
        'DUR.CopyTo(DataBytes, UBound(DataBytes) - UBound(DUR))

        Dim length() As Byte = System.BitConverter.GetBytes(DataBytes.Length)
        Array.Resize(length, 1)

        Dim CS As Byte = kommon.GetCheckSum(DataBytes)
        ReDim Preserve DataBytes(UBound(DataBytes) + 1)
        DataBytes(UBound(DataBytes)) = CS

        Dim newData(0) As Byte
        newData(0) = SOF
        ReDim Preserve newData(UBound(newData) + 1)
        newData(UBound(newData)) = MESSAGEID_ROOFTOP_SIGNAGE

        ReDim Preserve newData(UBound(newData) + length.Length)
        length.CopyTo(newData, UBound(newData) - UBound(length))

        ReDim Preserve newData(UBound(newData) + DataBytes.Length)
        DataBytes.CopyTo(newData, UBound(newData) - UBound(DataBytes))

        ReDim Preserve newData(UBound(newData) + 1)
        newData(UBound(newData)) = EOF

        Return kommon.ByteArrayToString(newData)

    End Function
    '
    'MESSAGE FRAME: SOF|MESSAGE ID|MESSAGE CONTENT|CS|EOF
    '
    '
    'MESSAGE CONTENT
    '===============
    'MESSAGE HEADER
    '--------------
    'Command Length
    'TrakerID
    'Timestamp
    'Message Serial No
    '
    'DATA FIELD
    '----------
    '
    '
    Public Function SendAcknowledge(ByVal Serial As String, ByVal dOption As emu_common.Common.Acknowledgement)
        Dim MessageID As Byte = MESSAGEID_SERVER_ACKNOWLEDGEMENT
        Dim DataBytes(-1) As Byte

        Dim SerialNo() As Byte = System.Text.Encoding.ASCII.GetBytes(Serial)

        'ReDim Preserve DataBytes(UBound(DataBytes) + 1)
        'DataBytes(UBound(DataBytes)) = MessageID

        ReDim Preserve DataBytes(UBound(DataBytes) + SerialNo.Length)
        'SerialNo.CopyTo(DataBytes, 1)
        SerialNo.CopyTo(DataBytes, 0)

        GetMessageFrame(DataBytes, MessageID)

        Return kommon.ByteArrayToString(DataBytes)
    End Function

#End Region



End Class