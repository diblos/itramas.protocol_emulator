Imports System.Net
Imports System.IO

Public Class Common
    Public Enum Device
        RoofTopSignage
        DriverConsole
        RearSeatMonitor
        TaxiMeter
        CashlessTerminal
    End Enum

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
        LOG
        TIMEOUT
        CLR
        DTO
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

    Public Sub UploadFtpFile(ByVal folderName As String, ByVal fileName As String)

        Dim request As FtpWebRequest
        Try
            Dim absoluteFileName As String = Path.GetFileName(fileName)

            request = TryCast(WebRequest.Create(New Uri(String.Format("ftp://{0}/{1}/{2}", ftpServ, folderName, absoluteFileName))), FtpWebRequest)
            request.Method = WebRequestMethods.Ftp.UploadFile
            request.UseBinary = 1
            request.UsePassive = 1
            request.KeepAlive = False
            request.Credentials = New NetworkCredential(UID, PWD)
            request.ConnectionGroupName = "group"

            Using fs As FileStream = File.OpenRead(fileName)
                Dim buffer As Byte() = New Byte(fs.Length - 1) {}
                fs.Read(buffer, 0, buffer.Length)
                fs.Close()
                Dim requestStream As Stream = request.GetRequestStream()
                requestStream.Write(buffer, 0, buffer.Length)
                requestStream.Flush()
                requestStream.Close()
            End Using

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub DownloadFtpFile(ByVal filePath As String, ByVal fileName As String)
        Dim reqFTP As FtpWebRequest = Nothing
        Dim ftpStream As Stream = Nothing

        Try
            Dim outputStream As New FileStream(Path.Combine(filePath, fileName), FileMode.Create)
            reqFTP = DirectCast(FtpWebRequest.Create(New Uri("ftp://" + ftpServ + "/" + fileName)), FtpWebRequest)
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile
            reqFTP.UseBinary = True
            reqFTP.Credentials = New NetworkCredential(UID, PWD)
            Dim response As FtpWebResponse = DirectCast(reqFTP.GetResponse(), FtpWebResponse)
            ftpStream = response.GetResponseStream()
            Dim cl As Long = response.ContentLength
            Dim bufferSize As Integer = 2048
            Dim readCount As Integer
            Dim buffer As Byte() = New Byte(bufferSize - 1) {}

            readCount = ftpStream.Read(buffer, 0, bufferSize)
            While readCount > 0
                outputStream.Write(buffer, 0, readCount)
                readCount = ftpStream.Read(buffer, 0, bufferSize)
            End While

            ftpStream.Close()
            outputStream.Close()
            response.Close()
        Catch ex As Exception
            If ftpStream IsNot Nothing Then
                ftpStream.Close()
                ftpStream.Dispose()
            End If
            Throw New Exception(ex.Message.ToString())
        End Try
    End Sub

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

Class TransactionDTO
    'Field Element 1
    Dim _fieldElement As Object
    'Field Type – 3430 (40-Amount, transaction)
    Dim _fieldType As Object
    'Field Length – 0012
    Dim _fieldData As Object
    'Field Data – 303030303030303030333030 (000000000300 – 300sen or RM3)
    'Field Separator - 1C
    Dim _fieldSeparator As Object

    Public Property FieldElement() As Object
        Get
            Return Me._fieldElement
        End Get
        Set(ByVal value As Object)
            Me._fieldElement = value
        End Set
    End Property
    Public Property FieldType() As Object
        Get
            Return Me._fieldType
        End Get
        Set(ByVal value As Object)
            Me._fieldType = value
        End Set
    End Property
    Public Property FieldData() As Object
        Get
            Return Me._fieldData
        End Get
        Set(ByVal value As Object)
            Me._fieldData = value
        End Set
    End Property
    Public Property FieldSeparator() As Object
        Get
            Return Me._fieldSeparator
        End Get
        Set(ByVal value As Object)
            Me._fieldSeparator = value
        End Set
    End Property
End Class