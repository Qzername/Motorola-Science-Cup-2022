using Analyzer.Models.Codons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodonEditor.Models.Draw
{
    public struct DrawingData
    {
        public ChemPoint[] Points { get; set; }
        public Line[] Lines { get; set; }

        public DrawingData(ChemPoint[] Points, Line[] Lines)
        {
            this.Points = Points;
            this.Lines = Lines;
        }

        public static DrawingData CombineCodonDrawings(Codon[] codons)
        {
            throw new Exception("nie zrobione");
            return new DrawingData();
        }
    }
}
