using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.DataBase
{
    public class SqlInfo
    {
        private string _sql = null;
        public string SQL
        {
            get { return _sql; }
            set { _sql = value; }
        }

        private string _resultTableName = null;
        public string ResultTableName
        {
            get { return _resultTableName; }
            set { _resultTableName = value; }
        } 

        public SqlInfo(string sql)
        {
            _sql = sql;
        }

        public SqlInfo(string sql,string resultTableName)
        {
            _sql = sql;
            _resultTableName = resultTableName;
        }
    }
}
