using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Timers;
using System.IO;

namespace Application.Model
{
    partial class DeviceManager
    {
        //General Device Variable
        private bool activeDataTransfer = false;

        //Recording Variables
        private volatile bool isRecording = false;
        BlockingCollection<Packet> recordQueue = new BlockingCollection<Packet>();
        private readonly object _lock = new object();
        private volatile int recordPacketsToRead;
        private Packet latestPacket;

        //Real-Time Variables
        private volatile bool isRTReading = false;
        private Packet _realTimePacket;
        public Packet realTimePacket => _realTimePacket;

        //Timers Variables
        private System.Timers.Timer realTimeTimer;
        private System.Timers.Timer recordTimer;

        private void setupTimers()
        {
            realTimeTimer = new System.Timers.Timer(100);
            realTimeTimer.Elapsed += realtimeThread;
            realTimeTimer.AutoReset = true;

            recordTimer = new System.Timers.Timer(10);
            recordTimer.Elapsed += recordThread;
            recordTimer.AutoReset = true;          
        }

        public void ChangeLed(string color)
        {
            switch (color)
            {
                case "Green":
                    sendCommand(new byte[] { 0x20, 0 });
                    break;
                case "Red":
                    sendCommand(new byte[] { 0x20, 1 });
                    break;
                default:
                    break;
            }
        }
        public void ChangeCurrent(byte value)
        {
            int calcValue = (int)(Math.Round((value * 126) / 198.5));
            for (int i = 0; i < 5; i++)
            {
                sendCommand(new byte[] { (byte)(i), (byte)calcValue });
            }
            //_connection.EmptyIncomingDataBuffer();
        }

        private void realtimeThread(object sender, ElapsedEventArgs e)
        {
            Packet TempPacket;
            //Case 1 - Active Recording 
            if(isRecording){
                //Get Packet from record thread
                lock (_lock){
                  _realTimePacket = new Packet(latestPacket.getRawData());
                }
            //Case 2 - Real Time Reading ONLY
            }
            else{
                _connection.EmptyIncomingDataBuffer();
                Thread.Sleep(10);
                //Get Packet from connection manager
                _realTimePacket = new Packet();
                _connection.ReceiveData(_realTimePacket, 1);
            }
            //Decode packet
            _realTimePacket.decode();
            NotifyPropertyChanged("realTimePacket");
        }

        private void recordThread(object sender, ElapsedEventArgs e)
        {
            Packet recordPacket = new Packet();
            if(recordPacketsToRead > 0){
                    //Read
                    _connection.ReceiveData(recordPacket,1);
                    //Save to Shared Packet
                    lock (_lock){
                    latestPacket = recordPacket;
                   }
                   //Save Data
                    recordQueue.Add(new Packet(recordPacket.getRawData()));
                   //Decrement record counter
                   --recordPacketsToRead;                    
                }else{
                    isRecording = false;
                    recordTimer.Enabled = false;
                    if(!isRTReading) _connection.StopDataTransfer();
                }                
        }


        //Record Functionality
        public void record(byte time)
        {
            if(!activeDataTransfer) _connection.StartDataTransfer();
            recordPacketsToRead = 100*time;
            isRecording = true;
            recordTimer.Enabled = true;

            //Process & Export
            Thread exportThread = new Thread(delegate ()
            {
                string filePath = System.IO.Path.GetFullPath($"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv");
                using (StreamWriter file = new StreamWriter(filePath))
                {
                    file.AutoFlush = true;
                    for (int i = 0; i < 100 * time; i++)
                    {
                        //Process Data
                        int[] processedData = recordQueue.Take().getDecodedData();
                        for (int j = 0; j < Packet.PD_SIZE; j++)
                        {
                            //Save to CSV
                            file.Write($"{processedData[j]},");
                        }
                        file.Write("\n");
                    }
                }
            });
            exportThread.Start();
        }

        public void startRealTimeReading(){
            isRTReading = true;
            if(!activeDataTransfer) _connection.StartDataTransfer();
            realTimeTimer.Enabled = true;

        }
        
        public void stopRealTimeReading(){
            realTimeTimer.Enabled = false;
            if(!isRecording) _connection.StopDataTransfer();
            isRTReading = false;
        }       
    }
}
