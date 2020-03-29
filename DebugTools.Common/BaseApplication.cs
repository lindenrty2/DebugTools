using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools;
using System.Windows.Forms;
using DebugTools.Const;
using DebugTools.Common.Manager;
using DebugTools.DataBase; 

namespace DebugTools.Common
{

    public abstract class BaseApplication : IApplication
    {

        public abstract AppMode AppMode { get; }

        private IClassManager _classManager = null;
        public IClassManager ClassManager
        {
            get
            {
                if (_classManager == null)
                {
                    _classManager = new ClassManager();
                }
                return _classManager;
            }
        }

        private IManagerCreator _managerCreator = null;
        public IManagerCreator ManagerCreator
        {
            get {
                if (_managerCreator == null)
                {
                    _managerCreator = new ManagerCreator(this.ClassManager);
                }
                return _managerCreator; 
            }
        }

        private IPathManager _pathManager = null;
        public IPathManager PathManager
        {
            get
            {
                if (_pathManager == null)
                {
                    _pathManager = new PathManager();
                }
                return _pathManager;
            }
        }

        private IPluginManager _pluginManager = null;
        public IPluginManager PluginManager
        {
            get {
                if (_pluginManager == null)
                {
                    _pluginManager = new PluginManager(this);
                }
                return _pluginManager;
            }
        }

        private IConfigManager _configManager = null;
        public IConfigManager ConfigManager
        {
            get {
                if (_configManager == null)
                {
                    _configManager = new ConfigManager();
                }
                return _configManager; 
            }
        }

        private IDataAccessorManager _dataAccessorManager = null;
        public IDataAccessorManager DataAccessorManager
        {
            get
            {
                if (_dataAccessorManager == null)
                {
                    _dataAccessorManager = new DataBaseAccessorManager(this);
                }
                return _dataAccessorManager;
            }
        }

        private IExceptionManager _exceptionManager = null;
        public IExceptionManager ExceptionManager
        {
            get {
                if (_exceptionManager == null)
                {
                    _exceptionManager = new ExceptionManager();
                }
                return _exceptionManager;
            }
        }

        private IMenuManager _menuManager;
        public IMenuManager MenuManager
        {
            get {
                if (_menuManager == null)
                {
                    _menuManager = new MenuManager();
                }
                return _menuManager; 
            }
        }

        private ILogManager _logManager;
        public ILogManager LogManager
        {
            get
            {
                if (_logManager == null)
                {
                    _logManager = new LogManager();
                }
                return _logManager;
            }
        }

        private IDBManager _dbManager;
        public IDBManager DBManager
        {
            get {
                if (_dbManager == null)
                {
                    _dbManager = this.ManagerCreator.GetInstance<IDBManager>(DebugTools.Const.ClassKeyConst.DATABASE);
                }
                return _dbManager;
            } 
        }

        private IViewManager _viewManager;

        public IViewManager ViewManager
        {
            get
            {
                if (_viewManager == null)
                {
                    _viewManager = new ViewManager();
                }
                return _viewManager;
            }
        }

        public event WndProcEventHanlder WndProc;

        public BaseApplication()
        {
            SystemCenter.CurrentApplication = this;
        }

        public virtual bool Init()
        {
            return true;
        }

        public virtual void SendMessage(DMessage message)
        {
            CoreWndProc(message);
            if (message.Handled) return;
            this.PluginManager.SendMessage(message);
        }

        public virtual void CoreWndProc(DMessage message)
        {
            if (this.WndProc != null) this.WndProc(this, message);
            if (message.Title == CommandConst.WND_STARTMENU)
            {
                 MenuItemInfo info = this.MenuManager.FindMenuItem(message.Message);
                 if (info == null)
                 {
                     LogManager.WriteWarning("无效命令", message);
                     return;
                 }
                 if (info.ClickAction != null)
                 {
                     info.ClickAction(message, null);
                 }
                 if (info.CheckedAction != null)
                 {
                     info.CheckedAction(message, null);
                 }
            }
        }
         
    }
}
