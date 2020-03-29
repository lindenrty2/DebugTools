using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools.Net;
using System.Net.Sockets;

namespace DebugTools.Common.Net.Client
{
    public class SocketClientProcessor : ISocketClientProcessor
    { 
        public void Process(ISocketClient client)
        { 
            SocketSession session = new SocketSession(client);
            OnConnected(session);
            session.ResetReceiveBuffer();
            IAsyncResult recAr = client.BeginReceive(
              session.ReceiveBuffer,new AsyncCallback(Socket_Received),session);
        }


        protected virtual void OnConnected(SocketSession session)
        {

        }

        public void Socket_Received(IAsyncResult ar)
        {
            SocketSession session = (SocketSession)ar.AsyncState;
            SocketError errorCode;
            int receiveLen = session.SocketClient.EndReceive(ar, out errorCode);

            OnReceived(session, receiveLen);

            session.ResetReceiveBuffer();
            IAsyncResult recAr = session.SocketClient.BeginReceive(
                session.ReceiveBuffer, new AsyncCallback(Socket_Received), session);
        }

        protected virtual void OnReceived(SocketSession session,int receiveLen)
        {

        }

        public virtual SocketSession CreateSession(ISocketClient client)
        {
            return new SocketSession(client);
        }

    }
}
