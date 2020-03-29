using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.DataBase
{
    public interface IDataAccessorFactory
    {
        string Type { get; }
        IDataAccessor Create(DBConnectInfo info);

    }
}
