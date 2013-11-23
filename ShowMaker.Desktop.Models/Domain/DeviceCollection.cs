using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ShowMaker.Desktop.Models.Domain
{
    [XmlRoot(ElementName = "devices", Namespace = "http://www.sec.ac.cn/devices-1.2", IsNullable = false)]
    public class DeviceCollection
    {
        private ObservableCollection<Device> deviceItems;
        
        [XmlElement("device")]
        [Category("信息")]
        [DisplayName("设备")]
        [Description("展区的设备集合")]
        public ObservableCollection<Device> DeviceItems
        {
            get { return deviceItems; }
            set { deviceItems = value; }
        }
    }
}
