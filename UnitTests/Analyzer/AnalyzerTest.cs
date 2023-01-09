using Analyzer;
using Analyzer.Models;
using Analyzer.Models.Codons;

namespace UnitTests.Analyzer
{
    [TestClass]
    public class AnalyzerTest
    {
        [TestMethod]
        public void RNASequenceTest() => SequenceTest("AAAUGAACGAAAAUCUGUUCGCUUCAUUCAUUGCCCCCACAAUCCUAGGCCUACCC");
        
        [TestMethod]
        public void DNASequenceTest() => SequenceTest("AAATGAACGAAAATCTGTTCGCTTCATTCATTGCCCCCACAATCCTAGGCCTACCC");
    
        public void SequenceTest(string sequenceRaw)
        {
            Sequence sequence = SequenceAnalyzer.CreateSequence(sequenceRaw);

            string shift1 = Sequence.CodonsToString(sequence.CodonsShift1);
            string correctShift1 = "K[stop]TKICSLHSLPPQS[stop]AY";
            Assert.AreEqual(shift1, correctShift1);

            string shift3 = Sequence.CodonsToString(sequence.CodonsShift3);
            string correctShift3 = "M(start)NENLFASFIAPTILGLP";
            Assert.AreEqual(shift3, correctShift3);

        }
    }
}