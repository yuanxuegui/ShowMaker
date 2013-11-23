using System.Windows.Input;
using Caliburn.Micro;

namespace OpenRcp
{
    public abstract class Document : DisplayBase, IDocument
	{
        public Document()
            : base(false) // NOT auto update DisplayName
        {
        }
        
        private ICommand _closeCommand;
		public ICommand CloseCommand
		{
			get
			{
				return _closeCommand ?? (_closeCommand = new RelayCommand(p => TryClose(null), p => true));
			}
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
            IoC.Get<IEventAggregator>().Publish(new PropertySelectedMessage() {
                SelectedObject = propertyObject
            });
        }
    }
}