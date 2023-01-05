using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodonEditor.Models.Draw
{
    public struct LineRaw
    {
        public int IDChemPoint1;
        public int IDChemPoint2;
        public byte NumberOfBind;

        public LineRaw(int IDChemPoint1, int IDChemPoint2)
        {
            this.IDChemPoint1 = IDChemPoint1;
            this.IDChemPoint2 = IDChemPoint2;
            NumberOfBind = 1;
        }

        public LineRaw(int IDChemPoint1, int IDChemPoint2, byte NumberOfBind)
        {
            this.IDChemPoint1 = IDChemPoint1;
            this.IDChemPoint2 = IDChemPoint2;
            this.NumberOfBind = NumberOfBind;
        }
    }
}
