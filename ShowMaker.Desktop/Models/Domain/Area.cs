using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ShowMaker.Desktop.Domain
{
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
            set { name = value; }
        }
        private ObservableCollection<Device> deviceItems = new ObservableCollection<Device>();

        [XmlElement("device")]
        [Category("信息")]
        [DisplayName("设备")]
        [Description("展区的设备集合")]
        public ObservableCollection<Device> DeviceItems
        {
            get { return deviceItems; }
            set { deviceItems = value; }
        }
        private Timeline timeline;

        [XmlElement("timeline")]
        [Category("信息")]
        [DisplayName("时间线")]
        [Description("展区的时间线")]
        public Timeline Timeline
        {
            get { return timeline; }
            set { timeline = value; }
        }

        public Area() { }

        public Area(string name)
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
    }
}
