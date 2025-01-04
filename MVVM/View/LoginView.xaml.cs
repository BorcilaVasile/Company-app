using Administrare_firma.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Administrare_firma.MVVM.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();

        }

        private void UsernameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.EmailIsFocused = true;
            }
        }

        private void UsernameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel && UsernameBox.Text == "")
            {
                viewModel.EmailIsFocused = false;
                viewModel.InputIsWrong = false; 
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.PasswordIsFocused = true;
                if(PasswordBox.Password=="")
                    viewModel.InputIsWrong = false;
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel && PasswordBox.Password == "")
            {
                viewModel.PasswordIsFocused = false;
            }
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordVisibleBox.Text = PasswordBox.Password;
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }
        private void TogglePasswordVisibility(object sender, MouseButtonEventArgs e)
        {
            if (PasswordBox.Visibility == Visibility.Visible)
            {
                PasswordVisibleBox.Text = PasswordBox.Password;
                PasswordBox.Visibility = Visibility.Collapsed;
                PasswordVisibleBox.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordBox.Password = PasswordVisibleBox.Text;
                PasswordBox.Visibility = Visibility.Visible;
                PasswordVisibleBox.Visibility = Visibility.Collapsed;
            }
        }
    }

}
