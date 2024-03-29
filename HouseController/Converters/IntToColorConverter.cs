using System.Globalization;

namespace HouseController.Converters
{
	public class IntToColorConverter : IValueConverter
	{
        private static readonly Color OnColor = new(0, 255, 0);
        private static readonly Color OffColor = new(255, 0, 0);
        private static readonly Color ErrorColor = new(128, 128, 128);
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if(value == null) return ErrorColor;
            return (int)value switch
            {
                0 => OffColor,
                1 => OnColor,
                _ => ErrorColor
            };
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			var greenColor = new Color(0, 255, 0);
			return ((Color?)value == greenColor) ? 1 : 0;
		}
	}
}
