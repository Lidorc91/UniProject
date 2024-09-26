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
using System;
using System.IO;
using System.Linq;
using System.IO.Ports;
using ScottPlot.AxisPanels;
using ScottPlot;
using System.Runtime.CompilerServices;
using Application.ViewModel;
using Application.Model;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UniProjectUI2
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variable Declarations
        DeviceViewModel vm;
        object Lock = new object();
        readonly ScottPlot.Plottables.DataLogger Logger1;
        readonly ScottPlot.Plottables.DataLogger Logger2;
        readonly ScottPlot.Plottables.DataLogger Logger3;
        readonly ScottPlot.Plottables.DataLogger Logger4;
        readonly ScottPlot.Plottables.DataLogger Logger5;
        
        private int[] _latestPacket;
        public int[] VM_realTimePacket{
            get => _latestPacket;
            set{
                if(_latestPacket != value){
                    _latestPacket=value;
                    UpdatePlotWithNewData(_latestPacket);
                }
            }
        }

        #endregion
       
        public MainWindow()
        {
            vm = new DeviceViewModel(new DeviceManager());
            DataContext = vm;

            InitializeComponent();

            // create  loggers and add them to the plot
            Logger1 = DevGraph.Plot.Add.DataLogger();
            Logger2 = DevGraph.Plot.Add.DataLogger();
            Logger3 = DevGraph.Plot.Add.DataLogger();
            Logger4 = DevGraph.Plot.Add.DataLogger();
            Logger5 = DevGraph.Plot.Add.DataLogger();
            Logger1.ViewSlide();
            Logger2.ViewSlide();
            Logger3.ViewSlide();
            Logger4.ViewSlide();
            Logger5.ViewSlide();

            InitializePlot();
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
            DevGraph.Plot.Axes.Bottom.Label.OffsetY = 4;
            DevGraph.Plot.Axes.SetLimitsY(bottom: 0, top: 33000);
            DashGraph.Plot.XLabel("Time [sec]");
            DashGraph.Plot.YLabel("Absorption Coefficient [1/m]");
            //adding the axies
            RightAxis axis1 = (RightAxis)DevGraph.Plot.Axes.Right;
            //configuring the legend
            Logger1.LegendText = "PD1";
            Logger2.LegendText = "PD2";
            Logger3.LegendText = "PD3";
            Logger4.LegendText = "PD4";
            Logger5.LegendText = "PD5";
            DevGraph.Plot.ShowLegend();
            DevGraph.Plot.Legend.Alignment = Alignment.UpperLeft;
            DevGraph.Plot.ScaleFactor = 2;
            DevGraph.Refresh();
        }
        private void UpdatePlotWithNewData(int[] data)
        {
            lock (Lock)
            {
                Logger1.Add(data[0]); //These is where the data is added to the dev_graph
                Logger2.Add(data[1]);
                Logger3.Add(data[2]);
                Logger4.Add(data[3]);
                Logger5.Add(data[4]);
                RecTime.Dispatcher.InvokeAsync(() =>
                {
                    DevGraph.Refresh();
                });
            }
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Play_button.Content = vm.StartReading ? "Halt" : "Play"; 
            vm.StartReading = !vm.StartReading;
        }
        
        private void StartRecord(object sender, RoutedEventArgs e)
        {
            byte recTime = byte.Parse(Recording_time_inputbox.Text);
            if(recTime <= 0){

                MessageBox.Show("Input values greater than 0", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            vm.StartRecording(recTime);
        }
        
        private void LEDChange(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null) {
                vm.SelectedLED = radioButton.Content.ToString();
            }
        }
        
        private void ChangeView(object sender, RoutedEventArgs e)
        {
            if (View_button.Content == "Change to Slide View")
            {
                Logger1.ViewSlide();
                Logger2.ViewSlide();
                Logger3.ViewSlide();
                Logger4.ViewSlide();
                Logger5.ViewSlide();
                RecTime.Dispatcher.InvokeAsync(() =>
                {
                    Logger1.ViewSlide();
                    Logger2.ViewSlide();
                    Logger3.ViewSlide();
                    Logger4.ViewSlide();
                    Logger5.ViewSlide();
                    View_button.Content = "Change to Full View";
                });
            }
            else
            {
                if (View_button.Content == "Change to Full View")
                {
                    Logger1.ViewFull();
                    Logger2.ViewFull();
                    Logger3.ViewFull();
                    Logger4.ViewFull();
                    Logger5.ViewFull();
                    View_button.Content = "Change to Slide View";
                }
            } 
        }

        private void RINTChange(object sender, RoutedEventArgs e)
        {
            
        }

        private void RTIAChange(object sender, RoutedEventArgs e)
        {

        }

        private void Analyze(object sender, RoutedEventArgs e)
        {

        }

    }
}
