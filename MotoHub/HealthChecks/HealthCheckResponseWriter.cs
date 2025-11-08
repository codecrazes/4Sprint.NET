using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MotoHub.HealthChecks
{
    public static class HealthCheckResponseWriter
    {
        public static Task Write(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json";

            var result = new
            {
                status = report.Status.ToString(),
                duration = report.TotalDuration.ToString(),
                components = report.Entries.ToDictionary(
                    e => e.Key,
                    e => e.Value.Status.ToString())
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }
}
