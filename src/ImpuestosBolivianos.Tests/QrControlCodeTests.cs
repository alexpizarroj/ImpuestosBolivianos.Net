using System;
using System.Globalization;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;
using Xunit;

namespace ImpuestosBolivianos.Tests
{
    public class QrControlCodeTests
    {
        [Theory]
        [CsvData(@"Data/QrControlCode-5000TCs.csv", typeof(TestArgs), typeof(TestArgsMapping))]
        public void PassesQrControlCodeTestCases(TestArgs t)
        {
            var invoice = new Invoice()
            {
                NitEmisor = t.NitEmisor,
                NroAutorizacion = t.NroAutorizacion,
                NroFactura = t.NroFactura,
                NitCliente = t.NitCliente,
                CodigoControl = t.CodigoControl,
                Fecha = t.Fecha,
                ImporteTotal = t.ImporteTotal,
                ImporteBaseCf = t.ImporteBaseCf,
                ImporteIceIehdTasas = t.ImporteIceIehdTasas,
                ImporteVentasNoGravadas = t.ImporteVentasNoGravadas,
                ImporteNoSujetoCf = t.ImporteNoSujetoCf,
                DescuentosBonosRebajas = t.DescuentosBonosRebajas,
            };

            string actual = new QrControlCode(invoice).Text;

            Assert.Equal(t.ExpectedQrCodeContent, actual);
        }

        public class TestArgs
        {
            public string NitEmisor { get; set; }
            public long NroFactura { get; set; }
            public long NroAutorizacion { get; set; }
            public DateTime Fecha { get; set; }
            public decimal ImporteTotal { get; set; }
            public decimal ImporteBaseCf { get; set; }
            public string CodigoControl { get; set; }
            public string NitCliente { get; set; }
            public decimal ImporteIceIehdTasas { get; set; }
            public decimal ImporteVentasNoGravadas { get; set; }
            public decimal ImporteNoSujetoCf { get; set; }
            public decimal DescuentosBonosRebajas { get; set; }
            public string ExpectedQrCodeContent { get; set; }
        }

        public class TestArgsMapping : CsvMapping<TestArgs>
        {
            public TestArgsMapping()
                : base()
            {
                MapProperty(1, x => x.NitEmisor);
                MapProperty(2, x => x.NroFactura);
                MapProperty(3, x => x.NroAutorizacion);
                MapProperty(4, x => x.Fecha, new DateTimeConverter("dd/MM/yyyy", CultureInfo.InvariantCulture));
                MapProperty(5, x => x.ImporteTotal, new DecimalConverter(CultureInfo.InvariantCulture));
                MapProperty(6, x => x.ImporteBaseCf, new DecimalConverter(CultureInfo.InvariantCulture));
                MapProperty(7, x => x.CodigoControl);
                MapProperty(8, x => x.NitCliente);
                MapProperty(9, x => x.ImporteIceIehdTasas, new DecimalConverter(CultureInfo.InvariantCulture));
                MapProperty(10, x => x.ImporteVentasNoGravadas, new DecimalConverter(CultureInfo.InvariantCulture));
                MapProperty(11, x => x.ImporteNoSujetoCf, new DecimalConverter(CultureInfo.InvariantCulture));
                MapProperty(12, x => x.DescuentosBonosRebajas, new DecimalConverter(CultureInfo.InvariantCulture));
                MapProperty(13, x => x.ExpectedQrCodeContent);
            }
        }
    }
}
