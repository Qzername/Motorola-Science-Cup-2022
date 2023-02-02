using Analyzer.Models.Codons;

namespace Analyzer.Models
{
    public struct Sequence
    {
        public string RawSequence;
        public Codon[] CodonsShift1;
        public Codon[] CodonsShift2;
        public Codon[] CodonsShift3;

        public Sequence(string RawSequence, Codon[] CodonsShift1, Codon[] CodonShift2, Codon[] CodonShift3)
        {
            this.RawSequence = RawSequence;
            this.CodonsShift1 = CodonsShift1;
            this.CodonsShift2 = CodonShift2;
            this.CodonsShift3 = CodonShift3;
        }

        public static string CodonsToString(Codon[] shift, bool isVerbose = true)
        {
            string final = string.Empty;

            foreach (var codon in shift)
            {
                if (codon.CodonType == CodonType.Start && isVerbose)
                    final += "M(start)";
                else if (codon.CodonType == CodonType.End)
                    final += isVerbose ? "[stop]" : "-";
                else
                    final += codon.Letter;
            }

            return final;
        }
    }
}