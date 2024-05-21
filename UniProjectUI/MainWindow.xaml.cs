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
using System.Text.RegularExpressions;



namespace UniProjectUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                double[] dataX = { 1, 2, 3, 4, 5 };
                double[] dataY = { 1, 4, 9, 16, 25 };
                WpfPlot1.Plot.Add.Scatter(dataX, dataY);
                WpfPlot1.Refresh();
            };

        }
        private void CurrentValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            // Check if the current text plus the new character is a valid integer
            string newText = Current_inputbox.Text + e.Text;
            Regex regex = new Regex(@"^\d+$"); // Regular expression to match digits only
            bool isValidInteger = regex.IsMatch(newText);

            // Check if the integer is within the range of 0 to 200
            bool isInRange = int.TryParse(newText, out int value) && value >= 0 && value <= 200;

            // Allow the input only if it passes both checks
            e.Handled = !(isValidInteger && isInRange);
        }
    }

}
