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

    }
}
