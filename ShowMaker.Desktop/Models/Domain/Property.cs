using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ShowMaker.Desktop.Domain
{
    public class Property
    {
        private string name;

        [XmlAttribute("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string value;

        [XmlAttribute("value")]
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public Property() { }

        public Property(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

    }
}
