using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Linq;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Media;
using System.Windows.Controls;
using System;
using System.Xml.Linq;
using System.Windows.Navigation;
using System.IO;
using System.Runtime.InteropServices;
using System.Data;
using DebugTools.DataBase;
using DebugTools.Package;

namespace DebugTools.DBO
{
    public class PackageDataAccessor : IDataAccessor
    {
        public string DisplayName
        {
            get
            {
                return _package.DisplayName;
            }
        }

        private IPackageExporter _packageExporter;
        private PackageFile _package;
        private string _path;
        public PackageDataAccessor(string path)
        {
            _path = path;
        }

        private TableList _tableList;
        public ITableList GetTables(string dbName = null)
        {
            if (_tableList == null)
                _tableList = new TableList(this, _package);
            return _tableList;
        }

        public DataSet ExecDataSet(string sql)
        {
            throw new NotImplementedException("未実装です");
        }

        public IEnumerable<IColumnInfo> GetColumns(ITableInfo tableInfo)
        {
            throw new NotSupportedException("パッケージの場合必要ない");
        }

        public IEnumerable<ITableRelationItemInfo> GetFKInfo(ITableRelationInfo relation)
        {
            throw new NotSupportedException("パッケージの場合必要ない");
        }

        public System.Data.DataTable GetTableData(ITableInfo tableInfo, int count, ISqlCondition where)
        {
            PackageTableInfo tablePackage = _package.Tables.FirstOrDefault(x => (string.IsNullOrWhiteSpace(tableInfo.DBName) || x.TableInfo.DBName.Equals(tableInfo.DBName)) && x.TableInfo.Name.Equals(tableInfo.Name));
            if (tablePackage == null)
                return new DataTable();
            return tablePackage.Data.TableData;
        }

        public ITableInfo GetTableInfo(string dbName, string tableName)
        {
            return _tableList.GetTableInfo(dbName, tableName);
        }

        public System.Collections.Generic.IEnumerable<ITableRelationInfo> GetTableRelation(ITableInfo tableInfo)
        {
            throw new NotSupportedException("パッケージの場合必要ない");
        }

        public void ClearCache()
        {
            _tableList = null;
        }

        public bool Connect()
        {
            try
            {
                _package = new PackageFile(this, _path);
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
            _package.Close();
            return true;
        }

        public IPackageExporter GetExporter()
        {
            if (_packageExporter == null)
                _packageExporter = new PackageExporter(this);
            return _packageExporter;
        }

        public class TableList : ITableList
        {
            private IDataAccessor _dataAccessor;
            public IDataAccessor DataAccessor
            {
                get
                {
                    return _dataAccessor;
                }
            }

            private IEnumerable<ITableInfo> _tableInfos;
            public object Source
            {
                get
                {
                    return _tableInfos;
                }
            }

            private PackageFile _package;
            /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="package"></param>
        /// <remarks></remarks>
            public TableList(IDataAccessor dataAccessor, PackageFile package)
            {
                _dataAccessor = dataAccessor;
                _package = package;
                _tableInfos = package.Tables.Select(x => x.TableInfo);
            }

            public object Filter(string key)
            {
                return _tableInfos.Where(x => x.Name.Contains(key) || x.Comment.Contains(key));
            }

            public ITableInfo GetTableInfo(string dbName, string name)
            {
                return _tableInfos.FirstOrDefault(x => (string.IsNullOrWhiteSpace(dbName) || (x.DBName ?? "") == (dbName ?? "")) && x.Name.Equals(name));
            }
        }
    }
}
