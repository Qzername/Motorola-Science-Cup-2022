using Analyzer.Models.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Models.Drawing
{
    /// <summary>
    /// struct that contains precalculated variables such as mass 
    /// </summary>
    public struct Data
    {
        public double Mass { get; set; }

        public Data(double Mass)
        {
            this.Mass = Mass;
        }
    }
}
