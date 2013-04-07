using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowMaker.Desktop.Sender;

namespace ShowMaker.Desktop.Domain
{
    public interface ICommandContext
    {
        string BuildCommand(Command command);
    }
}
