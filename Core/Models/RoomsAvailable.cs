using System.Text.Json.Serialization;

namespace Core.Models;

public class RoomsAvailable
{
    [JsonPropertyName("availability")] public Dictionary<string, string> Availability { get; set; }
}