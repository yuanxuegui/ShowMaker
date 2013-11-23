using System;
using System.Windows.Input;
using Caliburn.Micro;

namespace OpenRcp
{
    public abstract class Tool : DisplayBase, ITool
    {
        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(p => IsVisible = false, p => true)); }
        }

        public abstract PaneLocation PreferredLocation { get; }

        public virtual Uri IconSource
        {
            get { return null; }
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                NotifyOfPropertyChange(() => IsVisible);
            }
        }

        protected Tool()
            : base(true) // Auto update DisplayName
        {
            IsVisible = true;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            if (this is IPropertySelectable)
            {
                var propSel = this as IPropertySelectable;
                if (propSel != null)
                {
                    SelectPropertyObject(propSel.SelectProperty());
                }
            }
        }

        public void SelectPropertyObject(object propertyObject)
        {
            IoC.Get<IEventAggregator>().Publish(new PropertySelectedMessage()
            {
                SelectedObject = propertyObject
            });
        }
    }
}