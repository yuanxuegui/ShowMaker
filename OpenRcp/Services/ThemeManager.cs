#region License
// Copyright (c) 2013 Yuan Xuegui
#endregion
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Caliburn.Micro;

namespace OpenRcp
{
    /// <summary>
    /// The main theme manager used in OpenRcp
    /// </summary>
    internal sealed class ThemeManager : IThemeManager
    {
        /// <summary>
        /// The theme manager constructor
        /// </summary>
        public ThemeManager()
        {
            Themes = new ObservableCollection<ITheme>();
        }
        
        /// <summary>
        /// The current theme set in the theme manager
        /// </summary>
        public ITheme CurrentTheme { get; internal set; }

        #region IThemeManager Members
        /// <summary>
        /// The collection of themes
        /// </summary>
        [ImportMany]
        public IEnumerable<ITheme> Themes { get; internal set; }

        /// <summary>
        /// Set the current theme
        /// </summary>
        /// <param name="name">The name of the theme</param>
        /// <returns>true if the new theme is set, false otherwise</returns>
        public bool SetCurrent(string name)
        {
            ITheme newTheme = Themes.FirstOrDefault(i => string.Equals(i.Name, name));
            if (newTheme != null)
            {
                CurrentTheme = newTheme;

                ResourceDictionary theme = Application.Current.MainWindow.Resources.MergedDictionaries[0];
                ResourceDictionary appTheme = Application.Current.Resources.MergedDictionaries.Count > 0
                                                  ? Application.Current.Resources.MergedDictionaries[0]
                                                  : null;
                // Unload theme
                theme.MergedDictionaries.Clear();
                if (appTheme != null)
                {
                    appTheme.MergedDictionaries.Clear();
                }
                else
                {
                    appTheme = new ResourceDictionary();
                    Application.Current.Resources.MergedDictionaries.Add(appTheme);
                }
                appTheme.MergedDictionaries.Clear();
                
                // Load the new theme
                foreach (Uri uri in newTheme.UriList)
                {
                    theme.MergedDictionaries.Add(new ResourceDictionary { Source = uri });
                    if (uri.ToString().Contains("AvalonDock"))
                    {
                        appTheme.MergedDictionaries.Add(new ResourceDictionary { Source = uri });
                    }
                }
                /*
                IoC.Get<IOutput>().AppendLine("Theme set to " + name);
                IoC.Get<IEventAggregator>().Publish(new ThemeChangeMessage()
                {
                    NewTheme = newTheme
                });
                */
                return true;
            }
            return false;
        }

        #endregion
    }
}