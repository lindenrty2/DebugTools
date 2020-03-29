using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using DebugTools;
using DebugTools.Common.Warpper;
using System;

namespace DebugTools.Common.ObjectViewer
{
    public partial class PowerClassViewer : ListView ,IObjectViewer
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
            get { return "PowerView"; }
        }

        public string Description
        {
            get { return "Code"; }
        }

        public PowerClassViewer()
        {
            InitializeComponent();
            InitControl();
        }

        public void InitControl()
        {
            this.View = View.Details;
            this.FullRowSelect = true; 
            this.Columns.Add("Name", 150);
            this.Columns.Add("Value", 200);
            this.Columns.Add("Type", 200);
            this.ContextMenuStrip = cmsMenu;
            this.MouseDoubleClick += lvView_MouseDoubleClick;
            this.DrawItem += new DrawListViewItemEventHandler(PowerClassViewer_DrawItem);
            this.DrawSubItem += new DrawListViewSubItemEventHandler(PowerClassViewer_DrawSubItem);
            this.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(PowerClassViewer_DrawColumnHeader);
            this.OwnerDraw = true;
        }

        public Control GetTopElement()
        {
            return this;
        }

        void PowerClassViewer_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        void PowerClassViewer_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true; 
        }

        void PowerClassViewer_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        public void SetObject(ObjectInfo info)
        {
            if (info == null) return;
            this.Items.Clear();
            ListViewItem item = new ListViewItem();
            item.Text = "    this";
            item.Tag = info.Object; 
            item.SubItems.Add(GetDisplayText(info.Object ));
            item.SubItems[0].Tag = 1;
            this.Items.Add(item);
            ExpandData(0);
        }

        public void ExpandData(int index)
        {
            object expandData = this.Items[index].Tag;
            int level = ObjectConvert.ToInt(this.Items[index].SubItems[0].Tag);
            if (expandData == null) return;
            if (index < this.Items.Count - 1 )
            {
                int nextItemLevel = ObjectConvert.ToInt(this.Items[index + 1].SubItems[0].Tag);
                bool isExpanded = level != nextItemLevel;
                while (level != nextItemLevel)
                {
                    this.Items.RemoveAt(index + 1);
                    if (index + 1 >= this.Items.Count) break;
                    nextItemLevel = ObjectConvert.ToInt(this.Items[index + 1].SubItems[0].Tag);
                }
                if (isExpanded)
                {
                    this.Items[index].Text = this.Items[index].Text.Replace("⊟", "⊞");
                    this.Items[index].SubItems[1].Tag = false;
                    return;
                }

            }
            if (this.Items[index].Text.IndexOf("⊞") >= 0)
            {
                this.Items[index].Text = this.Items[index].Text.Replace("⊞", "⊟");
            }
            else
            {
                this.Items[index].Text = "".PadLeft(4 * level, ' ') + "⊟" + this.Items[index].Text.Trim();
            }
            this.Items[index].SubItems[1].Tag = true;
            WarpperObject warpperObject = new CommonWarpperObject(expandData);
            MemberInfo[] members = warpperObject.GetStaticMemberInfos();
            int ownIndex = 0;
            level += 1;
            foreach(MemberInfo member in members)
            {
                ListViewItem item = new ListViewItem(); 
                object value = null;
                if (member.MemberType == MemberTypes.Property)
                {
                    PropertyInfo info = (PropertyInfo)member;
                    if (!info.CanRead || info.GetGetMethod() == null) continue;
                    if ( info.GetGetMethod().IsStatic)
                    {
                        value = WarpperObject.GetStaticProperty(warpperObject.GetWarpperType(), info.Name );
                    }
                    else 
                    { 
                        value = warpperObject.GetProperty(info.Name);
                    }
                    item.ForeColor = Color.Blue;
                }
                else if (member.MemberType == MemberTypes.Field)
                {
                    FieldInfo info = (FieldInfo)member;
                    if (info.IsStatic) {
                        value = info.GetValue(null);
                    }
                    else { 
                        value = warpperObject.GetField(member.Name);
                    }
                    item.ForeColor = Color.Red;
                }
                item.Text = "".PadLeft(4 * level,' ') + member.Name;
                item.SubItems.Add(GetDisplayText(value));
                item.SubItems[0].Tag = level;
                item.Tag = value;
                this.Items.Insert(index + ownIndex + 1, item);
                ownIndex++;
            }
        }

        public string GetDisplayText(object obj)
        {
            if (obj == null)
            {
                return "NULL";
            }
            try
            {
                return obj.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        private void lvView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.SelectedIndices.Count == 0)
            {
                return;
            }
            ExpandData(this.SelectedIndices[0]);
        }

        private void tsmOpenNew_Click(object sender, System.EventArgs e)
        {
            if (this.SelectedIndices.Count == 0)
            {
                return;
            }
            ObjectViewer viewer = new ObjectViewer();
            viewer.SetObject(new ObjectInfo(this.SelectedItems[0].Tag, string.Empty));
            viewer.Show(); 
        }
    
    }

}
