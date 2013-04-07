using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ShowMaker.Desktop.Domain
{
    public class Parameter
    {
        private ParameterType typeField;

        [XmlAttribute("type")]
        public ParameterType Type
        {
            get { return typeField; }
            set { typeField = value; }
        }

        private string nameField;

        [XmlAttribute("name")]
        public string Name
        {
            get { return nameField; }
            set { nameField = value; }
        }
        private string minValueField;

        [XmlAttribute("minValue")]
        public string MinValue
        {
            get { return minValueField; }
            set { minValueField = value; }
        }

        private string maxValueField;

        [XmlAttribute("maxValue")]
        public string MaxValue
        {
            get { return maxValueField; }
            set { maxValueField = value; }
        }

        public Parameter(string name, ParameterType type)
        {
            this.nameField = name;
            this.typeField = type;
        }

        public Parameter(string name, ParameterType type, string minValue, string maxValue)
        {
            this.nameField = name;
            this.typeField = type;
            this.minValueField = minValue;
            this.maxValueField = maxValue;
        }

        public Parameter() { }

    }
}
