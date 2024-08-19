using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Model
{
    partial class DeviceManager : INotifyPropertyChanged
    {
        private IConnectionManager _connection;
        private SerialPortManager _serialPortManager;

        public DeviceManager()
        {
            //Initialize Relevant Connectivity (Serial Port / Bluetooth)
            //_connection = new ConnectionManager(new SerialPortManager());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void Initialize(String port)
        {
            if (_serialPortManager == null)
            {
                _serialPortManager = SerialPortManager.GetInstance();
                _serialPortManager.Connect(port);
            }
        }

        public void Connect(String port)
        {
            _connection.Connect(port);
            //_serialPortManager = new SerialPortManager(port);
            //_serialPortManager.OpenPort();
        }
        public void Disconnect()
        {
            _serialPortManager.Disconnect();
        }

        void sendCommand(byte[] dataToSend)
        {
            if (!_serialPortManager.IsConnected())
            {
                return;
            }

            try
            {
                Packet packet = new Packet(dataToSend);
                _serialPortManager.SendData(packet);
                Thread.Sleep(250);
                Console.WriteLine("Operation Successful");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        void StartDataTransmission()
        {
            byte[] dataToSend = new byte[] { 0x10, 1 };
            sendCommand(dataToSend);
        }
        void StopDataTransmission()
        {
            byte[] dataToSend = new byte[] { 0x10, 0 };
            sendCommand(dataToSend);
        }

    }
}
