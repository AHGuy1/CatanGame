using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace CatanGame.Converters
{
	public class MultiObjectConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			// Return the bound values as an object[] so the CommandParameter receives them
			return values;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			// Not needed for CommandParameter use-case
			return [];
		}
	}
}