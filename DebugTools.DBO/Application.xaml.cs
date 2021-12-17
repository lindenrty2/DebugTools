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
using System.Runtime.InteropServices;
using System.Data;
using System.Security.Cryptography;
using DebugTools.DataBase;
using DebugTools.Package;
using DebugTools.DBO;
using System.Windows;

public partial class Application
{
    MainApplication _innerApp = new MainApplication();
    public Application()
    {
        _innerApp = new MainApplication();
        _innerApp.Init();
        _innerApp.PluginManager.LoadAll();
    }

    public static int GetPackageVersionNo()
    {
        object[] attrs = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(PackageVersionAttribute), false);
        if (attrs.Length == 0)
            return 0;
        PackageVersionAttribute version = (PackageVersionAttribute)attrs[0];
        return version.VerNo;
    }

    private void Application_Startup(object sender, System.Windows.StartupEventArgs e)
    {
        if (e.Args.Length > 0)
        {
            string path = e.Args.First();
            if (!File.Exists(path))
            {
                MessageBox.Show("指定的文件找不到");
                return;
            }
            IDataAccessor accessor = AccessorCenter.Instance.CreateAccessor(path);
            if (accessor == null)
                Application.Current.Shutdown();
            MainTableView window = new MainTableView(accessor);
            window.ShowDialog();
        }
    }
}

