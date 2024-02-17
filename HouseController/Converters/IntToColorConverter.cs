using System.Globalization;

namespace HouseController.Converters
{
	public class IntToColorConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			var onColor = new Color(0, 255, 0);
			var offColor = new Color(255, 0, 0);
			return (int?)value == 1? onColor: offColor;
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			var greenColor = new Color(0, 255, 0);
			return ((Color?)value == greenColor) ? 1 : 0;
		}
	}
}
