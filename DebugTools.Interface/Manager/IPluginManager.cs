using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    public interface IPluginManager
    {
        void LoadAll();

        void SendMessage(DMessage message);

    }
}
