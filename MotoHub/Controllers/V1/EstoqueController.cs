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
    public class EstoqueController : ControllerBase
    {
        private readonly MotoHubContext _context;

        public EstoqueController(MotoHubContext context)
        {
            _context = context;
        }

        private object ToHateoas(Estoque estoque)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}/api/Estoque";

            return new
            {
                estoque.Id,
                estoque.MotoId,
                estoque.Quantidade,
                estoque.Preco,
                Moto = estoque.Moto == null ? null : new
                {
                    estoque.Moto.Id,
                    estoque.Moto.Modelo,
                    estoque.Moto.Marca,
                    estoque.Moto.Ano
                },
                links = new[]
                {
                    new { rel = "self", href = $"{baseUrl}/{estoque.Id}", method = "GET" },
                    new { rel = "update", href = $"{baseUrl}/{estoque.Id}", method = "PUT" },
                    new { rel = "delete", href = $"{baseUrl}/{estoque.Id}", method = "DELETE" },
                    new { rel = "all", href = baseUrl, method = "GET" }
                }
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetEstoques()
        {
            var estoques = await _context.Estoques
                                         .Include(e => e.Moto)
                                         .ToListAsync();

            return estoques.Select(e => ToHateoas(e)).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetEstoque(int id)
        {
            var estoque = await _context.Estoques
                                        .Include(e => e.Moto)
                                        .FirstOrDefaultAsync(e => e.Id == id);

            if (estoque == null)
                return NotFound();

            return ToHateoas(estoque);
        }

        [HttpPost]
        public async Task<ActionResult<object>> PostEstoque([FromBody] Estoque estoque)
        {
            estoque.Moto = await _context.Motos.FindAsync(estoque.MotoId);
            if (estoque.Moto == null)
                return BadRequest($"Moto com ID {estoque.MotoId} não encontrada.");

            _context.Estoques.Add(estoque);
            await _context.SaveChangesAsync();

            var response = ToHateoas(estoque);
            return CreatedAtAction(nameof(GetEstoque), new { id = estoque.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstoque(int id, [FromBody] Estoque estoque)
        {
            if (id != estoque.Id)
                return BadRequest();

            estoque.Moto = await _context.Motos.FindAsync(estoque.MotoId);
            if (estoque.Moto == null)
                return BadRequest($"Moto com ID {estoque.MotoId} não encontrada.");

            _context.Entry(estoque).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Estoques.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstoque(int id)
        {
            var estoque = await _context.Estoques.FindAsync(id);
            if (estoque == null)
                return NotFound();

            _context.Estoques.Remove(estoque);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
