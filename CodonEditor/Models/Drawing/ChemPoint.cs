using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodonEditor.Models.Drawing
{
    public struct ChemPoint
    {
        public int ID;
        public Point Position;
        public string MolecularFormula;
        public byte Charge;
    }
}
