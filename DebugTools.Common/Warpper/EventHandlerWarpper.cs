using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.Common.Warpper
{
    public class EventHandlerWarpper : WarpperObject 
    {
        public EventHandlerWarpper( object obj)
            : base(obj)
        {

        }
    }

    public class EventHandlerWarpper<T> : WarpperObject
    {
        public EventHandlerWarpper(object obj)
            : base(obj)
        {

        }
    }
}
