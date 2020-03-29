using DebugTools.DataBase;
using DebugTools.Package;
using DebugTools.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DebugTools.Common.DBSupport.MySQL
{
    public class MySQLDataAccessor : IDataAccessor
    {
        public string DisplayName
        {
            get
            {
                return _dbInfo.DisplayName;
            }
        }

        private MySQLStyle _sqlStyle = new MySQLStyle();
        private DBConnectInfo _dbInfo;
        private MySqlConnection _dbconnect;
        //private IPackageExporter _packageExporter;
        private Dictionary<string, ITableInfo> _tableInfoCacheList = new Dictionary<string, ITableInfo>();

        public static string CreateConnectString(DBConnectInfo connectInfo)
        {
            return string.Format("server={0};database={1};port=3306;uid={2};pwd={3};charset='utf8'", connectInfo.Host, connectInfo.DataBaseName, connectInfo.UserName, connectInfo.Password);
        }

        public MySQLDataAccessor(DBConnectInfo connectInfo)
        {
            _dbInfo = connectInfo;
        }

        public bool Connect()
        {
            try
            {
                _dbconnect = new MySqlConnection(CreateConnectString(_dbInfo));
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
            string sql = "SELECT TABLE_SCHEMA AS DBNAME, TABLE_NAME AS NAME,TABLE_COMMENT AS COMMENT,0 AS ID FROM information_schema.TABLES ";
            if (dbName != null)
                sql += "WHERE TABLE_SCHEMA = '" + dbName + "'";

            DataSet dataset = ExecDataSet(sql);
            if (dataset == null || dataset.Tables.Count == 0)
                return new TableList(this, new DataTable());

            dataset.Tables[0].CaseSensitive = false;
            return new TableList(this, dataset.Tables[0]);
        }

        public ITableInfo GetTableInfo(string tableDBName, string tableName)
        {
            string key = tableDBName + "." + tableName;
            if (_tableInfoCacheList.ContainsKey(key))
                return _tableInfoCacheList[key];
            string sql = string.Format("SELECT TABLE_SCHEMA AS DBNAME,TABLE_NAME AS NAME,TABLE_COMMENT AS COMMENT,0 AS ID FROM information_schema.TABLES WHERE TABLE_SCHEMA = '{0}' AND TABLE_NAME = '{1}'", tableDBName, tableName);

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
            string tableDBName = TableInfo.GetTableDBName(row);
            string key = tableDBName + "." + tableName;
            if (_tableInfoCacheList.ContainsKey(key))
                return _tableInfoCacheList[key];
            TableInfo ti = new TableInfo(this, row);
            string path = SystemCenter.CurrentApplication.PathManager.GetSettingPath("Table", key); 
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
                _tableInfoCacheList.Add(key, ti);
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
            string sql = string.Format("select `COLUMN_NAME` AS `NAME`,`COLUMN_COMMENT` AS `COMMENT`, 0 AS TABLE_ID,0 AS `COLUMN_ID`,`DATA_TYPE` AS `TYPENAME`,`CHARACTER_MAXIMUM_LENGTH` AS MAX_LENGTH,`NUMERIC_PRECISION` AS `PRECISION` ,`NUMERIC_SCALE` AS `SCALE`,IF(`COLUMN_KEY` IS NULL,0,1) AS `KEYNO` from information_schema.COLUMNS where table_name = '{0}' and table_schema = '{1}'", tableInfo.Name, tableInfo.DBName);

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
            if (count >= 0)
                sql += " LIMIT 0," + count.ToString() + " ";
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
            string sql = string.Format("SELECT FKDBNAME,FKNAME,SOURCEDBNAME,SOURCENAME,TARGETDBNAME,TARGETNAME,0 AS FKID FROM( SELECT `CONSTRAINT_SCHEMA` AS FKDBNAME, `CONSTRAINT_NAME` AS FKNAME, `TABLE_SCHEMA` AS SOURCEDBNAME, `TABLE_NAME` AS SOURCENAME,`REFERENCED_TABLE_SCHEMA` AS TARGETDBNAME, `REFERENCED_TABLE_NAME` AS TARGETNAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where ((TABLE_NAME='{0}' AND TABLE_SCHEMA = '{1}') OR (REFERENCED_TABLE_NAME='{0}' AND REFERENCED_TABLE_SCHEMA = '{1}')) AND CONSTRAINT_NAME <> 'PRIMARY') AS C GROUP BY FKNAME,SOURCENAME,TARGETNAME", tableInfo.Name, tableInfo.DBName);
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
            string sql = string.Format("select `COLUMN_NAME` AS SOURCENAME,`REFERENCED_COLUMN_NAME` AS TARGETNAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where `CONSTRAINT_NAME`='{0}' AND TABLE_SCHEMA = '{1}'", relation.FKName, relation.FKDBName);
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
            MySqlCommand command = new MySqlCommand(sql, _dbconnect);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
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

            private MySQLDataAccessor _dataAccessor;
            public IDataAccessor DataAccessor
            {
                get
                {
                    return _dataAccessor;
                }
            }

            public TableList(MySQLDataAccessor dataAccessor, DataTable dataTable)
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
                IEnumerable<DataRow> rows = _dataTable.Select(string.Format("DBName='{0}' AND Name='{1}'", dbName, name));
                if (!rows.Any())
                    return null;
                return _dataAccessor.CreateTableInfo(rows.First());
            }
        }
    }

    public class MySQLStyle : ISqlStyle
    {
        public string ModifierName(string name)
        {
            return "`" + name + "`";
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
