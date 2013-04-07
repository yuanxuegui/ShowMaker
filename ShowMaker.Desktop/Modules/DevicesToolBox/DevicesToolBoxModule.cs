using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using OpenRcp;
using ShowMaker.Desktop.Modules.DevicesToolBox.ViewModels;

namespace ShowMaker.Desktop.Modules.DevicesToolBox
{
    /// <summary>
    /// 设备工具箱模块
    /// </summary>
    [ExportModule]
    public class DevicesToolBoxModule : ModuleBase
    {
        public const string MENU_VIEW_DEVTOOLBOX = "MENU_VIEW_DEVTOOLBOX";
        
        protected override void RegisterMenus()
        {
            MainMenu[ShellModule.MENU_VIEW].Add(new CheckableMenuItem(MENU_VIEW_DEVTOOLBOX, openDevicesToolBox));
        }
        
        private IEnumerable<IResult> openDevicesToolBox(bool isChecked)
        {
            if (isChecked)
            {
                yield return ResultsHelper.ShowTool<DevicesToolBoxViewModel>();
            }
            else
            {
                yield return ResultsHelper.HideTool<DevicesToolBoxViewModel>();
            }
        }
    }
}
