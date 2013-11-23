using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ShowMaker.Desktop.Models.Domain;
using ShowMaker.Desktop.Models.Util;

namespace ShowMaker.Desktop.Models.Parser
{
    public class XmlSerializerExhibitionParser : IExhibitionParser
    {
        public Exhibition Parse(string file)
        {
            return (Exhibition)XmlSerializerUtil.LoadXml(typeof(Exhibition), file);
        }
    }
}
