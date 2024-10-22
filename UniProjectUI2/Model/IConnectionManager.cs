
namespace Application.Model
{
	internal interface IConnectionManager
	{
		//Business Logic
		void Connect(string portName);
        void Disconnect(); //Disconnects from port
        void Initiliaze(); //Opens a Connection
        void DisposeConnection(); //Closes Connection
        void StartDataTransfer();
        void StopDataTransfer();
        void SendCommand(byte[] data); //Sends Command to the device
        int ReceiveData(DataPacket buffer, int PacketsToRead); //Recevies number of bytes read
        void EmptyIncomingDataBuffer();
        bool IsConnected();
        static abstract IConnectionManager GetInstance();

        //Testing
        bool AvailableData();

    }
}