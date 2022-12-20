using Analyzer;
using Analyzer.Models;
using Analyzer.Models.Codons;

namespace UnitTests.Analyzer
{
    [TestClass]
    public class AnalyzerTest
    {
        [TestMethod]
        public void RNASequenceTest()
        {
            Analizer analizer = new Analizer();
            Sequence sequence = analizer.CreateSequence("AAAUGAACGAAAAUCUGUUCGCUUCAUUCAUUGCCCCCACAAUCCUAGGCCUACCC");

            string shift1 = CodonsToString(sequence.CodonsShift1);
            string correctShift1 = "K[stop]TKICSLHSLPPQS[stop]AY";
            Assert.AreEqual(shift1, correctShift1);

            string shift3 = CodonsToString(sequence.CodonsShift3);
            string correctShift3 = "M(start)NENLFASFIAPTILGLP";
            Assert.AreEqual(shift3, correctShift3);
        }

        string CodonsToString(Codon[] shift)
        {
            string final = string.Empty;

            foreach (var codon in shift)
            {
                if (codon.CodonType == CodonType.Start)
                    final += "M(start)";
                else if (codon.CodonType == CodonType.End)
                    final += "[stop]";
                else
                    final += codon.Letter;
            }

            return final;
        }
    }
}