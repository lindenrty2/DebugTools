using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools.Common;
using System.Xml;

namespace DebugTools.Common.Config
{
    public class BaseNode : CXmlNode
    {

        public BaseNode(XmlNode node) : base(node) { }

        public string MainAppDirectory
        {
            get { return base.GetNode("MainAppDirectory").InnerText; }
            set { this.GetNode("MainAppDirectory").InnerText = value; }
        }

    }
}
