using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.DataBase
{
    public interface ISqlCondition
    { 

        String ToSQLConditionString(ISqlStyle style);

        String ToSQLSortString(ISqlStyle style);

    }

    public interface ISqlConditionGroup : ISqlCondition 
    {

        IEnumerable<ISqlCondition> Items { get; }

    }
}
