Public Class TimerClass
    Dim _TargetTimeString As String

    Public Enum TimePeriod
        Daily
        Hourly
    End Enum

    Public ReadOnly Property GetInterval() As Integer
        Get
            Return DateDiff(DateInterval.Second, Now, get_Next_ImportDate) * 1000
        End Get
    End Property

    Public ReadOnly Property GetDate() As Date
        Get
            Return get_Next_ImportDate()
        End Get
    End Property

    Public Sub New(ByVal TargetTime As String)
        _TargetTimeString = TargetTime
    End Sub

    Private Function get_Next_ImportDate() As Date
        Dim tmpDateStr As String = Now.ToString("yyyy-MM-dd")
        Dim currHour, targetHour As Integer
        Dim currMinute, targetMinute As Integer
        Dim targetDate As Date = Nothing

        targetHour = CDate(tmpDateStr & " " & _TargetTimeString).Hour
        targetMinute = CDate(tmpDateStr & " " & _TargetTimeString).Minute
        currHour = Now.Hour
        currMinute = Now.Minute

        If currHour >= targetHour AndAlso currMinute >= targetMinute Then
            targetDate = CDate(DateAdd(DateInterval.Day, 1, Now).ToString("yyyy-MM-dd") & " " & _TargetTimeString)
        Else
            targetDate = CDate(tmpDateStr & " " & _TargetTimeString)
        End If

        Return targetDate

    End Function
End Class
