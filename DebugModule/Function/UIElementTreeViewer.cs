using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Media;

namespace DebugModule
{
    public partial class UIElementTreeViewer : DebugTools.Common.Window.FormBase
    {
        private DependencyObject _selectedObject;

        public UIElementTreeViewer()
        {
            InitializeComponent();
            InitControl();
        }

        public void InitControl()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem menuItem = new MenuItem();
            menuItem.Text = "新ウィンドウで表示";
            menuItem.Click += new EventHandler(OpenNewWindow_Click);
            menu.MenuItems.Add(menuItem);
            treeView1.ContextMenu = menu;
        }


        private void OpenNewWindow_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            UIElementTreeViewer viewer = new UIElementTreeViewer();
            viewer.SetObject((DependencyObject)treeView1.SelectedNode.Tag);
            viewer.Show();
        }

        public void SetObject(DependencyObject obj)
        {
            _selectedObject = obj;
            TreeNode node = new TreeNode();
            node.Text = GetObjectName(obj);
            node.Tag = obj;
            node.Nodes.Add(new TreeNode(""));
            treeView1.Nodes.Add(node);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid.SetObject( e.Node.Tag);
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Expand)
            {
                return;
            }
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text.Equals("")) {
                e.Node.Nodes.Clear();
                DependencyObject target = (DependencyObject)e.Node.Tag; 

                int count = VisualTreeHelper.GetChildrenCount(target);
                for (int i = 0; i < count; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(target,i);
                    TreeNode node = new TreeNode();
                    node.Text = GetObjectName(child);
                    node.Tag = child;
                    node.Nodes.Add(new TreeNode(""));
                    e.Node.Nodes.Add(node);
                }
            }
      
        }

        private string GetObjectName(DependencyObject obj)
        {
            if (obj is System.Windows.Controls.Control)
            {
                return String.Format("{0}({1})", ((System.Windows.Controls.Control)obj).Name, obj.ToString());
            }
            else
            {
                return obj.ToString();
            } 
        }

        private void mi_Reload_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            SetObject(_selectedObject);
        }

    }
}
