using Analyzer.Models;
using Analyzer.Models.Codons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Analyzers
{
    public static class PeptideAnalyzer
    {
        readonly static string[] IzoelectricPointCodons;

        static PeptideAnalyzer()
        {
            //small letters = not a codon
            //big letters = codon
            // + - = charge
            IzoelectricPointCodons = new string[] { "K", "R", "H", "D", "E" };
        }

        public static int CalculateCharge(Codon[] sequence, float ph)
        {
            int charge = 0;

            if (sequence[0].DrawingData.Value.Data.CValue >= ph)
                charge++;

            if (sequence[^1].DrawingData.Value.Data.NValue <= ph)
                charge--;

            var getNegative = sequence.Where(x => !GetChargeOfCodon(x));
            var getPositive = sequence.Where(x => GetChargeOfCodon(x));

            foreach(var c in getNegative)
            {
                if (c.DrawingData.Value.Data.RestValue <= ph)
                    charge--;
            }

            foreach(var c in getPositive)
            {
                if (c.DrawingData.Value.Data.RestValue >= ph)
                    charge++;
            }
            
            return charge;
        }

        public static double CalculateExtinctionCoefficient(Codon[] sequence)
        {
            double extinctionCoefficient = 0;

            //this is not efficient but its faster to write
            int WCount = sequence.Where(x => x.Letter == "W").Count();
            int YCount = sequence.Where(x => x.Letter == "Y").Count();
            int CCount = sequence.Where(x => x.Letter == "C").Count();

            extinctionCoefficient = WCount * 5500 + YCount*1490 + CCount*125;

            return extinctionCoefficient;
        }

        public static double CalculateIzoelectricPoint(Codon[] sequence)
        {
            double point = 0;

            float[] rawSequence = sequence.Except( sequence.Where(x => !IzoelectricPointCodons.Any(y=>y == x.Letter))).Select(x => x.DrawingData.Value.Data.RestValue).OrderBy(x=>x).ToArray();
            float[] realSequence = new float[rawSequence.Length + 2];

            realSequence[0] = sequence[0].DrawingData.Value.Data.CValue;
            realSequence[^1] = sequence[^1].DrawingData.Value.Data.CValue;
         
            for(int i = 0; i < rawSequence.Length; i++)
                realSequence[i + 1] = rawSequence[i];

            int numberOfNegative = sequence.Count(x => !GetChargeOfCodon(x));
            int index = realSequence.Length - numberOfNegative;

            point = (realSequence[index] + realSequence[index - 1]) / 2;

            return point;
        }

        /// <summary>
        /// Calculate mass of codon sequence
        /// </summary>
        public static double CalculateMass(Codon[] sequence)
        {
            string terminusFormula = DatabaseReader.Terminuses[0].Name + DatabaseReader.Terminuses[1].Name;
            double terminusMass = MassesOfElements.GetCompoundMass(terminusFormula);

            double singleConnection = MassesOfElements.GetCompoundMass("H2O");

            string formula = string.Empty;

            foreach (Codon codon in sequence.Where(x => x.DrawingData.HasValue))
                formula += codon.DrawingData.Value.Data.Formula + " ";

            double formulaMass = MassesOfElements.GetCompoundMass(formula);

            double mass = formulaMass + (terminusMass- MassesOfElements.GetCompoundMass("H2")) * sequence.Where(x=>x.CodonType != CodonType.End).Count();
            mass -= singleConnection * (sequence.Length - 1);

            return mass;
        }

        static bool GetChargeOfCodon(Codon codon)
        {
            return codon.DrawingData.Value.Points.Any(y => y.Charge < 0) ? false : true;
        }
    }
}
