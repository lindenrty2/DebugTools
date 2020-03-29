using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using DebugTools;

namespace DebugTools.Common.Manager
{
    public class PathManager : IPathManager
    { 

        public string GetRootPath()
        {
            return Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ) ;
        }

        public string GetRootPath(string addPath)
        {
            return Path.Combine(GetRootPath(), addPath);
        }

        public string GetToolsRootPath()
        {
            return Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
        }

        public string GetToolsRootPath(string addPath)
        {
            return Path.Combine(GetToolsRootPath(), addPath);
        }

        public string GetSettingPath()
        {
            return Path.Combine(GetToolsRootPath(), "Setting");
        }

        public string GetSettingPath(string fileName)
        {
            return Path.Combine(GetSettingPath(), fileName);
        }
        public string GetSettingPath(string additionPath,string fileName)
        {
            return Path.Combine(GetSettingPath(), additionPath, fileName);
        }

    }
}
