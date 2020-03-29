using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DebugTools;

namespace DebugTools.Common
{
    public class CXmlNode : IXmlNode
    {
        protected XmlNode _node = null;
        XmlNode IXmlNode.Node
        {
            get { return _node; }
        }

        protected XmlDocument _document = null;

        public CXmlNode(XmlNode node)
        {
            _node = node;
            if(node is XmlDocument ){
                _document = (XmlDocument)node;
            }else{
                _document = node.OwnerDocument;
            }
        }

        public string GetAttributeValue(string name)
        {
            XmlAttribute attr = _node.Attributes[name];
            if (attr == null)
            {
                return null;
            }
            return attr.Value;
        }

        public void SetAttributeValue(string name, string value)
        {
            XmlAttribute attr = _node.Attributes[name];
            if (attr == null)
            {
                attr = _document.CreateAttribute(name);
                _node.Attributes.Append(attr);
            }
            attr.Value = value;
        }

        public bool GetBooleanAttributeValue(string name)
        {
            string value = GetAttributeValue(name);
            return !string.IsNullOrEmpty(value) && !value.Equals("0") && !value.ToUpper().Equals("FALSE");
        }

        public void SetBooleanAttributeValue(string name, bool value)
        {
            SetAttributeValue(name, value ? "1" : "0");
        }

        public string GetValue(string xPath)
        {
            XmlNode node = _node.SelectSingleNode(xPath);
            if (node == null)
            {
                return null;
            }
            return GetNodeValue(node);
        }

        public string GetValue()
        {
            return GetNodeValue(_node);
        }

        public void SetValue(string xPath,string value)
        {
            XmlNode node = GetNode(xPath);
            this.SetNodeValue(node, value);
        }

        public void SetValue( string value)
        {
            SetNodeValue(_node, value);
        }

        public bool GetBooleanValue(string path)
        {
            string value = GetValue(path);
            return !string.IsNullOrEmpty(value) && !value.Equals("0") && !value.ToUpper().Equals("FALSE");
        }

        public void SetBooleanValue(string path,bool value)
        {
            SetValue(path, value ? "1" : "0");
        }

        public void SetNodeValue(XmlNode node, string value)
        {
            if (node.NodeType == XmlNodeType.Attribute)
            {
                node.Value = value;
            }
            else if (node.NodeType == XmlNodeType.Element )
            {
                node.InnerText = value;
            }
        }

        public XmlNode GetNode(string xPath)
        {
            XmlNode node = _node.SelectSingleNode(xPath);
            if (node == null)
            {
                CreateNode(xPath);
                node = _node.SelectSingleNode(xPath);
            }
            return node;
        }

        public T GetNode<T>(string xPath) where T : IXmlNode 
        {
            XmlNode node = GetNode(xPath);
            return (T)Activator.CreateInstance(typeof(T), node);
        }

        public CXmlNodeList<T> GetNodeList<T>(string xPath) where T : IXmlNode
        {
            string nodeName = xPath.Split('\\').Last();
            CXmlNodeList<T> xmlNodes = new CXmlNodeList<T>(this,nodeName);
            XmlNodeList nodes = _node.SelectNodes(xPath);
            foreach (XmlNode node in nodes)
            {
                T t = (T)Activator.CreateInstance(typeof(T), node);
                xmlNodes.AddExist(t);
            }
            return xmlNodes;
        }

        public T AppendNode<T>(string xPath, string name) where T : IXmlNode
        {
            XmlNode node = GetNode(xPath);
            XmlNode newNode = _document.CreateElement(name);
            node.AppendChild(newNode);
            return (T)Activator.CreateInstance(typeof(T), newNode);
        }

        public T AppendNode<T>(string name) where T : IXmlNode
        {
            XmlNode newNode = _document.CreateElement(name);
            _node.AppendChild(newNode);
            return (T)Activator.CreateInstance(typeof(T), newNode);
        }

        public T CreateNode<T>(string name) where T : IXmlNode
        {
            XmlNode newNode = _document.CreateElement(name); 
            return (T)Activator.CreateInstance(typeof(T), newNode);
        }

        public string GetNodeValue(XmlNode node)
        {
            if (node.NodeType == XmlNodeType.Attribute)
            {
                return node.Value;
            }
            else if (node.NodeType == XmlNodeType.Element)
            {
                return node.InnerText;
            }
            return node.InnerText;
        }

        public void CreateNode(string xpath)
        {
            XmlNode currentNode = _node;
            string[] nodeNames = xpath.Split('/');
            for (int i = 0; i < nodeNames.Length; i++)
            {
                string currentNodeName = nodeNames[i];
                if (currentNodeName.Trim().Length == 0)
                {
                    continue;
                }
                bool isAttribute = false;
                if(currentNodeName.StartsWith("@")){
                    currentNodeName = currentNodeName.Substring(1);
                    isAttribute = false; 
                }
                XmlNode workingNode = currentNode[currentNodeName];
                if (workingNode == null)
                {
                    if(isAttribute){
                        workingNode = _document.CreateAttribute(currentNodeName);
                        currentNode.Attributes.Append((XmlAttribute)workingNode);
                    }
                    else{
                        workingNode = _document.CreateElement(currentNodeName,"");
                        currentNode.AppendChild(workingNode);
                    }
                }
                currentNode = workingNode; 
            } 
        }

        public T GetSetting<T>(string xpath) where T : IXmlNode
        {
            XmlNode node = _node.SelectSingleNode(xpath);
            if (node == null)
            {
                CreateNode(xpath);
            }
            return (T)Activator.CreateInstance(typeof(T), node);
        }

        public void Attach(IXmlNode node)
        {
            _node.AppendChild(node.Node);
        }

        public void Delete()
        {
            _node.ParentNode.RemoveChild(_node);
        }
         
    }
}
