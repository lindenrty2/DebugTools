using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools;
using DebugTools.Common.Service;

namespace DebugTools.Common.Manager
{
    public class ImportServiceManager : ServiceManagerBase
    {
        public override string Name
        {
            get { return CommonConst.ImportService; }
        }

        public ImportServiceManager()
            : base()
        {

        }


        protected override string GetCategory(string function)
        {
            return string.Format("Import_{0}", function);
        }


        public override IExecuteResult Execute(string function, string name, params object[] parameter)
        {
            IExporter exporter = GetInstance<IExporter>(function, name, parameter);
            return exporter.Export();
        }

        public override IExecuteResult Execute(string function, params object[] parameter)
        {
            throw new NotImplementedException();
        }
    }
}
