using Microsoft.AspNetCore.Mvc;
using Reservations.Api.Dtos;
using Reservations.Api.Services;

namespace Reservations.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _service;
        public ReservationsController(IReservationService service) { _service = service; }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? roomId, [FromQuery] DateTime? date)
        {
            var list = await _service.ListAsync(roomId, date);
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var r = await _service.GetAsync(id);
            if (r == null) return NotFound();
            return Ok(r);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationDto dto)
        {
            var (ok, reservation, error) = await _service.CreateAsync(dto);
            if (!ok)
            {
                if (error == "Conflict") return Conflict(new { message = "Conflito de horário na mesma sala." });
                return BadRequest(new { message = error });
            }
            return CreatedAtAction(nameof(GetById), new { id = reservation!.Id }, reservation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateReservationDto dto)
        {
            var (ok, error) = await _service.UpdateAsync(id, dto);
            if (!ok)
            {
                if (error == "NotFound") return NotFound();
                if (error == "Conflict") return Conflict(new { message = "Conflito de horário na mesma sala." });
                return BadRequest(new { message = error });
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}