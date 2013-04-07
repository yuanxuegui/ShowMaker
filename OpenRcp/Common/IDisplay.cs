using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace OpenRcp
{
    /// <summary>
    /// Denotes an instance which implements <see cref="IHaveName"/> and <see cref="IHaveDisplayName"/>.
    /// </summary>
    public interface IDisplay : IHaveName, IHaveDisplayName
    {
    }
}
