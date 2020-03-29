using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DebugTools.Common.Warpper.API
{
    [StructLayout(LayoutKind.Sequential)]//定义与API相兼容结构体，实际上是一种内存转换 
    public struct APIPointer
    {
        public int X;
        public int Y;
    }
}
