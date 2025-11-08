using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoHub.Data;
using MotoHub.Models;
using Microsoft.AspNetCore.Authorization;
using MotoHub.Dto.V2.Requests;
using MotoHub.Dto.V2.Responses;

namespace MotoHub.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/aluguel")]
    public class AluguelV2Controller : ControllerBase
    {
        private readonly MotoHubContext _context;

        public AluguelV2Controller(MotoHubContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var alugueis = await _context.Alugueis
                .Include(a => a.Moto)
                .Include(a => a.Cliente)
                .ToListAsync();

            return Ok(alugueis);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var aluguel = await _context.Alugueis
                .Include(a => a.Moto)
                .Include(a => a.Cliente)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aluguel == null) return NotFound();
            return Ok(aluguel);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AluguelRequestDto dto)
        {
            var aluguel = new Aluguel
            {
                MotoId = dto.MotoId,
                ClienteId = dto.ClienteId,
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim
            };

            _context.Alugueis.Add(aluguel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = aluguel.Id }, aluguel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AluguelRequestDto dto)
        {
            var aluguel = await _context.Alugueis.FindAsync(id);
            if (aluguel == null) return NotFound();

            aluguel.MotoId = dto.MotoId;
            aluguel.ClienteId = dto.ClienteId;
            aluguel.DataInicio = dto.DataInicio;
            aluguel.DataFim = dto.DataFim;

            await _context.SaveChangesAsync();
            return Ok(aluguel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var aluguel = await _context.Alugueis.FindAsync(id);
            if (aluguel == null) return NotFound();

            _context.Alugueis.Remove(aluguel);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
