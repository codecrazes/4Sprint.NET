using Microsoft.EntityFrameworkCore;
using MotoHub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using MotoHub.Swagger;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MotoHub.HealthChecks;
using MotoHub.ML.Training;
using MotoHub.ML.Services;

namespace MotoHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AvaliacaoMotoModelBuilder.TreinarModelo();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<AvaliacaoMotoMLService>();

            builder.Services.AddDbContext<MotoHubContext>(options =>
                options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();

            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureOptions<SwaggerConfig>();

            builder.Services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>("database")
                .AddCheck("api", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MotoHubContext>();
                db.Database.Migrate();
            }

            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                    c.SwaggerEndpoint(
                        $"/swagger/{desc.GroupName}/swagger.json",
                        $"MotoHub API {desc.GroupName.ToUpper()}"
                    );
            });

            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = HealthCheckResponseWriter.Write
            });

            app.UseMiddleware<MotoHub.Middleware.ApiKeyMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
