using System;
using System.IO.Ports;

namespace Application.Model
{
    class SerialPortManager
    {
        private SerialPort _serialPort;
        private String _port {  get; set; }

        public SerialPortManager(String port)
        {
            this._port = port;
            this._serialPort = new SerialPort(port);

            //Serial Port settings
            this._serialPort.BaudRate = 115200;
            this._serialPort.DataBits = 8;
            this._serialPort.Parity = Parity.None;
            this._serialPort.StopBits = StopBits.One;
            this._serialPort.Handshake = Handshake.None;
        }


        public void OpenPort()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
        }

        public bool isOpen() { return _serialPort.IsOpen; }

        public void ClosePort()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        public void EmptyIncomingDataBuffer()
        {
            _serialPort.DiscardInBuffer();
        }

        public int Read(byte[] buffer, int offset, int maxBytesToRead)
        {
            return _serialPort.Read(buffer, offset, maxBytesToRead);
        }

        public void Write(byte[] buffer)
        {
            _serialPort.Write(buffer, 0, buffer.Length);
        }

    }
}
