using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowMaker.Desktop.Models.Domain
{
    public interface IPropertyFinder
    {
        string GetPropertyValue(string name);
    }
}
