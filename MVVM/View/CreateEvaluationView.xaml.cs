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

namespace Administrare_firma.MVVM.View
{
    /// <summary>
    /// Interaction logic for CreateEvaluationView.xaml
    /// </summary>
    public partial class CreateEvaluationView : UserControl
    {
        public CreateEvaluationView()
        {
            InitializeComponent();
        }
        private void ScoreTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsValidScore(e.Text);
        }

        private bool IsValidScore(string text)
        {
            int score;
            bool isValidNumber = int.TryParse(text, out score);
            return isValidNumber && score >= 1 && score <= 10;
        }
        private void ScoreTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox scoreTextBox = sender as TextBox;
            if (scoreTextBox != null)
            {
                int score;
                if (int.TryParse(scoreTextBox.Text, out score))
                {
                    if (score > 10)
                    {
                        scoreTextBox.Text = "10";
                    }
                    else if (score < 1)
                    {
                        scoreTextBox.Text = "1";
                    }
                }
            }
        }
    }
}
