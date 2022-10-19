namespace Core.Models;

public class RoomAvailabilityVm
{
    public string Room { get; set; }

    public List<DayAvailability> Schedule { get; set; } = new();
}