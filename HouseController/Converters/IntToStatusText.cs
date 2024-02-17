using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseController.Converters
{
	class IntToStatusText : IValueConverter
	{
		private const string On = "ON";
		private const string Off = "OFF";
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return value != null && (int)value == 1 ? On : Off;
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return (value != null && (string)value == On) ? 1 : 0;
		}
	}
}
