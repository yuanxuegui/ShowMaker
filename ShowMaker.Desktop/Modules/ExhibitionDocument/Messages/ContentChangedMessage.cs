using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowMaker.Desktop.Domain;

namespace ShowMaker.Desktop.Modules.ExhibitionDocument.Messages
{
    public class ContentChangedMessage
    {
        public Exhibition Content { get; set; }
    }
}
