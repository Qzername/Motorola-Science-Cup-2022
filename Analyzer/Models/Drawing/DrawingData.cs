using Analyzer.Models.Codons;
using Analyzer.Models.Drawing;
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
        public Data Data { get; set; }

        public DrawingData(ChemPoint[] Points, Line[] Lines, Data Data)
        {
            this.Points = Points;
            this.Lines = Lines;
            this.Data = Data;
        }

        public static DrawingData CombineCodonDrawings(Codon[] codons)
        {
            return new DrawingData();
        }
    }
}
