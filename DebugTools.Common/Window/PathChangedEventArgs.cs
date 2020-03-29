using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.Common.Window
{
    public class PathChangedEventArgs
    {
        private string _beforePath;
        public string BeforePath
        {
            get { return _beforePath; }
            set { _beforePath = value; }
        }

        private string _afterPath;
        public string AfterPath
        {
            get { return _afterPath; }
            set { _afterPath = value; }
        }

        private SelectPathType _selectPathType;
        public SelectPathType SelectPathType
        {
            get { return _selectPathType; }
            set { _selectPathType = value; }
        }

        public PathChangedEventArgs(string beforePath, string afterPath, SelectPathType selectPathType)
        {
            _beforePath = beforePath;
            _afterPath = afterPath;
            _selectPathType = selectPathType;
        }

    }
}
