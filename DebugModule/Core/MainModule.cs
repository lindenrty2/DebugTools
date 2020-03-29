using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DebugModule.Function;
using DebugModule.Properties;
using DebugTools.Common.Helper;
using DebugTools.Common.Hook.ExceptionHook;
using DebugTools;

namespace DebugModule.Core
{
    public class MainModule
    {
        public static bool _resumeError = true;
        public static bool _inited = false;
        public static void AttachedMain()
        {
            try
            {
                DeleteInvalidDll();

                if (_inited) return;
                _inited = true;

                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                Assembly.LoadFrom(MainApplication.Current.PathManager.GetToolsRootPath("DebugTools.Common.dll"));

                AttachedMain2();
            }
            catch (Exception e)
            {
                MessageBox.Show("初期化失敗" + e.Message);
            }
        }

        private static void DeleteInvalidDll()
        {
            string[] fileNames = Directory.GetFiles(@MainApplication.Current.PathManager.GetToolsRootPath(), "DebugTools.*.dll");

            foreach (string fileName in fileNames)
            {
                if (fileName.Equals(MainApplication.Current.PathManager.GetToolsRootPath("DebugTools.Common.dll"))) { continue; }
                if (fileName.Equals(MainApplication.Current.PathManager.GetToolsRootPath("DebugTools.dll"))) { continue; }

                if (File.Exists(MainApplication.Current.PathManager.GetToolsRootPath(fileName)))
                {
                    File.Delete(MainApplication.Current.PathManager.GetToolsRootPath(fileName));
                }
            }
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(Assembly assembly in assemblies)
            {
                if (assembly.FullName == args.Name) return assembly;
            }
            return null;
        } 

        public static void AttachedMain2()
        {
            MainApplication.Current.MainTray.Icon = Resources.XMarooon;

            MenuItemInfo itemInfo = new MenuItemInfo();
            itemInfo.Key = "Main_ExceptionRestore";
            itemInfo.Title = "強制的にエラーを復帰する";
            itemInfo.IsCheckMenu = true;
            itemInfo.IsChecked = true;
            itemInfo.CheckedAction = new EventHandler(resumeError_CheckStateChanged);
            MainApplication.Current.MenuManager.AddMenuItem("Main", itemInfo);

            itemInfo = new MenuItemInfo();
            itemInfo.Key = "Main_Spliter1";
            itemInfo.IsSpliter = true;
            MainApplication.Current.MenuManager.AddMenuItem("Main", itemInfo); 

            itemInfo = new MenuItemInfo();
            itemInfo.Key = "Main_ExceptionList";
            itemInfo.Title = "Exception一覧"; 
            itemInfo.ClickAction = new EventHandler(exceptionViewer_Click);
            MainApplication.Current.MenuManager.AddMenuItem("Main", itemInfo);

            itemInfo = new MenuItemInfo();
            itemInfo.Key = "Main_ObjectBrowser";
            itemInfo.Title = "オブジェクトブラウザ";
            itemInfo.ClickAction = new EventHandler(objectViewer_Click);
            MainApplication.Current.MenuManager.AddMenuItem("Main", itemInfo);

            itemInfo = new MenuItemInfo();
            itemInfo.Key = "Main_WindowsViewer";
            itemInfo.Title = "画面監視";
            itemInfo.ClickAction = new EventHandler(focusedElementViewer_Click);
            MainApplication.Current.MenuManager.AddMenuItem("Main", itemInfo);

            itemInfo = new MenuItemInfo();
            itemInfo.Key = "Main_AssmblyViewer";
            itemInfo.Title = "アセンブリ ブラウザ";
            itemInfo.ClickAction = new EventHandler(assembliesViewer_Click);
            MainApplication.Current.MenuManager.AddMenuItem("Main", itemInfo);

            itemInfo = new MenuItemInfo();
            itemInfo.Key = "Main_Spliter2";
            itemInfo.IsSpliter = true;
            MainApplication.Current.MenuManager.AddMenuItem("Main", itemInfo); 

            MainApplication.Current.MainTray.Visible = true;
            MainApplication.Current.MainTray.ShowBalloonTip(2000, "Message",
                "DebugToolsがMegaOak/iSプロセスを監視しています。監視中はアタッチしないでください。", ToolTipIcon.Info);
            MainApplication.Current.MainTray.Text = string.Format("Process名:{0}", Process.GetCurrentProcess().MainModule.ModuleName );

            MainApplication.Current.PluginManager.LoadAll();
            CreateTrayMenu();
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.DispatcherUnhandledException +=
                    new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
                System.Windows.Application.Current.Exit += new System.Windows.ExitEventHandler(Current_Exit);
                System.AppDomain.CurrentDomain.FirstChanceException += new EventHandler<System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs>(CurrentDomain_FirstChanceException);
            }
            else
            {
                System.Windows.Forms.Application.ThreadException +=
                    new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                System.Windows.Forms.Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            }

            MainApplication.Current.ConnectControlCenter();

        } 

        private static void CreateTrayMenu()
        {
            ContextMenuStrip mainMenu = new ContextMenuStrip();
            MenuItemInfo mainInfo = MainApplication.Current.MenuManager.FindMenuItem("Main");
            foreach (MenuItemInfo subInfo in mainInfo.SubMenuInfos)
            { 
                mainMenu.Items.Add(MenuHelper.CreateToolStripMenu(subInfo));
            }
            MenuItemInfo pluginInfo = MainApplication.Current.MenuManager.FindMenuItem("Plugin");
            if (pluginInfo == null) return;
            foreach (MenuItemInfo subInfo in pluginInfo.SubMenuInfos)
            { 
                mainMenu.Items.Add(MenuHelper.CreateToolStripMenu(subInfo));
            }
            MainApplication.Current.MainTray.ContextMenuStrip = mainMenu;
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            MainApplication.Current.MainTray.Visible = false;
        }

        private static void Current_Exit(object sender, System.Windows.ExitEventArgs e)
        {
            MainApplication.Current.MainTray.Visible = false;
        }

        private static void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MainApplication.Current.ExceptionManager.AddUntreated(e.Exception);
            ExceptionViewWindow window = new ExceptionViewWindow();
            window.SetExecption(e.Exception);
            window.ShowDialog();
            if (_resumeError)
            {
                e.Handled = true;
            }
            MainApplication.Current.LogManager.WriteInfo("Main", new DMessage("", e.Exception.Message , e.Exception ));
        }

        static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            MainApplication.Current.ExceptionManager.AddUntreated(e.Exception);
            MainApplication.Current.LogManager.WriteInfo("Main", new DMessage("", e.Exception.Message, e.Exception));
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MainApplication.Current.ExceptionManager.Add(e.Exception);
            ExceptionViewWindow window = new ExceptionViewWindow();
            window.SetExecption(e.Exception);
            window.ShowDialog(); 
        }

        private static void exceptionViewer_Click(object sender, EventArgs e)
        {
            CatchedExecptionView view = new CatchedExecptionView();
            view.Show();
        }

        private static void objectViewer_Click(object sender, EventArgs e)
        {
            ObjectViewer view = new ObjectViewer();
            view.Show();
        }

        private static void focusedElementViewer_Click(object sender, EventArgs e)
        {
            FocusedElementViewer view = new FocusedElementViewer();
            view.Show();
        }

        private static void assembliesViewer_Click(object sender, EventArgs e)
        {
            AssembliesView view = new AssembliesView();
            view.Show();
        }

        private static void resumeError_CheckStateChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            _resumeError = item.Checked;
            //if (_resumeError)
            //{
            //    System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //}
            //else
            //{
            //    System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException );
            //}
        } 
        
    }
}
