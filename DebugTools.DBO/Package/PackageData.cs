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
using System.Data;
using System.Xml;
using System.IO;
using DebugTools.Package;

namespace DebugTools.DBO
{
    public class PackageData : PackageSerializable
    {
        public override PackageBlockType Type
        {
            get
            {
                return PackageBlockType.Data;
            } 
        }

        private DataTable _tableData;
        public DataTable TableData
        {
            get
            {
                return _tableData;
            }
        }

        public PackageData(PackageFile package, DataTable tableData) : base(package)
        {
            _tableData = tableData;
        }

        public PackageData(PackageFile package, IBlockInfo blockInfo, byte[] bytes) : base(package, blockInfo)
        {
            Load(bytes);
        }

        public override bool Load(byte[] b)
        {
            try
            {
                MemoryStream stream = new MemoryStream(b);
                DataSet tableData = new DataSet();
                tableData.ReadXml(stream);
                stream.Close();
                _tableData = tableData.Tables[0];
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public override byte[] ToByte()
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                _tableData.WriteXml(stream);
                var bytes = stream.ToArray();
                stream.Close();
                return bytes;
            }
            catch (Exception ex)
            {
                return new byte[] { };
            }
        }
    }
}
