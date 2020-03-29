using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DebugTools.Common;
using DebugTools.Common.Manager;
using DebugTools;
using System.Reflection; 

namespace DebugTools.Common.Config
{
    public partial class MainConfigFrm : Form
    {

        private IEnumerable<XmlConfigInfo> _settingControlList;
        private Dictionary<string,List<IXmlEditControl >> _editControlDict = null;

        public MainConfigFrm()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            lvSetting.Clear();
            IConfigManager configManager = SystemCenter.CurrentApplication.ConfigManager; 
            _editControlDict = new Dictionary<string,List<IXmlEditControl>>();
            _settingControlList = configManager.GetConfigInfos();

            foreach (XmlConfigInfo configInfo in _settingControlList)
            {
                if (configInfo.SettingControlsTypes == null || configInfo.SettingControlsTypes.Length == 0) continue;
                List<IXmlEditControl> controlList = new List<IXmlEditControl>();
                foreach (Type editControlType in configInfo.SettingControlsTypes)
                {
                    IXmlEditControl editControl = (IXmlEditControl)Assembly.GetAssembly(editControlType).CreateInstance(editControlType.FullName);
                    ListViewItem item = new ListViewItem();
                    item.Text = string.IsNullOrEmpty(configInfo.Title) ? editControl.Title : configInfo.Title;
                    item.ToolTipText = editControl.Description;
                    editControl.Initialize(configInfo.XmlFile);
                    item.Tag = editControl;
                    lvSetting.Items.Add(item);
                    controlList.Add(editControl);
                } 
                _editControlDict.Add(configInfo.Key,controlList);
            }
             
        } 

        private void lvSetting_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            gbSetting.Controls.Clear();
            UserControl control = (UserControl)e.Item.Tag;
            control.Dock = DockStyle.Fill;
            gbSetting.Text = e.Item.Text;
            gbSetting.Controls.Add(control);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (XmlConfigInfo configInfo in _settingControlList)
            { 
                IEnumerable<IXmlEditControl> controls = this._editControlDict[configInfo.Key];
                foreach (IXmlEditControl editControl in controls)
                {
                    editControl.Save(configInfo.XmlFile);
                }
                configInfo.XmlFile.Save();
            }
            
        }
    }
}
