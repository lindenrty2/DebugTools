﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools;

namespace DebugTools.Common.DBSupport.MySQL
{
    [PluginEntry]
    public class PluginEntry
        : IPluginEntry
    {

        public string Name
        {
            get { return "DebugTools.Common.DBSupport.MySQL"; }
        }

        public string Description
        {
            get { return "MySQL数据库支持"; }
        }

        private IApplication _app;

        public bool Support(IApplication app)
        {
            return true;
        }

        public bool Connection(IApplication app)
        {
            _app = app;
            //_app.ClassManager.Regist(Const.ClassCategoryConst.DBACCESSOR ,"2", typeof(OracleDBAccessor));
            return true;
        }
         

        public bool Disconnection()
        { 
            return true;
        }

        public void WndProc(DMessage message)
        {
        }

    }
}
