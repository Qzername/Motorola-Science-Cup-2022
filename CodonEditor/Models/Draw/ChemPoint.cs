using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodonEditor.Models.Draw
{
    public struct ChemPointRaw
    {
        public int ID;
        public Point Position;
        public string MolecularFormula;
        public sbyte Charge;
    }
}
