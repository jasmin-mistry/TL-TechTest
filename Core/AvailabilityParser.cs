namespace Core;

public static class AvailabilityParser
{
    public static Dictionary<string, bool> Parse(string availability, TimeOnly startTime, int durationInMinutes)
    {
        var intervalInMinutes = 60 / (availability.Length / 24);

        var roomAvailability = ParseAll(availability);
        var keys = GenerateKeys(startTime, durationInMinutes, intervalInMinutes);

        var result = roomAvailability
            .Where(x => keys.Contains(x.Key))
            .ToDictionary(x => x.Key, y => y.Value);

        return result;
    }

    private static Dictionary<string, bool> ParseAll(string availability)
    {
        var intervalInMinutes = 60 / (availability.Length / 24);
        var appointments = new Dictionary<string, bool>();

        var timeSlot = TimeOnly.MinValue;

        foreach (var isAvailable in availability.Select(availableSlot => availableSlot == '0'))
        {
            appointments.Add(timeSlot.ToShortTimeString(), isAvailable);
            timeSlot = timeSlot.AddMinutes(intervalInMinutes);
        }

        return appointments;
    }

    private static List<string> GenerateKeys(TimeOnly startTime, int durationInMinutes, int intervalInMinutes)
    {
        var result = new List<string>();
        var endTime = startTime.AddMinutes(durationInMinutes);
        var hoursLeft = endTime - startTime;

        for (var slot = startTime;
             result.Count < hoursLeft.TotalMinutes / intervalInMinutes;
             slot = slot.AddMinutes(intervalInMinutes))
            result.Add(slot.ToShortTimeString());

        return result;
    }
}