using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools;

namespace DebugTools.Common
{
    public interface IXmlEditControl
    {
        string Title { get; }

        string Description { get; }

        bool Initialize(IXmlFile config);

        bool Save(IXmlFile config);
    }
}
