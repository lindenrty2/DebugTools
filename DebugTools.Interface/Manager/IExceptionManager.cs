using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools.Common.Hook.ExceptionHook;

namespace DebugTools
{
    public delegate void ExceptionAddedHanlder(ExceptionInformation ex);

    public interface IExceptionManager
    {
        event ExceptionAddedHanlder ExceptionAdded;

        void Add(ExceptionInformation ex);

        void Add(Exception ex);

        void AddUntreated(Exception ex);

        ExceptionInformation[] GetExceptions();

        ExceptionInformation[] GetExceptions<T>();

        void Clear();

    }
}
