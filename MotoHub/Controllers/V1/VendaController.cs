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
    public class VendaController : ControllerBase
    {
        private readonly MotoHubContext _context;

        public VendaController(MotoHubContext context)
        {
            _context = context;
        }

        private object ToHateoas(Venda venda)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}/api/Venda";

            return new
            {
                venda.Id,
                venda.MotoId,
                venda.ClienteId,
                venda.DataVenda,
                venda.Valor,
                Moto = venda.Moto == null ? null : new
                {
                    venda.Moto.Id,
                    venda.Moto.Modelo,
                    venda.Moto.Marca,
                    venda.Moto.Ano
                },
                Cliente = venda.Cliente == null ? null : new
                {
                    venda.Cliente.Id,
                    venda.Cliente.Nome,
                    venda.Cliente.CPF,
                    venda.Cliente.Telefone,
                    venda.Cliente.Email
                },
                links = new[]
                {
                    new { rel = "self", href = $"{baseUrl}/{venda.Id}", method = "GET" },
                    new { rel = "update", href = $"{baseUrl}/{venda.Id}", method = "PUT" },
                    new { rel = "delete", href = $"{baseUrl}/{venda.Id}", method = "DELETE" },
                    new { rel = "all", href = baseUrl, method = "GET" }
                }
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetVendas()
        {
            var vendas = await _context.Vendas
                                       .Include(v => v.Moto)
                                       .Include(v => v.Cliente)
                                       .ToListAsync();

            return vendas.Select(v => ToHateoas(v)).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetVenda(int id)
        {
            var venda = await _context.Vendas
                                      .Include(v => v.Moto)
                                      .Include(v => v.Cliente)
                                      .FirstOrDefaultAsync(v => v.Id == id);

            if (venda == null) return NotFound();
            return ToHateoas(venda);
        }

        [HttpPost]
        public async Task<ActionResult<object>> PostVenda([FromBody] VendaDto dto)
        {
            var venda = new Venda
            {
                MotoId = dto.MotoId,
                ClienteId = dto.ClienteId,
                DataVenda = dto.DataVenda,
                Valor = dto.Valor
            };

            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();

            var response = ToHateoas(venda);
            return CreatedAtAction(nameof(GetVenda), new { id = venda.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenda(int id, [FromBody] VendaDto dto)
        {
            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null) return NotFound();

            venda.MotoId = dto.MotoId;
            venda.ClienteId = dto.ClienteId;
            venda.DataVenda = dto.DataVenda;
            venda.Valor = dto.Valor;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenda(int id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null) return NotFound();

            _context.Vendas.Remove(venda);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
