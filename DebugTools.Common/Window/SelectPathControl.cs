using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DebugTools.Common.Helper;

namespace DebugTools.Common.Window
{
    public enum SelectPathType
    {
        OpenFile,
        SaveFile,
        Dictionary
    }

    public delegate void PathChangedEventHander(object sender, PathChangedEventArgs args);

    public partial class SelectPathControl : UserControl
    {
        private SelectPathType _selectPathType = SelectPathType.OpenFile;
        public SelectPathType SelectPathType
        {
            get { return _selectPathType; }
            set { _selectPathType = value; }
        }
         
        public string SelectedPath
        {
            get { return txtPath.Text; }
            set { txtPath.Text = value; }
        }

        private string _filter = string.Empty;
        public string Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        public bool IsVaildPath
        {
            get { 
                if(SelectPathType == SelectPathType.OpenFile || 
                    SelectPathType == SelectPathType.SaveFile ){
                    return FileSystemHelper.IsValidFile(this.SelectedPath);
                    }
                else if (SelectPathType == SelectPathType.Dictionary)
                {
                    return FileSystemHelper.IsVaildDirectory(this.SelectedPath);
                }
                return false;
            }
        }

        public event PathChangedEventHander PathChanged;

        public SelectPathControl()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            SelectObject();
        }
         
        private void txtPath_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks > 1)
            {
                SelectObject();
            }
        }


        private bool SelectObject()
        {
            if (_selectPathType == SelectPathType.OpenFile)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                string beforePath = txtPath.Text;
                dialog.InitialDirectory = beforePath;
                dialog.Filter = _filter;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtPath.Text = dialog.FileName;
                    OnPathChanged(beforePath, txtPath.Text);
                    return true;
                }
            }
            else if (_selectPathType == SelectPathType.SaveFile )
            {
                SaveFileDialog dialog = new SaveFileDialog();
                string beforePath = txtPath.Text;
                dialog.InitialDirectory = beforePath;
                dialog.Filter = _filter;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtPath.Text = dialog.FileName;
                    OnPathChanged(beforePath, txtPath.Text);
                    return true;
                }
            }
            else if (_selectPathType == SelectPathType.Dictionary )
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                string beforePath = txtPath.Text;
                dialog.SelectedPath = beforePath;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtPath.Text = dialog.SelectedPath;
                    OnPathChanged(beforePath, txtPath.Text);
                    return true;
                }
            }
            return false;
        }

        public void OnPathChanged(string beforePath, string afterPath)
        {
            if (PathChanged != null)
            {
                PathChanged(this, new PathChangedEventArgs(beforePath, afterPath, this.SelectPathType));
            }
        }
     
    }
}
