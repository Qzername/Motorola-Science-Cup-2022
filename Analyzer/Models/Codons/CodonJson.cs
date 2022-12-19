using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Models.Codons
{
    internal struct CodonJson
    {
        public string Letter { get; set; }
        public string Name { get; set; }
        public string[] IDs { get; set; }
        public CodonType CodonType { get; set; }
    }
}
