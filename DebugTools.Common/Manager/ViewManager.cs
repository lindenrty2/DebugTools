using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using DebugTools; 

namespace DebugTools.Common.Manager
{ 

    public class ViewManager : IViewManager
    {
        private TreeMap<IViewCreator> _viewCreatorMap = new TreeMap<IViewCreator>();

        public void Regist(string category, string name, IViewCreator creator)
        {
            _viewCreatorMap.AddSubItem(category, name, new TreeMapItem<IViewCreator>(creator));
        }

        public void UnRegist(string category, string name)
        {
            _viewCreatorMap.RemoveSubItem(category, name);
        }

        public IViewCreator[] GetViewCreators()
        {
            List<IViewCreator> list = new List<IViewCreator>();
            foreach(TreeMapItem<IViewCreator> item in _viewCreatorMap.GetSubItems())
            {
                list.AddRange(item.GetSubValues());
            }

            return list.ToArray();
        }

        public IViewCreator GetViewCreator(string category,string name)
        {
            return _viewCreatorMap.GetValue(category, name);
        }

        public IViewCreator[] GetViewCreators(string category)
        {
            TreeMapItem<IViewCreator> categoryItem = _viewCreatorMap.GetSubItem(category);
            if (categoryItem == null) return new IViewCreator[0];
            return categoryItem.GetSubValues();
        }

        public string[] GetCategories()
        {
            return _viewCreatorMap.GetSubKeys();
        }
    }
}
