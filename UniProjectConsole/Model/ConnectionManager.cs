
namespace Application.Model
{
	//TODO - ADD Singleton Implementation inside abstract class
	abstract class ConnectionManager		
	{
		private ConnectionManager() { }

		public abstract ConnectionManager GetInstance();

		//Business Logic
		public abstract void Connect(string portName);
		public abstract void Disconnect();
		public abstract void SendData(Packet data);
		public abstract int ReceiveData(Packet buffer, int PacketsToRead);
		public abstract void EmptyIncomingDataBuffer();
		public abstract bool IsConnected();
	}

}