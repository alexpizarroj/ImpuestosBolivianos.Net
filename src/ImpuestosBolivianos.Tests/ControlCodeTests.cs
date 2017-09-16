using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace ImpuestosBolivianos.Tests.Tests
{
    [TestClass]
    public class ControlCodeBuilderTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        [DeploymentItem("Testcases\\ControlCodeV7-5000TCs.csv")]
        [DeploymentItem("Testcases\\Schema.ini")]
        [DataSource(
            "Microsoft.VisualStudio.TestTools.DataSource.CSV",
            "|DataDirectory|\\Testcases\\ControlCodeV7-5000TCs.csv",
            "ControlCodeV7-5000TCs#csv", DataAccessMethod.Sequential
        )]
        public void Text_InputFromControlCodeV7TCs_AllShouldPass()
        {
            var invoice = CurrentInvoiceOnTestContext();
            String expected = Convert.ToString(TestContext.DataRow["CodigoControl"]);

            var sut = new ControlCode(invoice);
            String actual = sut.Text;

            Assert.AreEqual(expected, actual);
        }

        private Invoice CurrentInvoiceOnTestContext()
        {
            return new Invoice()
            {
                NroAutorizacion = Convert.ToInt64(TestContext.DataRow["NroAutorizacion"]),
                NroFactura = Convert.ToInt64(TestContext.DataRow["NroFactura"]),
                NitCliente = Convert.ToString(TestContext.DataRow["NitCliente"]),
                Fecha = Convert.ToDateTime(TestContext.DataRow["Fecha"], CultureInfo.InvariantCulture),
                ImporteTotal = Convert.ToDecimal(TestContext.DataRow["Monto"], CultureInfo.InvariantCulture),
                LlaveDosificacion = Convert.ToString(TestContext.DataRow["Llave"])
            };
        }
    }
}
