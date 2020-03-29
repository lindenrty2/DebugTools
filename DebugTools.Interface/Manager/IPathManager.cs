using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace DebugTools
{
    public interface IPathManager
    {
        string GetRootPath();
        string GetRootPath(string addPath);
        string GetToolsRootPath();
        string GetToolsRootPath(string addPath);
        string GetSettingPath();
        string GetSettingPath(string fileName);
        string GetSettingPath(string additionPath,string fileName);

    }
}
