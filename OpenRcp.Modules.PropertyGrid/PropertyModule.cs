using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;

namespace OpenRcp.Modules.PropertyGrid
{
    [ExportModule]
    public class PropertyModule : ModuleBase
    {
        public const string MENU_VIEW_PROP = "MENU_VIEW_PROP";

        #region Module Info const

        public const string PROP_MODULEINFO_NAME = "PROP_MODULEINFO_NAME";
        public const string PROP_MODULEINFO_HEADER = "PROP_MODULEINFO_HEADER";
        public const string PROP_MODULEINFO_AUTHORS = "PROP_MODULEINFO_AUTHORS";
        public const string PROP_MODULEINFO_DESCRIPTION = "PROP_MODULEINFO_DESCRIPTION";
        public const string PROP_MODULEINFO_COPYRIGHT = "PROP_MODULEINFO_COPYRIGHT";

        #endregion

        #region Override ModuleBase Methods

        protected override void PreInit()
        {
            var propertyTool = IoC.Get<IPropertyGrid>();
            Shell.ShowTool(propertyTool);
            
            IoC.Get<IEventAggregator>().Publish(new ModuleInitMessage
            {
                Content = "Loading Property Module"
            });
        }

        protected override void RegisterMenus()
        {
            MainMenu[ShellModule.MENU_VIEW]
                            .Add(new MenuItem(MENU_VIEW_PROP, OpenPropertiesTool).WithIcon(@"\Resources\Icons\properties.png"));
        }

        protected override ModuleInfoItem GetModuleInfo()
        {
            IResourceService rs = IoC.Get<IResourceService>();
            var item = new ModuleInfoItem
            {
                Name = rs.GetString(PROP_MODULEINFO_NAME),
                HeaderInfo = rs.GetString(PROP_MODULEINFO_HEADER),
                Authors = rs.GetString(PROP_MODULEINFO_AUTHORS),
                Description = rs.GetString(PROP_MODULEINFO_DESCRIPTION),
                Version = string.Format(rs.GetString(ModuleBase.MODULEINFO_VERSION), Assembly.GetExecutingAssembly().GetName().Version, IntPtr.Size * 8),
                CopyrightNotice = rs.GetString(PROP_MODULEINFO_COPYRIGHT),
                Rights = rs.GetString(ModuleBase.MODULEINFO_RIGHTS)
            };
            return item;
        }

        #endregion

        private static IEnumerable<IResult> OpenPropertiesTool()
        {
            yield return ResultsHelper.ShowTool<IPropertyGrid>();
        }
    }
}