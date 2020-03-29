using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    [Serializable]
    public class ObjectInfo
    {
        private object _object;
        public object Object {
            get { return _object; }
        }

        private string _path;
        public string Path 
        {
            get { return _path; }
        }

        public ObjectInfo(object obj ,string path)
        {
            _object = obj;
            _path = path;
        }
    }
}
