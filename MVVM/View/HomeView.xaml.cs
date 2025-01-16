using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Administrare_firma.MVVM.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }
        private void OpenPdfButton_Click(object sender, RoutedEventArgs e)
        {
            string pdfPath = "C:\\Users\\HUAWEI\\Desktop\\understanding-our-political-nature.pdf";

            if (File.Exists(pdfPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = pdfPath,
                    UseShellExecute = true // Folosește aplicația implicită de vizualizare PDF
                });
            }
            else
            {
                MessageBox.Show("Fișierul PDF nu a fost găsit.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
