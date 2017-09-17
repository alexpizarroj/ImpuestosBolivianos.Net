using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImpuestosBolivianos.Tests
{
    [TestClass]
    public class SpanishNumericStringsTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        [DeploymentItem("Testcases\\SpanishNumericStringsTCs.csv")]
        [DeploymentItem("Testcases\\Schema.ini")]
        [DataSource(
            "Microsoft.VisualStudio.TestTools.DataSource.CSV",
            "|DataDirectory|\\Testcases\\SpanishNumericStringsTCs.csv",
            "SpanishNumericStringsTCs#csv", DataAccessMethod.Sequential
        )]
        [Timeout(250)]
        public void ToCardinal_InputFromSpanishNumericStringsTCs_AllShouldPass()
        {
            var input = UInt64.Parse(TestContext.DataRow["Input"].ToString());
            var expected = TestContext.DataRow["Output"].ToString();

            var actual = SpanishNumericStrings.ToCardinal(input);

            Assert.AreEqual(expected, actual);
        }
    }
}
