using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace OpenRcp.Modules.PropertyGrid
{
	[Export(typeof(IPropertyGrid))]
	public class PropertyGridViewModel : Tool, IPropertyGrid, IHandle<PropertySelectedMessage>
    {
        
        #region Override Tool Methods

        public override string Name
		{
			get { return PropertyModule.MENU_VIEW_PROP; }
		}

		public override PaneLocation PreferredLocation
		{
			get { return PaneLocation.Right; }
		}

        #endregion

        private object _selectedObject;
		public object SelectedObject
		{
			get { return _selectedObject; }
			set
			{
				_selectedObject = value;
				NotifyOfPropertyChange(() => SelectedObject);
			}
		}

        public void Handle(PropertySelectedMessage message)
        {
            SelectedObject = message.SelectedObject;
        }
    }
}