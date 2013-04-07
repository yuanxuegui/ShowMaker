#region License
// Copyright (c) 2013 Yuan Xuegui
#endregion
using System.Collections.Generic;

namespace OpenRcp
{
    /// <summary>
    /// Interface IThemeManager
    /// </summary>
    public interface IThemeManager
    {
        /// <summary>
        /// The list of themes registered with the theme manager
        /// </summary>
        /// <value>The themes.</value>
        IEnumerable<ITheme> Themes { get; }

        /// <summary>
        /// Called to set the current theme from the list of themes
        /// </summary>
        /// <param name="name">The name of the theme</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise</returns>
        bool SetCurrent(string name);

        /// <summary>
        /// Returns the current theme set in the theme manager
        /// </summary>
        /// <value>The current theme.</value>
        ITheme CurrentTheme { get; }
    }
}