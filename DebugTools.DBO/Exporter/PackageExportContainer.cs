using System.Collections.Generic;
using System.Data;

namespace DebugTools.DBO
{
    public class PackageExportContainer
    {
        private DataSet _data = new DataSet();

        public DataSet GetData()
        {
            return _data;
        }

        public void Add(IEnumerable<DataRow> rows)
        {
            foreach (DataRow row in rows)
                Add(row);
        }

        public void Add(DataRow row)
        {
            DataTable table = null;
            if (_data.Tables.Contains(row.Table.TableName))
                table = _data.Tables[row.Table.TableName];
            else
            {
                table = row.Table.Clone();
                _data.Tables.Add(table);
            }
            table.ImportRow(row);
        }

        public void Remove(IEnumerable<DataRow> rows)
        {
            foreach (DataRow row in rows)
                Remove(row);
        }

        public void Remove(DataRow row)
        {
            DataTable table = null;
            if (_data.Tables.Contains(row.Table.TableName))
                return;
            table = _data.Tables[row.Table.TableName];
            table.Rows.Remove(row);
        }

        public void Clear()
        {
            _data.Clear();
        }
    }
}
