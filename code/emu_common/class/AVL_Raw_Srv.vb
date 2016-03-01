'Imports Itac.Win.Framework
'Imports Itac.Win.Framework.Common
Imports System.Data.Common

Friend Class AVL_Raw_Srv
    'Inherits BaseSrv
    'Implements IBaseSrv

    Public Const SQL_SELECT_FOR_LIST As String = ""
    Public Const SQL_SELECT_FOR_CARD As String = ""

    Public Function getInitialData() As DataTable
        Dim nCol As DataColumn
        Dim tempData As New DataTable

        nCol = New DataColumn()
        nCol.DataType = System.Type.GetType("System.String")
        nCol.ColumnName = "TrackerID"
        nCol.ReadOnly = False
        nCol.Unique = False
        tempData.Columns.Add(nCol)

        nCol = New DataColumn()
        nCol.DataType = System.Type.GetType("System.DateTime")
        nCol.ColumnName = "GPSDatetime"
        nCol.ReadOnly = False
        nCol.Unique = False
        tempData.Columns.Add(nCol)

        Return tempData
    End Function

#Region "Itac Frameworks Implements"
    'Public Function RetrieveDR(ByVal param As String) As System.Data.IDataReader Implements Itac.Win.Framework.IBaseSrv.RetrieveDR
    '    RetrieveDR = Nothing
    'End Function

    'Public Function RetrieveDS(ByVal param As String) As System.Data.DataSet Implements Itac.Win.Framework.IBaseSrv.RetrieveDS
    '    RetrieveDS = Nothing
    'End Function

    'Public Function RetrieveDTO(ByVal param As String) As Itac.Win.Framework.BaseDTO Implements Itac.Win.Framework.IBaseSrv.RetrieveDTO
    '    RetrieveDTO = Nothing
    'End Function

    'Public Function Insert(ByVal dto As Itac.Win.Framework.BaseDTO) As Integer Implements Itac.Win.Framework.IBaseSrv.Insert
    '    Dim sql As String
    '    Dim dtoTrx As AVL_Raw_DTO
    '    Dim connection As IDbConnection = AVL.CreateConnection
    '    connection.Open()
    '    Dim transaction As IDbTransaction = connection.BeginTransaction
    '    Dim command As DbCommand

    '    Try
    '        sql = "INSERT INTO AVL_Raw (Raw_Data,Received_Datetime,Tracker_ID) VALUES (" & _
    '            "@RawData, @ReceivedDateTime, @TrackerID)"

    '        dtoTrx = DirectCast(dto, AVL_Raw_DTO)
    '        command = AVL.GetSqlStringCommand(sql)
    '        AVL.AddInParameter(command, "@RawData", DbType.String, dtoTrx.RawData)
    '        AVL.AddInParameter(command, "@ReceivedDateTime", DbType.String, dtoTrx.ReceivedDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))
    '        AVL.AddInParameter(command, "@TrackerID", DbType.String, dtoTrx.TrackerID)
    '        AVL.ExecuteNonQuery(command)
    '        transaction.Commit()

    '        Insert = ReturnStatus.CI_OK
    '    Catch ex As Exception
    '        transaction.Rollback()
    '        Insert = ReturnStatus.CI_Fail
    '    Finally
    '        If connection.State = ConnectionState.Open Then
    '            connection.Close()
    '        End If
    '    End Try
    'End Function

    'Public Function Delete(ByVal dto As Itac.Win.Framework.BaseDTO) As Integer Implements Itac.Win.Framework.IBaseSrv.Delete
    '    Delete = ReturnStatus.CI_OK
    'End Function

    'Public Function Update(ByVal dto As Itac.Win.Framework.BaseDTO) As Integer Implements Itac.Win.Framework.IBaseSrv.Update
    '    Update = ReturnStatus.CI_OK
    'End Function
#End Region

End Class
