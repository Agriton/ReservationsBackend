using Reservations.Api.Dtos;
using Reservations.Api.Models;

namespace Reservations.Api.Services
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> ListAsync(int? roomId = null, DateTime? date = null);
        Task<Reservation?> GetAsync(int id);
        Task<(bool ok, Reservation? reservation, string? error)> CreateAsync(CreateReservationDto dto);
        Task<(bool ok, string? error)> UpdateAsync(int id, UpdateReservationDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> HasConflictAsync(int roomId, DateTime start, DateTime end, int? excludingId = null);
    }
}