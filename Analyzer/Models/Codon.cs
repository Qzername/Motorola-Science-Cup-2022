using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Models
{
    public struct Codon
    {
        public readonly string[] IDs;
        public readonly string Letter;
        public readonly string Name;
        public readonly CodonType CodonType;
    }

    public enum CodonType
    {
        Start,
        Normal,
        End
    }
}
