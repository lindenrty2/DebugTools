using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DebugTools.Common.Hook.ExceptionHook
{
    public partial class ExceptionViewWindow : Form
    {
        public ExceptionViewWindow()
        {
            InitializeComponent();
        }

        public void SetExecption(Exception ex)
        {
            exceptionViewer1.SetExecption(ex);
            objectViewerFrame1.SetObject(ex);
        }
         
    }
}
