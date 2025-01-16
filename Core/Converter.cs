using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Administrare_firma.Core
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool state)
            {
                if (parameter is string param)
                {
                    if (param == "UpText" || param == "WrongCredentials" || param == "Invert" || param == "Admin")
                    {
                        return state ? Visibility.Visible : Visibility.Collapsed;
                    }
                    else if (param == "CreateRequestButton")
                    {
                        return state ? Visibility.Collapsed : Visibility.Visible;
                    }
                    else if (param == "Clocking")
                    {
                        return state ? Visibility.Collapsed : Visibility.Visible;
                    }
                    else if (param == "EmployeesStatistics")
                    {
                        return state ? Visibility.Collapsed : Visibility.Visible;
                    }
                    else if (param == "Requests")
                    {
                        return state ? Visibility.Collapsed : Visibility.Visible;
                    }
                    else if (param == "ProjectsButton" || param == "DepartmentButton" || param == "EditInfoButton")
                    {
                        return state ? Visibility.Collapsed : Visibility.Visible;
                    }
                }
                return state ? Visibility.Hidden : Visibility.Visible;
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
    public class ButtonStateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool state && parameter is string buttonType)
            {
                if (buttonType == "WorkingTime")
                {
                    return state ? new SolidColorBrush(Colors.LightCoral) : new SolidColorBrush(Colors.LightGreen);
                }
                else if (buttonType == "Break")
                {
                    return state ? new SolidColorBrush(Colors.LightCoral) : new SolidColorBrush(Colors.LightGreen);
                }
            }

            return new SolidColorBrush(Colors.LightGray); // Default color
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
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool state)
            {
                if (parameter is string param)
                    if (param == "DepartmentName")
                        return state ? "Departments" : "Department details";
                    else if (param == "WorkingTime")
                        return state ? "Quit work" : "Start work";
                    else if (param == "BreakTime")
                        return state ? "Quit the brake" : "Take a break";
            }
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class HoursToMinutesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal hours)
            {
                TimeSpan timeSpan = TimeSpan.FromHours((double)hours);

                int displayHours = timeSpan.Hours;
                int displayMinutes = timeSpan.Minutes;
                int displaySeconds = timeSpan.Seconds;
                int displayMilliseconds = timeSpan.Milliseconds;

                StringBuilder result = new StringBuilder();
                if (displayHours > 0)
                {
                    result.Append($"{displayHours} h ");
                }
                if (displayMinutes > 0 || result.Length > 0)
                {
                    result.Append($"{displayMinutes} min ");
                }
                if (displaySeconds > 0 || result.Length > 0)
                {
                    result.Append($"{displaySeconds} sec ");
                }
                result.Append($"{displayMilliseconds} ms");

                return result.ToString().Trim();
            }
            return "0 ms";
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // Conversion back is not needed
        }
    }
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value as string;
            if (status == "Approved")
                return Brushes.LightSeaGreen;
            else if (status == "Rejected")
                return Brushes.RosyBrown;
            else if (status == "Pending")
                return Brushes.Yellow;
            else
                return Brushes.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class StatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine("Status Converter ");
            string status = value as string;

            if (string.IsNullOrEmpty(status))
                return null;

            // Calea relativă către folderul cu iconițe
            string basePath = "pack://application:,,,/MVVM/View/Icons/";

            // Determină iconița în funcție de status
           if (status == "Approved")
                return new BitmapImage(new Uri(basePath + "approved.png"));
            else if (status == "Rejected")
                return new BitmapImage(new Uri(basePath + "rejected.png"));
            else if (status == "Pending")
                return new BitmapImage(new Uri(basePath + "info.png"));
            else
                return null;


        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    public class NullableTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "-"; // Afișează o liniuță dacă valoarea este null

            if (value is TimeSpan time)
                return time.ToString(@"hh\:mm"); // Formatează valoarea TimeSpan

            return "-"; // Default în caz că nu e TimeSpan
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
