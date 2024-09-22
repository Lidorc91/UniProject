using System.Text;

namespace Application.Model
{
    class Packet
    {
        public const int PACKET_SIZE = 22;
        public const int PD_SIZE = 5;
        private byte[] _rawData;
        private int[] _decodedData = new int[PD_SIZE];

        public Packet()
        {
            _rawData = new byte[PACKET_SIZE];
        }

        public Packet(byte[] data)
        {
            _rawData = data;
        }

        public byte[] getRawData()
        {
            return _rawData;
        }

        public void setRawData(byte[] sourceData, int length)
        {
            Array.Copy(sourceData, _rawData, length);
        }

        public int[] getDecodedData(byte[] data)
        {
            return decode(getRawData());
        }

        private int[] decode(byte[] _data)
        {

            int[] arr = new int[PD_SIZE]; // final decoded array
            string[] hexString = new string[PACKET_SIZE];
            for (int i = 1; i < arr.Length * 4; i += 4) //run 5 times for the 5 PDs - takes into accout the prefix and suffix bytes
            {
                string a1 = _data[i].ToString("X").PadLeft(2, '0'); //this is the first number
                string a2 = _data[i + 1].ToString("X").PadLeft(2, '0'); //this is the 2nd number
                                                                    //numbers 3 & 4 are always 0 so they are ignored
                StringBuilder concatenatedHex = new StringBuilder();
                concatenatedHex.Append(a2);
                concatenatedHex.Append(a1); //this mean thats the 2nd number is the first element of the decoded number
                arr[(i - 1) / 4] = Convert.ToInt32(concatenatedHex.ToString(), 16); //convert back to dec
            }

            //PD Switch
            arr[2] += arr[3];
            arr[3] = arr[2] - arr[3];
            arr[2] -= arr[3];

            arr[3] += arr[4];
            arr[4] = arr[3] - arr[4];
            arr[3] -= arr[4];

            return arr;
        }

        //Discard raw data after decoding to save memory
        public void clearRawData()
        {
            _rawData = null;
        }

    }
}