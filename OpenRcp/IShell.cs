using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;

namespace OpenRcp
{
	public interface IShell
	{
		WindowState WindowState { get; set; }
		string Title { get; set; }
		ImageSource Icon { get; set; }
		IMenu MainMenu { get; }
		IStatusBar StatusBar { get; }

        IDocument ActiveItem { get; }

        IObservableCollection<IDocument> Documents { get; }
        IObservableCollection<ITool> Tools { get; }

		void ShowTool(ITool model);
        void HideTool(ITool model);

		void OpenDocument(IDocument model);
		void CloseDocument(IDocument document);
		void ActivateDocument(IDocument document);

		void Close();
	}
}