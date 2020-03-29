using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    public interface IXmlFile : IXmlNode
    {
        void SetOrginXml();

        bool Save();

        bool SaveAs(string path);

        void Close();
    }
}
