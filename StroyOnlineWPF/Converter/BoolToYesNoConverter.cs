using System;
using System.Globalization;
using System.Windows.Data;

namespace StroyOnlineWPF
{
    public class BoolToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                bool isActive = (bool)value;
                return isActive ? "Да" : "Нет";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}