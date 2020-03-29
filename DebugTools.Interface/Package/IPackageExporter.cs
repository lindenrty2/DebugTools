using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools.DataBase;
using System.Data;

namespace DebugTools.Package
{
    public interface IPackageExporter
    {

        IDataAccessor DataAccessor { get; }

        void AddTargets(IEnumerable<DataRow> datarow);

        void RemoveTargets(IEnumerable<DataRow> datarow);

        void ClearTargets();

        DataSet GetTargetData();

        bool Export(PackageExportOption packageOption);

    }
}
