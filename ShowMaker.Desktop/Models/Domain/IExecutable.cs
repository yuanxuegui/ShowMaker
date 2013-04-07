using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowMaker.Desktop.Domain
{
    public interface IExecutable
    {
        void Execute(ICommandContext commandContext);
    }
}
