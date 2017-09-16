using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImpuestosBolivianos.Tests
{
    [TestClass]
    public class NumericStringsTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        [DeploymentItem("Testcases\\SpanishNumericStrings-BusinessWritingsTCs.csv")]
        [DeploymentItem("Testcases\\Schema.ini")]
        [DataSource(
            "Microsoft.VisualStudio.TestTools.DataSource.CSV",
            "|DataDirectory|\\Testcases\\SpanishNumericStrings-BusinessWritingsTCs.csv",
            "SpanishNumericStrings-BusinessWritingsTCs#csv", DataAccessMethod.Sequential
        )]
        [Timeout(250)]
        public void FromDouble_InputFromBusinessWritingsTCs_AllShouldPass()
        {
            var input = UInt64.Parse(TestContext.DataRow["Input"].ToString());
            var expected = TestContext.DataRow["Output"].ToString().ToLower();

            var actual = SpanishNumericStrings.FromDouble(input).ToLower();

            Assert.AreEqual(expected, actual);
        }
    }
}
