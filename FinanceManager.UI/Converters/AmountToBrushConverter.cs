using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace FinanceManager.UI.Converters
{
    public class AmountToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal dec)
            {
                var key = dec >= 0 ? "AccentGreen" : "AccentRed";
                var brush = Application.Current.TryFindResource(key) as System.Windows.Media.Brush;
                if (brush != null) return brush;
            }
            return System.Windows.Media.Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
