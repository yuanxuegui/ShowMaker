﻿using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Caliburn.Micro;

namespace OpenRcp.Modules.Output
{
	[Export(typeof(IOutput))]
	public class OutputViewModel : Tool, IOutput
	{
		private readonly StringBuilder _stringBuilder;
		private readonly OutputWriter _writer;
		private IOutputView _view;
		private event EventHandler TextChanged;

        #region Override Tool Methods

        public override string Name
		{
			get { return OutputModule.MENU_VIEW_OUTPUT; }
		}

		public override PaneLocation PreferredLocation
		{
			get { return PaneLocation.Bottom; }
		}

        protected override void OnViewLoaded(object view)
        {
            _view = (IOutputView)view;
            _view.SetText(_stringBuilder.ToString());
            _view.ScrollToEnd();
        }

        #endregion

        #region IOutput Implementation
        public TextWriter Writer
		{
			get { return _writer; }
		}

		public OutputViewModel()
		{
			_stringBuilder = new StringBuilder();
			_writer = new OutputWriter(this);

			Observable.FromEventPattern<EventHandler, EventArgs>(h => TextChanged += h, h => TextChanged -= h)
				.Throttle(TimeSpan.FromSeconds(1))
				.Subscribe(_ =>
				{
					if (_view != null)
						Execute.OnUIThread(() => _view.SetText(_stringBuilder.ToString()));
				});
		}

		public void Clear()
		{
			if (_view != null)
				Execute.OnUIThread(() => _view.Clear());
			_stringBuilder.Clear();
		}

		public void AppendLine(string text)
		{
			Append(text + Environment.NewLine);
		}

		public void Append(string text)
		{
			_stringBuilder.Append(text);
			OnTextChanged(EventArgs.Empty);
		}

        #endregion

        private void OnTextChanged(EventArgs e)
		{
			EventHandler handler = TextChanged;
			if (handler != null) handler(this, e);
		}


	}

	internal class TextAppendedEventArgs : EventArgs
	{
		public string Text { get; set; }
	}
}