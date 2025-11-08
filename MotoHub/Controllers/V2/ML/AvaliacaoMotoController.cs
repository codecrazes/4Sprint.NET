using Microsoft.AspNetCore.Mvc;
using MotoHub.Dto.V2.Requests;
using MotoHub.Dto.V2.Responses;
using MotoHub.ML.Services;

namespace MotoHub.Controllers.V2.ML
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/ml/avaliar-condicao")]
    public class AvaliacaoMotoController : ControllerBase
    {
        private readonly AvaliacaoMotoMLService _service;

        public AvaliacaoMotoController(AvaliacaoMotoMLService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AvaliacaoMotoResponseDto), 200)]
        public ActionResult<AvaliacaoMotoResponseDto> Avaliar([FromBody] AvaliacaoMotoRequestDto dto)
        {
            var resultado = _service.AvaliarMoto(dto);
            return Ok(resultado);
        }
    }
}
