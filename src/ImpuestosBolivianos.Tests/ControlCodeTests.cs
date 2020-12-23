using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;
using Xunit;

namespace ImpuestosBolivianos.Tests
{
    public class ControlCodeTests
    {
        [Theory]
        [MemberData(nameof(GetData))]
        public void PassesControlCodeV7TestCases(TestArgs t)
        {
            string actual = Facturacion.MakeControlCode(
                t.NroAutorizacion,
                t.NroFactura,
                t.NitCliente,
                t.Fecha,
                t.ImporteTotal,
                t.LlaveDosificacion);

            Assert.Equal(t.ExpectedCodigoControl, actual);
        }

        public static IEnumerable<object[]> GetData()
        {
            var csvParserOptions = new CsvParserOptions(true, ',');
            var csvMapper = new TestArgsMapping();
            var csvParser = new CsvParser<TestArgs>(csvParserOptions, csvMapper);

            var results = csvParser.ReadFromFile(@"Data/ControlCodeV7-5000TCs.csv", Encoding.UTF8);
            foreach (var result in results)
            {
                if (!result.IsValid)
                {
                    throw new InvalidOperationException(result.Error.ToString());
                }

                yield return new object[] { result.Result };
            }
        }

        public class TestArgs
        {
            public long NroAutorizacion { get; set; }
            public long NroFactura { get; set; }
            public string NitCliente { get; set; }
            public DateTime Fecha { get; set; }
            public decimal ImporteTotal { get; set; }
            public string LlaveDosificacion { get; set; }
            public string ExpectedCodigoControl { get; set; }
        }

        public class TestArgsMapping : CsvMapping<TestArgs>
        {
            public TestArgsMapping()
                : base()
            {
                MapProperty(1, x => x.NroAutorizacion);
                MapProperty(2, x => x.NroFactura);
                MapProperty(3, x => x.NitCliente);
                MapProperty(4, x => x.Fecha, new DateTimeConverter("yyyy/MM/dd", CultureInfo.InvariantCulture));
                MapProperty(5, x => x.ImporteTotal, new DecimalConverter(CultureInfo.InvariantCulture));
                MapProperty(6, x => x.LlaveDosificacion);
                MapProperty(7, x => x.ExpectedCodigoControl);
            }
        }
    }
}
