using Analyzer.Models;
using System.Security.Cryptography.X509Certificates;

namespace TestProject;

public static class DatabaseCreator
{
    public static void CreateCodonDatabase()
    {
        string[] files = Directory.GetFiles("./CodonDatabase/Raw/");

        foreach (string file in files) PrepareFileForDatabase(file);

        ConnectDatabases(files[0], files[1]);

        List<Codon> codons = new List<Codon>();

        string[,] codonsRaw = new string[64,3];

        string[] lines = File.ReadAllLines(files[0]);

        foreach (string line in File)
        {
            co
        }
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

    /// <summary>
    /// Łączenie dwóch plików zawierających database kodonów
    /// </summary>
    /// <param name="primaryFile">Plik do którego będzie wpisany treść drugiego pliku</param>
    /// <param name="secondaryFile">Plik z któego będzie braną dane do pierwszego pliku</param>
    static void ConnectDatabases(string primaryFile, string secondaryFile)
    {
        string[] linesOfSecondFile = File.ReadAllLines(secondaryFile);
        string[] linesOfFirstFile = File.ReadAllLines(primaryFile);

        for (int i = 0; i < linesOfSecondFile.Length; i++)
        {
            string data = linesOfSecondFile[i].Split('-')[1];
            linesOfFirstFile[i] += "-" + data;
        }

        File.WriteAllLines(primaryFile, linesOfFirstFile);
    }
}