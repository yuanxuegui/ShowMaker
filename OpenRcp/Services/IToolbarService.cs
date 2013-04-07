#region License
// Copyright (c) 2013 Yuan Xuegui
#endregion
using System.Windows.Controls;

namespace OpenRcp
{
    /// <summary>
    /// Interface IToolbarService - the application's toolbar tray is returned by this service
    /// </summary>
    public interface IToolbarService
    {
        /// <summary>
        /// Gets the tool bar tray of the application.
        /// </summary>
        /// <value>The tool bar tray.</value>
        ToolBarTray ToolBarTray { get; }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if successfully added, <c>false</c> otherwise</returns>
        //bool Add(ToolbarViewModel item);

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if successfully removed, <c>false</c> otherwise</returns>
        bool Remove(string key);

        /// <summary>
        /// Gets the specified toolbar using the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ToolbarViewModel.</returns>
        //ToolbarViewModel Get(string key);
    }
}