using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools
{
    public interface IExecuteResult
    {
        bool IsSuccessfully { get; }

        bool HasError { get; }

        object Result { get; }

        IMessage[] Errors { get; }

        IMessage[] Warnings { get; }

        IMessage[] Informations { get; }
        
    }
}
