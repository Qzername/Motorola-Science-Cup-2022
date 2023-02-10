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
        public string Formula { get; set; }
        public float CValue { get; set; }
        public float NValue { get; set; }
        public float RestValue { get; set; }

        public Data(string Formula, float CValue, float NValue, float RestValue)
        {
            this.Formula = Formula;
            this.CValue = CValue;
            this.NValue = NValue;
            this.RestValue = RestValue;
        }
    }
}
