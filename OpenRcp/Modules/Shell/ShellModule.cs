using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Input;
using Caliburn.Micro;
using Microsoft.Win32;

namespace OpenRcp
{
    [ExportModule]
    public class ShellModule : ModuleBase
    {
        public const string MENU_FILE = "MENU_FILE";
        public const string MENU_EDIT = "MENU_EDIT";
        public const string MENU_VIEW = "MENU_VIEW";
        public const string MENU_TOOL = "MENU_TOOL";
        public const string MENU_HELP = "MENU_HELP";

        public const string MENU_FILE_OPEN = "MENU_FILE_OPEN";
        public const string MENU_FILE_EXIT = "MENU_FILE_EXIT";

        #region Module Info const

        public const string SHELL_MODULEINFO_NAME = "SHELL_MODULEINFO_NAME";
        public const string SHELL_MODULEINFO_HEADER = "SHELL_MODULEINFO_HEADER";
        public const string SHELL_MODULEINFO_AUTHORS = "SHELL_MODULEINFO_AUTHORS";
        public const string SHELL_MODULEINFO_DESCRIPTION = "SHELL_MODULEINFO_DESCRIPTION";
        public const string SHELL_MODULEINFO_COPYRIGHT = "SHELL_MODULEINFO_COPYRIGHT";
        public const string SHELL_MODULEINFO_RIGHTS = "SHELL_MODULEINFO_RIGHTS";

        #endregion

        #region Override ModuleBase Methods

        protected override void PreInit()
        {
            PublishMessage(new ModuleInitMessage
            {
                Content = "Loading Shell Module"
            });
        }

        protected override void RegisterMenus()
        {
            MainMenu.Add(
                new MenuItem(ShellModule.MENU_FILE)
                ,
                new MenuItem(ShellModule.MENU_EDIT)
                ,
                new MenuItem(ShellModule.MENU_VIEW)
                ,
                new MenuItem(ShellModule.MENU_TOOL)
                ,
                new MenuItem(ShellModule.MENU_HELP)
            );

            MainMenu[MENU_FILE].Add(
                new MenuItem(MENU_FILE_OPEN, openFile)
                .WithIcon(@"Modules\Shell\Resources\Icons\Open.png")
                .WithGlobalShortcut(ModifierKeys.Control, Key.O)).Add(MenuItemBase.Separator).Add(new MenuItem(MENU_FILE_EXIT, exit));
        }

        protected override ModuleInfoItem GetModuleInfo()
        {
            IResourceService rs = IoC.Get<IResourceService>();
            var item = new ModuleInfoItem
            {
                Name = rs.GetString(SHELL_MODULEINFO_NAME),
                HeaderInfo = rs.GetString(SHELL_MODULEINFO_HEADER),
                Authors = rs.GetString(SHELL_MODULEINFO_AUTHORS),
                Description = rs.GetString(SHELL_MODULEINFO_DESCRIPTION),
                Version = string.Format(rs.GetString(ModuleBase.MODULEINFO_VERSION), Assembly.GetExecutingAssembly().GetName().Version, IntPtr.Size * 8),
                CopyrightNotice = rs.GetString(SHELL_MODULEINFO_COPYRIGHT),
                Rights = rs.GetString(SHELL_MODULEINFO_RIGHTS)
            };
            return item;
        }

        #endregion

        private IEnumerable<IResult> openFile()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "所有文件|*.*|文本文件(*.txt)|*.txt|XML文件(*.xml)|*.xml";
            yield return ResultsHelper.ShowDialog(dialog);
            yield return ResultsHelper.OpenDocument(dialog.FileName);
        }

        private IEnumerable<IResult> exit()
        {
            Shell.Close();
            yield break;
        }

    }
}