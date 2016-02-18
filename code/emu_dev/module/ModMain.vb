Module ModMain

    '==================================================================
    Public Const ENTITY_OBU As String = "OnBoard Unit> "
    Public Const ENTITY_RSC As String = "Rooftop Signage> "
    Public Const ENTITY_RSM As String = "RearSeat Monitor> "
    Public Const ENTITY_DC As String = "Driver Console> "
    '==================================================================

    Public Const DEFAULT_DATESTRING As String = "yyyy-MM-dd HH:mm:ss"

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

    Public STX As Short = &HA2
    Public EOT As Short = &HA3
    Public SEPARATOR As Short = &HA0

    'ROOF TOP SIGNAGE COMMANDS
    Public COMMAND_ACK As Short = &H90
    Public COMMAND_START_ADHOC As Short = &H82
    Public COMMAND_END_ADHOC As Short = &H85
    Public COMMAND_READ As Short = &H84

    Public RESPONSE_TIME As Integer = 500

    'DRIVER CONSOLE COMMANDS
    Public COMMAND_DRV_LOGIN_DC As Short = &H41
    Public COMMAND_SOS_DC As Short = &H42
    Public COMMAND_UPDATE_PROFILE_DC As Short = &H43
    Public COMMAND_PROFILE_VALIDATION_DC As Short = &H44
    Public COMMAND_PROFILE_VALIDATION_REPLY_DC As Short = &H45
    Public COMMAND_DRIVER_INFORMATION_UPDATE_REQUEST_DC As Short = &H46

    'REAR SEAT MONITOR COMMANDS
    Public COMMAND_CONTEXT_UPDATE_REQUEST_RSM As Short = &H51
    Public COMMAND_CONTEXT_UPDATE_REQUEST_REPLY_RSM As Short = &H52
    Public COMMAND_SOS_RSM As Short = &H53
    Public COMMAND_DRIVER_INFORMATION_CONTEXT_UPDATE_REQUEST_RSM As Short = &H55

    Public DC_TAXI_VALIDATE_REQ() As Byte = {&HA2, &H44, &H0, &H1, &HA3}
    Public DC_ACK_HEX() As Byte = {&HA2, &H90, &H1, &H1, &HA3} 'A2 90 01 01 A3 [90-ACK]
    Public DC_NACK_HEX() As Byte = {&HA2, &H90, &H0, &H0, &HA3} 'A2 90 00 00 A3 [90-NACK]

    Public COM_ACK_HEX() As Byte = {&HA2, &H90, &H1, &H1, &H1, &HA3} 'A2 90 01 01 01 A3 [90-ACK]
    Public COM_NACK_HEX() As Byte = {&HA2, &H90, &H1, &H0, &H0, &HA3} 'A2 90 01 00 00 A3 [90-NACK]

    Public Const START_OF_LOG_INDEX As Integer = 23

    '54,45,4B,53,49,4D
    '54,45,4B,53,31,4D
    'Public PRESERVED_ADHOC() As Byte = {&HA2, &H82, &H9, &H54, &H45, &H4B, &H53, &H31, &H4D, &H1, &H1, &H3C, &H72, &HA3}
    Public PRESERVED_ADHOC() As Byte = {&HA2, &H82, &H9, &H53, &H4F, &H53, &H0, &H1, &H0, &H0, &H0, &H3C, &H72, &HA3}
    Public RSC_DATA As New emu_common.RooftopSignatureControl

    Public ftpServ As String
    Public UID As String
    Public PWD As String
    Public DUMMY As String

    Public FTP_DRIVER_INFO_FILE As String = "dummy.txt"
    Public FTP_CONTENT_FILE As String = "dummy.txt"

    Public OBU_IP As String = "127.0.0.1"
    Public DRIVER_CONSOLE_PORT As Integer = 9001
    Public REAR_SEAT_MONITOR_PORT As Integer = 9002

    Public Sub Initialization()
        APPLICATION_NAME = IIf(Configuration.ConfigurationManager.AppSettings("APPNAME") = "", "", Configuration.ConfigurationManager.AppSettings("APPNAME"))
        If Not Configuration.ConfigurationManager.AppSettings("FTP.SERVER") = "" Then ftpServ = Configuration.ConfigurationManager.AppSettings("FTP.SERVER")
        If Not Configuration.ConfigurationManager.AppSettings("FTP.UID") = "" Then UID = Configuration.ConfigurationManager.AppSettings("FTP.UID")
        If Not Configuration.ConfigurationManager.AppSettings("FTP.PWD") = "" Then PWD = Configuration.ConfigurationManager.AppSettings("FTP.PWD")
        If Not Configuration.ConfigurationManager.AppSettings("FTP.DUMMY.FILE") = "" Then DUMMY = Configuration.ConfigurationManager.AppSettings("FTP.DUMMY.FILE")
        If Not Configuration.ConfigurationManager.AppSettings("OBU.IP") = "" Then OBU_IP = Configuration.ConfigurationManager.AppSettings("OBU.IP")
        If Not Configuration.ConfigurationManager.AppSettings("DRIVER.CONSOLE.PORT") = "" Then DRIVER_CONSOLE_PORT = Configuration.ConfigurationManager.AppSettings("DRIVER.CONSOLE.PORT")
        If Not Configuration.ConfigurationManager.AppSettings("REAR.SEAT.MONITOR.PORT") = "" Then REAR_SEAT_MONITOR_PORT = Configuration.ConfigurationManager.AppSettings("REAR.SEAT.MONITOR.PORT")
        FTP_DRIVER_INFO_FILE = DUMMY
        FTP_CONTENT_FILE = DUMMY
    End Sub

End Module
