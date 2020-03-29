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
using DebugTools.Package;

namespace DebugTools.DBO
{
    public class PackageViewInfo : PackageSerializable
    {
        public override PackageBlockType Type
        {
            get
            {
                return PackageBlockType.View;
            }
        }

        // Sub New(package As PackageFile, tableInfo As TableInfo, table As DataTable)
        // MyBase.New(Package)
        // _tableInfo = tableInfo
        // _data = New PackageData(package, table)
        // End Sub

        public PackageViewInfo(PackageFile package, IBlockInfo blockInfo, byte[] bytes) : base(package, blockInfo)
        {
            Load(bytes);
        }

        public override bool Load(byte[] b)
        {
            return true;
        }

        public override byte[] ToByte()
        {
            return new byte[] { };
        }
    }
}
