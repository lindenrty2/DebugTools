using DebugTools.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace DebugTools.DataBase
{
    public class TableRelationInfo : ITableRelationInfo
    {
        private long _fkid;

        public long FKID
        {
            get
            {
                return _fkid;
            }
        }

        protected string _fkdbname;
        public string FKDBName
        {
            get
            {
                return _fkdbname;
            }
            set
            {
                _fkdbname = value;
            }
        }

        protected string _fkname;
        public string FKName
        {
            get
            {
                return _fkname;
            }
            set
            {
                _fkname = value;
            }
        }

        protected string _sourceDBName;
        public string SourceDBName
        {
            get
            {
                return _sourceDBName;
            }
            set
            {
                _sourceDBName = value;
                if ((_sourceDBName ?? "") == (value ?? ""))
                    return;
                _sourceDBName = value;
                _sourceInfo = null;
            }
        }

        protected string _sourceName;
        public string SourceName
        {
            get
            {
                return _sourceName;
            }
            set
            {
                _sourceName = value;
                if ((_sourceName ?? "") == (value ?? ""))
                    return;
                _sourceName = value;
                _sourceInfo = null;
            }
        }

        protected ITableInfo _sourceInfo;
        public ITableInfo SourceInfo
        {
            get
            {
                if (_sourceInfo == null)
                    _sourceInfo = _dataAccessor.GetTableInfo(_sourceDBName, _sourceName);
                return _sourceInfo;
            }
        }

        protected string _targetDBName;
        public string TargetDBName
        {
            get
            {
                return _targetDBName;
            }
            set
            {
                if ((_targetDBName ?? "") == (value ?? ""))
                    return;
                _targetDBName = value;
                _targetInfo = null;
            }
        }

        protected string _targetName;
        public string TargetName
        {
            get
            {
                return _targetName;
            }
            set
            {
                if ((_targetName ?? "") == (value ?? ""))
                    return;
                _targetName = value;
                _targetInfo = null;
            }
        }

        protected ITableInfo _targetInfo;
        public ITableInfo TargetInfo
        {
            get
            {
                if (_targetInfo == null && !string.IsNullOrWhiteSpace(_targetName))
                    _targetInfo = _dataAccessor.GetTableInfo(_targetDBName, _targetName);
                return _targetInfo;
            }
        }

        protected IEnumerable<ITableRelationItemInfo> _items;
        public IEnumerable<ITableRelationItemInfo> Items
        {
            get
            {
                if (_items == null)
                    _items = _dataAccessor.GetFKInfo(this);
                return _items;
            }
            set
            {
                _items = value;
            }
        }

        public virtual bool IsCustom
        {
            get
            {
                return false;
            }
        }

        protected bool _isOutputRelation;
        public bool IsOutputRelation
        {
            get
            {
                return _isOutputRelation;
            }
            set
            {
                _isOutputRelation = value;
            }
        }

        private IDataAccessor _dataAccessor;
        public IDataAccessor DataAccessor
        {
            get
            {
                return _dataAccessor;
            }
        }

        public TableRelationInfo(IDataAccessor dataAccessor)
        {
            _dataAccessor = dataAccessor;
            _fkid = DateTime.Now.Ticks;
        }

        public TableRelationInfo(IDataAccessor dataAccessor, System.Xml.XmlNode node)
        {
            _dataAccessor = dataAccessor;
            _fkid = System.Convert.ToInt64(XmlHelper.GetAttributeValue(node, "FKId"));
            _fkdbname = XmlHelper.GetAttributeValue(node, "FKDBName");
            _fkname = XmlHelper.GetAttributeValue(node, "FKName");
            _sourceDBName = XmlHelper.GetAttributeValue(node, "SourceDBName");
            _sourceName = XmlHelper.GetAttributeValue(node, "SourceName");
            _targetDBName = XmlHelper.GetAttributeValue(node, "TargetDBName");
            _targetName = XmlHelper.GetAttributeValue(node, "TargetName");

            List<ITableRelationItemInfo> relationItems = new List<ITableRelationItemInfo>();
            foreach (System.Xml.XmlNode relationColumnNode in node.FirstChild.ChildNodes)
                relationItems.Add(new TableRelationItem(this, relationColumnNode));
            this._items = relationItems;
            SetCustomInfo(node);
        }

        public TableRelationInfo(IDataAccessor dataAccessor, DataRow row)
        {
            _dataAccessor = dataAccessor;
            _fkid = System.Convert.ToInt32(row["FKId"]);
            _fkdbname = row["FKDBName"].ToString();
            _fkname = row["FKName"].ToString();
            _sourceDBName = row["SourceDBName"].ToString();
            _sourceName = row["SourceName"].ToString();
            _targetName = row["TargetName"].ToString();
            _targetDBName = row["TargetDBName"].ToString();
        }

        public TableRelationInfo(IDataAccessor dataAccessor, DataRow row, System.Xml.XmlNode node) : this(dataAccessor, row)
        {
            this.SetCustomInfo(node);
        }

        public virtual void SetCustomInfo(System.Xml.XmlNode node)
        {
            _isOutputRelation = XmlHelper.GetBooleanAttributeValue(node, "IsOutputRelation", false);
        }

        public bool Save(XmlNode node)
        {
            node.Attributes.Append(XmlHelper.CreateAttribute(node.OwnerDocument, "FKId", _fkid));
            node.Attributes.Append(XmlHelper.CreateAttribute(node.OwnerDocument, "FKDBName", _fkdbname));
            node.Attributes.Append(XmlHelper.CreateAttribute(node.OwnerDocument, "FKName", _fkname));
            node.Attributes.Append(XmlHelper.CreateAttribute(node.OwnerDocument, "SourceDBName", _sourceDBName));
            node.Attributes.Append(XmlHelper.CreateAttribute(node.OwnerDocument, "SourceName", _sourceName));
            node.Attributes.Append(XmlHelper.CreateAttribute(node.OwnerDocument, "TargetDBName", _targetDBName));
            node.Attributes.Append(XmlHelper.CreateAttribute(node.OwnerDocument, "TargetName", _targetName));
            node.Attributes.Append(XmlHelper.CreateAttribute(node.OwnerDocument, "IsCustom", this.IsCustom));
            node.Attributes.Append(XmlHelper.CreateAttribute(node.OwnerDocument, "IsOutputRelation", _isOutputRelation));

            XmlNode columnsNode = node.OwnerDocument.CreateNode(XmlNodeType.Element, "Columns", "");
            node.AppendChild(columnsNode);
            foreach (TableRelationItem item in this.Items)
            {
                XmlNode columnNode = node.OwnerDocument.CreateNode(XmlNodeType.Element, "Column", "");
                item.Save(columnNode);
                columnsNode.AppendChild(columnNode);
            }
            return true;
        }

        public bool IsParentTable(ITableInfo tableInfo)
        {
            return (tableInfo.Name ?? "") == (this.SourceName ?? "");
        }

        public ITableInfo GetTargetTableInfo(bool isParentView)
        {
            if (isParentView)
                return TargetInfo;
            else
                return SourceInfo;
        }

        public ISqlConditionGroup CreateWhere(bool isParent, IEnumerable<DataRow> rows)
        {
            SqlConditionGroup where = new SqlConditionGroup(false);
            ITableInfo tableAInfo = this.GetTargetTableInfo(isParent);

            foreach (DataRow row in rows)
            {
                SqlConditionGroup sqlGroup = new SqlConditionGroup(true);
                foreach (TableRelationItem item in Items)
                {
                    string columnA = item.GetTargetColumn(isParent);
                    IColumnInfo columnAInfo = tableAInfo.GetColumn(columnA);
                    if(columnAInfo == null)
                    {
                        //MessageBox.Show($"{tableAInfo.Name}.{columnA}找不到");
                        throw new KeyNotFoundException($"{tableAInfo.Name}.{columnA}找不到");
                    }
                    string columnB = item.GetTargetColumn(!isParent);
                    object value = row[columnB];
                    sqlGroup.Add(new SqlCondition(columnAInfo, value, false, "=", default(System.ComponentModel.ListSortDirection?)));
                }
                where.Add(sqlGroup);
            }
            return where;
        }

        public bool IsTwoWay()
        {
            return TargetInfo.ContainsRelation(this.FKName) && SourceInfo.ContainsRelation(this.FKName);
        }

        public bool IsVaild()
        {
            return TargetInfo != null && SourceInfo != null;
        }
    }

    public class CustomTableRelation : TableRelationInfo
    {
        public override bool IsCustom
        {
            get
            {
                return true;
            }
        }

        public CustomTableRelation(IDataAccessor dataAccessor) : base(dataAccessor)
        {
        }

        public CustomTableRelation(IDataAccessor dataAccessor, XmlNode node) : base(dataAccessor, node)
        {
        }

        public CustomTableRelation(IColumnInfo columnInfo) : this(columnInfo.TableInfo)
        {
            List<TableRelationItem> items = new List<TableRelationItem>();
            items.Add(new TableRelationItem(this, columnInfo.Name));
            _items = items;
        }

        public CustomTableRelation(ITableInfo tableInfo) : base(tableInfo.DataAccessor)
        {
            this._sourceName = tableInfo.Name;
        }

        public override void SetCustomInfo(System.Xml.XmlNode node)
        {
            base.SetCustomInfo(node);
        }
    }

    public class TableRelationItem : ITableRelationItemInfo
    {
        private ITableRelationInfo _tableRelation;
        public ITableRelationInfo TableRelation
        {
            get
            {
                return _tableRelation;
            }
        }

        private string _sourceName;
        public string SourceColumnName
        {
            get
            {
                return _sourceName;
            }
            set
            {
                _sourceName = value;
            }
        }

        private string _targetName;
        public string TargetColumnName
        {
            get
            {
                return _targetName;
            }
            set
            {
                _targetName = value;
            }
        }

        public string GetTargetColumn(bool isParentView)
        {
            if (isParentView)
                return TargetColumnName;
            else
                return SourceColumnName;
        }

        /// <summary>
    /// 新規用
    /// </summary>
    /// <param name="relation"></param>
    /// <param name="sourceName"></param>
    /// <remarks></remarks>
        public TableRelationItem(ITableRelationInfo relation, string sourceName)
        {
            _tableRelation = relation;
            _sourceName = sourceName;
        }

        /// <summary>
    /// 登録済データ
    /// </summary>
    /// <param name="relation"></param>
    /// <param name="row"></param>
    /// <remarks></remarks>
        public TableRelationItem(ITableRelationInfo relation, DataRow row)
        {
            _tableRelation = relation;
            _sourceName = row["SourceName"].ToString();
            _targetName = row["TargetName"].ToString();
        }

        public TableRelationItem(ITableRelationInfo relation, DataRow row, System.Xml.XmlNode node) : this(relation, row)
        {
            this.SetCustomInfo(node);
        }

        public TableRelationItem(ITableRelationInfo relation, System.Xml.XmlNode node)
        {
            _tableRelation = relation;
            _sourceName = XmlHelper.GetAttributeValue(node, "SourceName");
            _targetName = XmlHelper.GetAttributeValue(node, "TargetName");
            this.SetCustomInfo(node);
        }

        public void SetCustomInfo(System.Xml.XmlNode node)
        {
            if (node == null)
                return;
        }

        public bool Save(XmlNode node)
        {
            node.Attributes.Append(XmlHelper.CreateAttribute(node.OwnerDocument, "SourceName", _sourceName));
            node.Attributes.Append(XmlHelper.CreateAttribute(node.OwnerDocument, "TargetName", _targetName));
            return true;
        }
    }
}
