using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools.Common.Net.Client;
using DebugTools.Net;
using DebugTools;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using DebugTools.Const;
using DebugTools.Common.Manager;

namespace DebugModule.Core
{
    class DebugClientProcessor : SocketClientProcessor
    {

        private SocketSession _session;
        private MainApplication _application;
        public DebugClientProcessor(MainApplication application)
        {
            _application = application;
        } 

        protected override void OnConnected(SocketSession session)
        {
            _session = session;
            SendOnline(session);
        }

        public void SendOnline(SocketSession session)
        {
            string text = string.Format("pid={0}", System.Diagnostics.Process.GetCurrentProcess().Id);
            DMessage message = new DMessage(CommandConst.CLIENT_ONLINE, text);
            session.SocketClient.Send(message);
        }

        protected override void OnReceived(DebugTools.Net.SocketSession session, int receiveLen)
        {
            if (receiveLen == 0) return;
            MemoryStream stream = new MemoryStream(session.ReceiveBuffer, 0, receiveLen);
            BinaryFormatter formatter = new BinaryFormatter();
            object o = formatter.Deserialize(stream);
            if (o is DMessage)
            {
                DMessage message = (DMessage)o;
                this._application.CoreWndProc(message);
                if (message.Handled) return;
                this._application.PluginManager.SendMessage(message);
            }
            base.OnReceived(session, receiveLen);
        }

        public void SendMessage(DMessage message)
        {
            WriteExceptionManager.WriteMessage(message);
            _session.SocketClient.Send(message);
        }


    }
}
