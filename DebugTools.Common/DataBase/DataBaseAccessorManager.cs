using System.Collections.Generic;

namespace DebugTools.DataBase
{
    public class DataBaseAccessorManager : IDataAccessorManager
    {
        private static DataBaseAccessorManager _default;
        public static DataBaseAccessorManager Default { get { return _default; } }

        private Dictionary<string, IDataAccessorFactory> _accessorMap = new Dictionary<string, IDataAccessorFactory>();

        private IApplication _app;

        internal DataBaseAccessorManager(IApplication app)
        {
            _app = app;
            _default = this;
        }
        
        public IDataAccessorFactory Get(string type)
        {
            return _accessorMap[type];
        }

        public IDataAccessor Create(DBConnectInfo info)
        {
            return _accessorMap[info.Type].Create(info);
        }

        public void Regist<Factory>() where Factory : IDataAccessorFactory
        {
            IDataAccessorFactory factory = _app.ClassManager.GetInstance<Factory>("Main", "");
            if (_accessorMap.ContainsKey(factory.Type))
            {
                _accessorMap[factory.Type] = factory;
            }
            else
            {
                _accessorMap.Add(factory.Type, factory);
            }
        }

    }
}
