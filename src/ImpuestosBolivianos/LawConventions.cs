using System;
using System.Globalization;

namespace ImpuestosBolivianos
{
    internal class LawConventions
    {
        internal static class ControlCode
        {
            public static string StringifyDateTime(DateTime value)
            {
                return value.ToString("yyyyMMdd");
            }

            public static decimal RoundImporteTotal(decimal value)
            {
                return Math.Round(value, 0, MidpointRounding.AwayFromZero);
            }
        }

        internal static class QrControlCode
        {
            public static string StringifyDateTime(DateTime value)
            {
                return value.ToString("dd/MM/yyyy");
            }

            public static string StringifyDecimal(decimal value)
            {
                if (value <= 0m)
                {
                    return "0";
                }

                return value.ToString("0.00", ImpuestosNacionalesNFI);
            }

            private static NumberFormatInfo ImpuestosNacionalesNFI { get; }
                = new NumberFormatInfo()
                {
                    NumberDecimalSeparator = ".",
                    NumberGroupSeparator = "",
                    NumberDecimalDigits = 2,
                };
        }

        public static string StringifyInvoiceAmount(decimal amount)
        {
            ulong integerPart = Convert.ToUInt64(Math.Floor(amount));
            decimal decimals = Math.Round((amount - integerPart) * 100m, MidpointRounding.AwayFromZero);
            ulong cents = Convert.ToUInt64(decimals);

            string result = SpanishNumericStrings.ToCardinal(integerPart, true, true).ToUpper();
            result += $" {cents.ToString().PadLeft(2, '0')}/100";

            return result;
        }
    }
}
