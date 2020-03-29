using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    [Serializable]
    public class LogBlock
    {
        private List<DMessage> _errorMessages = null;
        public IEnumerable<DMessage> ErrorMessages
        {
            get { return _errorMessages; }
        }

        private List<DMessage> _warningMessages = null;
        public IEnumerable<DMessage> WarningMessages
        {
            get { return _warningMessages; }
        }

        private List<DMessage> _infoMessages = null;
        public IEnumerable<DMessage> InfoMessages
        {
            get { return _infoMessages; }
        }

        private string _category = string.Empty;
        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        public LogBlock(string category)
        {
            _errorMessages = new List<DMessage>();
            _warningMessages = new List<DMessage>();
            _infoMessages = new List<DMessage>();
        }

        public void AddErrorMessage(DMessage error)
        {
            _errorMessages.Add(error);
        }

        public void AddWarningMessage(DMessage warning)
        {
            _warningMessages.Add(warning);
        }

        public void AddInfoMessage(DMessage info)
        {
            _infoMessages.Add(info);
        }

        public void Clear()
        {
            _errorMessages.Clear();
            _warningMessages.Clear();
            _infoMessages.Clear();
        }

    }
}
