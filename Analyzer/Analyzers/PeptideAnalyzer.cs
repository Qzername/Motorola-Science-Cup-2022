using Analyzer.Models.Codons;
using System;
using System.Collections.Generic;
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
            double singleConnection = MassesOfElements.GetCompoundMass("H2O");

            sequence = sequence.Except(sequence.Where(x => !x.DrawingData.HasValue)).ToArray();

            double mass = sequence.Sum(x => x.DrawingData.Value.Data.Mass);
            mass -= singleConnection * (sequence.Length - 1);

            return mass;
        }

    }
}
