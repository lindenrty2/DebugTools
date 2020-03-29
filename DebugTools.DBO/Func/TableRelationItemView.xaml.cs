using System.Windows.Controls;
using System.Xml.Linq;
using DebugTools.DataBase;

namespace DebugTools.DBO
{
    public partial class TableRelationItemView
    {
        private ITableInfo _sourceTable;
        private ITableInfo _targetTable;
        private string _sourceColumnName;
        private string _targetColumnName;
        private bool _isCustom;
        private ITableRelationItemInfo _relationItem;

        public TableRelationItemView(ITableRelationItemInfo relationItem, ITableInfo sourceTable, ITableInfo targetTable)
        {
            InitializeComponent();
            _relationItem = relationItem;
            _sourceTable = sourceTable;
            _sourceColumnName = relationItem.SourceColumnName;
            _targetTable = targetTable;
            _targetColumnName = relationItem.TargetColumnName;
            _isCustom = relationItem.TableRelation.IsCustom;
            Update();
        }

        public TableRelationItemView(ITableRelationItemInfo relationItem)
        {
            InitializeComponent();
            _relationItem = relationItem;
            _sourceTable = relationItem.TableRelation.SourceInfo;
            _sourceColumnName = relationItem.SourceColumnName;
            _targetTable = relationItem.TableRelation.TargetInfo;
            _targetColumnName = relationItem.TargetColumnName;
            _isCustom = relationItem.TableRelation.IsCustom;
            Update();
        }

        public void ChangeTargetTable(ITableInfo tableInfo)
        {
            if (tableInfo == null)
                ctlTarget.ItemsSource = null;
            else
                ctlTarget.ItemsSource = tableInfo.Columns;
        }

        public void ChangeSourceTable(ITableInfo tableInfo)
        {
            if (tableInfo == null)
                ctlSource.ItemsSource = null;
            else
                ctlSource.ItemsSource = tableInfo.Columns;
        }

        private void Update()
        {
            if (_isCustom)
            {
                ctlSource.IsEditable = false;
                ctlTarget.IsEditable = false;
                if (_sourceTable != null)
                    ctlSource.ItemsSource = _sourceTable.Columns;
                if (_targetTable != null)
                    ctlTarget.ItemsSource = _targetTable.Columns;
            }
            else
            {
                ctlSource.IsEditable = true;
                ctlTarget.IsEditable = true;
                ctlSource.IsEnabled = false;
                ctlTarget.IsEnabled = false;
                ctlDelete.IsEnabled = false;
            }
            ctlSource.Text = _sourceColumnName;
            ctlTarget.Text = _targetColumnName;
        }

        public bool IsVaild()
        {
            if (ctlSource.SelectedIndex == -1 || ctlTarget.SelectedIndex == -1)
                return false;
            return true;
        }

        public ITableRelationItemInfo CreateRelationItem()
        {
            _relationItem.SourceColumnName = ctlSource.Text;
            _relationItem.TargetColumnName = ctlTarget.Text;
            return _relationItem;
        }

        private void ctlDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Panel pc = (Panel)this.Parent;
            pc.Children.Remove(this);
        }
    }
}
