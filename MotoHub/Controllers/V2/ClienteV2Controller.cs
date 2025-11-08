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
    public class ClienteV2Controller : ControllerBase
    {
        private readonly MotoHubContext _context;

        public ClienteV2Controller(MotoHubContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Clientes.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClienteRequestDto dto)
        {
            var cliente = new Cliente
            {
                Nome = dto.Nome,
                CPF = dto.CPF,
                Telefone = dto.Telefone,
                Email = dto.Email
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ClienteRequestDto dto)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            cliente.Nome = dto.Nome;
            cliente.CPF = dto.CPF;
            cliente.Telefone = dto.Telefone;
            cliente.Email = dto.Email;

            await _context.SaveChangesAsync();
            return Ok(cliente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
