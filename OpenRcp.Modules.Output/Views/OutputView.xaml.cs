using System.Windows.Controls;

namespace OpenRcp.Modules.Output
{
	/// <summary>
	/// Interaction logic for OutputView.xaml
	/// </summary>
	public partial class OutputView : UserControl, IOutputView
	{
		public OutputView()
		{
			InitializeComponent();
		}

		public void ScrollToEnd()
		{
			outputText.ScrollToEnd();
		}

		public void Clear()
		{
			outputText.Clear();
		}

		public void AppendText(string text)
		{
			outputText.AppendText(text);
			ScrollToEnd();
		}

		public void SetText(string text)
		{
			outputText.Text = text;
			ScrollToEnd();
		}
	}
}
