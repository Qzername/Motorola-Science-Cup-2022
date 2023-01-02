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
        public readonly Codon[] Codons;

        public Protein(Codon[] Codons)
        {
            this.Codons = Codons;
        }
    }
}
