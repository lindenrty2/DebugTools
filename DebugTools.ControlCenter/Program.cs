using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DebugTools.ControlCenter.Manager;
using DebugTools.Common;
using DebugTools.Common.Config;
using DebugTools;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;

namespace DebugTools.ControlCenter
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainApplication.Current.PluginManager.LoadAll();
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            Application.Run(new ControlCenterFrm());
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly.FullName == args.Name) return assembly;
            }
            return null;
        } 
    }
}
