using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodonEditor.Models.Drawing
{
    public struct Line
    {
        public int IDChemPoint1;
        public int IDChemPoint2;

        public Line(int IDChemPoint1, int IDChemPoint2)
        {
            this.IDChemPoint1 = IDChemPoint1;
            this.IDChemPoint2 = IDChemPoint2;
        }
    }
}
