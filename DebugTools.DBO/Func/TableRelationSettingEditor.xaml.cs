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
using DebugTools.DataBase;

namespace DebugTools.DBO
{
    public partial class TableRelationSettingEditor
    {
        private ITableInfo _tableInfo;
        private ITableRelationInfo _tableRelation;
        private ITableInfo _selectedSourceTable = null;
        private ITableInfo _selectedTargetTable = null;
        private bool _isNew = false;

        public TableRelationSettingEditor(ITableInfo tableInfo, ITableRelationInfo relation, bool isNew)
        {
            InitializeComponent();

            _tableInfo = tableInfo;
            _tableRelation = relation;
            _isNew = isNew;
            Update();
        }

        private void Update()
        {
            ctlIsCustom.Content = _tableRelation.IsCustom ? "然り" : "否";
            ctlRelationName.IsEnabled = _tableRelation.IsCustom;
            ctlRelationName.Text = _tableRelation.FKName;
            if (_tableRelation.FKName == null)
                ctlRelationName.Text = string.Format("{0}_{1}", _tableRelation.SourceName, _tableRelation.TargetName);
            ctlSelectSourceTable.IsEnabled = _tableRelation.IsCustom;
            ctlSourceTable.Content = _tableRelation.SourceName;
            _selectedSourceTable = _tableRelation.SourceInfo;

            ctlSelectTargetTable.IsEnabled = _tableRelation.IsCustom;
            if (string.IsNullOrWhiteSpace(_tableRelation.TargetName))
            {
                ctlTargetTable.Content = "未指定";
                _selectedTargetTable = null;
            }
            else
            {
                ctlTargetTable.Content = _tableRelation.TargetName;
                _selectedTargetTable = _tableRelation.TargetInfo;
            }

            if (_tableInfo.IsAutoRelationOutput)
                ctlIsOutputRelation.IsChecked = default(bool?);
            else
                ctlIsOutputRelation.IsChecked = _tableRelation.IsOutputRelation;
            foreach (TableRelationItem item in _tableRelation.Items)
            {
                TableRelationItemView itemView = new TableRelationItemView(item);
                ctlRelationColumnList.Children.Add(itemView);
            }
            if (_tableRelation.IsCustom)
            {
                if (_isNew)
                    ctlTwoWay.IsChecked = true;
                else
                    ctlTwoWay.IsChecked = _tableRelation.IsTwoWay();
                ctlSelectSourceTable.IsEnabled = _selectedSourceTable == null || (_selectedSourceTable.Name ?? "") != (_tableInfo.Name ?? "");
                ctlSelectTargetTable.IsEnabled = _selectedTargetTable == null || (_selectedTargetTable.Name ?? "") != (_tableInfo.Name ?? "");
                Button addButton = new Button();
                addButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                addButton.Content = "＋ 関連列";
                addButton.Click += addRelationButton_Click;
                ctlRelationColumnList.Children.Add(addButton);
            }
            else
            {
                ctlTwoWay.IsEnabled = false;
                ctlTwoWay.IsChecked = true;
            }
        }

        private void ctlSelectSourceTable_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            SelectTableWindow win = new SelectTableWindow(_tableInfo.DataAccessor);
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.ShowDialog();
            if (win.DialogResult == true && win.SelectedTable != null)
            {
                if (IsAutoFKName())
                    ctlRelationName.Text = string.Format("{0}_{1}", win.SelectedTable.Name, _selectedTargetTable.Name);
                _selectedSourceTable = win.SelectedTable;
                ctlSourceTable.Content = _selectedSourceTable.Name;
                foreach (object itemView in ctlRelationColumnList.Children)
                {
                    if (itemView is TableRelationItemView)
                        ((TableRelationItemView)itemView).ChangeSourceTable(_selectedSourceTable);
                }
            }
        }

