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

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _db.Place.Include(l => l.Rooms).ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create(Place place)
        {
            _db.Place.Add(place);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = place.Id }, place);
        }
    }
}