using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DebugTools.Common;
using DebugTools.DBSupport;

namespace DebugTools.Common.DBSupport
{
    public class DataBaseConfig : CXmlFile , IDataBaseConfig
    {
        private Dictionary<string,IConnectionSetting> _connectionSettingList = null;
        public IDictionary<string, IConnectionSetting> ConnectionSettingList
        {
            get { return _connectionSettingList; }
        }

        private string _mainConnectionkey = null;
        public string MainConnectionKey
        {
            get {
                return _mainConnectionkey; 
            }
            set {
                if (_mainConnectionkey == value)
                {
                    return;
                }
                if (MainConnectionSetting != null)
                {
                    MainConnectionSetting.IsDefault = false;
                }
                _mainConnectionkey = value;
                MainConnectionSetting.IsDefault = true;
            }
        }

        public IConnectionSetting MainConnectionSetting
        {
            get { return _connectionSettingList[_mainConnectionkey]; } 
        }

        public DataBaseConfig(string path)
            : base(path)
        {
            CreateConnectionSettings();
        }

        private void CreateConnectionSettings()
        {
            _connectionSettingList = new Dictionary<string, IConnectionSetting>();
            XmlNodeList nodeList = _document.SelectNodes("root/Connections/Connection");
            if (nodeList.Count == 0)
            {
                return;
            }
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlNode currentNode = nodeList[i];
                string key = currentNode.Attributes["Key"].Value;
                ConnectionSetting currentSetting = new ConnectionSetting(currentNode);
                _connectionSettingList.Add(key, currentSetting);
                if (currentSetting.IsDefault)
                {
                    _mainConnectionkey = currentSetting.Key;
                }
            }
            if (_mainConnectionkey == null)
            {
                _mainConnectionkey = _connectionSettingList.Values.ToArray()[0].Key ;
            }
        }

        public IConnectionSetting GetConnectionSetting(string key)
        {
            return _connectionSettingList[key];
        }
         
    }
}
