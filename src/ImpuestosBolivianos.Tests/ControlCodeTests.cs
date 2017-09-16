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

        private static CultureInfo EnUsCulture => CultureInfo.CreateSpecificCulture("en-US");
        private static CultureInfo EsBoCulture => CultureInfo.CreateSpecificCulture("es-BO");

        [TestMethod]
        [DeploymentItem("csv\\5000CasosPruebaCCVer7.csv")]
        [DataSource(
            "Microsoft.VisualStudio.TestTools.DataSource.CSV",
            "|DataDirectory|\\csv\\5000CasosPruebaCCVer7.csv",
            "5000CasosPruebaCCVer7#csv", DataAccessMethod.Sequential
        )]
        public void ToString_InputFromOfficial5000ControlCodeTestCases_AllShouldPass()
        {
            var invoice = CurrentInvoiceOnTestContext();
            String expected = Convert.ToString(TestContext.DataRow["CodigoControl"]);

            var sut = new ControlCode(invoice);
            String actual = sut.ToString();

            Assert.AreEqual(expected, actual);
        }

        private Invoice CurrentInvoiceOnTestContext()
        {
            return new Invoice()
            {
                NroAutorizacion = Convert.ToInt64(TestContext.DataRow["NroAutorizacion"]),
                NroFactura = Convert.ToInt64(TestContext.DataRow["NroFactura"]),
                NitCliente = Convert.ToString(TestContext.DataRow["NitCliente"]),
                Fecha = Convert.ToDateTime(TestContext.DataRow["Fecha"], EsBoCulture),
                ImporteTotal = Convert.ToDecimal(TestContext.DataRow["Monto"], EnUsCulture),
                LlaveDosificacion = Convert.ToString(TestContext.DataRow["Llave"])
            };
        }
    }
}
