using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;

namespace OpenRcp
{
    [ExportModule]
    public class ThemeModule : ModuleBase
    {
        public const string MENU_TOOL_THEME = "MENU_TOOL_THEME";

        #region Module Info const

        public const string THEME_MODULEINFO_NAME = "THEME_MODULEINFO_NAME";
        public const string THEME_MODULEINFO_HEADER = "THEME_MODULEINFO_HEADER";
        public const string THEME_MODULEINFO_AUTHORS = "THEME_MODULEINFO_AUTHORS";
        public const string THEME_MODULEINFO_DESCRIPTION = "THEME_MODULEINFO_DESCRIPTION";
        public const string THEME_MODULEINFO_COPYRIGHT = "THEME_MODULEINFO_COPYRIGHT";

        #endregion

        #region Override ModuleBase Methods

        protected override void PreInit()
        {
            PublishMessage(new ModuleInitMessage
            {
                Content = "Loading Theme Module"
            });
        }

        protected override void RegisterMenus()
        {
            IList<RadioMenuItem> themeGroup = new List<RadioMenuItem>();
            RadioMenuItem vs2010 = new RadioMenuItem("VS2010", themeGroup, changeThemeToVS2010);
            vs2010.IsChecked = true;
            MainMenu[ShellModule.MENU_TOOL].Add(new MenuItem(MENU_TOOL_THEME){
                new RadioMenuItem("Dark", themeGroup, changeThemeToDark),
                new RadioMenuItem("Light", themeGroup, changeThemeToLight),
                vs2010
            });
        }

        protected override ModuleInfoItem GetModuleInfo()
        {
            IResourceService rs = IoC.Get<IResourceService>();
            var item = new ModuleInfoItem
            {
                Name = rs.GetString(THEME_MODULEINFO_NAME),
                HeaderInfo = rs.GetString(THEME_MODULEINFO_HEADER),
                Authors = rs.GetString(THEME_MODULEINFO_AUTHORS),
                Description = rs.GetString(THEME_MODULEINFO_DESCRIPTION),
                Version = string.Format(rs.GetString(ModuleBase.MODULEINFO_VERSION), Assembly.GetExecutingAssembly().GetName().Version, IntPtr.Size * 8),
                CopyrightNotice = rs.GetString(THEME_MODULEINFO_COPYRIGHT),
                Rights = rs.GetString(ModuleBase.MODULEINFO_RIGHTS)
            };
            return item;
        }

        #endregion

        private IEnumerable<IResult> changeThemeToDark(bool isChecked)
        {
            if (isChecked)
            {
                yield return ResultsHelper.ChangeTheme("Default");
                MahApps.Metro.ThemeManager.ChangeTheme(Application.Current.MainWindow, MahApps.Metro.ThemeManager.DefaultAccents.First(a => a.Name == "Blue"), MahApps.Metro.Theme.Dark);
                yield break;
            }
            else
            {
                yield return ResultsHelper.ChangeTheme("Default");
            }
        }

        private IEnumerable<IResult> changeThemeToLight(bool isChecked)
        {
            if (isChecked)
            {
                yield return ResultsHelper.ChangeTheme("Default");
                MahApps.Metro.ThemeManager.ChangeTheme(Application.Current.MainWindow, MahApps.Metro.ThemeManager.DefaultAccents.First(a => a.Name == "Blue"), MahApps.Metro.Theme.Light);
                yield break;
            }
            else
            {
                yield return ResultsHelper.ChangeTheme("Default");
            }
        }

        private IEnumerable<IResult> changeThemeToVS2010(bool isChecked)
        {
            if (isChecked)
            {
                yield return ResultsHelper.ChangeTheme("VS2010");
            }
            else
            {
                yield return ResultsHelper.ChangeTheme("Default");
            }
        }
    }
}