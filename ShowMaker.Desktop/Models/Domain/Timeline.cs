using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ShowMaker.Desktop.Domain
{
    public class Timeline : IItemFinder<TimePoint,int>, IPropertyFinder
    {
        private ObservableCollection<Property> propertyItemsField = new ObservableCollection<Property>();

        [XmlElement("property")]
        public ObservableCollection<Property> PropertyItem
        {
            get { return propertyItemsField; }
            set { propertyItemsField = value; }
        }
        private ObservableCollection<TimePoint> timePointItemsField = new ObservableCollection<TimePoint>();

        [XmlElement("timePoint")]
        public ObservableCollection<TimePoint> TimePointItems
        {
            get { return timePointItemsField; }
            set { timePointItemsField = value; }
        }

        public TimePoint GetItemByKey(int key)
        {
            foreach (TimePoint timePoint in timePointItemsField)
            {
                if (timePoint.Tick.Equals(key))
                {
                    return timePoint;
                }
            }
            return null;
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
