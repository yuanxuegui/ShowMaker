using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Microsoft.Win32;
using OpenRcp;
using ShowMaker.Desktop.Domain;
using ShowMaker.Desktop.Modules.ExhibitionDocument.ViewModels;
using ShowMaker.Desktop.Modules.ExhibitionDocument.Views;
using ShowMaker.Desktop.Util;
using ShowMaker.Desktop.Models.Util;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace ShowMaker.Desktop.Modules.ExhibitionDocument
{
    [ExportModule]
    public class ExhibitionDocumentModule : ModuleBase
    {
        public const string MENU_FILE_NEW_SHOW = "MENU_FILE_NEW_SHOW";
        public const string MENU_FILE_SAVE_SHOW = "MENU_FILE_SAVE_SHOW";
        public const string MENU_FILE_CLOSE_SHOW = "MENU_FILE_CLOSE_SHOW";

        #region Override ModuleBase Methods

        protected override void RegisterMenus()
        {
            MainMenu[ShellModule.MENU_FILE].Add(
                new MenuItem(MENU_FILE_NEW_SHOW, newShowFile).WithGlobalShortcut(ModifierKeys.Control, Key.N));
            MainMenu[ShellModule.MENU_FILE].Add(
                new MenuItem(MENU_FILE_SAVE_SHOW, saveShowFile).WithGlobalShortcut(ModifierKeys.Control, Key.S));
            MainMenu[ShellModule.MENU_FILE].Add(
                new MenuItem(MENU_FILE_CLOSE_SHOW, closeShowFile).WithGlobalShortcut(ModifierKeys.Control, Key.C));
        }

        #endregion

        private IEnumerable<IResult> newShowFile()
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "展会定义文件(*.show)|*.show";
            dialog.Title = "新建展会定义文件(.show)";
            dialog.ShowDialog();

            // 保存空Exhibition xml内容到新建的文件中
            NewExhibitionView exhDlg = new NewExhibitionView();
            NewExhibitionViewModel exhDlgVM = IoC.Get<NewExhibitionViewModel>();
            ViewModelBinder.Bind(exhDlgVM, exhDlg, null);
            exhDlg.ShowDialog();

            Exhibition contentObject = exhDlgVM.NewExhibition;
            if (contentObject != null)
            {
                string path = dialog.FileName;
                XmlSerializerUtil.SaveXml(contentObject, path);

                ShowFileEncryptDecrypt.SaveShowFile(path);

                yield return ResultsHelper.OpenDocument(dialog.FileName);
            }
            else
                yield break;
        }

        private IEnumerable<IResult> saveShowFile()
        {
            IDocument doc = Shell.ActiveItem;
            ExhibitionEditorViewModel eevm = doc as ExhibitionEditorViewModel;
            if (eevm != null) eevm.SaveHandler(null, null);
            yield break;
        }

        private IEnumerable<IResult> closeShowFile()
        {
            Shell.ActiveItem.TryClose();
            yield break;
        }
    }
}