using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms; 
using DebugTools.ControlCenter.Manager;
using DebugTools;
using DebugTools.Common.Helper;
using DebugTools.Common;
using DebugTools.Common.Config;
using DebugTools.Common.Manager;


namespace DebugTools.ControlCenter 
{
    public partial class ControlCenterFrm : Form , IViewContainer 
    {
        public ControlCenterFrm()
        {
            InitializeComponent();
            InitMenu();
            InitView();
        }

        public void InitMenu()
        {
            MenuItemInfo menuInfo = MainApplication.Current.MenuManager.FindMenuItem(@"Plugin\Browser");
            MenuItemInfo menuInfoInOut = MainApplication.Current.MenuManager.FindMenuItem(@"Plugin\InOut");

            if (menuInfo != null)
            {
                foreach (MenuItemInfo subInfo in menuInfo.SubMenuInfos)
                {
                    tsmBrowser.DropDownItems.Add(MenuHelper.CreateToolStripMenu(subInfo));
                }
            }
            if (menuInfoInOut != null)
            {
                foreach (MenuItemInfo subInfo in menuInfoInOut.SubMenuInfos)
                {
                    InOutBrowser.DropDownItems.Insert(0,MenuHelper.CreateToolStripMenu(subInfo));
                    
                }
            }
        }

        public void InitView()
        {
            ctlTab.TabPages.Clear();
            IViewCreator[] viewCreators = MainApplication.Current.ViewManager.GetViewCreators();
            foreach (IViewCreator viewCreator in viewCreators)
            {
                IView view = viewCreator.CreateView(this);
                TabPage page = new TabPage();
                page.Text = view.Title;
                Control control = view.GetTopElement();
                control.Size = page.Size;
                control.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                page.Controls.Add(control);
                ctlTab.TabPages.Add(page);
            }

        }

        private void tsmOption_Click(object sender, EventArgs e)
        {
            //openfiledialog


            MainConfigFrm frm = new MainConfigFrm();
            frm.ShowDialog();
        }

        private void ControlCenterFrm_MdiChildActivate(object sender, EventArgs e)
        {

        }

        private void ControlCenterFrm_Load(object sender, EventArgs e)
        {

        }
    }
}
