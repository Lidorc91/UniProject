
namespace Application.Model
{
	internal interface IPacket
	{
		byte[] GetRawData();
		int[] GetProcessedData();
	}
}