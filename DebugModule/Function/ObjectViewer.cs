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
using System.Windows.Interop;
using DebugTools.Common.Warpper.API;

namespace DebugModule
{
    public partial class ObjectViewer : DebugTools.Common.Window.FormBase
    {
        private Rectangle _preRectangle ;
       
        public ObjectViewer()
        {
            InitializeComponent();
            InitControl();
            init();
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

        public void init()
        {
            try
            {
                treeView1.Nodes.Clear();
                if (System.Windows.Application.Current == null)
                {
                    foreach (PresentationSource presentationSource in PresentationSource.CurrentSources)
                    {
                        if
                        (
                            presentationSource.RootVisual is Window &&
                            ((Window)presentationSource.RootVisual).Visibility == Visibility.Visible
                        )
                        {
                            Window win = (Window)presentationSource.RootVisual;
                            TreeNode node = new TreeNode();
                            node.Text = win.Title;
                            node.Tag = win;
                            node.Nodes.Add(new TreeNode(""));
                            treeView1.Nodes.Add(node);
                        }
                    } 
                }
                else { 
                    WindowCollection windows = System.Windows.Application.Current.Windows;
                    for (int i = 0; i < windows.Count; i++)
                    {
                        Window win = windows[i]; 
                        TreeNode node = new TreeNode();
                        node.Text = win.Title;
                        node.Tag = win;
                        node.Nodes.Add(new TreeNode(""));
                        treeView1.Nodes.Add(node);
                    }
                }
                
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void SetObject(object o)
        {

            if (objectView.Object != null)
            {
                if (objectView.Object == o) return;
            }
            if (objectView.Object is DependencyObject)
            {
                //APIPointer pointer = new APIPointer();
                //WinAPIWarpper.GetCursorPos(ref pointer);
                //IntPtr winPtr = WinAPIWarpper.WindowFromPoint(pointer.X, pointer.Y);

                Window w = Window.GetWindow((DependencyObject)objectView.Object);
                if (w == null) return;
                WindowInteropHelper h = new WindowInteropHelper(w);
                bool result = WinAPIWarpper.RedrawWindow(h.Handle, null, IntPtr.Zero, 4 | 1 | 128);
                result = WinAPIWarpper.UpdateWindow(h.Handle);
                if (!result)
                {

                }
            }
            objectView.SetObject(o);
            if (o is UIElement)
            {
                UIElement element = (UIElement)o;
                System.Windows.Point point = element.PointToScreen(new System.Windows.Point());
                System.Drawing.Point loc = new System.Drawing.Point(Convert.ToInt32(point.X), Convert.ToInt32(point.Y));
                IntPtr screen = WinAPIWarpper.CreateDC("DISPLAY", "", "", "");
                IntPtr destDC = WinAPIWarpper.CreateCompatibleDC(screen);
                Graphics g = Graphics.FromHdc(screen, destDC);

                _preRectangle = new Rectangle(loc,
                    new System.Drawing.Size(Convert.ToInt32(element.RenderSize.Width), Convert.ToInt32(element.RenderSize.Height)));
                g.DrawRectangle(Pens.Red, _preRectangle);
                WinAPIWarpper.DeleteDC(screen);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetObject(e.Node.Tag);
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
                    if (child is System.Windows.Controls.Control)
                    {
                        node.Text = String.Format("{0}({1})", ((System.Windows.Controls.Control)child).Name, child.ToString());
                    }
                    else {
                        node.Text = child.ToString();
                    } 
                    node.Tag = child;
                    node.Nodes.Add(new TreeNode(""));
                    e.Node.Nodes.Add(node);
                }
            }
      
        }

        private void mi_Reload_Click(object sender, EventArgs e)
        {
            init();
        }

    }
}
