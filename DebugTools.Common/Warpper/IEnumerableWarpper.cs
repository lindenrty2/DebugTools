using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DebugTools.Common.Warpper
{
    public class IEnumerableWarpper<T>
        : IEnumerableWarpper, IEnumerable<T>
    {
        public IEnumerableWarpper(object obj)
            : base(obj)
        {

        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.InvokeMethod<IEnumerator<T>>("GetEnumerator");
        } 
    }

    public class IEnumerableWarpper : WarpperObject, IEnumerable
    {
        public IEnumerableWarpper(object obj)
            : base(obj)
        {

        }

        public IEnumerator GetEnumerator()
        {
            return this.InvokeMethod<IEnumeratorWarpper>("GetEnumerator");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.InvokeMethod<IEnumeratorWarpper>(typeof(IEnumerable), "GetEnumerator");
        }
    }
}
