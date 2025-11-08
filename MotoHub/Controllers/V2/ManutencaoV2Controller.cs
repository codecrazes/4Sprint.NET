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
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ManutencaoV2Controller : ControllerBase
    {
        private readonly MotoHubContext _context;

        public ManutencaoV2Controller(MotoHubContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var manutencoes = await _context.Manutencoes
                .Include(m => m.Moto)
                .ToListAsync();

            return Ok(manutencoes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var manutencao = await _context.Manutencoes
                .Include(m => m.Moto)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manutencao == null) return NotFound();
            return Ok(manutencao);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ManutencaoRequestDto dto)
        {
            var manutencao = new Manutencao
            {
                MotoId = dto.MotoId,
                Descricao = dto.Descricao,
                Data = dto.Data,
                Custo = dto.Custo
            };

            _context.Manutencoes.Add(manutencao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = manutencao.Id }, manutencao);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ManutencaoRequestDto dto)
        {
            var manutencao = await _context.Manutencoes.FindAsync(id);
            if (manutencao == null) return NotFound();

            manutencao.MotoId = dto.MotoId;
            manutencao.Descricao = dto.Descricao;
            manutencao.Data = dto.Data;
            manutencao.Custo = dto.Custo;

            await _context.SaveChangesAsync();
            return Ok(manutencao);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var manutencao = await _context.Manutencoes.FindAsync(id);
            if (manutencao == null) return NotFound();

            _context.Manutencoes.Remove(manutencao);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
