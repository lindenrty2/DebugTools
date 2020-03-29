Imports System.Data
Imports MySql.Data.MySqlClient
Imports DebugTools.Interface.DataBase
Imports DebugTools.Interface.Package

Public Class MySQLDataAccessor
    Implements IDataAccessor

    Public ReadOnly Property DisplayName As String Implements IDataAccessor.DisplayName
        Get
            Return _dbInfo.DisplayName
        End Get
    End Property

    Private _sqlStyle As New MySQLStyle()
    Private _dbInfo As DBConnectInfo
    Private _dbconnect As MySqlConnection
    Private _packageExporter As IPackageExporter
    Private _tableInfoCacheList As New Dictionary(Of String, TableInfo)

    Public Shared Function CreateConnectString(connectInfo As DBConnectInfo) As String
        Return String.Format("server={0};database={1};port=3306;uid={2};pwd={3};charset='utf8'", connectInfo.Host, connectInfo.DataBaseName, connectInfo.UserName, connectInfo.Password)
    End Function

    Public Sub New(connectInfo As DBConnectInfo)
        _dbInfo = connectInfo
    End Sub

    Public Function Connect() As Boolean Implements IDataAccessor.Connect
        Try
            _dbconnect = New MySqlConnection(CreateConnectString(_dbInfo))
            _dbconnect.Open()
            Return True
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return False
        End Try
    End Function

    Public Function Close() As Boolean Implements IDataAccessor.Close
        If _dbconnect Is Nothing Then Return True
        Try
            _dbconnect.Close()
            Return True
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return False
        End Try
    End Function

    Public Function GetTables(Optional dbName As String = Nothing) As ITableList Implements IDataAccessor.GetTables
        Dim sql As String = "SELECT TABLE_SCHEMA AS DBNAME, TABLE_NAME AS NAME,TABLE_COMMENT AS COMMENT,0 AS ID FROM information_schema.TABLES "
        If dbName IsNot Nothing Then
            sql += "WHERE TABLE_SCHEMA = '" + dbName + "'"
        End If

        Dim dataset As DataSet = ExecDataSet(sql)
        If dataset Is Nothing OrElse dataset.Tables.Count = 0 Then Return New TableList(Me, New DataTable)

        dataset.Tables(0).CaseSensitive = False
        Return New TableList(Me, dataset.Tables(0))
    End Function

    Public Function GetTableInfo(dbName As String, tableName As String) As ITableInfo Implements IDataAccessor.GetTableInfo
        If _tableInfoCacheList.ContainsKey(tableName) Then
            Return _tableInfoCacheList(tableName)
        End If
        Dim sql As String = String.Format("SELECT TABLE_SCHEMA AS DBNAME,TABLE_NAME AS NAME,TABLE_COMMENT AS COMMENT,0 AS ID FROM information_schema.TABLES WHERE TABLE_SCHEMA = '{0}' AND TABLE_NAME = '{1}'", dbName, tableName)

        Dim dataset As DataSet = ExecDataSet(sql)
        If dataset Is Nothing OrElse dataset.Tables.Count = 0 Then Return Nothing

        Dim table As DataTable = dataset.Tables(0)
        If table.Rows.Count = 0 Then Return Nothing
        Return CreateTableInfo(table.Rows(0))
    End Function

    Public Function CreateTableInfo(row As DataRow) As TableInfo
        Dim tableName As String = TableInfo.GetTableName(row)
        If _tableInfoCacheList.ContainsKey(tableName) Then
            Return _tableInfoCacheList(tableName)
        End If
        Dim ti As New TableInfo(Me, row)
        Dim path As String = PathHelper.GetCustomInfoPath(InfoType.TableInfo, ti.DBName + "." + ti.Name)
        Dim xc As Xml.XmlNode = Nothing
        Try
            If IO.File.Exists(path) Then
                Dim xd As New Xml.XmlDocument
                xd.Load(path)
                xc = xd.SelectSingleNode("/xml/Info")
                ti.Load(xc)
            End If
            _tableInfoCacheList.Add(tableName, ti)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Table custominfo loading fail")
        End Try
        Return ti
    End Function

    Public Sub ClearCache() Implements IDataAccessor.ClearCache
        _tableInfoCacheList.Clear()
    End Sub

    Public Function GetColumns(tableInfo As ITableInfo) As IEnumerable(Of IColumnInfo) Implements IDataAccessor.GetColumns
        Dim sql As String = String.Format("select `COLUMN_NAME` AS `NAME`,`COLUMN_COMMENT` AS `COMMENT`, 0 AS TABLE_ID,0 AS `COLUMN_ID`,`DATA_TYPE` AS `TYPENAME`,`CHARACTER_MAXIMUM_LENGTH` AS MAX_LENGTH,`NUMERIC_PRECISION` AS `PRECISION` ,`NUMERIC_SCALE` AS `SCALE`,IF(`COLUMN_KEY` IS NULL,0,1) AS `KEYNO` from information_schema.COLUMNS where table_name = '{0}' and table_schema = '{1}'", tableInfo.Name, tableInfo.DBName)

        Dim dataset As DataSet = ExecDataSet(sql)
        If dataset Is Nothing OrElse dataset.Tables.Count = 0 Then Return New List(Of IColumnInfo)

        Dim path As String = PathHelper.GetCustomInfoPath(InfoType.TableInfo, tableInfo.DBName + "." + tableInfo.Name)
        Try
            Dim xc As Xml.XmlNode = Nothing
            If IO.File.Exists(path) Then
                Dim xd As New Xml.XmlDocument
                xd.Load(path)
                xc = xd.SelectSingleNode("/xml/Columns")
            End If
            Dim columns As New List(Of IColumnInfo)
            For Each row As DataRow In dataset.Tables(0).Rows
                Dim columnInfo As New ColumnInfo(tableInfo, row)
                If xc IsNot Nothing Then
                    For Each columnNode As Xml.XmlNode In xc.ChildNodes
                        If XmlHelper.GetAttributeValue(columnNode, "Name") = columnInfo.Name Then
                            columnInfo.SetCustomInfo(columnNode)
                            Exit For
                        End If
                    Next
                End If
                columns.Add(columnInfo)
            Next
            Return columns
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Columns custominfo loading fail")
            Return Nothing
        End Try
    End Function

    Public Function GetTableData(tableInfo As ITableInfo, count As Integer, where As ISqlCondition) As DataTable Implements IDataAccessor.GetTableData
        Dim sql As String = "SELECT "
        sql += " * FROM " + _sqlStyle.ModifierName(tableInfo.DBName, tableInfo.Name)
        Dim whereStr As String = ""
        Dim sortStr As String = ""
        If where IsNot Nothing Then
            whereStr = where.ToSQLConditionString(_sqlStyle)
            sortStr = where.ToSQLSortString(_sqlStyle)
        End If
        If Not String.IsNullOrWhiteSpace(whereStr) Then
            sql += " WHERE " + whereStr
        End If
        If Not String.IsNullOrWhiteSpace(sortStr) Then
            sql += " ORDER BY " + sortStr
        End If
        If count >= 0 Then
            sql += " LIMIT 0," + count.ToString + " "
        End If
        Dim dataset As DataSet = ExecDataSet(sql)
        Dim table As DataTable
        If dataset Is Nothing OrElse dataset.Tables.Count = 0 Then
            table = New DataTable()
        Else
            table = dataset.Tables(0)
        End If
        table.TableName = tableInfo.Name
        Return table
    End Function

    Public Function GetTableRelation(tableInfo As ITableInfo) As IEnumerable(Of ITableRelationInfo) Implements IDataAccessor.GetTableRelation
        Dim sql As String = String.Format("SELECT FKDBNAME,FKNAME,SOURCEDBNAME,SOURCENAME,TARGETDBNAME,TARGETNAME,0 AS FKID FROM( SELECT `CONSTRAINT_SCHEMA` AS FKDBNAME, `CONSTRAINT_NAME` AS FKNAME, `TABLE_SCHEMA` AS SOURCEDBNAME, `TABLE_NAME` AS SOURCENAME,`REFERENCED_TABLE_SCHEMA` AS TARGETDBNAME, `REFERENCED_TABLE_NAME` AS TARGETNAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where ((TABLE_NAME='{0}' AND TABLE_SCHEMA = '{1}') OR (REFERENCED_TABLE_NAME='{0}' AND REFERENCED_TABLE_SCHEMA = '{1}')) AND CONSTRAINT_NAME <> 'PRIMARY') AS C GROUP BY FKNAME,SOURCENAME,TARGETNAME", tableInfo.Name, tableInfo.DBName)
        Dim dataset As DataSet = ExecDataSet(sql)
        Dim tableRelations As New List(Of ITableRelationInfo)
        If dataset Is Nothing OrElse dataset.Tables.Count = 0 Then Return tableRelations

        Dim path As String = PathHelper.GetCustomInfoPath(InfoType.TableInfo, tableInfo.DBName + "." + tableInfo.Name)
        Try
            Dim xc As Xml.XmlNode = Nothing
            If IO.File.Exists(path) Then
                Dim xd As New Xml.XmlDocument
                xd.Load(path)
                xc = xd.SelectSingleNode("/xml/Relations")
            End If

            For Each row As DataRow In dataset.Tables(0).Rows
                Dim tableRelation As New TableRelationInfo(Me, row)
                If xc IsNot Nothing Then
                    For Each relationNode As Xml.XmlNode In xc.ChildNodes
                        If XmlHelper.GetAttributeValue(relationNode, "Name") = tableRelation.FKName Then
                            tableRelation.SetCustomInfo(relationNode)
                            Exit For
                        End If
                    Next
                End If
                tableRelations.Add(tableRelation)
            Next
            If xc IsNot Nothing Then
                For Each relationNode As Xml.XmlNode In xc.ChildNodes
                    If XmlHelper.GetBooleanAttributeValue(relationNode, "IsCustom", False) Then
                        tableRelations.Add(New CustomTableRelation(Me, relationNode))
                    End If
                Next
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Table custominfo loading fail")
        End Try

        Return tableRelations
    End Function

    Public Function GetFKInfo(relation As ITableRelationInfo) As IEnumerable(Of ITableRelationItemInfo) Implements IDataAccessor.GetFKInfo
        Dim sql As String = String.Format("select `COLUMN_NAME` AS SOURCENAME,`REFERENCED_COLUMN_NAME` AS TARGETNAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where `CONSTRAINT_NAME`='{0}' AND TABLE_SCHEMA = '{1}'", relation.FKName, relation.FKDBName)
        Dim dataset As DataSet = ExecDataSet(sql)
        Dim tableRelationItems As New List(Of ITableRelationItemInfo)
        If dataset Is Nothing OrElse dataset.Tables.Count = 0 Then Return tableRelationItems
        For Each row As DataRow In dataset.Tables(0).Rows
            tableRelationItems.Add(New TableRelationItem(relation, row))
        Next
        Return tableRelationItems
    End Function

    Private Function ExecDataSet(sql As String) As DataSet Implements IDataAccessor.ExecDataSet
        Dim command As New MySqlCommand(sql, _dbconnect)
        Dim adapter As New MySqlDataAdapter(command)
        Dim dataset As New DataSet()
        dataset.CaseSensitive = True
        Try
            Console.WriteLine("【SQL実行】" + sql)
            adapter.Fill(dataset)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "データ取得失敗")
        End Try
        Return dataset
    End Function

    Public Function GetExporter() As IPackageExporter Implements IDataAccessor.GetExporter
        If _packageExporter Is Nothing Then
            _packageExporter = New PackageExporter(Me)
        End If
        Return _packageExporter
    End Function

    Public Class TableList
        Implements ITableList

        Private _dataTable As DataTable
        Public ReadOnly Property BindingSource As Object Implements ITableList.Source
            Get
                Return _dataTable
            End Get
        End Property

        Private _dataAccessor As MySQLDataAccessor
        Public ReadOnly Property DataAccessor As IDataAccessor Implements ITableList.DataAccessor
            Get
                Return _dataAccessor
            End Get
        End Property

        Public Sub New(dataAccessor As MySQLDataAccessor, dataTable As DataTable)
            _dataAccessor = dataAccessor
            _dataTable = dataTable
        End Sub

        Public Function Filter(key As String) As Object Implements ITableList.Filter
            Dim rows() As DataRow = _dataTable.Select(String.Format("NAME like '%{0}%' OR COMMENT like '%{0}%'", key.Replace("'", "''")))
            Dim dt As New DataTable
            For i As Integer = 0 To _dataTable.Columns.Count - 1
                Dim targetColumn = _dataTable.Columns(i)
                dt.Columns.Add(New DataColumn(targetColumn.ColumnName, targetColumn.DataType))
            Next
            For Each row In rows
                dt.ImportRow(row)
            Next
            Return dt
        End Function

        Public Function GetTableInfo(dbName As String, name As String) As ITableInfo Implements ITableList.GetTableInfo
            Dim rows As IEnumerable(Of DataRow) = _dataTable.Select(String.Format("DBName='{0}' AND Name='{1}'", dbName, name))
            If Not rows.Any() Then Return Nothing
            Return _dataAccessor.CreateTableInfo(rows.First)
        End Function
    End Class

End Class

Public Class MySQLStyle
    Implements ISqlStyle

    Public Function ModifierName(name As String) As String Implements ISqlStyle.ModifierName
        Return "`" + name + "`"
    End Function

    Public Function ModifierName(dbName As String, tableName As String) As String Implements ISqlStyle.ModifierName
        Return ModifierName(dbName, tableName, Nothing)
    End Function

    Public Function ModifierName(dbName As String, tableName As String, columnName As String) As String Implements ISqlStyle.ModifierName
        Dim results As New List(Of String)
        If Not String.IsNullOrWhiteSpace(dbName) Then
            results.Add(ModifierName(dbName))
        End If
        If Not String.IsNullOrWhiteSpace(tableName) Then
            results.Add(ModifierName(tableName))
        End If
        If Not String.IsNullOrWhiteSpace(columnName) Then
            results.Add(ModifierName(columnName))
        End If
        Return String.Join(".", results)
    End Function

End Class
