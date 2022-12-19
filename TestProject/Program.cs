using Analyzer;
using Analyzer.Models;
using Analyzer.Models.Codons;
using System;

namespace TestProject
{
    public class Program
    {
        public static void Main()
        {
            //DatabaseCreator.CreateCodonDatabase();
            
            Analizer analizer = new Analizer();
            Sequence sequence = analizer.CreateSequence("AAAUGAACGAAAAUCUGUUCGCUUCAUUCAUUGCCCCCACAAUCCUAGGCCUACCC");

            string shift1 = CodonsToString(sequence.CodonsShift1);
            string correctShift1 = "K[stop]TKICSLHSLPPQS[stop]AY";
            Console.WriteLine($"{shift1}\n{correctShift1}\n{shift1 == correctShift1}\n------------------------");

            string shift2 = CodonsToString(sequence.CodonsShift2);
            Console.WriteLine($"{shift2}\n------------------------");

            string shift3 = CodonsToString(sequence.CodonsShift3);
            string correctShift3 = "M(start)NENLFASFIAPTILGLP";
            Console.WriteLine($"{shift3}\n{correctShift3}\n{shift3 == correctShift3}");
        }

        static string CodonsToString(Codon[] shift)
        {
            string final = string.Empty;

            foreach (var codon in shift)
            {
                if (codon.CodonType == CodonType.Start)
                    final += "M(start)";
                else if (codon.CodonType == CodonType.End)
                    final += "[stop]";
                else
                    final += codon.Letter;
            }

            return final;
        }
    }
}