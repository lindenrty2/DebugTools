using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DebugTools.Common.Warpper
{
    public class IEnumeratorWarpper<TValue>
        : IEnumeratorWarpper, IEnumerator<TValue>
    {
        public IEnumeratorWarpper(object obj)
            : base(obj)
        {

        }

        public TValue Current
        {
            get { return this.GetProperty<TValue>("Current"); }
        } 
    }

    public class IEnumeratorWarpper
        : WarpperObject,
          IEnumerator
    {
        public IEnumeratorWarpper(object obj)
            : base(obj)
        {

        } 

        object IEnumerator.Current
        {
            get { return this.GetProperty("Current"); }
        }

        public bool MoveNext()
        {
            return (bool)this.InvokeMethod("MoveNext");
        }

        public void Reset()
        {
            this.InvokeMethod("Reset");
        } 
    }
}
