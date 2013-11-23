using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Caliburn.Micro;

namespace ShowMaker.Desktop.Models.Domain
{
    [XmlRoot(ElementName = "exhibition", Namespace = "http://www.sec.ac.cn/exhibition-1.2", IsNullable = false)]
    [DisplayName("展会")]
    public class Exhibition : IItemFinder<Area, string>, IPropertyFinder
    {
        private string descriptionField;

        [XmlElement("description")]
        [Category("信息")]
        [DisplayName("描述")]
        [Description("展会的描述")]
        public string Description
        {
            get { return descriptionField; }
            set { 
                descriptionField = value;
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
        }
        private ObservableCollection<Property> propertyItemsField;

        [XmlElement("property")]
        [Category("信息")]
        [DisplayName("属性")]
        [Description("展会的属性")]
        public ObservableCollection<Property> PropertyItems
        {
            get { return propertyItemsField; }
            set { 
                propertyItemsField = value;
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
        }
        private ObservableCollection<Area> areaItemsField = new ObservableCollection<Area>();

        [XmlElement("area")]
        [Category("信息")]
        [DisplayName("展区")]
        [Description("展会的展区集合")]
        public ObservableCollection<Area> AreaItems
        {
            get { return areaItemsField; }
            set { 
                areaItemsField = value;
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
        }

        public Area GetItemByKey(string key)
        {
            foreach (Area area in areaItemsField)
            {
                if (area.Name.Equals(key))
                    return area;
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

        public void SetPropertyValue(string name, string value)
        {
            foreach (Property property in propertyItemsField)
            {
                if (property.Name.Equals(name))
                {
                    property.Value = value;
                    return;
                }
            }
            // 不存在指定name的Property
            Property prop = new Property();
            prop.Name = name;
            prop.Value = value;
            propertyItemsField.Add(prop);
        }
    }
}
