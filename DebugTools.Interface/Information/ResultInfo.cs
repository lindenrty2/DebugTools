using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.Information
{

    [Serializable]
    public class ResultInfo<T>
    {
        private T _value;
        public T Value { get { return _value; } }

        private string[] _message;
        public string[] Message { get { return _message; } }

        public ResultInfo(T value) 
        {
            this._value = value;
        }

        public ResultInfo(T value, string message)
            : this(value, new string[] {message} )
        { 
        }

        public ResultInfo(T value, string[] message)
            : this(value)
        {
            this._message = message;
        }
    }
}
