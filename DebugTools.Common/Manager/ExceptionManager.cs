using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using DebugTools.Common.Service;
using DebugTools.Common.Hook.ExceptionHook;
using DebugTools;

namespace DebugTools.Common.Manager
{

    public class ExceptionManager : IExceptionManager
    {
        public event ExceptionAddedHanlder ExceptionAdded;

        private List<ExceptionInformation> _exceptionCollection = null;

        public ExceptionManager()
        {
            _exceptionCollection = new List<ExceptionInformation>();
        }

        public void Add(ExceptionInformation ex)
        {
            _exceptionCollection.Add(ex);
            if (ExceptionAdded != null)
            {
                ExceptionAdded(ex);
            }
        }

        public void Add(Exception ex)
        {
            ExceptionInformation exceptionInformation = new ExceptionInformation(ex,false );
            _exceptionCollection.Add(exceptionInformation);
            if (ExceptionAdded != null)
            {
                ExceptionAdded(exceptionInformation);
            }
        }

        public void AddUntreated(Exception ex)
        {
            ExceptionInformation exceptionInformation = new ExceptionInformation(ex);

            _exceptionCollection.Add(exceptionInformation);
            if (ExceptionAdded != null)
            {
                ExceptionAdded(exceptionInformation);
            }
        }

        public ExceptionInformation[] GetExceptions()
        {
            return _exceptionCollection.ToArray();
        }

        public ExceptionInformation[] GetExceptions<T>()
        {
            return _exceptionCollection.FindAll(delegate(ExceptionInformation ex) { return ex.Exception  is T; }).ToArray();
        }

        public void Clear( )
        {
            _exceptionCollection.Clear();
        }

    }
}
