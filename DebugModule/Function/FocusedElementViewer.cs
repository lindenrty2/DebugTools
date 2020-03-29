using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Windows;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Interop;
using DebugTools.Common;
using DebugTools.Common.Warpper;
using DebugTools.Common.Warpper.API;

namespace DebugModule
{
    public partial class FocusedElementViewer : DebugTools.Common.Window.FormBase
    {
        private static FocusedElementViewer _current; 

        private static Dictionary<String, Type> TYPE_MAP = null;
        private static Dictionary<String, RoutedEvent> EVENT_MAP = null;

        private FrameworkElement _topElement = null;
        private System.Drawing.Rectangle _preRectangle;

        public FocusedElementViewer()
        {
            InitializeComponent();
  
            _current = this;
            if (TYPE_MAP == null)
            {
                TYPE_MAP = new Dictionary<string, Type>();
                TYPE_MAP.Add("", null);
                TYPE_MAP.Add("Visual", typeof(System.Windows.Media.Visual));
                TYPE_MAP.Add("UIElement", typeof(UIElement));
                TYPE_MAP.Add("FrameworkElement", typeof(FrameworkElement));
                TYPE_MAP.Add("Control", typeof(System.Windows.Controls.Control));
                TYPE_MAP.Add("Panel", typeof(System.Windows.Controls.Panel));
                TYPE_MAP.Add("Page", typeof(System.Windows.Controls.Page));
                TYPE_MAP.Add("Window", typeof(System.Windows.Window));
            }
            if (EVENT_MAP == null)
            {
                EVENT_MAP = new Dictionary<string, RoutedEvent>();
                EVENT_MAP.Add("フォーカス",System.Windows.UIElement.GotFocusEvent);
                EVENT_MAP.Add("クリック",System.Windows.UIElement.MouseUpEvent);
                EVENT_MAP.Add("MouseEnter", System.Windows.UIElement.MouseEnterEvent);
                EVENT_MAP.Add("MouseMove", System.Windows.UIElement.MouseMoveEvent);
            }
            InitControl(); 
      
        }

        private void InitControl()
        {
            tscTarget.Items.Clear();
            Dictionary<String, Type>.Enumerator typeEnu = TYPE_MAP.GetEnumerator();
            while (typeEnu.MoveNext())
            {
                tscTarget.Items.Add(typeEnu.Current.Key);
            }
            tscTarget.SelectedIndex = 0;


            Dictionary<String, RoutedEvent>.Enumerator eventEnu = EVENT_MAP.GetEnumerator();
            while (eventEnu.MoveNext())
            {
                tscMode.Items.Add(eventEnu.Current.Key);
                RoutedEventHandler eventHandler = new RoutedEventHandler(FocusedElementViewer.HandleEvent);
                EventManager.RegisterClassHandler(typeof(UIElement), eventEnu.Current.Value, eventHandler, true);
            }
            tscMode.SelectedIndex = 0;
            ContextMenu menu = new ContextMenu();
            MenuItem menuItem = new MenuItem();
            menuItem.Text = "新ウィンドウで表示";
            menuItem.Click +=new EventHandler(OpenNewWindow_Click);
            menu.MenuItems.Add(menuItem);
            tvObjectView.ContextMenu = menu;
        }

        private void OpenNewWindow_Click(object sender, EventArgs e)
        {
            if(tvObjectView.SelectedNode == null) return;
            UIElementTreeViewer viewer = new UIElementTreeViewer();
            viewer.SetObject((DependencyObject)tvObjectView.SelectedNode.Tag);
            viewer.Show();
        }

        private System.Windows.FrameworkElement _preTarget = null;
        //private System.Windows.Style _preStyle = null;
        public void SetTarget(Object obj)
        {
            if (obj != null)
            {
                tsslTargetType.Text = obj.ToString();
            }
            if (_preTarget != null)
            {
                
            }
            this.DrawTree(obj);
            this.SetObject(obj);
            this.tsslTargetType.Text = GetObjectName(obj);
        }

