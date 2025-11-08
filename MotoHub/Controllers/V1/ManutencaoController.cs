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
    public class ManutencaoController : ControllerBase
    {
        private readonly MotoHubContext _context;

        public ManutencaoController(MotoHubContext context)
        {
            _context = context;
        }

        private object ToHateoas(Manutencao manutencao)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}/api/Manutencao";

            return new
            {
                manutencao.Id,
                manutencao.MotoId,
                manutencao.Descricao,
                manutencao.Data,
                manutencao.Custo,
                Moto = manutencao.Moto == null ? null : new
                {
                    manutencao.Moto.Id,
                    manutencao.Moto.Modelo,
                    manutencao.Moto.Marca,
                    manutencao.Moto.Ano
                },
                links = new[]
                {
                    new { rel = "self", href = $"{baseUrl}/{manutencao.Id}", method = "GET" },
                    new { rel = "update", href = $"{baseUrl}/{manutencao.Id}", method = "PUT" },
                    new { rel = "delete", href = $"{baseUrl}/{manutencao.Id}", method = "DELETE" },
                    new { rel = "all", href = baseUrl, method = "GET" }
                }
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetManutencoes()
        {
            var manutencoes = await _context.Manutencoes
                                            .Include(m => m.Moto)
                                            .ToListAsync();

            return manutencoes.Select(m => ToHateoas(m)).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetManutencao(int id)
        {
            var manutencao = await _context.Manutencoes
                                           .Include(m => m.Moto)
                                           .FirstOrDefaultAsync(m => m.Id == id);

            if (manutencao == null) return NotFound();
            return ToHateoas(manutencao);
        }

        [HttpPost]
        public async Task<ActionResult<object>> PostManutencao([FromBody] ManutencaoDto dto)
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

            var response = ToHateoas(manutencao);
            return CreatedAtAction(nameof(GetManutencao), new { id = manutencao.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutManutencao(int id, [FromBody] ManutencaoDto dto)
        {
            var manutencao = await _context.Manutencoes.FindAsync(id);
            if (manutencao == null) return NotFound();

            manutencao.MotoId = dto.MotoId;
            manutencao.Descricao = dto.Descricao;
            manutencao.Data = dto.Data;
            manutencao.Custo = dto.Custo;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManutencao(int id)
        {
            var manutencao = await _context.Manutencoes.FindAsync(id);
            if (manutencao == null) return NotFound();

            _context.Manutencoes.Remove(manutencao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
