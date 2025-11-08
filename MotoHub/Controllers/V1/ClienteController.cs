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
    public class ClienteController : ControllerBase
    {
        private readonly MotoHubContext _context;

        public ClienteController(MotoHubContext context)
        {
            _context = context;
        }

        private object ToHateoas(Cliente cliente)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host.Value}";

            return new
            {
                cliente.Id,
                cliente.Nome,
                cliente.CPF,
                cliente.Telefone,
                cliente.Email,
                links = new[]
                {
                    new { rel = "self", href = $"{baseUrl}/api/cliente/{cliente.Id}", method = "GET" },
                    new { rel = "update", href = $"{baseUrl}/api/cliente/{cliente.Id}", method = "PUT" },
                    new { rel = "delete", href = $"{baseUrl}/api/cliente/{cliente.Id}", method = "DELETE" },
                    new { rel = "all", href = $"{baseUrl}/api/cliente", method = "GET" }
                }
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetClientes()
        {
            var clientes = await _context.Clientes.ToListAsync();
            return clientes.Select(c => ToHateoas(c)).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return ToHateoas(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<object>> PostCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            var response = ToHateoas(cliente);
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.Id) return BadRequest();

            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
