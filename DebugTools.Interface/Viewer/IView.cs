using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DebugTools
{
    public interface IView
    {
        string Key { get; }
        string Title { get; }
        string Description { get; }


        Control GetTopElement();



    }
}
