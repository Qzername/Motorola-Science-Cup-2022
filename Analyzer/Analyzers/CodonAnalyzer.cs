using Analyzer.Models;
using Analyzer.Models.Draw;
using Analyzer.Models.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Analyzers
{
    /// <summary>
    /// Analazyer that is specialized in codon analization.
    /// Use only if you changed library's database
    /// </summary>
    public static class CodonAnalyzer
    {
        public static Data CreateCodonData(DrawingData data)
        {
            Data final = new Data()
            {
                Mass = CalculateMass(data),
                Formula = CalculateFormula(data),
            };

            return final;
        }

        public static double CalculateMass(DrawingData data)
        {
            //mass calculation
            double mass = 0;

            foreach (var point in data.Points)
            {
                string Compound = string.Empty;

                if (string.IsNullOrEmpty(point.MolecularFormula))
                {
                    Compound = "C";

                    var valency = 4 - data.Lines.Where(x => x.IDChemPoint1 == point.ID || x.IDChemPoint2 == point.ID).Sum(x => x.NumberOfBind);

                    if (valency < 4)
                        Compound += "H" + valency;
                }
                else
                    Compound = point.MolecularFormula;

                if (point.Charge < 0)
                    mass += 1d;

                mass += MassesOfElements.GetCompoundMass(Compound);
            }

            return mass;
        }
    
        public static string CalculateFormula(DrawingData data)
        {
            string formula = string.Empty;

            foreach (var point in data.Points)
            {
                string Compound = string.Empty;

                if (string.IsNullOrEmpty(point.MolecularFormula))
                {
                    Compound = "C";

                    var valency = 4 - data.Lines.Where(x => x.IDChemPoint1 == point.ID || x.IDChemPoint2 == point.ID).Sum(x => x.NumberOfBind);

                    if (valency < 4)
                        Compound += "H" + valency;
                }
                else
                    Compound = point.MolecularFormula;

                formula += Compound;
            }

            return formula;
        }
    }
}
