using DebugTools.DataBase;
using DebugTools.Helper;
using DebugTools.Package;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace DebugTools.Common.DBSupport.SQLServer
{
    public class SQLServerAccessor : IDataAccessor
    {
        public string DisplayName
        {
            get
            {
                return _dbInfo.DisplayName;
            }
        }

        private SQLServerStyle _sqlStyle = new SQLServerStyle();
        private DBConnectInfo _dbInfo;
        private SqlConnection _dbconnect;
        //private IPackageExporter _packageExporter;
        private Dictionary<string, ITableInfo> _tableInfoCacheList = new Dictionary<string, ITableInfo>();

        public static string CreateConnectString(DBConnectInfo connectInfo)
        {
            return string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", connectInfo.Host, connectInfo.DataBaseName, connectInfo.UserName, connectInfo.Password);
        }

        public SQLServerAccessor(DBConnectInfo connectInfo)
        {
            _dbInfo = connectInfo;
        }

        public bool Connect()
        {
            try
            {
                _dbconnect = new SqlConnection(CreateConnectString(_dbInfo));
                _dbconnect.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool Close()
        {
            if (_dbconnect == null)
                return true;
            try
            {
                _dbconnect.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public ITableList GetTables(string dbName = null)
        {
            string sql = "SELECT '" + dbName + "' AS DBNAME,T.NAME,EP.VALUE AS COMMENT,T.OBJECT_ID AS ID FROM SYS.TABLES AS T LEFT JOIN SYS.EXTENDED_PROPERTIES AS EP ON T.OBJECT_ID = EP.MAJOR_ID AND EP.name='MS_Description' AND EP.MINOR_ID=0 ORDER BY T.NAME";

            DataSet dataset = ExecDataSet(sql);
            if (dataset == null || dataset.Tables.Count == 0)
                return new TableList(this, new DataTable());

            dataset.Tables[0].CaseSensitive = false;
            return new TableList(this, dataset.Tables[0]);
        }

        public ITableInfo GetTableInfo(string dbName, string tableName)
        {
            if (_tableInfoCacheList.ContainsKey(tableName))
                return _tableInfoCacheList[tableName];
            string sql = string.Format("SELECT '" + dbName + "' AS DBNAME,T.NAME,EP.VALUE AS COMMENT,T.OBJECT_ID AS ID FROM SYS.TABLES AS T LEFT JOIN SYS.EXTENDED_PROPERTIES AS EP ON T.OBJECT_ID = EP.MAJOR_ID AND EP.name='MS_Description' AND EP.MINOR_ID=0 AND T.NAME='{0}' ORDER BY T.NAME", tableName);

            DataSet dataset = ExecDataSet(sql);
            if (dataset == null || dataset.Tables.Count == 0)
                return null;

            DataTable table = dataset.Tables[0];
            if (table.Rows.Count == 0)
                return null;
            return CreateTableInfo(table.Rows[0]);
        }

        public ITableInfo CreateTableInfo(DataRow row)
        {
            string tableName = TableInfo.GetTableName(row);
            if (_tableInfoCacheList.ContainsKey(tableName))
                return _tableInfoCacheList[tableName];
            TableInfo ti = new TableInfo(this, row); 
            string path = SystemCenter.CurrentApplication.PathManager.GetSettingPath("Table", ti.DBName + "." + ti.Name);
            System.Xml.XmlNode xc = null;
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
                    xd.Load(path);
                    xc = xd.SelectSingleNode("/xml/Info");
                    ti.Load(xc);
                }
                _tableInfoCacheList.Add(tableName, ti);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Table custominfo loading fail");
            }
            return ti;
        }

        public void ClearCache()
        {
            _tableInfoCacheList.Clear();
        }

        public IEnumerable<IColumnInfo> GetColumns(ITableInfo tableInfo)
        {
            string sql = string.Format("SELECT COL.*,PKCOL.KEYNO FROM (SELECT T.NAME,EP.VALUE AS COMMENT,T.OBJECT_ID AS TABLE_ID,T.COLUMN_ID,TP.name AS TYPENAME,T.MAX_LENGTH,T.PRECISION,T.SCALE FROM SYS.COLUMNS AS T LEFT JOIN SYS.EXTENDED_PROPERTIES AS EP ON T.OBJECT_ID = EP.MAJOR_ID AND EP.CLASS=1 AND EP.name='MS_Description' AND T.COLUMN_ID=EP.MINOR_ID LEFT JOIN sys.types as TP ON T.user_type_id = TP.system_type_id AND T.system_type_id = TP.user_type_id WHERE OBJECT_ID={0}) AS COL LEFT JOIN (SELECT SYSCOLUMNS.ID,SYSCOLUMNS.COLID,SYSINDEXKEYS.KEYNO FROM SYSCOLUMNS,SYSOBJECTS,SYSINDEXES,SYSINDEXKEYS WHERE SYSCOLUMNS.id={0} AND SYSOBJECTS.xtype = 'PK' AND SYSOBJECTS.parent_obj = SYSCOLUMNS.id AND SYSINDEXES.id = SYSCOLUMNS.id AND SYSOBJECTS.name = SYSINDEXES.name AND SYSINDEXKEYS.id = SYSCOLUMNS.id AND SYSINDEXKEYS.indid = SYSINDEXES.indid AND SYSCOLUMNS.colid = SYSINDEXKEYS.colid) AS PKCOL ON COL.TABLE_ID = PKCOL.ID AND COL.COLUMN_ID = PKCOL.COLID", tableInfo.Id);

            DataSet dataset = ExecDataSet(sql);
            if (dataset == null || dataset.Tables.Count == 0)
                return new List<IColumnInfo>();

            string path = SystemCenter.CurrentApplication.PathManager.GetSettingPath("Table", tableInfo.DBName + "." + tableInfo.Name); 
            try
            {
                System.Xml.XmlNode xc = null;
                if (System.IO.File.Exists(path))
                {
                    System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
                    xd.Load(path);
                    xc = xd.SelectSingleNode("/xml/Columns");
                }
                List<IColumnInfo> columns = new List<IColumnInfo>();
                foreach (DataRow row in dataset.Tables[0].Rows)
                {
                    ColumnInfo columnInfo = new ColumnInfo(tableInfo, row);
                    if (xc != null)
                    {
                        foreach (System.Xml.XmlNode columnNode in xc.ChildNodes)
                        {
                            if ((XmlHelper.GetAttributeValue(columnNode, "Name") ?? "") == (columnInfo.Name ?? ""))
                            {
                                columnInfo.SetCustomInfo(columnNode);
                                break;
                            }
                        }
                    }
                    columns.Add(columnInfo);
                }
                return columns;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Columns custominfo loading fail");
                return null;
            }
        }

        public DataTable GetTableData(ITableInfo tableInfo, int count, ISqlCondition where)
        {
            string sql = "SELECT ";
            if (count >= 0)
                sql += " TOP " + count.ToString() + " ";
            sql += " * FROM " + _sqlStyle.ModifierName(tableInfo.DBName, tableInfo.Name);
            string whereStr = "";
            string sortStr = "";
            if (where != null)
            {
                whereStr = where.ToSQLConditionString(_sqlStyle);
                sortStr = where.ToSQLSortString(_sqlStyle);
            }
            if (!string.IsNullOrWhiteSpace(whereStr))
                sql += " WHERE " + whereStr;
            if (!string.IsNullOrWhiteSpace(sortStr))
                sql += " ORDER BY " + sortStr;
            DataSet dataset = ExecDataSet(sql);
            DataTable table;
            if (dataset == null || dataset.Tables.Count == 0)
                table = new DataTable();
            else
                table = dataset.Tables[0];
            table.TableName = tableInfo.Name;
            return table;
        }

        public IEnumerable<ITableRelationInfo> GetTableRelation(ITableInfo tableInfo)
        {
            string sql = string.Format("SELECT '" + tableInfo.DBName + "' AS FKDBName, A.NAME AS FKNAME,'' AS SOURCEDBNAME, B.NAME AS SOURCENAME,'' AS TARGETDBNAME, C.NAME AS TARGETNAME,A.OBJECT_ID AS FKID FROM SYS.TABLES AS C,SYS.TABLES AS B,SYS.foreign_keys AS A WHERE B.OBJECT_ID = A.REFERENCED_OBJECT_ID AND C.OBJECT_ID = A.PARENT_OBJECT_ID AND (A.PARENT_OBJECT_ID={0} OR A.REFERENCED_OBJECT_ID={0})", tableInfo.Id);
            DataSet dataset = ExecDataSet(sql);
            List<ITableRelationInfo> tableRelations = new List<ITableRelationInfo>();
            if (dataset == null || dataset.Tables.Count == 0)
                return tableRelations;
             
            string path = SystemCenter.CurrentApplication.PathManager.GetSettingPath("Table", tableInfo.DBName + "." + tableInfo.Name);
            try
            {
                System.Xml.XmlNode xc = null;
                if (System.IO.File.Exists(path))
                {
                    System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
                    xd.Load(path);
                    xc = xd.SelectSingleNode("/xml/Relations");
                }

                foreach (DataRow row in dataset.Tables[0].Rows)
                {
                    TableRelationInfo tableRelation = new TableRelationInfo(this, row);
                    if (xc != null)
                    {
                        foreach (System.Xml.XmlNode relationNode in xc.ChildNodes)
                        {
                            if ((XmlHelper.GetAttributeValue(relationNode, "Name") ?? "") == (tableRelation.FKName ?? ""))
                            {
                                tableRelation.SetCustomInfo(relationNode);
                                break;
                            }
                        }
                    }
                    tableRelations.Add(tableRelation);
                }
                if (xc != null)
                {
                    foreach (System.Xml.XmlNode relationNode in xc.ChildNodes)
                    {
                        if (XmlHelper.GetBooleanAttributeValue(relationNode, "IsCustom", false))
                            tableRelations.Add(new CustomTableRelation(this, relationNode));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Table custominfo loading fail");
            }

            return tableRelations;
        }

        public IEnumerable<ITableRelationItemInfo> GetFKInfo(ITableRelationInfo relation)
        {
            string sql = string.Format("SELECT B.NAME AS SOURCENAME,A.NAME AS TARGETNAME  FROM sys.foreign_key_columns AS C,sys.columns AS A,sys.columns AS B WHERE C.parent_object_id = A.object_id AND C.parent_column_id = A.column_id AND C.referenced_object_id = B.object_id AND C.referenced_column_id = B.column_id AND C.constraint_object_id = {0}", relation.FKID);
            DataSet dataset = ExecDataSet(sql);
            List<ITableRelationItemInfo> tableRelationItems = new List<ITableRelationItemInfo>();
            if (dataset == null || dataset.Tables.Count == 0)
                return tableRelationItems;
            foreach (DataRow row in dataset.Tables[0].Rows)
                tableRelationItems.Add(new TableRelationItem(relation, row));
            return tableRelationItems;
        }

        public DataSet ExecDataSet(string sql)
        {
            SqlCommand command = new SqlCommand(sql, _dbconnect);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet dataset = new DataSet();
            dataset.CaseSensitive = true;
            try
            {
                Console.WriteLine("【SQL実行】" + sql);
                adapter.Fill(dataset);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "データ取得失敗");
            }
            return dataset;
        }

        //public IPackageExporter GetExporter()
        //{
        //    if (_packageExporter == null)
        //        _packageExporter = new PackageExporter(this);
        //    return _packageExporter;
        //}

        public class TableList : ITableList
        {
            private DataTable _dataTable;
            public object Source
            {
                get
                {
                    return _dataTable;
                }
            }

            private SQLServerAccessor _dataAccessor;
            public IDataAccessor DataAccessor
            {
                get
                {
                    return _dataAccessor;
                }
            }

            public TableList(SQLServerAccessor dataAccessor, DataTable dataTable)
            {
                _dataAccessor = dataAccessor;
                _dataTable = dataTable;
            }

            public object Filter(string key)
            {
                DataRow[] rows = _dataTable.Select(string.Format("NAME like '%{0}%' OR COMMENT like '%{0}%'", key.Replace("'", "''")));
                DataTable dt = new DataTable();
                var loopTo = _dataTable.Columns.Count - 1;
                for (int i = 0; i <= loopTo; i++)
                {
                    var targetColumn = _dataTable.Columns[i];
                    dt.Columns.Add(new DataColumn(targetColumn.ColumnName, targetColumn.DataType));
                }
                foreach (var row in rows)
                    dt.ImportRow(row);
                return dt;
            }

            public ITableInfo GetTableInfo(string dbName, string name)
            {
                IEnumerable<DataRow> rows = _dataTable.Select(string.Format("Name='{0}'", name));
                if (!rows.Any())
                    return null;
                return _dataAccessor.CreateTableInfo(rows.First());
            }
        }
    }

    public class SQLServerStyle : ISqlStyle
    {
        public string ModifierName(string name)
        {
            return "[" + name + "]";
        }

        public string ModifierName(string dbName, string tableName)
        {
            return ModifierName(dbName, tableName, null);
        }

        public string ModifierName(string dbName, string tableName, string columnName)
        {
            List<string> results = new List<string>();
            if (!string.IsNullOrWhiteSpace(dbName))
                results.Add(ModifierName(dbName));
            if (!string.IsNullOrWhiteSpace(tableName))
                results.Add(ModifierName(tableName));
            if (!string.IsNullOrWhiteSpace(columnName))
                results.Add(ModifierName(columnName));
            return string.Join(".", results);
        }
    }
}
