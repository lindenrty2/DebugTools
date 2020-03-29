using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DebugTools.Common;
using DebugTools.Interface;

namespace DebugTools.ControlCenter.Config
{
    public partial class BaseSettingControl : UserControl, ISettingControl
    {
        public string Title
        {
            get { return "基本設定"; }
        }

        public string Description
        {
            get { return ""; }
        } 

        public BaseSettingControl()
        {
            InitializeComponent();
        }

        public bool Initialize(IXmlFile config)
        {
            MainConfig mainConfig = (MainConfig)config;
            spMainAppDirectory.SelectedPath = mainConfig.BaseSetting.MainAppDirectory;
            return true;
        }

        public bool Save(IXmlFile config)
        {
            MainConfig mainConfig = (MainConfig)config;
            mainConfig.BaseSetting.MainAppDirectory = spMainAppDirectory.SelectedPath;
            return true;
        }
    }
}
