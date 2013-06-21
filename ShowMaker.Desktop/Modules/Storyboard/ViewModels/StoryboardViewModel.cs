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
using ShowMaker.Desktop.Modules.Storyboard.Controls;
using ShowMaker.Desktop.Models.Domain;

namespace ShowMaker.Desktop.Modules.Storyboard.ViewModels
{
    enum SelectedItemType { AREA, DEVICE, OPERATION};

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
        private double selectedVerticalPosition;
        private SelectedItemType selectedItemType;
        private Ellipse selectedTimePointGraphic;
        private Command selectedCommand;

        private int timelineMaximum;

        public int TimelineMaximum
        {
            get { return timelineMaximum; }
            set
            {
                timelineMaximum = value;
                NotifyOfPropertyChange(() => TimelineMaximum);
            }
        }

        private int timelineWidth;

        public int TimelineWidth
        {
            get { return timelineWidth; }
            set
            {
                timelineWidth = value;
                timelineMaximum = timelineWidth / 10;
                NotifyOfPropertyChange(() => TimelineWidth);
                NotifyOfPropertyChange(() => TimelineMaximum);
            }
        }

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

        public void OnNewArea()
        {
            if (exhibition != null)
            {
                NewAreaView areaDlg = new NewAreaView();
                NewAreaViewModel areaDlgVM = IoC.Get<NewAreaViewModel>();
                ViewModelBinder.Bind(areaDlgVM, areaDlg, null);
                areaDlg.ShowDialog();

                Area a = areaDlgVM.NewArea;
                if (a != null)
                {
                    a.SetParent(SelectedExhibition);
                    SelectedExhibition.AreaItems.Add(a);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("请先新建展区定义文件","错误", System.Windows.MessageBoxButton.OK);
            }
        }

        public void OnAddDevice()
        {
            if (selectedArea != null)
            {
                AddDeviceView devDlg = new AddDeviceView();
                AddDeviceViewModel devDlgVM = IoC.Get<AddDeviceViewModel>();
                ViewModelBinder.Bind(devDlgVM, devDlg, null);
                devDlg.ShowDialog();

                Device dev = devDlgVM.NewDevice;
                if (dev != null)
                {
                    dev.SetParent(selectedArea);
                    selectedArea.DeviceItems.Add(dev);
                }
            }
            else
                System.Windows.MessageBox.Show("请选择展区后再添加设备", "错误", System.Windows.MessageBoxButton.OK);
        }

        public void OnAddOperation()
        {
            if (selectedDevice != null)
            {
                AddOperationView opDlg = new AddOperationView();
                AddOperationViewModel opDlgVM = IoC.Get<AddOperationViewModel>();
                ViewModelBinder.Bind(opDlgVM, opDlg, null);
                opDlg.ShowDialog();

                Operation op = opDlgVM.NewOperation;
                if (op != null)
                {
                    op.SetParent(selectedDevice);
                    selectedDevice.OperationItems.Add(op);
                }
            }
            else
                System.Windows.MessageBox.Show("请选择设备后再添加操作", "错误", System.Windows.MessageBoxButton.OK);
        }

        public void OnDeviceItemDrop(object sender, System.Windows.DragEventArgs e)
        {
            Device dev = e.Data.GetData(typeof(Device)) as Device;
            if (selectedArea != null)
            {
                dev.SetParent(selectedArea);
                selectedArea.DeviceItems.Add(dev);
            }
            else
                System.Windows.MessageBox.Show("请选择展区后再添加设备", "错误", System.Windows.MessageBoxButton.OK);
        }

        public void OnDeleteItem()
        {
            switch (selectedItemType)
            {
                case SelectedItemType.AREA:
                    if (System.Windows.MessageBox.Show("您确定删除" + selectedArea.Name + "展区吗?", "提示", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                        SelectedExhibition.AreaItems.Remove(selectedArea);
                    break;
                case SelectedItemType.DEVICE:
                    if (System.Windows.MessageBox.Show("您确定删除" + selectedDevice.Name + "设备吗?", "提示", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                        selectedArea.DeviceItems.Remove(selectedDevice);
                    break;
                case SelectedItemType.OPERATION:
                    if (System.Windows.MessageBox.Show("您确定删除" + selectedOperation.Name + "操作吗?", "提示", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                        selectedDevice.OperationItems.Remove(selectedOperation);
                    break;
            }

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
            selectedItemType = SelectedItemType.AREA;
            IoC.Get<IPropertyGrid>().SelectedObject = area;
            // TODO. 加载时间线
            TimelineControl tlc = view.timelineControl;
            Canvas drawPanel = tlc.Slider.Template.FindName("DrawPanel", tlc.Slider) as Canvas;
            drawPanel.Children.Clear();
            foreach (TimePoint tp in area.Timeline.TimePointItems)
            {
                foreach (Command cmd in tp.CommandItems)
                {
                    Device dev = area.GetItemByKey(cmd.DeviceId);
                    Operation op = dev.GetItemByKey(cmd.OperationName);
                    double verticalPosition = calculateVerticalPosition(op);
                    // 绘制时间点图形
                    Ellipse tpg = DrawCommandGraph(tlc, tp.Tick, verticalPosition);
                    // 时间点图形添加点击事件
                    tpg.MouseRightButtonDown += (s, evt) =>
                    {
                        IoC.Get<IPropertyGrid>().SelectedObject = cmd;
                        selectedTimePointGraphic = tpg;
                        selectedCommand = cmd;
                    };
                }
            }
        }

        private double calculateVerticalPosition(Operation op)
        {
            Device dev = op.GetParent();
            Area area = dev.GetParent();
            Exhibition ex = area.GetParent();
            int deep = 0;
            int i = 0;
            for(i=0; i<ex.AreaItems.Count; i++)
            {
                Area a = ex.AreaItems[i];
                deep++;
                if (a == area)
                    break;  
            }
            for(i=0; i< area.DeviceItems.Count; i++)
            {
                Device d = area.DeviceItems[i];
                deep++;
                if (d == dev)
                    break;
            }
            for(i=0; i<dev.OperationItems.Count; i++)
            {
                Operation o = dev.OperationItems[i];
                deep++;
                if (o == op)
                    break;
            }
            return (deep-1) * 22;
        }

        public void OnDeviceItemClick(object sender, EventArgs e, Device device)
        {
            selectedDevice = device;
            selectedItemType = SelectedItemType.DEVICE;
            selectedArea = selectedDevice.GetParent();
            IoC.Get<IPropertyGrid>().SelectedObject = device;
        }

        public void OnOperationItemClick(object sender, EventArgs e, Operation operation, StoryboardView view)
        {
            MouseButtonEventArgs me = e as MouseButtonEventArgs;
            selectedVerticalPosition = me.GetPosition(view.timelineControl).Y - 26;
            selectedOperation = operation;
            selectedItemType = SelectedItemType.OPERATION;
            selectedDevice = operation.GetParent();
            selectedArea = selectedDevice.GetParent();
            IoC.Get<IPropertyGrid>().SelectedObject = operation;
        }

        public void OnAddNewCommand(object sender, EventArgs e, StoryboardView view)
        {
            if (selectedDevice != null && selectedOperation != null)
            {
                Command cmd = new Command();
                cmd.DeviceId = selectedDevice.Id;
                cmd.OperationName = selectedOperation.Name;
                foreach (ShowMaker.Desktop.Domain.Parameter param in selectedOperation.ParameterItems)
                {
                    cmd.PropertyItems.Add(new Property(param.Name, ""));
                }
                // TODO. 获取选择的时间点
                selectedTick = (int)view.timelineControl.Slider.Value;
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
                Ellipse tpg = DrawCommandGraph(view.timelineControl, selectedVerticalPosition);
                // 时间点图形添加点击事件
                tpg.MouseRightButtonDown += (s, evt) =>
                {
                    IoC.Get<IPropertyGrid>().SelectedObject = cmd;
                    selectedTimePointGraphic = tpg;
                    selectedCommand = cmd;
                };
            }
        }

        public void OnDeleteCommand(object sender, EventArgs e, StoryboardView view)
        {
            if (selectedTimePointGraphic != null && selectedCommand != null && view != null && System.Windows.MessageBox.Show("您确定删除该命令吗?", "提示", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
            {
                Canvas drawPanel = view.timelineControl.Slider.Template.FindName("DrawPanel", view.timelineControl.Slider) as Canvas;
                drawPanel.Children.Remove(selectedTimePointGraphic);
                selectedTick = (int)view.timelineControl.Slider.Value;
                Timeline tl = selectedArea.Timeline;
                TimePoint tp = tl.GetItemByKey(selectedTick);
                if (tp != null)
                {
                    tp.CommandItems.Remove(selectedCommand);
                    if (tp.CommandItems.Count == 0)
                        tl.TimePointItems.Remove(tp);
                }
            }
        }

        /// <summary>
        /// 指定位置绘制时间点图形
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="tick"></param>
        /// <param name="position"></param>
        public Ellipse DrawCommandGraph(TimelineControl timelineControl, int tick, double verticalPosition)
        {
            Ellipse tpg = new Ellipse();
            tpg.Fill = System.Windows.Media.Brushes.DimGray;
            tpg.Width = 30;
            tpg.Height = 20;

            Canvas.SetLeft(tpg, tick * 10 - tpg.Width / 2);
            Canvas.SetTop(tpg, verticalPosition); // 根据选择操作的位置计算
            Canvas drawPanel = timelineControl.Slider.Template.FindName("DrawPanel", timelineControl.Slider) as Canvas;
            if (drawPanel != null)
                drawPanel.Children.Add(tpg);

            return tpg;
        }

        /// <summary>
        /// 绘制时间点图形
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="tick"></param>
        /// <param name="position"></param>
        public Ellipse DrawCommandGraph(TimelineControl timelineControl, double verticalPosition)
        {
            Ellipse tpg = new Ellipse();
            tpg.Fill = System.Windows.Media.Brushes.DimGray;
            tpg.Width = 30;
            tpg.Height = 20;
            Canvas.SetLeft(tpg, timelineControl.Slider.Value * 10 - tpg.Width / 2);
            Canvas.SetTop(tpg, verticalPosition); // 根据选择操作的位置计算
            Canvas drawPanel = timelineControl.Slider.Template.FindName("DrawPanel", timelineControl.Slider) as Canvas;
            if (drawPanel != null)
                drawPanel.Children.Add(tpg);

            return tpg;
        }

        public void OnTimelineControlZoomValueChanged(object sender, EventArgs e)
        {
            Slider sl = sender as Slider;
            if (sl != null && selectedArea != null)
            {
                TimelineMaxChangedMessage tm = new TimelineMaxChangedMessage();
                tm.Max = (int)sl.Value / 10;
                tm.TimelineTarget = selectedArea.Timeline;
                IoC.Get<IEventAggregator>().Publish(tm);
            }
        }

        #endregion


    }
}
