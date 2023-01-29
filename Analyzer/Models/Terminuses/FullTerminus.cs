using Analyzer.Models.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Models.Terminuses
{
    public struct FullTerminus
    {
        public int ExitPoint { get; set; }
        public int ConnectionPoint { get; set; }
        public int CodonConnectionPoint { get; set; }
        public DrawingData DrawingData { get; set; }
    }
}
