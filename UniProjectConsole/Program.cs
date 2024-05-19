// See https://aka.ms/new-console-template for more information
using System;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;

namespace UARTConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Variable Declarations
            int packetNum = 1;
            int bytesToRead = 22 * packetNum;
            byte[] buffer = new byte[bytesToRead];
            byte[] dataToSend;
            #endregion
            #region SerialPort Definition
            SerialPort serialPort = new SerialPort();
            serialPort.PortName = "COM3";
            serialPort.BaudRate = 115200;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;
            #endregion
            
            int operation = 1;
            do
            {
                switch (operation)
                {
                    #region Initilization
                    case 1: //Initilization
                        try
                        {
                            serialPort.Open();
                            if (serialPort.IsOpen)
                            {
                                Console.WriteLine("Connected");
                            }
                        }
                        catch (Exception e) { Console.WriteLine(e.Message); }
                        break;
                    #endregion
                    #region Read
                    case 2: //Read
                        startTransmit(serialPort);
                        int[] data = printRead(serialPort);
                        Console.WriteLine("The Decoded packet is :");
                        foreach (int a in data)
                        {
                            Console.WriteLine(a); //posts the decoded array one by one
                        }
                        //TEST
                        break;
                    #endregion
                    #region Record
                    case 3: //In Progress
                        int[,] arr;
                        arr = Record();     
                        for (int i =0; i< arr.GetLength(0); i++)
                        {
                            for (int j = 0; j < arr.GetLength(1); j++)
                            {
                                Console.Write(arr[i, j] + " ");
                            }
                            Console.WriteLine();
                        }
                        break;
                    #endregion
                    #region Stop
                    case 4: //Stop Play
                        dataToSend = new byte[] { 0x10, 0 };
                        try
                        {
                            serialPort.Write(dataToSend, 0, dataToSend.Length);
                            Console.WriteLine("Stopped Reading");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        break;
                    #endregion
                    #region Disconnect
                    case 5: //Disconnect
                        try
                        {
                            serialPort.Dispose();
                            if (!serialPort.IsOpen)
                            {
                                Console.WriteLine("Disconnected");
                            }
                        }
                        catch (Exception e) { Console.WriteLine(e.Message); }
                        break;
                    #endregion
                    #region LED Change
                    case 6:
                        byte led=1;
                        Console.WriteLine("Enter LED Color Number: (0 - Green , 1 - Red)");
                        led = byte.Parse(Console.ReadLine());
                        dataToSend = new byte[] { 0x20, led };
                        try
                        {
                            serialPort.Write(dataToSend, 0, dataToSend.Length);
                            Console.WriteLine("LED Changed");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    #endregion
                    #region Current Change 
                    case 7: //TEST
                        Console.WriteLine("Pick a value between 0 and 200: YTESTY");
                        int value = int.Parse(Console.ReadLine());
                        int calcValue = (int)(Math.Round((value * 126) / 198.5));
                        //byte hexValue = Convert.ToByte(calcValue.ToString("X"),16);
                        for (int i = 0; i < 5; i++)
                        {
                            Console.WriteLine("the values being sent to the device are");
                            Console.WriteLine("{0} :  {1}",i, calcValue);
                            sendCommand(new byte[] {(byte)(i), (byte)calcValue});
                        }
                        serialPort.DiscardInBuffer();
                        break;
                    #endregion
                    #region Resistance Change
                    case 8: //In Progress
                        
                        
                        break;
                    #endregion
                    default:
                        break;
                }
                operation = int.Parse(Console.ReadLine());
            } while (operation!=0);

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
            void startTransmit(SerialPort serialPort)
            {                
                byte[] dataToSend = new byte[] { 0x10, 1 };
                sendCommand(dataToSend);                
            }

            int[] printRead(SerialPort serialPort)
            {                
                try
                {
                    int bytesRead = serialPort.Read(buffer, 0, bytesToRead);
                    foreach (byte b in buffer)
                    {
                        Console.WriteLine(b);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                int[] data = decode(buffer);
                return data;
            }
            int[] read(SerialPort serialPort)
            {
                try
                {
                    int bytesRead = serialPort.Read(buffer, 0, bytesToRead);                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                int[] data = decode(buffer);
                return data;
            }
            int[,] Record()
            {
                int recTime = 0;
                Console.WriteLine("How many seconds would like to record ?");
                recTime = int.Parse(Console.ReadLine());
                int[,] data = new int[recTime * 100,6];
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    data[i, 0] = i+1; // FIX - Change to 0.01[s] jumps
                    for (int j = 1; j < 6; j++)
                    {
                        int [] arr = read(serialPort);
                        data[i,j] = arr[j-1];
                    }
                }
                return data;                
            }

            void sendCommand(byte[] dataToSend)
            {
                try
                {
                    serialPort.Write(dataToSend, 0, dataToSend.Length);
                    Console.WriteLine("Operation Successful");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }
    }
}
