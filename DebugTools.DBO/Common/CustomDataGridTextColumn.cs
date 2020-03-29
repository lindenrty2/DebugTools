using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Linq;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Media;
using System.Windows.Controls;
using System;
using System.Xml.Linq;
using System.Windows.Navigation;

namespace DebugTools.DBO
{
    public class CustomDataGridTextColumn : DataGridTextColumn
    {
        private DataWhereView _whereView;
        public DataWhereView WhereView
        {
            get
            {
                return _whereView;
            }
        }

        public CustomDataGridTextColumn(DataWhereView whereView) : base()
        {
            _whereView = whereView;
        }
    }
}
