using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DebugTools.Common.Config
{
    public class ConnectionItemNode : CXmlNode 
    {
        public string Key
        {
            get { return this.GetAttributeValue("Key"); }
            set { this.SetAttributeValue("Key", value); }
        }

        public string ConnectionString
        {
            get { return this.GetAttributeValue("ServiceName"); }
            set { this.SetAttributeValue("ServiceName",value); }
        }

        public bool IsDisplay
        {
            get { return this.GetBooleanAttributeValue("IsDisplay"); }
            set { this.SetBooleanAttributeValue("IsDisplay", value); }
        }

        public ConnectionItemNode(XmlNode node) : base(node) { }
         
    }
}
