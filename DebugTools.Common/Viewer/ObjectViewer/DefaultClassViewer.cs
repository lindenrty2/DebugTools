using System;
using System.Windows.Forms;
using DebugTools;
using System.ComponentModel;

namespace DebugTools.Common.ObjectViewer
{
    public partial class DefaultClassViewer : PropertyGrid , IObjectViewer 
    {

        private ObjectInfo _objectInfo = null;
        public ObjectInfo Object
        {
            get
            {
                return _objectInfo;
            }
        }

        public string Key
        {
            get { return "Default"; }

        }
        public string Title
        {
            get { return "Default"; }
        }

        public string Description
        {
            get { return "Code"; }
        }

        public DefaultClassViewer()
        {
            InitializeComponent();
        }

        public Control GetTopElement()
        {
            return this;
        }

        public void SetObject(ObjectInfo info )
        {
            _objectInfo = info; 
            this.SelectedObject = info.Object;
            
        }

        private void tsmOpenNewWindow_Click(object sender, EventArgs e)
        {
            object selectedValue = this.SelectedGridItem.Value;
            ObjectViewer viewer = new ObjectViewer();
            string path = _objectInfo.Path + "." + this.SelectedGridItem.Label;
            viewer.SetObject(new ObjectInfo(selectedValue, path));
            viewer.Show();
        }
         
     
    }
}
