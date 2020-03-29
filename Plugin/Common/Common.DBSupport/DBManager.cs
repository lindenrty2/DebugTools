using System.Collections.Generic;
using DebugTools.Common.Manager;
using DebugTools.DataBase;
using DebugTools.DBSupport;
using DebugTools.Interface;

namespace DebugTools.Common.DBSupport
{
    public class DBManager : IDBManager
    {
        private IDataBaseConfig _setting = null;
        public IDataBaseConfig Setting
        {
            get
            {
                if (_setting == null)
                {
                    _setting = new DataBaseConfig(ApplicationDomain.CurrentApplication.PathManager.GetSettingPath("DataBaseConfig.xml"));
                }
                return _setting;
            }
        }

        private IDBAccessor _dbAccessor = null;
        public IDBAccessor GetMainAccessor()
        {
            if (_dbAccessor == null)
            {
                _dbAccessor = CreateAccessor(Setting.MainConnectionKey);
                _accessors.Add(Setting.MainConnectionKey, _dbAccessor);
            }
            return _dbAccessor;
        }

        private Dictionary<string, IDBAccessor> _accessors = new Dictionary<string, IDBAccessor>();
        public IDBAccessor GetAccessor(string key)
        {
            if (!_accessors.ContainsKey(key))
            {
                _accessors.Add(key, CreateAccessor(key));
            }
            return _accessors[key];
        }

        protected IDBAccessor CreateAccessor(string key)
        {
            IConnectionSetting connectionSetting = Setting.GetConnectionSetting(key);
            return ApplicationDomain.CurrentApplication.ClassManager.GetInstance<IDBAccessor>(
                Const.ClassCategoryConst.DBACCESSOR,connectionSetting.DataBaseType.ToString(), connectionSetting);
        }

        public void Reset()
        {
            _accessors.Clear();
            _dbAccessor = null;
        }

         
    }
}
