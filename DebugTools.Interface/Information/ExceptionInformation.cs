using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.Common.Hook.ExceptionHook
{
    [Serializable]
    public class ExceptionInformation
    {
        private Exception _exception = null;
        public Exception Exception
        {
            get
            {
                return _exception;
            }
        }

        private bool _isUntreated;
        public bool IsUntreated
        {
            get{
                return _isUntreated;
            }
        }

        private DateTime _catchTime;
        public DateTime CatchTime
        {
            get
            {
                return _catchTime;
            }
        }

        private string _comment;
        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
            }
        }

        public ExceptionInformation(Exception ex)
            : this(ex,DateTime.Now,true )
        {

        }

        public ExceptionInformation(Exception ex, DateTime catchTime)
            : this(ex, catchTime, true )
        {

        }

        public ExceptionInformation(Exception ex, bool isUntreated)
            : this(ex,DateTime.Now,isUntreated )
        {

        }

        public ExceptionInformation(Exception ex,DateTime catchTime,bool isUntreated)
        {
            _exception = ex;
            _catchTime = catchTime;
            _isUntreated = isUntreated;
        }
    }
}
