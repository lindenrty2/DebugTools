using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools.Package;
using System.Data;

namespace DebugTools.DataBase
{
    public interface IDataAccessor
    {
        String DisplayName{get;}

        bool Connect() ;

        bool Close() ;

        ITableList GetTables(string dbName = null);

        ITableInfo GetTableInfo(string dbName,String tableName) ;

        void ClearCache();

        IEnumerable<IColumnInfo> GetColumns(ITableInfo tableInfo ) ;

        DataTable GetTableData(ITableInfo tableInfo, int count,ISqlCondition where );

        IEnumerable<ITableRelationInfo> GetTableRelation(ITableInfo tableInfo ) ;

        IEnumerable<ITableRelationItemInfo> GetFKInfo(ITableRelationInfo relation ) ;

        DataSet ExecDataSet(String sql) ;

        //IPackageExporter GetExporter() ;

    }
}
