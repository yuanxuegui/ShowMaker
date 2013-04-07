using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace OpenRcp
{
    [Export(typeof(ISplash))]
    public class SplashViewModel : PropertyChangedBase, ISplash, IHandle<ModuleInitMessage>
    {
        #region Declarations

        private string appName;
        private string status;

        #endregion

        #region Constructor

        public SplashViewModel()
        {
            this.appName = "OpenRcp";
            IEventAggregator eventAggregator = IoC.Get<IEventAggregator>();
            eventAggregator.Subscribe(this);
        }

        #endregion

        #region Public Properties

        public string AppName
        {
            get { return appName; }
            set
            {
                if (appName != value)
                {
                    appName = value;
                    NotifyOfPropertyChange(() => AppName);
                }

            }
        }
        

        public string Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    NotifyOfPropertyChange(() => Status);
                }
            }
        }
        #endregion

        #region Private Methods

        private void updateMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            Status += string.Concat(Environment.NewLine, message, "...");
        }

        #endregion

        public void Handle(ModuleInitMessage message)
        {
            updateMessage(message.Content);
        }
    }
}
