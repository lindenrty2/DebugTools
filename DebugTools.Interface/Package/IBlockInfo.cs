using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.Package
{
    public interface IBlockInfo
    {
        int Index { get; set; }
        long StartPosition { get; set; }
        int Size { get; set; }
        short Type { get; set; }

    }
}
