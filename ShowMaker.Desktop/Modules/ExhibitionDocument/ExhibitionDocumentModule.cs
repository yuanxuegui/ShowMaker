using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Microsoft.Win32;
using OpenRcp;
using ShowMaker.Desktop.Domain;
using ShowMaker.Desktop.Util;

namespace ShowMaker.Desktop.Modules.ExhibitionDocument
{
    [ExportModule]
    public class ExhibitionDocumentModule : ModuleBase
    {
        public const string MENU_FILE_NEW = "MENU_FILE_NEW";

        #region Override ModuleBase Methods

        protected override void RegisterMenus()
        {
            MainMenu[ShellModule.MENU_FILE].Add(
                new MenuItem(MENU_FILE_NEW, newFile));
        }

        #endregion

        private IEnumerable<IResult> newFile()
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "所有文件|*.*|文本文件(*.txt)|*.txt|XML文件(*.xml)|*.xml";
            dialog.Title = "新建文件";
            dialog.ShowDialog();

            // 保存空Exhibition xml内容到新建的文件中
            Exhibition contentObject = new Exhibition();
            XmlSerializerUtil.SaveXml(contentObject, dialog.FileName);

            yield return ResultsHelper.OpenDocument(dialog.FileName);
        }
    }
}