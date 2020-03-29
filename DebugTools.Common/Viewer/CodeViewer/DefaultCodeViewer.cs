using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DebugTools;
using System.Reflection;

namespace DebugTools.Common.Viewer.CodeViewer
{
    public partial class DefaultCodeViewer : UserControl ,ICodeViewer
    {
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

        public DefaultCodeViewer()
        {
            InitializeComponent();
        }

        public Control GetTopElement()
        {
            return this;
        }

        public void SetCode(CodeInfo codeInfo)
        {
            UpdateCode(codeInfo);
            UpdateMember(codeInfo);
        }

        public void UpdateCode(CodeInfo codeInfo)
        {
            this.rtxtCode.Text = codeInfo.Code;
        }

        public void UpdateMember(CodeInfo codeInfo)
        {
            tvMember.Nodes.Clear();
            if (codeInfo.MemberInfos == null) return;
            foreach (MemberInfo member in codeInfo.MemberInfos)
            {
                TreeNode node = new TreeNode();
                node.Text = member.Name;
                if (member.MemberType == MemberTypes.Field)
                { 
                    node.ForeColor = Color.OrangeRed;
                }
                else if (member.MemberType == MemberTypes.Property)
                { 
                    node.ForeColor = Color.DarkRed;
                }
                else if (member.MemberType == MemberTypes.Method)
                { 
                    node.ForeColor = Color.Blue;
                }
                else if (member.MemberType == MemberTypes.Event)
                { 
                    node.ForeColor = Color.Green;
                }
                node.Tag = member;
                tvMember.Nodes.Add(node);
            }
        }



    
    }
}
