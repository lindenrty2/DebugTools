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
using System.ComponentModel;
using DebugTools.DataBase;

namespace DebugTools.DBO
{
    public partial class ConditionView
    {
        public event Action<ConditionView> OnChange;

        private ColumnInfo _column;
        public ColumnInfo Column
        {
            get
            {
                return _column;
            }
        }

        private string _oldValue;

        public ConditionView()
        {
            InitializeComponent();
            ctlOperater.SelectionChanged += ctlOperater_SelectionChanged;
            ctlValue.LostFocus += ctlValue_LostFocus;
            ctlValue.PreviewKeyDown += ctlValue_PreviewKeyDown;
            ctlNull.Checked += ctlNull_CheckChanged;
            ctlNull.Unchecked += ctlNull_CheckChanged;
            ctlNotNull.Checked += ctlNull_CheckChanged;
            ctlNotNull.Unchecked += ctlNull_CheckChanged;
            ctlValue.TextChanged += ctlValue_TextChanged;
            ctlName.PreviewMouseUp += ctlName_MouseUp;


        }

        public ConditionView(ColumnInfo column, SqlCondition sw) : this()
        {
            _column = column;
            this.UpdateInfo();
            if (sw != null)
            {
                if (sw.Value == null)
                    this.ctlNull.IsChecked = true;
                else
                    this.ctlValue.Text = sw.ValueStr;
                ctlOperater.SelectedValue = sw;
            }
        }

        private void UpdateInfo()
        {
            ctlName.Text = Column.Name;
            ctlName.ItemsSource = Column.TableInfo.Columns;
            ctlName.ToolTip = Column.Comment;
        }

        private void ctlOperater_SelectionChanged(System.Object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            OnInputChanged();
        }

        private void ctlValue_LostFocus(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            if ((ctlValue.Text ?? "") == (_oldValue ?? ""))
                return;
            _oldValue = ctlValue.Text;
            OnInputChanged();
        }

        private void ctlValue_PreviewKeyDown(System.Object sender, System.Windows.Input.KeyEventArgs e)
        {
            if ((int)e.Key != (int)Key.Enter)
                return;
            if ((ctlValue.Text ?? "") == (_oldValue ?? ""))
                return;
            _oldValue = ctlValue.Text;
            OnInputChanged();
        }

        private void ctlNull_CheckChanged(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            ctlValue.IsEnabled = (ctlNull.IsChecked == true || ctlNotNull.IsChecked == true) ? false : true;
            if ((((CheckBox)sender).Name ?? "") == (ctlNull.Name ?? "") && ctlNull.IsChecked == true)
                ctlNotNull.IsChecked = false;
            if ((((CheckBox)sender).Name ?? "") == (ctlNotNull.Name ?? "") && ctlNotNull.IsChecked == true)
                ctlNull.IsChecked = false;
            OnInputChanged();
        }

        private void OnInputChanged()
        {
            OnChange?.Invoke(this);
        }

        public ISqlCondition GetCondition()
        {
            if (ctlValue.Text.Length == 0 && ctlNull.IsChecked != true && ctlNotNull.IsChecked != true)
                return null;
            else if (ctlNull.IsChecked == true)
            {
                SqlConditionGroup group = new SqlConditionGroup(false);
                group.Add(new SqlCondition(this._column, null, false, "IS", default(ListSortDirection?)));
                group.Add(new SqlCondition(this._column, "", false, "=", default(ListSortDirection?)));
                return group;
            }
            else if (ctlNotNull.IsChecked == true)
            {
                SqlConditionGroup group = new SqlConditionGroup(true);
                group.Add(new SqlCondition(this._column, null, false, "IS NOT", default(ListSortDirection?)));
                group.Add(new SqlCondition(this._column, "", false, "<>", default(ListSortDirection?)));
                return group;
            }
            else
                return new SqlCondition(this._column, ctlValue.Text, ctlRaw.IsChecked.Value, ((ComboBoxItem)this.ctlOperater.SelectedValue).Content.ToString(), default(ListSortDirection?));
        }

        private void ctlValue_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (_column.IsNumberType())
            {
                decimal result;
                if (decimal.TryParse(ctlValue.Text, out result))
                    ctlValue.Text = result.ToString();
                else
                    ctlValue.Text = "";
            }
        }

        private void ctlName_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ColumnSettingEditor columnEdit = new ColumnSettingEditor(_column);
            if (columnEdit.ShowDialog() == true)
                this.UpdateInfo();
        }
    }
}
