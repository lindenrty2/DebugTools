using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.Common.Warpper
{
    public class KeyValuePairWarpper<TKey,TValue> : 
        WarpperObject
    {
        public TValue Value
        {
            get
            {
                return this.GetProperty<TValue>("Value");
            }
            set
            {
                this.SetProperty("Value", value);
            }
        }

        public TKey Key
        {
            get
            {
                return this.GetProperty<TKey>("Key");
            }
            set
            {
                this.SetProperty("Key", value);
            }
        }

        public KeyValuePairWarpper(object obj)
            : base(obj)
        { 
        } 

    }
}
