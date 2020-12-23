using System.Globalization;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;
using Xunit;

namespace ImpuestosBolivianos.Tests
{
    public class LawConventionsTests
    {
        [Theory]
        [CsvData(@"Data/StringifyInvoiceAmountTCs.csv", typeof(TestArgs), typeof(TestArgsMapping))]
        public void PassesStringifyInvoiceAmountTestCases(TestArgs t)
        {
            var actual = LawConventions.StringifyInvoiceAmount(t.Amount);

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
