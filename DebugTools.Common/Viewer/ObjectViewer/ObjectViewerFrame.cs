using System.Collections.Generic;
using System.Windows.Forms;
using DebugTools;

namespace DebugTools.Common.ObjectViewer
{
    public partial class ObjectViewerFrame : UserControl
    {

        private object _object;
        public object Object
        {
            get
            {
                return _object;
            }
        }

        private List<IObjectViewer> _objectViewerList = new List<IObjectViewer>();
        public ObjectViewerFrame()
        {
            InitializeComponent();
            _objectViewerList.Add(new DefaultClassViewer());
            _objectViewerList.Add(new PowerClassViewer());
            AddViewer();
        }

        public void AddViewer()
        { 
            foreach (IObjectViewer objectViewer in _objectViewerList)
            {
                TabPage tabPage = new TabPage();
                tabPage.Text = objectViewer.Title;
                Control control = (Control)objectViewer;
                control.Dock = DockStyle.Fill;
                tabPage.Controls.Add(control);
                this.tabControl1.TabPages.Add(tabPage);
            }
        }

        public void SetObject(object obj)
        {
            _object = obj;
            ObjectInfo objInfo = new ObjectInfo(obj,"");
            foreach (IObjectViewer objectViewer in _objectViewerList)
            {
                objectViewer.SetObject(objInfo);
            }
        }
        
    }
}
