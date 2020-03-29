using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools.Common;

namespace DebugTools
{
    public interface IConfigManager
    {
        IXmlFile GetConfig(string key);

        void SetConfig(string key,IXmlFile config);

        void SetConfig(string key,string title, IXmlFile config,Type[] editControlTypes);

        bool ContainsConfig(string key);

        XmlConfigInfo GetConfigInfo(string key);

        XmlConfigInfo[] GetConfigInfos();

    }
}
