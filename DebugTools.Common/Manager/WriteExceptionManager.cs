using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DebugTools.Common.Manager
{
    public static class WriteExceptionManager
    {
        public static Boolean OnOrOff = false;

        private static FileStream _file;
        private static Int32 _suffixTag = 0;

        private static string _path = SystemCenter.CurrentApplication.PathManager.GetRootPath() + "\\Exception";
        public static string Path
        {
            get { return _path; }
            set { _path = value; }

        }

        private static string _logFileName;
        public static string LogFileName
        {
            get { return _logFileName; }
            set { _logFileName = value; }
        }

        public static void WriteMessage(byte[] buffer, int bufferContentLength)
        {
            if (!OnOrOff)
            {
                return;
            }
            _logFileName = string.Format("{0} {1} {2}", DateTime.Now.ToString(), DateTime.Now.Millisecond.ToString(), Process.GetCurrentProcess().ProcessName).Replace('/', '_').Replace(':', '_');
            if (!File.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            string fullPathAndName = _path + "\\" + LogFileName + _suffixTag.ToString() + "Receive" + ".txt";
            try
            {
                _file = new FileStream(fullPathAndName, FileMode.Append);
            }
            catch
            {
                _suffixTag++;
                fullPathAndName = _path + "\\" + LogFileName + _suffixTag.ToString() + "Receive" + ".txt";
                _file = new FileStream(fullPathAndName, FileMode.Append);
            }
            _file.Write(buffer, 0, bufferContentLength);
            _file.Dispose();
        }

        public static void WriteMessage(object message)
        {
            if (!OnOrOff)
            {
                return;
            }
            _logFileName = string.Format("{0} {1} {2}", DateTime.Now.ToString(), DateTime.Now.Millisecond.ToString(), Process.GetCurrentProcess().ProcessName).Replace('/', '_').Replace(':', '_');
            string fullPathAndName = _path + "\\" + LogFileName + _suffixTag.ToString() + "Send" + ".txt";
            try
            {
                _file = new FileStream(fullPathAndName, FileMode.Append);
            }
            catch
            {
                _suffixTag++;
                fullPathAndName = _path + "\\" + LogFileName + _suffixTag.ToString() + "Send" + ".txt";
                _file = new FileStream(fullPathAndName, FileMode.Append);
            }
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, message);
            byte[] buffer = stream.GetBuffer();
            _file.Write(buffer, 0, buffer.Length);
            _file.Dispose();
        }
    }
}
