using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace MotoHub.Controllers.V1
{
    [ApiController]
    [Route("api/teste")]
    public class TesteController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TesteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("conexao")]
        public IActionResult TestarConexao()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using var connection = new OracleConnection(connectionString);
                connection.Open();

                return Ok("Conexão com o Oracle bem-sucedida!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao conectar: {ex.Message}");
            }
        }
    }
}
