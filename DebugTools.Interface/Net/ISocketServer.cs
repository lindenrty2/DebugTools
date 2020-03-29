using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace DebugTools.Net
{
    public interface ISocketServer
    {

        void SetProcessor(ISocketServerProcessor processor);

        bool Init();

        bool Start();

        bool Stop();
        
        void BeginAccept(AsyncCallback asyncCallback);
         
        ISocketClient EndAccept(IAsyncResult ar);
         
    }
}
