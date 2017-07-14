using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace Chem4Word.Controls
{
    public class DoNothingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine("Value of {0} is {1}", parameter?.ToString(), value);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}