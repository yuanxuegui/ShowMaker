using System.Windows.Controls;

namespace OpenRcp
{
	/// <summary>
	/// Interaction logic for PropertyGridView.xaml
	/// </summary>
	public partial class PropertyGridView : UserControl
	{
		public PropertyGridView()
		{
			// The following line simply forces Visual Studio to copy the
			// WPF Toolkit DLL to the output folder.
 			_propertyGrid = null;

			InitializeComponent();
		}
	}
}
