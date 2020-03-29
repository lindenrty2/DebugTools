using DebugTools.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DebugTools
{

    public delegate void WndProcEventHanlder(object sender, DMessage message); 

    public interface IApplication
    {

        AppMode AppMode { get; }

        IClassManager ClassManager { get; }

        IDataAccessorManager DataAccessorManager { get; }
  
        IManagerCreator ManagerCreator { get; }

        IPathManager PathManager { get; }

        IPluginManager PluginManager { get; }

        IConfigManager ConfigManager { get; }

        IExceptionManager ExceptionManager { get; }

        IMenuManager MenuManager { get; }

        ILogManager LogManager { get; }

        IDBManager DBManager { get; }

        IViewManager ViewManager { get; } 

        event WndProcEventHanlder WndProc;

        bool Init();

        void SendMessage(DMessage message);

        void CoreWndProc(DMessage message);

    }
}
