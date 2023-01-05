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
        Codons = JsonConverter.Deserialize<Codon[]>(File.ReadAllText("./database.json"));
    }
}
