using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DebugTools.Common.Warpper
{
    public class ICollectionWarpper<T>
        : IEnumerableWarpper<T>, ICollection<T>
    {
        public ICollectionWarpper(object obj)
            : base(obj)
        {

        }

        public void Add(T item)
        {
            this.InvokeMethod("Add", item);
        }

        public void Clear()
        {
            this.InvokeMethod("Clear");
        }

        public bool Contains(T item)
        {
            return (bool)this.InvokeMethod("Contains",item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.InvokeMethod("CopyTo", array, arrayIndex);
        }

        public int Count
        {
            get { return (int)this.GetProperty("Count"); }
        }

        public bool IsReadOnly
        {
            get { return (bool)this.GetProperty("IsReadOnly"); }
        }

        public bool Remove(T item)
        {
            return (bool)this.InvokeMethod("Remove", item);
        }

         
    }
}
