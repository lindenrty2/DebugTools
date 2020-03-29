using DebugTools.Common;
using DebugTools.DataBase;
using DebugTools.Helper;
using DebugTools.Package;
using System;
using System.Data;
using System.Xml;

namespace DebugTools.DBO
{
    public class PackageTableInfo : PackageSerializable
    {
        public override PackageBlockType Type
        {
            get
            {
                return PackageBlockType.Table;
            }
        }

        private ITableInfo _tableInfo;
        public ITableInfo TableInfo
        {
            get
            {
                return _tableInfo;
            }
        }

        private int _dataBlockIndex = -1;
        public int DataBlockIndex
        {
            get
            {
                return _dataBlockIndex;
            }
        }

        private PackageData _data;
        public PackageData Data
        {
            get
            {
                if (_data == null)
                    _data = Package.ReadElement<PackageData>(_dataBlockIndex);
                return _data;
            }
        }

        public PackageTableInfo(PackageFile package, ITableInfo tableInfo, DataTable table) : base(package)
        {
            _tableInfo = tableInfo;
            _data = new PackageData(package, table);
        }

        public PackageTableInfo(PackageFile package, IBlockInfo blockInfo, byte[] bytes) : base(package, blockInfo)
        {
            Load(bytes);
        }

        public override bool Load(byte[] b)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string xmlStr = System.Text.UTF8Encoding.UTF8.GetString(b);
            xmlDoc.LoadXml(xmlStr);
            _tableInfo = new TableInfo(Package.DataAccessor, xmlDoc.FirstChild);
            _dataBlockIndex = System.Convert.ToInt32(XmlHelper.GetAttributeValue(xmlDoc.FirstChild, "DataBlockIndex"));
            return true;
        }

        public override byte[] ToByte()
        {
            if ((_tableInfo) is IXmlSerializable)
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode tableNode = xmlDoc.CreateNode(XmlNodeType.Element, "Table", "");
                if (this.Data != null)
                    tableNode.Attributes.Append(XmlHelper.CreateAttribute(xmlDoc, "DataBlockIndex", this.Data.BlockIndex));

                ((IXmlSerializable)_tableInfo).Write(tableNode);
                xmlDoc.AppendChild(tableNode);

                string xml = xmlDoc.OuterXml;
                return System.Text.UTF8Encoding.UTF8.GetBytes(xml);
            }
            else
                throw new NotSupportedException();
        }
    }
}
