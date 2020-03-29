using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools;

namespace DebugTools.Common.Manager
{
    public class PluginInformation
    {
        private IPluginEntry _entry;
        public IPluginEntry Entry
        {
            get { return _entry; }
        }

        private string _path ;
        public string Path
        {
            get { return _path; }
        }
         
        public string Name
        {
            get { return _entry.Name ; }
        }
         
        public string Description
        {
            get { return _entry.Description ; }
        }

        public PluginInformation(IPluginEntry entry ,string path)
        {
            _entry = entry;
            _path = path;
        }


    }
}
