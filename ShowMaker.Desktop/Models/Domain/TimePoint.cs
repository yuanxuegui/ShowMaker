using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ShowMaker.Desktop.Domain
{
    public class TimePoint
    {
        private int tickField;

        [XmlAttribute("tick")]
        public int Tick
        {
            get { return tickField; }
            set { tickField = value; }
        }
        private ObservableCollection<Command> commandItemsField = new ObservableCollection<Command>();

        [XmlElement("command")]
        public ObservableCollection<Command> CommandItems
        {
            get { return commandItemsField; }
            set { commandItemsField = value; }
        }

        public TimePoint() { }

        public TimePoint(int tick)
        {
            Tick = tick;
        }
    }
}
