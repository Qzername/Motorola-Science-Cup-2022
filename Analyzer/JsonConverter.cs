using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Analyzer;

/// <summary>
/// Tool class for json managment
/// </summary>
internal static class JsonConverter
{
    /// <summary>
    /// Serialize object to json
    /// </summary>
    public static string Serialize(object text) =>
        JsonSerializer.Serialize(text, text.GetType());

    /// <summary>
    /// Deserialize json to object
    /// </summary>
    public static T Deserialize<T>(string json) =>
        JsonSerializer.Deserialize<T>(json);
}
