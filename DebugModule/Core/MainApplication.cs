using DebugTools;
using DebugTools.Common;
using DebugTools.Const;
using DebugTools.Common.Net.Client;
using DebugModule.Function;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace DebugModule.Core
{
    public class MainApplication : BaseApplication
    {
        private static MainApplication _current;
        public static MainApplication Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new MainApplication();
                    _current.Init(); 
                }
                return _current;
            }
        }

        public override AppMode AppMode
        {
            get { return AppMode.DebugTools; }
        }

        public NotifyIcon _mainTray = null;
        public NotifyIcon MainTray
        {
            get
            {
                if (_mainTray == null)
                {
                    _mainTray = new NotifyIcon();
                }
                return _mainTray;
            }
        }

        private SocketClient _client;
        private DebugClientProcessor _processor;

        public MainApplication()
        {
            
        }

        public override bool Init()
        {
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Exit += new System.Windows.ExitEventHandler(Current_Exit);
            }
            else
            {
                System.Windows.Forms.Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            }
            return base.Init();
        }

        private void Current_Exit(object sender, EventArgs e)
        {
            SendOffLineMessage();
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            SendOffLineMessage();
        }

        private void SendOffLineMessage()
        {
            DMessage message = new DMessage(CommandConst.CLIENT_OFFLINE, Process.GetCurrentProcess().Id.ToString());
            SendMessage(message);
        }

        public override void SendMessage(DMessage message)
        {
            if (_processor != null) _processor.SendMessage(message);
            CoreWndProc(message);
            if (message.Handled) return;
            PluginManager.SendMessage(message);
                        
        }

        public override void CoreWndProc(DMessage message)
        {
            base.CoreWndProc(message);
            if (message.Message.Equals("StartBatchWarpperMakerView"))
            {
                BatchWarpperMakerView view = new BatchWarpperMakerView();
                view.SetAssembly((Assembly)message.Tag );
                view.Show();
            }
        }

        public void ConnectControlCenter()
        {
            try
            {
                _processor = new DebugClientProcessor(this);
                _client = new SocketClient();
                _client.SetProcessor(_processor);
                _client.Connect(10086); 
            }
            catch
            {

            } 

        }
    }
}
