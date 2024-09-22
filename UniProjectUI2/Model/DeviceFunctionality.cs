using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Model
{
    partial class DeviceManager
    {
        //Recording Variables
        BlockingCollection<Packet> recordQueue;
        private bool isRecording = false;
        private int recordPacketsToRead;
        private int recTime;
        private Packet latestPacket;
        private readonly object _lock = new object();
        private Timer realTimeTimer;
        private Packet realTimePacket;

        private setupRealTimeTimer()
        {
            realTimeTimer = new Timer(100);
            realTimeTimer.Elapsed += realtimeThread;
            realTimeTimer.AutoReset = true;            
        }

        private void ChangeLed()
        {
            byte led = 1;
            Console.WriteLine("Enter LED Color Number: (0 - Green , 1 - Red)");
            led = byte.Parse(Console.ReadLine());
            byte[] dataToSend = new byte[] { 0x20, led };
            sendCommand(dataToSend);
        }
        private void ChangeCurrent()
        {
            Console.WriteLine("Pick a value between 0 and 200:");
            int value = int.Parse(Console.ReadLine());
            int calcValue = (int)(Math.Round((value * 126) / 198.5));
            for (int i = 0; i < 5; i++)
            {
                sendCommand(new byte[] { (byte)(i), (byte)calcValue });
            }
            _connection.EmptyIncomingDataBuffer();
        }

        //Continuous read 
        private Packet getLatestPacket()
        {
            lock (_lock)
            {

            }
            Packet packet = new Packet();
            int bytesRead = _connection.ReceiveData(packet, Packet.PACKET_SIZE);
            packet.getDecodedData(); // Data processing in the model
            packet.clearRawData();
            return (bytesRead == 0) ? null : packet;
        }

        private int[] realtimeThread()
        {
            //Case for Active Recording 
            if(isRecording){
                //Get Packet
                lock (_lock){
                  realTimePacket = latestPacket;
                }
                //Decode Packet
                realTimePacket.decode();
                return;
            }else{
                _connection.EmptyIncomingDataBuffer();
                Thread.sleep(10);
                //Get Packet
                realTimePacket = ReceiveData(new Packet(),1);
                //Decode packet
                realTimePacket.decode();
            }
        }

        //Record Functionality
        public void record(byte time)
        {
            recordQueue = new ConcurrentQueue<Packet>;
            int packetsToRead = 100*time;
            isRecording = true;
            Packet recordPacket = new Packet();
            
            //Record
            Thread recordThread = new Thread(delegate(){
                while(packetsToRead > 0){
                    //Read
                    ReceiveData(recordPacket,1);
                    //Save to Shared Packet
                    lock (_lock){
                    latestPacket = recordPacket;
                   }
                   //Save Data
                    recordQueue.Enqueue(new Packet(recordPacket));
                    
                   --packetsToRead;                    
                }
                
                isRecording = false;            
            }).start();
            
            //Process & Export
            Thread exportThread = new Thread(delegate(){
                for(int i=0; i < 100*time ; i++){                    
                    using (StreamWriter file = new StreamWriter("2dArrayOut.csv"))
                    {
                        file.AutoFlush = true;
                        for (int i = 0; i < 100*time; i++)
                        {
                            //Process Data
                            int[] processedData = recordQueue.Dequeue().getDecodedData();
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
            setupRealTimeTimer();
            realTimeTimer.Enabled = true;
        }
        
        public void stopRealTimeReading(){
            realTimeTimer.Enabled = false;
        }       
    }
}
