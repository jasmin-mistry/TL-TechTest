using Core.Interfaces;
using Core.Models;

namespace Core;

public class RoomAvailabilityService : IRoomAvailabilityService
{
    private readonly IExternalService _service;

    public RoomAvailabilityService(IExternalService service)
    {
        _service = service;
    }

    public async Task<RoomAvailabilityVm> GetAvailability(string roomName, DayOfWeek? dayOfTheWeek,
        TimeOnly startTime = default,
        int durationInMinutes = 24 * 60 - 1)
    {
        var roomsAvailable = await _service.GetAvailability();
        var result = new RoomAvailabilityVm
        {
            Room = roomName
        };

        if (dayOfTheWeek.HasValue)
        {
            var day = dayOfTheWeek.Value.ToString().ToLower();
            var availabilityString = roomsAvailable?.Availability[day];

            result.Schedule.Add(CreateDayAvailability(day, availabilityString, startTime, durationInMinutes));
        }
        else
        {
            foreach (var a in roomsAvailable?.Availability)
                result.Schedule.Add(CreateDayAvailability(a.Key, a.Value, startTime, durationInMinutes));
        }

        return result;
    }

    private static DayAvailability CreateDayAvailability(string day, string availabilityString, TimeOnly startTime,
        int durationInMinutes)
    {
        return new DayAvailability
        {
            Day = Enum.Parse<DayOfWeek>(day, true),
            Availability = AvailabilityParser.Parse(availabilityString, startTime, durationInMinutes)
        };
    }
}