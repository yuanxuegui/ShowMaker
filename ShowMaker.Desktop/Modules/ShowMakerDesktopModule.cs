using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using Caliburn.Micro;
using OpenRcp;

namespace ShowMaker.Desktop.Modules
{
    [ExportModule]
    public class ShowMakerDesktopModule : ModuleBase
    {
        public const string MENU_TOOL_LANGUAGE = "MENU_TOOL_LANGUAGE";
        public const string MENU_TOOL_LANGUAGE_ZHCN = "MENU_TOOL_LANGUAGE_ZHCN";
        public const string MENU_TOOL_LANGUAGE_EN = "MENU_TOOL_LANGUAGE_EN";
        public const string MENU_HELP_ABOUT = "MENU_HELP_ABOUT";

        public const string SHOW_FILE_EXTENSION = ".show";

        [Import]
        private IOutput _output = null;

        [Import]
        private IResourceService _resourceManager = null;

        protected override void PreInit()
        {
            Shell.WindowState = WindowState.Maximized;
            Shell.Title = "ShowMaker";
            Shell.StatusBar.Status = "就绪";
            Shell.StatusBar.Message = "欢迎使用ShowMaker";
            Shell.Icon = _resourceManager.GetBitmap("Resources/Icon.png",
                Assembly.GetExecutingAssembly().GetAssemblyName());

            _output.AppendLine("Loading ShowMaker Module");

            IThemeManager themeMan = IoC.Get<IThemeManager>();
            if (themeMan != null)
                themeMan.SetCurrent("VS2010");
        }

        protected override void RegisterMenus()
        {
            IList<RadioMenuItem> langGroup = new List<RadioMenuItem>();
            MainMenu[ShellModule.MENU_TOOL].Add(new MenuItem(MENU_TOOL_LANGUAGE)
            {
                new RadioMenuItem(MENU_TOOL_LANGUAGE_ZHCN, langGroup, changeLangToChinese),
                new RadioMenuItem(MENU_TOOL_LANGUAGE_EN, langGroup, changeLangToEnglish),
            });
            MainMenu[ShellModule.MENU_HELP].Add(new MenuItem(MENU_HELP_ABOUT));
        }

        private IEnumerable<IResult> changeLangToChinese(bool isChecked)
        {
            if (isChecked)
            {
                IoC.Get<IResourceService>().ChangeLanguage("zh-cn");
                yield break;
            }
            else
            {
                IoC.Get<IResourceService>().ChangeLanguage("en-us");
                yield break;
            }
        }

        private IEnumerable<IResult> changeLangToEnglish(bool isChecked)
        {
            if (isChecked)
            {
                IoC.Get<IResourceService>().ChangeLanguage("en-us");
                yield break;
            }
            else
            {
                IoC.Get<IResourceService>().ChangeLanguage("zh-cn");
                yield break;
            }
        }
    }
}
