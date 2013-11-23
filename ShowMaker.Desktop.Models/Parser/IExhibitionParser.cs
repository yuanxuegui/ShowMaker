using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ShowMaker.Desktop.Models.Domain;

namespace ShowMaker.Desktop.Models.Parser
{
    public interface IExhibitionParser
    {
        Exhibition Parse(string file);
    }
}
