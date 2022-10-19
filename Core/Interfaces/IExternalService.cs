using Core.Models;

namespace Core.Interfaces;

public interface IExternalService
{
    Task<RoomsAvailable?> GetAvailability();
}