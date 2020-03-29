using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using DebugTools;

namespace DebugTools.Common
{
    public class CXmlFile : CXmlNode , IXmlFile
    {
        private string _path = string.Empty;
        public string FilePath
        {
            get { return _path; }
            set { _path = value; }
        }

        private string _orginXml = string.Empty;

        public CXmlFile(string path) : base( new XmlDocument())
        {
            _document = (XmlDocument)base._node;
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "<?xml version=\"1.0\"?><configuration/>");
            }
            _document.Load(path); 
            _path = path;
            SetOrginXml();
        }

        public CXmlFile(XmlDocument doc) : base(doc)
        {
            _document = doc;
            SetOrginXml();
        }

        public void SetOrginXml()
        {
            MemoryStream writer = new MemoryStream();
            _document.Save(writer);
            _orginXml = writer.ToString();
        }

        public bool Save()
        {
            _document.Save(_path);
            SetOrginXml();
            return true;
        }

        public bool SaveAs(string path)
        {
            _path = path;
            _document.Save(path);
            SetOrginXml();
            return true;
        }

        public void Close()
        {
            _orginXml = string.Empty;
            _document = null;
        }

    }
}
