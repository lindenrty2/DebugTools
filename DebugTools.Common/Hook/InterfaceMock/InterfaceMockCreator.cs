using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.Common.Hook.InterfaceMock
{
    public class InterfaceMockCreator
    {

        public static T Create<T>()
        {
            return default(T);
        }


    }
}
