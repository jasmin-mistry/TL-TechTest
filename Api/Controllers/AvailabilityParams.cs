using System.ComponentModel.DataAnnotations;

namespace Api.Controllers;

public class AvailabilityParams
{
    [Range(1, 5)] public DayOfWeek? DayOfTheWeek { get; init; }

    public string? TimeOfTheDay { get; init; }

    [Range(1, 24 * 60 - 1)] public int? DurationInMinutes { get; init; }
}