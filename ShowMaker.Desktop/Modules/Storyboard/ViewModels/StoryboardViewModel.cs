using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Caliburn.Micro;
using OpenRcp;
using ShowMaker.Desktop.Domain;
using ShowMaker.Desktop.Modules.ExhibitionDocument.Messages;
using ShowMaker.Desktop.Modules.Storyboard.Views;
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
            set
            {
                exhibition = value;
                NotifyOfPropertyChange(() => SelectedExhibition);
            }
        }

        private Area selectedArea;
        private Device selectedDevice;
        private Operation selectedOperation;
        private int selectedTick;

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
            NewAreaView areaDlg = new NewAreaView();
            NewAreaViewModel areaDlgVM = IoC.Get<NewAreaViewModel>();
            ViewModelBinder.Bind(areaDlgVM, areaDlg, null);
            areaDlg.ShowDialog();

            Area a = areaDlgVM.NewArea;
            a.SetParent(SelectedExhibition);
            SelectedExhibition.AreaItems.Add(a);
        }

        public void OnAddNewDevice()
        {
            if (selectedArea != null)
            {
                Device dev = new Device();
                dev.SetParent(selectedArea);
                selectedArea.DeviceItems.Add(dev);
            }
            else
                Xceed.Wpf.Toolkit.MessageBox.Show("请选择展区后再添加设备", "错误", System.Windows.MessageBoxButton.OK);
        }

        public void OnAddNewOperation()
        {
            if (selectedDevice != null)
            {
                Operation op = new Operation();
                op.SetParent(selectedDevice);
                selectedDevice.OperationItems.Add(op);
            }
            else
                Xceed.Wpf.Toolkit.MessageBox.Show("请选择设备后再添加操作", "错误", System.Windows.MessageBoxButton.OK);
        }

        public void OnDeviceItemDrop(object sender, System.Windows.DragEventArgs e)
        {
            Device dev = e.Data.GetData(typeof(Device)) as Device;
            if (selectedArea != null)
                selectedArea.DeviceItems.Add(dev);
            else
                Xceed.Wpf.Toolkit.MessageBox.Show("请选择展区后再添加设备", "错误", System.Windows.MessageBoxButton.OK);
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

        public void OnAreaItemClick(object sender, EventArgs e, Area area, StoryboardView view)
        {
            selectedArea = area;
            IoC.Get<IPropertyGrid>().SelectedObject = area;
            // TODO. 加载时间线
            Canvas tlc = view.TimelineCanvas;
            tlc.Children.Clear();
            foreach (TimePoint tp in area.Timeline.TimePointItems)
            {
                foreach (Command cmd in tp.CommandItems)
                {
                    // 绘制时间点图形
                    Ellipse tpg = DrawCommandGraph(tlc, tp.Tick, 100);
                    // 时间点图形添加点击事件
                    tpg.MouseLeftButtonUp += (s, evt) =>
                    {
                        IoC.Get<IPropertyGrid>().SelectedObject = cmd;
                    };
                }
            }
        }

        public void OnDeviceItemClick(object sender, EventArgs e, Device device)
        {
            selectedDevice = device;
            IoC.Get<IPropertyGrid>().SelectedObject = device;
        }

        public void OnOperationItemClick(object sender, EventArgs e, Operation operation)
        {
            selectedOperation = operation;
            IoC.Get<IPropertyGrid>().SelectedObject = operation;
        }

        public void OnAddNewCommand(object sender, EventArgs e, StoryboardView view)
        {
            // 自动关联层次结构
            if (selectedDevice == null)
            {
                selectedDevice = selectedOperation.GetParent();

                if (selectedDevice != null && selectedArea == null)
                    selectedArea = selectedDevice.GetParent();
            }
            if (selectedDevice != null && selectedOperation != null)
            {
                Command cmd = new Command();
                cmd.DeviceId = selectedDevice.Id;
                cmd.OperationName = selectedOperation.Name;
                // TODO. 获取选择的时间点
                Timeline tl = selectedArea.Timeline;
                TimePoint tp = tl.GetItemByKey(selectedTick);
                if (tp == null)
                {
                    tp = new TimePoint(selectedTick);
                    tp.CommandItems.Add(cmd);
                    tl.TimePointItems.Add(tp);
                }
                else
                {
                    tp.CommandItems.Add(cmd);
                }
                // 绘制时间点图形
                Ellipse tpg = DrawCommandGraph(view.TimelineCanvas, selectedTick, 100);
                // 时间点图形添加点击事件
                tpg.MouseLeftButtonUp += (s, evt) =>
                {
                    IoC.Get<IPropertyGrid>().SelectedObject = cmd;
                };
            }
        }

        /// <summary>
        /// 指定位置绘制时间点图形
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="tick"></param>
        /// <param name="position"></param>
        public Ellipse DrawCommandGraph(Canvas canvas, int tick, int position)
        {
            Ellipse tpg = new Ellipse();
            tpg.Fill = System.Windows.Media.Brushes.Black;
            tpg.Width = 20;
            tpg.Height = 50;
            Canvas.SetLeft(tpg, 100 + selectedTick);
            Canvas.SetTop(tpg, position); // 根据选择操作的位置计算
            canvas.Children.Add(tpg);

            return tpg;
        }

        #endregion


    }
}
