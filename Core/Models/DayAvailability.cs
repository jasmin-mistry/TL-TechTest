using System.Text.Json.Serialization;

namespace Core.Models;

public class DayAvailability
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DayOfWeek Day { get; set; }

    public Dictionary<string, bool> Availability { get; set; } = new();
}