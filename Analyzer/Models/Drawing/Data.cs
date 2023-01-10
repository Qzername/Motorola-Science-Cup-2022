using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Models.Drawing
{
    public struct Data
    {
        public int ChemPointStart { get; set; }
        public int ChemPointEnd { get; set; }
        public double Mass { get; set; }

        public Data(int ChemPointStart, int ChemPointEnd, double Mass)
        {
            this.ChemPointStart = ChemPointStart;
            this.ChemPointEnd = ChemPointEnd;
            this.Mass = Mass;
        }
    }
}
