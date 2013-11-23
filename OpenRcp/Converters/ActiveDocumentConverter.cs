using System;
using System.Globalization;
using System.Windows.Data;

namespace OpenRcp
{
	public class ActiveDocumentConverter : IValueConverter
	{
		#region IValueConverter Members
		
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is IDocument)
				return value;

			return Binding.DoNothing;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is IDocument)
				return value;

			return Binding.DoNothing;
		}

		#endregion
	}
}