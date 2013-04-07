using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace OpenRcp
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportResourceAttribute : ExportAttribute
    {
        public ExportResourceAttribute() : base(typeof(IResource)) { }
        public ExportResourceAttribute(Type contractType) : base(contractType) { }
    }
}
