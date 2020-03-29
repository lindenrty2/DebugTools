using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace DebugTools.Net
{
    public interface ISocketClientProcessor
    {

        void Process(ISocketClient socket);

        SocketSession CreateSession(ISocketClient socket);

    }
}
