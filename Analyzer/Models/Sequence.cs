using Analyzer.Models.Codons;

namespace Analyzer.Models
{
    public struct Sequence
    {
        public readonly string RawSequence;
        public readonly Codon[] CodonsShift1;
        public readonly Codon[] CodonsShift2;
        public readonly Codon[] CodonsShift3;

        public Sequence(string RawSequence, Codon[] CodonsShift1, Codon[] CodonShift2, Codon[] CodonShift3)
        {
            this.RawSequence = RawSequence;
            this.CodonsShift1 = CodonsShift1;
            this.CodonsShift2 = CodonShift2;
            this.CodonsShift3 = CodonShift3;
        }
    }
}