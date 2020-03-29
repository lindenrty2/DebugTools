using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DebugTools.Common.Helper
{
    public class FileSystemHelper
    {

        public static bool IsValidFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return false;
            if (string.IsNullOrWhiteSpace(Path.GetFileName(filePath))) return false;
            if (!Path.IsPathRooted(filePath)) return false;
            return true;
        }

        public static bool IsVaildDirectory(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath)) return false;
            if (string.IsNullOrWhiteSpace(Path.GetDirectoryName(directoryPath))) return false;
            if (!Path.IsPathRooted(directoryPath)) return false;
            return true;   
        }

        public static string GetSafePath(string fileName)
        {
            if (fileName == null) return string.Empty;
            foreach(char c in Path.GetInvalidPathChars()){
                fileName = fileName.Replace(c,'_');
            }
            return fileName;
        }
    }
}
