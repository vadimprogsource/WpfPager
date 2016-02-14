using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace System.Windows.Mvc
{
	public class BoolToVisibility : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, Globalization.CultureInfo culture)
		{
			return true.Equals(value) ? Visibility.Visible : Visibility.Hidden;  
		}

		public object ConvertBack(object value, Type targetType, object parameter, Globalization.CultureInfo culture)
		{
			return Visibility.Visible.Equals(value); 
		}
	}
}
