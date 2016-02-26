Imports System.Net
Imports System.IO

Public Class Common
    Inherits emu_common.Common

    'Dim input As String = "The data from the scale"
    'Dim hexData() As Byte = System.Text.Encoding.ASCII.GetBytes(input)

    '' After calling the function GetCheckSum the variable will 
    '' contain &H30 using your test data
    'Dim checkSum As Byte = _GetCheckSum(hexData)

    Private Function _GetCheckSum(ByVal hexData() As Byte) As Byte
        '' The value to be returned to the caller                                                        
        Dim checkSum As Byte = 0
        '' This is here so that we wait til the second value is being processed
        Dim isFirst As Boolean = True

        '' Calcuate the check sum. Do not process the last three characters and 
        '' the reason for -4 on the first line                                                        
        For i As Integer = 0 To hexData.Length - 4
            '' If checkSum does not already have a value then skip this iteration
            If isFirst Then
                isFirst = False
                checkSum = hexData(i)
                Continue For
            End If
            '' Calculating the checkSum
            checkSum = checkSum Xor hexData(i)
        Next

        Return checkSum

    End Function

    Public Function GetCheckSum(ByVal hexData() As Object) As Byte
        Dim h(UBound(hexData)) As Byte
        Array.Copy(hexData, h, hexData.Length)
        Return BinChkSum(h)
    End Function

    Public Function GetCheckSum(ByVal hexData() As Byte) As Byte
        Return BinChkSum(hexData)
    End Function

    Private Function BinChkSum(ByRef varrData() As Byte) As Byte
        '************************************************************************************
        'Arguments: varrData - data block array
        'Returns : check sum in byte
        'Description : XOR the data block to get the check sum
        '************************************************************************************
        'Revision History:
        '#    Date      By               Modify
        '---  --------  ---------------  ----------
        '1    05/2011   Weng Kit         Create
        '************************************************************************************
        Dim bytChkSum As Byte
        Dim intI As Integer
        'Dim filenum As Integer

        bytChkSum = 0
        '    filenum = FreeFile
        '    Open App.Path & "crc.txt" For Output As #filenum
        For intI = 0 To UBound(varrData)
            '        Print #filenum, , arrdata(intI)
            bytChkSum = bytChkSum Xor varrData(intI)

        Next
        '    Close #filenum
        BinChkSum = bytChkSum

    End Function

    Public Function ByteArrayToString(ByVal ba As Byte()) As String
        Dim hex As String = BitConverter.ToString(ba)
        Return hex.Replace("-", " ")
    End Function

    Public Function TrimBytesArray(ByVal arrByte() As Byte)

        Array.Resize(arrByte, Array.FindIndex(arrByte, AddressOf EndsWithEOT) + 1)

        Return arrByte
    End Function

    Private Function EndsWithEOT(ByVal s As Byte) As Boolean

        If s = EOT Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function ShowVersion() As String
        Dim s() As String = Application.ProductVersion.Split(".")
        Return s(0) & "." & s(1) & s(2)
    End Function

End Class

Public Class FTPFileInfo
    Public Filename As String
    Public Filesize As Long
    Public Filecreation As Date
End Class