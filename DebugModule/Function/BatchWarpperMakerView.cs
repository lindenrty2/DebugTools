using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using DebugTools.Common.Warpper;
using DebugTools;
using System.IO;
using DebugTools.Common.Helper;

namespace DebugModule.Function
{
    public partial class BatchWarpperMakerView : DebugTools.Common.Window.FormBase
    {
        public BatchWarpperMakerView()
        {
            InitializeComponent();
        }

        private Assembly _assembly = null;
        public void SetAssembly(Assembly assembly)
        {
            if (assembly == null) return;
            _assembly = assembly;
            Update(assembly);
        }

        public void Update(Assembly assembly)
        {
            txtTarget.SelectedPath = assembly.CodeBase;
            chklTargetDetailList.Items.Clear();
            Type[] types = assembly.GetTypes(); 
            int index = 0;
            foreach (Type type in types)
            { 
                chklTargetDetailList.Items.Add(type);
                chklTargetDetailList.SetItemChecked(index, true);
                index += 1;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!(spCreatePath.IsVaildPath)) return;
            Create(spCreatePath.SelectedPath);
        }

        public void Create(string directory)
        {
            foreach (Type type in this.chklTargetDetailList.CheckedItems)
            {
                WarpperCreator warpper = new WarpperCreator(type);
                CodeInfo codeInfo = warpper.Create(txtNameSpace.Text);
                WriteFile(directory,codeInfo.Name, codeInfo.Code);
            }
            MessageBox.Show("作成完了");
        }

        private void WriteFile(string directory, string className, string code)
        {
            string fileName = string.Format("{0}.cs", FileSystemHelper.GetSafePath(className));
            string path = Path.Combine(directory, fileName);
            if (File.Exists(path))
            {
                if (rdoAlert.Checked) {
                    string message = string.Format("ファイル「{0}」は既に存在している,上書するか",path);
                    if (MessageBox.Show(this, message, "警告", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }
                else if (rdoSkip.Checked)
                {
                    return;
                }
            }
            File.WriteAllText(path,code);
        }

        private void chklTargetDetailList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Type type = (Type)this.chklTargetDetailList.SelectedItem;
            if (type == null) return;
            WarpperCreator warpper = new WarpperCreator(type);
            this.codeViewerFrame1.SetCode(warpper.Create(txtNameSpace.Text));
        }

        private void txtTarget_PathChanged(object sender, DebugTools.Common.Window.PathChangedEventArgs args)
        {
            string newPath = args.AfterPath;
            Assembly assembly = Assembly.LoadFrom(newPath);
            SetAssembly(assembly);
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i=0;i< this.chklTargetDetailList.Items.Count;i++ )
            {
                chklTargetDetailList.SetItemChecked(i, true);
            }
        }

        private void btnUnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chklTargetDetailList.Items.Count; i++)
            {
                chklTargetDetailList.SetItemChecked(i, false);
            }
        }

    
    }
}
