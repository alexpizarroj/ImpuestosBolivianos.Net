using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace ImpuestosBolivianos.Tests
{
    [TestClass]
    public class LawConventionsTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        [DeploymentItem("Testcases\\StringifyInvoiceAmountTCs.csv")]
        [DeploymentItem("Testcases\\Schema.ini")]
        [DataSource(
            "Microsoft.VisualStudio.TestTools.DataSource.CSV",
            "|DataDirectory|\\Testcases\\StringifyInvoiceAmountTCs.csv",
            "StringifyInvoiceAmountTCs#csv", DataAccessMethod.Sequential
        )]
        public void StringifyInvoiceAmount_InputFromStringifyInvoiceAmountTCs_AllShouldPass()
        {
            var amount = Convert.ToDecimal(TestContext.DataRow["Input"], CultureInfo.InvariantCulture);
            var expected = Convert.ToString(TestContext.DataRow["Output"]);

            var actual = LawConventions.StringifyInvoiceAmount(amount);

            Assert.AreEqual(expected, actual);
        }
    }
}
