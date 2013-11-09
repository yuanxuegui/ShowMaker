using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Caliburn.Micro;

namespace ShowMaker.Desktop.Domain
{
    [DisplayName("属性")]
    public class Property
    {
        private string name;

        [XmlAttribute("name")]
        [Category("信息")]
        [DisplayName("名称")]
        [Description("属性的名称")]
        public string Name
        {
            get { return name; }
            set { 
                name = value;
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
        }
        private string value;

        [XmlAttribute("value")]
        [Category("信息")]
        [DisplayName("值")]
        [Description("属性的值")]
        public string Value
        {
            get { return this.value; }
            set { 
                this.value = value;
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
        }

        public Property() {
            IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
        }

        public Property(string name, string value)
        {
            this.name = name;
            this.value = value;
            IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
        }

    }
}
