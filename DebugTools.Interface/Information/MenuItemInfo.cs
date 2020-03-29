using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DebugTools
{
    [Serializable]
    public class MenuItemInfo
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public EventHandler ClickAction { get; set; }
        public EventHandler CheckedAction { get; set; }
        public bool IsCheckMenu { get; set; }
        public bool IsChecked { get; set; }
        public bool IsSpliter { get; set; }
        public Image Icon { get; set; }
        public object Tag { get; set; }

        private Dictionary<string, MenuItemInfo> _subMenuInfos = new Dictionary<string, MenuItemInfo>();
        public IEnumerable<MenuItemInfo> SubMenuInfos
        {
            get { return _subMenuInfos.Values; }
        }

        public void AddSubMenuItem(MenuItemInfo info)
        {
            if (this._subMenuInfos.ContainsKey(info.Key))
            {
                return;
            }
            _subMenuInfos.Add(info.Key,info);
        }

        public MenuItemInfo GetSubMenuItem(string key)
        {
            if (!this._subMenuInfos.ContainsKey(key))
            {
                return null;
            }
            return this._subMenuInfos[key];
        }

        public void RemoveSubMenuItem(MenuItemInfo info)
        {
            if (!this._subMenuInfos.ContainsKey(info.Key))
            {
                return;
            }
            _subMenuInfos.Remove(info.Key);
        }

        public void ClearSubItems()
        {
            _subMenuInfos.Clear();
        }
    }
}
