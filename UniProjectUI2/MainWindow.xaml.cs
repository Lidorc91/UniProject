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
using System.IO.Ports;
using System.Runtime.CompilerServices;

namespace UniProjectUI2
{
   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        #region Variable Declarations
            int packetNum = 1;
            int bytesToRead = 22 * packetNum;
            byte[] buffer = new byte[bytesToRead];
            byte[] dataToSend;
            readonly ScottPlot.Plottables.DataLogger Logger1;
            readonly ScottPlot.Plottables.DataLogger Logger2;
            readonly ScottPlot.Plottables.DataLogger Logger3;
            readonly ScottPlot.Plottables.DataLogger Logger4;
            readonly ScottPlot.Plottables.DataLogger Logger5;
            System.Timers.Timer timer = new System.Timers.Timer(100);
            private double[] times;
            private double[] values1;
            private double[] values2;
            private DateTime startTime;
            private static int data;
        #endregion
        #region SerialPort Definition
            private static SerialPort serialPort = new SerialPort();
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            InitializePlot();
            InitializeSerialPort();

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
        private void UpdatePlotWithNewData(double time, int[] data)
        {
            Logger1.Add(data[0]);
            Logger2.Add(data[1]);
            Logger3.Add(data[2]);
            Logger4.Add(data[3]);
            Logger5.Add(data[4]);
            DevGraph.Refresh(); 
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if(Play_button.Content == "Play")
            {
            
            startTransmit();
            timer.Elapsed += ReadData();
            timer.AutoReset = true; // Continuously fire the Elapsed event
            timer.Enabled = true; // Start the timer

            // Record the start time
            startTime = DateTime.Now;

            // Turn the play button into a Stop buttion
            Play_button.Content = "Stop";
            }
            if(Play_button.Content == "Stop")
            {
                stopTransmit()
                timer.Enabled = false;
                Play_button.Content = "Play";
            }
        }
        private void ReadData(object sender, ElapsedEventArgs e)
        {
                try
                {                 
                    serialPort.ReadExisting(); //Read(Clear) old data in buffer
                    Thread.Sleep(10); //Fill up buffer
                    int bytesRead = serialPort.Read(buffer, 0, bytesToRead);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                int[] data = decode(buffer);
            // Calculate elapsed time
            TimeSpan elapsedTime = e.SignalTime - startTime;
            //update Graph
            UpdatePlotWithNewData(elapsedTime.TotalSeconds,data);
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
               if (s.IsOpen)
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
            }
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
        private void LEDColorChange(object sender, RoutedEventArgs e)
        {
            RadioButton color = (sender as RadioButton);
            if(color.Content == Green_LED.Content)
            {
                byte led=0;
                dataToSend = new byte[] { 0x20, led };
                sendCommand(dataToSend);

            }
            if(color.Content == Red_LED.Content)
            {
                byte led=1;
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
            ComboBox RTIABox = sender as ComboBox;
            int RTIA = int.Parse(RTIABox.SelectedItem.ToString());
            for (int i = 0; i < 5; i++)
            {
                sendCommand(new byte[] {(byte)(i+8), (byte)RTIA});
            }
        }
        private void RINTChange(object sender, SelectionChangedEventArgs e)
        {
            ComboBox RINTBox = sender as ComboBox;
            int RINT = int.Parse(RINTBox.SelectedItem.ToString());
            for (int i = 0; i < 11; i++)
            {
                sendCommand(new byte[] {(byte)(i+11), (byte)RINT});
            }
        }
        private void ChangeCurrent(object sender, TextChangedEventArgs e)
        {
                
                int value = Current_inputbox.Text;
                int calcValue = (int)(Math.Round((value * 126) / 198.5));
                for (int i = 0; i < 5; i++)
                {
                    sendCommand(new byte[] { (byte)(i), (byte)calcValue });
                }
                serialPort.DiscardInBuffer();
        }

        private void RecordTimeChanged(object sender, TextChangedEventArgs e)
        {
            retrun  //not sure this method is needed
        }

        private void StartTest(object sender, RoutedEventArgs e)
        {

        }

        private void StartRecord(object sender, RoutedEventArgs e)
        {

        }

        private void StopRecording(object sender, RoutedEventArgs e)
        {

        }
        int[] decode(byte[] b) //TEST
        {
                int[] arr = new int[5*packetNum]; // here is the decoded array
                string[] hexString = new string[b.Length];
                for(int i = 1; i< arr.Length*4;i+=4) //run 5 times for the 5 PDs
                {             
                    string a1 = b[i].ToString("X").PadLeft(2,'0'); //this is the first number
                    string a2 = b[i+1].ToString("X").PadLeft(2,'0'); //this is the 2nd number
                    //numbers 3 & 4 are always 0 so they are ignored
                    StringBuilder concatenatedHex = new StringBuilder(); 
                    concatenatedHex.Append(a2);
                    concatenatedHex.Append(a1); //this mean thats the 2nd number is the first element of the decoded number
                    arr[(i-1) / 4] = Convert.ToInt32(concatenatedHex.ToString(),16); //convert back to dec
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

    }

}