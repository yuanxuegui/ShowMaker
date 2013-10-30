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
using ShowMaker.Desktop.Util;
using System.Windows.Data;
using System.Windows;

namespace ShowMaker.Desktop.Modules.Storyboard.ViewModels
{
    enum SelectedItemType { EXHIBITION, AREA, DEVICE, OPERATION};

    [Export(typeof(StoryboardViewModel))]
    public class StoryboardViewModel : Tool, ILocalizableDisplay, IHandle<ShowDefinationChangedMessage>
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

        public Device SelectedDevice
        {
            get { return selectedDevice; }
            set
            {
                selectedDevice = value;
                NotifyOfPropertyChange(() => SelectedDevice);
            }
        }


        private Operation selectedOperation;
        private int selectedTick;
        private double selectedVerticalPosition;
        private SelectedItemType selectedItemType;
        private Shape selectedTimePointGraphic;
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

        private int deviceCtrHeight = 50;

        public StoryboardViewModel()
        {
            IoC.Get<IEventAggregator>().Subscribe(this);
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
                    IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
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
                    IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
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
                    IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
                }
            }
            else
                System.Windows.MessageBox.Show("请选择设备后再添加操作", "错误", System.Windows.MessageBoxButton.OK);
        }

        private void addDeviceToAreaListPanel(StackPanel panel, Device dev)
        {
            Button deviceCtr = new Button();
            deviceCtr.Height = deviceCtrHeight;
            deviceCtr.FontSize = 15;
            deviceCtr.HorizontalAlignment = HorizontalAlignment.Stretch;
            Binding binding = new Binding("Name");
            binding.Source = dev;
            deviceCtr.SetBinding(Button.ContentProperty, binding);
            deviceCtr.DataContext = dev;
            deviceCtr.Click += (s, evt) =>
            {
                Button b = s as Button;
                SelectedDevice = b.DataContext as Device;
                selectedItemType = SelectedItemType.DEVICE;
                IoC.Get<IPropertyGrid>().SelectedObject = SelectedDevice;
                int deep = -1;
                for (int i = 0; i < selectedArea.DeviceItems.Count; i++)
                {
                    Device d = selectedArea.DeviceItems[i];
                    deep++;
                    if (d == SelectedDevice)
                        break;
                }
                selectedVerticalPosition = deep * b.Height;
            };
            panel.Children.Add(deviceCtr);
        }

        public void OnDeviceItemDrop(object sender, System.Windows.DragEventArgs e)
        {
            Device dev = e.Data.GetData(typeof(Device)) as Device;
            if (selectedArea != null)
            {
                StoryboardView view = GetView() as StoryboardView;
                dev.SetParent(selectedArea);
                selectedArea.DeviceItems.Add(dev);
                StackPanel sp = sender as StackPanel;
                addDeviceToAreaListPanel(sp, dev);

                // TODO.画线条
                TimelineControl tlc = view.timelineControl;
                Canvas drawPanel = tlc.Slider.Template.FindName("DrawPanel", tlc.Slider) as Canvas;
                drawPanel.Height = selectedArea.DeviceItems.Count * this.deviceCtrHeight;
                /*
                Rectangle devTimelineArea = new Rectangle();
                devTimelineArea.Fill = System.Windows.Media.Brushes.AntiqueWhite;
                devTimelineArea.Stroke = System.Windows.Media.Brushes.Blue;
                devTimelineArea.StrokeThickness = 10;

                devTimelineArea.Width = tlc.Width;
                devTimelineArea.Height = tb.Height;
                Canvas.SetLeft(devTimelineArea, 0);
                Canvas.SetTop(devTimelineArea, (selectedArea.DeviceItems.Count - 1) * tb.Height);
                drawPanel.Children.Add(devTimelineArea);
                */

                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
            else
                System.Windows.MessageBox.Show("请选择展区后再添加设备", "错误", System.Windows.MessageBoxButton.OK);
        }

        public void OnDeleteItem()
        {
            switch (selectedItemType)
            {
                case SelectedItemType.EXHIBITION:
                    System.Windows.MessageBox.Show("展示会不能被删除!", "错误", System.Windows.MessageBoxButton.OK);
                    break;
                case SelectedItemType.AREA:
                    if (System.Windows.MessageBox.Show("您确定删除" + selectedArea.Name + "展区吗?", "提示", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes) {
                        SelectedExhibition.AreaItems.Remove(selectedArea);
                        IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
                    }
                    break;
                case SelectedItemType.DEVICE:
                    if (System.Windows.MessageBox.Show("您确定删除" + selectedDevice.Name + "设备吗?", "提示", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes) {
                        selectedArea.DeviceItems.Remove(selectedDevice);
                        IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
                    }
                    break;
                case SelectedItemType.OPERATION:
                    if (System.Windows.MessageBox.Show("您确定删除" + selectedOperation.Name + "操作吗?", "提示", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                    {
                        selectedDevice.OperationItems.Remove(selectedOperation);
                        IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
                    }
                    break;
            }

        }

        public void OnExhibitionClick(object sender, EventArgs e)
        {
            IoC.Get<IPropertyGrid>().SelectedObject = SelectedExhibition;
            selectedItemType = SelectedItemType.EXHIBITION;
        }

        private void selectedCommandShapeHandle(object source, MouseButtonEventArgs evt)
        {
            Shape tpg = source as Shape;
            Command cmd = tpg.DataContext as Command;
            IoC.Get<IPropertyGrid>().SelectedObject = cmd;
            focusSelectedCommandShape(tpg);
            selectedCommand = cmd;
        }

        private void focusSelectedCommandShape(Shape selectedCmdShape)
        {
            if (this.selectedTimePointGraphic != null)
            {
                selectedTimePointGraphic.Fill = System.Windows.Media.Brushes.AntiqueWhite;
                selectedTimePointGraphic.Stroke = System.Windows.Media.Brushes.Blue;
                selectedTimePointGraphic.StrokeThickness = 10;
            }
            selectedTimePointGraphic = selectedCmdShape;
            selectedCmdShape.Fill = System.Windows.Media.Brushes.AntiqueWhite;
            selectedCmdShape.Stroke = System.Windows.Media.Brushes.Green;
            selectedCmdShape.StrokeThickness = 10;
        }

        public void OnAreaItemSelected(object sender, EventArgs e, Area area)
        {
            if (area == null) return;
            selectedArea = area;
            selectedItemType = SelectedItemType.AREA;
            IoC.Get<IPropertyGrid>().SelectedObject = area;
            this.TimelineWidth = int.Parse(area.Timeline.GetPropertyValue(Constants.TIME_MAX_KEY)) * 10;

            StoryboardView view = GetView() as StoryboardView;
            // 加载设备列表
            StackPanel sp = view.FindName("ShowAreaDevicesPanel") as StackPanel;
            sp.Children.Clear();
            foreach (Device dev in selectedArea.DeviceItems)
            {
                addDeviceToAreaListPanel(sp, dev);
            }

            // 加载时间线
            TimelineControl tlc = view.timelineControl;
            Canvas drawPanel = tlc.Slider.Template.FindName("DrawPanel", tlc.Slider) as Canvas;
            drawPanel.Height = selectedArea.DeviceItems.Count * this.deviceCtrHeight;
            drawPanel.Children.Clear();
            foreach (TimePoint tp in area.Timeline.TimePointItems)
            {
                foreach (Command cmd in tp.CommandItems)
                {
                    Device dev = area.GetItemByKey(cmd.DeviceId);
                    Operation op = dev.GetItemByKey(cmd.OperationName);
                    if(op == null)
                    {
                        
                        String msg = String.Format("查找设备{0}[{1}]的操作[{2}]失败",dev.Name, dev.Id,cmd.OperationName);
                        System.Windows.MessageBox.Show(msg, "错误");
                        break;
                    }
                    double verticalPosition = calculateVerticalPosition(op);
                    // 绘制时间点图形
                    Shape tpg = DrawCommandGraph(tlc, tp.Tick, verticalPosition);
                    tpg.DataContext = cmd;
                    // 时间点图形添加点击事件
                    tpg.MouseRightButtonDown += selectedCommandShapeHandle;
                }
            }
        }

        private double calculateVerticalPosition(Operation op)
        {
            if (op == null) return 0;
            Device dev = op.GetParent();
            Area area = dev.GetParent();
            Exhibition ex = area.GetParent();
            int deep = -1;
            int i = 0;
            for(i=0; i< area.DeviceItems.Count; i++)
            {
                Device d = area.DeviceItems[i];
                deep++;
                if (d == dev)
                    break;
            }
            return deep * this.deviceCtrHeight;
        }

        public void OnDeviceItemClick(object sender, EventArgs e, Device device)
        {
            selectedDevice = device;
            selectedItemType = SelectedItemType.DEVICE;
            selectedArea = selectedDevice.GetParent();
            IoC.Get<IPropertyGrid>().SelectedObject = device;
        }

        public void OnOperationItemSelected(object sender, EventArgs e, Operation operation)
        {
            if (operation == null) return;
            StoryboardView view = GetView() as StoryboardView;
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
                    IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
                }
                else
                {
                    tp.CommandItems.Add(cmd);
                    IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
                }
                // 绘制时间点图形
                Shape tpg = DrawCommandGraph(view.timelineControl, selectedVerticalPosition);
                tpg.DataContext = cmd;
                // 时间点图形添加点击事件
                tpg.MouseRightButtonDown += selectedCommandShapeHandle;
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
                    IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
                }
            }
        }

        /// <summary>
        /// 指定位置绘制时间点图形
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="tick"></param>
        /// <param name="position"></param>
        public Shape DrawCommandGraph(TimelineControl timelineControl, int tick, double verticalPosition)
        {
            Rectangle tpg = new Rectangle();
            tpg.Fill = System.Windows.Media.Brushes.AntiqueWhite;
            tpg.Stroke = System.Windows.Media.Brushes.Blue;
            tpg.StrokeThickness = 10;

            tpg.Width = 10;
            tpg.Height = this.deviceCtrHeight;

            Canvas.SetLeft(tpg, tick * 10);
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
        public Shape DrawCommandGraph(TimelineControl timelineControl, double verticalPosition)
        {
            Rectangle tpg = new Rectangle();
            tpg.Fill = System.Windows.Media.Brushes.AntiqueWhite;
            tpg.Stroke = System.Windows.Media.Brushes.Blue;
            tpg.StrokeThickness = 10;

            tpg.Width = 10;
            tpg.Height = this.deviceCtrHeight;

            Canvas.SetLeft(tpg, timelineControl.Slider.Value * 10);
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
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
        }

        #endregion

        public void Handle(ShowDefinationChangedMessage message)
        {
            IoC.Get<IEventAggregator>().Publish(new ContentChangedMessage()
            {
                Content = SelectedExhibition
            });
        }
    }
}
