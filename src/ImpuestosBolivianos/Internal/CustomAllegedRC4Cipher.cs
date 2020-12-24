using System;
using System.Text;

namespace ImpuestosBolivianos.Internal
{
    internal static class CustomAllegedRC4Cipher
    {
        public static string Encode(string message, string key)
        {
            var state = new int[256];
            int x = 0;
            int y = 0;
            int index1 = 0;
            int index2 = 0;
            int nmen;

            for (int i = 0; i <= 255; i++)
            {
                state[i] = i;
            }

            for (int i = 0; i <= 255; i++)
            {
                index2 = (Convert.ToInt32(key[index1]) + state[i] + index2) % 256;
                SwapValues(ref state[i], ref state[index2]);
                index1 = (index1 + 1) % key.Length;
            }

            var resultStringBuilder = new StringBuilder();
            for (int i = 0, loopTo = message.Length - 1; i <= loopTo; i++)
            {
                x = (x + 1) % 256;
                y = (state[x] + y) % 256;
                SwapValues(ref state[x], ref state[y]);
                nmen = Convert.ToInt32(message[i]) ^ state[(state[x] + state[y]) % 256];
                resultStringBuilder.Append("-" + ToHexValue(nmen));
            }

            return resultStringBuilder.ToString().Substring(1);
        }

        private static void SwapValues(ref int left, ref int right)
        {
            int tmp = left;
            left = right;
            right = tmp;
        }

        private static string ToHexValue(int value)
        {
            return Convert.ToString(value, 16).ToUpper().PadLeft(2, '0');
        }
    }
}
