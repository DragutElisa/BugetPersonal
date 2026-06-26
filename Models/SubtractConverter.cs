using System.Globalization;

namespace BugetPersonal.Models
{
    public class SubtractConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal val1 && parameter is decimal val2)
            {
                return val1 - val2;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}