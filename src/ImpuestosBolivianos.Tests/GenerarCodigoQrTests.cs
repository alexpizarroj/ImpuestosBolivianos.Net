using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;
using Xunit;

namespace ImpuestosBolivianos.Tests
{
    public class GenerarCodigoQrTests
    {
        [Fact]
        public void GeneratesProperPngBytes()
        {
            var res = ImpuestosBolivianos.Facturacion.GenerarCodigoQr(
                471625511829685L,
                876814,
                "7904006306693",
                new DateTime(2008, 5, 19),
                35958.60m,
                "7B-F3-48-A8",
                "1665979",
                35958.60m,
                0.00m,
                0.00m,
                0.00m,
                0.00m);

            var actualMd5 = CalculateMd5(res.BytesPng);

            const string expectedMd5 = "554fb8a814402e6b6b206427452cc1a4";
            Assert.Equal(expectedMd5, actualMd5);
        }

        [Theory]
        [CsvData(@"Data/QrControlCode-5000TCs.csv", typeof(TestArgs), typeof(TestArgsMapping))]
        public void PassesTestCases(TestArgs t)
        {
            var codigoQrDeFactura = Facturacion.GenerarCodigoQr(
                t.NroAutorizacion,
                t.NroFactura,
                t.NitCliente,
                t.Fecha,
                t.ImporteTotal,
                t.CodigoControl,
                t.NitEmisor,
                t.ImporteBaseCf,
                t.ImporteIceIehdTasas,
                t.ImporteVentasNoGravadas,
                t.ImporteNoSujetoCf,
                t.DescuentosBonosRebajas);

            Assert.Equal(t.ExpectedQrCodeContent, codigoQrDeFactura.Texto);
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

        private string CalculateMd5(IEnumerable<byte> bytes)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = new MemoryStream(bytes.ToArray()))
                {
                    var hashBytes = md5.ComputeHash(stream);
                    var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                    return hash;
                }
            }
        }
    }
}
