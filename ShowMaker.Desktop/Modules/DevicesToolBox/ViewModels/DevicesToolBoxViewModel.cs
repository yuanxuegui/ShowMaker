using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using Caliburn.Micro;
using OpenRcp;
using ShowMaker.Desktop.Domain;
using System.Windows.Input;
using System.Windows.Controls;

namespace ShowMaker.Desktop.Modules.DevicesToolBox.ViewModels
{
    [Export(typeof(DevicesToolBoxViewModel))]
    public class DevicesToolBoxViewModel : Tool, ILocalizableDisplay
    {
        private Device[] deviceItems;

        public Device[] DeviceItems
        {
            get { return deviceItems; }
            set
            {
                deviceItems = value;
                NotifyOfPropertyChange(() => DeviceItems);
            }
        }

        private string hello;

        [Category("Information")]
        [DisplayName("Hello String")]
        [Description("This property uses a TextBox as the default editor.")]
        public string Hello
        {
            get { return hello; }
            set { hello = value; }
        }

        public DevicesToolBoxViewModel()
        {
            hello = "Hello World";
            DeviceItems = new Device[2];
            DeviceItems[0] = new Device();
            DeviceItems[1] = new Device();
            DeviceItems[0].Type = DeviceType.CURTAIN;
            DeviceItems[1].Type = DeviceType.PC;
        }

        #region Override Tool Methods

        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Left; }
        }

        public override string Name
        {
            get { return DevicesToolBoxModule.MENU_VIEW_DEVTOOLBOX; }
        }

        #endregion

        #region Interaction
		
        public void OnDeviceItemMouseMove(object sender, MouseEventArgs e, object selectedDevice)
        {
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                if (sender != null)
                {
					DragDrop.DoDragDrop(sender as StackPanel, new DataObject(selectedDevice), DragDropEffects.Copy);
                }
            }
        }
		
        #endregion
    }
}
