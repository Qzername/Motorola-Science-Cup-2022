using Analyzer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Models.Draw
{
    public struct ChemPoint
    {
        public int ID { get; set; }
        public Position Position { get; set; }
        public string MolecularFormula { get; set; }
        public sbyte Charge { get; set; }
    }
}
