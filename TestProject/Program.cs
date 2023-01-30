
using Analyzer.Models;
using Analyzer.Models.Codons;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using Analyzer;

namespace TestProject
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine(MassesOfElements.GetCompoundMass("C6H13NO2"));
        }
    }
}

//DatabaseCreator.CreateCodonDatabase();

/*List<Codon> codons = new List<Codon>();

foreach (var codon in CodonDatabase.Codons)
{
    List<ChemPoint> points = new List<ChemPoint>();

    if (codon.CodonType == CodonType.End)
        continue;

    foreach (var point in codon.DrawingData.Points)
        points.Add(new ChemPoint()
        {
            ID = point.ID,
            Charge = point.Charge,
            MolecularFormula = point.MolecularFormula,
            Position = new Position(point.PositionX, point.PositionY)
        });

    Codon newCodon = new Codon(codon.Letter, codon.Name, codon.IDs, codon.CodonType, new DrawingData(points.ToArray(), codon.DrawingData.Lines));
    codons.Add(newCodon);
}

string json = JsonConvert.SerializeObject(codons.ToArray());
File.WriteAllText("./CodonDatabase/database.json", json);
*/
/*
            int totalPoints = 0;
            int totalLines = 0;

            foreach(var codon in CodonDatabase.Codons)
            {
                totalPoints += codon.DrawingData.Points.Length;
                totalLines += codon.DrawingData.Lines.Length;
            }

            float avgPoints = Convert.ToSingle(totalPoints) / CodonDatabase.Codons.Length;
            float avgLines = Convert.ToSingle(totalLines) / CodonDatabase.Codons.Length;

            Console.WriteLine("total points: " + totalPoints);
            Console.WriteLine("total lines: " + totalLines);
            Console.WriteLine("avg points: " + avgPoints);
            Console.WriteLine("avg lines: " + avgLines);*/