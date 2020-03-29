using DebugTools.DBO;
using DebugTools.DataBase;
using DebugTools.Secret; 
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

public partial class ColumnSettingEditor
{
    private IColumnInfo _columnInfo;

    public ColumnSettingEditor(IColumnInfo columnInfo)
    {
        InitializeComponent();
        ctlSave.Click += ctlSave_Click;
        ctlCancel.Click += ctlCancel_Click;
        _columnInfo = columnInfo;
        Update();
    }

    private void Update()
    {
        this.ctlSave.IsEnabled = (_columnInfo.TableInfo) is TableInfo;
        this.lblTableName.Text = _columnInfo.TableInfo.Name;
        this.lblColumnName.Text = _columnInfo.Name + "    " + _columnInfo.DisplayTypeName;
        this.lblComment.Text = _columnInfo.Comment;
        if (!string.IsNullOrWhiteSpace(_columnInfo.Memo))
        {
            var sr = new System.IO.StringReader(_columnInfo.Memo);
            var xr = System.Xml.XmlReader.Create(sr);
            txtMemo.Document = (FlowDocument)System.Windows.Markup.XamlReader.Load(xr);
        }
        else
            txtMemo.Document.Blocks.Clear();
        this.cmbSecretType.SelectedIndex = System.Convert.ToInt32(_columnInfo.SecretType);
        UpdateRelationArea();
    }

    private void UpdateRelationArea()
    {
        this.ctlRelation.Children.Clear();
        foreach (TableRelationInfo relation in _columnInfo.TableInfo.Relations)
        {
            string relationTableName = null;
            if (relation.IsParentTable(this._columnInfo.TableInfo))
            {
                if (relation.Items.Any(x => x.SourceColumnName == this._columnInfo.Name))
                    relationTableName = relation.TargetName;
                else
                    continue;
            }
            else if (relation.Items.Any(x => x.TargetColumnName == this._columnInfo.Name))
                relationTableName = relation.SourceName;
            else
                continue;

            Button relationButton = new Button();
            relationButton.Margin = new Thickness(5);
            relationButton.VerticalAlignment = VerticalAlignment.Stretch;
            relationButton.HorizontalAlignment = HorizontalAlignment.Stretch;
            relationButton.HorizontalContentAlignment = HorizontalAlignment.Left;
            if (relation.IsCustom)
                relationButton.Background = Brushes.LightBlue;
            else
                relationButton.Background = Brushes.Snow;
            relationButton.DataContext = relation;
            relationButton.Content = relation.FKName + System.Environment.NewLine + relationTableName;
            relationButton.Click += relationButton_clicked;
            this.ctlRelation.Children.Add(relationButton);
        }
        Button newRelationButton = new Button();
        newRelationButton.Margin = new Thickness(5);
        newRelationButton.VerticalAlignment = VerticalAlignment.Stretch;
        newRelationButton.HorizontalAlignment = HorizontalAlignment.Stretch;
        newRelationButton.Content = "+新しいカスタム関連";
        newRelationButton.Click += newRelationButton_clicked;
        this.ctlRelation.Children.Add(newRelationButton);
    }

    private void ctlSave_Click(System.Object sender, System.Windows.RoutedEventArgs e)
    {
        if (txtMemo.Document.Blocks.Count > 0)
        {
            string xw = System.Windows.Markup.XamlWriter.Save(txtMemo.Document);
            this._columnInfo.Memo = xw;
        }
        else
            this._columnInfo.Memo = string.Empty;
        this._columnInfo.SecretType = (SecretType)System.Convert.ToInt16(cmbSecretType.SelectedIndex);
        if ((_columnInfo.TableInfo) is TableInfo)
        {
            if (((TableInfo)this._columnInfo.TableInfo).Save())
                this.DialogResult = true;
        }
    }

    private void ctlCancel_Click(System.Object sender, System.Windows.RoutedEventArgs e)
    {
        this.DialogResult = false;
    }

    private void relationButton_clicked(object sender, RoutedEventArgs e)
    {
        Button btn = (Button)sender;
        TableRelationInfo relation = (TableRelationInfo)btn.DataContext;
        if (TableRelationSettingEditor.ShowEditWindow(this, this._columnInfo.TableInfo, relation) == true)
            UpdateRelationArea();
    }

    private void newRelationButton_clicked(object sender, RoutedEventArgs e)
    {
        if (TableRelationSettingEditor.ShowNewWindow(this, this._columnInfo) == true)
            UpdateRelationArea();
    }
}

