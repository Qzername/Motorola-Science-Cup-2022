using Analyzer.Models.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Models.Codons
{
    public struct Codon
    {
        public string Letter { get; set; }
        public string Name { get; set; }
        public string[] IDs { get; set; }
        public CodonType CodonType { get; set; }
        public DrawingData? DrawingData { get; set; }

        public Codon(string Letter, string Name, string[] IDs, CodonType CodonType)
        {
            this.Letter = Letter;
            this.Name = Name;
            this.IDs = IDs;
            this.CodonType = CodonType;
            DrawingData = new DrawingData();
        }
        
        public Codon(string Letter, string Name, string[] IDs, CodonType CodonType, DrawingData DrawingData)
        {
            this.Letter = Letter;
            this.Name = Name;
            this.IDs = IDs;
            this.CodonType = CodonType;
            this.DrawingData = DrawingData;
        }
    }

}
