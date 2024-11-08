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
using System.Windows.Shapes;

namespace Administrare_firma
{
    /// <summary>
    /// Interaction logic for MainPageWindow.xaml
    /// </summary>
    public partial class MainPageWindow : Window
    {
        public MainPageWindow()
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
    }
}
