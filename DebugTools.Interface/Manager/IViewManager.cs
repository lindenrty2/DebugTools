using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    public interface IViewManager
    {
        void Regist(string category, string name, IViewCreator creator);

        void UnRegist(string category, string name);

        IViewCreator[] GetViewCreators();

        IViewCreator[] GetViewCreators(string category);

        string[] GetCategories();

    }
}
