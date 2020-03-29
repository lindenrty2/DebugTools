using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DebugTools.Common.Warpper.API
{
    public class ComRect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
         
        public ComRect(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public ComRect(Rectangle rect)
        {
            this.left = rect.Left;
            this.top = rect.Top;
            this.right = rect.Right;
            this.bottom = rect.Bottom;
        }
    }
}
