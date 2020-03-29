using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools;

namespace DebugTools.Common.Manager
{
    public class ConfigManager : IConfigManager
    {
        private Dictionary<string, XmlConfigInfo> _configDictionary = new Dictionary<string, XmlConfigInfo>();

        public IXmlFile GetConfig(string key)
        {
            if (_configDictionary.ContainsKey(key))
            {
                return _configDictionary[key].XmlFile;
            }
            return null;
        }

        public XmlConfigInfo GetConfigInfo(string key)
        {
            if (_configDictionary.ContainsKey(key))
            {
                return _configDictionary[key];
            }
            return null;
        }

        public XmlConfigInfo[] GetConfigInfos()
        {
            return _configDictionary.Values.ToArray();
        }

        public void SetConfig(string key, IXmlFile config)
        {
            if (_configDictionary.ContainsKey(key))
            {
                _configDictionary[key].XmlFile = config;
            }
            else
            {
                _configDictionary.Add(key, new XmlConfigInfo(key, config));
            }
        }

        public void SetConfig(string key,string title, IXmlFile config,Type[] editControlTypes)
        {
            if (_configDictionary.ContainsKey(key))
            {
                _configDictionary[key] = new XmlConfigInfo(key, title, config, editControlTypes);
            }
            else
            {
                _configDictionary.Add(key, new XmlConfigInfo(key, title, config, editControlTypes));
            }
        }

        public void AddConfigSetting(string key, string title, IXmlFile config, Type[] editControlTypes)
        {
            if (!_configDictionary.ContainsKey(key)) return;

            XmlConfigInfo info = _configDictionary[key];
            info.AddSettingControlTypes(editControlTypes);
        } 

        public bool ContainsConfig(string key)
        {
            return _configDictionary.ContainsKey(key);
        } 

 
    }
}
