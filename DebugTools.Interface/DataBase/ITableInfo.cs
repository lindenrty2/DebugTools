using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.DataBase
{
    public interface ITableInfo
    {
        int Id{get;} 
        String DBName { get; }
        String Name{get;} 
        String DisplayName{get;} 
        String Comment{get;} 

        IEnumerable<IColumnInfo> Columns{get;} 
        IEnumerable<ITableRelationInfo> Relations{get;} 
        
        bool IsAutoRelationOutput{get;set;} 
     
        IDataAccessor DataAccessor{get;}

        IColumnInfo GetColumn(string columnName);
        void AddRelation(ITableRelationInfo tableRelation);
        bool ContainsRelation(String relationName);
        ITableRelationInfo GetRelation(String relationName);
        void ChangeRelation(String oldName ,ITableRelationInfo relation);
  
    }
}
