using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ShowMaker.Desktop.Domain
{
    public class Operation : IItemFinder<Parameter, string>
    {
        private string nameField;

        [XmlAttribute("name")]
        public string Name
        {
            get { return nameField; }
            set { nameField = value; }
        }
        private string commandField;

        [XmlAttribute("command")]
        public string Command
        {
            get { return commandField; }
            set { commandField = value; }
        }
        private ObservableCollection<Parameter> parameterItemsField = new ObservableCollection<Parameter>();

        [XmlElement("parameter")]
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
    }
}
