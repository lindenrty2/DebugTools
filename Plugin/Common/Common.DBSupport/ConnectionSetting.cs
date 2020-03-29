using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DebugTools.Common;
using DebugTools.DBSupport;

namespace DebugTools.Common.DBSupport
{
    public class ConnectionSetting : CXmlNode, IConnectionSetting
    {
        public String Key
        {
            get { return GetAttributeValue("Key"); }
            set { SetAttributeValue("Key", value); }
        }

        public String Name
        {
            get { return GetAttributeValue("Name"); }
            set { SetAttributeValue("Name", value); }
        }

        public int DataBaseType
        {
            get { return Convert.ToInt16( GetAttributeValue("DataBaseType")); }
            set { SetAttributeValue("DataBaseType", value.ToString()); }
        }

        public String ConnectionString
        {
            get { return GetAttributeValue("ConnectionString"); }
            set { SetAttributeValue("ConnectionString", value); }
        }

        public bool IsDefault
        {
            get { return GetBooleanAttributeValue("Defualt"); }
            set { SetBooleanAttributeValue("Defualt", value); }
        }

        public ConnectionSetting(XmlNode node)
            :base(node)
        {
        
        }
        
        public override string ToString()
        {
            return this.Name;
        }

    }
}
