using System.Net;
using System.Net.Sockets;
using DebugTools.Net;
using System;
using DebugTools;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace DebugTools.Common.Net.Client
{
    public class SocketClient : ISocketClient
    {
        private IPEndPoint _serverPoint;
        public IPEndPoint ServerPoint
        {
            get { return _serverPoint; }
        }

        private Socket _clientSocket;
        public Socket Socket
        {
            get { return _clientSocket; }
        }

        private ISocketClientProcessor _processor ;

        public SocketClient()
        {

        }

        public SocketClient(Socket socket)
        {
            _clientSocket = socket;
        }


        public void SetProcessor(ISocketClientProcessor processor)
        {
            _processor = processor;
        }


        public virtual bool Init(IPEndPoint endPoint)
        {
            _clientSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            return true;
        } 

        public virtual bool Connect(int port)
        {
            return Connect("127.0.0.1", port);
        }

        public virtual bool Connect(string ipAddressStr, int port)
        {
            IPAddress ipAddress = IPAddress.Parse(ipAddressStr);
            _serverPoint = new IPEndPoint(ipAddress, port);
            Init(_serverPoint);
            try
            {
                _clientSocket.Connect(_serverPoint);
                _processor.Process(this);
                return true;
            }
            catch 
            { 
                return false;
            }
        }

        public virtual bool Send(DMessage message)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, message);
            return Send(stream.GetBuffer());
        }

        public virtual bool Send(String text)
        {
            return Send(System.Text.UTF8Encoding.UTF8.GetBytes(text));
        }

        public virtual bool Send(byte[] sendBuffer)
        {
            try
            {
                _clientSocket.Send(sendBuffer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IAsyncResult BeginReceive(byte[] buffer, AsyncCallback asyncCallback, SocketSession session)
        {
            if (!_clientSocket.Connected) { return null; }
            return this._clientSocket.BeginReceive(
                buffer, 0, buffer.Length, SocketFlags.None, asyncCallback, session);
        }
          
        public int EndReceive(IAsyncResult ar, out SocketError errorCode)
        {
            return this._clientSocket.EndReceive(ar, out errorCode);
        }
    }
}
