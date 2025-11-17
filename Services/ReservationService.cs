using Microsoft.EntityFrameworkCore;
using Reservations.Api.Data;
using Reservations.Api.Dtos;
using Reservations.Api.Models;

namespace Reservations.Api.Services
{
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _db;
        public ReservationService(AppDbContext db) { _db = db; }

        public async Task<Reservation?> GetAsync(int id)
        {
            return await _db.Reservations.Include(r => r.CoffeeOption).Include(r => r.Room).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Reservation>> ListAsync(int? roomId = null, DateTime? date = null)
        {
            var q = _db.Reservations.Include(r => r.Room).Include(r => r.CoffeeOption).AsQueryable();
            if (roomId.HasValue) q = q.Where(r => r.RoomId == roomId.Value);
            if (date.HasValue)
            {
                var d = date.Value.Date;
                var next = d.AddDays(1);
                q = q.Where(r => r.StartAt >= d && r.StartAt < next);
            }
            return await q.OrderBy(r => r.StartAt).ToListAsync();
        }

        public async Task<(bool ok, Reservation? reservation, string? error)> CreateAsync(CreateReservationDto dto)
        {
            if (dto.StartAt >= dto.EndAt)
                return (false, null, "StartAt must be before EndAt");

            if (await HasConflictAsync(dto.RoomId, dto.StartAt, dto.EndAt))
                return (false, null, "Conflict");

            var r = new Reservation
            {
                RoomId = dto.RoomId,
                StartAt = dto.StartAt,
                EndAt = dto.EndAt,
                Responsible = dto.Responsible,
                CoffeeRequested = dto.CoffeeRequested
            };

            _db.Reservations.Add(r);
            await _db.SaveChangesAsync();

            if (dto.CoffeeOption != null && dto.CoffeeOption.Quantity > 0)
            {
                var c = new CoffeeOption { ReservationId = r.Id, Quantity = dto.CoffeeOption.Quantity, Description = dto.CoffeeOption.Description };
                _db.CoffeeOptions.Add(c);
                await _db.SaveChangesAsync();
                r.CoffeeOption = c;
            }

            return (true, r, null);
        }

        public async Task<(bool ok, string? error)> UpdateAsync(int id, UpdateReservationDto dto)
        {
            var existing = await _db.Reservations.Include(r => r.CoffeeOption).FirstOrDefaultAsync(r => r.Id == id);
            if (existing == null) return (false, "NotFound");

            if (dto.StartAt >= dto.EndAt)
                return (false, "StartAt must be before EndAt");

            if (await HasConflictAsync(dto.RoomId, dto.StartAt, dto.EndAt, id))
                return (false, "Conflict");

            existing.RoomId = dto.RoomId;
            existing.StartAt = dto.StartAt;
            existing.EndAt = dto.EndAt;
            existing.Responsible = dto.Responsible;
            existing.CoffeeRequested = dto.CoffeeRequested;
            existing.UpdatedAt = DateTime.UtcNow;

            if (dto.CoffeeOption != null && dto.CoffeeOption.Quantity > 0)
            {
                if (existing.CoffeeOption == null)
                {
                    existing.CoffeeOption = new CoffeeOption { ReservationId = existing.Id, Quantity = dto.CoffeeOption.Quantity, Description = dto.CoffeeOption.Description };
                    _db.CoffeeOptions.Add(existing.CoffeeOption);
                }
                else
                {
                    existing.CoffeeOption.Quantity = dto.CoffeeOption.Quantity;
                    existing.CoffeeOption.Description = dto.CoffeeOption.Description;
                }
            }
            else
            {
                if (existing.CoffeeOption != null)
                {
                    _db.CoffeeOptions.Remove(existing.CoffeeOption);
                }
            }

            await _db.SaveChangesAsync();
            return (true, null);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _db.Reservations.Include(r => r.CoffeeOption).FirstOrDefaultAsync(r => r.Id == id);
            if (existing == null) return false;
            if (existing.CoffeeOption != null) _db.CoffeeOptions.Remove(existing.CoffeeOption);
            _db.Reservations.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasConflictAsync(int roomId, DateTime start, DateTime end, int? excludingId = null)
        {
            var q = _db.Reservations.Where(r => r.RoomId == roomId && r.StartAt < end && r.EndAt > start);
            if (excludingId.HasValue) q = q.Where(r => r.Id != excludingId.Value);
            return await q.AnyAsync();
        }
    }
}