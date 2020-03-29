using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DebugTools
{
    [Serializable]
    public class CodeInfo
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        private string _nameSpace;
        public string NameSpace
        {
            get { return _nameSpace; }
            set { _nameSpace = value; }
        }

        private MemberInfo[] _memberInfos;
        public MemberInfo[] MemberInfos
        {
            get { return _memberInfos; }
            set { _memberInfos = value; }
        }

        public CodeInfo(string code)
            : this(string.Empty, code, new MemberInfo[0])
        {
        }

        public CodeInfo(string name, string code)
            : this(name,code, new MemberInfo[0])
        {
        }

        public CodeInfo(string name,string code, MemberInfo[] memberInfos)
        {
            _name = name;
            _code = code;
            _memberInfos = memberInfos;
        }
    }
}
