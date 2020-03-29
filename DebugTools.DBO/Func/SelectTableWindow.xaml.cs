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
using System.Windows.Input;

namespace DebugTools.DBO
{
    public partial class SelectTableWindow
    {
        public ITableInfo SelectedTable;

        private ITableList _tableList;
        private IDataAccessor _dataAccessor;

        public SelectTableWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            dgTables.MouseDoubleClick += dgTables_MouseDoubleClick;
            dgTables.PreviewKeyDown += dgTables_PreviewKeyDown;
            txtName.TextChanged += txtName_TextChanged;
            txtName.PreviewKeyDown += txtName_KeyUp; 
        }

        public SelectTableWindow(IDataAccessor dataAccessor) : this()
        {
            _dataAccessor = dataAccessor; 
            this.Title = string.Format("【{0}】选择表", _dataAccessor.DisplayName);
        }

        private void Window_Loaded(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            txtName.Focus();
            _tableList = _dataAccessor.GetTables();
            if ((_tableList.Source) is DataTable)
            {
                dgTables.Columns.Clear();
                dgTables.AutoGenerateColumns = true;
            }
            UpdateTableList();
        }

        private void dgTables_MouseDoubleClick(System.Object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SelectIt(dgTables.SelectedItem);
        }

        private void txtName_TextChanged(System.Object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateTableList();
        }

        private void UpdateTableList()
        {
            try
            {
                dgTables.DataContext = _tableList.Filter(txtName.Text);
            }
            catch (Exception ex)
            {
            }
        }

        private void txtName_KeyUp(System.Object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                txtName.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                dgTables.SelectedIndex = 0;
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                if (dgTables.Items.Count == 1)
                {
                    SelectIt(dgTables.SelectedItem);
                    e.Handled = true;
                    this.DialogResult = true;
                }
                else
                {
                    txtName.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    dgTables.SelectedIndex = 0;
                }
            }
        }

        private void dgTables_PreviewKeyDown(System.Object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgTables.SelectedIndex > 0)
                    return;
                txtName.Focus();
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                if (dgTables.SelectedItem != null)
                    SelectIt(dgTables.SelectedItem);
                else
                {
                    txtName.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    e.Handled = true;
                }
            }
        }

        private void SelectIt(object row)
        {
            if ((row) is DataRowView)
            {
                var datarow = (DataRowView)dgTables.SelectedItem;
                if (datarow == null)
                    return;
                SelectedTable = _tableList.GetTableInfo(datarow.Row["DBName"].ToString(), datarow.Row["Name"].ToString());
                this.DialogResult = true;
            }
            else if ((row) is TableInfo)
            {
                SelectedTable = (TableInfo)row;
                this.DialogResult = true;
            }
        }
    }

}