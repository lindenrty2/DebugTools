using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace DebugTools.Common
{
    public class RegistryHelper 
    {

        public static string GetKeyValue(string path, string name)
        {
            return GetKeyValue(path,name,null);
        }

        public static string GetKeyValue(string path, string name, string defaultValue)
        {
            return (string)Registry.GetValue(path, name, defaultValue);
        }

        public static bool SetKeyValue(string path, string name, string value)
        {
            Registry.SetValue(path,name,value);
            return true;
        }

        public static bool DeleteKey(string path, string keyName)
        { 
            RegistryKey key = Registry.LocalMachine.OpenSubKey(path, false);
            if(key != null){
                key.DeleteSubKeyTree(keyName);
            }
            return true;
        }


    }
}
