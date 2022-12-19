using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Models.Codons;

namespace Analyzer;
public static class CodonDatabase
{
    public static readonly Codon[] Codons;
    
    static CodonDatabase()
    {
        var CodonsJson = JsonConverter.Deserialize<CodonJson[]>(File.ReadAllText("./database.json"));
        Codons = new Codon[CodonsJson.Length];

        for(int i = 0; i < CodonsJson.Length;i++)
        {
            var currentJson = CodonsJson[i];
            Codons[i] = new Codon(currentJson.Letter, currentJson.Name, currentJson.IDs, currentJson.CodonType);
        }
    }
}
