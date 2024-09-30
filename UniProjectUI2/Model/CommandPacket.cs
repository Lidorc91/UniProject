using System.Text;

namespace Application.Model
{
    class CommandPacket : IPacket
    {
        public const int PACKET_SIZE = 8;
        private byte[] _rawData;

        public CommandPacket(byte[] data)
        {
            _rawData = data;
        }

        public byte[] GetRawData()
        {
            return _rawData;
        }
        public int[] GetProcessedData() { return null; }
    }
}