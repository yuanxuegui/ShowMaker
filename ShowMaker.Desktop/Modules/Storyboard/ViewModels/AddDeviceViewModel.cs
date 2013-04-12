using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using OpenRcp;
using ShowMaker.Desktop.Domain;

namespace ShowMaker.Desktop.Modules.Storyboard.ViewModels
{
    [Export(typeof(AddDeviceViewModel))]
    public class AddDeviceViewModel : DisplayBase
    {
        #region View Data

        private string deviceName;
        public string DeviceName 
        { 
            get
            {
                return deviceName;
            }
            set
            {
                deviceName = value;
                NotifyOfPropertyChange(() => DeviceName);
            }
        }

        public Device NewDevice { get; set; }

        #endregion

        #region Override Screen Method

        public override string Name
        {
            get
            {
                return StoryboardModule.STORYBOARD_ADD_DEVICE;
            }
        }

        #endregion


        #region Interaction

        /*
        public bool CanOnAddDevice()
        {
            return string.IsNullOrEmpty(DeviceName) ? false : true;
        }
        */

        public void OnAddDevice()
        {
            NewDevice = new Device();
            NewDevice.Type = DeviceType.CURTAIN;
            
            TryClose();
        }

        #endregion
    }
}
