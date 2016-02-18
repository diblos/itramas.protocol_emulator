Imports System.Net
Imports System.IO

Public Class Common
    Public Enum Device
        RoofTopSignage
        DriverConsole
        RearSeatMonitor
    End Enum

    Public Enum TRX
        SEND
        RECEIVE
    End Enum

    Public Enum ReceiveEvents
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
    'Dim checkSum As Byte = GetCheckSum(hexData)

    Public Function GetCheckSum(ByVal hexData() As Byte) As Byte
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

    Public Function ByteArrayToString(ByVal ba As Byte()) As String
        Dim hex As String = BitConverter.ToString(ba)
        Return hex.Replace("-", " ")
    End Function

    Public Sub UploadFtpFile(ByVal folderName As String, ByVal fileName As String, ByVal FTPServerInfo As FTPServerInfo)

        Dim request As FtpWebRequest
        Try
            Dim absoluteFileName As String = Path.GetFileName(fileName)

            request = TryCast(WebRequest.Create(New Uri(String.Format("ftp://{0}/{1}/{2}", FTPServerInfo.Server, folderName, absoluteFileName))), FtpWebRequest)
            request.Method = WebRequestMethods.Ftp.UploadFile
            request.UseBinary = 1
            request.UsePassive = 1
            request.KeepAlive = False
            request.Credentials = New NetworkCredential(FTPServerInfo.UID, FTPServerInfo.PWD)
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

    Public Sub DownloadFtpFile(ByVal filePath As String, ByVal fileName As String, ByVal FTPServerInfo As FTPServerInfo)
        Dim reqFTP As FtpWebRequest = Nothing
        Dim ftpStream As Stream = Nothing

        Try
            Dim outputStream As New FileStream(Path.Combine(filePath, fileName), FileMode.Create)
            reqFTP = DirectCast(FtpWebRequest.Create(New Uri("ftp://" + FTPServerInfo.Server + "/" + fileName)), FtpWebRequest)
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile
            reqFTP.UseBinary = True
            reqFTP.Credentials = New NetworkCredential(FTPServerInfo.UID, FTPServerInfo.PWD)
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

        If s = EOF Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function ParseData(ByVal _data As String) As avlraw
        Dim splitData As String()
        Dim avl As New avlraw
        Try
            splitData = _data.Split(DELIMITOR_COLON)

            If (splitData Is Nothing) Then
                '_errorId = "ERR0008"
                '_errorMsg = "[Data] is NULL or Empty"
                Return Nothing
            End If

            If (splitData.Length <= 0) Then
                '_errorId = "ERR0009"
                '_errorMsg = "[Data] length is 0"
                Return Nothing
            End If

            avl.Enable = splitData(4)
            avl.Alarm = splitData(5)
            avl.Speed = splitData(6)
            avl.Heading = splitData(8) + splitData(7)
            avl.Longitude = splitData(12) + splitData(11) + splitData(10) + splitData(9)
            avl.Latitude = splitData(16) + splitData(15) + splitData(14) + splitData(13)
            avl.GPSDatetime = splitData(20) + splitData(19) + splitData(18) + splitData(17)
            avl.TrackerID = splitData(21) + splitData(22) + splitData(23) + splitData(24) + _
            splitData(25) + splitData(26) + splitData(27) + splitData(28) + splitData(29) + splitData(30) + splitData(31)

            Return avl
        Catch ex As Exception
            '_errorId = "ERR0010"
            '_errorMsg = ex.Message
            Return Nothing
        End Try
    End Function

    Public Function ShowVersion() As String
        Dim s() As String = Application.ProductVersion.Split(".")
        Return s(0) & "." & s(1) & s(2)
    End Function

End Class

Public Class FTPServerInfo
    Public Server As String
    Public Port As Long
    Public UID As String
    Public PWD As String

    Public Sub New(ByVal server As String, ByVal Port As Long, ByVal UserID As String, ByVal UserPassword As String)
        Me.Server = server
        Me.Port = Port
        Me.UID = UserID
        Me.PWD = UserPassword
    End Sub

End Class

Public Class FTPFileInfo
    Public Filename As String
    Public Filesize As Long
    Public Filecreation As Date
End Class

Public Class avlraw
    Public Enable As String
    Public Alarm As String
    Public Speed As String
    Public Heading As String
    Public Longitude As String
    Public Latitude As String
    Public GPSDatetime As String
    Public TrackerID As String

    Public Function GetTrackerID() As String

        Dim field1 As String
        Dim field2 As String
        Dim field3 As String
        Dim field4 As String
        Dim field5 As String
        Dim field6 As String
        Dim field7 As String
        Dim field8 As String
        Dim field9 As String
        Dim field10 As String

        Try
            field1 = ConvertHexToASC(TrackerID.Substring(0, 2))
            field2 = ConvertHexToASC(TrackerID.Substring(2, 2))
            field3 = ConvertHexToASC(TrackerID.Substring(4, 2))
            field4 = ConvertHexToASC(TrackerID.Substring(6, 2))
            field5 = ConvertHexToASC(TrackerID.Substring(8, 2))
            field6 = ConvertHexToASC(TrackerID.Substring(10, 2))
            field7 = ConvertHexToASC(TrackerID.Substring(12, 2))
            field8 = ConvertHexToASC(TrackerID.Substring(14, 2))
            field9 = ConvertHexToASC(TrackerID.Substring(16, 2))
            field10 = ConvertHexToASC(TrackerID.Substring(18, 2))

            Dim tmp As String = field1 & field2 & field3 & field4 & field5 & field6 & field7 & field8 & field9 & field10

            If ValidAlphanumeric(tmp) Then
                Return tmp
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Debug.Print(ex.Message)
            Return Nothing
        End Try

    End Function

End Class