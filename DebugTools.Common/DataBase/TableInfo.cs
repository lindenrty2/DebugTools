using DebugTools.Common;
using DebugTools.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;

namespace DebugTools.DataBase
{
    public class TableInfo : ITableInfo, IXmlSerializable
    {
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
        }

        private string _dbName;
        public string DBName
        {
            get
            {
                return _dbName;
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

        private string _comment;
        public string Comment
        {
            get
            {
                return _comment;
            }
        }

        private IEnumerable<IColumnInfo> _columns;
        public IEnumerable<IColumnInfo> Columns
        {
            get
            {
                if (_columns == null)
                    _columns = _dataAccessor.GetColumns(this);
                return _columns;
            }
        }

        private List<ITableRelationInfo> _relations;
        public IEnumerable<ITableRelationInfo> Relations
        {
            get
            {
                if (_relations == null)
                    _relations = _dataAccessor.GetTableRelation(this).ToList();
                return _relations;
            }
        }

        public string DisplayName
        {
            get
            {
                return string.Format("{0}({1})", Name, Comment);
            }
        }

        private bool _isAutoRelationOutput = true;
        public bool IsAutoRelationOutput
        {
            get
            {
                return _isAutoRelationOutput;
            }
            set
            {
                _isAutoRelationOutput = value;
            }
        }

        private IDataAccessor _dataAccessor = null;
        public IDataAccessor DataAccessor
        {
            get
            {
                return _dataAccessor;
            }
        }

        public TableInfo(IDataAccessor dataAccessor, DataRow row)
        {
            _dataAccessor = dataAccessor;
            _id = System.Convert.ToInt32(row["ID"]);
            _name = GetTableName(row);
            _dbName = row["DBNAME"].ToString();
            _comment = row["COMMENT"].ToString();
        }

        public TableInfo(IDataAccessor dataAccessor, DataRow row, XmlNode node) : this(dataAccessor, row)
        {
            this.Load(node);
        }

        public TableInfo(IDataAccessor dataAccessor, XmlNode node)
        {
            _dataAccessor = dataAccessor;
            var infoNode = node.SelectSingleNode("Info");
            _id = System.Convert.ToInt32(infoNode.Attributes["Id"].Value);
            _dbName = infoNode.Attributes["DBName"].Value;
            _name = infoNode.Attributes["Name"].Value;
            _comment = infoNode.Attributes["Comment"].Value;

            var columnTopNode = node.SelectSingleNode("Columns");
            List<IColumnInfo> columns = new List<IColumnInfo>();
            foreach (XmlNode columnNode in columnTopNode.ChildNodes)
                columns.Add(new ColumnInfo(this, columnNode));
            _columns = columns;

            var relationTopNode = node.SelectSingleNode("Relations");
            List<ITableRelationInfo> relations = new List<ITableRelationInfo>();
            foreach (XmlNode relation in relationTopNode.ChildNodes)
                relations.Add(new TableRelationInfo(this.DataAccessor, relation));
            _relations = relations;
            _isAutoRelationOutput = true;
            Load(node);
        }

        public static string GetTableName(DataRow row)
        {
            return row["NAME"].ToString();
        }

        public static string GetTableDBName(DataRow row)
        {
            return row["DBNAME"].ToString();
        }

        public bool Load(XmlNode node)
        {
            if (node == null)
                return false;

            _isAutoRelationOutput = XmlHelper.GetBooleanAttributeValue(node, "IsAutoRelationOutput", true);
            return true;
        }

        public IColumnInfo GetColumn(string columnName)
        {
            return Columns.FirstOrDefault(x => (x.Name ?? "") == (columnName ?? ""));
        }

        public void AddRelation(ITableRelationInfo tableRelation)
        {
            var a = this.Relations;
            this._relations.Add(tableRelation);
        }

        public bool ContainsRelation(string relationName)
        {
            return this.Relations.Any(x => (x.FKName ?? "") == (relationName ?? ""));
        }

        public ITableRelationInfo GetRelation(string relationName)
        {
            return this.Relations.FirstOrDefault(x => (x.FKName ?? "") == (relationName ?? ""));
        }

        public void ChangeRelation(string oldName, ITableRelationInfo relation)
        {
            var a = this.Relations;
            var loopTo = this._relations.Count - 1;
            for (int index = 0; index <= loopTo; index++)
            {
                if ((this._relations[index].FKName ?? "") == (oldName ?? ""))
                {
                    this._relations[index] = relation;
                    return;
                }
            }
        }

        public bool Save()
        {
            return Save(SystemCenter.CurrentApplication.PathManager.GetSettingPath("Table", this.DBName + "." + this.Name));
        }

        public bool Save(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
                XmlDocument xd = new XmlDocument();
                XmlNode node = xd.AppendChild(xd.CreateNode(XmlNodeType.Element, "xml", ""));
                Write(node);
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                xd.Save(path);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool Write(XmlNode node)
        {
            try
            {
                XmlDocument xd = node.OwnerDocument;
                var infoNode = node.AppendChild(xd.CreateNode(XmlNodeType.Element, "Info", ""));
                infoNode.Attributes.Append(XmlHelper.CreateAttribute(xd, "Id", _id));
                infoNode.Attributes.Append(XmlHelper.CreateAttribute(xd, "DBName", _dbName));
                infoNode.Attributes.Append(XmlHelper.CreateAttribute(xd, "Name", _name));
                infoNode.Attributes.Append(XmlHelper.CreateAttribute(xd, "Comment", _comment));
                infoNode.Attributes.Append(XmlHelper.CreateAttribute(xd, "IsAutoRelationOutput", _isAutoRelationOutput));

                var columnTopNode = node.AppendChild(xd.CreateNode(XmlNodeType.Element, "Columns", ""));
                foreach (ColumnInfo columnNode in this.Columns)
                {
                    XmlNode xmlNode = xd.CreateNode(XmlNodeType.Element, "Column", "");
                    columnNode.Save(xmlNode);
                    columnTopNode.AppendChild(xmlNode);
                }

                var relationTopNode = node.AppendChild(xd.CreateNode(XmlNodeType.Element, "Relations", ""));
                foreach (TableRelationInfo relation in this.Relations)
                {
                    XmlNode xmlNode = xd.CreateNode(XmlNodeType.Element, "Relation", "");
                    relation.Save(xmlNode);
                    relationTopNode.AppendChild(xmlNode);
                }
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
