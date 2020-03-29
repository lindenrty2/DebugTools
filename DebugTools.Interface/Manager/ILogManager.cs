using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    public delegate void LogAppendedEventHanlder(DMessage message);

    public interface ILogManager
    {
        event LogAppendedEventHanlder LogAppended; 

        int SizeLimit { get; set; }

        int TimeSpan { get; set; }

        bool AutoSave { get; set; }

        void WriteInfo(string category, DMessage info);

        void WriteWarning(string category, DMessage warning);

        void WriteError(string category, DMessage error);

        bool Save();

        bool Save(string path);

        void Clear();

        void Clear(string category);

    }
}
