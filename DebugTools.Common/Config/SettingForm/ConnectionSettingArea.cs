using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DebugTools.Common;

namespace DebugTools.Common.Config
{
    public partial class ConnectionSettingArea : UserControl
    {
        private bool _isNew = true;
        public bool IsNew
        {
            get { return _isNew; }
            set { _isNew = value; }
        }

        public ConnectionSettingArea()
        {
            InitializeComponent();
        }

        public void SetSetting(ConnectionItemNode setting)
        {
            chkDisplay.Checked = setting.IsDisplay;
            txtKey.Text = setting.Key;
            txtConnection.Text = setting.ConnectionString;
            _isNew = false;
        }

        public void SaveSetting(ConnectionItemNode setting)
        {
            setting.IsDisplay = chkDisplay.Checked;
            setting.Key = txtKey.Text;
            setting.ConnectionString = txtConnection.Text;
        } 
    }
}
