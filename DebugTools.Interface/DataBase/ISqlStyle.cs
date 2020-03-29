using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.DataBase
{
    public interface ISqlStyle
    {
        /// <summary>
        /// 列名表名与关键字冲突时用的修饰符
        /// </summary>
        string ModifierName(string name);

        string ModifierName(string dbName,string tableName);

        string ModifierName(string dbName, string tableName,string columnName);

    }
}
