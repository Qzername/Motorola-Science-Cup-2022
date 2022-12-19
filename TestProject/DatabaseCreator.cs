using Analyzer.Models.Codons;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;


namespace TestProject;

public static class DatabaseCreator
{
    public static void CreateCodonDatabase()
    {
        string[] files = Directory.GetFiles("./CodonDatabase/Raw/");

        foreach (string file in files) PrepareFileForDatabase(file);

        var shortcuts = ReverseDatabase(ReadDatabase(files[0]));
        var names = ReadDatabase(files[1]);

        List<Codon> codons = new List<Codon>();

        foreach(var key in shortcuts.Keys)
        {
            CodonType type = CodonType.Normal;

            if (key == "STOP")
                type = CodonType.End;
            else if (key == "M")
                type = CodonType.Start;

            codons.Add(new Codon(key, names[shortcuts[key][0]][0], shortcuts[key], type));
        }

        string json = JsonConvert.SerializeObject(codons.ToArray());

        File.WriteAllText("./CodonDatabase/database.json", json);
    }

    /// <summary>
    /// Przygotowanie i usuwanie zbędnych symboli z plików 
    /// </summary>
    static void PrepareFileForDatabase(string file)
    {
        string[] lines = File.ReadAllLines(file);

        var charToDelete = new string[] { '"'.ToString(), ",", "{", "}"};

        foreach (var c in charToDelete)
            for (int i = 0; i < lines.Length; i++)
                lines[i] = lines[i].Replace(c, string.Empty).Replace(": ", "-");
        
        File.WriteAllLines(file, lines.Where(x=> x != string.Empty));
    }

    static Dictionary<string, string[]> ReadDatabase(string file)
    {
        Dictionary<string, List<string>> Raw = new Dictionary<string, List<string>>();

        foreach (string line in File.ReadAllLines(file))
        {
            string[] data = line.Split('-');

            if (!Raw.ContainsKey(data[0]))
                Raw.Add(data[0], new List<string>());

            Raw[data[0]].Add(data[1]);
        }

        return ConvertListDictonaryToArray(Raw);
    }
    
    static Dictionary<string, string[]> ReverseDatabase(Dictionary<string, string[]> database)
    {
        Dictionary<string, List<string>> Raw = new Dictionary<string, List<string>>();

        foreach (var KeyValuePair in database.ToList())
        {
            if (!Raw.ContainsKey(KeyValuePair.Value[0]))
                Raw.Add(KeyValuePair.Value[0], new List<string>());

            Raw[KeyValuePair.Value[0]].Add(KeyValuePair.Key);
        }

        return ConvertListDictonaryToArray(Raw);
    }

    /// <summary>
    /// Łączenie dwóch plików zawierających database kodonów
    /// </summary>
    /// <param name="primaryFile">Plik do którego będzie wpisany treść drugiego pliku</param>
    /// <param name="secondaryFile">Plik z któego będzie braną dane do pierwszego pliku</param>
    static void ConnectDatabases(string primaryFile, string secondaryFile)
    { //chyba ostatecznie nie będzie ta metoda potrzebna
        string[] linesOfSecondFile = File.ReadAllLines(secondaryFile);
        string[] linesOfFirstFile = File.ReadAllLines(primaryFile);

        for (int i = 0; i < linesOfSecondFile.Length; i++)
        {
            string data = linesOfSecondFile[i].Split('-')[1];
            linesOfFirstFile[i] += "-" + data;
        }

        File.WriteAllLines(primaryFile, linesOfFirstFile);
    }

    static Dictionary<string, string[]> ConvertListDictonaryToArray(Dictionary<string, List<string>> database)
    {
        Dictionary<string, string[]> Final = new Dictionary<string, string[]>();

        foreach (var key in database.Keys)
            Final.Add(key, database[key].ToArray());

        return Final;
    }
}