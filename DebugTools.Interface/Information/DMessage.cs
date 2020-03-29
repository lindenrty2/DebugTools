using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    [Serializable]
    public class DMessage
    {

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _message; 
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private Dictionary<string,string> _args;
        public Dictionary<string, string> Args
        {
            get { return _args; } 
        }

        private object _tag;
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        private bool _handled;
        public bool Handled
        {
            get { return _handled; }
            set { _handled = value; }
        }

        public DMessage(string title,string message)
            : this(title,message,null)
        { 
        }

        public DMessage(string title,string message,object tag)
        {
            _title = title;
            _message = message;
            _tag = tag;
            _args = new Dictionary<string, string>();
        }

        public void SetArg(string key, string value)
        {
            if (!_args.ContainsKey(key))
            {
                _args.Add(key,value);
            }
            _args[key] = value;
        }

        public string GetArg(string key)
        {
            if (!_args.ContainsKey(key))
            {
                return string.Empty;
            }
            return _args[key];
        }
    }
}
