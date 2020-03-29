using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools;

namespace DebugTools.Common.DBSupport
{
    [PluginEntry]
    public class PluginEntry
        : IPluginEntry
    {

        public string Name
        {
            get { return "DebugTools.Common.DBSupport"; }
        }

        public string Description
        {
            get { return "数据库支持"; }
        }

        private IApplication _app;

        public bool Support(IApplication app)
        {
            return true;
        }

        public bool Connection(IApplication app)
        {
            _app = app; 
            return true;
        }
         

        public bool Disconnection()
        { 
            return true;
        }

        public void WndProc(DMessage message)
        {
        }

    }
}
