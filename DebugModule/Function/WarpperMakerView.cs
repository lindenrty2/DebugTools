using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using DebugTools.Common;
using DebugTools.Common.Warpper;

namespace DebugModule.Function
{
    public partial class WarpperMakerView : DebugTools.Common.Window.FormBase
    {
        public WarpperMakerView()
        {
            InitializeComponent();
        }

        public void DisplayWarpper(Type type)
        {
            WarpperCreator warpper = new WarpperCreator(type);
            this.codeViewerFrame1.SetCode(warpper.Create());
        } 
    }

}
