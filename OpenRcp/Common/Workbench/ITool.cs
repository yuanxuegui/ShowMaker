using System.Windows.Input;
using Caliburn.Micro;

namespace OpenRcp
{
	public interface ITool : IScreen
	{
		ICommand CloseCommand { get; }
		PaneLocation PreferredLocation { get; }
		bool IsVisible { get; set; }
	}
}