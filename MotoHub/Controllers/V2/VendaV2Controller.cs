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
	public class VendaV2Controller : ControllerBase
	{
		private readonly MotoHubContext _context;

		public VendaV2Controller(MotoHubContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var vendas = await _context.Vendas
				.Include(v => v.Moto)
				.Include(v => v.Cliente)
				.ToListAsync();

			return Ok(vendas);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var venda = await _context.Vendas
				.Include(v => v.Moto)
				.Include(v => v.Cliente)
				.FirstOrDefaultAsync(v => v.Id == id);

			if (venda == null) return NotFound();
			return Ok(venda);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] VendaRequestDto dto)
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

			return CreatedAtAction(nameof(GetById), new { id = venda.Id }, venda);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] VendaRequestDto dto)
		{
			var venda = await _context.Vendas.FindAsync(id);
			if (venda == null) return NotFound();

			venda.MotoId = dto.MotoId;
			venda.ClienteId = dto.ClienteId;
			venda.DataVenda = dto.DataVenda;
			venda.Valor = dto.Valor;

			await _context.SaveChangesAsync();
			return Ok(venda);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var venda = await _context.Vendas.FindAsync(id);
			if (venda == null) return NotFound();

			_context.Vendas.Remove(venda);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
