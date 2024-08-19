using System;
using System.IO.Ports;

namespace Application.Model
{
    partial class SerialPortManager
    {
        /*  Variable Declaration  */
        private SerialPort _serialPort;
        private String _port { get; set; }

        /*  Connection Methods  */
        private void DefineConnectionSettings(SerialPort _serialPort)
        {
            this._serialPort.BaudRate = 115200;
            this._serialPort.DataBits = 8;
            this._serialPort.Parity = Parity.None;
            this._serialPort.StopBits = StopBits.One;
            this._serialPort.Handshake = Handshake.None;
        }

        public void Connect(string port)
        {
            this._port = port;
            this._serialPort.PortName = this._port;
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
        }

        public void Disconnect()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        public void Initiliaze()
        {
            if (_serialPort == null)
            {
                _serialPort = new SerialPort();
                DefineConnectionSettings(_serialPort);
            }
        }

        public void Dispose()
        {
            if (_serialPort != null)
            {
                _serialPort.Dispose();
            }
        }

        /*  Data Management Methods  */
        public void SendData(Packet buffer)
        {
            _serialPort.Write(buffer.getData(), 0, Packet.PACKET_SIZE);
        }

        public int ReceiveData(Packet buffer, int PacketsToRead)
        {
            return _serialPort.Read(buffer.getData(), 0, PacketsToRead * Packet.PACKET_SIZE);
        }

        public void EmptyIncomingDataBuffer()
        {
            _serialPort.DiscardInBuffer();
        }

        public bool IsConnected() { return _serialPort.IsOpen; }
    }
}
