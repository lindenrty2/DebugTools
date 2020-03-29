using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    public delegate void MenuChangedEventHandler();

    public interface IMenuManager
    {
        event MenuChangedEventHandler MenuChanged;
 
        void AddMenuItem(string path,MenuItemInfo info);

        MenuItemInfo FindMenuItem(string path);

        void RemoveMenuItem(string path);

        void RemoveMenuItem(string path, MenuItemInfo info);
        
    }
}
