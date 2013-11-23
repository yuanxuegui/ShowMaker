using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Caliburn.Micro;
using ShowMaker.Desktop.Models.Sender;

namespace ShowMaker.Desktop.Models.Domain
{
    [DisplayName("命令")]
    public class Command : IExecutable, IPropertyFinder
    {
        private string deviceIdField;

        [XmlAttribute("deviceId")]
        [Category("信息")]
        [DisplayName("设备标识")]
        [Description("命令所属的设备标识")]
        public string DeviceId
        {
            get { return deviceIdField; }
            set { 
                deviceIdField = value;
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
        }
        private string operationNameField;

        [XmlAttribute("operationName")]
        [Category("信息")]
        [DisplayName("操作名称")]
        [Description("命令绑定的设备操作")]
        public string OperationName
        {
            get { return operationNameField; }
            set { 
                operationNameField = value;
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
        }

        private ObservableCollection<Property> propertyItemsField = new ObservableCollection<Property>();

        [XmlElement("property")]
        [Category("信息")]
        [DisplayName("属性")]
        [Description("命令的属性集合")]
        public ObservableCollection<Property> PropertyItems
        {
            get { return propertyItemsField; }
            set { 
                propertyItemsField = value;
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
        }


        public void Execute(ICommandContext commandContext)
        {
            SenderLocator.Lookup().Send(commandContext.BuildCommand(this));
        }

        public string GetPropertyValue(string name)
        {
            foreach (Property property in propertyItemsField)
            {
                if (property.Name.Equals(name))
                {
                    return property.Value;
                }
            }
            return null;
        }
    }
}
