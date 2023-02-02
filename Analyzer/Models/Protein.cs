using Analyzer.Models.Codons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Models
{
    public struct Protein
    {
        public int StartPosition;
        public Codon[] Codons;

        public Protein(int StartPosition, Codon[] Codons)
        {
            this.StartPosition = StartPosition;
            this.Codons = Codons;
        }
    }
}
