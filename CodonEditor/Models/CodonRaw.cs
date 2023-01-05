using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Models.Codons;
using CodonEditor.Models.Draw;

namespace CodonEditor.Models
{
    public struct CodonRaw
    {
        public string Letter { get; set; }
        public string Name { get; set; }
        public string[] IDs { get; set; }
        public CodonType CodonType { get; set; }
        public DrawingDataRaw? DrawingData { get; set; }
    }
}
