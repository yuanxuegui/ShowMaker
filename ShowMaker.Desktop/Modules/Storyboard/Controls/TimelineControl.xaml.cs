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

namespace ShowMaker.Desktop.Modules.Storyboard.Controls
{
    /// <summary>
    /// TimelineControl.xaml 的交互逻辑
    /// </summary>
    public partial class TimelineControl : UserControl
    {


        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(TimelineControl), new UIPropertyMetadata(0));


        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(TimelineControl), new UIPropertyMetadata(new PropertyChangedCallback(maximumPropertyChanged)));

        private static void maximumPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            TimelineControl tc = sender as TimelineControl;
            if (tc != null)
            {
                tc.Slider.Maximum = tc.Maximum;
            }
        }

        public int SliderWidth
        {
            get { return (int)GetValue(SliderWidthProperty); }
            set { SetValue(SliderWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SliderWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliderWidthProperty =
            DependencyProperty.Register("SliderWidth", typeof(int), typeof(TimelineControl), new UIPropertyMetadata(new PropertyChangedCallback(sliderWidthPropertyChanged)));

        private static void sliderWidthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            TimelineControl tc = sender as TimelineControl;
            if (tc != null)
            {
                tc.Slider.Width = tc.SliderWidth;
            }
        }

        
        public TimelineControl()
        {
            InitializeComponent();

            this.Slider.ValueChanged += (sender, e) => {
                Slider s = sender as Slider;
                if (s != null)
                    this.Value = (int) s.Value;
            };
        }
    }
}
