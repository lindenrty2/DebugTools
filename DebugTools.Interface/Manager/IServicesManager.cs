using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    public interface IServicesManager
    {
        string Name { get; }

        void Regist(string function, string name, Type type);

        void UnRegist(string function, string name);

        T[] GetInstance<T>(string function, params object[] parameter);

        T GetInstance<T>(string function, string name, params object[] parameter);

        IExecuteResult Execute(string function, params object[] parameter);

        IExecuteResult Execute(string function, string name, params object[] parameter);

        void SetClassManager(IClassManager classManager);
    }
}
