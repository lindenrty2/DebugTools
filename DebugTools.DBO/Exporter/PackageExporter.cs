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
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using DebugTools.DataBase;
using DebugTools.Package;
using DebugTools.Secret;

namespace DebugTools.DBO
{
    public class PackageExporter : IPackageExporter
    {
        public event OnTargetUpdatedEventHandler OnTargetUpdated;

        public delegate void OnTargetUpdatedEventHandler(IPackageExporter sender, PackageEventArgs args);

        public event OnTargetAddedEventHandler OnTargetAdded;

        public delegate void OnTargetAddedEventHandler(IPackageExporter sender, PackageEventArgs args);

        public event OnTargetRemovedEventHandler OnTargetRemoved;

        public delegate void OnTargetRemovedEventHandler(IPackageExporter sender, PackageEventArgs args);

        public event OnTargetClearedEventHandler OnTargetCleared;

        public delegate void OnTargetClearedEventHandler(IPackageExporter sender);

        private PackageExportContainer _container = new PackageExportContainer();
        private IDataAccessor _dataAccessor;
        public IDataAccessor DataAccessor
        {
            get
            {
                return _dataAccessor;
            }
        }

        public PackageExporter(IDataAccessor dataAccessor)
        {
            _dataAccessor = dataAccessor;
        }

        public void AddTargets(IEnumerable<DataRow> rows)
        {
            _container.Add(rows);
            PackageEventArgs args = new PackageEventArgs(PackageEventType.Add, rows);
            OnTargetAdded?.Invoke(this, args);
            OnTargetUpdated?.Invoke(this, args);
        }

        public void RemoveTargets(IEnumerable<DataRow> rows)
        {
            _container.Remove(rows);
            PackageEventArgs args = new PackageEventArgs(PackageEventType.Remove, rows);
            OnTargetRemoved?.Invoke(this, args);
            OnTargetUpdated?.Invoke(this, args);
        }

        public void ClearTargets()
        {
            _container.Clear();
            PackageEventArgs args = new PackageEventArgs(PackageEventType.Clear, new DataRow[] { });
            OnTargetCleared?.Invoke(this);
            OnTargetUpdated?.Invoke(this, args);
        }

        public System.Data.DataSet GetTargetData()
        {
            return _container.GetData();
        }

        public bool Export(PackageExportOption packageOption)
        {
            if (File.Exists(packageOption.Path))
                File.Delete(packageOption.Path);
            try
            {
                FileStream stream = File.OpenWrite(packageOption.Path);
                ISecretService secretService = null;
                if (!string.IsNullOrEmpty(packageOption.KeyPath))
                    secretService = new RSASecretService(packageOption.KeyPath);
                PackageFile package = new PackageFile(this.DataAccessor);
                package.Header.Tags = new List<string>();
                DataSet targetData = this._container.GetData();

                foreach (DataTable table in targetData.Tables)
                {
                    ITableInfo tableInfo = _dataAccessor.GetTableInfo("", table.TableName);
                    package.Add(new PackageTableInfo(package, tableInfo, table));
                }
                package.Write(stream, secretService);
                stream.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
