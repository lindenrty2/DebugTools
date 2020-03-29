using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    public enum MessageType
    {
        Error,
        Warning,
        Information,
        Status,
        Other
    }

    public interface IMessage
    {
        string Title { get; }

        string Detail { get; }

        MessageType Type { get; }
    }
}
