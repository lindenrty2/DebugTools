using System.Collections.Generic;
using System;
using System.ComponentModel;
using DebugTools.DataBase;
using DebugTools.Helper;

namespace DebugTools.DataBase
{
    public class SqlCondition : ISqlCondition
    {
        private IColumnInfo _column;
        public IColumnInfo Column
        {
            get
            {
                return _column;
            }
        }

        public string DBName
        {
            get
            {
                if (_column == null) return string.Empty;
                return _column.TableInfo.DBName;
            }
        }

        public string TableName
        {
            get
            {
                if (_column == null) return string.Empty;
                return _column.TableInfo.Name;
            }
        }

        private string _columnName;
        public string ColumnName
        {
            get
            {
                if (_column == null) return string.Empty;
                return _column.Name;
            }
        }

        private object _value;
        public object Value
        {
            get
            {
                return _value;
            }
        }

        public string ValueStr
        {
            get
            {
                return _value == null ? "" : _value.ToString();
            }
        }

        public System.Type ValueType
        {
            get
            {
                if (_column == null) return null;
                return _column.DataType;
            }
        }

        private bool _isRaw = false;
        public bool IsRaw
        {
            get
            {
                return _isRaw;
            }
        }

        private string _operater;
        public string Operater
        {
            get
            {
                return _operater;
            }
        }

        private ListSortDirection? _sortDirection;
        public ListSortDirection? SortDirection
        {
            get
            {
                return _sortDirection;
            }
        }

        public SqlCondition(IColumnInfo column, ListSortDirection? sort)
        {
            _column = column;
            _value = null;
            _isRaw = true;
            _operater = null;
            _sortDirection = sort;
        }

        public SqlCondition(IColumnInfo column, object value, bool isRaw, string operater, ListSortDirection? sort)
        {
            _column = column;
            _value = value;
            _isRaw = isRaw;
            _operater = operater;
            _sortDirection = sort;
        }

        public static ISqlCondition CreateSqlCondition(ITableRelationInfo relation, System.Xml.XmlNode node)
        {
            string tableName = XmlHelper.GetAttributeValue(node, "TableName");
            string columnName = XmlHelper.GetAttributeValue(node, "ColumnName");
            ITableInfo tableInfo = null;
            if ((tableName ?? "") == (relation.SourceName ?? ""))
                tableInfo = relation.SourceInfo;
            else if ((tableName ?? "") == (relation.SourceName ?? ""))
                tableInfo = relation.TargetInfo;
            else
                return null;
            IColumnInfo columnInfo = tableInfo.GetColumn(columnName);
            if (columnInfo == null)
                return null;
            string value = XmlHelper.GetAttributeValue(node, "Value");
            bool isRaw = XmlHelper.GetBooleanAttributeValue(node, "IsRaw", false);
            bool @null = XmlHelper.GetBooleanAttributeValue(node, "Null", false);
            string op = XmlHelper.GetAttributeValue(node, "Operater");
            return new SqlCondition(columnInfo, @null ? null : value, isRaw, op, default(ListSortDirection?));
        }

        public void Save(System.Xml.XmlNode node)
        {
            node.Attributes.Append(XmlHelper.CreateAttribute(node.OwnerDocument, "TableName", TableName));
            node.Attributes.Append(XmlHelper.CreateAttribute(node.OwnerDocument, "ColumnName", ColumnName));
            node.Attributes.Append(XmlHelper.CreateAttribute(node.OwnerDocument, "Value", ValueStr));
            node.Attributes.Append(XmlHelper.CreateAttribute(node.OwnerDocument, "Null", this.Value == null));
            node.Attributes.Append(XmlHelper.CreateAttribute(node.OwnerDocument, "IsRaw", IsRaw));
            node.Attributes.Append(XmlHelper.CreateAttribute(node.OwnerDocument, "Operater", Operater));
        }

        public string ToSQLConditionString(ISqlStyle sqlStyle)
        {
            if (string.IsNullOrWhiteSpace(Operater))
                return null;
            return string.Format("{0} {1} {2}", GetTargetStr(sqlStyle), Operater, GetSQLValueExp());
        }

        public string ToSQLSortString(ISqlStyle sqlStyle)
        {
            if (!SortDirection.HasValue)
                return null;
            if(SortDirection == ListSortDirection.Ascending)
            {
                return GetTargetStr(sqlStyle);
            }
            else
            {
                return string.Format("{0} DESC", GetTargetStr(sqlStyle));
            } 
        }

        private string GetSQLValueExp()
        {
            if (Value == null)
                return "NULL";
            if ((Value) is DBNull)
                return "NULL";
            if(Column == null)
                return "NULL";
            if (IsRaw)
                return this.ValueStr;

            if ((Operater.ToUpper() ?? "") == "LIKE")
            {
                if (Column.IsTextType())
                    return string.Format("'%{0}%'", ValueStr);
                else
                    return string.Format("{0}", ValueStr);
            }
            else if ((Operater.ToUpper() ?? "") == "IN")
            {
                string result = "";
                foreach (var v in ValueStr.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(result))
                        result += ",";
                    if (Column.IsTextType())
                        result += string.Format("'{0}'", v);
                    else
                        result += v;
                }
                return string.Format("({0})", result);
            }
            else if (Column.IsTextType())
                return string.Format("'{0}'", ValueStr);
            else
                return Value.ToString(); 
        }


        private string GetTargetStr(ISqlStyle sqlStyle)
        {
            string result = sqlStyle.ModifierName(this.ColumnName);
            if (!string.IsNullOrWhiteSpace(this.TableName))
                result = sqlStyle.ModifierName(this.TableName) + "." + result;
            if (!string.IsNullOrWhiteSpace(this.DBName))
                result = sqlStyle.ModifierName(this.DBName) + "." + result;
            return result;
        }
    }

    public class SqlConditionGroup : ISqlConditionGroup
    {
        private bool _isAnd = false;
        public bool IsAnd
        {
            get
            {
                return _isAnd;
            }
        }

        private List<ISqlCondition> _items = new List<ISqlCondition>();
        public IEnumerable<ISqlCondition> Items
        {
            get
            {
                return _items;
            }
        }

        public SqlConditionGroup(bool isAnd)
        {
            _isAnd = isAnd;
        }

        public void Add(ISqlCondition item)
        {
            if (item == null)
                return;
            _items.Add(item);
        }

        public void Remove(ISqlCondition item)
        {
            _items.Remove(item);
        }

        public void RemoveAll(ISqlCondition item)
        {
            _items.Clear();
        }

        public virtual string ToSQLConditionString(ISqlStyle sqlStyle)
        {
            string result = "";
            foreach (ISqlCondition item in _items)
            {
                string itemWhereStr = item.ToSQLConditionString(sqlStyle);
                if (string.IsNullOrWhiteSpace(itemWhereStr))
                    continue;
                if (!string.IsNullOrWhiteSpace(result))
                    result += IsAnd ? "AND" : "OR";
                result += "(" + itemWhereStr + ")";
            }
            return result;
        }

        public string ToSQLSortString(ISqlStyle style)
        {
            string result = "";
            foreach (ISqlCondition item in _items)
            {
                string itemSortStr = item.ToSQLSortString(style);
                if (string.IsNullOrWhiteSpace(itemSortStr))
                    continue;
                if (!string.IsNullOrWhiteSpace(result))
                    result += ",";
                result += itemSortStr;
            }
            return result;
        }
    }
}
