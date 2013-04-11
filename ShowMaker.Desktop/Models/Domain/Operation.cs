using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ShowMaker.Desktop.Domain
{
    public class Operation : IItemFinder<Parameter, string>
    {
        private string nameField;

        [XmlAttribute("name")]
        [Category("信息")]
        [DisplayName("名称")]
        [Description("操作的名称")]
        public string Name
        {
            get { return nameField; }
            set { nameField = value; }
        }
        private string commandField;

        [XmlAttribute("command")]
        [Category("信息")]
        [DisplayName("命令")]
        [Description("操作的命令字符串")]
        public string Command
        {
            get { return commandField; }
            set { commandField = value; }
        }
        private ObservableCollection<Parameter> parameterItemsField = new ObservableCollection<Parameter>();

        [XmlElement("parameter")]
        [Category("信息")]
        [DisplayName("参数")]
        [Description("操作的参数集合")]
        public ObservableCollection<Parameter> ParameterItems
        {
            get { return parameterItemsField; }
            set { parameterItemsField = value; }
        }

        public Parameter GetItemByKey(string key)
        {
            foreach (Parameter parameter in parameterItemsField)
            {
                if (parameter.Name.Equals(key))
                {
                    return parameter;
                }
            }
            return null;
        }

        private Device _parent;
        public void SetParent(Device parent)
        {
            _parent = parent;
        }
        public Device GetParent()
        {
            return _parent;
        }
    }
}
