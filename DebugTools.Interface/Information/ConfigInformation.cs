using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    [Serializable]
    public class ConfigInformation
    {
        public string Key { get; set; }

        public string ConfigName { get; set; }

        public string FilePath { get; set; }

        public Type ConfigSettingControl { get; set; }
    }
}
