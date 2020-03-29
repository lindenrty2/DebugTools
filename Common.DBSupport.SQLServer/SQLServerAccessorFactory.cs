using DebugTools.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.Common.DBSupport.SQLServer
{
    public class SQLServerAccessorFactory : IDataAccessorFactory
    {
        public string Type
        {
            get { return "SqlServer"; }
        }

        public IDataAccessor Create(DBConnectInfo info)
        {
            return new SQLServerAccessor(info);
        }
    }
}
