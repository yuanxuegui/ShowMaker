using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ShowMaker.Desktop.Sender;

namespace ShowMaker.Desktop.Domain
{
    public class Command : IExecutable, IPropertyFinder
    {
        private string deviceIdField;

        [XmlAttribute("deviceId")]
        public string DeviceId
        {
            get { return deviceIdField; }
            set { deviceIdField = value; }
        }
        private string operationNameField;

        [XmlAttribute("operationName")]
        public string OperationName
        {
            get { return operationNameField; }
            set { operationNameField = value; }
        }

        private ObservableCollection<Property> propertyItemsField = new ObservableCollection<Property>();

        [XmlElement("property")]
        public ObservableCollection<Property> PropertyItems
        {
            get { return propertyItemsField; }
            set { propertyItemsField = value; }
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
