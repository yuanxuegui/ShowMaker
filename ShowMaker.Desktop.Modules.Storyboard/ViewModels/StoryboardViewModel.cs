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
using ShowMaker.Desktop.Modules.Storyboard.Views;
using ShowMaker.Desktop.Modules.Storyboard.Controls;
using ShowMaker.Desktop.Models.Domain;
using ShowMaker.Desktop.Models.Parser;
using ShowMaker.Desktop.Models.Util;
using System.Windows.Data;
using System.Windows;
using Xceed.Wpf.Toolkit;

namespace ShowMaker.Desktop.Modules.Storyboard.ViewModels
{
    /// <summary>
    /// 故事板工具的ViewModel
    /// </summary>
    [Export(typeof(StoryboardViewModel))]
    public class StoryboardViewModel : Tool, ILocalizableDisplay, IHandle<ShowDefinationChangedMessage>, IHandle<SelectedExhibitionChangedMessage>
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
        public Area SelectedArea
        {
            get { return selectedArea; }
            set
            {
                selectedArea = value;
                NotifyOfPropertyChange(() => SelectedArea);
            }
        }

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
        private Shape selectedTimePointGraphic;
        private Command selectedCommand;

        private int timelineMaximum = 10;

        public int TimelineMaximum
        {
            get { return timelineMaximum; }
            set
            {
                timelineMaximum = value;
                TimelineWidth = timelineMaximum * 10;
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
                NotifyOfPropertyChange(() => TimelineWidth);
            }
        }

        #endregion

        private int deviceCtrHeight = 50;

        public int DeviceCtrHeight
        {
            get { return deviceCtrHeight; }
            set
            {
                deviceCtrHeight = value;
                NotifyOfPropertyChange(() => DeviceCtrHeight);
            }
        }

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

