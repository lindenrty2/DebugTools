using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.DataBase
{
    public interface ITableList
    {

        IDataAccessor DataAccessor { get; }

        Object Source{get;}

        Object Filter(String key); 

        ITableInfo GetTableInfo(String dbName, String name);

    }
}
