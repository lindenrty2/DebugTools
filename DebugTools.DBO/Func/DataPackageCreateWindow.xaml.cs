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
using System.Data;
using DebugTools.Package;

namespace DebugTools.DBO
{
    public partial class DataPackageCreateWindow
    {
        private IPackageExporter _exporter = null;

        // Public Sub New()
        // _exporter = DataCenter.GetSharePackageExporter()
        // InitializeComponent()
        // End Sub

        public DataPackageCreateWindow(IPackageExporter exporter)
        {
            _exporter = exporter;
            InitializeComponent();
        }

        private void DataPackageCreateWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Update();
        }

        public void Update()
        {
            DataSet data = _exporter.GetTargetData();
            this.ctlTableList.AutoGenerateColumns = false;
            this.ctlTableList.ItemsSource = null;
            this.ctlTableList.ItemsSource = data.Tables;
        }

        private void ctlTableList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DataTable selectedTable = (DataTable)ctlTableList.SelectedValue;

            this.ctlDataList.AutoGenerateColumns = true;
            ctlDataList.DataContext = null;
            ctlDataList.DataContext = selectedTable;
        }

        private void ctlSave_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            DataPackageCreateOptionWindow optionWindow = new DataPackageCreateOptionWindow(_exporter);
            optionWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            if (optionWindow.ShowDialog() == true)
                this.Close();
        }

        private void ctlCancel_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }

}