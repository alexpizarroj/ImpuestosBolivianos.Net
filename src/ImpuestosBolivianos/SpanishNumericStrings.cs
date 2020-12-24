using System;
using System.Text;

namespace ImpuestosBolivianos
{
    internal static class SpanishNumericStrings
    {
        private static readonly string[] UnitsPlusString = new string[]
        {
            string.Empty,
            "un", "dos", "tres", "cuatro", "cinco",
            "seis", "siete", "ocho", "nueve", "diez",
            "once", "doce", "trece", "catorce", "quince",
            "dieciséis", "diecisiete", "dieciocho", "diecinueve", "veinte",
        };

        private static readonly string[] TensString = new string[]
        {
            string.Empty,
            "diez", "veinti", "treinta", "cuarenta", "cincuenta",
            "sesenta", "setenta", "ochenta", "noventa", "cien",
        };

        private static readonly string[] HundredsString = new string[]
        {
            string.Empty,
            "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos",
            "seiscientos", "setecientos", "ochocientos", "novecientos",
        };

        // *
        // * RAE's Spanish Cardinal Numerals Specification (seen on 9/17/2017):
        // * http://lema.rae.es/dpd/srv/search?key=cardinales&origen=RAE&lema=cardinales
        // *
        public static string ToCardinal(ulong value, bool apocopate = false, bool useVerboseBusinessMode = false)
        {
            if (value == 0m)
                return "cero";
            int nTrillones = Convert.ToInt32(value / 1_000_000_000_000_000_000UL);
            value -= Convert.ToUInt64(nTrillones) * 1_000_000_000_000_000_000UL;
            int nBillones = Convert.ToInt32(value / 1_000_000_000_000UL);
            value -= Convert.ToUInt64(nBillones) * 1_000_000_000_000UL;
            int nMillones = Convert.ToInt32(value / 1_000_000UL);
            value -= Convert.ToUInt64(nMillones) * 1_000_000UL;
            int nMiles = Convert.ToInt32(value / 1000UL);
            value -= Convert.ToUInt64(nMiles) * 1000UL;
            int nCentenas = Convert.ToInt32(value / 100UL);
            value -= Convert.ToUInt64(nCentenas) * 100UL;
            var nDecenas = default(int);
            if (value > 20m)
            {
                nDecenas = Convert.ToInt32(value / 10UL);
                value -= Convert.ToUInt64(nDecenas) * 10UL;
            }

            var resultBuilder = new StringBuilder();
            if (nTrillones > 0)
            {
                resultBuilder.Append(ToCardinal(Convert.ToUInt64(nTrillones), true, useVerboseBusinessMode));
                resultBuilder.Append(nTrillones > 1 ? " trillones " : " trillón ");
            }

            if (nBillones > 0)
            {
                resultBuilder.Append(ToCardinal(Convert.ToUInt64(nBillones), true, useVerboseBusinessMode));
                resultBuilder.Append(nBillones > 1 ? " billones " : " billón ");
            }

            if (nMillones > 0)
            {
                resultBuilder.Append(ToCardinal(Convert.ToUInt64(nMillones), true, useVerboseBusinessMode));
                resultBuilder.Append(nMillones > 1 ? " millones " : " millón ");
            }

            if (nMiles > 0)
            {
                if (nMiles >= 2 | useVerboseBusinessMode)
                {
                    resultBuilder.Append(ToCardinal(Convert.ToUInt64(nMiles), true, useVerboseBusinessMode));
                    resultBuilder.Append(" mil ");
                }
                else
                {
                    resultBuilder.Append("mil ");
                }
            }

            if (nCentenas > 0)
            {
                if (nCentenas == 1 & nDecenas == 0 & value == 0m)
                {
                    resultBuilder.Append("cien");
                }
                else
                {
                    resultBuilder.Append(HundredsString[nCentenas]);
                    resultBuilder.Append(" ");
                }
            }

            if (nDecenas > 0)
            {
                resultBuilder.Append(TensString[nDecenas]);
                if (nDecenas >= 3 & value > 0m)
                {
                    resultBuilder.Append(" y ");
                }
            }

            // Default case: 1 <= value <= 20
            if (value > 0m)
            {
                if (nDecenas == 2 & value == 1m & apocopate)
                {
                    resultBuilder.Append("ún");
                }
                else if (nDecenas == 2 & value == 2m)
                {
                    resultBuilder.Append("dós");
                }
                else if (nDecenas == 2 & value == 3m)
                {
                    resultBuilder.Append("trés");
                }
                else if (nDecenas == 2 & value == 6m)
                {
                    resultBuilder.Append("séis");
                }
                else
                {
                    resultBuilder.Append(UnitsPlusString[Convert.ToInt32(value)]);
                }

                if (value == 1m & !apocopate)
                {
                    resultBuilder.Append("o");
                }
            }

            return resultBuilder.ToString().Trim();
        }
    }
}
