using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ShowMaker.Desktop.Domain;

namespace ShowMaker.Desktop.Parser
{
    public interface IExhibitionParser
    {
        Exhibition Parse(string file);
    }
}
