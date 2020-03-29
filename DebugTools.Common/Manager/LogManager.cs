using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools;
using System.IO;
using System.Diagnostics;

namespace DebugTools.Common.Manager
{

    public class LogManager : ILogManager
    {
        private FileStream _file;
        private StreamWriter _writer;
        private static Int32 _nameSeq = 0;

        private int _sizeLimit = 0;
        public int SizeLimit
        {
            get { return _sizeLimit; }
            set { _sizeLimit = value; }
        }

        private int _timeSpan = 0;
        public int TimeSpan
        {
            get { return _timeSpan; }
            set { _timeSpan = value; }
        }

        private bool _autoSave = true;
        public bool AutoSave
        {
            get { return _autoSave; }
            set { _autoSave = value; }
        }

        private string _logFileName = string.Format("{0:yyyy_MM_dd}_{1}", DateTime.Now,Process.GetCurrentProcess().ProcessName);
        public string LogFileName
        {
            get { return _logFileName; }
            set { _logFileName = value; }
        }

        public event LogAppendedEventHanlder LogAppended;

        private Dictionary<string, LogBlock> _logBlockMap = null;
        public LogManager()
        {
            string LogFolderPath = SystemCenter.CurrentApplication.PathManager.GetRootPath("Log");
            string path = LogFolderPath + "\\" + LogFileName+_nameSeq.ToString() + ".txt";
            if (!File.Exists(LogFolderPath))
            {
                Directory.CreateDirectory(LogFolderPath);
            }
            try
            {
                _file = new FileStream(path, FileMode.Append);
            }
            catch
            {
                _nameSeq++;
                path = LogFolderPath + "\\" + LogFileName + _nameSeq.ToString() + ".txt";
                _file = new FileStream(path, FileMode.Append);
            }
            _writer = new StreamWriter(_file, System.Text.Encoding.GetEncoding("utf-8"));
            _writer.AutoFlush = true;

            _logBlockMap = new Dictionary<string, LogBlock>();
        }

        ~LogManager()
        {
            _file.Close();
        }

        public void WriteInfo(string category, DMessage info)
        {
            GetLogBlock(category).AddInfoMessage(info);
            DoLogAppended(info);
            WritePermissionLog(info.Message);
          
        }

        public void WriteWarning(string category, DMessage warning)
        {
            GetLogBlock(category).AddWarningMessage(warning);
            DoLogAppended(warning);
        }

        public void WriteError(string category, DMessage error)
        {
            GetLogBlock(category).AddErrorMessage(error);
            DoLogAppended(error);
        }

        private void DoLogAppended(DMessage message)
        {
            if (LogAppended != null)
            {
                LogAppended(message);
            }
        }

        private void WritePermissionLog(string message)
        {
            _writer.WriteLine(message);
        }

        public bool Save()
        {
            return true;
        }

        public bool Save(string path)
        {
            //TODO 保存機能
            return true;
        }

        public void Clear()
        {
            _logBlockMap.Clear();
        }

        public void Clear(string category)
        {
            if (!_logBlockMap.ContainsKey(category)) return;
            _logBlockMap[category].Clear();
        }

        private LogBlock GetLogBlock(string category)
        {
            if (_logBlockMap.ContainsKey(category)) return _logBlockMap[category];
            LogBlock logBlock = new LogBlock(category);
            _logBlockMap.Add(category, logBlock);
            return logBlock;
        }


    }
}
