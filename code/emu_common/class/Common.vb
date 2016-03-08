'===================
'emu_common.common
'===================
Imports System.Net
Imports System.IO

Public Class Common

#Region "Enumerations"
    Public Enum Result
        SUCCESS
        FAIL
        NULL
    End Enum

    Public Enum TRX
        SEND
        RECEIVE
    End Enum

    Public Enum ReceiveEvents
        TIMEOUT
        DTO
        LOG
        [STRING]
        ERRORS
    End Enum

    Public Enum ReplyType
        Acknowledge
        NAcknowledge
        Message
    End Enum

    Public Enum RequestUpdateType
        DriverInformation
        ContextUpdate
    End Enum

    Public Function OBUData2String()
        Return String.Empty
    End Function

    Public Enum Device
        RoofTopSignage
        DriverConsole
        RearSeatMonitor
        TaxiMeter
        CashlessTerminal
    End Enum

    Public Enum RooftopSignageColor
        Red = 0
        Green = 1
    End Enum

    Public Enum RooftopSignageEffect
        [Static] = 0
        [Blink] = 1
        [ScrollSpeed1] = 2
        [ScrollSpeed2] = 3
        [ScrollSpeed3] = 4
        [ScrollSpeed4] = 5
        [ScrollSpeed5] = 6
        [ScrollSpeed6] = 7
        [ScrollSpeed7] = 8
        [ScrollSpeed8] = 9
        [ScrollSpeed9] = 10
        [ScrollSpeed10] = 11
    End Enum

    Public Enum Acknowledgement
        Success = 0
        Fail = 1
        Wrong_Info = 2
        Not_Supported = 3
    End Enum
#End Region

#Region "Methods"

#End Region

End Class