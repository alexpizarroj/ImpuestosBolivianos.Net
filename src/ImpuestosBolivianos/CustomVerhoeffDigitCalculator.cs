namespace ImpuestosBolivianos
{
    internal static class CustomVerhoeffDigitCalculator
    {
        private static readonly int[,] VerhoeffMul = new int[10, 10]
        {
            { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
            { 1, 2, 3, 4, 0, 6, 7, 8, 9, 5 },
            { 2, 3, 4, 0, 1, 7, 8, 9, 5, 6 },
            { 3, 4, 0, 1, 2, 8, 9, 5, 6, 7 },
            { 4, 0, 1, 2, 3, 9, 5, 6, 7, 8 },
            { 5, 9, 8, 7, 6, 0, 4, 3, 2, 1 },
            { 6, 5, 9, 8, 7, 1, 0, 4, 3, 2 },
            { 7, 6, 5, 9, 8, 2, 1, 0, 4, 3 },
            { 8, 7, 6, 5, 9, 3, 2, 1, 0, 4 },
            { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 },
        };

        private static readonly int[,] VerhoeffPer = new int[8, 10]
        {
            { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
            { 1, 5, 7, 6, 2, 8, 3, 0, 9, 4 },
            { 5, 8, 0, 3, 7, 9, 6, 1, 4, 2 },
            { 8, 9, 1, 6, 0, 4, 3, 5, 2, 7 },
            { 9, 4, 5, 3, 1, 2, 6, 8, 7, 0 },
            { 4, 2, 8, 6, 5, 7, 3, 9, 0, 1 },
            { 2, 7, 9, 3, 8, 0, 6, 4, 1, 5 },
            { 7, 0, 4, 6, 9, 1, 3, 2, 5, 8 }
        };

        private static readonly int[] VerhoeffInv = new int[10] { 0, 4, 3, 2, 1, 5, 6, 7, 8, 9 };

        public static int Get(int value)
        {
            return Get(value.ToString());
        }

        public static int Get(string value)
        {
            int check = 0;
            for (int i = 0, loopTo = value.Length - 1; i <= loopTo; i++)
            {
                int x = (i + 1) % 8;
                int y = int.Parse(value[value.Length - 1 - i].ToString());
                check = VerhoeffMul[check, VerhoeffPer[x, y]];
            }
            return VerhoeffInv[check];
        }
    }
}
