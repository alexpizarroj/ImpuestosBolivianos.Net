using System;
using System.Globalization;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;
using Xunit;

namespace ImpuestosBolivianos.Tests
{
    public class ControlCodeTests
    {
        [Fact]
        public void Fails()
        {
            Assert.True(false);
        }

        [Theory]
        [CsvData(@"Data/ControlCodeV7-5000TCs.csv", typeof(TestArgs), typeof(TestArgsMapping))]
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
