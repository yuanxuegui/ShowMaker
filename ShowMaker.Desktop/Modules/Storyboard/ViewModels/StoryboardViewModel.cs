using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using OpenRcp;
using ShowMaker.Desktop.Domain;
using ShowMaker.Desktop.Modules.ExhibitionDocument.Messages;
using ShowMaker.Desktop.Parser;

namespace ShowMaker.Desktop.Modules.Storyboard.ViewModels
{
    [Export(typeof(StoryboardViewModel))]
    public class StoryboardViewModel : Tool, ILocalizableDisplay
    {
        #region View Data

        private Exhibition exhibition;

        public Exhibition SelectedExhibition
        {
            get { return exhibition; }
            set{
                exhibition = value;
                NotifyOfPropertyChange(() => SelectedExhibition);
            }
        }

        private Area selectedArea;
        private Device selectedDevice;

        #endregion

        public StoryboardViewModel()
        {
        }

        #region Override Tool Methods

        public override string Name
        {
            get
            {
                return StoryboardModule.MENU_VIEW_STORYBOARD;
            }
        }

        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Left; }
        }

        #endregion

        #region Interaction

        public void OnAddNewArea()
        {
            SelectedExhibition.AreaItems.Add(new Area("某展区"));
        }

        public void OnAddNewDevice()
        {
            if (selectedArea != null)
                selectedArea.DeviceItems.Add(new Device());
            else
                Xceed.Wpf.Toolkit.MessageBox.Show("请选择展区后再添加设备", "错误", System.Windows.MessageBoxButton.OK);
        }

        public void OnDeviceItemDrop(object sender, System.Windows.DragEventArgs e)
        {
            Device dev = e.Data.GetData(typeof(Device)) as Device;
        }

        public void OnSyncExhibition()
        {
            IoC.Get<IEventAggregator>().Publish(new ContentChangedMessage()
            {
                Content = SelectedExhibition
            });
        }

        public void OnExhibitionClick(object sender, EventArgs e)
        {
            IoC.Get<IPropertyGrid>().SelectedObject = SelectedExhibition;
        }

        public void OnAreaItemClick(object sender, EventArgs e, Area area)
        {
            selectedArea = area;
            IoC.Get<IPropertyGrid>().SelectedObject = area;
        }

        public void OnDeviceItemClick(object sender, EventArgs e, Device device)
        {
            selectedDevice = device;
            IoC.Get<IPropertyGrid>().SelectedObject = device;
        }

        public void OnOperationItemClick(object sender, EventArgs e, Operation operation)
        {
            IoC.Get<IPropertyGrid>().SelectedObject = operation;
        }

        #endregion

        
    }
}
