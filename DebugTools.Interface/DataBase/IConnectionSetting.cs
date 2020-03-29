using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DebugTools.Common;

namespace DebugTools.DBSupport
{
    public interface IConnectionSetting 
    {
        String Key { get; set; }

        String Name { get; set; }

        int DataBaseType { get; set; }

        String ConnectionString { get; set; }

        bool IsDefault { get; set; }


    }
}
