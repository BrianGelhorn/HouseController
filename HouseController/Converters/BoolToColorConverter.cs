using System.Globalization;

namespace HouseController.Converters
{
	public class BoolToColorConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			var onColor = new Color(0, 255, 0);
			var offColor = new Color(255, 0, 0);
			return (bool?)value == true? onColor: offColor;
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			var greenColor = new Color(0, 255, 0);
			return (Color?)value == greenColor;
		}
	}
}
