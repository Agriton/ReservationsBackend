using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservations.Api.Data;
using Reservations.Api.Models;

namespace Reservations.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public RoomsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? placeId)
        {
            var query = _db.Rooms.AsQueryable();

            if (placeId.HasValue)
            {
                query = query.Where(r => r.PlaceId == placeId.Value);
            }
            var rooms = await query
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.Capacity,
                    r.PlaceId,
                    PlaceName = r.Place != null ? r.Place.Name : null
                })
                .ToListAsync();

            return Ok(rooms);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _db.Rooms
                .Where(r => r.Id == id)
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.Capacity,
                    r.PlaceId,
                    PlaceName = r.Place != null ? r.Place.Name : null
                })
                .FirstOrDefaultAsync();

            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Room room)
        {
            if (room == null) return BadRequest();

            var existingPlace = await _db.Place.FindAsync(room.PlaceId);
            if (existingPlace == null) return BadRequest($"Place {room.PlaceId} not found.");

            room.Place = null;

            _db.Rooms.Add(room);
            await _db.SaveChangesAsync();

            var result = new
            {
                room.Id,
                room.Name,
                room.Capacity,
                room.PlaceId,
                PlaceName = existingPlace.Name
            };

            return CreatedAtAction(nameof(GetById), new { id = room.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Room updated)
        {
            if (updated == null || id != updated.Id) return BadRequest();

            var room = await _db.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            if (updated.PlaceId != room.PlaceId)
            {
                var place = await _db.Place.FindAsync(updated.PlaceId);
                if (place == null) return BadRequest($"Place {updated.PlaceId} not found.");
            }

            room.Name = updated.Name;
            room.Capacity = updated.Capacity;
            room.PlaceId = updated.PlaceId;

            _db.Rooms.Update(room);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _db.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            _db.Rooms.Remove(room);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}