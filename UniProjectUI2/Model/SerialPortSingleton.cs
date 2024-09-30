using System;
using System.IO.Ports;

namespace Application.Model
{
    partial class SerialPortManager : IConnectionManager
    {
        private static SerialPortManager _instance;

        private SerialPortManager()
        {
            Initiliaze();
            DefineConnectionSettings(this._serialPort);

            //TODO - Change port selection location later - In UI to DeviceManager
            this._port = "COM3";
            Connect(this._port);
        }

        public static IConnectionManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SerialPortManager();
            }
            return _instance;
        }
    }
}