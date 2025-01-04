using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Administrare_firma.Core
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isFocused)
            {
                if (parameter is string param)
                {
                    if (param == "UpText" || param == "WrongCredentials" || param == "Invert")
                    {
                        return isFocused ? Visibility.Visible : Visibility.Hidden;
                    }

                }

                return isFocused ? Visibility.Hidden : Visibility.Visible;
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }

            return false;
        }
    }
    public class BoolToForegroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isWrong)
            {
                if (parameter is string param)
                    if (param == "Manager")
                    {
                        var color1 = (Color)ColorConverter.ConvertFromString("#F5E4A1");
                        var color2 = (Color)ColorConverter.ConvertFromString("#B1E5F2");
                        return isWrong ? new SolidColorBrush(color1) : new SolidColorBrush(color2);
                    }
                return isWrong ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Gray);
            }
            return new SolidColorBrush(Colors.Gray);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MultiColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return Brushes.Black;

            bool passwordsMatch = values[0] is bool match && match;
            bool inputIsWrong = values[1] is bool wrong && wrong;

            if (inputIsWrong || !passwordsMatch)
                return Brushes.Red;

            return Brushes.Gray;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
