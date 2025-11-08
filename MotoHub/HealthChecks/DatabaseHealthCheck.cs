using Microsoft.Extensions.Diagnostics.HealthChecks;
using Oracle.ManagedDataAccess.Client;

namespace MotoHub.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        public DatabaseHealthCheck(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = new OracleConnection(_connectionString);
                await connection.OpenAsync(cancellationToken);
                return HealthCheckResult.Healthy("Conexão com Oracle OK");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Falha ao conectar no Oracle", ex);
            }
        }
    }
}
