﻿Module ModMain

    '==================================================================
    Public Const ENTITY_OBU As String = "OnBoard Unit> "
    Public Const ENTITY_RSC As String = "Rooftop Signage> "
    Public Const ENTITY_RSM As String = "RearSeat Monitor> "
    Public Const ENTITY_DC As String = "Driver Console> "
    Public Const ENTITY_TM As String = "Taxi Meter> "
    Public Const ENTITY_CT As String = "Cashless Terminal> "
    Public Const ENTITY_UFO As String = "Unidentified> "
    '==================================================================

    Public Const DEFAULT_DATESTRING As String = "yyyy-MM-dd HH:mm:ss"

    Public SELECTED_CONNECTION As String = String.Empty
    Public APPLICATION_NAME As String = String.Empty

    Public Const DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss"
    Public dec0F As Integer = Convert.ToInt32("0x0F", 16)
    Public dec1F As Integer = Convert.ToInt32("0x1F", 16)
    Public dec3F As Integer = Convert.ToInt32("0x3F", 16)

    Public STX As Byte = &H2
    Public ETX As Byte = &H3
    Public ACK As Byte = &H6
    Public SEP As Byte = &H1C

    Public EOT As Short = &HA3

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

    Public PRESERVED_ADHOC() As Byte = {&HA2, &H82, &H9, &H53, &H4F, &H53, &H0, &H1, &H0, &H0, &H0, &H3C, &H72, &HA3}
    Public RSC_DATA As New emu_common.RooftopSignatureControl

    Public OBU_IP As String = "127.0.0.1"

    Public Const DATA_DELIMITER As String = "|"

    Public Sub Initialization()
        APPLICATION_NAME = IIf(Configuration.ConfigurationManager.AppSettings("APPNAME") = "", "", Configuration.ConfigurationManager.AppSettings("APPNAME"))
        If Not Configuration.ConfigurationManager.AppSettings("OBU.IP") = "" Then OBU_IP = Configuration.ConfigurationManager.AppSettings("OBU.IP")

    End Sub

End Module