        public void DrawTree(object obj)
        {
            tvObjectView.Nodes.Clear();
            if (!(obj is FrameworkElement))
            {
                return;
            }
            FrameworkElement frameworkElement = (FrameworkElement)obj;
            TreeNode currentNode = new TreeNode();
            currentNode.Text = GetObjectName(frameworkElement);
            currentNode.Tag = frameworkElement;
            DependencyObject parentObject = VisualTreeHelper.GetParent(frameworkElement);
            while (frameworkElement != null && parentObject != null)
            {
                TreeNode newNode = new TreeNode();
                newNode.Text = GetObjectName(parentObject);
                newNode.Tag = parentObject;
                newNode.Expand();
                newNode.Nodes.Add(currentNode);
                currentNode = newNode;
                if (parentObject is FrameworkElement)
                {
                    frameworkElement = (FrameworkElement)parentObject;
                }
                parentObject = VisualTreeHelper.GetParent(frameworkElement);
                _topElement = frameworkElement;
            }
            tvObjectView.Nodes.Add(currentNode);
        }

        private string GetObjectName(object obj)
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

        private static void HandleEvent(object sender, RoutedEventArgs e)
        { 
            FocusedElementViewer form = FocusedElementViewer._current;
            if (form == null || form.IsDisposed) return;
            if (!Keyboard.IsKeyDown(Key.LeftCtrl )  && form.tsbCtrl.Checked )
            {
                return;
            }
            if (!IsTargetEvent(e.RoutedEvent, form.tscMode.SelectedItem.ToString()))
            {
                return;
            }
            if (!IsTargetType(sender,form.tscTarget.SelectedItem.ToString()))
            {
                return;
            }
            if (!e.OriginalSource.Equals(sender)) return;
            form.SetTarget(e.OriginalSource);
        }

        private static bool IsTargetType(object sender,string selectedType)
        {
            Type type = TYPE_MAP[selectedType];
            if (type == null)
            {
                return true;
            }
            return type.IsInstanceOfType(sender); 
        }

        private static bool IsTargetEvent(RoutedEvent e, string eventName)
        {
            RoutedEvent routedEvent = EVENT_MAP[eventName];
            return (routedEvent == e);
        }



        private void FocusedElementViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            _current = null;
        }

        public static void ShowWindow()
        {
            if (FocusedElementViewer._current != null) { return; }
            FocusedElementViewer form = new FocusedElementViewer();
            form.Show();
        }

        private void tscTarget_SelectedIndexChanged(object sender, EventArgs e)
        { 
        }

        private void tvObjectView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.SetObject(e.Node.Tag);
        }

        private void SetObject(object o)
        {
            
            if (objectViewer.Object != null)
            {
                if (objectViewer.Object == o) return;
            }
            if (objectViewer.Object is DependencyObject)
            {
                //APIPointer pointer = new APIPointer();
                //WinAPIWarpper.GetCursorPos(ref pointer);
                //IntPtr winPtr = WinAPIWarpper.WindowFromPoint(pointer.X, pointer.Y);
                
                Window w = Window.GetWindow((DependencyObject)objectViewer.Object);
                if (w == null) return;
                WindowInteropHelper h = new WindowInteropHelper(w);
                bool result = WinAPIWarpper.RedrawWindow(h.Handle, null, IntPtr.Zero, 4 | 1 | 128);
                result = WinAPIWarpper.UpdateWindow(h.Handle);
                if (!result)
                {
                    
                }
            }
            objectViewer.SetObject(o);
            if (o is UIElement) 
            {
                UIElement element = (UIElement)o;
                System.Windows.Point point = element.PointToScreen(new System.Windows.Point()); 
                System.Drawing.Point loc = new System.Drawing.Point(Convert.ToInt32(point.X), Convert.ToInt32( point.Y));
                IntPtr screen = WinAPIWarpper.CreateDC("DISPLAY", "", "", ""); 
                IntPtr destDC = WinAPIWarpper.CreateCompatibleDC(screen);
                Graphics g = Graphics.FromHdc(screen, destDC);
         
                _preRectangle = new Rectangle(loc,
                    new System.Drawing.Size(Convert.ToInt32(element.RenderSize.Width), Convert.ToInt32(element.RenderSize.Height)));
                g.DrawRectangle(Pens.Red, _preRectangle);
                WinAPIWarpper.DeleteDC(screen); 
            }
        }


         
    }
}
