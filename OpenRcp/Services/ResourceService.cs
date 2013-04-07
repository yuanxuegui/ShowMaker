using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using Caliburn.Micro;

namespace OpenRcp
{
	/// <summary>
	/// Resource service class implements IResourceService interface
	/// </summary>
    [Export(typeof(IResourceService))]
	public class ResourceService : IResourceService
	{
        [ImportMany]
        public IResource[] Resources { get; set; }

        public ResourceService()
        {
        }
        
        /// <summary>
        /// Get resource stream
        /// </summary>
        /// <param name="relativeUri"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public Stream GetStream(string relativeUri, string assemblyName)
		{
			try
			{
				var resource = Application.GetResourceStream(new Uri(assemblyName + ";component/" + relativeUri, UriKind.Relative))
					?? Application.GetResourceStream(new Uri(relativeUri, UriKind.Relative));

				return (resource != null)
					? resource.Stream
					: null;
			}
			catch
			{
				return null;
			}
		}

        /// <summary>
        /// Get bitmap
        /// </summary>
        /// <param name="relativeUri"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
		public BitmapImage GetBitmap(string relativeUri, string assemblyName)
		{
			var s = GetStream(relativeUri, assemblyName);
			if (s == null) return null;

			using (s)
			{
				var bmp = new BitmapImage();
				bmp.BeginInit();
				bmp.StreamSource = s;
				bmp.EndInit();
				bmp.Freeze();
				return bmp;
			}
		}

        /// <summary>
        /// Get bitmap
        /// </summary>
        /// <param name="relativeUri"></param>
        /// <returns></returns>
		public BitmapImage GetBitmap(string relativeUri)
		{
			return GetBitmap(relativeUri, ExtensionMethods.GetExecutingAssemblyName());
		}

        /// <summary>
        /// Change language
        /// </summary>
        /// <param name="language"></param>
        public void ChangeLanguage(string language)
        {
            CultureInfo culture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            Resources.Apply(item => item.CurrentCulture = culture);

            IEventAggregator eventAggregator = IoC.Get<IEventAggregator>();
            eventAggregator.Publish(new LanguageChangedMessage());

            if (LanguageChanged != null)
            {
                LanguageChanged(this, null);
            }
        }

        /// <summary>
        /// Get string value by name from resource
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetString(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            string str = null;

            foreach (var resource in Resources)
            {
                str = resource.GetString(name);
                if (str != null)
                {
                    break;
                }
            }
            return str;
        }

        /// <summary>
        /// Language changed event
        /// </summary>
        public event EventHandler LanguageChanged;
    }
}