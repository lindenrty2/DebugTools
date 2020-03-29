using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    public class InstanceDictionary
    {
        private Dictionary<string, Dictionary<string,Instance>> _dictionary;

        public Instance this[string category, string name]
        {
            get { return Find(category, name); } 
        }

        public InstanceDictionary()
        {
            _dictionary = new Dictionary<string, Dictionary<string, Instance>>();
        }

        public void Add(Instance info)
        {
            if (!_dictionary.ContainsKey(info.ClassInformation.Category))
            {
                _dictionary.Add(info.ClassInformation.Category, new Dictionary<string, Instance>());
            }
            Dictionary<string, Instance> classDict = _dictionary[info.ClassInformation.Category];
            if (classDict.ContainsKey(info.ClassInformation.Name))
            {
                classDict[info.ClassInformation.Name] = info;
            }
            else
            {
                classDict.Add(info.ClassInformation.Name, info);
            }
        }

        public void Remove(Instance info)
        {
            Remove(info.ClassInformation.Category, info.ClassInformation.Name);
        }

        public void Remove(string category,string name)
        {
            if (!_dictionary.ContainsKey(category))
            {
                return;
            }
            Dictionary<string, Instance> classDict = _dictionary[category];
            if (classDict.ContainsKey(name))
            {
                classDict.Remove(name);
            }
        }

        public Instance[] Find(string category)
        {
            if (!_dictionary.ContainsKey(category))
            {
                return new Instance[0];
            }
            return _dictionary[category].Values.ToArray();
        }

        public Instance Find(string category, string name)
        {
            if (!_dictionary.ContainsKey(category))
            {
                return null;
            }
            Dictionary<string, Instance> classDict = _dictionary[category];
            if (classDict.ContainsKey(name))
            {
                return classDict[name];
            }
            return null;
        }

        public Instance Find(ClassInformation information)
        {
            return Find(information.Category, information.Name);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }
    }
}
