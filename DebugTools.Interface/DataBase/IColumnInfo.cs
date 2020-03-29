using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools.Secret;

namespace DebugTools.DataBase
{
    public interface IColumnInfo
    {
         
        int Id {get;}
        ITableInfo TableInfo {get;}
        String Name { get; }
        String DisplayName { get; }  
        String TypeName {get;}
        String DisplayTypeName {get;}
        Type DataType {get;}
        long MaxLength {get;}
        int Precision {get;}  
        int Scale {get;}  
        int PKNo {get;}  
        bool IsPK {get;}  
        String Comment {get;}  
        String Memo {get;set;}  
        SecretType SecretType {get;set;}

        bool IsTextType();
        bool IsNumberType();

    }
}
