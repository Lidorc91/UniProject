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
        static int packetNum = 1;
        static int bytesToRead = 22 * packetNum;
        static int pace = 200;
        byte[] buffer = new byte[bytesToRead];
        byte[] dataToSend;
        readonly ScottPlot.Plottables.DataLogger Logger1;
        readonly ScottPlot.Plottables.DataLogger Logger2;
        readonly ScottPlot.Plottables.DataLogger Logger3;
        readonly ScottPlot.Plottables.DataLogger Logger4;
        readonly ScottPlot.Plottables.DataLogger Logger5;
        System.Timers.Timer timer = new System.Timers.Timer(pace);
        System.Timers.Timer timer2 = new System.Timers.Timer(pace);
        private double[] times;
        private double[] values1;
        private double[] values2;
        private DateTime startTime;
        private static int data;
        private object Lock = new object();
        private object Lock2 = new object();

        //Record Variables
        Queue<Packet> recordQueue = new Queue<Packet>();
        int recordPacketsToRead = 0;
        int recTime = 0;
        Packet copyPacket = new Packet();
        // Specify the path for the output file
        string filePath = "output.csv";
        string exepath = null; //PLEASE CHANGE THIS TO THE LOCATION OF YOUR EXE FILE

        #endregion
        #region SerialPort Definition
        static private SerialPort serialPort;
        #endregion
        public MainWindow()
        {
            serialPort = new SerialPort();
            InitializeComponent();

            vm = new DeviceViewModel(new DeviceManager());
            DataContext = vm;

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
            InitializeSerialPort();

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
        private void UpdatePlotWithNewData(double time, int[] data)
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
            if (Play_button.Content == "Halt")
            {
                stopTransmit();
                timer.Enabled = false;
                Play_button.Content = "Play";
                return;
            }

            //This should only while the button says "play"
            startTransmit();
            timer.Elapsed += ReadData;
            timer.AutoReset = true; // Continuously fire the Elapsed event
            timer.Enabled = true; // Start the timer

            // Record the start time
            startTime = DateTime.Now;

            // Turn the play button into a Stop buttion
            RecTime.Dispatcher.InvokeAsync(() =>
            {
                Play_button.Content = "Halt";
            });
        }
        private void ReadData(object sender, ElapsedEventArgs e)
        {
            lock (Lock)
            {
                try
                {
                    serialPort.ReadExisting(); //Read(Clear) old data in buffer
                    Thread.Sleep(10); //Fill up buffer
                    int bytesRead = serialPort.Read(buffer, 0, bytesToRead);
                }
                catch (Exception e1)
                {
                    Console.WriteLine(e1.Message);
                }
                int[] data = decode(buffer);
                // Calculate elapsed time
                TimeSpan elapsedTime = e.SignalTime - startTime;
                //update Graph
                UpdatePlotWithNewData(elapsedTime.TotalSeconds, data);
            }
        }
        private void InitializeSerialPort()
        {
            serialPort = new SerialPort();
            serialPort.PortName = "COM3";
            serialPort.BaudRate = 115200;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;
            try
            {
                serialPort.Open();
                if (serialPort.IsOpen)
                {
                    Console.WriteLine("Connected");
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
        void startTransmit()
        {
            byte[] dataToSend = new byte[] { 0x10, 1 };
            sendCommand(dataToSend);
            if (Green_LED.IsChecked == true)
            {
                byte led = 0;
                dataToSend = new byte[] { 0x20, led };
                sendCommand(dataToSend);

            }
            if (Red_LED.IsChecked == true)
            {
                byte led = 1;
                dataToSend = new byte[] { 0x20, led };
                sendCommand(dataToSend);
            }
        }
        private void LEDColorChange(object sender, RoutedEventArgs e)
        {
            RadioButton color = (sender as RadioButton);
            if (color.Content == Green_LED.Content)
            {
                byte led = 0;
                dataToSend = new byte[] { 0x20, led };
                sendCommand(dataToSend);

            }
            if (color.Content == Red_LED.Content)
            {
                byte led = 1;
                dataToSend = new byte[] { 0x20, led };
                sendCommand(dataToSend);
            }
        }
        void sendCommand(byte[] dataToSend)
        {
            try
            {
                serialPort.Write(dataToSend, 0, dataToSend.Length);
                Thread.Sleep(250);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private void RTIAChange(object sender, SelectionChangedEventArgs e)
        {

            if (!this.IsLoaded) //Makes sure that ui has loaded to stop premature running
            {
                return;
            }
            ComboBox RTIABox = sender as ComboBox;
            double RTIA = double.Parse(RTIABox.SelectedValue.ToString());
            for (int i = 0; i < 5; i++)
            {
                sendCommand(new byte[] { (byte)(i + 8), (byte)RTIA });
            }
        }
        private void RINTChange(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsLoaded) //Makes sure that ui has loaded to stop premature running
            {
                return;
            }
            ComboBox RINTBox = sender as ComboBox;
            int RINT = int.Parse(RINTBox.SelectedValue.ToString());
            for (int i = 0; i < 5; i++)
            {
                sendCommand(new byte[] { (byte)(i + 11), (byte)RINT });
            }
        }
        private void ChCurrent(object sender, RoutedEventArgs e2)
        {

            if (!this.IsLoaded || !serialPort.IsOpen) //Makes sure that ui has loaded to stop premature running
            {
                return;
            }
            int value = int.Parse(Current_inputbox.Text);
            int calcValue = (int)(Math.Round((value * 126) / 198.5));
            for (int i = 0; i < 5; i++)
            {
                sendCommand(new byte[] { (byte)(i), (byte)calcValue });
            }
            serialPort.DiscardInBuffer();
        }
        private void RecordTimeChanged(object sender, RoutedEventArgs e)
        {

            recTime = int.Parse(Recording_time_inputbox.Text);

        }
            void Record(object sender, ElapsedEventArgs e)
            {

            try
            {
                serialPort.ReadExisting(); // clear the buffer
                copyPacket = new Packet();
                int bytesRead = serialPort.Read(buffer, 0, bytesToRead);
                //Thread.Sleep(10); //Fill up buffer
            }

            
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
            }
            int displaynumber = -(recordPacketsToRead - recTime*100) / 100;
            RecTime.Dispatcher.InvokeAsync(() =>
            {
                RecTime.Content = recordPacketsToRead.ToString();
            });

            copyPacket.setData(buffer, bytesToRead);
            recordQueue.Enqueue(copyPacket);
            --recordPacketsToRead;
            //now for standard reading
            int[] data = decode(copyPacket.getData());
            // Calculate elapsed time
            TimeSpan elapsedTime = e.SignalTime - startTime;

                //update Graph
            UpdatePlotWithNewData(elapsedTime.TotalSeconds, data);
            if (recordPacketsToRead == 0)
               EndRecording();
            }
        private void StartRecord(object sender, RoutedEventArgs e)
        {
            if(recTime == 0)
                recTime = 1; // set a hidden defualt
            recordPacketsToRead = 100 * recTime;
            // Subscribe the new event handler
            timer2.Elapsed += Record;
            timer2.AutoReset = true; // Continuously fire the Elapsed event
            timer2.Enabled = true; // Start the timer


            Record_button.Content = "Record Active!";
            Record_button.IsEnabled = false;
        }
        void EndRecording()
        {

            timer2.Enabled = false; // Start the timer
            RecTime.Dispatcher.InvokeAsync(() =>
            {
                Record_button.Content = "Record";
                Record_button.IsEnabled = true;
            });

            int[,] decodedArray = new int[recTime * 100, 5];
            for (int i = 0; i < recTime * 100; i++)
            {
                int[] decoded = decode(recordQueue.Dequeue().getData());
                for (int j = 0; j < 5; j++)
                    decodedArray[i,j] = decoded[j];
            }
            PrintToCSV(decodedArray);
        }
        void PrintToCSV(int[,] array)
        {

            var rows = array.GetLength(0);
            var cols = array.GetLength(1);

            using (StreamWriter file = new StreamWriter("2dArrayOut.csv"))
            {
                file.AutoFlush = true;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        file.Write($"{array[i, j]},");
                    }
                    file.Write("\n");
                }
            }
            RecTime.Dispatcher.InvokeAsync(() =>
            {
                RecTime.Content = "Printing to " + filePath;
            });

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
        int[] decode(byte[] b) //TEST
        {
            int[] arr = new int[5 * packetNum]; // here is the decoded array
            string[] hexString = new string[b.Length];
            for (int i = 1; i < arr.Length * 4; i += 4) //run 5 times for the 5 PDs
            {
                string a1 = b[i].ToString("X").PadLeft(2, '0'); //this is the first number
                string a2 = b[i + 1].ToString("X").PadLeft(2, '0'); //this is the 2nd number
                                                                    //numbers 3 & 4 are always 0 so they are ignored
                StringBuilder concatenatedHex = new StringBuilder();
                concatenatedHex.Append(a2);
                concatenatedHex.Append(a1); //this mean thats the 2nd number is the first element of the decoded number
                arr[(i - 1) / 4] = Convert.ToInt32(concatenatedHex.ToString(), 16); //convert back to dec
            }

            //PD Switch
            arr[2] += arr[3];
            arr[3] = arr[2] - arr[3];
            arr[2] -= arr[3];

            arr[3] += arr[4];
            arr[4] = arr[3] - arr[4];
            arr[3] -= arr[4];

            /*foreach (int a in arr)
            {
                Console.WriteLine(a); //posts the decoded array one by one
            }*/
            return arr;
        }
        void stopTransmit()
        {
            byte[] dataToSend = new byte[] { 0x10, 0 };
            sendCommand(dataToSend);

        }
        private void Analyze(object sender, RoutedEventArgs e)
        {
           // Process.Start(exePath, filePath);
        }


    }
}
