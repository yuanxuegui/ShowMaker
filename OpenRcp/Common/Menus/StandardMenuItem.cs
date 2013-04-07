using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;

namespace OpenRcp
{
	public class StandardMenuItem : MenuItemBase
	{
		private readonly Func<bool> _canExecute = () => true;
		private KeyGesture _keyGesture;

		public Image Icon { get; private set; }

		public string ActionText
		{
			get { return "Execute"; }
		}

		public bool CanExecute
		{
			get { return _canExecute(); }
		}

		public string InputGestureText
		{
			get
			{
				return _keyGesture == null
					? string.Empty
					: _keyGesture.GetDisplayStringForCulture(CultureInfo.CurrentUICulture);
			}
		}

		public StandardMenuItem(string name) : base(name)
		{
		}

        public StandardMenuItem(string name, Func<bool> canExecute)
            : this(name)
		{
			_canExecute = canExecute;
		}

		#region Fluent interface

		public StandardMenuItem WithGlobalShortcut(ModifierKeys modifier, Key key)
		{
			_keyGesture = new KeyGesture(key, modifier);
			IoC.Get<IInputManager>().SetShortcut(_keyGesture, this);
			return this;
		}

		public StandardMenuItem WithIcon()
		{
			return WithIcon(Assembly.GetCallingAssembly(), "Resources/Icons/" + Name.Replace(" ", string.Empty) + ".png");
		}

		public StandardMenuItem WithIcon(string path)
		{
			return WithIcon(Assembly.GetCallingAssembly(), path);
		}

		public StandardMenuItem WithIcon(Assembly source, string path)
		{
			var manager = IoC.Get<IResourceService>();
			var iconSource = manager.GetBitmap(path, source.GetAssemblyName());

			if (source != null)
				Icon = new Image
				{
					Source = iconSource,
					Width = 16,
					Height = 16
				};

			return this;
		}

		#endregion
	}
}