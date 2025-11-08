using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace MotoHub.Controllers.V2.System
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/health")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var report = await _healthCheckService.CheckHealthAsync();

            var result = new
            {
                status = report.Status.ToString(),
                duration = report.TotalDuration.ToString(),
                components = report.Entries.ToDictionary(
                    e => e.Key,
                    e => e.Value.Status.ToString())
            };

            return Ok(result);
        }
    }
}
