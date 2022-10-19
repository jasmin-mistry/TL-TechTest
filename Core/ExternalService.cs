using System.Net.Http.Json;
using System.Text.Json;
using Core.Interfaces;
using Core.Models;

namespace Core;

public class ExternalService : IExternalService
{
    private readonly HttpClient _client;

    public ExternalService(HttpClient client)
    {
        _client = client;
    }

    public async Task<RoomsAvailable?> GetAvailability()
    {
        var roomsAvailable = await _client.GetFromJsonAsync<RoomsAvailable>(
            "/trainlinerecruitment/room-availability/main/availability.json", new JsonSerializerOptions
            {
                AllowTrailingCommas = true
            });

        return roomsAvailable;
    }
}