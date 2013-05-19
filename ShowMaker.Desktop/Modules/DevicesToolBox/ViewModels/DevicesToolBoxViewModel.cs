﻿using System;
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
using System.Collections.ObjectModel;
using ShowMaker.Desktop.Util;
using ShowMaker.Desktop.Models.Domain;
using System.IO;
using System.Reflection;

namespace ShowMaker.Desktop.Modules.DevicesToolBox.ViewModels
{
    [Export(typeof(DevicesToolBoxViewModel))]
    public class DevicesToolBoxViewModel : Tool, ILocalizableDisplay
    {
        private ObservableCollection<Device> deviceItems = new ObservableCollection<Device>();

        public ObservableCollection<Device> DeviceItems
        {
            get { return deviceItems; }
            set
            {
                deviceItems = value;
                NotifyOfPropertyChange(() => DeviceItems);
            }
        }

        public DevicesToolBoxViewModel()
        {
            string devicesXml = Assembly.GetExecutingAssembly().Location + @"\..\" + Constants.DEVICES_FILE;
            if (File.Exists(devicesXml))
            {
                DeviceCollection dc = (DeviceCollection)XmlSerializerUtil.LoadXml(typeof(DeviceCollection), devicesXml);
                DeviceItems = dc.DeviceItems;
                connectOperationParent(DeviceItems);
            }
            else
            {
                Device curtainDev = new Device();
                curtainDev.Type = DeviceType.CURTAIN;
                Operation op1 = new Operation();
                op1.Name = "开关";
                op1.SetParent(curtainDev);
                curtainDev.OperationItems.Add(op1);
                DeviceItems.Add(curtainDev);

                Device fogDev = new Device();
                fogDev.Type = DeviceType.FOG;
                DeviceItems.Add(fogDev);
            }
        }

        private void connectOperationParent(ObservableCollection<Device> devices)
        {
            foreach (Device dev in devices)
            {
                foreach (Operation op in dev.OperationItems)
                {
                    op.SetParent(dev);
                }
            }
        }

        #region Override Tool Methods

        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Right; }
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
