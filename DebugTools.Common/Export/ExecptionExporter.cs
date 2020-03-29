using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools;
using DebugTools.Common.Service;

namespace DebugTools.Common.Export
{
    public class ExecptionExporter
        : IExporter
    {
        public string Category
        {
            get
            {
                return "ExecptionExporter";
            }
        }

        public IExecuteResult Export()
        {

            return new ExecuteResult(true);
        }
         
    }
}
