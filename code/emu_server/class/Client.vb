Public Class Client
    Private _trackerId As String
    Private _ip As String
    Private _timestamp As Date
    Private _TcpClient As Net.Sockets.TcpClient

    Public Property TrackerID() As String
        Get
            Return Me._trackerId
        End Get
        Set(ByVal value As String)
            Me._trackerId = value
        End Set
    End Property
    Public Property IPAddress() As String
        Get
            Return Me._ip
        End Get
        Set(ByVal value As String)
            Me._ip = value
        End Set
    End Property
    Public Property Timestamp() As Date
        Get
            Return Me._timestamp
        End Get
        Set(ByVal value As Date)
            Me._timestamp = value
        End Set
    End Property

    Public Sub New(ByVal TrackerID As String, ByVal IPAddress As String, ByVal Timestamp As Date)
        Me._trackerId = TrackerID
        Me._ip = IPAddress
        Me._timestamp = Timestamp
    End Sub
    Public Sub New(ByVal TrackerID As String, ByVal IPAddress As String)
        Me._trackerId = TrackerID
        Me._ip = IPAddress
        Me._timestamp = Now
    End Sub
    Public Sub New(ByVal IPAddress As String)
        Me._trackerId = String.Empty
        Me._ip = IPAddress
        Me._timestamp = Now
    End Sub
End Class
