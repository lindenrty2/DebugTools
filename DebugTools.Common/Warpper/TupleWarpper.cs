using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.Common.Warpper
{
    public class TupleWarpper<T1,T2>
        : WarpperObject
    {
        public TupleWarpper(object obj)
            : base(obj)
        {

        }
    }
}
