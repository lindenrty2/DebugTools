using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using DebugModule.Function;

namespace DebugModule
{
    public partial class AssembliesView : DebugTools.Common.Window.FormBase
    {
        public AssembliesView()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            IOrderedEnumerable<Assembly> orderedAssemblies = assemblies.OrderBy(assembly => assembly.FullName);
            foreach (Assembly assembly in orderedAssemblies)
            {
                string assemblyName = assembly.FullName ;
                TreeNode node = new TreeNode();
                node.Text = assemblyName;
                node.Tag = assembly;
                itemView.Nodes.Add(node);
            }
        }

        private void itemView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid.SelectedObject = e.Node.Tag;
            if (e.Node.Nodes.Count != 0)
            {
                return;
            }
            string filterString = txtFilter.Text;
            if (e.Node.Tag is Assembly)
            {
                Assembly assembly = (Assembly)e.Node.Tag ;
                Type[] types = assembly.GetTypes();
                IOrderedEnumerable<Type> orderedTypes = types.OrderBy(type => type.Name );
                foreach (Type type in orderedTypes)
                {
                    TreeNode node = new TreeNode();
                    node.Text = type.Name;
                    node.Tag = type;
                    if (!string.IsNullOrEmpty(filterString) && node.Text.IndexOf(filterString) >= 0)
                    {
                        node.BackColor = Color.LightGreen;
                    }
                    e.Node.Nodes.Add(node);
                }
            }
            else if (e.Node.Tag is Type)
            {
                Type type = (Type)e.Node.Tag;
                MemberInfo[] members = type.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                IOrderedEnumerable<MemberInfo> orderedMembers = members.OrderBy(member => member.Name);
                foreach (MemberInfo member in orderedMembers)
                {
                    TreeNode node = new TreeNode();
                    node.Text = member.Name;
                    if (member.MemberType == MemberTypes.Field)
                    {
                        FieldInfo fieldInfo = (FieldInfo)member;
                        node.Tag = fieldInfo.FieldType;
                    }
                    else if (member.MemberType == MemberTypes.Property )
                    {
                        PropertyInfo propertyInfo = (PropertyInfo)member;
                        node.Tag = propertyInfo.PropertyType;
                    }
                    else if (member.MemberType == MemberTypes.Method)
                    {
                        MethodInfo methodInfo = (MethodInfo)member;
                        node.Tag = methodInfo.ReturnType;
                    }
                    else if (member.MemberType == MemberTypes.Event )
                    {
                        EventInfo eventInfo = (EventInfo)member;
                        node.Tag = eventInfo.EventHandlerType;
                    }
                    if (!string.IsNullOrEmpty(filterString) && node.Text.IndexOf(filterString) >= 0)
                    {
                        node.BackColor = Color.LightGreen;
                    }
                    e.Node.Nodes.Add(node);
                }
            }
        }

        private void tsMakeWarpper_Click(object sender, EventArgs e)
        {
            TreeNode treeNode = this.itemView.SelectedNode ;
            if (treeNode.Tag is Type) { 
                Type type = (Type)treeNode.Tag;
                WarpperMakerView view = new WarpperMakerView();
                view.DisplayWarpper(type);
                view.ShowDialog();
            }
            else if (treeNode.Tag is Assembly)
            {
                Assembly assembly = (Assembly)treeNode.Tag;
                BatchWarpperMakerView view = new BatchWarpperMakerView();
                view.SetAssembly(assembly);
                view.Show();
            }
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                FilterTree(itemView.Nodes,txtFilter.Text,true);
            }
        }

        private void FilterTree(TreeNodeCollection nodes,string filterString,bool isFirst)
        {
            foreach(TreeNode node in nodes){ 
                if (!string.IsNullOrEmpty(filterString) && node.Text.IndexOf(filterString) >= 0)
                {
                    node.BackColor = Color.LightGreen;
                    if (isFirst)
                    {
                        itemView.SelectedNode = node;
                        isFirst = false;
                    }
                }
                else
                {
                    node.BackColor = SystemColors.Window;
                }
                FilterTree(node.Nodes, filterString, isFirst);
            }
        }
         
    }
}
