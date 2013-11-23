﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ShowMaker.Desktop.Models.Domain
{
    public interface ITimelineContext : IPropertyFinder
    {
        void MergeProperties(ObservableCollection<Property> properties);
        Area GetArea();
        Timeline GetTimeline();
    }
}
