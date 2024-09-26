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