using Core.Models;

namespace Core.Interfaces;

public interface IRoomAvailabilityService
{
    Task<RoomAvailabilityVm> GetAvailability(string roomName, DayOfWeek? dayOfTheWeek, TimeOnly startTime,
        int durationInMinutes);
}