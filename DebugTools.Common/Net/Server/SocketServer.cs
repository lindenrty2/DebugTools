using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections;
using System.IO;
using DebugTools.Net;
using DebugTools.Common.Net.Client;

namespace DebugTools.Common.Net.Server
{

    public class SocketServer : ISocketServer
    {
        Socket _listenSocket = null;
        public ManualResetEvent tcpClientConnected = new ManualResetEvent(false);
        ISocketServerProcessor _process;
        private int _port;

        public SocketServer(int port)
        {
            _port = port;    
        }

        public void SetProcessor(ISocketServerProcessor processor)
        {
            _process = processor;
        }

        public virtual bool Init()
        {
            IPAddress localAddr = IPAddress.Parse("0.0.0.0");
            _listenSocket = new Socket(localAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _listenSocket.Bind(new IPEndPoint(localAddr, _port));
            return true;
        }

        public virtual bool Start()
        {
            if (_listenSocket == null) Init();
            _listenSocket.Listen(1000);
            _process.Process(this);

            return true;
        }

        public virtual bool Stop()
        {
            _listenSocket.Close();
            return true;
        } 

        public void BeginAccept(AsyncCallback asyncCallback)
        {
            this._listenSocket.BeginAccept(asyncCallback, this);
        }

        public ISocketClient EndAccept(IAsyncResult ar)
        {
            return new SocketClient(this._listenSocket.EndAccept(ar));
        }
    }
}

