using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    [Serializable]
    public class ClassInformation
    {
        private string _category = string.Empty;
        public string Category {
            get { return _category; } 
        }

        private string _name = string.Empty;
        public string Name { 
            get { return _name; } 
        }

        private Type _classType = null;
        public Type ClassType { 
            get { return _classType; } 
        }

        private bool _isSingle = false;
        public bool IsSingle
        {
            get { return _isSingle; }
        }

        public ClassInformation(string category, Type classType)
            : this(category,classType.Name,classType,false )
        {

        }

        public ClassInformation(string category, Type classType,bool isSingle)
            : this(category,classType.Name,classType,isSingle )
        {

        }

        public ClassInformation(string category, string name, Type classType)
            : this(category, name, classType, false)
        {

        }

        public ClassInformation(string category, string name, Type classType,bool isSingle)
        {
            _category = category;
            _name = name;
            _classType = classType;
            _isSingle = isSingle;
        }

    }
}
