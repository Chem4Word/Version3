using System;
using System.Globalization;
using System.Windows.Data;

namespace Chem4Word.Library
{
    public class DefaultNameConverter : IValueConverter
    {
        private const string UNNAMED = "<no name>";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (String.IsNullOrEmpty(value as string))
            {
                return UNNAMED;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value as string) == UNNAMED)
            {
                return "";
            }
            return value;
        }
    }
}