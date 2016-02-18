'Imports Itac.Win.Framework

Friend Class AVL_Raw_DTO
    'Inherits BaseDTO

    Private _receivedDatetime As DateTime
    Private _rawData As String
    Private _trackerId As String

    Public Property RawData() As String
        Get
            Return _rawData
        End Get
        Set(ByVal value As String)
            _rawData = value
        End Set
    End Property

    Public Property TrackerID() As String
        Get
            Return _trackerId
        End Get
        Set(ByVal value As String)
            _trackerId = value
        End Set
    End Property

    Public Property ReceivedDateTime() As DateTime
        Get
            Return _receivedDatetime
        End Get
        Set(ByVal value As DateTime)
            _receivedDatetime = value
        End Set
    End Property
End Class
