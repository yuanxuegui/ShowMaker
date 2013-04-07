using System;
using System.Collections.Generic;
using Caliburn.Micro;

namespace OpenRcp
{
	public class MenuItem : StandardMenuItem
	{
		private readonly Func<IEnumerable<IResult>> _execute;

		#region Constructors

		public MenuItem(string name)
            : base(name)
		{
			
		}

        public MenuItem(string name, Func<IEnumerable<IResult>> execute)
            : base(name)
		{
			_execute = execute;
		}

        public MenuItem(string name, Func<IEnumerable<IResult>> execute, Func<bool> canExecute)
            : base(name, canExecute)
		{
			_execute = execute;
		}

		#endregion

		public IEnumerable<IResult> Execute()
		{
			return _execute != null ? _execute() : new IResult[] { };
		}
	}
}