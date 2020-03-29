using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    public interface IPluginEntry
    {
        string Name { get; }
        string Description { get; }

        bool Support(IApplication app);

        bool Connection(IApplication app);

        bool Disconnection();

        void WndProc(DMessage message);
    }
}
