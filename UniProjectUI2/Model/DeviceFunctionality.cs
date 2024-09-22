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
        Queue<Packet> recordQueue;
        private int recordPacketsToRead;
        private int recTime;
        private Packet latestPacket;
        private readonly object _lock = new object();
        Packet copyPacket = new Packet();
        private Timer realTimeTimer;

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

        private void realtimeThread()
        {
            //Decode packet
        }

        private void recordThread()
        {
            //Save raw packets
        }

        //Record Functionality
        public void startRecord(byte time)
        {
            recordQueue = new Queue<Packet>;

        }
       
    }
}