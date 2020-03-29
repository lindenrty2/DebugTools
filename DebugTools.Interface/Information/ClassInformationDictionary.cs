using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    [Serializable]
    public class ClassInformationDictionary
    {
        private Dictionary<string, Dictionary<string,ClassInformation>> _dictionary;

        public ClassInformation this[string category, string name]
        {
            get
            {
                return Find(category, name);
            } 
        }

        public ClassInformationDictionary()
        {
            _dictionary = new Dictionary<string, Dictionary<string, ClassInformation>>();
        }

        public void Add(ClassInformation info)
        {
            if (!_dictionary.ContainsKey(info.Category))
            {
                _dictionary.Add(info.Category,new Dictionary<string,ClassInformation>());
            }
            Dictionary<string, ClassInformation> classDict = _dictionary[info.Category];
            if (classDict.ContainsKey(info.Name))
            {
                classDict[info.Name] = info;
            }
            else
            {
                classDict.Add(info.Name, info);
            }
        }

        public void Remove(ClassInformation info)
        {
            Remove(info.Category,info.Name );
        }

        public void Remove(string category,string name)
        {
            if (!_dictionary.ContainsKey(category))
            {
                return;
            }
            Dictionary<string, ClassInformation> classDict = _dictionary[category];
            if (classDict.ContainsKey(name))
            {
                classDict.Remove(name);
            }
        }

        public ClassInformation[] Find(string category)
        {
            if (!_dictionary.ContainsKey(category))
            {
                return new ClassInformation[0];
            }
            return _dictionary[category].Values.ToArray();
        }

        public ClassInformation Find(string category,string name)
        {
            if (!_dictionary.ContainsKey(category))
            {
                return null;
            }
            Dictionary<string, ClassInformation> classDict = _dictionary[category];
            if (classDict.ContainsKey(name))
            {
                return classDict[name];
            }
            return null;
        }

        public void Clear()
        {
            _dictionary.Clear();
        }
    }
}
