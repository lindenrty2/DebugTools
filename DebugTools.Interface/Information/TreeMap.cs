using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DebugTools
{
    public class TreeMap<T> : TreeMapItem<T>
    {
        public TreeMap() : base(default(T))
        { 
        }
        

    }

    public class TreeMapItem<T> : IEnumerable<KeyValuePair<string, TreeMapItem<T>>>
    {

        private T _value = default(T);
        public T Value
        {
            get { return _value; }
            set {_value = value; }
        }

        private Dictionary<string,TreeMapItem<T>> _subItems;
        
        public TreeMapItem<T> this[string key]
        {
            get { return this.GetSubItem(key); }
            set { this.AddSubItem(key, value); }
        }

        public int SubItemCount
        {
            get { return _subItems.Count; }
        }

        public TreeMapItem(T value)
        {
            _subItems = new Dictionary<string,TreeMapItem<T>>();
            _value = value;
        }

        public TreeMapItem<T> AddSubItem(string key,T t)
        {
            TreeMapItem<T> item = new TreeMapItem<T>(t);
            this._subItems.Add(key,item);
            return item;
        }

        public void AddSubItem(string path,string key, TreeMapItem<T> item)
        {
            TreeMapItem<T> parentItem = FindOrCreateSubItem(path);
            parentItem.AddSubItem(key,item);
        }

        public void AddSubItem(string[] parentKey, string key, TreeMapItem<T> item)
        {
            TreeMapItem<T> parentItem = FindOrCreateSubItem(parentKey);
            parentItem.AddSubItem(key, item);
        }

        public void AddSubItem(string key,TreeMapItem<T> item)
        { 
            this._subItems.Add(key, item); 
        }

        public void RemoveSubItem(string key)
        {
            this._subItems.Remove(key);
        }

        public void RemoveSubItem(string path, string key )
        {
            TreeMapItem<T> parentItem = FindSubItem(path);
            parentItem.RemoveSubItem(key);
        }

        public void RemoveSubItem(string[] parentKey, string key )
        {
            TreeMapItem<T> parentItem = FindSubItem(parentKey);
            parentItem.RemoveSubItem(key);
        }

        public TreeMapItem<T> GetSubItem(string key)
        {
            if (_subItems.ContainsKey(key))
            {
                return _subItems[key];
            }
            return null ;
        }

        public T GetValue(params string[] keys)
        {
            TreeMapItem<T> item = FindSubItem(keys);
            if (item == null) return default(T);
            return item.Value;
        }

        public TreeMapItem<T>[] GetSubItems()
        {
            return this._subItems.Values.ToArray();
        }

        public string[] GetSubKeys()
        {
            return this._subItems.Keys.ToArray();
        }

        public T[] GetSubValues()
        {
            return this._subItems.Values.Select( x=> x.Value ).ToArray();
        }

        public bool ContainsItem(string path)
        {
            return FindSubItem(path) != null;
        }

        public TreeMapItem<T> FindSubItem(string path)
        { 
            string[] nodeNames = path.Split('\\');
            return FindSubItem(nodeNames);
        }

        public TreeMapItem<T> FindSubItem(string[] keys)
        { 
            TreeMapItem<T> currentInfo = this;
            foreach (string key in keys)
            {
                currentInfo = currentInfo.GetSubItem(key);
                if (currentInfo == null) return null;
            }
            if (currentInfo == null) return null;
            return currentInfo;
        }

        public TreeMapItem<T> FindOrCreateSubItem(string path)
        {
            string[] nodeNames = path.Split('\\');
            return FindOrCreateSubItem(nodeNames);
        }

        public TreeMapItem<T> FindOrCreateSubItem(string[] keys)
        {
            TreeMapItem<T> currentInfo = this;
            foreach (string key in keys)
            {
                TreeMapItem<T>  findItem = currentInfo.GetSubItem(key);
                if (findItem == null)
                {
                    findItem = currentInfo.AddSubItem(key,default(T));
                }
                currentInfo = findItem;
            }
            if (currentInfo == null) return null;
            return currentInfo;
        }

        public IEnumerator<KeyValuePair<string, TreeMapItem<T>>> GetEnumerator()
        {
            return this._subItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._subItems.GetEnumerator();
        }
    }
}
