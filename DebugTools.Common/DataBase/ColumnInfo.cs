using System;
using System.Data;
using System.Xml;
using DebugTools.Helper;
using DebugTools.Secret;

namespace DebugTools.DataBase
{
    public class ColumnInfo : IColumnInfo
    {
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
        }

        private ITableInfo _tableInfo;
        public ITableInfo TableInfo
        {
            get
            {
                return _tableInfo;
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        private string _typeName;
        public string TypeName
        {
            get
            {
                return _typeName;
            }
        }

        public string DisplayTypeName
        {
            get
            {
                if (Precision > 0 || Scale > 0)
                    return string.Format("{0}({2},{3})", TypeName, MaxLength, Precision, Scale);
                else if (MaxLength > (long)0)
                    return string.Format("{0}({1})", TypeName, MaxLength);
                else
                    return _typeName;
            }
        }

        private Type _dataType;
        public Type DataType
        {
            get
            {
                return _dataType;
            }
        }

        private long _maxLength;
        public long MaxLength
        {
            get
            {
                return _maxLength;
            }
        }

        private int _precision;
        public int Precision
        {
            get
            {
                return _precision;
            }
        }

        private int _scale;
        public int Scale
        {
            get
            {
                return _scale;
            }
        }

        private int _pkno;
        public int PKNo
        {
            get
            {
                return _pkno;
            }
        }

        public bool IsPK
        {
            get
            {
                return _pkno > 0;
            }
        }

        private string _comment;
        public string Comment
        {
            get
            {
                return _comment;
            }
        }

        private string _memo;
        public string Memo
        {
            get
            {
                return _memo;
            }
            set
            {
                _memo = value;
            }
        }

        private SecretType _secretType = SecretType.None;
        public SecretType SecretType
        {
            get
            {
                return _secretType;
            }
            set
            {
                _secretType = value;
            }
        }

        public string DisplayName
        {
            get
            {
                return string.Format("{0}({1})", Name, Comment);
            }
        }

        public ColumnInfo(ITableInfo tableInfo, DataRow row)
        {
            _tableInfo = tableInfo;
            _id = System.Convert.ToInt32(row["COLUMN_ID"]);
            _name = row["NAME"].ToString();
            _comment = row["COMMENT"].ToString();
            _typeName = row["TYPENAME"].ToString();
            _dataType = GetSqlType(_typeName);
            _maxLength = row.IsNull("MAX_LENGTH") ? 0 : System.Convert.ToInt64(row["MAX_LENGTH"]);
            _precision = row.IsNull("PRECISION") ? 0 : System.Convert.ToInt32(row["PRECISION"]);
            _scale = row.IsNull("SCALE") ? 0 : System.Convert.ToInt32(row["SCALE"]);
            if ((row["KEYNO"]) is DBNull)
                _pkno = 0;
            else
                _pkno = System.Convert.ToInt32(row["KEYNO"]);
        }

        public ColumnInfo(ITableInfo tableInfo, DataRow row, XmlNode node) : this(tableInfo, row)
        {
            this.SetCustomInfo(node);
        }

        public ColumnInfo(ITableInfo tableInfo, XmlNode node)
        {
            _tableInfo = tableInfo;
            _id = System.Convert.ToInt32(XmlHelper.GetAttributeValue(node, "Id"));
            _name = XmlHelper.GetAttributeValue(node, "Name");
            _comment = XmlHelper.GetAttributeValue(node, "Comment");
            _typeName = XmlHelper.GetAttributeValue(node, "TypeName");
            _dataType = GetSqlType(_typeName);
            _maxLength = System.Convert.ToInt32(XmlHelper.GetAttributeValue(node, "MaxLength"));
            _precision = System.Convert.ToInt32(XmlHelper.GetAttributeValue(node, "Precision"));
            _scale = System.Convert.ToInt32(XmlHelper.GetAttributeValue(node, "Scale"));
            _pkno = System.Convert.ToInt32(XmlHelper.GetAttributeValue(node, "PKNo"));
            SetCustomInfo(node);
        }

        public void SetCustomInfo(XmlNode node)
        {
            if (node == null)
                return;
            _memo = node.InnerText;
            _secretType = (SecretType)System.Convert.ToInt16(XmlHelper.GetAttributeValue(node, "SecretType"));
        }

        public bool Save(XmlNode columnNode)
        {
            columnNode.Attributes.Append(XmlHelper.CreateAttribute(columnNode.OwnerDocument, "Id", _id));
            columnNode.Attributes.Append(XmlHelper.CreateAttribute(columnNode.OwnerDocument, "Name", _name));
            columnNode.Attributes.Append(XmlHelper.CreateAttribute(columnNode.OwnerDocument, "Comment", _comment));
            columnNode.Attributes.Append(XmlHelper.CreateAttribute(columnNode.OwnerDocument, "TypeName", _typeName));
            columnNode.Attributes.Append(XmlHelper.CreateAttribute(columnNode.OwnerDocument, "MaxLength", _maxLength));
            columnNode.Attributes.Append(XmlHelper.CreateAttribute(columnNode.OwnerDocument, "Precision", _precision));
            columnNode.Attributes.Append(XmlHelper.CreateAttribute(columnNode.OwnerDocument, "PKNo", _pkno));
            columnNode.Attributes.Append(XmlHelper.CreateAttribute(columnNode.OwnerDocument, "Scale", _scale));
            columnNode.Attributes.Append(XmlHelper.CreateAttribute(columnNode.OwnerDocument, "SecretType", System.Convert.ToInt32(_secretType)));
            if (!string.IsNullOrWhiteSpace(this.Memo))
                columnNode.InnerText = Memo;
            return true;
        }

        public static Type GetSqlType(string typeName)
        {
            if ((typeName ?? "") == "image")
                return typeof(System.Data.SqlTypes.SqlBinary);
            else if ((typeName ?? "") == "text")
                return typeof(System.Data.SqlTypes.SqlString);
            else if ((typeName ?? "") == "uniqueidentifier")
                return typeof(System.Data.SqlTypes.SqlString);
            else if ((typeName ?? "") == "date")
                return typeof(System.Data.SqlTypes.SqlDateTime);
            else if ((typeName ?? "") == "time")
                return typeof(System.Data.SqlTypes.SqlDateTime);
            else if ((typeName ?? "") == "smalldatetime")
                return typeof(System.Data.SqlTypes.SqlDateTime);
            else if ((typeName ?? "") == "datetime")
                return typeof(System.Data.SqlTypes.SqlDateTime);
            else if ((typeName ?? "") == "datetime2")
                return typeof(System.Data.SqlTypes.SqlDateTime);
            else if ((typeName ?? "") == "datetimeoffset")
                return typeof(System.Data.SqlTypes.SqlInt64);
            else if ((typeName ?? "") == "tinyint")
                return typeof(System.Data.SqlTypes.SqlInt16);
            else if ((typeName ?? "") == "smallint")
                return typeof(System.Data.SqlTypes.SqlInt16);
            else if ((typeName ?? "") == "int")
                return typeof(System.Data.SqlTypes.SqlInt32);
            else if ((typeName ?? "") == "real")
                return typeof(object);
            else if ((typeName ?? "") == "money")
                return typeof(System.Data.SqlTypes.SqlMoney);
            else if ((typeName ?? "") == "float")
                return typeof(System.Data.SqlTypes.SqlSingle);
            else if ((typeName ?? "") == "sql_variant")
                return typeof(object);
            else if ((typeName ?? "") == "ntext")
                return typeof(System.Data.SqlTypes.SqlString);
            else if ((typeName ?? "") == "bit")
                return typeof(System.Data.SqlTypes.SqlByte);
            else if ((typeName ?? "") == "decimal")
                return typeof(System.Data.SqlTypes.SqlDecimal);
            else if ((typeName ?? "") == "numeric")
                return typeof(System.Data.SqlTypes.SqlDecimal);
            else if ((typeName ?? "") == "smallmoney")
                return typeof(System.Data.SqlTypes.SqlMoney);
            else if ((typeName ?? "") == "bigint")
                return typeof(System.Data.SqlTypes.SqlInt64);
            else if ((typeName ?? "") == "hierarchyid")
                return typeof(System.Data.SqlTypes.SqlInt32);
            else if ((typeName ?? "") == "geometry")
                return typeof(object);
            else if ((typeName ?? "") == "geography")
                return typeof(object);
            else if ((typeName ?? "") == "varbinary")
                return typeof(System.Data.SqlTypes.SqlBinary);
            else if ((typeName ?? "") == "binary")
                return typeof(System.Data.SqlTypes.SqlBinary);
            else if ((typeName ?? "") == "timestamp")
                return typeof(System.Data.SqlTypes.SqlInt64);
            else if ((typeName ?? "") == "char")
                return typeof(System.Data.SqlTypes.SqlChars);
            else if ((typeName ?? "") == "varchar")
                return typeof(System.Data.SqlTypes.SqlChars);
            else if ((typeName ?? "") == "nvarchar")
                return typeof(System.Data.SqlTypes.SqlChars);
            else if ((typeName ?? "") == "nchar")
                return typeof(System.Data.SqlTypes.SqlChars);
            else if ((typeName ?? "") == "xml")
                return typeof(System.Data.SqlTypes.SqlXml);
            else if ((typeName ?? "") == "sysname")
                return typeof(object);
            else
                return typeof(object);
        }

        public bool IsTextType()
        {
            return _dataType == typeof(System.Data.SqlTypes.SqlString) || _dataType == typeof(System.Data.SqlTypes.SqlChars) || _dataType == typeof(System.Data.SqlTypes.SqlDateTime) || _dataType == typeof(System.Data.SqlTypes.SqlXml) || _dataType == typeof(string) || _dataType == typeof(char) || _dataType == typeof(char[]) || _dataType == typeof(DateTime) || _dataType == typeof(DateTime) || _dataType == typeof(System.Data.SqlTypes.SqlBinary);
        }

        public bool IsNumberType()
        {
            return _dataType == typeof(System.Data.SqlTypes.SqlInt64) || _dataType == typeof(System.Data.SqlTypes.SqlInt32) || _dataType == typeof(System.Data.SqlTypes.SqlInt16) || _dataType == typeof(System.Data.SqlTypes.SqlMoney) || _dataType == typeof(System.Data.SqlTypes.SqlByte) || _dataType == typeof(System.Data.SqlTypes.SqlDecimal) || _dataType == typeof(System.Data.SqlTypes.SqlSingle);
        }
    }
}
