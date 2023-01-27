using Analyzer.Models.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Models.Terminuses
{
    public struct Terminus
    {
        public string Name { get; set; }
        public TerminusType Type { get; set; }
        public int ExitPoint { get; set; }
        public int ConnectionPoint { get; set; }
        public DrawingData DrawingData { get; set; }
    }
}
