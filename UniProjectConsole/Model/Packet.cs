using System.Text;

namespace Application.Model
{
    class Packet
    {
        public const int PACKET_SIZE = 22;
        public const int PD_SIZE = 5;
        private byte[] _data;


        public Packet()
        {
            _data = new byte[PACKET_SIZE];
        }

        public Packet(byte[] data)
        {
            _data = data;
        }

        public byte[] getData()
        {
            return _data;
        }

        public void setData(byte[] sourceData, int length)
        {
            Array.Copy(sourceData, _data, length);
        }

        public int[] Decode()
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

    }
}