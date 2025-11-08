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
    public class EstoqueV2Controller : ControllerBase
    {
        private readonly MotoHubContext _context;

        public EstoqueV2Controller(MotoHubContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var estoques = await _context.Estoques.Include(e => e.Moto).ToListAsync();
            return Ok(estoques);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var estoque = await _context.Estoques
                .Include(e => e.Moto)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (estoque == null) return NotFound();
            return Ok(estoque);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EstoqueRequestDto dto)
        {
            var estoque = new Estoque
            {
                MotoId = dto.MotoId,
                Quantidade = dto.Quantidade,
                Preco = dto.Preco
            };

            _context.Estoques.Add(estoque);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = estoque.Id }, estoque);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EstoqueRequestDto dto)
        {
            var estoque = await _context.Estoques.FindAsync(id);
            if (estoque == null) return NotFound();

            estoque.MotoId = dto.MotoId;
            estoque.Quantidade = dto.Quantidade;
            estoque.Preco = dto.Preco;

            await _context.SaveChangesAsync();
            return Ok(estoque);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var estoque = await _context.Estoques.FindAsync(id);
            if (estoque == null) return NotFound();

            _context.Estoques.Remove(estoque);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
