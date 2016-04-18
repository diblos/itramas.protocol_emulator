Module ModMain

    Public Const ENTITY_OBU As String = "OnBoard Unit> "
    Public Const ENTITY_SERVER As String = "Server> "

    Public ClientGroup As New DataTable

    Public Const DEFAULT_DATESTRING As String = "yyyy-MM-dd HH:mm:ss"

    Public Const DELIMITOR_COLON As String = ":"

    Public SELECTED_CONNECTION As String = String.Empty
    Public APPLICATION_NAME As String = String.Empty

    Public DEFAULT_DRIVER_NAME As String = "Mr Taxi"
    Public DEFAULT_DRIVER_PASSWORD As String = "1234567890"
    Public DEFAULT_TRACKERID As String = "NR09G03514"
    Public DEFAULT_PLATENO As String = "WMU232C"

    Public Const DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss"
    Public dec0F As Integer = Convert.ToInt32("0x0F", 16)
    Public dec1F As Integer = Convert.ToInt32("0x1F", 16)
    Public dec3F As Integer = Convert.ToInt32("0x3F", 16)

    Public SOF As Short = &HA2
    Public EOF As Short = &HA3
    Public SEPARATOR As Short = &HA0

    'MESSAGE ID TABLE
    Public Const MESSAGEID_OBU_CONNECT As Short = &H1
    Public Const MESSAGEID_OBU_PING As Short = &H2
    Public Const MESSAGEID_UPLOAD_FIRMWARE_VERSION As Short = &H3
    Public Const MESSAGEID_UPLOAD_PARAMETER As Short = &H4
    Public Const MESSAGEID_DEVICE_STATUS As Short = &H5
    Public Const MESSAGEID_UPLOAD_LOG As Short = &H6
    Public Const MESSAGEID_UPGRADE_STATUS As Short = &H7
    Public Const MESSAGEID_OBU_REPLY As Short = &H8
    Public Const MESSAGEID_UPLOAD_NETWORK_PARAMETER_PHONE_NUMBER As Short = &H9
    Public Const MESSAGEID_SERVER_REPLY As Short = &H81
    Public Const MESSAGEID_SERVER_CONNECT As Short = &H82
    Public Const MESSAGEID_SET_PARAMETER As Short = &H83
    Public Const MESSAGEID_READ_PARAMETER As Short = &H84
    Public Const MESSAGEID_READ_DEVICE_STATUS As Short = &H85
    Public Const MESSAGEID_READ_LOG As Short = &H86
    Public Const MESSAGEID_FIRMWARE_UPGRADE As Short = &H87
    Public Const MESSAGEID_OBU_RESET As Short = &H88
    Public Const MESSAGEID_READ_NETWORK_PARAMETER_PHONE_NUMBER As Short = &H89

    Public Const CONTENT_NULL As Short = &H0

    'MESSAGE ID
    Public Const MESSAGEID_ROOFTOP_SIGNAGE As Short = &H21
    Public Const MESSAGEID_REQUEST_ROOFTOP_SIGNAGE_CONTENT As Short = &H22
    Public Const MESSAGEID_SERVER_ACKNOWLEDGEMENT As Short = &H81

    Public RESPONSE_TIME As Integer = 500

    Public DC_TAXI_VALIDATE_REQ() As Byte = {&HA2, &H44, &H0, &H1, &HA3}
    Public DC_ACK_HEX() As Byte = {&HA2, &H90, &H1, &H1, &HA3} 'A2 90 01 01 A3 [90-ACK]
    Public DC_NACK_HEX() As Byte = {&HA2, &H90, &H0, &H0, &HA3} 'A2 90 00 00 A3 [90-NACK]

    Public COM_ACK_HEX() As Byte = {&HA2, &H90, &H1, &H1, &H1, &HA3} 'A2 90 01 01 01 A3 [90-ACK]
    Public COM_NACK_HEX() As Byte = {&HA2, &H90, &H1, &H0, &H0, &HA3} 'A2 90 01 00 00 A3 [90-NACK]

    Public Const START_OF_LOG_INDEX As Integer = 23

    '54,45,4B,53,49,4D
    '54,45,4B,53,31,4D
    Public PRESERVED_ADHOC() As Byte = {&HA2, &H82, &H9, &H54, &H45, &H4B, &H53, &H31, &H4D, &H1, &H1, &H3C, &H72, &HA3}

    Public SERVER_PORT As Integer = 60000

    Public ftpServ As String
    Public UID As String
    Public PWD As String
    Public DUMMY As String

    Public FTP_DRIVER_INFO_FILE As String = "dummy.txt"
    Public FTP_CONTENT_FILE As String = "dummy.txt"

    Public OBU_IP As String = "127.0.0.1"
    Public DRIVER_CONSOLE_PORT As Integer = 9001
    Public REAR_SEAT_MONITOR_PORT As Integer = 9002

    Public Enum IncomingStatus
        Inserted
        Updated
        Ignored
    End Enum

    Public Sub Initialization()
        APPLICATION_NAME = IIf(Configuration.ConfigurationManager.AppSettings("APPNAME") = "", "", Configuration.ConfigurationManager.AppSettings("APPNAME"))
        If Not Configuration.ConfigurationManager.AppSettings("FTP.SERVER") = "" Then ftpServ = Configuration.ConfigurationManager.AppSettings("FTP.SERVER")
        If Not Configuration.ConfigurationManager.AppSettings("FTP.UID") = "" Then UID = Configuration.ConfigurationManager.AppSettings("FTP.UID")
        If Not Configuration.ConfigurationManager.AppSettings("FTP.PWD") = "" Then PWD = Configuration.ConfigurationManager.AppSettings("FTP.PWD")
        If Not Configuration.ConfigurationManager.AppSettings("FTP.DUMMY.FILE") = "" Then DUMMY = Configuration.ConfigurationManager.AppSettings("FTP.DUMMY.FILE")
        If Not Configuration.ConfigurationManager.AppSettings("OBU.IP") = "" Then OBU_IP = Configuration.ConfigurationManager.AppSettings("OBU.IP")
        If Not Configuration.ConfigurationManager.AppSettings("DRIVER.CONSOLE.PORT") = "" Then DRIVER_CONSOLE_PORT = Configuration.ConfigurationManager.AppSettings("DRIVER.CONSOLE.PORT")
        If Not Configuration.ConfigurationManager.AppSettings("REAR.SEAT.MONITOR.PORT") = "" Then REAR_SEAT_MONITOR_PORT = Configuration.ConfigurationManager.AppSettings("REAR.SEAT.MONITOR.PORT")

        If Not Configuration.ConfigurationManager.AppSettings("SERVER.PORT") = "" Then SERVER_PORT = Configuration.ConfigurationManager.AppSettings("SERVER.PORT")
        FTP_DRIVER_INFO_FILE = DUMMY
        FTP_CONTENT_FILE = DUMMY

        ClientGroup = initiateClientTable()

    End Sub

    Private Function initiateClientTable() As DataTable
        Dim tmpTable As New DataTable
        Dim col As DataColumn

        'col = New DataColumn()
        'col.DataType = System.Type.GetType("system.int64")
        'col.ColumnName = "joblist"
        'col.DefaultValue = 0
        'col.Unique = False
        'tmpTable.Columns.Add(col)

        col = New DataColumn()
        col.DataType = System.Type.GetType("System.String")
        col.ColumnName = "IPAddress"
        col.DefaultValue = ""
        col.Unique = False
        tmpTable.Columns.Add(col)

        col = New DataColumn()
        col.DataType = System.Type.GetType("System.String")
        col.ColumnName = "TrackerID"
        col.DefaultValue = ""
        col.Unique = False
        tmpTable.Columns.Add(col)

        col = New DataColumn()
        col.DataType = System.Type.GetType("System.DateTime")
        col.ColumnName = "Timestamp"
        col.DefaultValue = Now
        col.Unique = False
        tmpTable.Columns.Add(col)

        Return tmpTable

    End Function

    Public Function ConvertHexToASC(ByVal hexData As String) As String
        Try
            ConvertHexToASC = Convert.ToChar(System.Convert.ToUInt32(hexData, 16)).ToString
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function ValidAlphanumeric(ByVal value As String) As Boolean
        ValidAlphanumeric = True
        Static validator As New System.Text.RegularExpressions.Regex("^[a-zA-Z0-9 ,-]*$")
        If Not String.IsNullOrEmpty(value) Then
            If Not validator.IsMatch(value) Then
                Return False
            End If
        End If
    End Function

End Module
