using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using DebugTools.Common.Manager;
using DebugTools;
using DebugTools.Common;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using DebugTools.Common.Net.Server;
using DebugTools.Net;
using DebugTools.Const; 

namespace DebugTools.DBO
{
    public class MainApplication : BaseApplication
    { 

        public override AppMode AppMode
        {
            get { return AppMode.ControlCenter; }
        }   

        public MainApplication()
        {
        }

        public override bool Init()
        {  
            return base.Init();
        }

        public override void SendMessage(DMessage message)
        { 
        }

        public override void CoreWndProc(DebugTools.DMessage message)
        {
            base.CoreWndProc(message); 
        }

    }
}
