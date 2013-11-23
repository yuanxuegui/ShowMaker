using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowMaker.Desktop.Models.Sender;

namespace ShowMaker.Desktop.Models.Domain
{
    public interface ICommandContext
    {
        string BuildCommand(Command command);
    }
}
