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
using DebugTools.DataBase; 
using DebugTools;

namespace DebugTools.DBO
{
    public class AccessorCenter
    {
        private static AccessorCenter _instance;
        public static AccessorCenter Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AccessorCenter();
                return _instance;
            }
        }

        private Dictionary<string, IDataAccessor> _dataAccessorList = new Dictionary<string, IDataAccessor>();

        private AccessorCenter()
        {
        }

        public IDataAccessor GetAccessor(DBConnectInfo dbInfo)
        {
            if (_dataAccessorList.ContainsKey(dbInfo.Key))
                return _dataAccessorList[dbInfo.Key];
            else
            {
                IDataAccessor newAccessor = CreateAccessor(dbInfo);
                if (newAccessor == null)
                    return null;
                _dataAccessorList.Add(dbInfo.Key, newAccessor);
                return newAccessor;
            }
        }

        public IDataAccessor CreateAccessor(DBConnectInfo dbInfo)
        {
            try
            {
                IDataAccessor accessor = SystemCenter.CurrentApplication.DataAccessorManager.Create(dbInfo); 
                if (!accessor.Connect())
                    return null;
                return accessor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public IDataAccessor CreateAccessor(string path)
        {
            try
            {
                PackageDataAccessor accessor = new PackageDataAccessor(path);
                if (!accessor.Connect())
                    return null;
                return accessor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
