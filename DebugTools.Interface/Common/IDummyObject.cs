using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    public interface IDummyObject
    {
        string TargetClassName { get; set; }

        bool IsEnabled { get; set; }

    }
}
