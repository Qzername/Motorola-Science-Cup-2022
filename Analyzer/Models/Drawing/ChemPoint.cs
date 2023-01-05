using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodonEditor.Models.Draw
{
    public struct ChemPoint
    {
        public int ID { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public string MolecularFormula { get; set; }
        public sbyte Charge { get; set; }
    }
}
