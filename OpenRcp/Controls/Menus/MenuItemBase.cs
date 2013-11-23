using System.Collections;
using System.Collections.Generic;
using Caliburn.Micro;

namespace OpenRcp
{
    public class MenuItemBase : PropertyChangedBase, IEnumerable<MenuItemBase>, ILocalizableDisplay
    {
        #region Static stuff

        public static MenuItemBase Separator
        {
            get { return new MenuItemSeparator(); }
        }

        #endregion

        #region Properties

        public IObservableCollection<MenuItemBase> Children { get; private set; }

        private string name;
        public virtual string Name
        {
            get { return name; }
            protected set { name = value; NotifyOfPropertyChange(() => Name); }
        }

        #endregion

        #region Constructors

        protected MenuItemBase()
        {
            Children = new BindableCollection<MenuItemBase>();
        }

        protected MenuItemBase(string name) : this()
        { 
            Name = name;
            AutoUpdateDisplayName(true);// Auto update displayName
        }

        #endregion

        public MenuItemBase Add(params MenuItemBase[] menuItems)
        {
            menuItems.Apply(Children.Add);
            return this;
        }

        public IEnumerator<MenuItemBase> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private string displayName;
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; NotifyOfPropertyChange(() => DisplayName); }
        }

        public void Handle(LanguageChangedMessage message)
        {
            this.UpdateDisplayName();
        }

        public void UpdateDisplayName()
        {
            DisplayName = IoC.Get<IResourceService>().GetString(Name);
            if (DisplayName == null)
                DisplayName = Name;
        }

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
    }
}