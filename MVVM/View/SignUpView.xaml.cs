using Administrare_firma.MVVM.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Administrare_firma.MVVM.View
{
    /// <summary>
    /// Interaction logic for SignUpView.xaml
    /// </summary>
    public partial class SignUpView : UserControl
    {
        public SignUpView()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignUpViewModel viewModel)
            {
                if (sender is TextBox textBox)
                {
                    switch (textBox.Name)
                    {
                        case "FullnameBox":
                            viewModel.FullnameIsFocused = true;
                            break;
                        case "EmailBox":
                            viewModel.EmailIsFocused = true;
                            break;
                    }
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignUpViewModel viewModel)
            {
                if (sender is TextBox textBox)
                {
                    switch (textBox.Name)
                    {
                        case "FullnameBox":
                            viewModel.FullnameIsFocused = !string.IsNullOrEmpty(textBox.Text);
                            break;
                        case "EmailBox":
                            viewModel.EmailIsFocused = !string.IsNullOrEmpty(textBox.Text);
                            break;
                    }
                }
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignUpViewModel viewModel)
            {
                if (sender is PasswordBox passwordBox)
                {
                    switch (passwordBox.Name)
                    {
                        case "PasswordBox":
                            viewModel.PasswordIsFocused = true;
                            break;
                        case "ConfirmPasswordBox":
                            viewModel.ConfirmPasswordIsFocused = true;
                            break;
                    }
                }
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignUpViewModel viewModel)
            {
                if (sender is PasswordBox passwordBox)
                {
                    switch (passwordBox.Name)
                    {
                        case "PasswordBox":
                            viewModel.PasswordIsFocused = !string.IsNullOrEmpty(passwordBox.Password);
                            break;
                        case "ConfirmPasswordBox":
                            viewModel.ConfirmPasswordIsFocused = !string.IsNullOrEmpty(passwordBox.Password);
                            break;
                    }
                }
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

        private void ToggleConfirmPasswordVisibility(object sender, MouseButtonEventArgs e)
        {
            if (ConfirmPasswordBox.Visibility == Visibility.Visible)
            {
                ConfirmPasswordVisibleBox.Text = ConfirmPasswordBox.Password;
                ConfirmPasswordBox.Visibility = Visibility.Collapsed;
                ConfirmPasswordVisibleBox.Visibility = Visibility.Visible;
            }
            else
            {
                ConfirmPasswordBox.Password = ConfirmPasswordVisibleBox.Text;
                ConfirmPasswordBox.Visibility = Visibility.Visible;
                ConfirmPasswordVisibleBox.Visibility = Visibility.Collapsed;
            }
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignUpViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
                //viewModel.ValidatePasswords();
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignUpViewModel viewModel)
            {
                viewModel.ConfirmPassword = ((PasswordBox)sender).Password;
                //viewModel.ValidatePasswords();
            }
        }

    }
}
