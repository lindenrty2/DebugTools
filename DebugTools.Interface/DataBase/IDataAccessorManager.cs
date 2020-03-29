using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.DataBase
{
    public interface IDataAccessorManager
    {
        IDataAccessorFactory Get(string type);

        IDataAccessor Create(DBConnectInfo info);

        void Regist<Factory>() where Factory : IDataAccessorFactory;

    }
}
