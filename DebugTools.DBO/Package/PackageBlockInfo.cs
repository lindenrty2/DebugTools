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
using DebugTools.Package;

namespace DebugTools.DBO
{
    public struct PackageBlockInfo : IBlockInfo
    {
        public int Index { get; set; }

        public long StartPosition { get; set; }

        public int Size { get; set; }

        public short Type { get; set; }

        public PackageBlockInfo(int ix, long sp, int s, PackageBlockType t)
        {
            Index = ix;
            StartPosition = sp;
            Size = s;
            Type = (short)t;
        }
    }
}
