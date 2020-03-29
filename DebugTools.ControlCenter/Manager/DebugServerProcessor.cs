using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DebugTools.Common.Net.Server;
using DebugTools;
using DebugTools.Net;
using DebugTools.Const;
using DebugTools.Common.Manager;

namespace DebugTools.ControlCenter.Manager
{
    class DebugServerProcessor : SocketServerProcessor
    {

        private MainApplication _application;
        public DebugServerProcessor(MainApplication application)
        {
            _application = application;
        }

        protected override void OnClientConnected(SocketSession session)
        {
            base.OnClientConnected(session);
        }

        protected override void OnReceived(SocketSession session, int receiveLen)
        {
            try
            {
                if (receiveLen == 0) return;
                MemoryStream stream = new MemoryStream(session.ReceiveBuffer, 0, receiveLen);
                BinaryFormatter formatter = new BinaryFormatter();
                object o = formatter.Deserialize(stream);
                if (o is DMessage)
                {
                    DMessage message = (DMessage)o;
                    if (message.Title == CommandConst.CLIENT_ONLINE)
                    {
                        DMessage revMessage = new DMessage(CommandConst.WND_STARTMENU, @"Plugin\Plugin_Monitoring\Plugin_Permission_Monitoring");
                        revMessage.SetArg(CommandConst.ARG_KEY_STATUS, CommandConst.ARG_VALUE_TRUE);
                        session.SocketClient.Send(revMessage);
                    }
                    
                    this._application.CoreWndProc(message);
                    if (message.Handled) return;
                    this._application.PluginManager.SendMessage(message);
                }
                base.OnReceived(session, receiveLen);
            }
            catch
            {
                WriteExceptionManager.OnOrOff = true;
                WriteExceptionManager.WriteMessage(session.ReceiveBuffer, receiveLen);
                WriteExceptionManager.OnOrOff = false;
            }
        }

        public void SendMessage(DMessage message)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, message);
            foreach (SocketSession session in this.SessionManager)
            {
                session.SocketClient.Send(stream.GetBuffer());
            }
        }

        protected override void OnClientDisconnect(SocketSession session)
        {
            base.OnClientDisconnect(session);
            DMessage message = new DMessage(CommandConst.CLIENT_OFFLINE, System.Diagnostics.Process.GetCurrentProcess().Id.ToString());
            SystemCenter.CurrentApplication.SendMessage(message);
        }

    }
}
