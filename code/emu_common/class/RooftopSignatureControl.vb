Imports System.Reflection

Public Class RooftopSignatureControl
    Dim _color_id As Integer
    Dim _effect_id As Integer
    Dim _duration As Integer
    Dim _message As String

    Public Property ColorID() As Integer
        Get
            Return Me._color_id
        End Get
        Set(ByVal value As Integer)
            Me._color_id = value
        End Set
    End Property

    Public Property EffectID() As Integer
        Get
            Return Me._effect_id
        End Get
        Set(ByVal value As Integer)
            Me._effect_id = value
        End Set
    End Property

    Public Property Duration() As Integer
        Get
            Return Me._duration
        End Get
        Set(ByVal value As Integer)
            Me._duration = value
        End Set
    End Property

    Public Property Message() As String
        Get
            Return Me._message
        End Get
        Set(ByVal value As String)
            Me._message = value
        End Set
    End Property

    Public ReadOnly Property DataCollection() As List(Of Data)
        Get
            Dim tmp As New List(Of Data)
            Dim props() As System.Reflection.PropertyInfo = Me.GetType.GetProperties(BindingFlags.Public Or _
                                                                                     BindingFlags.Instance Or BindingFlags.DeclaredOnly)
            For Each p As System.Reflection.PropertyInfo In props
                If p.Name <> "DataCollection" Then
                    tmp.Add(New Data(p.Name, GetPropertyValue(p.Name)))
                End If
            Next
            Return tmp
        End Get
    End Property

    Private Shared Function GetName(Of T As Class)(ByVal item As T) As String
        Dim properties = GetType(T).GetProperties()
        Return properties(0).Name
    End Function

    Private Function GetPropertyValue(ByVal propertyName As String) As Object

        Dim pi As System.Reflection.PropertyInfo = Me.GetType().GetProperty(propertyName)

        If (Not pi Is Nothing) And pi.CanRead Then
            Return pi.GetValue(Me, Nothing)
        End If

        Dim a As Type() = Nothing
        Dim mi As System.Reflection.MethodInfo = Me.GetType().GetMethod("Get" + propertyName, a)

        If Not mi Is Nothing Then
            Return mi.Invoke(Me, Nothing)
        End If

        Return Nothing
    End Function

End Class

Public Class Data

    Dim _id As String
    Dim _value As Object

    Public Property ID() As String
        Get
            Return Me._id
        End Get
        Set(ByVal value As String)
            Me._id = value
        End Set
    End Property

    Public Property Value() As Object
        Get
            Return Me._value
        End Get
        Set(ByVal value As Object)
            Me._value = value
        End Set
    End Property

    Public Sub New(ByVal ID As String, ByVal Value As Object)
        Me._id = ID
        Me._value = Value
    End Sub

End Class