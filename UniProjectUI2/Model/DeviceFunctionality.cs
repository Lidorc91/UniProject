using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;


namespace Application.Model
{
    partial class DeviceManager
    {
        //General Device Variable
        private bool activeDataTransfer = false;

        //Recording Variables
        private volatile bool isRecording = false;
        Queue<Packet> recordQueue = new BlockingCollection<Packet>();
        private readonly object _lock = new object();
        private volatile int recordPacketsToRead;
        private Packet latestPacket;

        //Real-Time Variables
        private volatile bool isRTReading = false;
        private Packet _realTimePacket;
        public Packet realTimePacket => _realTimePacket;

        //Timers Variables
        private Timer realTimeTimer;
        private Timer recordTimer;

        private setupTimers()
        {
            realTimeTimer = new Timer(100);
            realTimeTimer.Elapsed += realtimeThread;
            realTimeTimer.AutoReset = true;

            recordTimer = new Timer(100);
            realTimeTimer.Elapsed += recordThread;
            realTimeTimer.AutoReset = true;          
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
        public void ChangeCurrent(int value)
        {
            int calcValue = (int)(Math.Round((value * 126) / 198.5));
            for (int i = 0; i < 5; i++)
            {
                sendCommand(new byte[] { (byte)(i), (byte)calcValue });
            }
            //_connection.EmptyIncomingDataBuffer();
        }

        private void realtimeThread()
        {
            Packet TempPacket;
            //Case 1 - Active Recording 
            if(isRecording){
                //Get Packet from record thread
                lock (_lock){
                  _realTimePacket = new Packet(latestPacket);
                }
            //Case 2 - Real Time Reading ONLY
            }
            else{
                _connection.EmptyIncomingDataBuffer();
                Thread.sleep(10);
                //Get Packet from connection manager
                _realTimePacket = ReceiveData(new Packet(),1);                
            }
            //Decode packet
            _realTimePacket.decode();
            NotifyPropertyChanged("realTimePacket");
        }

        private void recordThread()
        {
            Packet recordPacket = new Packet();
            if(recordPacketsToRead > 0){
                    //Read
                    ReceiveData(recordPacket,1);
                    //Save to Shared Packet
                    lock (_lock){
                    latestPacket = recordPacket;
                   }
                   //Save Data
                    recordQueue.Add(new Packet(recordPacket));
                   //Decrement record counter
                   --packetsToRead;                    
                }else{
                    isRecording = false;
                    recordTimer.Enabled = false;
                    if(!isRTReading) StopDataTransfer();
                }                
        }


        //Record Functionality
        public void record(byte time)
        {
            if(!activeDataTransfer) StartDataTransfer();
            recordPacketsToRead = 100*time;
            isRecording = true;
            recordTimer.Enabled = true;
            
            //Process & Export
            Thread exportThread = new Thread(delegate(){
                filePath = System.IO.Path.GetFullPath($"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv")
                for(int i=0; i < 100*time ; i++){                    
                    using (StreamWriter file = new StreamWriter(filePath))
                    {
                        file.AutoFlush = true;
                        for (int i = 0; i < 100*time; i++)
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
                }
            }).start();
        }

        public void startRealTimeReading(){
            isRTReading = true;
            if(!activeDataTransfer) StartDataTransfer();
            realTimeTimer.Enabled = true;

        }
        
        public void stopRealTimeReading(){
            realTimeTimer.Enabled = false;
            if(!isRecording) StopDataTransfer();
            isRTReading = false;
        }       
    }
}
