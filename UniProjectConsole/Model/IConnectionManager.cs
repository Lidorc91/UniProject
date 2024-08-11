
namespace Application.Model
{
	//TODO - ADD Singleton Implementation inside abstract class
	internal interface IConnectionManager		
	{
		//Business Logic
		void Connect(string portName);
		void Disconnect();
		void SendData(Packet data);
		int ReceiveData(Packet buffer, int PacketsToRead);
		void EmptyIncomingDataBuffer();
		bool IsConnected();
	}
}