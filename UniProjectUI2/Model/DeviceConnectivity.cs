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

        public DeviceManager()
        {
            _connection = GetInstance();
            setupTimers();
        }

        //Initialize Relevant Connectivity (SerialPort / Bluetooth / etc.)
        private SerialPortManager GetInstance()
        {
            return (SerialPortManager)SerialPortManager.GetInstance();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void Connect(String port)
        {
            _connection.Connect(port);
        }

        public void Disconnect()
        {
            _connection.Disconnect();
        }

        void sendCommand(byte[] dataToSend)
        {
            if (!_connection.IsConnected())
            {
                return;
            }

            try
            {
                Packet packet = new Packet(dataToSend);
                _connection.SendCommand(packet);
                Thread.Sleep(250);
                Console.WriteLine("Operation Successful");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private void StartDataTransmission()
        {
            byte[] dataToSend = new byte[] { 0x10, 1 };
            sendCommand(dataToSend);
        }
        private void StopDataTransmission()
        {
            byte[] dataToSend = new byte[] { 0x10, 0 };
            sendCommand(dataToSend);
        }

    }
}
