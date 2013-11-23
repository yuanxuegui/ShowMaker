using System.Windows.Input;
using Caliburn.Micro;

namespace OpenRcp
{
	public interface IDocument : IScreen
	{
		ICommand CloseCommand { get; }
	}
}