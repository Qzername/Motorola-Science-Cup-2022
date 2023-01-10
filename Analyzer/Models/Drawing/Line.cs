using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Models.Draw
{
    public struct Line
    {
        public int IDChemPoint1 { get; set; }
        public int IDChemPoint2 { get; set; }
        public byte NumberOfBind { get; set; }

        public Line(int IDChemPoint1, int IDChemPoint2)
        {
            this.IDChemPoint1 = IDChemPoint1;
            this.IDChemPoint2 = IDChemPoint2;
            NumberOfBind = 1;
        }
        public Line(int IDChemPoint1, int IDChemPoint2, byte NumberOfBind)
        {
            this.IDChemPoint1 = IDChemPoint1;
            this.IDChemPoint2 = IDChemPoint2;
            this.NumberOfBind = NumberOfBind;
        }
    }
}
