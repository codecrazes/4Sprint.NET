using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoHub.Data;
using MotoHub.Models;

namespace MotoHub.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class MotoController : ControllerBase
    {
        private readonly MotoHubContext _context;

        public MotoController(MotoHubContext context)
        {
            _context = context;
        }

        private object ToHateoas(Moto moto)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}/api/Moto";

            return new
            {
                moto.Id,
                moto.Modelo,
                moto.Marca,
                moto.Ano,
                moto.Placa,
                moto.Preco,
                links = new[]
                {
                    new { rel = "self", href = $"{baseUrl}/{moto.Id}", method = "GET" },
                    new { rel = "update", href = $"{baseUrl}/{moto.Id}", method = "PUT" },
                    new { rel = "delete", href = $"{baseUrl}/{moto.Id}", method = "DELETE" },
                    new { rel = "all", href = baseUrl, method = "GET" }
                }
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetMotos()
        {
            var motos = await _context.Motos.ToListAsync();
            return motos.Select(m => ToHateoas(m)).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetMoto(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return NotFound();
            return ToHateoas(moto);
        }

        [HttpPost]
        public async Task<ActionResult<object>> PostMoto([FromBody] Moto moto)
        {
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();

            var response = ToHateoas(moto);
            return CreatedAtAction(nameof(GetMoto), new { id = moto.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMoto(int id, [FromBody] Moto moto)
        {
            if (id != moto.Id) return BadRequest();

            _context.Entry(moto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Motos.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoto(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return NotFound();

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
