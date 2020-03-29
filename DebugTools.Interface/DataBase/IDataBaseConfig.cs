using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DebugTools.Common;

namespace DebugTools.DBSupport
{
    public interface IDataBaseConfig 
    {
        IDictionary<string, IConnectionSetting> ConnectionSettingList { get; }

        string MainConnectionKey { get; set; }

        IConnectionSetting MainConnectionSetting { get; }

        IConnectionSetting GetConnectionSetting(string key);

    }
}
