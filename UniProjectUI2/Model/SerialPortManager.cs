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

        public void StartDataTransfer(){
            byte[] command = new byte[] {0x10, 1};
            SendCommand(command);
        }
        public void StopDataTransfer(){
            byte[] command = new byte[] { 0x10, 0 };
            SendCommand(command);
        }

        public void DisposeConnection()
        {
            if (_serialPort != null)
            {
                _serialPort.Dispose();
            }
        }

        /*  Data Management Methods  */
        public void SendCommand(byte[] command)
        {
            _serialPort.Write(command, 0, command.Length);
        }

        public int ReceiveData(DataPacket buffer, int PacketsToRead)
        {
            return _serialPort.Read(buffer.GetRawData(), 0, PacketsToRead * DataPacket.PACKET_SIZE);
        }

        public void EmptyIncomingDataBuffer()
        {
            _serialPort.DiscardInBuffer();
        }

        public bool IsConnected() { return _serialPort.IsOpen; }
    }
}
