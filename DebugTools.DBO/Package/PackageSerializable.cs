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
    public abstract class PackageSerializable
    {
        public IBlockInfo BlockInfo { get; set; }

        public abstract PackageBlockType Type { get; }

        private PackageFile _package;
        public PackageFile Package
        {
            get
            {
                return _package;
            }
        }

        private int _blockIndex;
        public int BlockIndex
        {
            get
            {
                return _blockIndex;
            }
        }

        public PackageSerializable(PackageFile package)
        {
            this._package = package;
            this.BlockInfo = new PackageBlockInfo();
            this._blockIndex = _package.GetNextBlockIndex();
        }

        public PackageSerializable(PackageFile package, IBlockInfo bi)
        {
            this._package = package;
            this.BlockInfo = bi;
            this._blockIndex = bi.Index;
        }

        public abstract byte[] ToByte();

        public abstract bool Load(byte[] b);
    }
}
