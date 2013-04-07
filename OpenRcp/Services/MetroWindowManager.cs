using System;
using System.Windows;
using Caliburn.Micro;
using MahApps.Metro.Controls;

namespace OpenRcp
{
    public class MetroWindowManager : WindowManager
    {
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            MetroWindow window = null;
            Window inferOwnerOf;
            if (view is MetroWindow)
            {
                window = CreateCustomWindow(view, true);
                inferOwnerOf = InferOwnerOf(window);
                if (inferOwnerOf != null && isDialog)
                {
                    window.Owner = inferOwnerOf;
                }
            }
            if (window == null)
            {
                window = CreateCustomWindow(view, false);
            }
            ConfigureWindow(window);
            window.SetValue(View.IsGeneratedProperty, true);
            inferOwnerOf = InferOwnerOf(window);
            if (inferOwnerOf != null)
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                window.Owner = inferOwnerOf;
            }
            else 
			{ 
				window.WindowStartupLocation = WindowStartupLocation.CenterScreen; 
			} 
			return window;
        }
        public virtual void ConfigureWindow(MetroWindow window) { }
        public virtual MetroWindow CreateCustomWindow(object view, bool windowIsView)
        {
            MetroWindow result;
            if (windowIsView)
            {
                result = view as MetroWindow;
            }
            else
            {
                result = new MetroWindow { Content = view };
            }
            return result;
        }
    }
}