        private void ctlSelectTargetTable_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            SelectTableWindow win = new SelectTableWindow(this._tableInfo.DataAccessor);
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.ShowDialog();
            if (win.DialogResult == true && win.SelectedTable != null)
            {
                if (IsAutoFKName())
                    ctlRelationName.Text = string.Format("{0}_{1}", _selectedSourceTable.Name, win.SelectedTable.Name);
                _selectedTargetTable = win.SelectedTable;
                ctlTargetTable.Content = _selectedTargetTable.Name;
                foreach (object itemView in ctlRelationColumnList.Children)
                {
                    if (itemView is TableRelationItemView)
                        ((TableRelationItemView)itemView).ChangeTargetTable(_selectedTargetTable);
                }
            }
        }

        private bool IsAutoFKName()
        {
            if (string.IsNullOrWhiteSpace(ctlRelationName.Text))
                return true;
            string autoName = string.Format("{0}_{1}", _selectedSourceTable == null ? string.Empty : _selectedSourceTable.Name, _selectedTargetTable == null ? string.Empty : _selectedTargetTable.Name);
            return (autoName ?? "") == (ctlRelationName.Text ?? "");
        }

        private void ctlCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ctlOK_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ctlRelationName.Text))
            {
                MessageBox.Show("関連名未入力");
                return;
            }
            if (_selectedSourceTable == null)
            {
                MessageBox.Show("関連元未指定");
                return;
            }
            if (_selectedTargetTable == null)
            {
                MessageBox.Show("関連先未指定");
                return;
            }
            bool hasError = false;
            List<ITableRelationItemInfo> relationItemList = new List<ITableRelationItemInfo>();
            foreach (object childView in ctlRelationColumnList.Children)
            {
                if (childView is TableRelationItemView)
                {
                    TableRelationItemView relationItemView = (TableRelationItemView)childView;
                    if (!relationItemView.IsVaild())
                    {
                        relationItemView.BorderThickness = new Thickness((double)1);
                        relationItemView.BorderBrush = Brushes.Red;
                        hasError = true;
                    }
                    else
                        relationItemList.Add(relationItemView.CreateRelationItem());
                }
            }
            if (relationItemList.Count == 0)
            {
                MessageBox.Show("最低一個の関連項目を設定してください");
                return;
            }
            if (hasError)
            {
                MessageBox.Show("関連関係設定エラー");
                return;
            }
            if (this._tableRelation.IsCustom)
            {
                string oldName = _tableRelation.FKName;

                string relationName = ctlRelationName.Text;
                ITableInfo sourceTableInfo = ((_selectedSourceTable.Name ?? "") == (_tableInfo.Name ?? "")) ? _tableInfo : _selectedSourceTable;
                ITableRelationInfo exsitRelation = sourceTableInfo.GetRelation(relationName);
                if (exsitRelation != null && exsitRelation.FKID != this._tableRelation.FKID)
                {
                    MessageBox.Show("同じ名称の関連関係が存在する、関連名を修正してください");
                    return;
                }
                ITableInfo targetTableInfo = ((_selectedTargetTable.Name ?? "") == (_tableInfo.Name ?? "")) ? _tableInfo : _selectedTargetTable;
                exsitRelation = targetTableInfo.GetRelation(relationName);
                if (exsitRelation != null && exsitRelation.FKID != this._tableRelation.FKID)
                {
                    MessageBox.Show("同じ名称の関連関係が存在する、関連名を修正してください");
                    return;
                }

                _tableRelation.FKName = ctlRelationName.Text;
                _tableRelation.SourceDBName = _selectedSourceTable.DBName;
                _tableRelation.SourceName = _selectedSourceTable.Name;
                _tableRelation.TargetDBName = _selectedTargetTable.DBName;
                _tableRelation.TargetName = _selectedTargetTable.Name;
                _tableRelation.Items = relationItemList;

                bool sourceTableSave = ctlTwoWay.IsChecked.Value || (_tableRelation.SourceInfo != null && (_selectedSourceTable.Name ?? "") == (_tableInfo.Name ?? ""));
                bool targetTableSave = ctlTwoWay.IsChecked.Value || (_tableRelation.TargetInfo != null && (_selectedTargetTable.Name ?? "") == (_tableInfo.Name ?? ""));

                if (_isNew)
                {
                    if (sourceTableSave)
                        _tableRelation.SourceInfo.AddRelation(_tableRelation);
                    if (targetTableSave)
                        _tableRelation.TargetInfo.AddRelation(_tableRelation);
                }
                else
                {
                    if (sourceTableSave)
                        _tableRelation.SourceInfo.ChangeRelation(oldName, _tableRelation);
                    if (targetTableSave)
                        _tableRelation.TargetInfo.ChangeRelation(oldName, _tableRelation);
                }
                if (sourceTableSave)
                {
                    if ((_tableRelation.SourceInfo) is TableInfo)
                        ((TableInfo)_tableRelation.SourceInfo).Save();
                }

                if (targetTableSave)
                {
                    if ((_tableRelation.SourceInfo) is TableInfo)
                        ((TableInfo)_tableRelation.TargetInfo).Save();
                }
            }
            if (ctlIsOutputRelation.IsChecked == true)
                this._tableRelation.IsOutputRelation = ctlIsOutputRelation.IsChecked.Value;
            this.DialogResult = true;
        }

        private void addRelationButton_Click(object sender, RoutedEventArgs e)
        {
            TableRelationItemView newItem = new TableRelationItemView(new TableRelationItem(this._tableRelation, ""), _selectedSourceTable, _selectedTargetTable);
            ctlRelationColumnList.Children.Insert(ctlRelationColumnList.Children.Count - 1, newItem);
        }

        public static bool? ShowEditWindow(Window parent, ITableInfo tableInfo, TableRelationInfo relation)
        {
            TableRelationSettingEditor relationEditor = new TableRelationSettingEditor(tableInfo, relation, false);
            relationEditor._isNew = false;
            relationEditor.Owner = parent;
            if (relationEditor.ShowDialog() == true)
            {
            }
            return relationEditor.DialogResult;
        }

        public static bool? ShowNewWindow(Window parent, ITableInfo tableInfo)
        {
            CustomTableRelation tableRelation = new CustomTableRelation(tableInfo);
            TableRelationSettingEditor relationEditor = new TableRelationSettingEditor(tableInfo, tableRelation, true);
            if (relationEditor.ShowDialog() == true)
            {
            }
            return relationEditor.DialogResult;
        }

        public static bool? ShowNewWindow(Window parent, IColumnInfo columnInfo)
        {
            CustomTableRelation tableRelation = new CustomTableRelation(columnInfo);
            TableRelationSettingEditor relationEditor = new TableRelationSettingEditor(columnInfo.TableInfo, tableRelation, true);
            if (relationEditor.ShowDialog() == true)
            {
            }
            return relationEditor.DialogResult;
        }
    }
}
