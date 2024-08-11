using System;
using System.IO.Ports;

namespace Application.Model
{
    partial class SerialPortManager
    {
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
