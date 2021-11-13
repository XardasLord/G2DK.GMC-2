using System;
using System.Globalization;
using System.Windows.Data;

namespace GothicModComposer.UI.Converters
{
    public class IsNotNullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value != null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException("Two-way binding not supported by IsNotNullToBoolConverter");
    }
}