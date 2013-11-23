using System;
using System.Collections.Generic;
using Caliburn.Micro;

namespace OpenRcp
{
   
    public class RadioMenuItem : StandardMenuItem, ICheckable
	{
		private readonly Func<bool, IEnumerable<IResult>> _execute;

        public IList<RadioMenuItem> Group { get; private set; }

        private bool _isChecked;
		public bool IsChecked
		{
			get { return _isChecked; }
			set { _isChecked = value; NotifyOfPropertyChange(() => IsChecked); }
		}

		#region Constructors

        public RadioMenuItem(string name)
            : base(name)
        {

        }

        public RadioMenuItem(string name, IList<RadioMenuItem> group)
            : base(name)
		{
            Group = group;
            Group.Add(this);
		}

        public RadioMenuItem(string name, IList<RadioMenuItem> group, Func<bool, IEnumerable<IResult>> execute)
            : this(name, group)
		{
			_execute = execute;
		}

        public RadioMenuItem(string name, IList<RadioMenuItem> group, Func<bool, IEnumerable<IResult>> execute, Func<bool> canExecute)
            : this(name, group, execute)
        {
            _execute = execute;
        }

		#endregion

        public RadioMenuItem Checked()
        {
            this.IsChecked = true;
            return this;
        }

        public RadioMenuItem UnChecked()
        {
            this.IsChecked = false;
            return this;
        }

		public IEnumerable<IResult> Execute()
		{
            if (IsChecked)
            {
                // unCheck group other
                foreach (RadioMenuItem rd in Group)
                {
                    if(!rd.Name.Equals(this.Name))
                        rd.IsChecked = false;
                }
            }
            return _execute != null ? _execute(IsChecked) : new IResult[] { };
		}
	}
}