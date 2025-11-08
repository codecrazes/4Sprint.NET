using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MotoHub.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEY_HEADER = "X-API-KEY";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            var expectedApiKey = configuration["ApiKey"];

            Console.WriteLine("Esperado: " + expectedApiKey);

            if (!context.Request.Headers.TryGetValue(APIKEY_HEADER, out var providedKey))
            {
                Console.WriteLine("Header NÃO encontrado.");
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key não informada.");
                return;
            }

            Console.WriteLine("Recebido no header: " + providedKey);

            if (providedKey != expectedApiKey)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("API Key inválida.");
                return;
            }

            await _next(context);
        }

    }
}
