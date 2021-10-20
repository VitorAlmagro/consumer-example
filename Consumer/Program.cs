using System.IO;
using Consumer.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();

            var configuration = builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger.Information("Iniciando Consumer Example.");

            ConfigureHost(configuration).Run();
        }

        private static IHost ConfigureHost(IConfiguration configuration)
        {
            Log.Logger.Information("############ - Inicio método ConfigureHost - ############");

            WebHost.CreateDefaultBuilder()

                .ConfigureServices(services =>
                {
                    services.DependencyInjection();
                    services
                    .AddHealthChecks()
                    .AddCheck<WorkerHealthCheck>("HealthCheck", tags: new[] { "HealthCheck" });
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    //app.UseMetricServer();
                    //app.UseHttpMetrics();
                    app.UseEndpoints(endpoints =>
                    {
                        //endpoints.MapMetrics();
                        endpoints.MapHealthChecks("/healthcheck", new HealthCheckOptions()
                        {
                            Predicate = (check) => check.Tags.Contains("HealthCheck")
                        });
                    });
                })
                .Build()
                .StartAsync();

            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.DependencyInjection();
                    services.AddHttpClient();

                    services.AddHostedService<Worker>();
                })
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .WriteTo.Console()
                )
                .Build();
        }
    }
}
