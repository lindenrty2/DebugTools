using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Data;
using DebugTools.DataBase;
using System.Windows;
using System.Windows.Controls;

namespace DebugTools.DBO
{

    public partial class DataView
    {
        private ITableInfo _selectedTable;
        private ITableRelationInfo _relation;
        private ITableInfo _relationTable;
        private bool _isParentView;
        private ISqlConditionGroup _sqlWhere = new SqlConditionGroup(true);
        private RowDefinition _relationRowDefine;

        public DataView(ITableInfo selectedTable, ITableRelationInfo relation)
        {
            InitializeComponent();
            Loaded += DataView_Loaded;
            LayoutUpdated += DataView_LayoutUpdated;
            ctlTableName.MouseUp += ctlName_MouseUp;
            ctlRelationName.MouseUp += ctlName_MouseUp;

           _selectedTable = selectedTable;
            _relation = relation;
            _isParentView = relation.IsParentTable(selectedTable);
            _relationTable = _relation.GetTargetTableInfo(_isParentView);
            ctlRelationName.Text = relation.FKName;
            ctlTableName.Text = _relationTable.DisplayName;
        }

        private void DataView_LayoutUpdated(object sender, System.EventArgs e)
        {
        }


        private void DataView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        public void SetKeys(IEnumerable<DataRow> rows)
        {
            if (rows != null && rows.Count() == 0)
                _sqlWhere = new SqlConditionGroup(true);
            else
                _sqlWhere = _relation.CreateWhere(_isParentView, rows);
            updateData();
        }

        public void SetKeys(SqlConditionGroup where)
        {
            _sqlWhere = where;
            updateData();
        }

        private void ctlName_MouseUp(System.Object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainTableView tableView = new MainTableView(_relationTable, _sqlWhere);
            tableView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            tableView.Show();
        }

        public void updateData()
        {
            DataTable data = null;
            if (_sqlWhere != null && _sqlWhere.Items.Count() == 0)
                data = _relationTable.DataAccessor.GetTableData(_relationTable, 0, null);
            else
                data = _relationTable.DataAccessor.GetTableData(_relationTable, 100, _sqlWhere);
            int rowCount = data.Rows.Count;
            if (rowCount == 0)
                _relationRowDefine.Height = new GridLength(100);
            else if (rowCount < 10)
                _relationRowDefine.Height = new GridLength(100 + (rowCount * 26));
            else
                _relationRowDefine.Height = new GridLength(300);
            this.ctlDataGrid.DataContext = data;
        }

        public void SetRelationRow(RowDefinition rowDefine)
        {
            _relationRowDefine = rowDefine;
        }
    }

}