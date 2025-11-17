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
        public async Task<IActionResult> Get() => Ok(await _db.Rooms.Include(r => r.Place).ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create(Room room)
        {
            _db.Rooms.Add(room);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = room.Id }, room);
        }
    }
}