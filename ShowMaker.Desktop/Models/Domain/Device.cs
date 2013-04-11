using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ShowMaker.Desktop.Domain
{
    public class Device : IItemFinder<Operation, string>
    {
        private string idField;

        [XmlAttribute("id")]
        [Category("信息")]
        [DisplayName("ID")]
        [Description("设备的ID")]
        public string Id
        {
            get { return idField; }
            set { idField = value; }
        }
        private DeviceType typeField;

        [XmlAttribute("type")]
        [Category("信息")]
        [DisplayName("类型")]
        [Description("设备的类型")]
        public DeviceType Type
        {
            get { return typeField; }
            set { typeField = value; }
        }
        private ObservableCollection<Operation> operationItemsField = new ObservableCollection<Operation>();

        [XmlElement("operation")]
        [Category("信息")]
        [DisplayName("操作")]
        [Description("设备的操作集合")]
        public ObservableCollection<Operation> OperationItems
        {
            get { return operationItemsField; }
            set { operationItemsField = value; }
        }

        public Operation GetItemByKey(string key)
        {
            foreach (Operation operation in operationItemsField)
            {
                String opName = operation.Name;
                if (opName != null && opName.Equals(key))
                    return operation;
            }
            return null;
        }

        private Area _parent;
        public void SetParent(Area parent)
        {
            _parent = parent;
        }
        public Area GetParent()
        {
            return _parent;
        }
    }
}
