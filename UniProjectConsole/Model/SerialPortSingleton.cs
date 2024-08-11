using System;
using System.IO.Ports;

namespace Application.Model
{
    partial class SerialPortManager : IConnectionManager
    {
        private static SerialPortManager _instance;

        private SerialPort _serialPort;
        private String _port { get; set; }

        private SerialPortManager()
        {
            this._serialPort = new SerialPort();

            //Serial Port settings
            this._serialPort.BaudRate = 115200;
            this._serialPort.DataBits = 8;
            this._serialPort.Parity = Parity.None;
            this._serialPort.StopBits = StopBits.One;
            this._serialPort.Handshake = Handshake.None;
        }

        public SerialPortManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SerialPortManager();
            }
            return _instance;
        }
    }
}