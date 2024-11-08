using System;
using System.Collections.Generic;
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

namespace Administrare_firma
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void UsernameTextMouseDown(object sender, MouseEventArgs e)
        {
            textBlockUsername.Focus();
        }
            
        private void UsernameTextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxUsername.Text.Length > 0)
            {
                textBlockUsername.Visibility = Visibility.Collapsed;
            }
            else
            {
                textBlockUsername.Visibility = Visibility.Visible;
            }
        }

        private void PasswordTextMouseDown(object sender, MouseEventArgs e)
        {
            textBlockPassword.Focus();
        }
        private void PasswordTextChanged(object sender, RoutedEventArgs e)
        {
            if (textBoxPassword.Password.Length > 0)
            {
                textBlockPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                textBlockPassword.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainPageWindow mainPage = new MainPageWindow();
            mainPage.Show();
            this.Close();
        }
    }
}
