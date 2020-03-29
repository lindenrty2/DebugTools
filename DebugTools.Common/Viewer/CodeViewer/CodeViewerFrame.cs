using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DebugTools;

namespace DebugTools.Common.Viewer.CodeViewer
{
    public partial class CodeViewerFrame : UserControl
    {
        private List<ICodeViewer> _codeViewerList = new List<ICodeViewer>();
        public CodeViewerFrame()
        {
            InitializeComponent();
            _codeViewerList.Add(new DefaultCodeViewer());
            AddViewer();
        }

        public void AddViewer()
        { 
            foreach (ICodeViewer codeViewer in _codeViewerList)
            {
                TabPage tabPage = new TabPage();
                tabPage.Text = codeViewer.Title;
                Control control = (Control)codeViewer;
                control.Dock = DockStyle.Fill;
                tabPage.Controls.Add(control);
                this.tabCodeViewer.TabPages.Add(tabPage);
            }
        }

        public void SetCode(string code)
        {
            CodeInfo codeInfo = new CodeInfo(code);
            SetCode(codeInfo);
        }

        public void SetCode(CodeInfo codeInfo)
        {
            foreach (ICodeViewer objectViewer in _codeViewerList)
            {
                objectViewer.SetCode(codeInfo);
            }
        }
    }
}
