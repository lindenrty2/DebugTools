using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools;

namespace DebugTools
{
    [Serializable]
    public class Message : IMessage
    {
        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
        }

        private string _detail = string.Empty;
        public string Detail
        {
            get { return _detail; }
        }

        private MessageType _type;
        public MessageType Type
        {
            get { return _type; }
        }

        public Message(string title, string detail)
            : this(title, detail,MessageType.Information )
        {
        }

        public Message(string title,string detail,MessageType type)
        {
            _title = title;
            _detail = detail;
            _type = type;
        }
    }
}
