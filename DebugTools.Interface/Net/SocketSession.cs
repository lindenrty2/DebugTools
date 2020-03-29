using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace DebugTools.Net
{
    /// <summary>
    /// Socket通讯情报
    /// </summary>
    public class SocketSession
    {
        private ISocketClient _client;
        public ISocketClient SocketClient
        {
            get { return _client; } 
        }

        private Byte[] _recelveBuffer = null;
        /// <summary>
        /// 接收用Buffer
        /// </summary>
        public byte[] ReceiveBuffer
        {
            get { return _recelveBuffer; }
            set { _recelveBuffer = value; }
        }

        private Byte[] _sendBuffer = null;
        /// <summary>
        /// 发送用Buffer
        /// </summary>
        public byte[] SendBuffer
        {
            get { return _sendBuffer; }
            set { _sendBuffer = value; }
        }

        public SocketSession(ISocketClient socket)
        {
            _client = socket; 
        }

        public void ResetReceiveBuffer()
        {
            _recelveBuffer = new byte[4096];
        }

        public void ResetSendBuffer()
        {
            _sendBuffer = new byte[4096];
        }
    }
}
