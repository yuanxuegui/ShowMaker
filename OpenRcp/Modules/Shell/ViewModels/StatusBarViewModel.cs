using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace OpenRcp
{
	[Export(typeof(IStatusBar))]
	public class StatusBarViewModel : PropertyChangedBase, IStatusBar
	{
        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }
        
        private string _message;
		public string Message
		{
			get { return _message; }
			set
			{
				_message = value;
				NotifyOfPropertyChange(() => Message);
			}
		}
	}
}