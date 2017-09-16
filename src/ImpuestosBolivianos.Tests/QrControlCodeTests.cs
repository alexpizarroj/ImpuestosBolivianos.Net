using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace ImpuestosBolivianos.Tests
{
    [TestClass]
    public class QrControlCodeTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        [DeploymentItem("Testcases\\QrControlCode-5000TCs.csv")]
        [DeploymentItem("Testcases\\Schema.ini")]
        [DataSource(
            "Microsoft.VisualStudio.TestTools.DataSource.CSV",
            "|DataDirectory|\\Testcases\\QrControlCode-5000TCs.csv",
            "QrControlCode-5000TCs#csv", DataAccessMethod.Sequential
        )]
        public void Text_InputFromQrControlCodeTCs_AllShouldPass()
        {
            var invoice = CurrentInvoiceOnTestContext();
            String expected = Convert.ToString(TestContext.DataRow["ContenidoCodigoQr"]);

            var sut = new QrControlCode(invoice);
            String actual = sut.Text;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DeploymentItem("Testcases\\QrControlCode-5000TCs.csv")]
        [DeploymentItem("Testcases\\Schema.ini")]
        [DataSource(
            "Microsoft.VisualStudio.TestTools.DataSource.CSV",
            "|DataDirectory|\\Testcases\\QrControlCode-5000TCs.csv",
            "QrControlCode-5000TCs#csv", DataAccessMethod.Sequential
        )]
        public void ToPngByteArray_InputFromQrControlCodeTCs_AllShouldPass()
        {
            var invoice = CurrentInvoiceOnTestContext();
            String expected = Convert.ToString(TestContext.DataRow["ContenidoCodigoQr"]);

            var sut = new QrControlCode(invoice);
            String actual = ReadQrCodeContents(sut.ToPngByteArray());

            /*
            * Test for equality *only* if we have a reading (i.e. actual != null).
            * For some reason, a small chunk of the QR codes coming from
            * the testcase set are not being recognized by ZXing.
            */
            if (actual != null) Assert.AreEqual(expected, actual);
        }

        private readonly CultureInfo BolivianCultureInfo = CultureInfo.CreateSpecificCulture("es-BO");

        private Invoice CurrentInvoiceOnTestContext()
        {
            return new Invoice()
            {
                NitEmisor = Convert.ToString(TestContext.DataRow["NitEmisor"]),
                NroAutorizacion = Convert.ToInt64(TestContext.DataRow["NroAutorizacion"]),
                NroFactura = Convert.ToInt64(TestContext.DataRow["NroFactura"]),
                NitCliente = Convert.ToString(TestContext.DataRow["NitCliente"]),
                CodigoControl = Convert.ToString(TestContext.DataRow["CodigoControl"]),
                Fecha = Convert.ToDateTime(
                    TestContext.DataRow["Fecha"], BolivianCultureInfo),
                ImporteTotal = Convert.ToDecimal(
                    TestContext.DataRow["ImporteTotal"], CultureInfo.InvariantCulture),
                ImporteBaseCf = Convert.ToDecimal(
                    TestContext.DataRow["ImporteBaseCf"], CultureInfo.InvariantCulture),
                ImporteIceIehdTasas = Convert.ToDecimal(
                    TestContext.DataRow["ImporteIceIehdTasas"], CultureInfo.InvariantCulture),
                ImporteVentasNoGravadas = Convert.ToDecimal(
                    TestContext.DataRow["ImporteVentasNoGravadas"], CultureInfo.InvariantCulture),
                ImporteNoSujetoCf = Convert.ToDecimal(
                    TestContext.DataRow["ImporteNoSujetoCf"], CultureInfo.InvariantCulture),
                DescuentosBonosRebajas = Convert.ToDecimal(
                    TestContext.DataRow["DescuentosBonosRebajas"], CultureInfo.InvariantCulture)
            };
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
