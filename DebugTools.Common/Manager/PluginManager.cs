using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DebugTools;
using DebugTools;
using System.Reflection;

namespace DebugTools.Common.Manager
{
    public class PluginManager : IPluginManager 
    {
        private List<PluginInformation> _pluginInfoList = null;
        public List<PluginInformation> PluginInfoList
        {
            get
            {
                return _pluginInfoList;
            }
        }
        private IApplication _app;

        public PluginManager(IApplication app)
        {
            _pluginInfoList = new List<PluginInformation>();
            _app = app;
        }

        public void LoadAll()
        {
            
            string[] files = Directory.GetFiles(_app.PathManager.GetRootPath("Plugins"));
            foreach (string file in files)
            {
                if (!file.ToUpper().EndsWith(".DLL"))
                {
                    continue;
                }
                IPluginEntry pluginEntry = null;
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (IsPluginEntry(type))
                        {
                            pluginEntry = (IPluginEntry)type.Assembly.CreateInstance(type.FullName);
                            break;
                        }
                    }
                    if (pluginEntry == null)
                    {
                        continue;
                    }
                    PluginInformation pluginInfo = new PluginInformation(pluginEntry, file);
                    try
                    {
                        if(!pluginInfo.Entry.Support(_app)) continue ;
                        if(!pluginInfo.Entry.Connection(_app)) continue ;
                        _pluginInfoList.Add(pluginInfo);
                    }
                    catch (Exception ex)
                    {
                        //System.Windows.Forms.MessageBox.Show(ex.Message, pluginInfo.Name );
                        _app.LogManager.WriteError("", new DMessage(ex.Message, ex.ToString()));
                    } 
                }
                catch (Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show(ex.ToString(), ex.Message);
                    _app.LogManager.WriteError("", new DMessage(ex.Message, ex.ToString()));
                }
            }
        }

        private bool IsPluginEntry(Type type)
        {
            return type.GetCustomAttributes(typeof(PluginEntryAttribute), true).Length > 0;
        }

        public void SendMessage(DMessage message)
        {
            foreach (PluginInformation info in _pluginInfoList)
            {
                info.Entry.WndProc(message);
                if (message.Handled)
                {
                    return;
                }
            }
        }
         
    }
}
