using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace ShowMaker.Desktop.Domain
{
    public class Parameter
    {
        private ParameterType typeField;

        [XmlAttribute("type")]
        [Category("信息")]
        [DisplayName("类型")]
        [Description("参数的类型")]
        public ParameterType Type
        {
            get { return typeField; }
            set { typeField = value; }
        }

        private string nameField;

        [XmlAttribute("name")]
        [Category("信息")]
        [DisplayName("名称")]
        [Description("参数的名称")]
        public string Name
        {
            get { return nameField; }
            set { nameField = value; }
        }
        private string minValueField;

        [XmlAttribute("minValue")]
        [Category("信息")]
        [DisplayName("最小值")]
        [Description("参数的最小值")]
        public string MinValue
        {
            get { return minValueField; }
            set { minValueField = value; }
        }

        private string maxValueField;

        [XmlAttribute("maxValue")]
        [Category("信息")]
        [DisplayName("最大值")]
        [Description("参数的最大值")]
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
