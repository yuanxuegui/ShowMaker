using System;
using Caliburn.Micro;

namespace OpenRcp
{
	public abstract class CloseResultBase<TTarget> : ICloseResult<TTarget>
	{
		protected Action<TTarget> _setData;
		protected Action<TTarget> _onConfigure;
		protected Action<TTarget> _onShutDown;

		Action<TTarget> ICloseResult<TTarget>.OnConfigure
		{
			get { return _onConfigure; }
			set { _onConfigure = value; }
		}

		Action<TTarget> ICloseResult<TTarget>.OnShutDown
		{
			get { return _onShutDown; }
			set { _onShutDown = value; }
		}

		protected virtual void OnCompleted(Exception exception)
		{
			if (Completed != null)
				Completed(this, new ResultCompletionEventArgs { Error = exception });
		}

		public abstract void Execute(ActionExecutionContext context);

		public event EventHandler<ResultCompletionEventArgs> Completed;
	}
}