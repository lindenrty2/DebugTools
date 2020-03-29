using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools;
using System.Windows.Forms;

namespace DebugTools.Common.Helper
{
    public class MenuHelper
    {
        public static ToolStripItem CreateToolStripMenu(MenuItemInfo info)
        {

            ToolStripMenuItem menuItem ;
            if (info.IsSpliter)
            {
                return new ToolStripSeparator();
            }
            else { 
                menuItem = new ToolStripMenuItem();
                menuItem.CheckOnClick = info.IsCheckMenu;
                menuItem.Checked = info.IsChecked;
                menuItem.Click += info.ClickAction;
                menuItem.CheckedChanged += info.CheckedAction;
                menuItem.Image = info.Icon;
                menuItem.Text = info.Title;
                menuItem.Tag = info.Tag;
                foreach (MenuItemInfo subInfo in info.SubMenuInfos)
                {
                    menuItem.DropDownItems.Add(CreateToolStripMenu(subInfo));
                }
            }
            return menuItem;
        }

    }
}
