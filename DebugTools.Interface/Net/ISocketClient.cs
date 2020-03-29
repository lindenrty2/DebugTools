using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace DebugTools.Net
{
    public interface ISocketClient
    {
        IPEndPoint ServerPoint { get; }
        Socket Socket { get; }

        void SetProcessor(ISocketClientProcessor processor);
        bool Init(IPEndPoint endPoint);
        bool Connect(int port);
        bool Connect(string ipAddressStr, int port);

        bool Send(DMessage message);
        bool Send(byte[] sendBuffer);
        bool Send(string text);
        IAsyncResult BeginReceive(byte[] p, AsyncCallback asyncCallback, SocketSession session);
        int EndReceive(IAsyncResult ar, out SocketError errorCode);

    }
}
