using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;

namespace DebugTools.Common.Warpper
{
    [WarpperTargetTypeAttribute("mscorlib", "System.Collections.Generic.List`1")]
    public class ListWarpper<T> : WarpperObject,
        ICollection<T>,
        IEnumerable<T>,
        ICollection, IEnumerable,
        ISerializable, IDeserializationCallback
    {
        public ListWarpper(object obj)
            : base(obj)
        {
            
        }

        public void Add(T item)
        {
            this.InvokeMethod("Add",item);
        }

        public void Clear()
        {
            this.InvokeMethod("Clear");
        }

        public bool Contains(T item)
        {
            return this.InvokeMethod<bool>("Contains", item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.InvokeMethod("CopyTo", array, arrayIndex);
        }

        public int Count
        {
            get { return this.GetProperty<int>("Count"); }
        }

        public bool IsReadOnly
        {
            get { return this.GetProperty<bool>("IsReadOnly"); }
        }

        public bool Remove(T item)
        {
            return this.InvokeMethod<bool>("Remove", item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.InvokeMethod<IEnumeratorWarpper<T>>("GetEnumerator");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.InvokeMethod<IEnumeratorWarpper>("GetEnumerator");
        }

        public void CopyTo(Array array, int index)
        {
            this.InvokeMethod("CopyTo", array, index);
        }

        public bool IsSynchronized
        {
            get { return this.GetProperty<bool>("IsSynchronized"); }
        }

        public object SyncRoot
        {
            get { return this.GetProperty("SyncRoot"); }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.InvokeMethod("GetObjectData", info, context);
        }

        public void OnDeserialization(object sender)
        {
            this.InvokeMethod("OnDeserialization", sender);
        }
    }
}
