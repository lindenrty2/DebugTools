using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    public class Instance
    {
        private DateTime _createDateTime;
        public DateTime CreateDateTime
        {
            get
            {
                return _createDateTime;
            }
        }

        private ClassInformation _information;
        public ClassInformation ClassInformation
        {
            get
            {
                return _information;
            }
        }

        private object _object;
        public object Object
        {
            get
            {
                return _object;
            }
        }

        public Instance(ClassInformation information,object obj)
        {
            _information = information;
            _object = obj;
            _createDateTime = DateTime.Now;
        }

    }
}
