using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using Caliburn.Micro;

namespace ShowMaker.Desktop.Models.Domain
{
    [DisplayName("参数")]
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
            set { 
                typeField = value;
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());                
            }
        }

        private string nameField;

        [XmlAttribute("name")]
        [Category("信息")]
        [DisplayName("名称")]
        [Description("参数的名称")]
        public string Name
        {
            get { return nameField; }
            set { 
                nameField = value;
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());            
            }
        }
        private string minValueField;

        [XmlAttribute("minValue")]
        [Category("信息")]
        [DisplayName("最小值")]
        [Description("参数的最小值")]
        public string MinValue
        {
            get { return minValueField; }
            set { 
                minValueField = value;
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage()); 
            }
        }

        private string maxValueField;

        [XmlAttribute("maxValue")]
        [Category("信息")]
        [DisplayName("最大值")]
        [Description("参数的最大值")]
        public string MaxValue
        {
            get { return maxValueField; }
            set { 
                maxValueField = value; 
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
        }

        public Parameter(string name, ParameterType type)
        {
            this.nameField = name;
            this.typeField = type;
            IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
        }

        public Parameter(string name, ParameterType type, string minValue, string maxValue)
        {
            this.nameField = name;
            this.typeField = type;
            this.minValueField = minValue;
            this.maxValueField = maxValue;
            IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
        }

        public Parameter() {
            IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
        }

    }
}
