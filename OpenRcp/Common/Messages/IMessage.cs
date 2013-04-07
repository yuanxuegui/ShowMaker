using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRcp
{
    public interface IMessage
    {
        string Content { get; set; }
    }
}
