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
            
            SequenceAnalyzer analizer = new SequenceAnalyzer();
            Sequence sequence = analizer.CreateSequence("AAAUGAACGAAAAUCUGUUCGCUUCAUUCAUUGCCCCCACAAUCCUAGGCCUACCC");

            string shift1 = Sequence.CodonsToString(sequence.CodonsShift1);
            string correctShift1 = "K[stop]TKICSLHSLPPQS[stop]AY";
            Console.WriteLine($"{shift1}\n{correctShift1}\n{shift1 == correctShift1}\n------------------------");

            string shift2 = Sequence.CodonsToString(sequence.CodonsShift2);
            Console.WriteLine($"{shift2}\n------------------------");

            string shift3 = Sequence.CodonsToString(sequence.CodonsShift3);
            string correctShift3 = "M(start)NENLFASFIAPTILGLP";
            Console.WriteLine($"{shift3}\n{correctShift3}\n{shift3 == correctShift3}");
        }
    }
}