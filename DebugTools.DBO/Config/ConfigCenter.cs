using DebugTools.DataBase;
using System.Collections.Generic;
using System.IO;

namespace DebugTools.DBO
{
    public class ConfigCenter
    {
        private static ConfigCenter _instance;
        public static ConfigCenter Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ConfigCenter();
                return _instance;
            }
        }

        public IEnumerable<DBConnectInfo> GetConnectInfoList()
        {
            if (!File.Exists(@"setting\\connects.xml"))
                return new List<DBConnectInfo>();
            return XmlSerializeHelper.Deserialize<List<DBConnectInfo>>(@"setting\\connects.xml");
        }

        public int GetDefaultMaxSearchCount()
        {
            return 100;
        }
    }
}
