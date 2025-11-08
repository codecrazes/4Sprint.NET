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
	public class MotoV2Controller : ControllerBase
	{
		private readonly MotoHubContext _context;

		public MotoV2Controller(MotoHubContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var motos = await _context.Motos.ToListAsync();
			return Ok(motos);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var moto = await _context.Motos.FindAsync(id);
			if (moto == null) return NotFound();
			return Ok(moto);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] MotoRequestDto dto)
		{
			var moto = new Moto
			{
				Modelo = dto.Modelo,
				Marca = dto.Marca,
				Ano = dto.Ano,
				Placa = dto.Placa,
				Preco = dto.Preco
			};

			_context.Motos.Add(moto);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetById), new { id = moto.Id }, moto);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] MotoRequestDto dto)
		{
			var moto = await _context.Motos.FindAsync(id);
			if (moto == null) return NotFound();

			moto.Modelo = dto.Modelo;
			moto.Marca = dto.Marca;
			moto.Ano = dto.Ano;
			moto.Placa = dto.Placa;
			moto.Preco = dto.Preco;

			await _context.SaveChangesAsync();
			return Ok(moto);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var moto = await _context.Motos.FindAsync(id);
			if (moto == null) return NotFound();

			_context.Motos.Remove(moto);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
