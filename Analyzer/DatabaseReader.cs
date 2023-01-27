using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Models.Codons;
using Analyzer.Models.Terminuses;

namespace Analyzer;
public static class DatabaseReader
{
    public static readonly Codon[] Codons;
    public static readonly Terminus[] Terminuses;
    
    static DatabaseReader()
    {
        Codons = JsonConverter.Deserialize<Codon[]>(File.ReadAllText("./AnalyzerData/peptideDatabase.json"));
        Terminuses = JsonConverter.Deserialize<Terminus[]>(File.ReadAllText("./AnalyzerData/terminusDatabase.json"));
    }
}
