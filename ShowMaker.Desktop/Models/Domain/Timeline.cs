using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ShowMaker.Desktop.Models.Domain;
using Caliburn.Micro;
using ShowMaker.Desktop.Util;

namespace ShowMaker.Desktop.Domain
{
    public class Timeline : IItemFinder<TimePoint,int>, IPropertyFinder, IHandle<TimelineMaxChangedMessage>
    {
        public Timeline()
        {
            IoC.Get<IEventAggregator>().Subscribe(this);
        }
        
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


        public void Handle(TimelineMaxChangedMessage message)
        {
            if (message.TimelineTarget == this)
            {
                foreach (Property prop in PropertyItem)
                {
                    if (prop.Name.Equals(Constants.TIME_MAX_KEY))
                        prop.Value = "" + message.Max;
                }
            }
        }
    }
}
