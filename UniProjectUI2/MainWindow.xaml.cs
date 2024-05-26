using ScottPlot.Colormaps;
using ScottPlot.Plottables;
using ScottPlot.WPF;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using ScottPlot.AxisPanels;
using ScottPlot;

namespace UniProjectUI2
{
   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly ScottPlot.Plottables.DataLogger Logger1;
        readonly ScottPlot.Plottables.DataLogger Logger2;
        System.Timers.Timer timer = new System.Timers.Timer(1000);
        private double[] times; 
        private double[] values1;
        private double[] values2;
        public MainWindow()
        {
            InitializeComponent();
            InitializePlot();

            // create two loggers and add them to the plot
            Logger1 = DevGraph.Plot.Add.DataLogger();
            Logger2 = DevGraph.Plot.Add.DataLogger();
            //adding the axies
            RightAxis axis1 = (RightAxis)DevGraph.Plot.Axes.Right;


        }
        private void CurrentValidationTextBox(object sender, TextCompositionEventArgs e) //this method validates inputs into the current box
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
        private void InitializePlot()
        {
            DevGraph.Plot.Title("Detectors' intensity");
            DevGraph.Plot.XLabel("Time [sec]");
            DevGraph.Plot.YLabel("Intensity [a.u]");

        }
        private void UpdatePlotWithNewData(double time, double value1, double value2)
        {
            Logger1.Add(value1);
            Logger2.Add(value2);
            DevGraph.Refresh(); 
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if(Play_button.Content == "Play")
            {
            timer.Elapsed += GenerateRandomNumbers;
            timer.AutoReset = true; // Continuously fire the Elapsed event
            timer.Enabled = true; // Start the timer

            // Record the start time
            startTime = DateTime.Now;

            // Turn the play button into a Stop buttion
            Play_button.Content = "Stop";
            }
            if(Play_button.Content == "Stop")
            {
                timer.Enabled = false;
                Play_button.Content = "Play"
            }
        }
        private DateTime startTime;
        private void GenerateRandomNumbers(object sender, ElapsedEventArgs e)
        {
            Random rnd = new Random();
            int num1 = rnd.Next(1, 101); // Generates a random number between 1 and 100
            int num2 = rnd.Next(1, 101); // Generates another random number between 1 and 100

            // Calculate elapsed time
            TimeSpan elapsedTime = e.SignalTime - startTime;

            //update Graph
            UpdatePlotWithNewData(elapsedTime.TotalSeconds, num1, num2);

            if (elapsedTime.TotalSeconds >= 100) // if enough time has passed
            {
                ((System.Timers.Timer)sender).Stop(); // Stop the timer
            }
        }

    }

}