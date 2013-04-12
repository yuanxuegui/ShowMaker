using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace OpenRcp
{
    public class DisplayBase : Screen, ILocalizableDisplay
    {
        public DisplayBase()
        {
            AutoUpdateDisplayName(true);
        }

        public DisplayBase(bool autoUpdate)
        {
            AutoUpdateDisplayName(autoUpdate);
        }
        
        #region ILocalizableDisplay Implementation

        public void AutoUpdateDisplayName(bool isAutoUpdate)
        {
            IEventAggregator eventAggregator = IoC.Get<IEventAggregator>();
            if (isAutoUpdate)
            {
                eventAggregator.Subscribe(this);
                this.UpdateDisplayName();
            }
            else
            {
                eventAggregator.Unsubscribe(this);
            }
        }

        public void UpdateDisplayName()
        {
            DisplayName = IoC.Get<IResourceService>().GetString(Name);
            if (DisplayName == null)
                DisplayName = Name;
        }

        public virtual string Name
        {
            get { return null; }
        }

        public void Handle(LanguageChangedMessage message)
        {
            this.UpdateDisplayName();
        }

        #endregion

    }
}
