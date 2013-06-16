using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowMaker.Desktop.Domain;

namespace ShowMaker.Desktop.Models.Domain
{
    public class TimelineMaxChangedMessage
    {
        public int Max { get; set; }
        public Timeline TimelineTarget { get; set; }
    }
}
