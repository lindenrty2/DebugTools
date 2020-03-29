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
using System.ComponentModel;
using DebugTools.Secret;
using DebugTools.DataBase;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace DebugTools.DBO
{
    public partial class DataWhereView
    {
        public event Action<DataWhereView> OnChange;

        private IColumnInfo _column;
        public IColumnInfo Column
        {
            get
            {
                return _column;
            }
        }

        private ListSortDirection? _sortDirection;
        public ListSortDirection? SortDirection
        {
            get
            {
                return _sortDirection;
            }
            set
            {
                _sortDirection = value;
            }
        }

        private string _oldValue;

        protected DataWhereView()
        {
            this.InitializeComponent();
            ctlOperater.SelectionChanged += ctlOperater_SelectionChanged;
            ctlValue.LostFocus += ctlValue_LostFocus;
            ctlValue.PreviewKeyDown += ctlValue_PreviewKeyDown;
            ctlNull.Checked += ctlNull_CheckChanged;
            ctlNull.Unchecked += ctlNull_CheckChanged;
            ctlNotNull.Checked += ctlNull_CheckChanged;
            ctlNotNull.Unchecked += ctlNull_CheckChanged;
            ctlValue.TextChanged += ctlValue_TextChanged;

        }

        public DataWhereView(IColumnInfo column, SqlCondition sw) : this()
        {
            InitializeComponent();
            _column = column;
            this.UpdateInfo();
            if (sw != null)
            {
                if (sw.Value == null)
                    this.ctlNull.IsChecked = true;
                else
                {
                    this._oldValue = sw.ValueStr;
                    this.ctlValue.Text = sw.ValueStr;
                }

                var loopTo = ctlOperater.Items.Count;
                for (int index = 0; index <= loopTo; index++)
                {
                    ComboBoxItem item = (ComboBoxItem)ctlOperater.Items[index];
                    if (item.Content.Equals(sw.Operater))
                    {
                        ctlOperater.SelectedIndex = index;
                        break;
                    }
                }
            }
            else if (column.IsTextType())
                ctlOperater.SelectedIndex = 0;
            else
                ctlOperater.SelectedIndex = 1;
        }

        private void UpdateInfo()
        {
            if (Column.IsPK)
            {
                ctlKeyType.Text = "主键";
                ctlName.Foreground = Brushes.Blue;
            }
            else
                ctlName.Foreground = Brushes.Black;
            ctlName.Text = Column.Name;
            if (!string.IsNullOrWhiteSpace(_column.Memo))
            {
                ctlName.Foreground = Brushes.Green;
                ctlName.TextDecorations = TextDecorations.Underline;
            }
            else
            {
            }
            if (Column.SecretType == SecretType.Level1)
                ctlName.Foreground = Brushes.DarkRed;
            else if (Column.SecretType == SecretType.Level2)
                ctlName.Foreground = Brushes.OrangeRed;
            else if (Column.SecretType == SecretType.Level3)
                ctlName.Foreground = Brushes.Red;
            ctlType.Text = Column.DisplayTypeName;
            ctlName.ToolTip = Column.Comment;
        }

        private void ctlOperater_SelectionChanged(System.Object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CheckValueTextBox();
            OnInputChanged();
        }

        private void ctlValue_LostFocus(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            CheckValueTextBox();
            if (ctlValue.Text == _oldValue)
                return;
            _oldValue = ctlValue.Text;
            OnInputChanged();
        }

        private void ctlValue_PreviewKeyDown(System.Object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            CheckValueTextBox();
            if (ctlValue.Text == _oldValue)
                return;
            _oldValue = ctlValue.Text;
            OnInputChanged();
        }

        private void ctlNull_CheckChanged(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            ctlValue.IsEnabled = (ctlNull.IsChecked == true || ctlNotNull.IsChecked == true) ? false : true;
            if (((CheckBox)sender).Name == ctlNull.Name && ctlNull.IsChecked == true)
                ctlNotNull.IsChecked = false;
            if (((CheckBox)sender).Name == ctlNotNull.Name && ctlNotNull.IsChecked == true)
                ctlNull.IsChecked = false;
            OnInputChanged();
        }

        private void OnInputChanged()
        {
            OnChange?.Invoke(this);
        }

        public ISqlCondition GetSQLWhere()
        {
            if (ctlValue.Text.Length == 0 && ctlNull.IsChecked != true && ctlNotNull.IsChecked != true)
            {
                if (this.SortDirection.HasValue)
                    return new SqlCondition(this._column, this.SortDirection);
                else
                    return null;
            }
            else if (ctlNull.IsChecked == true)
            {
                SqlConditionGroup group = new SqlConditionGroup(false);
                group.Add(new SqlCondition(this._column, null, false, "IS", this.SortDirection));
                group.Add(new SqlCondition(this._column, "", false, "=", this.SortDirection));
                return group;
            }
            else if (ctlNotNull.IsChecked == true)
            {
                SqlConditionGroup group = new SqlConditionGroup(true);
                group.Add(new SqlCondition(this._column, null, false, "IS NOT", this.SortDirection));
                group.Add(new SqlCondition(this._column, "", false, "<>", this.SortDirection));
                return group;
            }
            else
                return new SqlCondition(this._column, ctlValue.Text, ctlRaw.IsChecked.Value, ((ComboBoxItem)this.ctlOperater.SelectedValue).Content.ToString(), this.SortDirection);
        }

        private void ctlValue_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!this.IsLoaded)
                return;
            CheckValueTextBox();
        }

        private void CheckValueTextBox()
        {
            if (!this.IsLoaded)
                return;
            if (!_column.IsNumberType())
                return;
            string op = ((ComboBoxItem)this.ctlOperater.SelectedValue).Content.ToString();
            if (op.ToUpper().Equals("IN"))
                return;

            decimal result = default(decimal);
            if (decimal.TryParse(ctlValue.Text, out result))
                ctlValue.Text = result.ToString();
            else
                ctlValue.Text = "";
        }

        private void ctlName_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ColumnSettingEditor columnEdit = new ColumnSettingEditor(_column);
            if (columnEdit.ShowDialog() == true)
                this.UpdateInfo();
        }
    }

}