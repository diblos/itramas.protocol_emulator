'2.1 AVL/Alarm Data from OBU- √
'2.2 Rooftop Signage Control via OBU- √
'2.3 Taxi Meter/ Cashless Terminal via OBU - √
'2.4 Driver Console Control via OBU - √
'2.5 Rear Seat Monitor Control via OBU – √
'2.6 Driver Behavior Data from OBU

Public Class OBUDataClass

    Private _trackerId As String

    Public Property TrackerId() As String
        Get
            Return _trackerId
        End Get
        Set(ByVal value As String)
            _trackerId = value
        End Set
    End Property

End Class

Public Class RSC
    'A2 90 01 01 A3 [90-ACK]
    'A2 90 00 00 A3 [90-NACK]
    'A2 82 [82-Enter Adhoc Message Mode and Send Adhoc Message],90 from OBU
    'A2 84 01 01 A3 [84-Read Message]
    'A2 85 01 01 A3 [85-Exit Ad-hoc Message Mode]
End Class

Public Class CT

End Class

Public Class TM

End Class

Public Class DCC

End Class

Public Class RSMC

End Class

Public Class DBD

End Class