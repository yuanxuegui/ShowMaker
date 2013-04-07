using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Microsoft.Win32;
using System.Windows.Input;

namespace OpenRcp
{
	[Export(typeof(IMenu))]
	public class MainMenuViewModel : MenuModel
	{
		public MainMenuViewModel()
		{   
		}
	}
}