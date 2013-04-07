using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace OpenRcp
{
    public class ChangeThemeResult : IResult
    {
        private string themeName;

        public string ThemeName
        {
            get { return themeName; }
            set { themeName = value; }
        }

        public ChangeThemeResult(string themeName)
        {
            this.themeName = themeName;
        }

        public event EventHandler<ResultCompletionEventArgs> Completed;

        protected virtual void OnCompleted(Exception exception)
        {
            if (Completed != null)
                Completed(this, new ResultCompletionEventArgs { Error = exception });
        }

        public void Execute(ActionExecutionContext context)
        {
            if (themeName == null)
            {
                OnCompleted(null);
                return;
            }
            else
            {
                IThemeManager themeMan = IoC.Get<IThemeManager>();
                if (themeMan != null)
                    themeMan.SetCurrent(themeName);
                OnCompleted(null);
            }
        }
    }
}
