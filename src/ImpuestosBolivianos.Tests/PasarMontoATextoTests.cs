using System.Globalization;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;
using Xunit;

namespace ImpuestosBolivianos.Tests
{
    public class PasarMontoATextoTests
    {
        [Theory]
        [CsvData(@"Data/PasarMontoATextoTCs.csv", typeof(TestArgs), typeof(TestArgsMapping))]
        public void PassesTestCases(TestArgs t)
        {
            var actual = Facturacion.PasarMontoATexto(t.Amount);

            Assert.Equal(t.ExpectedString, actual);
        }

        public class TestArgs
        {
            public decimal Amount { get; set; }
            public string ExpectedString { get; set; }
        }

        public class TestArgsMapping : CsvMapping<TestArgs>
        {
            public TestArgsMapping()
                : base()
            {
                MapProperty(1, x => x.Amount, new DecimalConverter(CultureInfo.InvariantCulture));
                MapProperty(2, x => x.ExpectedString);
            }
        }
    }
}
