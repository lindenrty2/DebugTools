using DebugTools.DataBase;
using DebugTools.Package;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DebugTools.DBO
{
    public partial class MainTableView
    {
        private List<DataView> _parentRelationViews = new List<DataView>();
        private List<DataView> _subRelationViews = new List<DataView>();
        private IDataAccessor _dataAccessor;

        public MainTableView() : this(null)
        {
        }

        public MainTableView(IDataAccessor dataAccessor)
        {
            _dataAccessor = dataAccessor;
            InitializeComponent();
            InitControl();
            mainDataView.Init(dataAccessor, null);
        }

        public MainTableView(ITableInfo table, ISqlConditionGroup where)
        {
            _dataAccessor = table.DataAccessor;
            InitializeComponent(); 
            InitControl();
            mainDataView.Init(_dataAccessor, where);
            mainDataView.TargetInfo = table;
        }

        private void InitControl()
        {
            Loaded += Window_Loaded;
            mainDataView.OnDataSelectChanged += mainDataView_OnDataSelectChanged;
            ctlMenuShowParentRelation.Checked += ctlShowParentRelation_Checked;
            ctlMenuShowParentRelation.Unchecked += ctlShowParentRelation_Checked;
            ctlMenuShowSubRelation.Checked += ctlShowSubRelation_Checked;
            ctlMenuShowSubRelation.Unchecked += ctlShowSubRelation_Checked;
            ctlMenuShowParentRelationData.Checked += ctlShowParentRelationData_Checked;
            ctlMenuShowParentRelationData.Unchecked += ctlShowParentRelationData_Checked;
            ctlMenuShowSubRelationData.Checked += ctlShowSubRelationData_Checked;
            ctlMenuShowSubRelationData.Unchecked += ctlShowSubRelationData_Checked;
            ctlShowParentRelation.Checked += ShortCheckbox_CheckedChanged;
            ctlShowParentRelation.Unchecked += ShortCheckbox_CheckedChanged;
            ctlShowSubRelation.Checked += ShortCheckbox_CheckedChanged;
            ctlShowSubRelation.Unchecked += ShortCheckbox_CheckedChanged;
            ctlShowParentRelationData.Checked += ShortCheckbox_CheckedChanged;
            ctlShowParentRelationData.Unchecked += ShortCheckbox_CheckedChanged;
            ctlShowSubRelationData.Checked += ShortCheckbox_CheckedChanged;
            ctlShowSubRelationData.Unchecked += ShortCheckbox_CheckedChanged;

            this.ctlShowParentRelation.Tag = this.ctlMenuShowParentRelation;
            this.ctlShowParentRelationData.Tag = this.ctlMenuShowParentRelationData;
            this.ctlShowSubRelation.Tag = this.ctlMenuShowSubRelation;
            this.ctlShowSubRelationData.Tag = this.ctlMenuShowSubRelationData;
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_dataAccessor == null)
                SelectConnect();
            if (_dataAccessor == null)
                return;
            this.Title = string.Format("データビュー：【{0}】", _dataAccessor.DisplayName);
            if (mainDataView.TargetInfo == null)
                SelectTable();
            else
            {
                UpdateTable();
                UpdateRelation();
                UpdateData();
            }
        }

        // Private Sub ctlTableName_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles ctlTableName.MouseUp
        // SelectTable()
        // End Sub

        public void SelectTable()
        {
            SelectTableWindow win = new SelectTableWindow(this._dataAccessor);
            win.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            win.ShowDialog();
            if (win.DialogResult != true || win.SelectedTable == null)
                return;
            mainDataView.TargetInfo = win.SelectedTable;
            UpdateTable();
            UpdateRelation();
            UpdateData();
        }


        private void mainDataView_OnDataSelectChanged(DataViewEx sender, DataViewEx.DataSelectChangeArgs args)
        {
            if (ctlMenuShowParentRelationData.IsChecked)
            {
                foreach (var relationView in _parentRelationViews)
                    relationView.SetKeys(args.Rows);
            }
            if (ctlMenuShowSubRelationData.IsChecked)
            {
                foreach (var relationView in _subRelationViews)
                    relationView.SetKeys(args.Rows);
            }
        }

        private void UpdateRelationTables()
        {
        }

        private void SelectConnect()
        {
            SelectConnect selectConnect = new SelectConnect();
            selectConnect.Owner = this;
            if (!selectConnect.ShowDialog() == true)
            {
                if (_dataAccessor == null)
                    this.Close();
            }
            else
            {
                _dataAccessor = selectConnect.SelectedDataAccessor;
                mainDataView.DataAccessor = _dataAccessor;
            }
        }

        private void UpdateRelation()
        {
            if (!this.IsLoaded)
                return;
            if (_lock)
                return;
            _parentRelationViews.Clear();
            _subRelationViews.Clear();

            ctlSubTableList.Children.Clear();
            ctlParentTableList.Children.Clear();

            ctlSubTableList.RowDefinitions.Clear();
            ctlParentTableList.RowDefinitions.Clear();

            if (!ctlMenuShowParentRelation.IsChecked)
                colParent.Width = new GridLength(0);

            if (!ctlMenuShowSubRelation.IsChecked)
                colSub.Width = new GridLength(0);

            if (!ctlMenuShowSubRelation.IsChecked && !ctlMenuShowParentRelation.IsChecked)
                return;

            foreach (TableRelationInfo relation in mainDataView.TargetInfo.Relations)
            {
                if (!relation.IsVaild())
                    continue;
                var dv = new DataView(mainDataView.TargetInfo, relation);
                dv.HorizontalAlignment = HorizontalAlignment.Stretch;
                Grid targetTableList = default(Grid);
                bool isParent = relation.IsParentTable(mainDataView.TargetInfo);
                if (isParent)
                {
                    if (!ctlMenuShowSubRelation.IsChecked)
                        continue;
                    targetTableList = ctlSubTableList;
                }
                else
                {
                    if (!ctlMenuShowParentRelation.IsChecked)
                        continue;
                    targetTableList = ctlParentTableList;
                }
                RowDefinition rowDefine = new RowDefinition() { Height = new GridLength(120) };
                dv.SetRelationRow(rowDefine);
                targetTableList.RowDefinitions.Add(rowDefine);
                int rowNo = targetTableList.RowDefinitions.Count - 1;
                Grid.SetRow(dv, rowNo);
                targetTableList.Children.Add(dv);
                if (isParent)
                    _subRelationViews.Add(dv);
                else
                    _parentRelationViews.Add(dv);

                GridSplitter splitter = new GridSplitter();
                splitter.Height = 2;
                splitter.ResizeDirection = GridResizeDirection.Rows;
                splitter.VerticalAlignment = VerticalAlignment.Bottom;
                splitter.HorizontalAlignment = HorizontalAlignment.Stretch;
                Grid.SetRow(splitter, rowNo);
                targetTableList.Children.Add(splitter);
            }
            ctlSubTableList.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(10) });
            ctlParentTableList.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(10) });

            if (ctlParentTableList.Children.Count == 0)
                colParent.Width = new GridLength(0);
            else
                colParent.Width = new GridLength(300);
            if (ctlSubTableList.Children.Count == 0)
                colSub.Width = new GridLength(0);
            else
                colSub.Width = new GridLength(300);
        }

        private bool _lock = false;
        private void UpdateTable()
        {
            if (!this.IsLoaded)
                return;

            if (mainDataView.TargetInfo.Relations.Count() > 20)
            {
                _lock = true;
                this.ctlMenuShowParentRelation.IsChecked = false;
                this.ctlMenuShowParentRelationData.IsChecked = false;
                this.ctlMenuShowSubRelation.IsChecked = false;
                this.ctlMenuShowSubRelationData.IsChecked = false;
                this.ctlShowParentRelation.IsChecked = false;
                this.ctlShowParentRelationData.IsChecked = false;
                this.ctlShowSubRelation.IsChecked = false;
                this.ctlShowSubRelationData.IsChecked = false;
                _lock = false;
            }

            mainDataView.TargetInfo = mainDataView.TargetInfo;
            UpdateRelation();
            UpdateRelationData();
        }

        private void UpdateRelationData()
        {
            if (!this.IsLoaded)
                return;
            if (_lock)
                return;
            if (ctlMenuShowParentRelationData.IsChecked)
            {
                foreach (var relationView in _parentRelationViews)
                    relationView.updateData();
            }
            if (ctlMenuShowSubRelationData.IsChecked)
            {
                foreach (var relationView in _subRelationViews)
                    relationView.updateData();
            }
        }

        private void UpdateData()
        {
            mainDataView.UpdateData();
        }

        private void ctlShowParentRelation_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;
            SetShowParentRelation(ctlMenuShowParentRelation.IsChecked);
        }

        private void ctlShowSubRelation_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;
            SetShowSubRelation(ctlMenuShowSubRelation.IsChecked);
        }

        private void ctlShowParentRelationData_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;
            SetShowParentRelationData(ctlMenuShowParentRelationData.IsChecked);
        }

        private void ctlShowSubRelationData_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;
            SetShowSubRelationData(ctlMenuShowSubRelationData.IsChecked);
        }

        private void ShortCheckbox_CheckedChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;
            CheckBox checkbox = (CheckBox)sender;
            MenuItem relationMenuItem = (MenuItem)checkbox.Tag;
            relationMenuItem.IsChecked = checkbox.IsChecked.Value;
        }

        private void SetShowParentRelation(bool isShow)
        {
            ctlMenuShowParentRelation.IsChecked = isShow;
            ctlShowParentRelation.IsChecked = isShow;
            UpdateRelation();
            if (!isShow)
            {
                ctlShowParentRelationData.IsChecked = false;
                ctlShowParentRelationData.IsEnabled = false;
            }
            else
                ctlShowParentRelationData.IsEnabled = true;
        }

        private void SetShowParentRelationData(bool isShow)
        {
            UpdateRelationData();
        }

        private void SetShowSubRelation(bool isShow)
        {
            ctlMenuShowSubRelation.IsChecked = isShow;
            ctlShowSubRelation.IsChecked = isShow;
            UpdateRelation();
            if (!isShow)
            {
                ctlShowSubRelationData.IsChecked = false;
                ctlShowSubRelationData.IsEnabled = false;
            }
            else
                ctlShowSubRelationData.IsEnabled = true;
        }

        private void SetShowSubRelationData(bool isShow)
        {
            UpdateRelationData();
        }

        private void MainTableView_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                UpdateData();
                UpdateRelation();
            }
            else if (e.Key == Key.F2)
                ctlOpenConnectSelect_Click(this, null);
            else if (e.Key == Key.F3)
                ctlOpenTableSelect_Click(this, null);
            else if (e.Key == Key.F4)
                ctlMenuTableChange_Click(this, null);
        }

        private void ctlOpenConnectSelect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainTableView newWindow = new MainTableView(null);
            newWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            newWindow.Show();
        }

        private void ctlOpenTableSelect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainTableView newWindow = new MainTableView(this._dataAccessor);
            newWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            newWindow.Show();
        }

        private void ctlMenuTableChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SelectTable();
        }

        private void ctlMenuShowPackageCreateView_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            IPackageExporter exporter = new PackageExporter(this._dataAccessor);
            DataPackageCreateWindow packageView = new DataPackageCreateWindow(exporter);
            packageView.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            packageView.Show();
        }

        private void SelectTable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelectTable();
        }

        private void ctlExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }

}