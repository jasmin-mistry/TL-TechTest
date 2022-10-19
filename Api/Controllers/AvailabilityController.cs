using Core;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/Rooms/{room}/[controller]")]
public class AvailabilityController : ControllerBase
{
    private readonly ILogger<AvailabilityController> _logger;
    private readonly IRoomAvailabilityService _service;

    public AvailabilityController(ILogger<AvailabilityController> logger, IRoomAvailabilityService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] string room, [FromQuery] AvailabilityParams parameters)
    {
        var startTime = string.IsNullOrWhiteSpace(parameters.TimeOfTheDay)
            ? TimeOnly.MinValue
            : TimeOnly.Parse(parameters.TimeOfTheDay);

        int duration;
        if (parameters.DurationInMinutes is null)
        {
            if (parameters.TimeOfTheDay is null)
                duration = 24 * 60 - 1;
            else
                duration = (int)(TimeOnly.MaxValue - startTime).TotalMinutes;
        }
        else
        {
            if (startTime > startTime.AddMinutes(parameters.DurationInMinutes.Value))
                return BadRequest("Duration should be in the same day.");
            duration = parameters.DurationInMinutes.Value;
        }

        var roomName = string.IsNullOrWhiteSpace(room) ? "Dummy Room Name" : room;

        var result = await _service.GetAvailability(roomName, parameters.DayOfTheWeek, startTime, duration);

        return Ok(result);
    }
}