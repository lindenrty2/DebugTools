using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools.Common;

namespace DebugTools
{
    public class XmlConfigInfo
    {
        private string _key;
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private IXmlFile _xmlFile;
        public IXmlFile XmlFile
        {
            get { return _xmlFile; }
            set { _xmlFile = value; }
        }

        private Type[] _xmlEditControlTypes;
        public Type[] SettingControlsTypes
        {
            get { return _xmlEditControlTypes; }
            set { _xmlEditControlTypes = value; }
        }

        public XmlConfigInfo(string key, IXmlFile xmlFile )
        {
            _key = key;
            _xmlFile = xmlFile; 
        }

        public XmlConfigInfo(string key, string title,IXmlFile xmlFile, Type[] xmlEditControlTypes)
        {
            _key = key;
            _title = title;
            _xmlFile = xmlFile;
            _xmlEditControlTypes = xmlEditControlTypes;
        }

        public void AddSettingControlTypes(Type[] editControlTypes)
        {
            if (_xmlEditControlTypes == null)
            {
                _xmlEditControlTypes = editControlTypes;
            }
            else
            {
                _xmlEditControlTypes = _xmlEditControlTypes.Union(editControlTypes).ToArray();
            }
        }
    }
}
