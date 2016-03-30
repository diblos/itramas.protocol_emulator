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
        OBU
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
    Public Function ConvertDecToHex(ByVal decData As Integer) As String
        Dim tmpHex As String = Hex(decData)
        Try
            Return IIf(tmpHex.Length = 1, "0" & tmpHex, tmpHex)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function ConvertHexToBinary(ByVal hexData As String) As Integer
        Dim decData As Integer = Nothing

        Try
            decData = ConvertHexToDec(hexData)
            Return Convert.ToString(decData, 2)
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Function ConvertHexToDec(ByVal hexData As String) As Integer
        Try
            Return Convert.ToInt32(hexData, 16)
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Function ConvertHexToASC(ByVal hexData As String) As String
        Try
            If hexData = "00" Then
                ConvertHexToASC = ""
            Else
                ConvertHexToASC = Convert.ToChar(System.Convert.ToUInt32(hexData, 16)).ToString
            End If
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Public Function ConvertHexToSingle(ByVal hexValue As String) As Single
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

    Public Function ConvertHexStringToASCII(ByVal hexDataString As String) As String
        Try
            ConvertHexStringToASCII = "" 'Convert.ToChar(System.Convert.ToUInt32(hexData, 16)).ToString
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function BCD_to_Int(ByVal LowW As Int16, ByVal HighW As Int16) As Int32

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
#End Region

End Class