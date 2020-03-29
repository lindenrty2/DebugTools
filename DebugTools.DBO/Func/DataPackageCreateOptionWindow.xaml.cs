using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using DebugTools.Package;
using System.Windows;

namespace DebugTools.DBO
{
    public partial class DataPackageCreateOptionWindow
    {
        private IPackageExporter _exporter = null;
        public DataPackageCreateOptionWindow(IPackageExporter exporter)
        {
            _exporter = exporter;
            InitializeComponent();
        }

        private void Window_Loaded(System.Object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void ctlOutputPathSelect_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = ctlOutputPath.Text;
            dialog.Filter = "(データパッケージファイル *.dpkt)|*.dpkt";
            if (dialog.ShowDialog() == true)
                ctlOutputPath.Text = dialog.FileName;
        }

        private void ctlSave_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ctlOutputPath.Text))
            {
                MessageBox.Show("请指定输出路径。");
                return;
            }
            PackageExportOption op = new PackageExportOption();
            op.Path = ctlOutputPath.Text;
            // op.KeyPath = PathHelper.GetPrivateKeyPath("marooon")
            if (_exporter.Export(op))
            {
                MessageBox.Show("输出成功");
                this.DialogResult = true;
            }
        }

        private void ctlCancel_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }

}