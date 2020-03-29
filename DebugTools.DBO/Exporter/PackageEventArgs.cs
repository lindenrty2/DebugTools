using System.Collections.Generic;
using System;
using System.Data;

namespace DebugTools.DBO
{
    public class PackageEventArgs : EventArgs
    {
        private IEnumerable<DataRow> _targetRow;
        public IEnumerable<DataRow> TargetRows
        {
            get
            {
                return _targetRow;
            }
        }

        private PackageEventType _operaterType;
        public PackageEventType OperaterType
        {
            get
            {
                return _operaterType;
            }
        }

        public PackageEventArgs(PackageEventType operaterType, IEnumerable<DataRow> rows)
        {
            _operaterType = operaterType;
            _targetRow = rows;
        }
    }

    public enum PackageEventType
    {
        Add,
        Modify,
        Remove,
        Clear
    }
}
