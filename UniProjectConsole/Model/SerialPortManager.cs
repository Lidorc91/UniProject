using System;
using System.IO.Ports;

namespace Application.Model
{
    class SerialPortManager : ConnectionManager
    {
        private static SerialPortManager _instance;

        private SerialPort _serialPort;
        private String _port {  get; set; }

        public SerialPortManager()
        {
            this._serialPort = new SerialPort();

            //Serial Port settings
            this._serialPort.BaudRate = 115200;
            this._serialPort.DataBits = 8;
            this._serialPort.Parity = Parity.None;
            this._serialPort.StopBits = StopBits.One;
            this._serialPort.Handshake = Handshake.None;
        }

        public override ConnectionManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SerialPortManager();
            }
            return _instance;
        }

        public override void Connect(string port)
        {
            this._port = port;
            this._serialPort.PortName = this._port;
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
        }

        public override void Disconnect()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        public override void SendData(Packet buffer)
        {
            _serialPort.Write(buffer.getData(), 0, Packet.PACKET_SIZE);
        }

        public override int ReceiveData(Packet buffer, int PacketsToRead)
        {
            return _serialPort.Read(buffer.getData(), 0, PacketsToRead * Packet.PACKET_SIZE);
        }

        public override void EmptyIncomingDataBuffer()
        {
            _serialPort.DiscardInBuffer();
        }

        public override bool IsConnected() { return _serialPort.IsOpen; }
    }
}
