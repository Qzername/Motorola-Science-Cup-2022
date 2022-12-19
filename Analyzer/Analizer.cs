using Analyzer.Models;
using Analyzer.Models.Codons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public class Analizer
    {
        public Sequence CreateSequence(string rawSequence)
        {
            Codon[][] shifts = new Codon[3][];

            int codonNumber = rawSequence.Length / 3;

            for (int i = 0; i < rawSequence.Length % 3+1;i++)
                shifts[i] = ReadCodonsFromString(rawSequence.Substring(i, codonNumber*3));

            return new Sequence(rawSequence, shifts[0], shifts[1], shifts[2]);
        }

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
