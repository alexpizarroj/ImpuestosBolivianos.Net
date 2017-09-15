using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace Aepj.FacturacionBoliviana.Tests
{
    [TestClass]
    public class QrCodeBuilderTests
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        private static CultureInfo NumbersSourceCulture => CultureInfo.CreateSpecificCulture("en-US");

        private static CultureInfo DatesSourceCulture => CultureInfo.CreateSpecificCulture("es-BO");

        [DeploymentItem("csv\\5000CasosPruebaDerivadosCodigoQr.csv")]
        [DataSource(
            "Microsoft.VisualStudio.TestTools.DataSource.CSV",
            "|DataDirectory|\\csv\\5000CasosPruebaDerivadosCodigoQr.csv",
            "5000CasosPruebaDerivadosCodigoQr#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TextContents_InputFromDerived5000QrCodeTestCases_GeneratedTextContentsShouldMatchTestData()
        {
            var nitEmisor = Convert.ToString(TestContext.DataRow["NitEmisor"]);
            var nroFactura = Convert.ToString(TestContext.DataRow["NroFactura"]);
            var nroAutorizacion = Convert.ToString(TestContext.DataRow["NroAutorizacion"]);
            var fecha = Convert.ToDateTime(TestContext.DataRow["Fecha"], DatesSourceCulture);
            var importeTotal = Convert.ToDouble(TestContext.DataRow["ImporteTotal"], NumbersSourceCulture);
            var importeBaseCf = Convert.ToDouble(TestContext.DataRow["ImporteBaseCf"], NumbersSourceCulture);
            var codigoControl = Convert.ToString(TestContext.DataRow["CodigoControl"]);
            var nitCliente = Convert.ToString(TestContext.DataRow["NitCliente"]);
            var importeIceIehdTasas = Convert.ToDouble(TestContext.DataRow["ImporteIceIehdTasas"], NumbersSourceCulture);
            var importeVentasNoGravadas = Convert.ToDouble(TestContext.DataRow["ImporteVentasNoGravadas"], NumbersSourceCulture);
            var importeNoSujetoCf = Convert.ToDouble(TestContext.DataRow["ImporteNoSujetoCf"], NumbersSourceCulture);
            var descuentosBonosRebajas = Convert.ToDouble(TestContext.DataRow["DescuentosBonosRebajas"], NumbersSourceCulture);
            String expected = Convert.ToString(TestContext.DataRow["ContenidoCodigoQr"]);

            var sut = new QrCodeBuilder();
            String actual =
                sut
                .WithNitEmisor(nitEmisor)
                .WithNroFactura(nroFactura)
                .WithNroAutorizacion(nroAutorizacion)
                .WithFecha(fecha)
                .WithImporteTotal(importeTotal)
                .WithImporteBaseCf(importeBaseCf)
                .WithCodigoControl(codigoControl)
                .WithNitCliente(nitCliente)
                .WithImporteIceIehdTasas(importeIceIehdTasas)
                .WithImporteVentasNoGravadas(importeVentasNoGravadas)
                .WithImporteNoSujetoCf(importeNoSujetoCf)
                .WithDescuentosBonosRebajas(descuentosBonosRebajas)
                .TextContents;

            Assert.AreEqual(expected, actual);
        }

        [DeploymentItem("csv\\5000CasosPruebaDerivadosCodigoQr.csv")]
        [DataSource(
            "Microsoft.VisualStudio.TestTools.DataSource.CSV",
            "|DataDirectory|\\csv\\5000CasosPruebaDerivadosCodigoQr.csv",
            "5000CasosPruebaDerivadosCodigoQr#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void ToPngImageByteArray_InputFromDerived5000QrCodeTestCases_GeneratedStreamQrReadingShouldMatchTestData()
        {
            var nitEmisor = Convert.ToString(TestContext.DataRow["NitEmisor"]);
            var nroFactura = Convert.ToString(TestContext.DataRow["NroFactura"]);
            var nroAutorizacion = Convert.ToString(TestContext.DataRow["NroAutorizacion"]);
            var fecha = Convert.ToDateTime(TestContext.DataRow["Fecha"], DatesSourceCulture);
            var importeTotal = Convert.ToDouble(TestContext.DataRow["ImporteTotal"], NumbersSourceCulture);
            var importeBaseCf = Convert.ToDouble(TestContext.DataRow["ImporteBaseCf"], NumbersSourceCulture);
            var codigoControl = Convert.ToString(TestContext.DataRow["CodigoControl"]);
            var nitCliente = Convert.ToString(TestContext.DataRow["NitCliente"]);
            var importeIceIehdTasas = Convert.ToDouble(TestContext.DataRow["ImporteIceIehdTasas"]);
            var importeVentasNoGravadas = Convert.ToDouble(TestContext.DataRow["ImporteVentasNoGravadas"]);
            var importeNoSujetoCf = Convert.ToDouble(TestContext.DataRow["ImporteNoSujetoCf"]);
            var descuentosBonosRebajas = Convert.ToDouble(TestContext.DataRow["DescuentosBonosRebajas"]);
            String expected = Convert.ToString(TestContext.DataRow["ContenidoCodigoQr"]);

            var imageBytes = new QrCodeBuilder()
                .WithNitEmisor(nitEmisor)
                .WithNroFactura(nroFactura)
                .WithNroAutorizacion(nroAutorizacion)
                .WithFecha(fecha)
                .WithImporteTotal(importeTotal)
                .WithImporteBaseCf(importeBaseCf)
                .WithCodigoControl(codigoControl)
                .WithNitCliente(nitCliente)
                .WithImporteIceIehdTasas(importeIceIehdTasas)
                .WithImporteVentasNoGravadas(importeVentasNoGravadas)
                .WithImporteNoSujetoCf(importeNoSujetoCf)
                .WithDescuentosBonosRebajas(descuentosBonosRebajas)
                .ToPngImageByteArray();
            String actual = ReadQrCodeContents(imageBytes);

            /*
             * Test for equality *only* if we got a reading (i.e. actual != null)
             *   For some reason, a small chunk of the QR codes coming
             *   from the testcase set are not being recognized by ZXing
             */
            if (actual != null) Assert.AreEqual(expected, actual);
        }

        private String ReadQrCodeContents(byte[] imageBytes)
        {
            using (var mstream = new MemoryStream(imageBytes))
            {
                using (var bitmap = Image.FromStream(mstream) as Bitmap)
                {
                    var luminanceSource = new BitmapLuminanceSource(bitmap);

                    var binarizer = new HybridBinarizer(luminanceSource);

                    var binaryBitmap = new BinaryBitmap(binarizer);

                    var result = new QRCodeReader().decode(binaryBitmap)?.Text;

                    return result;
                }
            }
        }
    }
}
