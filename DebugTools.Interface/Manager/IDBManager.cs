using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools.DataBase;

namespace DebugTools
{
    public interface IDBManager
        : IServicesManager
    {
        IDBAccessor GetMainAccessor(); 

        IDBAccessor GetAccessor(string key); 

        void Reset();
    }
}
