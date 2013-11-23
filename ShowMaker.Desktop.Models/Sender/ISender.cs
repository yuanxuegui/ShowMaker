using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowMaker.Desktop.Models.Sender
{
    public interface ISender
    {
        bool Send(string message);
    }
}
