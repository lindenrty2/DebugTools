using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    public class ListMap<TKEY,TVALUE>  
    {
        private Dictionary<TKEY, List<TVALUE>> _dict = null;
        public ListMap()
        {
            _dict = new Dictionary<TKEY, List<TVALUE>>();
        }

        public void AddItem(TKEY key, TVALUE value)
        {
            AddItems(key,new TVALUE[]{value});
        }

        public void AddItems(TKEY key, TVALUE[] values)
        {
            List<TVALUE> valueList = null;
            if (!_dict.ContainsKey(key))
            {
                valueList = new List<TVALUE>();
                _dict.Add(key, valueList);
            }
            else
            {
                valueList = _dict[key];
            }
            valueList.AddRange(values);
        }

        public List<TVALUE> GetList(TKEY key)
        { 
            if (!_dict.ContainsKey(key)) { return null; }
            return _dict[key];
        }

    }
}
