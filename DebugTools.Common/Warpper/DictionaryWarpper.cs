using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;
using System.Reflection;
using DebugTools.Common.Warpper;

namespace DebugTools.Common.Warpper
{
    [WarpperTargetType("mscorlib", "System.Collections.Generic.Dictionary`2")]
    public class DictionaryWarpper<TKey,TValue> 
        : WarpperObject,
        ICollection<KeyValuePairWarpper<TKey, TValue>>,
        IEnumerable<KeyValuePairWarpper<TKey, TValue>>,
        IDictionary, ICollection, IEnumerable,
        ISerializable, IDeserializationCallback

    {

        public TValue this[string key]
        {
            get
            {
                return this.InvokeMethod<TValue>("get_Item", key);
            }
            set
            {
                this.InvokeMethod("set_Item", key, value);
            }
        }

        public DictionaryWarpper(object obj)
            : base(obj)
        {

        }

        public void Add(TKey key, TValue value)
        {
            this.InvokeMethod("Add", key, value );
        }

        public bool ContainsKey(TKey key)
        {
            return (bool)this.InvokeMethod("ContainsKey", key);
        }

        public ICollection<TKey> Keys
        {
            get { return (ICollection<TKey>)this.GetProperty("Keys"); }
        }

        public bool Remove(TKey key)
        {
            return (bool)this.InvokeMethod("Remove", key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            ParameterModifier[] mods = new ParameterModifier[]{ new ParameterModifier() };
            mods[0][1] = true;
            TValue outValue = default(TValue);
            bool result = (bool)this.InvokeMethod("TryGetValue", new object[] { key, outValue }, mods);
            value = outValue;
            return result;
        }

        public ICollection<TValue> Values
        {
            get { return this.GetProperty<ICollectionWarpper<TValue>>("Values"); }
        }

        public TValue this[TKey key]
        {
            get
            {
                return this.InvokeMethod<TValue>("get_Item", key);
            }
            set
            {
                this.InvokeMethod("set_Item", key, value );
            }
        }

        public object this[object key]
        {
            get
            {
                return this.InvokeMethod("get_Item", key);
            }
            set
            {
                this.InvokeMethod("set_Item", key, value);
            }
        }

        public void Add(KeyValuePairWarpper<TKey, TValue> item)
        {
            this.InvokeMethod("Add", item );
        }

        public void Clear()
        {
            this.InvokeMethod("Clear");
        }

        public bool Contains(KeyValuePairWarpper<TKey, TValue> item)
        {
            return (Boolean)this.InvokeMethod("Contains", item );
        }

        public void CopyTo(KeyValuePairWarpper<TKey, TValue>[] array, int arrayIndex)
        {
            this.InvokeMethod("Contains", array,arrayIndex );
        }

        public int Count
        {
            get { return (int)this.GetProperty("Count"); }
        }

        public bool IsReadOnly
        {
            get { return (bool)this.GetProperty("IsReadOnly"); }
        }

        public bool Remove(KeyValuePairWarpper<TKey, TValue> item)
        {
            return (bool)this.InvokeMethod("Remove", item);
        }

        public IEnumerator<KeyValuePairWarpper<TKey, TValue>> GetEnumerator()
        {
            return this.InvokeMethod<IEnumeratorWarpper<KeyValuePairWarpper<TKey, TValue>>>("GetEnumerator");
        }

        public void Add(object key, object value)
        {
            this.InvokeMethod("Add",key,value);
        }

        public bool Contains(object key)
        {
            return (bool)this.InvokeMethod("Contains", key);
        }

        public bool IsFixedSize
        {
            get { return (bool)this.GetProperty("IsFixedSize"); }
        }

        public void Remove(object key)
        {
            this.InvokeMethod("Remove", key);
        }

        public void CopyTo(Array array, int index)
        {
            this.InvokeMethod("CopyTo", array,index);
        }

        public bool IsSynchronized
        {
            get { return (bool)this.GetProperty("IsSynchronized"); }
        }

        public object SyncRoot
        {
            get { return (object)this.GetProperty("SyncRoot"); }
        }

        public void OnDeserialization(object sender)
        {
            this.InvokeMethod("OnDeserialization", sender); 
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.InvokeMethod("GetObjectData", info, context);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.InvokeMethod(typeof(IEnumerable), "GetEnumerator");
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return (IDictionaryEnumerator)this.InvokeMethod(typeof(IDictionary), "GetEnumerator");
        }

        ICollection IDictionary.Keys
        {
            get { return (ICollection)this.GetProperty(typeof(IEnumerable), "Keys"); }
        }

        ICollection IDictionary.Values
        {
            get { return (ICollection)this.GetProperty(typeof(IDictionary), "Values"); }
        } 

    }
}
