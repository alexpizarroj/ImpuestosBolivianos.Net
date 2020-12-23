using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace ImpuestosBolivianos.Tests
{
    /*
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
    }
    */
}