        private void propertySelected(object propertyObject)
        {
            IoC.Get<IEventAggregator>().Publish(new PropertySelectedMessage()
            {
                SelectedObject = propertyObject
            });
        }
        public void OnNewArea()
        {
            if (exhibition != null)
            {
                NewAreaView areaDlg = new NewAreaView();
                NewAreaViewModel areaDlgVM = IoC.Get<NewAreaViewModel>();
                areaDlgVM.Clear();
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
                devDlgVM.Clear();
                ViewModelBinder.Bind(devDlgVM, devDlg, null);
                devDlg.ShowDialog();

                Device dev = devDlgVM.NewDevice;
                if (dev != null)
                {
                    dev.SetParent(selectedArea);
                    SelectedArea.DeviceItems.Add(dev);
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
                opDlgVM.Clear();
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

        public void OnDeviceItemDrop(object sender, System.Windows.DragEventArgs e)
        {
            Device dev = e.Data.GetData(typeof(Device)) as Device;
            if (selectedArea != null)
            {
                StoryboardView view = GetView() as StoryboardView;
                dev.SetParent(selectedArea);
                SelectedArea.DeviceItems.Add(dev);
                StackPanel sp = sender as StackPanel;

                // TODO.画线条
                TimelineControl tlc = view.timelineControl;
                Canvas drawPanel = tlc.Slider.Template.FindName("DrawPanel", tlc.Slider) as Canvas;
                drawPanel.Height = selectedArea.DeviceItems.Count * this.deviceCtrHeight;
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
            else
                System.Windows.MessageBox.Show("请选择展区后再添加设备", "错误", System.Windows.MessageBoxButton.OK);
        }

        /// <summary>
        /// 移除展区
        /// </summary>
        public void OnDeleteArea()
        {
            if (selectedArea == null) return;
            if (System.Windows.MessageBox.Show("您确定删除\"" + selectedArea.Name + "\"展区吗?", "提示", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes) {
                SelectedExhibition.AreaItems.Remove(selectedArea);
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
        }

        /// <summary>
        /// 移除设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="dev"></param>
        public void OnDeleteDevice(object sender, EventArgs e, Device dev)
        {
            if (dev == null) return;
            selectedDevice = dev;
            if (System.Windows.MessageBox.Show("您确定删除\"" + selectedDevice.Name + "\"设备吗?", "提示", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
            {
                SelectedArea.DeviceItems.Remove(dev);
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
        }

        /// <summary>
        /// 移除操作
        /// </summary>
        public void OnDeleteOperation()
        {
            if (selectedOperation == null) return;
            if (System.Windows.MessageBox.Show("您确定删除\"" + selectedOperation.Name + "\"操作吗?", "提示", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
            {
                selectedDevice.OperationItems.Remove(selectedOperation);
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage());
            }
        }

        private void selectedCommandShapeHandle(object source, MouseButtonEventArgs evt)
        {
            Shape tpg = source as Shape;
            Command cmd = tpg.DataContext as Command;
            propertySelected(cmd);
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

        private void clearStoryboardContent()
        {
            SelectedExhibition = null;
            SelectedArea = null;
            SelectedDevice = null;
            selectedOperation = null;
            ClearTimelineDrawPanel();
        }

        public void ClearTimelineDrawPanel()
        {
            StoryboardView view = GetView() as StoryboardView;
            // 加载时间线
            TimelineControl tlc = view.timelineControl;
            Canvas drawPanel = tlc.Slider.Template.FindName("DrawPanel", tlc.Slider) as Canvas;
            drawPanel.Children.Clear();
        }
        public void OnAreaItemSelected(object sender, EventArgs e, Area area)
        {
            if (area == null) return;
            SelectedArea = area;
            propertySelected(area);
            this.TimelineMaximum = int.Parse(area.Timeline.GetPropertyValue(Constants.TIME_MAX_KEY));

            StoryboardView view = GetView() as StoryboardView;
            // 加载时间线
            TimelineControl tlc = view.timelineControl;
            Canvas drawPanel = tlc.Slider.Template.FindName("DrawPanel", tlc.Slider) as Canvas;
            drawPanel.Height = SelectedArea.DeviceItems.Count * this.deviceCtrHeight;
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
                    double verticalPosition = calculateVerticalPosition(dev);
                    // 绘制时间点图形
                    Shape tpg = DrawCommandGraph(tlc, tp.Tick, verticalPosition);
                    tpg.DataContext = cmd;
                    // 时间点图形添加点击事件
                    tpg.MouseRightButtonDown += selectedCommandShapeHandle;
                }
            }
        }

        private double calculateVerticalPosition(Device dev)
        {
            if (dev == null) return 0;
            Area area = dev.GetParent();

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

        public void OnDeviceItemListSelectionChanged(object sender, SelectionChangedEventArgs e, Device device)
        {
            if (device == null || selectedArea == null) return;
            SelectedDevice = device;
            selectedOperation = null; // 重置当前选择的操作
            propertySelected(device);
            int deep = -1;
            for (int i = 0; i < selectedArea.DeviceItems.Count; i++)
            {
                Device d = selectedArea.DeviceItems[i];
                deep++;
                if (d == SelectedDevice)
                    break;
            }
            selectedVerticalPosition = deep * this.deviceCtrHeight;
        }

        public void OnOperationItemSelected(object sender, EventArgs e, Operation operation)
        {
            if (operation == null) return;
            StoryboardView view = GetView() as StoryboardView;
            selectedOperation = operation;
            selectedDevice = operation.GetParent();
            selectedArea = selectedDevice.GetParent();
            propertySelected(operation);
        }

        public void OnAddNewCommand(object sender, EventArgs e, StoryboardView view)
        {
            if (selectedDevice != null && selectedOperation != null)
            {
                Command cmd = new Command();
                cmd.DeviceId = selectedDevice.Id;
                cmd.OperationName = selectedOperation.Name;
                foreach (ShowMaker.Desktop.Models.Domain.Parameter param in selectedOperation.ParameterItems)
                {
                    cmd.PropertyItems.Add(new Property(param.Name, ""));
                }
                // 获取选择的时间点
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
        /// <param name="timelineControl"></param>
        /// <param name="tick"></param>
        /// <param name="verticalPosition"></param>
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
        /// <param name="timelineControl"></param>
        /// <param name="verticalPosition"></param>
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
            IntegerUpDown sl = sender as IntegerUpDown;
            if (sl != null && selectedArea != null)
            {
                // 同步修改时间线定义文件-时间线max值
                TimelineMaxChangedMessage tm = new TimelineMaxChangedMessage();
                tm.Max = (int)sl.Value;
                TimelineMaximum = (int)sl.Value;
                tm.TimelineTarget = selectedArea.Timeline;
                IoC.Get<IEventAggregator>().Publish(tm); // 发送给Timeline处理
                IoC.Get<IEventAggregator>().Publish(new ShowDefinationChangedMessage()); // 发送show定义变更消息
            }
        }

        #endregion

        public void Handle(ShowDefinationChangedMessage message)
        {
            // 发送更新编辑器内容消息
            IoC.Get<IEventAggregator>().Publish(new ExhibitionContentChangedMessage()
            {
                Content = SelectedExhibition
            });
        }

        public void Handle(SelectedExhibitionChangedMessage message)
        {
            clearStoryboardContent();
            SelectedExhibition = message.SelectedExhibition;
        }
    }
}
