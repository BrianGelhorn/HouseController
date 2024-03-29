using System.Globalization;

namespace HouseController.Converters
{
	class IntToStatusText : IValueConverter
	{
		private const string On = "ON";
		private const string Off = "OFF";
        private const string Error = "ERROR";
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
            if (value == null || (int)value == -1)
                return Error;
            else
                return (int)value switch
                {
                    0 => Off,
                    1 => On,
                    _ => Error
                };
        }

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return (value != null && (string)value == On) ? 1 : 0;
		}
	}
}
