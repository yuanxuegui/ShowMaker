using System;
using System.Collections.Generic;
using Caliburn.Micro;

namespace OpenRcp
{
    public interface ICheckable
    {
        bool IsChecked { get; set; }
    }
    
    public class CheckableMenuItem : StandardMenuItem, ICheckable
	{
		private readonly Func<bool, IEnumerable<IResult>> _execute;

		private bool _isChecked;
		public bool IsChecked
		{
			get { return _isChecked; }
			set { _isChecked = value; NotifyOfPropertyChange(() => IsChecked); }
		}

		#region Constructors

		public CheckableMenuItem(string name)
            : base(name)
		{
			
		}

        public CheckableMenuItem(string name, Func<bool, IEnumerable<IResult>> execute)
            : base(name)
		{
			_execute = execute;
		}

        public CheckableMenuItem(string name, Func<bool, IEnumerable<IResult>> execute, Func<bool> canExecute)
            : base(name, canExecute)
		{
			_execute = execute;
		}

		#endregion

        public CheckableMenuItem Checked()
        {
            this.IsChecked = true;
            return this;
        }

        public CheckableMenuItem UnChecked()
        {
            this.IsChecked = false;
            return this;
        }

		public IEnumerable<IResult> Execute()
		{
			return _execute != null ? _execute(IsChecked) : new IResult[] { };
		}
	}
}