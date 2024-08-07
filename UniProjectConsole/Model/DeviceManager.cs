using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Model
{
    class DeviceManager : INotifyPropertyChanged
    {
        private SerialPortManager _serialPortManager;

        //Recording Variables
        Queue<Packet> recordQueue;
        private int recordPacketsToRead;
        private int recTime;
        Packet copyPacket = new Packet();

        public DeviceManager()
        {

        }

        public void InitializeSerialPort(String port)
        {
            if (_serialPortManager == null)
            {
                _serialPortManager = new SerialPortManager(port);
                _serialPortManager.OpenPort();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void Connect(String port)
        {
            _serialPortManager = new SerialPortManager(port);
            _serialPortManager.OpenPort();
        }
        public void Disconnect()
        {
            _serialPortManager.ClosePort();
        }

        void sendCommand(byte[] dataToSend)
        {
            if (!_serialPortManager.isOpen())
            {
                return;
            }

            try
            {
                _serialPortManager.Write(dataToSend);
                Thread.Sleep(250);
                Console.WriteLine("Operation Successful");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        void startTransmit()
        {
            byte[] dataToSend = new byte[] { 0x10, 1 };
            sendCommand(dataToSend);
        }
        void stopTransmit()
        {
            byte[] dataToSend = new byte[] { 0x10, 0 };
            sendCommand(dataToSend);
        }

        private void changeLed()
        {
            byte led = 1;
            Console.WriteLine("Enter LED Color Number: (0 - Green , 1 - Red)");
            led = byte.Parse(Console.ReadLine());
            byte[] dataToSend = new byte[] { 0x20, led };
            sendCommand(dataToSend);
        }
        private void changeCurrent()
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

        private Packet readPacket()
        {
            byte[] buffer = new byte[Packet.PACKET_SIZE];
            int bytesRead = _serialPortManager.Read(buffer, 0, Packet.PACKET_SIZE);
            return (bytesRead == 0) ? null : new Packet(buffer);
        }

    }
}
