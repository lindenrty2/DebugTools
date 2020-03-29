using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DebugTools
{
    public interface IXmlNode
    {
        XmlNode Node { get; }

        string GetAttributeValue(string name);

        void SetAttributeValue(string name, string value);

        bool GetBooleanAttributeValue(string name);

        void SetBooleanAttributeValue(string name, bool value);

        string GetValue(string xPath);

        string GetValue();

        void SetValue(string xPath, string value);

        void SetValue(string value);

        bool GetBooleanValue(string path);

        void SetBooleanValue(string path, bool value);

        void SetNodeValue(XmlNode node, string value);

        XmlNode GetNode(string xPath);

        T GetNode<T>(string xPath) where T : IXmlNode;

        T AppendNode<T>(string xPath, string name) where T : IXmlNode;

        T AppendNode<T>(string name) where T : IXmlNode;

        T CreateNode<T>(string name) where T : IXmlNode;

        string GetNodeValue(XmlNode node);

        void CreateNode(string xpath);

        T GetSetting<T>(string xpath) where T: IXmlNode ;

        void Attach(IXmlNode node);

        void Delete();
    }
}
