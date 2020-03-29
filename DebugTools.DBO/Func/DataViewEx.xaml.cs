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
using System.Data;
using System.ComponentModel;
using DebugTools.DataBase;
using DebugTools.DBO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace DebugTools.DBO
{
    public partial class DataViewEx
    {
        public class DataSelectChangeArgs
        {
            public DataRow[] Rows { get; set; }
        }

        public event DataSelectChangedHanlder OnDataSelectChanged;

        public delegate void DataSelectChangedHanlder(DataViewEx sender, DataSelectChangeArgs args);

        private int _displayCount;
        public int DisplayCount
        {
            get
            {
                return System.Convert.ToInt32(ctlShowCount.Text);
            }
            set
            {
                ctlShowCount.Text = value.ToString();
            }
        }

        private ISqlConditionGroup _sqlWhere = null;
        public ISqlConditionGroup SqlWhere
        {
            get
            {
                return _sqlWhere;
            }
            set
            {
                _sqlWhere = value;
            }
        }

        private ITableInfo _targetInfo;
        public ITableInfo TargetInfo
        {
            get
            {
                return _targetInfo;
            }
            set
            {
                _targetInfo = value;
                UpdateTable();
            }
        }

        private IDataAccessor _dataAccessor;
        public IDataAccessor DataAccessor
        {
            get
            {
                return _dataAccessor;
            }
            set
            {
                _dataAccessor = value;
            }
        }

        private List<DataWhereView> _whereViews = new List<DataWhereView>();

        public DataViewEx()
        {
            InitializeComponent();

            ctlShowConverter.Checked += ctlShowConverter_Checked;
            ctlShowCount.TextChanged += ctlShowCount_TextChanged;
            dgMain.SelectionChanged += dgMain_SelectionChanged;
            dgMain.Sorting += dgMain_Sorting;
            ctlAddPackageData.Click += ctlAddPackageData_Click;
            ctlShowCount.KeyUp += ctlShowCount_KeyUp;
        }

        public void Init(IDataAccessor dataAccessor, ISqlConditionGroup sqlWhere)
        {
            _dataAccessor = dataAccessor;
            _sqlWhere = sqlWhere;

            this.DisplayCount = ConfigCenter.Instance.GetDefaultMaxSearchCount();
        }

        private void WhereChange(DataWhereView obj)
        {
            UpdateData();
        }

        private void ctlShowConverter_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;
        }

        public void UpdateTable()
        {
            if (!this.IsLoaded)
                return;

            ctlTableName.Text = _targetInfo.DisplayName;
            _whereViews.Clear();
            ctlTableWhereList.Children.Clear();

            IEnumerable<ISqlCondition> conditions = _sqlWhere == null ? null : _sqlWhere.Items;
            if (conditions != null && conditions.Count() == 1 && conditions.ElementAtOrDefault(0) is SqlConditionGroup)
                conditions = ((SqlConditionGroup)conditions.ElementAtOrDefault(0)).Items;
            IEnumerable<SqlCondition> conditionItems = null;
            if (conditions != null && !conditions.Any(x => !(x is SqlCondition)))
                conditionItems = conditions.Select(x => (SqlCondition)x);

            dgMain.Columns.Clear();
            foreach (IColumnInfo column in _targetInfo.Columns)
            {
                string columnName = column.Name;

                SqlCondition sw = conditionItems == null ? null : conditionItems.FirstOrDefault(x => x.ColumnName == columnName);
                DataWhereView wh = new DataWhereView(column, sw);
                wh.OnChange += WhereChange;
                ctlTableWhereList.Children.Add(wh);
                _whereViews.Add(wh);

                CustomDataGridTextColumn gridColumn = new CustomDataGridTextColumn(wh);
                TextBlock headerText = new TextBlock() { Text = column.Name + System.Environment.NewLine + column.Comment };
                gridColumn.Header = headerText;
                gridColumn.Binding = new Binding(columnName);
                dgMain.Columns.Add(gridColumn);
            }
        }

        public void UpdateData()
        {
            if (!this.IsLoaded)
                return;
            dgMain.DataContext = _dataAccessor.GetTableData(_targetInfo, DisplayCount, GetWhere());
            foreach (CustomDataGridTextColumn column in dgMain.Columns)
            {
                if (column.WhereView == null)
                    return;
                column.SortDirection = column.WhereView.SortDirection;
            }
        }

        private SqlConditionGroup GetWhere()
        {
            SqlConditionGroup where = new SqlConditionGroup(true);
            _whereViews.ForEach(x => where.Add(x.GetSQLWhere()));
            return where;
        }

        private void ctlShowCount_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            decimal result = default(decimal);
            if (decimal.TryParse(ctlShowCount.Text, out result))
                ctlShowCount.Text = result.ToString();
            else
                ctlShowCount.Text = "";
        }

        private void dgMain_SelectionChanged(System.Object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            List<DataRow> dataRows = new List<DataRow>();
            foreach (object selectItem in this.dgMain.SelectedItems)
            {
                if (!((selectItem) is DataRowView))
                    continue;
                DataRowView datarow = (DataRowView)selectItem;
                if (datarow.IsNew)
                    continue;
                dataRows.Add(datarow.Row);
            }
            OnDataSelectChanged?.Invoke(this, new DataSelectChangeArgs() { Rows = dataRows.ToArray() });
        }

        private void dgMain_Sorting(object sender, System.Windows.Controls.DataGridSortingEventArgs e)
        {
            if (!((e.Column) is CustomDataGridTextColumn))
                return;
            CustomDataGridTextColumn customColumn = (CustomDataGridTextColumn)e.Column;
            ListSortDirection? sort = customColumn.SortDirection;
            if (!e.Column.SortDirection.HasValue)
                sort = ListSortDirection.Ascending;
            else if (e.Column.SortDirection == ListSortDirection.Descending)
                sort = ListSortDirection.Ascending;
            else
                sort = ListSortDirection.Descending;
            customColumn.WhereView.SortDirection = sort;
            UpdateData();
            e.Handled = true;
        }

        private void ctlAddPackageData_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            List<DataRow> dataRows = new List<DataRow>();
            foreach (object selectItem in this.dgMain.SelectedItems)
            {
                if (!((selectItem) is DataRowView))
                    continue;
                DataRowView datarow = (DataRowView)selectItem;
                if (datarow.IsNew)
                    continue;
                dataRows.Add(datarow.Row);
            }
            System.Windows.MessageBox.Show("正在改造");
            //_dataAccessor.GetExporter().AddTargets(dataRows);
        }

        private void ctlShowCount_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                UpdateData();
        }
    }

}