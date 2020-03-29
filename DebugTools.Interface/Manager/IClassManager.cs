using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace DebugTools
{
    public interface IClassManager
    {
        void Regist(ClassInformation information);

        void Regist(string category, string name, Type type);

        void RegistSingle(string category, string name, Type type);

        void Unregist(ClassInformation information);

        void Unregist(string category, string name);

        ClassInformation Find(string category, string name);

        ClassInformation[] Find(string category);

        T GetInstance<T>(string category, string name, params object[] parameters);

        T[] GetInstance<T>(string category, params object[] parameters);

        T GetInstance<T>(ClassInformation information, params object[] parameters);

        void Reset();
    }
}
