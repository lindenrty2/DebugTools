using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Reflection;
using DebugTools;

namespace DebugTools.Common
{
    public class ClassManager : IClassManager
    {
        private ClassInformationDictionary _classDictionary = new ClassInformationDictionary();
        private InstanceDictionary _instanceDictionary = new InstanceDictionary();

        public void Regist(ClassInformation information)
        {
            _classDictionary.Add(information);
        }

        public void Regist(string category,string name,Type type)
        {
            _classDictionary.Add(new ClassInformation(category,name,type));
        }

        public void RegistSingle(string category, string name, Type type)
        {
            _classDictionary.Add(new ClassInformation(category, name, type, true));
        }

        public void Unregist(ClassInformation information)
        {
            _classDictionary.Remove(information);
        }

        public void Unregist(string category,string name)
        {
            _classDictionary.Remove(category, name);
        }

        public ClassInformation Find(string category,string name)
        {
            return _classDictionary.Find(category, name);
        }

        public ClassInformation[] Find(string category)
        {
            return _classDictionary.Find(category);
        }

        public T GetInstance<T>(string category,string name,params object[] parameters)
        {
            ClassInformation information = Find(category, name);
            return GetInstance<T>(information,parameters);
        }

        public T[] GetInstance<T>(string category, params object[] parameters)
        {
            ClassInformation[] informations = Find(category);
            List<T> objects = new List<T>();
            foreach (ClassInformation classInformation in informations)
            {
                T obj = GetInstance<T>(classInformation,parameters);
                if (obj != null)
                {
                    objects.Add(obj);
                }
            }
            return objects.ToArray();
        } 

        public T GetInstance<T>(ClassInformation information, params object[] parameters)
        { 
            T obj = default(T);
            if (information.IsSingle)
            {
                Instance instance = _instanceDictionary.Find(information);
                if (instance != null) return (T)instance.Object;
            }
            obj = (T)information.ClassType.Assembly.CreateInstance(
                information.ClassType.FullName, false, BindingFlags.Default | BindingFlags.CreateInstance, null, parameters, null, null);
           
            if (obj != null && information.IsSingle)
            {
                _instanceDictionary.Add(new Instance(information, obj));
            }
            return obj;
        } 

        public void Reset()
        {
            _classDictionary.Clear();
            _instanceDictionary.Clear();
        }
    }
}
