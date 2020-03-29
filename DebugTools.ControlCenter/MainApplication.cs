using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using DebugTools.Common.Manager;
using DebugTools;
using DebugTools.Common;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using DebugTools.Common.Net.Server;
using DebugTools.Net;
using DebugTools.Const;
using DebugTools.ControlCenter.Manager;

namespace DebugTools.ControlCenter
{
    public class MainApplication : BaseApplication
    {
        private static MainApplication _application;
        public static MainApplication Current
        {
            get
            {
                if (_application == null)
                {
                    _application = new MainApplication();
                    _application.Init(); 
                }
                return _application;
            }
        } 

        public override AppMode AppMode
        {
            get { return AppMode.ControlCenter; }
        } 

        private Task _monitoringTask ;
        private SocketServer _socketServer;
        private DebugServerProcessor _processor;
        private List<string> _attachedProcess = new List<string>();

        public MainApplication()
        {
        }

        public override bool Init()
        {
            _processor = new DebugServerProcessor(this);
            _socketServer = new SocketServer(10086);
            _socketServer.SetProcessor(_processor);
            _socketServer.Start();
            _monitoringTask = new Task(new Action(ProcessMonitor));
            _monitoringTask.Start();

            this.ManagerCreator.Regist(CommonConst.ExportService, typeof(ExportServiceManager));
            this.ManagerCreator.Regist(CommonConst.ImportService, typeof(ImportServiceManager)); 

            return base.Init();
        }

        public override void SendMessage(DMessage message)
        {
            if (_processor != null) _processor.SendMessage(message);
            base.SendMessage(message);
        }

        public override void CoreWndProc(DebugTools.DMessage message)
        {
            base.CoreWndProc(message);
            if (message.Title == CommandConst.CLIENT_ONLINE)
            {
                if (!_attachedProcess.Exists(x => x.Equals(message.Message.Substring(4))))
                {
                    _attachedProcess.Add(message.Message.Substring(4));
                }
            }
            if (message.Title == CommandConst.CLIENT_OFFLINE)
            {
                if (_attachedProcess.Exists(x => x.Equals(message.Message)))
                {
                    _attachedProcess.Remove(message.Message);
                }
            }
        }

        private void ProcessMonitor()
        {
            while (true)
            {
                SearchAndAttach();
                Thread.Sleep(3000);
            }
        }

        
	    private void SearchAndAttach() {
            Process[] processes = Process.GetProcesses(); 
            String filter = "MainPortalService";
			for(int i=0;i<processes.Length;i++){
				Process process = processes[i];

                if (_attachedProcess.Exists(x=>x.Equals(process.Id.ToString ())))
                {
                    continue;
                }
				if(!String.IsNullOrWhiteSpace(filter) && process.ProcessName.ToUpper().IndexOf(filter.ToUpper()) == -1){
					continue;
				}
				if(!CanAttachProcess(process)){
					continue;
				}
                Attach(process);
			}

        }

        bool CanAttachProcess(Process process)
        {
			try{
				bool result = false;
				if (Environment.Is64BitOperatingSystem )
				{
                    //一時
					result= true;
				}
				foreach (ProcessModule module in process.Modules)
				{ 
					if (String.Compare(module.ModuleName, "InjectModule.dll", true) == 0)
						return false;
					if (String.Compare(module.ModuleName, "mscoree.dll", true) == 0)
						result = true;
				}
				return result;
			}
			catch(Exception ex)
			{
				System.Console.WriteLine(ex.ToString());
				return false;
			}
        } 

	    private void  Attach(Process process ) { 
            string args = string.Format(" -p {0}",process.Id);
            Process.Start(PathManager.GetToolsRootPath("Tools.DebugTools.exe"),args);
		}

    }
}
