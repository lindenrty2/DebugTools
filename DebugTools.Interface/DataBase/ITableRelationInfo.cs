using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DebugTools.DataBase
{
    public interface ITableRelationInfo
    {
        
        long FKID{get; }
        string FKDBName { get; set; }
        string FKName{get;set; }
        string SourceDBName { get; set; }
        string SourceName { get; set; }
        ITableInfo SourceInfo{get; }
        string TargetDBName { get; set; }
        string TargetName{get;set;}
        ITableInfo TargetInfo{get;}
        IEnumerable<ITableRelationItemInfo> Items { get; set; }
        IDataAccessor DataAccessor { get; }
        bool IsCustom{get;}
        bool IsOutputRelation { get; set; } 
        
        bool IsParentTable(ITableInfo tableInfo);
        ITableInfo GetTargetTableInfo(bool isParentView);
        ISqlConditionGroup CreateWhere(bool isParentView, IEnumerable<DataRow> rows);
        bool IsTwoWay();
        bool IsVaild();
         
    }

    public interface ITableRelationItemInfo
    {
        
        String SourceColumnName{get;set;}
        String TargetColumnName{get;set;}

        ITableRelationInfo TableRelation { get; }

        String GetTargetColumn(bool isParentView);
          
    }
}
