using System;
using System.Net.Sockets;
using DebugTools;
using DebugTools.Net;
using System.Windows.Forms;
using System.Collections.Generic;

namespace DebugTools.Common.Net.Server
{
    public class SocketServerProcessor : ISocketServerProcessor
    {

        private SessionManager _sessionManager;
        public SessionManager SessionManager
        {
            get { return _sessionManager; }
        }

        public SocketServerProcessor()
        {
            _sessionManager = new SessionManager();
        }

        public void Process(ISocketServer socketServer)
        {
            socketServer.BeginAccept(new AsyncCallback(DoAcceptTcpClientCallback));
        }

        // Process the client connection.
        public void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
            ISocketServer listener = (ISocketServer)ar.AsyncState;
            ISocketClient socketClient = listener.EndAccept(ar);
            SocketSession session = CreateSession(socketClient);
            OnClientConnected(session);

            session.ResetReceiveBuffer();
            IAsyncResult recAr = socketClient.BeginReceive(
                session.ReceiveBuffer, new AsyncCallback(Socket_Received), session);

            listener.BeginAccept(new AsyncCallback(DoAcceptTcpClientCallback));
             
        }

        protected virtual void OnClientConnected(SocketSession session)
        {
            _sessionManager.Add(session);
            Console.WriteLine("Client Connected");
        }

        public void Socket_Received(IAsyncResult ar)
        {
            SocketSession session = (SocketSession)ar.AsyncState;
            SocketError errorCode;
            int receiveLen = session.SocketClient.EndReceive(ar, out errorCode);
            OnReceived(session, receiveLen);

            session.ResetReceiveBuffer();
            try
            {
                IAsyncResult recAr = session.SocketClient.BeginReceive(
                    session.ReceiveBuffer, new AsyncCallback(Socket_Received), session);
            }
            catch
            {
                OnClientDisconnect(session);
            }
        }

        protected virtual void OnReceived(SocketSession session,int receiveLen)
        { 
            Console.WriteLine("Server Received");
        }

        protected virtual void OnClientDisconnect(SocketSession session)
        {
            Console.WriteLine("Server OnDisconnect");
            _sessionManager.Remove(session);
        }

        public virtual SocketSession CreateSession(ISocketClient socket)
        {
            return new SocketSession(socket);
        }
    }
}
