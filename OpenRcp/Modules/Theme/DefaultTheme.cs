#region License
// Copyright (c) 2013 Yuan Xuegui
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace OpenRcp
{
    /// <summary>
    /// Class DefaultTheme
    /// </summary>
    [Export(typeof(ITheme))]
    public sealed class DefaultTheme : ITheme
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTheme"/> class.
        /// </summary>
        public DefaultTheme()
        {
            UriList = new List<Uri>
                          {
                              new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Colours.xaml"),
                              new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml"),
                              new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml"),
                              new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml"),
                              new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml")
                          };
        }

        #region ITheme Members
        /// <summary>
        /// Lists of valid URIs which will be loaded in the theme dictionary
        /// </summary>
        /// <value>The URI list.</value>
        public IList<Uri> UriList { get; internal set; }

        /// <summary>
        /// The name of the theme - "Default"
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return "Default"; }
        }
        #endregion
    }
}