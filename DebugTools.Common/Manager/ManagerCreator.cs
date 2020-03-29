using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using DebugTools;
using DebugTools.Const;

namespace DebugTools.Common.Manager
{
    public class ManagerCreator
        : IManagerCreator
    { 

        private IClassManager _classManager;
        public ManagerCreator(IClassManager classManager)
        {
            _classManager = classManager;
        }

        public void Regist(string name, Type type)
        {
            _classManager.RegistSingle(ClassCategoryConst.MANAGER, name, type);
        }

        public void UnRegist( string name)
        {
            _classManager.Unregist(ClassCategoryConst.MANAGER, name);
        }

        public T GetInstance<T>(string name, params object[] parameter) where T : IServicesManager
        {
            T instance= _classManager.GetInstance<T>(ClassCategoryConst.MANAGER, name, parameter);
            instance.SetClassManager(_classManager);
            return instance;
        } 

    }
}
