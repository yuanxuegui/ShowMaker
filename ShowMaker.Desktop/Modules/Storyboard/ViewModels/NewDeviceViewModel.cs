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
    [Export(typeof(NewDeviceViewModel))]
    public class NewDeviceViewModel : Screen
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

        public override string DisplayName
        {
            get
            {
                return "新建设备";
            }
            set
            {
                base.DisplayName = value;
            }
        }

        #endregion


        #region Interaction

        /*
        public bool CanOnNewDevice()
        {
            return string.IsNullOrEmpty(DeviceName) ? false : true;
        }
        */

        public void OnNewDevice()
        {
            NewDevice = new Device();
            NewDevice.Type = DeviceType.CURTAIN;
            
            TryClose();
        }

        #endregion
    }
}
