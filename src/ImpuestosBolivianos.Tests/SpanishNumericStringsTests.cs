using TinyCsvParser.Mapping;
using Xunit;

namespace ImpuestosBolivianos.Tests
{
    public class SpanishNumericStringsTests
    {
        [Theory]
        [CsvData(@"Data/SpanishNumericStringsTCs.csv", typeof(TestArgs), typeof(TestArgsMapping))]
        public void ToCardinal_InputFromSpanishNumericStringsTCs_AllShouldPass(TestArgs t)
        {
            var actual = SpanishNumericStrings.ToCardinal(t.InputValue);

            Assert.Equal(t.ExpectedString, actual);
        }

        public class TestArgs
        {
            public ulong InputValue { get; set; }
            public string ExpectedString { get; set; }
        }

        public class TestArgsMapping : CsvMapping<TestArgs>
        {
            public TestArgsMapping()
                : base()
            {
                MapProperty(1, x => x.InputValue);
                MapProperty(2, x => x.ExpectedString);
            }
        }
    }
}
