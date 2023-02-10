using Analyzer.Analyzers;
using Analyzer.Models.Codons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Analyzer
{
    [TestClass]
    public class PeptideAnalyzerTest
    {
        [TestMethod]
        public void CalculateNetCharge()
        {
            Codon[] sequence = CodonAnalyzer.CreateCodonsFromString("HKASNPQWAE");

            for(int i = 1; i <2; i++)
                Assert.AreEqual(3, PeptideAnalyzer.CalculateCharge(sequence, i));

            for(int i = 3; i <4; i++)
                Assert.AreEqual(2, PeptideAnalyzer.CalculateCharge(sequence, i));

            for(int i = 5; i <6; i++)
                Assert.AreEqual(1, PeptideAnalyzer.CalculateCharge(sequence, i));

            for(int i = 7; i <9; i++)
                Assert.AreEqual(0, PeptideAnalyzer.CalculateCharge(sequence, i));

            Assert.AreEqual(-1, PeptideAnalyzer.CalculateCharge(sequence, 10));

            for(int i = 11; i <15; i++)
                Assert.AreEqual(-2, PeptideAnalyzer.CalculateCharge(sequence, i));
        }

        [TestMethod]
        public void CalculateMassTest()
        {
            double result = PeptideAnalyzer.CalculateMass(CodonAnalyzer.CreateCodonsFromString("CEQKLISEDLN"));
            Assert.AreEqual(1290.6105, result, 1d);
        }

        [TestMethod]
        public void CalculateIzoelectricPointTest()
        {
            double result = PeptideAnalyzer.CalculateIzoelectricPoint(CodonAnalyzer.CreateCodonsFromString("CEQKLISEDLN"));
            Assert.AreEqual(3.935, result, 0.01);
        }

        [TestMethod]
        public void CalculateExtinctionCoefficientTest()
        {
            double result = PeptideAnalyzer.CalculateExtinctionCoefficient(CodonAnalyzer.CreateCodonsFromString("WCYCER"));
            Assert.AreEqual(7240, result);
        }
    }
}
