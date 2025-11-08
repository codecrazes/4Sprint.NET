using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoHub.Data;
using MotoHub.Models;
using MotoHub.Models.Hateoas; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoHub.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class AluguelController : ControllerBase
    {
        private readonly MotoHubContext _context;

        public AluguelController(MotoHubContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetAllAlugueis")]
        public async Task<IActionResult> GetAlugueis()
        {
            var alugueis = await _context.Alugueis
                .Include(a => a.Moto)
                .Include(a => a.Cliente)
                .ToListAsync();

            var resources = alugueis.Select(a =>
            {
                var res = new Resource<Aluguel>(a);
                res.Links.Add(new Link("self", Url.Link("GetAluguelById", new { id = a.Id })!, "GET"));
                res.Links.Add(new Link("update_aluguel", Url.Link("UpdateAluguel", new { id = a.Id })!, "PUT"));
                res.Links.Add(new Link("delete_aluguel", Url.Link("DeleteAluguel", new { id = a.Id })!, "DELETE"));
                return res;
            }).ToList();

            var collectionResource = new Resource<object>(new { count = resources.Count });
            collectionResource.Links.Add(new Link("self", Url.Link("GetAllAlugueis", null)!, "GET"));
            collectionResource.Links.Add(new Link("create", Url.Link("CreateAluguel", null)!, "POST"));

            return Ok(new { collection = collectionResource, items = resources });
        }

        [HttpGet("{id}", Name = "GetAluguelById")]
        public async Task<IActionResult> GetAluguel(int id)
        {
            var aluguel = await _context.Alugueis
                .Include(a => a.Moto)
                .Include(a => a.Cliente)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aluguel == null) return NotFound();

            var resource = new Resource<Aluguel>(aluguel);
            resource.Links.Add(new Link("self", Url.Link("GetAluguelById", new { id = aluguel.Id })!, "GET"));
            resource.Links.Add(new Link("alugueis", Url.Link("GetAllAlugueis", null)!, "GET"));
            resource.Links.Add(new Link("update_aluguel", Url.Link("UpdateAluguel", new { id = aluguel.Id })!, "PUT"));
            resource.Links.Add(new Link("delete_aluguel", Url.Link("DeleteAluguel", new { id = aluguel.Id })!, "DELETE"));

            return Ok(resource);
        }

        [HttpPost(Name = "CreateAluguel")]
        public async Task<IActionResult> PostAluguel([FromBody] AluguelDto dto)
        {
            var moto = await _context.Motos.FindAsync(dto.MotoId);
            var cliente = await _context.Clientes.FindAsync(dto.ClienteId);

            if (moto == null || cliente == null)
                return BadRequest("Moto ou Cliente não encontrados.");

            var aluguel = new Aluguel
            {
                MotoId = dto.MotoId,
                ClienteId = dto.ClienteId,
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim
            };

            _context.Alugueis.Add(aluguel);
            await _context.SaveChangesAsync();

            var resource = new Resource<Aluguel>(aluguel);
            resource.Links.Add(new Link("self", Url.Link("GetAluguelById", new { id = aluguel.Id })!, "GET"));
            resource.Links.Add(new Link("alugueis", Url.Link("GetAllAlugueis", null)!, "GET"));

            return CreatedAtRoute("GetAluguelById", new { id = aluguel.Id }, resource);
        }

        [HttpPut("{id}", Name = "UpdateAluguel")]
        public async Task<IActionResult> PutAluguel(int id, [FromBody] Aluguel aluguel)
        {
            if (id != aluguel.Id) return BadRequest();

            _context.Entry(aluguel).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var resource = new Resource<Aluguel>(aluguel);
            resource.Links.Add(new Link("self", Url.Link("GetAluguelById", new { id = aluguel.Id })!, "GET"));
            resource.Links.Add(new Link("alugueis", Url.Link("GetAllAlugueis", null)!, "GET"));
            resource.Links.Add(new Link("delete_aluguel", Url.Link("DeleteAluguel", new { id = aluguel.Id })!, "DELETE"));

            return Ok(resource);
        }
        
        [HttpDelete("{id}", Name = "DeleteAluguel")]
        public async Task<IActionResult> DeleteAluguel(int id)
        {
            var aluguel = await _context.Alugueis.FindAsync(id);
            if (aluguel == null) return NotFound();

            _context.Alugueis.Remove(aluguel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
