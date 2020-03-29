using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Linq;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Media;
using System.Windows.Controls;
using System;
using System.Xml.Linq;
using System.Windows.Navigation;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml;
using DebugTools.Package;
using DebugTools.Helper;

namespace DebugTools.DBO
{
    public class PackageHeader : PackageSerializable
    {
        public override PackageBlockType Type
        {
            get
            {
                return PackageBlockType.Header;
            }
        }

        public string Name { get; set; }

        public string Keyword { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public string Comment { get; set; }

        public string CreateUserName { get; set; }

        public DateTime CreateTime { get; set; }

        private PackageFile _packageFile;
        /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="package"></param>
    /// <remarks></remarks>
        public PackageHeader(PackageFile package) : base(package)
        {
            this.BlockInfo = new PackageBlockInfo() { StartPosition = Marshal.SizeOf(typeof(PackageMate)) };
        }

        public PackageHeader(PackageFile package, IBlockInfo blockInfo, byte[] bytes) : base(package, blockInfo)
        {
            Load(bytes);
        }

        public override bool Load(byte[] b)
        {
            string str = System.Text.UTF8Encoding.UTF8.GetString(b);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);
            XmlNode headerNode = xmlDoc.FirstChild;
            this.Name = XmlHelper.GetAttributeValue(headerNode, "n");
            this.Keyword = XmlHelper.GetAttributeValue(headerNode, "k");
            this.Tags = XmlHelper.GetAttributeValue(headerNode, "t").Split(',');
            this.Comment = XmlHelper.GetAttributeValue(headerNode, "c");
            this.CreateUserName = XmlHelper.GetAttributeValue(headerNode, "cn");
            this.CreateTime = new DateTime(System.Convert.ToInt64(XmlHelper.GetAttributeValue(headerNode, "ct")));
            return true;
        }

        public override byte[] ToByte()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode headerNode = xmlDoc.CreateNode(XmlNodeType.Element, "Header", "");
            headerNode.Attributes.Append(XmlHelper.CreateAttribute(xmlDoc, "n", Name));
            headerNode.Attributes.Append(XmlHelper.CreateAttribute(xmlDoc, "k", Keyword));
            headerNode.Attributes.Append(XmlHelper.CreateAttribute(xmlDoc, "t", string.Join(",", Tags)));
            headerNode.Attributes.Append(XmlHelper.CreateAttribute(xmlDoc, "c", Comment));
            headerNode.Attributes.Append(XmlHelper.CreateAttribute(xmlDoc, "cn", CreateUserName));
            headerNode.Attributes.Append(XmlHelper.CreateAttribute(xmlDoc, "ct", CreateTime.Ticks));
            xmlDoc.AppendChild(headerNode);

            string xml = xmlDoc.InnerXml;
            return System.Text.UTF8Encoding.UTF8.GetBytes(xml);
        }
    }
}
