using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools; 

namespace DebugTools.Common.Service
{
    public abstract class ServiceManagerBase
        : IServicesManager
    { 
        public abstract string Name { get ;}

        private IClassManager _classManager;
        public ServiceManagerBase()
        {
        }

        public void Regist(string function, string name, Type type)
        {
            _classManager.Regist(GetCategory(function), name, type);
        }

        public void UnRegist(string function, string name)
        {
            _classManager.Unregist(GetCategory(function), name);
        }

        protected abstract string GetCategory(string function);

        public T GetInstance<T>(string function, string name, params object[] parameter)
        {
            return _classManager.GetInstance<T>(GetCategory(function), name, parameter);
        }

        public T[] GetInstance<T>(string function, params object[] parameter) 
        {
            return _classManager.GetInstance<T>(GetCategory(function), parameter);
        }

        public abstract IExecuteResult Execute(string function, string name, params object[] parameter);

        public abstract IExecuteResult Execute(string function, params object[] parameter);

        public void  SetClassManager(IClassManager classManager)
        {
            _classManager = classManager;
        }
    }
}
