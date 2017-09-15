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

        private static CultureInfo NumbersSourceCulture => CultureInfo.CreateSpecificCulture("en-US");

        private static CultureInfo DatesSourceCulture => CultureInfo.CreateSpecificCulture("es-BO");

        [DeploymentItem("csv\\5000CasosPruebaCCVer7.csv")]
        [DataSource(
            "Microsoft.VisualStudio.TestTools.DataSource.CSV",
            "|DataDirectory|\\csv\\5000CasosPruebaCCVer7.csv",
            "5000CasosPruebaCCVer7#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void Build_InputFromOfficial5000ControlCodeTestCases_ShouldPassAll()
        {
            var nroAutorizacion = Convert.ToString(TestContext.DataRow["NroAutorizacion"]);
            var nroFactura = Convert.ToString(TestContext.DataRow["NroFactura"]);
            var nitCliente = Convert.ToString(TestContext.DataRow["NitCliente"]);
            var fecha = Convert.ToDateTime(TestContext.DataRow["Fecha"], DatesSourceCulture);
            var monto = Convert.ToDouble(TestContext.DataRow["Monto"], NumbersSourceCulture);
            var llave = Convert.ToString(TestContext.DataRow["Llave"]);
            String expected = Convert.ToString(TestContext.DataRow["CodigoControl"]);

            var sut = new ControlCodeBuilder();
            String actual =
                sut
                .WithNroAutorizacion(nroAutorizacion)
                .WithNroFactura(nroFactura)
                .WithNitCliente(nitCliente)
                .WithFecha(fecha)
                .WithMonto(monto)
                .WithLlave(llave)
                .Build();

            Assert.AreEqual(expected, actual);
        }
    }
}
