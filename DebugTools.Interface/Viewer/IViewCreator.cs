﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    public interface IViewCreator
    {
        IView CreateView(IViewContainer container);


    }
}
