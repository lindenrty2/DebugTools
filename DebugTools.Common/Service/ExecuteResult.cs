using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools;

namespace DebugTools.Common.Service
{
    public class ExecuteResult
        : IExecuteResult
    {
        private bool _isSuccessfully = false;
        public bool IsSuccessfully
        {
            get { return _isSuccessfully; }
        }

        public bool HasError
        {
            get { return _errors !=null && _errors.Length != 0; }
        }

        private object _result = null;
        public object Result
        {
            get { return _result; }
        }

        private IMessage[] _errors = null;
        public IMessage[] Errors
        {
            get { return _errors; }
        }

        private IMessage[] _warnings = null;
        public IMessage[] Warnings
        {
            get { return _warnings; }
        }

        private IMessage[] _informations = null;
        public IMessage[] Informations
        {
            get { return _informations; }
        }

        public ExecuteResult(bool isSuccessfully)
            : this(isSuccessfully, null, new IMessage[0], new IMessage[0], new IMessage[0])
        {
        }

        public ExecuteResult(bool isSuccessfully, object result)
            : this(isSuccessfully,result,new IMessage[0],new IMessage[0],new IMessage[0])
        { 
        }

        public ExecuteResult(bool isSuccessfully, object result, IMessage[] errors, IMessage[] warnings, IMessage[] informations)
        {
            _isSuccessfully = isSuccessfully;
            _result = result;
            _errors = errors;
            _warnings = warnings;
            _informations = informations;
        }
    }
}
