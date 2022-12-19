using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Models.Codons
{
    public struct Codon
    {
        public readonly string Letter;
        public readonly string Name;
        public readonly string[] IDs;
        public readonly CodonType CodonType;

        public Codon(string Letter, string Name, string[] IDs, CodonType CodonType)
        {
            this.Letter = Letter;
            this.Name = Name;
            this.IDs = IDs;
            this.CodonType = CodonType;
        }
    }

}
