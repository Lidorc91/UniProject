using System.Text;

namespace Application.Model
{
    class DataPacket
    {
        public const int PACKET_SIZE = 22;
        public const int PD_SIZE = 5;
        private byte[] _rawData;
        private int[] _processedData;

        public DataPacket()
        {
            _rawData = new byte[PACKET_SIZE];
        }

        public DataPacket(byte[] data)
        {
            _rawData = data;
        }

        public byte[] GetRawData()
        {
            //if(_rawData == null) return null;
            return _rawData;
        }

        public void setRawData(byte[] sourceData, int length)
        {
            Array.Copy(sourceData,0 ,_rawData,0, length);
        }

        public int[] GetProcessedData()
        {
            if(_processedData == null){
                decode();                                
            }
            return _processedData;
        }

        public void decode()
        {
            if(_rawData == null) return;

            _processedData = new int[PD_SIZE];
            string[] hexString = new string[PACKET_SIZE];
            for (int i = 1; i < _processedData.Length * 4; i += 4) //run 5 times for the 5 PDs - takes into accout the prefix and suffix bytes
            {
                string a1 = _rawData[i].ToString("X").PadLeft(2, '0'); //this is the first number
                string a2 = _rawData[i + 1].ToString("X").PadLeft(2, '0'); //this is the 2nd number
                                                                    //numbers 3 & 4 are always 0 so they are ignored
                StringBuilder concatenatedHex = new StringBuilder();
                concatenatedHex.Append(a2);
                concatenatedHex.Append(a1); //this mean thats the 2nd number is the first element of the decoded number
                _processedData[(i - 1) / 4] = Convert.ToInt32(concatenatedHex.ToString(), 16); //convert back to dec
            }

            //PD Switch
            _processedData[2] += _processedData[3];
            _processedData[3] = _processedData[2] - _processedData[3];
            _processedData[2] -= _processedData[3];

            _processedData[3] += _processedData[4];
            _processedData[4] = _processedData[3] - _processedData[4];
            _processedData[3] -= _processedData[4];            
        }

        //Discard raw data after decoding to save memory
        public void clearRawData()
        {
            _rawData = null;
        }

    }
}