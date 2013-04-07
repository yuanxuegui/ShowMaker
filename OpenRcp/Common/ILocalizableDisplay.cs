using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace OpenRcp
{
    /// <summary>
    /// Denotes an extend <see cref="IDisplay"/> which is localizable to update its DisplayName.
    /// </summary>
    public interface ILocalizableDisplay : IDisplay, IHandle<LanguageChangedMessage>
    {
        void AutoUpdateDisplayName(bool isAutoUpdate);
        void UpdateDisplayName();
    }
}
