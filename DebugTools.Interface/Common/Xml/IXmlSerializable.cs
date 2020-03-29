using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DebugTools.Common
{
    public interface IXmlSerializable
    {
        bool Load(XmlNode node);

        bool Write(XmlNode node);

    }
}
