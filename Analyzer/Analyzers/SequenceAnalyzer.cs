using Analyzer.Models;
using Analyzer.Models.Codons;
using Analyzer.Models.Draw;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Analyzers
{
    /// <summary>
    /// Codon/Sequence Analyzer
    /// </summary>
    public static class SequenceAnalyzer
    {
        /// <summary>
        /// Detect proteins in codon sequence
        /// </summary>
        public static Protein[] DetectProteins(Codon[] shift)
        {
            List<Protein> proteins = new List<Protein>();
            List<Codon> currentProtein = new List<Codon>();

            for (int i = 0; i < shift.Length; i++)
            {
                var current = shift[i];

                if (currentProtein.Count == 0 && current.CodonType != CodonType.Start)
                    continue;

                currentProtein.Add(current);

                if (current.CodonType == CodonType.End)
                {
                    proteins.Add(new Protein(currentProtein.ToArray()));
                    currentProtein.Clear();
                }
            }

            return proteins.ToArray();
        }
        /// <summary>
        /// Creates Sequence from raw RNA/DNA code
        /// </summary>
        /// <param name="rawSequence">string that contains RNA/DNA code</param>
        public static Sequence CreateSequence(string rawSequence)
        {
            //changing DNA to RNA if sequence is DNA sequence
            rawSequence = rawSequence.Replace('T', 'U');

            Codon[][] shifts = new Codon[3][];

            int codonNumber = rawSequence.Length / 3;

            for (int i = 0; i < rawSequence.Length % 3 + 1; i++)
                shifts[i] = ReadCodonsFromString(rawSequence.Substring(i, codonNumber * 3));

            return new Sequence(rawSequence, shifts[0], shifts[1], shifts[2]);
        }

        /// <summary>
        /// Creates rna code from codon Sequence
        /// </summary>
        /// <param name="codons"></param>
        /// <returns></returns>
        public static string CreateCode(Codon[] codons)
        {
            string sequence = string.Empty;

            foreach (var codon in codons)
                sequence += codon.IDs[0];

            return sequence;
        }

        //Reading one shift
        static Codon[] ReadCodonsFromString(string series)
        {
            List<Codon> codonsID = new List<Codon>();

            for (int i = 0; i < series.Length / 3; i++)
            {
                string ID = string.Empty;

                for (int j = 0; j < 3; j++)
                    ID += series[i * 3 + j];

                Codon codon = DatabaseReader.Codons.Single(x => x.IDs.Contains(ID));
                codonsID.Add(codon);
            }

            return codonsID.ToArray();
        }
    }
}
