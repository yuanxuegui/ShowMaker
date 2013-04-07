using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace OpenRcp
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportModuleAttribute : ExportAttribute
    {
        public ExportModuleAttribute() : base(typeof(IModule)) { }
        public ExportModuleAttribute(Type contractType) : base(contractType) { }
    }
}
