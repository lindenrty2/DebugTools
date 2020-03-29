using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.Information
{
    [Serializable]
    public class ProgressInfo
    {
        private double _total = 0.0d;
        public double Total
        {
            get
            {
                return _total;
            }
            set
            {
                _total = value;
            }
        }

        private double _current = 0.0d;
        public double Current
        {
            get
            {
                return _current;
            }
            set
            {
                _current = value;
            }
        }
    }
}
