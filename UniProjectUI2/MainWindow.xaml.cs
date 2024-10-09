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
        static int pace = 1000;
        static int pace2 = 10;
        static int freq2 = (1000 / pace2);
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
        Stopwatch _stopwatch;
        private const int packetSize = 22;
        int totalPackets;
        int targetSize;
        private byte[,] dataArray;
        string csvname;


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

            _stopwatch = new Stopwatch();

            // create  loggers and add them to the plot
            Logger1 = DevGraph.Plot.Add.DataLogger();
            Logger2 = DevGraph.Plot.Add.DataLogger();
            Logger3 = DevGraph.Plot.Add.DataLogger();
            Logger4 = DevGraph.Plot.Add.DataLogger();
            Logger5 = DevGraph.Plot.Add.DataLogger();
            Logger1.ViewJump();
            Logger2.ViewJump();
            Logger3.ViewJump();
            Logger4.ViewJump();
            Logger5.ViewJump();

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
            DevGraph.Plot.Axes.SetLimits(-5, 100, -5, 35000);
            DevGraph.Refresh();
            DashGraph.Plot.Title("PPG");
            DashGraph.Plot.XLabel("Time [sec]");
            DashGraph.Plot.YLabel("Intensity [a.u]");



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
                DevGraph.Plot.Axes.SetLimits(-5, 100, -5, 35000);
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
                //update Graph
                UpdatePlotWithNewData(0.0, data);
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
                _stopwatch.Reset();
                serialPort.ReadExisting(); // clear the buffer
                copyPacket = new Packet();
                _stopwatch.Start();
                int bytesRead = serialPort.Read(buffer, 0, bytesToRead);
                _stopwatch.Stop();

            }

            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
            }
            copyPacket.setData(buffer, bytesToRead);
            recordQueue.Enqueue(copyPacket);
            --recordPacketsToRead;
            RecTime.Dispatcher.InvokeAsync(() =>
            {
                RecTime.Content = _stopwatch.Elapsed.TotalMilliseconds.ToString();
            });
            if (recordPacketsToRead == 0)
                EndRecording();
        }
        private void StartRecord(object sender, RoutedEventArgs e)
        {
            // if(recTime == 0)
            //     recTime = 1; // set a hidden defualt
            // recordPacketsToRead = freq2 * recTime;

            //DateTime RectimeStart = DateTime.Now;

            // // Subscribe the new event handler
            // timer2.Elapsed += Record;
            // timer2.AutoReset = true; // Continuously fire the Elapsed event
            //timer2.Enabled = true; // Start the timer
            //timer.Enabled = false;
            timer.Elapsed -= ReadData;
            timer.Elapsed += ReadData2;
            startTime = DateTime.Now;

            Record_button.Content = "Record Active!";
            Record_button.IsEnabled = false;
            totalPackets = freq2 * recTime;
            targetSize = totalPackets * packetSize;
            dataArray = new byte[totalPackets, packetSize]; // 60000 x 22 array

            serialPort.DiscardInBuffer(); // Clear the input buffer
            // Start a new thread to wait for the buffer to fill and then transfer data
            Thread recordThread = new Thread(WaitForBufferFill);
            recordThread.Start();

        }
        void EndRecording()
        {
            timer2.Enabled = false; // End the timer
            TimeSpan elapsedRecTime = DateTime.Now - startTime;

            RecTime.Dispatcher.InvokeAsync(() =>
            {
                RecTime.Content = elapsedRecTime.ToString();
                Record_button.Content = "Record";
                Record_button.IsEnabled = true;
            });

            int[,] decodedArray = new int[recTime * freq2, 5];
            for (int i = 0; i < recTime * freq2; i++)
            {
                int[] decoded = decode(recordQueue.Dequeue().getData());
                for (int j = 0; j < 5; j++)
                    decodedArray[i, j] = decoded[j];
            }
            PrintToCSV(decodedArray);
        }
        void PrintToCSV(int[,] array)
        {

            var rows = array.GetLength(0);
            var cols = array.GetLength(1);
            csvname = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";
            // Get the current directory
            string currentDirectory = Directory.GetCurrentDirectory();

            // Define paths to the 'Log' and 'Analysefolder' directories
            string logFolderPath = System.IO.Path.Combine(currentDirectory, "Log");
            string analyseFolderPath = System.IO.Path.Combine(currentDirectory, "Anslysefolder");
            string logFilePath = System.IO.Path.Combine(logFolderPath, csvname);
            string analyseFilePath = System.IO.Path.Combine(analyseFolderPath, csvname);

            using (StreamWriter file = new StreamWriter(analyseFilePath))
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
            using (StreamWriter file = new StreamWriter(logFilePath))
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
            //update Most_Recent_Output.txt
            string outputFilePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Most_Recent_Output.txt");
            using (StreamWriter outputFile = new StreamWriter(outputFilePath, false)) // 'true' for append mode
            {
                outputFile.WriteLine(csvname); // Write the full path of the CSV file
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

            //PD Switch due to manufacture error
            arr[2] += arr[3];
            arr[3] = arr[2] - arr[3];
            arr[2] -= arr[3];

            arr[3] += arr[4];
            arr[4] = arr[3] - arr[4];
            arr[3] -= arr[4];
            return arr;
        }
        void stopTransmit()
        {
            byte[] dataToSend = new byte[] { 0x10, 0 };
            sendCommand(dataToSend);

        }
        private void Analyze(object sender, RoutedEventArgs e)
        {

            string curretDirectory = Directory.GetCurrentDirectory();
            string analyseFolderPath = System.IO.Path.Combine(curretDirectory, "Anslysefolder");
            string analyseFilePath = System.IO.Path.Combine(analyseFolderPath, csvname);

            string exename = "PPG_analyzer_v1.exe";
            exepath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), exename);
            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = exepath,
                Arguments = analyseFilePath,  // Pass the filepath to the .exe
                UseShellExecute = false, // We don't use shell execute here
                RedirectStandardOutput = false, // We don't need to capture the output
                CreateNoWindow = false, // You can set this to true if you don't want a new window
                RedirectStandardError = true
            };

            try
            {
                // Start the external MATLAB process
                using (Process process = new Process())
                {
                    // Assign the start info
                    process.StartInfo = processInfo;

                    // Subscribe to error streams
                    process.ErrorDataReceived += (sender, e) => Console.WriteLine("ERROR: " + e.Data);

                    // Start the process
                    process.Start();

                    // Begin async reading of output and error streams
                    process.BeginErrorReadLine();

                    // Wait for the process to exit
                    process.WaitForExit();

                    // Keep the console open
                    Console.WriteLine("Process completed. Press any key to exit...");
                    Console.Read();
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                MessageBox.Show($"Error starting the MATLAB app: {ex.Message} # \n{ex.StackTrace}");
            }
        }
        private void WaitForBufferFill()
        {
            while (true)
            {
                int bytesToRec = serialPort.BytesToRead;
                int count = 0;
                // Check if the input buffer has filled to the target size
                if (bytesToRec >= targetSize)
                {
                    // Transfer data from input buffer to the 60000 x 22 array
                    TransferDataToArray();
                    break;
                }

                // Optional: Sleep for a short time to prevent busy waiting
                Thread.Sleep(10); // Adjust the sleep time as needed
            }
        }
        void ReadData2(object sender, ElapsedEventArgs e)
        {
            try
            {
                int bytesRead = serialPort.Read(buffer, 0, bytesToRead);
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
            }
            int[] data = decode(buffer);
            // Calculate elapsed time
            //update Graph
            UpdatePlotWithNewData(0.0, data);
        }
        private void TransferDataToArray()
        {
            byte[] buffer = new byte[targetSize];
            int bytesRead = serialPort.Read(buffer, 0, targetSize); // Read the entire buffer into a byte array

            if (bytesRead == targetSize)
            {
                // Populate the 60000 x 22 array with the received data
                for (int i = 0; i < totalPackets; i++)
                {
                    for (int j = 0; j < packetSize; j++)
                    {
                        dataArray[i, j] = buffer[i * packetSize + j];
                    }
                }
            }
            Packet[] packets = new Packet[totalPackets];
            for (int i = 0; i < totalPackets; i++)
            {
                byte[] row = new byte[packetSize];
                for (int j = 0; j < packetSize; j++)
                {
                    row[j] = dataArray[i, j]; // Copy the row data
                }
                packets[i] = new Packet(row);
            }
            int[,] decodedArray = new int[totalPackets, 5];
            for (int i = 0; i < totalPackets; i++)
            {
                int[] decoded = decode(packets[i].getData());
                for (int j = 0; j < 5; j++)
                    decodedArray[i, j] = decoded[j];
            }
            TimeSpan elapsedRecTime = DateTime.Now - startTime;
            RecTime.Dispatcher.InvokeAsync(() =>
            {
                RecTime.Content = elapsedRecTime.ToString();
                Record_button.Content = "Record";
                Record_button.IsEnabled = true;
            });
            timer.Elapsed -= ReadData2;
            timer.Elapsed += ReadData;
            PrintToCSV(decodedArray);
        }
        string getFilePath()
        {
            // Define the path to the Most_Recent_Output.txt file
            string txtFilePath = @"Most_Recent_Output.txt";

            // Check if the file exists
            if (!File.Exists(txtFilePath))
            {
                throw new FileNotFoundException("The file Most_Recent_Output.txt was not found.");
            }

            // Read the first line
            using (StreamReader reader = new StreamReader(txtFilePath))
            {
                string filePath = reader.ReadLine();

                // Remove ".csv" from the end if present
                if (filePath.EndsWith(".csv"))
                {
                    filePath = filePath.Substring(0, filePath.Length - 4);
                }

                // Append "_PPG_output.csv" to the file path
                return filePath + "_PPG_output.csv";
            }
        }
            void Readanalysed()
            {

                // File path
                string filePath = getFilePath();

                // Array to store the PPG values
                List<double> PPG_Array = new List<double>();

                // Variables to store the non-zero values from the second row
                int HR = 0;
                int RR = 0;
                int HRV = 0;
                double SpO2 = 0;

                // Reading the CSV file
                using (var reader = new StreamReader(filePath))
                {
                    // Skipping the header row
                    var header = reader.ReadLine();

                    int currentRow = 0;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        // Storing all values from the PPG column (column 1)
                        PPG_Array.Add(double.Parse(values[0]));

                        // If it's the second row, extract non-zero values
                        if (currentRow == 1)
                        {
                            HR = int.Parse(values[1]);
                            RR = int.Parse(values[2]);
                            HRV = int.Parse(values[3]);
                            SpO2 = double.Parse(values[4]);
                        }

                        currentRow++;
                    }
                }
                upDateDashboard(PPG_Array, HR, RR, HRV, SpO2);
            }
            void upDateDashboard(List<double> PPG_Array, int HR, int RR, int HRV, double SpO2)
            {
                SpO2_number.Content = SpO2.ToString();
                HR_number.Content = HR.ToString();
                RR_number.Content = RR.ToString();
                HRV_number.Content = HRV.ToString();
                double[] PPGArray = PPG_Array.ToArray();
                DashGraph.Plot.Add.Signal(PPGArray);
            }


        private void Start_Analyze(object sender, RoutedEventArgs e)
        {
            
                // File path
                string filePath = getFilePath();

                // Array to store the PPG values
                List<double> PPG_Array = new List<double>();

                // Variables to store the non-zero values from the second row
                int HR = 0;
                int RR = 0;
                int HRV = 0;
                double SpO2 = 0;

                // Reading the CSV file
                using (var reader = new StreamReader(filePath))
                {
                    // Skipping the header row
                    var header = reader.ReadLine();

                    int currentRow = 0;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        // Storing all values from the PPG column (column 1)
                        PPG_Array.Add(double.Parse(values[0]));

                        // If it's the second row, extract non-zero values
                        if (currentRow == 1)
                        {
                            HR = int.Parse(values[1]);
                            RR = int.Parse(values[2]);
                            HRV = int.Parse(values[3]);
                            SpO2 = double.Parse(values[4]);
                        }

                        currentRow++;
                    }
                }
                upDateDashboard(PPG_Array, HR, RR, HRV, SpO2);
        }
    }
}
