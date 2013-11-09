#region License
// Copyright (c) 2013 Yuan Xuegui
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace OpenRcp
{
    /// <summary>
    /// Class VS2010
    /// </summary>
    [Export(typeof(ITheme))]    
    public sealed class VS2010 : ITheme
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VS2010"/> class.
        /// </summary>
        public VS2010()
        {
            UriList = new List<Uri>
                          {
                              new Uri("pack://application:,,,/AvalonDock.Themes.VS2010;component/Theme.xaml"),
                          };
        }

        #region ITheme Members
        /// <summary>
        /// Lists of valid URIs which will be loaded in the theme dictionary
        /// </summary>
        /// <value>The URI list.</value>
        public IList<Uri> UriList { get; private set; }

        /// <summary>
        /// The name of the theme - "VS2010"
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return "VS2010"; }
        }
        #endregion
    }
}