using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    public interface IManagerCreator
    {
        void Regist(string name, Type type);
        void UnRegist(string name);
        T GetInstance<T>(string name, params object[] parameter) where T : IServicesManager ;

    }
}
