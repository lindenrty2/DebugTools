using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools;
using System.Collections;

namespace DebugTools.Common
{
    public class CXmlNodeList<T> : IEnumerable<T> where T : IXmlNode
    {
        private CXmlNode _parentNode;
        private List<T> _nodeList;
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public CXmlNodeList(CXmlNode parentNode,string name)
        {
            _name = name;
            _parentNode = parentNode;
            _nodeList = new List<T>();
        }

        public T Create() 
        {
            T t = _parentNode.AppendNode<T>(_name); 
            _nodeList.Add(t);
            return t;
        }

        public void AddExist(T t)
        {
            _nodeList.Add(t); 
        }

        public void AddExistRange(IEnumerable <T> ts)
        {
            this.AddExistRange(ts);
        }

        public void Remove(T t)
        {
            _nodeList.Remove(t);
        }

        public void Remove(int index)
        {
            _nodeList.RemoveAt(index);
        }

        public void Clear()
        {
            _nodeList.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this._nodeList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._nodeList.GetEnumerator();
        }
    }
}
