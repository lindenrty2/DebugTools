using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools;

namespace DebugTools.Common.Manager
{
    public class MenuManager : IMenuManager
    {
        public event MenuChangedEventHandler MenuChanged;

        private MenuItemInfo _topMenu = new MenuItemInfo() ;

        public void AddMenuItem(string path, MenuItemInfo info)
        {
            MenuItemInfo targetMenuItem = FindOrCreateMenuItem(path);
            targetMenuItem.AddSubMenuItem(info);
            if (MenuChanged != null)
            {
                MenuChanged();
            }
        }

        private MenuItemInfo FindOrCreateMenuItem(string path)
        {
            string[] nodeNames = path.Split('\\');
            MenuItemInfo targetInfo = _topMenu;
            foreach (string nodeName in nodeNames)
            {
                MenuItemInfo currentInfo = targetInfo.GetSubMenuItem(nodeName);
                if (currentInfo == null)
                {
                    currentInfo = new MenuItemInfo();
                    currentInfo.Key = nodeName;
                    targetInfo.AddSubMenuItem(currentInfo);
                }
                targetInfo = currentInfo;
            }
            return targetInfo; 
        }

        public MenuItemInfo FindMenuItem(string path)
        {
            string[] nodeNames = path.Split('\\');
            MenuItemInfo currentInfo = _topMenu;
            foreach (string nodeName in nodeNames)
            {
                currentInfo = currentInfo.GetSubMenuItem(nodeName );
                if (currentInfo == null) return null;
            }
            if (currentInfo == null) return null;
            return currentInfo;
        }

        public void RemoveMenuItem(string path)
        {
            MenuItemInfo targetMenuItem = FindMenuItem(path);
            if (targetMenuItem == null) return;
            targetMenuItem.ClearSubItems();
        }

        public void RemoveMenuItem(string path, MenuItemInfo info)
        {
            MenuItemInfo targetMenuItem = FindMenuItem(path);
            if (targetMenuItem == null) return;
            targetMenuItem.RemoveSubMenuItem(info);
        }
    }
}
