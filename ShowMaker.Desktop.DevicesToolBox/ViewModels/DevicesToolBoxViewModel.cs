﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using Caliburn.Micro;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using OpenRcp;
using ShowMaker.Desktop.Models.Domain;
using ShowMaker.Desktop.Models.Util;


namespace ShowMaker.Desktop.Modules.DevicesToolBox.ViewModels
{
    /// <summary>
    /// 设备工具箱的ViewModel
    /// </summary>
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
                    Device dev = selectedDevice as Device;
                    if (dev != null)
                    {
                        Device devCopy = new Device();
                        devCopy.Id = dev.Id;
                        devCopy.Name = dev.Name;
                        devCopy.Type = dev.Type;
                        foreach(Operation op in dev.OperationItems)
                        {
                            Operation opCopy = new Operation();
                            opCopy.Name = op.Name;
                            opCopy.Command = op.Command;
                            foreach (ShowMaker.Desktop.Models.Domain.Parameter param in op.ParameterItems)
                            {
                                ShowMaker.Desktop.Models.Domain.Parameter paramCopy = new ShowMaker.Desktop.Models.Domain.Parameter();
                                paramCopy.Name = param.Name;
                                paramCopy.Type = param.Type;
                                paramCopy.MinValue = param.MinValue;
                                paramCopy.MaxValue = param.MaxValue;
                                opCopy.ParameterItems.Add(paramCopy);
                            }
                            opCopy.SetParent(devCopy);
                            devCopy.OperationItems.Add(opCopy);
                        }
                        DragDrop.DoDragDrop(sender as StackPanel, new DataObject(devCopy), DragDropEffects.Copy);
                    }
                    
                }
            }
        }

        #endregion
    }
}
