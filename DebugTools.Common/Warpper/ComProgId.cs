using System;

namespace DebugTools.Common
{
    public class ComProgIdAttribute : Attribute 
    {
        private string _progId;
        public string ProgId{
            get{
                return _progId;
            }
        }

        public ComProgIdAttribute(string progId)
        {
            _progId = progId;
        }
    }
}
