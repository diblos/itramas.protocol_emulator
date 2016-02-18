Public Class Message
    Private _recipientId As String
    Private _msg() As Byte
    Private _timestamp As Date

    Public Property ReceipientID() As String
        Get
            Return Me._recipientId
        End Get
        Set(ByVal value As String)
            Me._recipientId = value
        End Set
    End Property

    Public Property Message() As Byte()
        Get
            Return Me._msg
        End Get
        Set(ByVal value As Byte())
            Me._msg = value
        End Set
    End Property

    Public Sub New(ByVal ID As String, ByVal msg() As Byte)
        Me._recipientId = ID
        Me._msg = msg
    End Sub
End Class
