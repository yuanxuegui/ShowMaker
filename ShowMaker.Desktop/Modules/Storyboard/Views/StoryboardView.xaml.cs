using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShowMaker.Desktop.Models.Domain;
using Caliburn.Micro;

namespace ShowMaker.Desktop.Modules.Storyboard.Views
{
    /// <summary>
    /// StoryboardView.xaml 的交互逻辑
    /// </summary>
    public partial class StoryboardView : UserControl
    {
        public StoryboardView()
        {
            InitializeComponent();
        }

        private void timelineControlZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider sl = sender as Slider;
            if (sl != null)
            {
                TimelineMaxChangedMessage tm = new TimelineMaxChangedMessage();
                tm.Max = (int)sl.Value / 10;
                IoC.Get<IEventAggregator>().Publish(tm);
            }
        }
    }
}
