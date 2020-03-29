using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugTools.Secret;

namespace DebugTools.Package
{
    public class PackageExportOption
    {
        public String Path { get; set; }
        public SecretMode SecretMode { get; set; }
        public String KeyPath { get; set; }
        public String KeyName { get; set; } 

    }
}
