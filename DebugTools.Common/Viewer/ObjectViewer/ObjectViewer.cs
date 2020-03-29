using System.Windows.Forms;
using DebugTools; 

namespace DebugTools.Common.ObjectViewer
{
    public partial class ObjectViewer : Form
    {
        public ObjectViewer()
        {
            InitializeComponent();
        }

        public void SetObject(ObjectInfo obj)
        {
            this.lblPath.Text = obj.Path;
            this.propertyGrid.SetObject(obj.Object);
        }
         
    }
}
