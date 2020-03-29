using System.Diagnostics;
using DebugTools.DataBase;

namespace DebugTools.DBO
{
    public class PathHelper
    {
        public static string GetCustomInfoPath(InfoType type, string name)
        {
            return System.IO.Path.Combine(GetAppPath(), "Default", GetTypeFolderName(type), GetTypeFileName(type, name));
        }

        public static string GetOutputInfoPath(string targetDir, InfoType type, string name)
        {
            return System.IO.Path.Combine(targetDir, GetTypeFolderName(type), GetTypeFileName(type, name));
        }

        public static string GetPublicKeyPath(string name)
        {
            return System.IO.Path.Combine(GetAppPath(), "key", "public", name + ".dpbk");
        }

        public static string GetPrivateKeyPath(string name)
        {
            return System.IO.Path.Combine(GetAppPath(), "key", "private", name + ".dpvk");
        }

        private static string GetTypeFolderName(InfoType type)
        {
            if ((int)type == (int)InfoType.TableInfo)
                return "TableInfo";
            else if ((int)type == (int)InfoType.TableData)
                return "TableData";
            else if ((int)type == (int)InfoType.PackageInfo)
                return string.Empty;
            else
                return string.Empty;
        }

        private static string GetTypeFileName(InfoType type, string name)
        {
            if ((int)type == (int)InfoType.TableInfo)
                return name + ".ti";
            else if ((int)type == (int)InfoType.TableData)
                return name + ".data";
            else if ((int)type == (int)InfoType.PackageInfo)
                return name + ".pkgi";
            else
                return name + ".xml";
        }

        public static string GetAppPath()
        {
            return System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        }
    }
}
