using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DebugModule.Core;
using DebugTools.Common.Hook.ExceptionHook;
using DebugTools;

namespace DebugModule.Function
{
    public partial class CatchedExecptionView : DebugTools.Common.Window.FormBase
    {
        private IExceptionManager _manager = null;

        public CatchedExecptionView()
        {
            InitializeComponent();

            _manager = MainApplication.Current.ExceptionManager ;
            _manager.ExceptionAdded += new ExceptionAddedHanlder(Manager_ExceptionAdded);
            PaintExceptions();
        }

        private void PaintExceptions()
        {
            lvExecptionList.Items.Clear();
            ExceptionInformation[] exceptions = _manager.GetExceptions();
            for (int i = 0; i < exceptions.Length; i++)
            {
                AddException( exceptions[i]);
            }
        }

        private void AddException(ExceptionInformation exception)
        {
            ListViewItem item = new ListViewItem();
            item.Text = (lvExecptionList.Items.Count+1).ToString();
            item.SubItems.Add(exception.IsUntreated ? "●" : "");
            item.SubItems.Add(exception.CatchTime.ToString("HH:mm:ss") );
            item.SubItems.Add(exception.Exception.GetType().FullName);
            item.SubItems.Add(exception.Exception.Message );
            item.SubItems.Add(exception.Comment);
            item.Tag = exception;
            lvExecptionList.Items.Add(item);
        }

        public void Manager_ExceptionAdded(ExceptionInformation ex)
        {
            AddException(ex);
        }

        private void tsmClear_Click(object sender, EventArgs e)
        {
            lvExecptionList.Items.Clear();
            _manager.Clear();
        }

        private void tsmClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CatchedExecptionView_FormClosed(object sender, FormClosedEventArgs e)
        {
            _manager.ExceptionAdded -= new ExceptionAddedHanlder(Manager_ExceptionAdded);
        }

        private void lvExecptionList_DoubleClick(object sender, EventArgs e)
        {
            if (lvExecptionList.SelectedItems.Count == 0) return;
            ExceptionInformation ex = (ExceptionInformation)lvExecptionList.SelectedItems[0].Tag;
            ExceptionViewWindow window = new ExceptionViewWindow();
            window.SetExecption(ex.Exception );
            window.Show();

        }
         
    }
}
