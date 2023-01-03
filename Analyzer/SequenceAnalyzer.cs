using Analyzer.Models;
using Analyzer.Models.Codons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    /// <summary>
    /// Codon/Sequence Analyzer
    /// </summary>
    public class SequenceAnalyzer
    {
        public Protein[] DetectProteins(Codon[] shift)
        {
            throw new Exception("musisz to jeszcze zrobić");

            return new Protein[0];
        }

        /// <summary>
        /// Creates Sequence from raw RNA/DNA code
        /// </summary>
        /// <param name="rawSequence">string that contains RNA/DNA code</param>
        public Sequence CreateSequence(string rawSequence)
        {
            //changing DNA to RNA if sequence is DNA sequence
            rawSequence = rawSequence.Replace('T', 'U');

            Codon[][] shifts = new Codon[3][];

            int codonNumber = rawSequence.Length / 3;

            for (int i = 0; i < rawSequence.Length % 3+1;i++)
                shifts[i] = ReadCodonsFromString(rawSequence.Substring(i, codonNumber*3));

            return new Sequence(rawSequence, shifts[0], shifts[1], shifts[2]);
        }

        //Reading one shift
        Codon[] ReadCodonsFromString(string series)
        {
            List<Codon> codonsID = new List<Codon>();

            for (int i = 0; i < series.Length/3; i++)
            {
                string ID = string.Empty;

                for (int j = 0; j < 3; j++)
                    ID += series[i * 3 + j];

                Codon codon = CodonDatabase.Codons.Single(x => x.IDs.Contains(ID));
                codonsID.Add(codon);
            }

            return codonsID.ToArray();
        }
    }
}
