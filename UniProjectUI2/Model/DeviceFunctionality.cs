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
        Packet copyPacket = new Packet();

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
            _serialPortManager.EmptyIncomingDataBuffer();
        }

        private Packet ReadPacket()
        {
            byte[] buffer = new byte[Packet.PACKET_SIZE];
            Packet packet = new Packet(buffer);
            int bytesRead = _serialPortManager.ReceiveData(packet, Packet.PACKET_SIZE);
            return (bytesRead == 0) ? null : packet;
        }
    }
}