using System;

namespace ImpuestosBolivianos
{
    internal static class CustomBase64Encoder
    {
        private static readonly char[] BaseSymbols = new char[64]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
            'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
            'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
            'y', 'z', '+', '/',
        };

        public static string Encode(long value)
        {
            string result = string.Empty;

            long quotient = 1L;
            int remainder;
            while (quotient > 0L)
            {
                quotient = value / 64L;
                remainder = Convert.ToInt32(value % 64L);
                result = BaseSymbols[remainder].ToString() + result;
                value = quotient;
            }

            return result;
        }
    }
}
