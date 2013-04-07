using System.ComponentModel.Composition;
using Caliburn.Micro;
namespace OpenRcp
{
    public abstract class ModuleBase : IModule
    {
        public const string MODULEINFO_RIGHTS = "MODULEINFO_RIGHTS";
        public const string MODULEINFO_VERSION = "MODULEINFO_VERSION";

        [Import]
        private IShell _shell = null;

        protected IShell Shell
        {
            get { return _shell; }
        }

        protected IMenu MainMenu
        {
            get { return _shell.MainMenu; }
        }
        [Import]
        private IEventAggregator _eventAggregator = null;

        protected void SubcribeMessage(object instance)
        {
            _eventAggregator.Subscribe(instance);
        }

        protected void PublishMessage(object message)
        {
            _eventAggregator.Publish(message);
        }

        protected virtual ModuleInfoItem GetModuleInfo()
        {
            return null;
        }

        protected virtual bool IsLoadModule()
        {
            return true;
        }

        protected virtual void PreInit()
        {

        }

        protected virtual void RegisterCommands()
        {
        }

        protected virtual void RegisterMenus()
        {
        }

        protected virtual void RegisterToolbar()
        {
        }

        protected virtual void SetupModuleInfo()
        {
            ModuleInfoItem infoItem = GetModuleInfo();
            if (infoItem != null)
            {
                //TODO. 绑定信息到模块列表模板
            }
        }

        protected virtual void PostInit()
        {

        }

        #region IModule Implementation

        void IModule.Initialize()
        {
            if (IsLoadModule())
            {
                PreInit();
                RegisterCommands();
                RegisterMenus();
                RegisterToolbar();
                SetupModuleInfo();
                PostInit();
            }
        }

        #endregion
    }
}