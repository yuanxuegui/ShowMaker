using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Caliburn.Micro;
using ShowMaker.Desktop.Models.Util;

namespace ShowMaker.Desktop.Models.Domain
{
    [DisplayName("展区")]
    public class Area : IItemFinder<Device, string> 
    {
        private string name;

        [XmlAttribute("name")]
        [Category("信息")]
        [DisplayName("名称")]
        [Description("展区的名称")]
        public string Name
        {
            get { return name; }
            set { 
                name = value;
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
        }
        private ObservableCollection<Device> deviceItems = new ObservableCollection<Device>();

        [XmlElement("device")]
        [Category("信息")]
        [DisplayName("设备")]
        [Description("展区的设备集合")]
        public ObservableCollection<Device> DeviceItems
        {
            get { return deviceItems; }
            set { 
                deviceItems = value;
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
        }
        private Timeline timeline = new Timeline();

        [XmlElement("timeline")]
        [Category("信息")]
        [DisplayName("时间线")]
        [Description("展区的时间线")]
        public Timeline Timeline
        {
            get { return timeline; }
            set { 
                timeline = value;
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
        }

        public Area() {
            timeline.PropertyItem.Add(new Property(Constants.TIME_UNIT_KEY, Constants.TIME_UNIT_S));
            timeline.PropertyItem.Add(new Property(Constants.TIME_MAX_KEY, "10"));
            IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
        }

        public Area(string name) : this()
        {
            Name = name;
        }

        public Device GetItemByKey(string key)
        {
            foreach (Device device in deviceItems)
            {
                if (device.Id.Equals(key))
                    return device;
            }
            return null;
        }

        private Exhibition _parent;
        public void SetParent(Exhibition parent)
        {
            _parent = parent;
        }
        public Exhibition GetParent()
        {
            return _parent;
        }
    }
}
