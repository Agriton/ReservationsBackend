using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservations.Api.Data;
using Reservations.Api.Models;

namespace Reservations.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaceController : ControllerBase
    {
        private readonly AppDbContext _db;
        public PlaceController(AppDbContext db) { _db = db; }

        // GET /api/Place
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var places = await _db.Place
                .Select(p => new
                {
                    p.Id,
                    p.Name
                })
                .ToListAsync();

            return Ok(places);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var place = await _db.Place
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    Rooms = p.Rooms.Select(r => new { r.Id, r.Name, r.Capacity, r.PlaceId })
                })
                .FirstOrDefaultAsync();

            if (place == null) return NotFound();
            return Ok(place);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Place payload)
        {
            if (payload == null) return BadRequest();
            var rooms = payload.Rooms ?? new List<Room>();

            foreach (var r in rooms)
            {
                r.Place = null;
                r.Reservations = null;
            }

            var place = new Place
            {
                Name = payload.Name,
                Rooms = rooms.Select(r => new Room
                {
                    Name = r.Name,
                    Capacity = r.Capacity
                }).ToList()
            };

            _db.Place.Add(place);
            await _db.SaveChangesAsync();

            var result = new
            {
                place.Id,
                place.Name,
                Rooms = place.Rooms.Select(r => new { r.Id, r.Name, r.Capacity, r.PlaceId })
            };

            return CreatedAtAction(nameof(GetById), new { id = place.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Place updated)
        {
            if (updated == null || id != updated.Id) return BadRequest();

            var place = await _db.Place.Include(p => p.Rooms).FirstOrDefaultAsync(p => p.Id == id);
            if (place == null) return NotFound();

            place.Name = updated.Name;

            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
