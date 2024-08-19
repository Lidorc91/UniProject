
namespace Application.Model
{
	internal interface IConnectionManager
	{
		//Business Logic
		void Connect(string portName);
        void Disconnect(); //Disconnects from port
        void Initiliaze(); //Opens a Connection
        void Dispose(); //Closes Connection
        void SendData(Packet data);
        int ReceiveData(Packet buffer, int PacketsToRead);
        void EmptyIncomingDataBuffer();
        bool IsConnected();
        static abstract IConnectionManager GetInstance();
	}
}